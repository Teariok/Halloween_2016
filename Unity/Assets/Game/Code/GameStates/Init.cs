using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class Init : BaseState
    {
        private ObjectLocator m_ObjectLocator = null;
        private WeaponController m_WeaponController = null;
        private Rect m_UIElementPos = new Rect( 0,0,0,0 );

        void Start()
        {
            m_ObjectLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( m_ObjectLocator != null );

            if( m_ObjectLocator != null )
            {
                m_WeaponController = m_ObjectLocator.FetchObject<WeaponController>();
                Debug.Assert( m_WeaponController != null );
            }
        }

        void Update()
        {
            if( m_WeaponController != null )
            {
                if( m_WeaponController.GetRegistrationState() == WeaponController.eWeaponState.REGISTERED )
                {
                    StateManager lStates = m_ObjectLocator.FetchObject<StateManager>();
    
                    Debug.Assert( lStates != null );
                    if( lStates != null )
                    {
                        lStates.PushState( typeof(StartWait) );
                    }
                }
            }
        }

        void OnGUI()
        {
            if( m_WeaponController != null )
            {
                WeaponController.eWeaponState lState = m_WeaponController.GetRegistrationState();

                if( lState == WeaponController.eWeaponState.UNREGISTERED )
                {
                    UpdateElementPos( 400, 50 );

                    if( GUI.Button( m_UIElementPos, "Register Controller" ) )
                    {
                        m_WeaponController.RegisterControlpad();
                    }
                }
                else if( lState == WeaponController.eWeaponState.SEARCHING )
                {
                    UpdateElementPos( 140, 20 );

                    GUI.Label( m_UIElementPos, "Searching for Controller" );
                }
            }
            else
            {
                UpdateElementPos( 400, 50 );

                GUI.Label( m_UIElementPos, "Catastrophic Initialisation Failure - No Weapon Controller" );
            }
        }

        private void UpdateElementPos( int width, int height )
        {
            m_UIElementPos.x = (Screen.width/2) - (width/2);
            m_UIElementPos.y = (Screen.height/2) - (height/2);
            m_UIElementPos.width = width;
            m_UIElementPos.height = height;
        }
    }
}