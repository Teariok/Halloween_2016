using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private Camera m_ViewCamera;
		[SerializeField]
		private float m_MaxFireDistance;
		[SerializeField]
		private float m_AimCameraMaxRotation;
	
		private RaycastHit m_RaycastInfo;
		private Ray m_FireRay;
		//private Quaternion m_InitialDirection;
	
		private const int MOUSE_ACTION_BUTTON = 0;
	
		void Start()
		{
			//m_InitialDirection = transform.rotation;
		}
	
		void Update()
		{
			if( Input.GetMouseButtonDown( MOUSE_ACTION_BUTTON ) )
			{
				m_FireRay = m_ViewCamera.ScreenPointToRay( Input.mousePosition );
				if( Physics.Raycast( m_FireRay, out m_RaycastInfo, m_MaxFireDistance ) )
				{
					Enemy lEnemy = m_RaycastInfo.collider.GetComponent<Enemy>();
					if( lEnemy )
					{
						lEnemy.TakeHit();
					}
				}
			}
		}
	
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if( Application.isPlaying )
			{
				Gizmos.DrawLine( m_FireRay.origin, m_FireRay.origin + (m_MaxFireDistance * m_FireRay.direction) );
			}
		}
#endif
	}
}