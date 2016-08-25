using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class GameOverMenu : BaseMenu
    {
        [SerializeField]
        private Image m_Background;
        [SerializeField]
        private float m_FadeTime;

        private float m_BackgroundTimer;
        private Color m_BackgroundColour;

        void Start()
        {
            m_BackgroundColour = m_Background.color;
        }

        public override void OnPreEnter()
        {
            m_BackgroundTimer = 0f;
            m_BackgroundColour.a = 0f;

            m_Background.color = m_BackgroundColour;
        }

        void Update()
        {
            if( m_BackgroundTimer < m_FadeTime )
            {
                if( (m_BackgroundTimer += Time.deltaTime) >= m_FadeTime )
                {
                }

                m_BackgroundColour.a = Mathf.Clamp( m_BackgroundTimer / m_FadeTime, 0f, 1f );
                m_Background.color = m_BackgroundColour;
            }
        }
    }
}