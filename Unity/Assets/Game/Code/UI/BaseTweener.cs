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
            STATE_TWEEN,
            STATE_COMPLETE
        };

        public enum eType
        {
            TYPE_ONCE,
            TYPE_SWITCH
        }

        [SerializeField]
        private float m_Delay;
        [SerializeField]
        private AnimationCurve m_Animation;
        [SerializeField]
        private float m_Duration;
        [SerializeField]
        private eType m_Type;

        private eState m_State;
        private float m_Timer;
        private bool m_Reverse;

        public virtual void OnEnable()
        {
            Restart();
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
                    
                    if( m_Type == eType.TYPE_ONCE )
                    {
                        m_State = eState.STATE_COMPLETE;
                    }
                    else if( m_Type == eType.TYPE_SWITCH )
                    {
                        if( m_Reverse )
                        {
                            Play();
                        }
                        else
                        {
                            Reverse();
                        }

                        Restart();
                    }
                }
            }
        }

        protected abstract void UpdateTween( float lProgress );

        public void Play()
        {
            m_Reverse = false;
        }

        public void Reverse()
        {
            m_Reverse = true;
        }

        public float GetPosition()
        {
            float lProgress = m_Timer / m_Duration;

            if( m_Reverse )
            {
                return 1f - lProgress;
            }

            return lProgress;
        }

        private void Restart()
        {
            m_State = eState.STATE_DELAY;
            m_Timer = 0f;

            //UpdateTween( GetPosition() );
        }
    }
}