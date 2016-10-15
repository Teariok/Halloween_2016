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
		private ObjectLocator m_Locator;

		private int m_CurrentScore;

		public override void OnPreEnter()
		{
			Reset();

			Debug.Assert( m_Locator != null );
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