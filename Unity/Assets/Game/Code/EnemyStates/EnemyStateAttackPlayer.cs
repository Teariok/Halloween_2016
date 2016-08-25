using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateAttackPlayer : EnemyBaseState
    {
        private const string ANIMATION_NAME = "SwingNormal";

        private NavMeshObstacle m_NavObstacle;
        private GameObject m_AttackCollider;

        public override void EnterState()
        {
            m_NavObstacle.enabled = true;

            m_RootTransform.LookAt( Vector3.zero );
            Quaternion lRot = m_RootTransform.rotation;
            lRot.z = 0f;
            m_RootTransform.rotation = lRot;

            PlayAnimation( ANIMATION_NAME );

            m_AttackCollider.SetActive( true );
        }
        
        public override void ExitState()
        {
            m_NavObstacle.enabled = false;
            m_AttackCollider.SetActive( false );
        }
    
        public void SetNavigationObstacle( NavMeshObstacle lNavObstacle )
        {
            m_NavObstacle = lNavObstacle;
        }

        public void SetAttackCollider( GameObject lObject )
        {
            m_AttackCollider = lObject;
        }
    }
}