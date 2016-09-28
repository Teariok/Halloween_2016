using UnityEngine;
using System.Collections;

public class PointerFollower : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image m_Image;
    [SerializeField]    
    private Teario.Util.BaseInput m_WC;

	// Update is called once per frame
	void Update()
    {
        Vector2 pos = m_WC.GetCursorPosition();
        m_Image.transform.position = new Vector3(pos.x, pos.y,0);
	}
}
