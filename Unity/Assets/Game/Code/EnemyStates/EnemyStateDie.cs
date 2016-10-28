using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateDie : EnemyBaseState
    {
        private const string ANIMATION_NAME = "die";

        private Material m_Material;
        private float m_OriginalAlpha;

        [SerializeField]
        private float m_FadeDuration;
        [SerializeField]
        private AnimationCurve m_FadeCurve;

        private float m_FadeTimer;
        private NavMeshObstacle m_NavObstacle;
        private ParticleSystem m_DespawnParticleSystem;

        public override void EnterState()
        {
            m_FadeTimer = m_FadeDuration;

            m_NavObstacle.enabled = true;
            m_DespawnParticleSystem.Play();

            AudioClip lClip = GetAudioProvider().GetRandom( m_AudioName );
            Debug.Assert( lClip != null );
            m_AudioSource.PlayOneShot( lClip );

            PlayAnimation( ANIMATION_NAME );
        }
        
        public override void ExitState()
        {
            m_DespawnParticleSystem.Stop();
            m_NavObstacle.enabled = false;

            Color lColor = m_Material.color;
            lColor.a = m_OriginalAlpha;
            m_Material.color = lColor;
        }

        void Update()
        {
            if( m_FadeTimer > 0f )
            {
                Color lColor = m_Material.color;
                lColor.a = m_FadeCurve.Evaluate( m_FadeTimer / m_FadeDuration );
                m_Material.color = lColor;

                if( ( m_FadeTimer -= Time.deltaTime ) <= 0f )
                {
                    m_StateExitCallback( null );
                }
            }
        }

        public void SetNavigationObstacle( NavMeshObstacle lNavObstacle )
        {
            m_NavObstacle = lNavObstacle;
        }

        public void SetMaterial( Material lMaterial )
        {
            m_Material = lMaterial;
            m_OriginalAlpha = lMaterial.color.a;
        }

        public void SetDespawnParticleSystem( ParticleSystem lParticleSystem )
        {
            m_DespawnParticleSystem = lParticleSystem;
        }

        
    }
}