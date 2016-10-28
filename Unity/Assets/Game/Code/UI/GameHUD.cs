using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class GameHUD : BaseMenu
	{
		[SerializeField]
		private Text m_ScoreLabel;
        [SerializeField]
        private Text m_TimeLabel;
		[SerializeField]
		private ObjectLocator m_Locator;

		private int m_CurrentScore;
        private GameTimer m_GameTimer;

		public override void OnPreEnter()
		{
			Reset();

			Debug.Assert( m_Locator != null );

            if( m_GameTimer == null )
            {
                m_GameTimer = m_Locator.FetchObject<GameTimer>();
                Debug.Assert( m_GameTimer != null );
            }

			EventRouter lEvents = m_Locator.FetchObject<EventRouter>();
			Debug.Assert( lEvents != null );
			
			if( lEvents != null )
			{
				lEvents.RegisterListener( "enemy_despawn", OnEnemyDespawned );
			}
		}

        public override void OnPreExit( System.Action lCallback )
		{
			Debug.Assert( m_Locator != null );
			EventRouter lEvents = m_Locator.FetchObject<EventRouter>();
			Debug.Assert( lEvents != null );
			
			if( lEvents != null )
			{
				lEvents.DeregisterListener( "enemy_despawn", OnEnemyDespawned );
			}

			Reset();

            lCallback();
		}

		private void Reset()
		{
			m_CurrentScore = 0;
            UpdateScore();
		}

        void Update()
        {
            if( m_GameTimer != null )
            {
                float lTimeLeft = m_GameTimer.GetTimeLeft();
                m_TimeLabel.text = lTimeLeft.ToString("n0");
            }
        }

		private void OnEnemyDespawned()
		{
			++m_CurrentScore;
			UpdateScore();
		}

		private void UpdateScore()
		{
			m_ScoreLabel.text = m_CurrentScore.ToString();
		}
	}
}