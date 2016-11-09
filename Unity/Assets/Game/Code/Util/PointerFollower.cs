using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Teario.Util
{
    public class PointerFollower : MonoBehaviour
    {
        [SerializeField]
        private RawImage m_Image;
    
        private BaseInput m_Controller;
    
        void Start()
        {
            m_Image.enabled = false;
        }
    
    	void Update()
        {
            if( m_Controller != null )
            {
                Vector2 pos = m_Controller.GetCursorPosition();
                m_Image.transform.position = new Vector3(pos.x, pos.y,0);
            }
    	}

        public void SetController( BaseInput lController )
        {
            m_Controller = lController;
            m_Image.enabled = lController != null;
        }
    }
}