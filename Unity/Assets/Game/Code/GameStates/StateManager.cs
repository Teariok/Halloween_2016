using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Teario.Util
{
	public class StateManager : MonoBehaviour
	{
		[SerializeField]
		private BaseState m_FirstState;
	
	    private Dictionary<Type, BaseState> m_AllStates;
	    private Stack<BaseState> m_States;
	
		void Awake()
	    {
			m_States = new Stack<BaseState>();

			BaseState[] lStates = GetComponentsInChildren<BaseState>( true );
			Debug.Assert( lStates != null && lStates.Length != 0, "No Game States Found!" );
	
			m_AllStates = new Dictionary<Type, BaseState>( lStates.Length );
	
			for( int i = 0; i < lStates.Length; ++i )
			{
				BaseState lCurrent = lStates[i];
				m_AllStates[lCurrent.GetType()] = lCurrent;
			}
		}

		void Start()
		{
			Reset();
		}
	
		public void Reset()
		{
			m_States.Clear();
			PushState( m_FirstState.GetType() );
		}
	
		public bool PushState( Type lState )
		{
			if( m_AllStates.ContainsKey( lState ) && ( m_States.Count == 0 || m_States.Peek().GetType() != lState ) )
			{
				BaseState lCurrent = m_States.Count == 0 ? null : m_States.Peek();
				BaseState lNext = m_AllStates[lState];
	
				if( lCurrent != null )
				{
					lCurrent.OnPreExit();
				}

				lNext.OnPreEnter();
	
				if( lCurrent != null )
				{
					lCurrent.gameObject.SetActive( false );
				}

				m_States.Push( lNext );
				lNext.gameObject.SetActive( true );
				
				lNext.OnPostEnter();

				if( lCurrent != null )
				{
					lCurrent.OnPostExit();
				}
	
				return true;
			}
	
			return false;
		}

		public bool PopState()
		{
			return true;
		}
	}
}