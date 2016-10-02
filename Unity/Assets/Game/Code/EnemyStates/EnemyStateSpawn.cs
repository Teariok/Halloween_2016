using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateSpawn : EnemyBaseState
    {
        [SerializeField]
        private float m_SpawnPeakHeight;
        [SerializeField]
        private AnimationCurve m_SpawnHeightCurve;
        [SerializeField]
        private float m_SpawnAnimateTime;

        private float m_AnimationTimer;
        private const string ANIMATION_NAME = "surface";
        private Vector3 m_BasePosition;
        private ParticleSystem m_SpawnParticleSystem;

        public override void EnterState()
        {
            m_SpawnParticleSystem.Play();

            PlayAnimation( ANIMATION_NAME, () => {
                m_StateExitCallback( typeof(EnemyStateSeekPlayer) );
            });
        }
    
        public override void ExitState()
        {
            m_SpawnParticleSystem.Stop();
        }

        public void SetSpawnParticleSystem( ParticleSystem lParticleSystem )
        {
            m_SpawnParticleSystem = lParticleSystem;
        }
    }
}