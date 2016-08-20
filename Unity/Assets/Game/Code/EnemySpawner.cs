using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Teario.Util;

namespace Teario.Halloween
{
	public class EnemySpawner : MonoBehaviour
	{
		[Header("Enemies")]
		[SerializeField]
		private ObjectPool m_EnemyPool;
	
		[Header("Spawn Details")]
		[SerializeField]
		private float m_SpawnSecondsMin;
		[SerializeField]
		private float m_SpawnSecondsMax;
        [SerializeField]
        private SpawnLocationManager m_SpawnPointManager;

		private float m_SpawnTimer;
		
		void Start()
		{
            Debug.Assert( m_SpawnPointManager != null, "Spawn Point Manager has not been set!" );
			Reset();
		}
	
		public void Reset()
		{
			m_SpawnTimer = Random.Range( m_SpawnSecondsMin, m_SpawnSecondsMax );
		}
	
		void Update()
		{
			if( (m_SpawnTimer -= Time.deltaTime) <= 0f )
			{
				m_SpawnTimer = Random.Range( m_SpawnSecondsMin, m_SpawnSecondsMax );
				Enemy lEnemy = (Enemy)m_EnemyPool.FetchObject();
				if( lEnemy != null )
				{
                    Vector3 lSpawnPoint = m_SpawnPointManager.FetchSpawnPoint();
                    lEnemy.Spawn( lSpawnPoint, typeof( EnemyStateSpawn ) );
				}
			}
		}
	}
}