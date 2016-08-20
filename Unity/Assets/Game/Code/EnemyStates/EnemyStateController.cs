using UnityEngine;
using System.Collections;

namespace Teario.Halloween
{
    public class EnemyStateController : MonoBehaviour
    {
        private EnemyBaseState[] m_States;
        private EnemyBaseState m_Current;
    
    	void Awake()
        {
            m_Current = null;
    
            m_States = GetComponentsInChildren<EnemyBaseState>();
            Debug.Assert( m_States != null && m_States.Length > 0, "Failed to find any enemy states" );
    
            for( int i = 0; i < m_States.Length; ++i )
            {
                m_States[i].gameObject.SetActive( false );
            }
    	}

        private void OnStateFinishedCallback( System.Type lType )
        {
            SetState( lType );
        }
    
        public bool SetState( System.Type lState )
        {
            if( m_Current != null )
            {
                m_Current.DeregisterCompletionListener( OnStateFinishedCallback );
                m_Current.ExitState();
                m_Current.gameObject.SetActive( false );
            }
    
            m_Current = FetchState( lState );
            Debug.Assert( m_Current != null, "Failed to find state " + lState );

            if( m_Current != null )
            {
                m_Current.RegisterCompletionListener( OnStateFinishedCallback );
                m_Current.gameObject.SetActive( true );
                m_Current.EnterState();
            }
    
            return m_Current != null;
        }
   
        public EnemyBaseState FetchState( System.Type lState )
        {
            for( int i = 0; i < m_States.Length; ++i )
            {
                if( m_States[i].GetType() == lState )
                {
                    return m_States[i];
                }
            }

            return null;
        }
    }
}