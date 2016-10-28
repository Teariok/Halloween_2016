using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class EnemyStateHurt : EnemyBaseState
    {
        private string[] ANIMATION_NAMES = { "hitleft", "hitright" };

        private float m_FadeTimer;
        private NavMeshObstacle m_NavObstacle;
        private ParticleSystem m_DespawnParticleSystem;

        public override void EnterState()
        {
            AudioClip lClip = GetAudioProvider().GetRandom( m_AudioName );
            Debug.Assert( lClip != null );
            m_AudioSource.PlayOneShot( lClip );

            int lAnimIdx = Random.Range( 0, ANIMATION_NAMES.Length );
            PlayAnimation( ANIMATION_NAMES[lAnimIdx], () => {
                m_StateExitCallback( typeof(EnemyStateSeekPlayer) );
            } );
        }
    }
}