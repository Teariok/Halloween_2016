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
            Vector3 lPos = m_RootTransform.position;
            m_BasePosition = lPos;
            m_AnimationTimer = m_SpawnAnimateTime;

            m_RootTransform.LookAt( Vector3.zero );

            // Force position under ground so it doesn't briefly show up
            // in the scene (we're spawning from underground)
            lPos.y = -10f;
            transform.position = lPos;

            //m_SpawnParticleSystem.emission.rate = 0;
            m_SpawnParticleSystem.Play();

            PlayAnimation( ANIMATION_NAME, () => {
                m_StateExitCallback( typeof(EnemyStateSeekPlayer) );
            });
        }
    
        public override void ExitState()
        {
            m_SpawnParticleSystem.Stop();
        }

        void Update()
        {
            if( (m_AnimationTimer -= Time.deltaTime) >= 0 )
            {
                float lSample = m_SpawnHeightCurve.Evaluate( 1.0f - (m_AnimationTimer / m_SpawnAnimateTime) );
                Vector3 lPosition = m_RootTransform.position;

                lPosition.y = m_BasePosition.y + (m_SpawnPeakHeight * lSample);

                m_RootTransform.position = lPosition;

                m_SpawnParticleSystem.maxParticles = (int)(100f * lSample);
            }
        }

        public void SetSpawnParticleSystem( ParticleSystem lParticleSystem )
        {
            m_SpawnParticleSystem = lParticleSystem;
        }
    }
}