using UnityEngine;
using System.Collections;
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

		[Header("Spawn Locations")]
		[SerializeField]
		private Vector3[] m_SpawnLocations;
	
		private float m_SpawnTimer;
		
		void Start()
		{
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
					int lIndex = Random.Range( 0, m_SpawnLocations.Length );
					lEnemy.Spawn( m_SpawnLocations[lIndex] );
				}
			}
		}
	
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			for( int i = 0; i < m_SpawnLocations.Length; ++i )
			{
				Gizmos.DrawWireSphere( m_SpawnLocations[i], 0.5f );
			}
		}
#endif
	}
}