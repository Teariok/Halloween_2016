using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class PositionTweener : BaseTweener
    {
        [SerializeField]
        private Vector3 m_StartPos;
        [SerializeField]
        private Vector3 m_EndPos;

        private Transform m_CachedTransform;
        private Vector3 m_CurrentPos;
        private Vector3 m_MoveDistance;

    	override public void OnEnable()
        {
    	    m_CachedTransform = transform;
            m_MoveDistance = m_EndPos - m_StartPos;
            m_CurrentPos = m_StartPos;
            
            base.OnEnable();
    	}
    	
        override protected void UpdateTween( float lProgress)
        {
            m_CurrentPos = m_StartPos + (m_MoveDistance * lProgress);
            m_CachedTransform.localPosition = m_CurrentPos;
    	}
    }
}