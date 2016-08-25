using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateSeekPlayer : EnemyBaseState
    {
        private const float DISTANCE_CHECK_INTERVAL = 0.5f;

        [SerializeField]
        private float m_AttackRadius;

        private NavMeshAgent m_NavAgent;
        private float m_Speed;
    
        private const string ANIMATION_WALK_NAME = "Walk01";
        private const string ANIMATION_RUN_NAME = "Run";
        private const float RUN_SWITCH_SPEED = 5f;
    
        public override void EnterState()
        {
            m_NavAgent.enabled = true;
            m_NavAgent.destination = Vector3.zero;
            m_NavAgent.speed = m_Speed;

            PlayAnimation( m_Speed < RUN_SWITCH_SPEED ? ANIMATION_WALK_NAME : ANIMATION_RUN_NAME );
            StartCoroutine( WaitForPlayerInRange( ()=>{
                m_StateExitCallback( typeof(EnemyStateAttackPlayer) );
            } ) );
        }
        
        public override void ExitState()
        {
            m_NavAgent.enabled = false;
        }
    
        public void SetNavigationAgent( NavMeshAgent lAgent )
        {
            m_NavAgent = lAgent;        
        }

        public void SetMoveSpeed( float lSpeed )
        {
            m_Speed = lSpeed;
        }

        private IEnumerator WaitForPlayerInRange( System.Action lCallback )
        {
            Debug.Assert( lCallback != null );
            
            if( lCallback != null )
            {
                while( (m_RootTransform.position - Vector3.zero).magnitude > m_AttackRadius )
                {
                    yield return new WaitForSeconds( DISTANCE_CHECK_INTERVAL );
                }
    
                lCallback();
            }
        }
    }
}