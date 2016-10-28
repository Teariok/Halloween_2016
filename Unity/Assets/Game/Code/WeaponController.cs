using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class WeaponController : MonoBehaviour
    {
        public enum eWeaponState
        {
            UNREGISTERED,
            SEARCHING,
            REGISTERED,
            FAILED
        };

        [SerializeField]
        private Camera m_ViewCamera;
        [SerializeField]
        private float m_MaxFireDistance;
        [SerializeField]
        private AudioClip m_FireSound;
    
        private bool m_CanFire;
        private eWeaponState m_WeaponState = eWeaponState.UNREGISTERED;
        private BaseInput[] m_AvailableInputs;
        private int m_ActiveInput;
        private Ray m_FireRay;
        private RaycastHit m_RaycastInfo;
        private AudioSource m_AudioSource;

        void Start()
        {
            m_CanFire = false;
            m_AvailableInputs = null;
            m_ActiveInput = -1;
            m_AudioSource = GetComponent<AudioSource>();
            Debug.Assert( m_AudioSource != null );
        }

        public void SetFiringEnabled( bool lEnabled )
        {
            m_CanFire = lEnabled;
        }
    
        public eWeaponState GetRegistrationState()
        {
            return m_WeaponState;
        }
    
        public void RegisterControlpad()
        {
            if( m_WeaponState != eWeaponState.SEARCHING )
            {
                m_WeaponState = eWeaponState.SEARCHING;
            }
    
            m_AvailableInputs = (BaseInput[])GetComponentsInChildren<BaseInput>();
            Debug.Assert( m_AvailableInputs.Length > 0 );

            SetInputIndex( 0 );
        }

        private void SetInputIndex( int lIndex )
        {
            if( m_AvailableInputs != null && lIndex < m_AvailableInputs.Length )
            {
                m_AvailableInputs[lIndex].gameObject.SetActive( true );
                m_AvailableInputs[lIndex].Select( (bool lSuccess) => {
                    if( lSuccess )
                    {
                        m_ActiveInput = lIndex;
                        m_WeaponState = eWeaponState.REGISTERED;

                        ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
                        Debug.Assert( lLocator != null );
                        PointerFollower lFollower = lLocator.FetchObject<PointerFollower>();
                        Debug.Assert( lFollower != null );
                        lFollower.SetController( m_AvailableInputs[m_ActiveInput] );
                    }
                    else
                    {
                        m_AvailableInputs[lIndex].gameObject.SetActive( false );
                        SetInputIndex( lIndex + 1 );
                    }
                });
            }
            else
            {
                m_WeaponState = eWeaponState.FAILED;
            }
        }

        void Update()
        {
            if( m_CanFire )
            {
                BaseInput lActiveInput = null;
                if( m_ActiveInput != -1 )
                {
                    lActiveInput = m_AvailableInputs[m_ActiveInput];
                }

                if( lActiveInput != null && lActiveInput.GetButtonState( BaseInput.eInputButton.BUTTON_MAIN_ACTION ) == BaseInput.eButtonState.STATE_PUSHED )
                {
                    m_AudioSource.PlayOneShot( m_FireSound );
                    m_FireRay = m_ViewCamera.ScreenPointToRay( lActiveInput.GetCursorPosition() );
                    if( Physics.Raycast( m_FireRay, out m_RaycastInfo, m_MaxFireDistance ) )
                    {
                        Enemy lEnemy = m_RaycastInfo.collider.GetComponent<Enemy>();
                        if( lEnemy )
                        {
                            lEnemy.TakeHit();
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if( Application.isPlaying )
            {
                Gizmos.DrawLine( m_FireRay.origin, m_FireRay.origin + (m_MaxFireDistance * m_FireRay.direction) );
            }
        }
#endif
    }
}