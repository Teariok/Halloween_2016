using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateAttackPlayer : EnemyBaseState
    {
        private const string ANIMATION_NAME = "SwingNormal";

        private NavMeshObstacle m_NavObstacle;

        public override void EnterState()
        {
            m_NavObstacle.enabled = true;

            PlayAnimation( ANIMATION_NAME );
        }
        
        public override void ExitState()
        {
            m_NavObstacle.enabled = false;
        }
    
        public void SetNavigationObstacle( NavMeshObstacle lNavObstacle )
        {
            m_NavObstacle = lNavObstacle;
        }
    }
}