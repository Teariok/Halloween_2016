using UnityEngine;
using System.Collections;

namespace Teario.Util
{
	public class PoolableObject : MonoBehaviour
	{
		private bool m_IsActive = false;
		
		public bool IsPooled()
		{
			return !m_IsActive;
		}
	
		public void RemoveFromPool()
		{
			m_IsActive = true;
			gameObject.SetActive( true );
		}
	
		public void ReturnToPool()
		{
			m_IsActive = false;
			gameObject.SetActive( false );
		}
	}
}