using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateIdle : EnemyBaseState
    {
        private const string ANIMATION_NAME = "idle";

        public override void EnterState()
        {
            m_RootTransform.LookAt( Vector3.zero );

            m_AnimController.Play( ANIMATION_NAME );
            
        }
    
        public override void ExitState()
        {
        }
    }
}