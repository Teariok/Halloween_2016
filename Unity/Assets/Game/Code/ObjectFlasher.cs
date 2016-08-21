using UnityEngine;
using System.Collections;

namespace Teario.Util
{
    public class ObjectFlasher : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_Objects;
        [SerializeField]
        private float m_ActiveTime;
        [SerializeField]
        private float m_StartDelay;
    
        private float m_Timer = -1f;
        private int m_Idx = 0;
    
    	void Start()
        {
            m_Timer = m_StartDelay;
    	}
    	
    	void Update()
        {
            if( m_Timer >= 0f )
            {
                if( (m_Timer -= Time.deltaTime) <= 0f )
                {
                    m_Objects[m_Idx].SetActive( false );
                    if( (++m_Idx) >= m_Objects.Length )
                    {
                        m_Idx = 0;
                    }
                    m_Objects[m_Idx].SetActive(true);
                    m_Timer = m_ActiveTime;
                }
            }
    	}
    }
}