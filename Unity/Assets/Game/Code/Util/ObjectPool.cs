using UnityEngine;
using System.Collections;

namespace Teario.Util
{
	public class ObjectPool : MonoBehaviour
	{
		[Header("Pool Details")]
		[SerializeField]
		private PoolableObject m_Prefab;
		[SerializeField]
		private int m_PoolSize;
	
		private PoolableObject[] m_Pool;
	
		void Awake()
		{
			m_Pool = new PoolableObject[m_PoolSize];
			for( int i = 0; i < m_PoolSize; ++i )
			{
				PoolableObject lObject = Object.Instantiate( m_Prefab );
	
				lObject.transform.parent = transform;
				lObject.gameObject.SetActive( false );
	
				m_Pool[i] = lObject;
			}
		}
	
		public PoolableObject FetchObject()
		{
			for( int i = 0; i < m_Pool.Length; ++i )
			{
				if( m_Pool[i].IsPooled() )
				{
					m_Pool[i].RemoveFromPool();
					return m_Pool[i];
				}
			}
	
			Debug.LogWarning( "An Object Pool is exhaused!" );
			return null;
		}
	}
}