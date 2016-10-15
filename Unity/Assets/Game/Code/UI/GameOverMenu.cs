using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class GameOverMenu : BaseMenu
    {
        [SerializeField]
        private BaseTweener m_TriggerTween;

        private EventRouter m_EventRouter;
        private bool m_PendingEventTrigger;

        void Start()
        {
            Debug.Assert( m_TriggerTween != null );

            ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );

            m_EventRouter = lLocator.FetchObject<EventRouter>();
            Debug.Assert( m_EventRouter != null );
        }

        public override void OnPostEnter()
        {
            m_PendingEventTrigger = true;
        }

        void Update()
        {
            if( m_PendingEventTrigger && m_TriggerTween.GetPosition() >= 1f )
            {
                m_PendingEventTrigger = false;
                m_EventRouter.TriggerEvent( "game_covered" );
            }
        }
    }
}