using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float m_AimCameraMaxRotation;

        private EventRouter m_EventRouter;
		
        private bool m_PlayerActive;
	
		void Start()
		{
            ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );
            if( lLocator != null )
            {
                m_EventRouter = ObjectLocator.FindObjectOfType<EventRouter>();
                Debug.Assert( m_EventRouter != null );

                m_EventRouter.RegisterListener( "game_start", OnGameStarted );
                m_EventRouter.RegisterListener( "enemy_attack_complete", OnEnemyAttackFinished );
            }

			//m_InitialDirection = transform.rotation;
            m_PlayerActive = true;
		}

        private void OnGameStarted()
        {
            m_PlayerActive = true;
        }

        void OnEnemyAttackFinished( )
        {
            if( m_PlayerActive )
            {
                m_PlayerActive = false;
                m_EventRouter.TriggerEvent( "player_death" );
            }
        }
	}
}