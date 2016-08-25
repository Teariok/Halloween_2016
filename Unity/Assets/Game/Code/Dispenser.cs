using UnityEngine;
using System.Collections;
using Teario.Util;
using System.IO.Ports;
using System.IO;

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
            m_SerialPort = null;
			//InitSerialPort();

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
					lEvents.RegisterListener( "enemy_destroyed", OnEnemyDespawned );
				}
			}
		}
		
		void Update()
		{
		
		}

        private string[] FetchAvailablePorts()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return SerialPort.GetPortNames();
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            string[] lCandidates = Directory.GetFiles( "/dev", "tty.usbmodem*" );
            for( int i = 0; i < lCandidates.Length; ++i )
            {
                Debug.Log(lCandidates[i]);
            }

            return lCandidates;
#else
            return null;
#endif
        }

		private void OnEnemyDespawned()
		{
			if( m_SerialPort != null )
			{
                m_SerialPort.Write( "1" );
				m_SerialPort.BaseStream.Flush();
			}
		}

		private void InitSerialPort()
		{
            CloseSerialPort();

			m_SerialPort = new SerialPort();// m_SerialName, 9600 );
            m_SerialPort.ReadBufferSize = 8192;
            m_SerialPort.WriteBufferSize = 128;
            m_SerialPort.PortName = m_SerialName;
            m_SerialPort.BaudRate = 9600;
            m_SerialPort.Parity = Parity.None;
            m_SerialPort.StopBits = StopBits.One;

			m_SerialPort.Open();
			m_SerialPort.DiscardInBuffer();
		}

        private void CloseSerialPort()
        {
            if( m_SerialPort != null )
            {
                m_SerialPort.Close();
                m_SerialPort = null;
            }
        }

        void OnGUI()
        {
            if( GUI.Button( new Rect(0,0,100,10), "Open Serial" ) )
            {
                InitSerialPort();
            }

            if( GUI.Button( new Rect(150,0,100,10), "Close Serial" ) )
            {
                CloseSerialPort();
            }

            if( GUI.Button( new Rect(300,0,100,10), "Test Serial" ) )
            {
                OnEnemyDespawned();
            }
        }
	}
}