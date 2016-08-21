using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateDie : EnemyBaseState
    {
        private const string ANIMATION_NAME = "Death";

        private NavMeshObstacle m_NavObstacle;

        public override void EnterState()
        {
            m_NavObstacle.enabled = true;

            PlayAnimation( ANIMATION_NAME, ()=>{
                m_StateExitCallback( null );
            });
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