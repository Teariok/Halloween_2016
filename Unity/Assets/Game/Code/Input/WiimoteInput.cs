using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WiimoteApi;

namespace Teario.Util
{
    public class WiimoteInput : BaseInput
    {
        private Dictionary<eInputButton,eButtonState> m_InputStates = new Dictionary<eInputButton, eButtonState>(){
            { eInputButton.BUTTON_MAIN_ACTION, eButtonState.STATE_NONE }
        };

        private Wiimote m_Wiimote;
        private Vector2 m_CursorPosition;

        void Start()
        {
            m_Wiimote = null;
            m_CursorPosition = Vector2.zero;
        }

        void OnDestroy()
        {
            Disable();
        }

        public override bool IsSelectable()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return !m_ForceDisabled;
#else
            return false;
#endif
        }

        public override void Disable()
        {
            if( m_Wiimote != null )
            {
                WiimoteManager.Cleanup( m_Wiimote );
                m_Wiimote = null;
            }

            base.Disable();
        }

        void Update()
        {
            if( m_Wiimote != null )
            {
                UpdateButton( eInputButton.BUTTON_MAIN_ACTION, m_Wiimote.Button.b );
            }
        }

        public override void Select( System.Action<bool> lCallback )
        {
            StartCoroutine( Initialise(lCallback) );
        }

        public override Vector2 GetCursorPosition()
        {
            if( m_Wiimote != null )
            {
                float[] lRawPosition = m_Wiimote.Ir.GetPointingPosition();

                Debug.Assert( lRawPosition.Length >= 2 );

                m_CursorPosition.x = lRawPosition[0] * Screen.width;
                m_CursorPosition.y = lRawPosition[1] * Screen.height;
            }

//            return Input.mousePosition;
            return m_CursorPosition;
        }

        public override eButtonState GetButtonState( eInputButton lButton )
        {
            if( m_InputStates.ContainsKey( lButton ) )
            {
                return m_InputStates[ lButton ];
            }

            return eButtonState.STATE_NONE;
        }

        private IEnumerator Initialise( System.Action<bool> lCallback )
        {
            Debug.Assert( lCallback != null );

            if( lCallback != null )
            {
                WiimoteManager.FindWiimotes();

                // Don't know how long FindWiimotes is going to take so
                // ensure we yeild for at least a frame after it finishes
                yield return null;

                if( !WiimoteManager.HasWiimote() )
                {
                    lCallback( false );
                }
                else
                {
                    m_Wiimote = WiimoteManager.Wiimotes[0];

                    //m_Wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_IR12);
                    //m_Wiimote.SetupIRCamera( IRDataType.EXTENDED );

                    //m_Wiimote.SendDataReportMode(InputDataType.REPORT_INTERLEAVED);
                    //m_Wiimote.SetupIRCamera( IRDataType.FULL );

                    // Can't proceed if there's no IR camera available
                    if( !m_Wiimote.SetupIRCamera( IRDataType.BASIC ) )
                    {
                        lCallback( false );
                    }
                    else
                    {
                        yield return null;
        
                        // Default the first controller LED on
                        m_Wiimote.SendPlayerLED( true, false, false, false );
            
                        lCallback( true );
        
                        // The Wiimote will send reports back in a queue, so we
                        // can use this coroutine to ensure we always keep the
                        // input data up to date with the latest report
                        while( m_Wiimote != null )
                        {
                            if( m_Wiimote.ReadWiimoteData() == 0 )
                            {
                                yield return null;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateButton( eInputButton lButton, bool lIsDown )
        {
            if( m_InputStates.ContainsKey( lButton ) )
            {
                eButtonState lCurrentState = m_InputStates[lButton];
                if( lIsDown )
                {
                    m_InputStates[lButton] = lCurrentState == eButtonState.STATE_HELD || lCurrentState == eButtonState.STATE_PUSHED ? eButtonState.STATE_HELD : eButtonState.STATE_PUSHED;
                }
                else
                {
                    m_InputStates[lButton] = lCurrentState == eButtonState.STATE_NONE || lCurrentState == eButtonState.STATE_RELEASED ? eButtonState.STATE_NONE : eButtonState.STATE_RELEASED;
                }
            }
            else
            {
                m_InputStates[lButton] = lIsDown? eButtonState.STATE_PUSHED : eButtonState.STATE_RELEASED;
            }
        }
    }
}