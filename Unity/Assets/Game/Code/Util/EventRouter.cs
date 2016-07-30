using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Teario.Util
{
	public class EventRouter : MonoBehaviour
	{
		private Dictionary<string, Action> m_Listeners;
	
		void Awake()
		{
			EventRouter[] lRouters = FindObjectsOfType<EventRouter>();
			
			Debug.Assert( lRouters.Length == 1, "Only one EventRouter instance is allowed. This instance will be deleted." );
			if( lRouters.Length > 1 )
			{
				GameObject.Destroy( gameObject );
			}
	
			m_Listeners = new Dictionary<string, Action>();
		}
	
		public void RegisterListener( string lName, System.Action lCallback )
		{
			if( m_Listeners.ContainsKey(lName) )
			{
				m_Listeners[lName] += lCallback;
			}
			else
			{
				m_Listeners[lName] = lCallback;
			}
		}

		public void DeregisterListener( string lName, System.Action lCallback )
		{
			if( m_Listeners.ContainsKey(lName) )
			{
				m_Listeners[lName] -= lCallback;
			}
		}
	
		public void TriggerEvent( string lName )
		{
			if( m_Listeners.ContainsKey( lName ) && m_Listeners[lName] != null )
			{
				m_Listeners[lName]();
			}
		}
	}
}