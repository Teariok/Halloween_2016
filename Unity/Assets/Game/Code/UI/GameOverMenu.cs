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

        private BaseTweener[] m_Tweens;

        private EventRouter m_EventRouter;
        private bool m_PendingEventTrigger;

        void Awake()
        {
            Debug.Assert( m_TriggerTween != null );

            ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
            Debug.Assert( lLocator != null );

            m_EventRouter = lLocator.FetchObject<EventRouter>();
            Debug.Assert( m_EventRouter != null );

            m_Tweens = GetComponentsInChildren<BaseTweener>( true );
            Debug.Assert( m_Tweens != null );
        }

        public override void OnPostEnter()
        {
            m_PendingEventTrigger = true;

            for( int i = 0; i < m_Tweens.Length; ++i )
            {
                m_Tweens[i].Play();
            }
        }

        public override void OnPreExit( System.Action lCallback )
        {
            StartCoroutine( ExitTransition(lCallback) );
        }

        void Update()
        {
            if( m_PendingEventTrigger && m_TriggerTween.GetPosition() >= 1f )
            {
                m_PendingEventTrigger = false;
                m_EventRouter.TriggerEvent( "game_covered" );
            }
        }

        private IEnumerator ExitTransition( System.Action lCallback )
        {
            for( int i = 0; i < m_Tweens.Length; ++i )
            {
                m_Tweens[i].Reverse();
            }

            WaitForEndOfFrame lFrameWait = new WaitForEndOfFrame();

            while( m_Tweens[m_Tweens.Length-1].GetPosition() < 1f )
            {
                yield return lFrameWait;
            }

            lCallback();
        }
    }
}