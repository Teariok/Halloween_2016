using UnityEngine;
using System.Collections;
using Teario.Util;
using System;

namespace Teario.Halloween
{
    public abstract class EnemyBaseState : MonoBehaviour
    {
        private const float ANIMATION_CHECK_INTERVAL = 0.25f;

        [SerializeField]
        protected Transform m_RootTransform;

        private string m_CachedAnimName = null;
        private Action m_CachedAnimCallback = null;
    
        protected AnimationController m_AnimController;
        protected System.Action<Type> m_StateExitCallback;

        void Awake()
        {
            m_AnimController = GetComponentInParent<AnimationController>();
            Debug.Assert( m_AnimController != null, "Failed to find enemy animation controller" );

            if( !string.IsNullOrEmpty( m_CachedAnimName ) )
            {
                PlayAnimation( m_CachedAnimName, m_CachedAnimCallback );
                m_CachedAnimName = null;
                m_CachedAnimCallback = null;
            }
        }

        public void RegisterCompletionListener( Action<Type> lListener )
        {
            m_StateExitCallback += lListener;
        }

        public void DeregisterCompletionListener( Action<Type> lListener )
        {
            m_StateExitCallback -= lListener;
        }

        protected bool PlayAnimation( string lAnimName, Action lCallback = null )
        {
            bool lResult = false;

            if( m_AnimController != null )
            {
                lResult = m_AnimController.Play( lAnimName );

                if( lCallback != null )
                {
                    StartCoroutine( WaitForAnimFinish( lCallback ) );
                }
            }
            else
            {
                m_CachedAnimName = lAnimName;
                m_CachedAnimCallback = lCallback;
            }

            return lResult;
        }

        protected IEnumerator WaitForAnimFinish( Action lCallback )
        {
            if( lCallback != null )
            {
                while( m_AnimController.IsPlaying() )
                {
                    yield return new WaitForSeconds( ANIMATION_CHECK_INTERVAL );
                }

                lCallback();
            }
        }
    
        public virtual void EnterState(){}
        public virtual void ExitState(){}
    }
}