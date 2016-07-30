using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Teario.Util
{
	public class ObjectLocator : MonoBehaviour
	{
		private Dictionary<Type, object> m_Cache;
	
		void Awake()
		{
			m_Cache = new Dictionary<Type, object>();
			ObjectLocator[] lLocators = FindObjectsOfType<ObjectLocator>();
			
			Debug.Assert( lLocators.Length == 1, "Only one ObjectLocator instance is allowed. This instance will be deleted." );
			if( lLocators.Length > 1 )
			{
				GameObject.Destroy( gameObject );
			}
		}
	
		public T FetchObject<T>( bool lForceRecache = false ) where T : MonoBehaviour
		{
			Type lType = typeof( T );
			if( !lForceRecache && m_Cache.ContainsKey( lType ) )
			{
				return (T)m_Cache[lType];
			}
			
			T lObject = (T)FindObjectOfType<T>();
			if( lObject != null )
			{
				m_Cache[lType] = lObject;
			}
	
			return lObject;
		}
	
		public T[] FetchAllObjects<T>( bool lForceRecache = false ) where T : MonoBehaviour
		{
			Type lType = typeof( T );
			if( !lForceRecache && m_Cache.ContainsKey( lType ) )
			{
				return (T[])m_Cache[lType];
			}
	
			T[] lObject = FindObjectsOfType<T>();
			if( lObject != null )
			{
				m_Cache[lType] = lObject;
			}
	
			return lObject;
		}
	}
}