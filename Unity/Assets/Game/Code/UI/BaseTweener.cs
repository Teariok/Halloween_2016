using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public abstract class BaseTweener : MonoBehaviour
    {
        private enum eState
        {
            STATE_NONE,
            STATE_DELAY,
            STATE_TWEEN
        };

        [SerializeField]
        private float m_Delay;
        [SerializeField]
        private AnimationCurve m_Animation;
        [SerializeField]
        private float m_Duration;

        private eState m_State;
        private float m_Timer;
        private bool m_Reverse;

        public virtual void OnEnable()
        {
            m_State = eState.STATE_DELAY;
            m_Timer = 0f;

            UpdateTween( 0f );
        }
        
        void Update()
        {
            if( m_State == eState.STATE_DELAY && (m_Timer += Time.deltaTime) >= m_Delay )
            {
                m_Timer = 0f;
                m_State = eState.STATE_TWEEN;
            }
            else if( m_State == eState.STATE_TWEEN )
            {
                if( (m_Timer += Time.deltaTime) < m_Duration )
                {
                    float lProgress = m_Timer / m_Duration;
                    if( m_Reverse )
                    {
                        lProgress = 1f - lProgress;
                    }

                    UpdateTween( m_Animation.Evaluate( lProgress ) );
                }
                else
                {
                    UpdateTween( 1f );
                    m_State = eState.STATE_NONE;
                    enabled = false;
                }
            }
        }

        protected abstract void UpdateTween( float lProgress );

        public void Play()
        {
            m_Reverse = false;
            enabled = true;
        }

        public void Reverse()
        {
            m_Reverse = true;
            enabled = true;
        }
    }
}