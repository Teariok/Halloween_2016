using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class StartWait : BaseState
	{
        private const float FIRING_ENABLE_DELAY = 2f;

		[SerializeField]
		private Vector3 m_TutorialEnemyPosition;
		[SerializeField]
		private ObjectPool m_EnemyPool;
		[SerializeField]
		private ObjectLocator m_ObjectLocator;

		public override void OnPreEnter()
		{
			Debug.Assert( m_EnemyPool != null );
			if( m_EnemyPool != null )
			{
				Enemy lEnemy = (Enemy)m_EnemyPool.FetchObject();
				Debug.Assert( lEnemy != null );
				if( lEnemy != null )
				{
                    lEnemy.Spawn( m_TutorialEnemyPosition, typeof( EnemyStateIdle ) );
				}
			}

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.RegisterListener( "enemy_despawn", OnTutorialEnemyDespawned );
				}

				MenuManager lMenus = m_ObjectLocator.FetchObject<MenuManager>();
				Debug.Assert( lMenus != null );
				if( lMenus != null )
				{
					lMenus.PushMenu( typeof(TutorialOverlay) );
				}
			}
		}

        public override void OnPostEnter()
        {
            StartCoroutine( EnableFiring() );
        }

		public override void OnPreExit()
		{
			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.DeregisterListener( "enemy_despawn", OnTutorialEnemyDespawned );
				}

				MenuManager lMenus = m_ObjectLocator.FetchObject<MenuManager>();
				Debug.Assert( lMenus != null );
				if( lMenus != null )
				{
					lMenus.PopMenu();
				}
			}
		}

		private void OnTutorialEnemyDespawned()
		{
			Debug.Assert( m_ObjectLocator != null );

			if( m_ObjectLocator != null )
			{
				StateManager lStates = m_ObjectLocator.FetchObject<StateManager>();

				Debug.Assert( lStates != null );
				if( lStates != null )
				{
					lStates.PushState( typeof(InGame) );
				}
			}
		}

        private IEnumerator EnableFiring()
        {
            yield return new WaitForSeconds( FIRING_ENABLE_DELAY );

            WeaponController lWeaponController = m_ObjectLocator.FetchObject<WeaponController>();
            Debug.Assert( lWeaponController != null );

            if( lWeaponController != null )
            {
                lWeaponController.SetFiringEnabled( true );
            }
        }

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Gizmos.DrawSphere( m_TutorialEnemyPosition, 0.5f );
		}
#endif
	}
}