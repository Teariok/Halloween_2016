using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField]
        private float m_GameTime;

        private bool m_IsActive = false;
        private float m_ActiveTimer;
        private EventRouter m_EventRouter;

        void Start()
        {
            ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );

            m_EventRouter = lLocator.FetchObject<EventRouter>();
            Debug.Assert( m_EventRouter != null );
            m_EventRouter.RegisterListener( "game_start", OnGameStarted );
            m_EventRouter.RegisterListener( "game_reset", OnGameReset );
        }

        void Update()
        {
            if( m_IsActive && ( m_ActiveTimer += Time.deltaTime ) >= m_GameTime )
            {
                m_EventRouter.TriggerEvent( "game_expired" );
            }
        }

        private void OnGameStarted()
        {
            m_ActiveTimer = 0f;
            m_IsActive = true;
        }

        private void OnGameReset()
        {
            m_ActiveTimer = 0f;
            m_IsActive = false;
        }

        public float GetTimeLeft()
        {
            return m_GameTime - m_ActiveTimer;
        }
    }
}