using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Teario.Halloween
{
    public class LightIntensityTweener : BaseTweener
    {
        [SerializeField]
        private float m_StartValue;
        [SerializeField]
        private float m_EndValue;

        private Light m_Light;
        private float m_ChangeAmount;

        override public void OnEnable()
        {
            m_Light = GetComponent<Light>();
            Debug.Assert( m_Light != null );

            m_ChangeAmount = m_EndValue - m_StartValue;

            base.OnEnable();
        }
        
        override protected void UpdateTween( float lProgress )
        {
            float lIntensity = m_Light.intensity;
            lIntensity = m_StartValue + (m_ChangeAmount * lProgress);
            m_Light.intensity = lIntensity;
        }
    }
}