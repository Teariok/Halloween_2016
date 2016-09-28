using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teario.Util
{
    public class MouseInput : BaseInput
    {
        private Dictionary<eInputButton,int> BUTTON_MAP = new Dictionary<eInputButton, int>(){
            { eInputButton.BUTTON_MAIN_ACTION, 0 }
        };

        void Start()
        {
        }

        public override bool IsSelectable()
        {
#if UNITY_EDITOR
            return Input.mousePresent && !m_ForceDisabled;
#else
            return false;
#endif
        }

        public override void Select( System.Action<bool> lCallback )
        {
            Debug.Assert( lCallback != null );

            if( lCallback != null )
            {
                lCallback( Input.mousePresent );
            }
        }

        public override Vector2 GetCursorPosition()
        {
            return Input.mousePosition;
        }

        public override eButtonState GetButtonState( eInputButton lButton )
        {
            int lButtonIndex = -1;

            if( BUTTON_MAP.ContainsKey( lButton ) )
            {
                lButtonIndex = BUTTON_MAP[lButton];
            }

            Debug.Assert( lButtonIndex != -1 );

            if( Input.GetMouseButtonDown(lButtonIndex) )
            {
                return eButtonState.STATE_PUSHED;
            }
            else if( Input.GetMouseButtonUp(lButtonIndex) )
            {
                return eButtonState.STATE_RELEASED;
            }
            if( Input.GetMouseButton(lButtonIndex) )
            {
                return eButtonState.STATE_HELD;
            }

            return eButtonState.STATE_NONE;
        }
    }
}