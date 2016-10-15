using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class TutorialOverlay : BaseMenu
	{
        [SerializeField]
        private Text m_ShootLabel;

        [SerializeField]
        private float m_ExitDuration;

        private float m_ExitTimer;

        public override void OnPreEnter()
        {
            Debug.Assert( m_ShootLabel != null );

            Color lColour = m_ShootLabel.color;
            lColour.a = 1f;
            m_ShootLabel.color = lColour;
        }

        public override void OnPreExit( System.Action lCallback )
        {
            StartCoroutine( ExitMenu(lCallback) );
        }

        private IEnumerator ExitMenu( System.Action lCallback )
        {
            Color lColour = m_ShootLabel.color;
            m_ExitTimer = m_ExitDuration;

            WaitForEndOfFrame lFrameWait = new WaitForEndOfFrame();

            while( lColour.a > 0f )
            {
                yield return lFrameWait;

                m_ExitTimer -= Time.deltaTime;
                lColour.a = m_ExitTimer / m_ExitDuration;
                m_ShootLabel.color = lColour;
            }

            lColour.a = 0f;
            m_ShootLabel.color = lColour;

            lCallback();
        }
	}
}
