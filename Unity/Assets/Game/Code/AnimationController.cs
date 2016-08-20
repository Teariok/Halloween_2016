using UnityEngine;
using System.Collections;

namespace Teario.Util
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animation m_Animator;
    
        public bool Play( string lAnimationName )
        {
            return m_Animator.Play( lAnimationName, PlayMode.StopAll );
        }

        public bool IsPlaying( string lClip = null )
        {
            if( string.IsNullOrEmpty( lClip ) )
            {
                return m_Animator.isPlaying;
            }

            return m_Animator.IsPlaying( lClip );
        }
    }
}