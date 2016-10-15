using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class InGame : BaseState
	{
		[SerializeField]
		private ObjectLocator m_ObjectLocator;
		[SerializeField]
		private int m_SpawnerIncreaseStep;

        private EventRouter m_EventRouter;
        private MenuManager m_MenuManager;
		private EnemySpawner[] m_EnemySpawners;
		private int m_EnemiesKilled;

		public override void OnPreEnter()
		{
			m_EnemiesKilled = 0;

			m_EnemySpawners = GetComponentsInChildren<EnemySpawner>( true );
			Debug.Assert( m_EnemySpawners.Length > 0, "No Enemy Spawners Found!" );

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				m_EventRouter = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( m_EventRouter != null );
				if( m_EventRouter != null )
				{
					m_EventRouter.RegisterListener( "enemy_despawn", OnEnemyDespawned );
                    m_EventRouter.RegisterListener( "player_death", OnPlayerKilled );
                    m_EventRouter.RegisterListener( "game_covered", OnGameCovered );
                    m_EventRouter.TriggerEvent( "game_start" );
				}

				m_MenuManager = m_ObjectLocator.FetchObject<MenuManager>();
				Debug.Assert( m_MenuManager != null );
				if( m_MenuManager != null )
				{
					m_MenuManager.PushMenu( typeof(GameHUD) );
				}
			}
		}

		public override void OnPostEnter()
		{
			Debug.Assert( m_EnemySpawners.Length > 0 );
			m_EnemySpawners[0].gameObject.SetActive( true );
		}

		public override void OnPreExit()
		{
			for( int i = 0; i < m_EnemySpawners.Length; ++i )
			{
				m_EnemySpawners[i].Reset();
				m_EnemySpawners[i].gameObject.SetActive( false );
			}

			if( m_EventRouter != null )
			{
				m_EventRouter.DeregisterListener( "enemy_despawn", OnEnemyDespawned );
			}

			if( m_MenuManager != null )
			{
				m_MenuManager.PopMenu();
			}

            WeaponController lWeaponController = m_ObjectLocator.FetchObject<WeaponController>();
            Debug.Assert( lWeaponController != null );
            if( lWeaponController != null )
            {
                lWeaponController.SetFiringEnabled( false );
            }
		}

		private void OnEnemyDespawned()
		{
			++m_EnemiesKilled;
			if( m_EnemiesKilled % m_SpawnerIncreaseStep == 0 )
			{
				for( int i = 0; i < m_EnemySpawners.Length; ++i )
				{
					if( !m_EnemySpawners[i].gameObject.activeInHierarchy )
					{
						m_EnemySpawners[i].gameObject.SetActive( true );
						break;
					}
				}
			}
		}

        private void OnPlayerKilled()
        {
            m_MenuManager.PushMenu( typeof(GameOverMenu) );
        }

        private void OnGameCovered()
        {
            m_EventRouter.TriggerEvent("game_reset");
            Debug.Assert( m_ObjectLocator != null );

            if( m_ObjectLocator != null )
            {
                StateManager lStates = m_ObjectLocator.FetchObject<StateManager>();

                Debug.Assert( lStates != null );
                if( lStates != null )
                {
                    lStates.PopState();
                }
            }
        }
	}
}