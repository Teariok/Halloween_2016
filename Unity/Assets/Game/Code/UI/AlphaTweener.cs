using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Teario.Halloween
{
    public class AlphaTweener : BaseTweener
    {
        [SerializeField]
        private float m_StartAlpha;
        [SerializeField]
        private float m_EndAlpha;

        private Graphic m_Image;
        private float m_ChangeAmount;

        override public void OnEnable()
        {
            m_Image = GetComponent<Graphic>();
            Debug.Assert( m_Image != null );

            m_ChangeAmount = m_EndAlpha - m_StartAlpha;

            base.OnEnable();
        }
        
        override protected void UpdateTween( float lProgress )
        {
            Color lColour = m_Image.color;
            lColour.a = m_StartAlpha + (m_ChangeAmount * lProgress);
            m_Image.color = lColour;
        }
    }
}