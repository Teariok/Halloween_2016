using UnityEngine;
using System.Collections;

namespace Teario.Util
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_MusicClip;
        [SerializeField]
        private AudioSource m_MusicSource;
        [SerializeField]
        private float m_DuckedVolume;
        [SerializeField]
        private float m_PeakVolume;
        [SerializeField]
        private float m_VolumeStep;

        private float m_TargetVolume;
    
    	void Start()
        {
            ObjectLocator lObjectLocator = (ObjectLocator)FindObjectOfType<ObjectLocator>();
            DebugUtil.Assert( lObjectLocator != null );
            if( lObjectLocator != null )
            {
                EventRouter lEventRouter = (EventRouter)lObjectLocator.FetchObject<EventRouter>();
                DebugUtil.Assert( lEventRouter != null );
                if( lEventRouter != null )
                {
                    lEventRouter.RegisterListener( "game_start", OnGameStarted );
                    lEventRouter.RegisterListener( "player_death", OnGameEnded );
                }
            }

            m_TargetVolume = m_DuckedVolume;
    
            m_MusicSource.volume = m_TargetVolume;
            m_MusicSource.clip = m_MusicClip;
            m_MusicSource.loop = true;

            m_MusicSource.Play();
    	}

        void Update()
        {
            float lStep = m_VolumeStep * Time.deltaTime;
            if( m_MusicSource.volume > m_TargetVolume )
            {
                m_MusicSource.volume -= lStep;
                if( m_MusicSource.volume < m_TargetVolume )
                {
                    m_MusicSource.volume = m_TargetVolume;
                }
            }
            else if( m_MusicSource.volume < m_TargetVolume )
            {
                m_MusicSource.volume += lStep;
                if( m_MusicSource.volume > m_TargetVolume )
                {
                    m_MusicSource.volume = m_TargetVolume;
                }
            }
        }

        private void OnGameStarted()
        {
            m_TargetVolume = m_PeakVolume;
        }

        private void OnGameEnded()
        {
            m_TargetVolume = m_DuckedVolume;
        }
    }
}