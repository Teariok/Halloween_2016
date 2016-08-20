using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnLocationManager : MonoBehaviour
{
    // Used a transform so it can be set in the inspector
    [SerializeField]
    private Transform[] m_SpawnMarkers;

    // Position values are cached here on start instead of
    // looking them up from the transform every time
    private Queue<Vector3> m_SpawnLocations;

	void Start()
    {
        m_SpawnLocations = new Queue<Vector3>( m_SpawnMarkers.Length );
        for( int i = 0; i < m_SpawnMarkers.Length; ++i )
        {
            m_SpawnLocations.Enqueue( m_SpawnMarkers[i].position );
        }
	}

    public Vector3 FetchSpawnPoint()
    {
        Vector3 lSpawnPoint = m_SpawnLocations.Dequeue();
        m_SpawnLocations.Enqueue( lSpawnPoint );

        return lSpawnPoint;
    }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if( m_SpawnMarkers != null )
            {
                for( int i = 0; i < m_SpawnMarkers.Length; ++i )
                {
                    Gizmos.DrawWireSphere( m_SpawnMarkers[i].position, 0.25f );
                }
            }
        }
#endif
}
