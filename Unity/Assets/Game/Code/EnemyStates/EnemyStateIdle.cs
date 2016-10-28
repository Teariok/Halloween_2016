using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
    public class EnemyStateIdle : EnemyBaseState
    {
        [SerializeField]
        private int m_AudioPlayMin;
        [SerializeField]
        private int m_AudioPlayMax;

        private const string ANIMATION_NAME = "idle";

        private float m_AudioTimer;

        public override void EnterState()
        {
            m_RootTransform.LookAt( Vector3.zero );

            m_AnimController.Play( ANIMATION_NAME );
        }

        void Update()
        {
            if( (m_AudioTimer -= Time.deltaTime) <= 0 )
            {
                m_AudioTimer = Random.Range( m_AudioPlayMin, m_AudioPlayMax );

                AudioClip lClip = GetAudioProvider().GetRandom( m_AudioName );
                Debug.Assert( lClip != null );
                m_AudioSource.PlayOneShot( lClip );
            }
        }
    }
}