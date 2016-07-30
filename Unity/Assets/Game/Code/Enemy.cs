using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class Enemy : PoolableObject
	{
		[SerializeField]
		private float m_WalkSpeed;
		[SerializeField]
		private int m_MaxHealth;
		
		private int m_Health;
	
		private EventRouter m_EventRouter;

		public enum EnemyState
		{
			eIdle,
			eWalking,
			eBumpWait,
			eAttack
		};
		
		private EnemyState m_State;
	
		void Start()
		{
			ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
			Debug.Assert( lLocator != null );
			if( lLocator != null )
			{
				m_EventRouter = ObjectLocator.FindObjectOfType<EventRouter>();
				Debug.Assert( m_EventRouter != null );
			}
		}
		
		public void Spawn( Vector3 lPosition, EnemyState lDefaultState = EnemyState.eWalking )
		{
			transform.position = lPosition;
			m_Health = m_MaxHealth;
			SetState( lDefaultState );
		}
		
		void Update()
		{
			switch( m_State )
			{
				case EnemyState.eWalking:
				{
					Vector3 lTarget = Vector3.zero;
					transform.LookAt( lTarget );
					transform.position = Vector3.MoveTowards( transform.position, Vector3.zero, m_WalkSpeed * Time.deltaTime );
				}
				break;
			}
		}
	
		public void TakeHit()
		{
			if( m_Health > 0 && (--m_Health <= 0) )
			{
				m_EventRouter.TriggerEvent( "enemy_despawn" );
				ReturnToPool();
			}
		}

		private void SetState( EnemyState lNewState )
		{
			m_State = lNewState;
		}
	}
}