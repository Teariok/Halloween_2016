using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateDie : EnemyBaseState
    {
        private const string ANIMATION_NAME = "die";

        private NavMeshObstacle m_NavObstacle;
        private ParticleSystem m_DespawnParticleSystem;

        public override void EnterState()
        {
            m_NavObstacle.enabled = true;
            m_DespawnParticleSystem.Play();

            PlayAnimation( ANIMATION_NAME, ()=>{
                m_StateExitCallback( null );
            });
        }
        
        public override void ExitState()
        {
            m_DespawnParticleSystem.Stop();
            m_NavObstacle.enabled = false;
        }

        public void SetNavigationObstacle( NavMeshObstacle lNavObstacle )
        {
            m_NavObstacle = lNavObstacle;
        }

        public void SetDespawnParticleSystem( ParticleSystem lParticleSystem )
        {
            m_DespawnParticleSystem = lParticleSystem;
        }
    }
}