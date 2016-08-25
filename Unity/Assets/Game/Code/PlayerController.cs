using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private Camera m_ViewCamera;
		[SerializeField]
		private float m_MaxFireDistance;
		[SerializeField]
		private float m_AimCameraMaxRotation;

        private EventRouter m_EventRouter;
		private RaycastHit m_RaycastInfo;
		private Ray m_FireRay;
		//private Quaternion m_InitialDirection;
        private bool m_PlayerActive;
	
		private const int MOUSE_ACTION_BUTTON = 0;
	
		void Start()
		{
            ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );
            if( lLocator != null )
            {
                m_EventRouter = ObjectLocator.FindObjectOfType<EventRouter>();
                Debug.Assert( m_EventRouter != null );

                m_EventRouter.RegisterListener( "game_start", OnGameStarted );
            }

			//m_InitialDirection = transform.rotation;
            m_PlayerActive = true;
		}
	
		void Update()
		{
			if( m_PlayerActive && Input.GetMouseButtonDown( MOUSE_ACTION_BUTTON ) )
			{
				m_FireRay = m_ViewCamera.ScreenPointToRay( Input.mousePosition );
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

        private void OnGameStarted()
        {
            m_PlayerActive = true;
        }

        void OnCollisionEnter( Collision lCollision )
        {
            if( m_PlayerActive )
            {
                m_PlayerActive = false;
                m_EventRouter.TriggerEvent( "player_death" );
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