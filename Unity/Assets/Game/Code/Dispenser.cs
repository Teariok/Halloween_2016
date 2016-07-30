using UnityEngine;
using System.Collections;
using Teario.Util;
using System.IO.Ports;

namespace Teario.Halloween
{
	public class Dispenser : MonoBehaviour
	{
		[SerializeField]
		private ObjectLocator m_ObjectLocator;
		[SerializeField]
		private string m_SerialName;

		private SerialPort m_SerialPort;
	
		void Start()
		{
			InitSerialPort();

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.RegisterListener( "enemy_despawn", OnEnemyDespawned );
				}
			}
		}
		
		void Update()
		{
		
		}

		private void OnEnemyDespawned()
		{
			/*if( m_SerialPort != null )
			{
				m_SerialPort.Write("1");
				m_SerialPort.BaseStream.Flush();
			}*/
		}

		private void InitSerialPort()
		{
			/*m_SerialPort = new SerialPort( m_SerialName, 9600 );
			m_SerialPort.Open();
			m_SerialPort.DiscardInBuffer();*/
		}
	}
}