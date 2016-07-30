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
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.RegisterListener( "enemy_despawn", OnEnemyDespawned );
				}

				MenuManager lMenus = m_ObjectLocator.FetchObject<MenuManager>();
				Debug.Assert( lMenus != null );
				if( lMenus != null )
				{
					lMenus.PushMenu( typeof(GameHUD) );
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

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.DeregisterListener( "enemy_despawn", OnEnemyDespawned );
				}

				MenuManager lMenus = m_ObjectLocator.FetchObject<MenuManager>();
				Debug.Assert( lMenus != null );
				if( lMenus != null )
				{
					lMenus.PopMenu();
				}
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
	}
}