using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Teario.Util;

namespace Teario.Halloween
{
    public class MetricsCollector : MonoBehaviour
    {
        private class Session
        {
            public double StartTime;
            public double EndTime;
            public int EnemiesDestroyed;

            public void Clear()
            {
                StartTime = 0;
                EndTime = 0;
                EnemiesDestroyed = 0;
            }
        };

        [SerializeField]
        private string m_LogFolder;

        private Session m_Session;

    	void Start()
        {
            if( !string.IsNullOrEmpty( m_LogFolder ) && !Directory.Exists( m_LogFolder ) )
            {
                Directory.CreateDirectory( m_LogFolder );
            }

            m_Session = new Session();

    	    ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );

            EventRouter lRouter = lLocator.FetchObject<EventRouter>();

            lRouter.RegisterListener( "game_start", OnGameStarted );
            lRouter.RegisterListener( "enemy_destroyed", OnEnemyDestroyed );
            lRouter.RegisterListener( "player_death", OnGameEnded );
    	}
    	
        private void OnGameStarted()
        {
            m_Session.Clear();
            m_Session.StartTime = GetTimestamp();
        }

        private void OnEnemyDestroyed()
        {
            m_Session.EnemiesDestroyed++;
        }

        private void OnGameEnded()
        {
            m_Session.EndTime = GetTimestamp();

            string lData = JsonUtility.ToJson(m_Session);
            Debug.LogError(lData);

            System.IO.File.WriteAllText( string.Format( "{0}/{1}.{2}", m_LogFolder, GetTimestamp(), ".log" ), lData );
        }

        private double GetTimestamp()
        {
            DateTime lEpoch = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
            return (DateTime.UtcNow - lEpoch).TotalSeconds;
        }
    }
}