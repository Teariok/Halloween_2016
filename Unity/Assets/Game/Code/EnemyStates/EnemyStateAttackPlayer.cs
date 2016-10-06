using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class EnemyStateAttackPlayer : EnemyBaseState
    {
        private const string ANIMATION_NAME = "attack2";

        [SerializeField]
        private float m_AttackGraceDuration;

        private float m_AttackGraceTimer;
        private NavMeshObstacle m_NavObstacle;
        private EventRouter m_EventRouter;

        public override void EnterState()
        {
            if( m_EventRouter == null )
            {
                ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
                Debug.Assert( lLocator != null );
                m_EventRouter = lLocator.FetchObject<EventRouter>();
                Debug.Assert( m_EventRouter != null );
            }

            m_NavObstacle.enabled = true;

            m_RootTransform.LookAt( Vector3.zero );
            Quaternion lRot = m_RootTransform.rotation;
            lRot.z = 0f;
            m_RootTransform.rotation = lRot;

            PlayAnimation( ANIMATION_NAME );
            m_AttackGraceTimer = m_AttackGraceDuration;
        }
        
        public override void ExitState()
        {
            m_NavObstacle.enabled = false;
        }
    
        public void SetNavigationObstacle( NavMeshObstacle lNavObstacle )
        {
            m_NavObstacle = lNavObstacle;
        }

        void Update()
        {
            if( m_AttackGraceTimer > 0f && (m_AttackGraceTimer -= Time.deltaTime) <= 0f )
            {
                m_EventRouter.TriggerEvent( "enemy_attack_complete" );
            }
        }
    }
}