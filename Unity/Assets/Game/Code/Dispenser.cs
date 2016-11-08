using UnityEngine;
using System.Collections;
using Teario.Util;
using System.IO.Ports;
using System.IO;

using System;

namespace Teario.Halloween
{
	public class Dispenser : MonoBehaviour
	{
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern bool OpenSerialPort( string pAddress );
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern void CloseSerialPort();

		[SerializeField]
		private ObjectLocator m_ObjectLocator;
        [SerializeField]
        private int m_PayoutInterval;

        private int m_PayoutStep;
        private SerialPort m_SerialPort;
        private bool m_Initialised;
	
		void Start()
		{
            m_Initialised = false;

			Debug.Assert( m_ObjectLocator != null );
			if( m_ObjectLocator != null )
			{
				EventRouter lEvents = m_ObjectLocator.FetchObject<EventRouter>();
				Debug.Assert( lEvents != null );
				if( lEvents != null )
				{
                    lEvents.RegisterListener( "game_start", Reset );
					lEvents.RegisterListener( "enemy_destroyed", OnEnemyDespawned );
				}
			}
		}

        void Reset()
        {
            m_PayoutStep = 0;
        }

        void OnDisable()
        {
            DestroyRemoteDevice();
            m_Initialised = false;
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

		public void InitRemoteDevice()
		{
            StartCoroutine( InitRemoteDeviceCR() );
		}

        private void DestroyRemoteDevice()
        {
            EndSerial();
        }

        private IEnumerator InitRemoteDeviceCR()
        {
            const float WAIT_TIME = 2f;
            const int IDENTIFY_RETRIES = 3;
            bool lComplete = false;

            string[] lPorts = FetchAvailablePorts();
            if( lPorts != null && lPorts.Length > 0 )
            {
                for( int i = 0; i < lPorts.Length && !lComplete; ++i )
                {
                    if( !BeginSerial( lPorts[i] ) )
                    {
                        continue;
                    }

                    yield return new WaitForSeconds( 5f );

                    m_SerialPort.Write( new byte[]{0xFF}, 0, 1 );

                    yield return new WaitForSeconds( 2f );

                    /*for( int r = 0; r < IDENTIFY_RETRIES; ++r )
                    {
                        if( IsDeviceIdentified() )
                        {
                            lComplete = true;
                            break;
                        }

                        yield return lDelay;
                    }

                    if( !lComplete )
                    {
                        DestroyDevice();
                    }*/
                }
            }

            m_Initialised = true;
        }

        private bool BeginSerial( string lPort )
        {
            bool lResult = false;

            #if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                lResult = OpenSerialPort( lPort );
            #endif

            if( lResult )
            {
                m_SerialPort = new SerialPort( lPort );
                m_SerialPort.Open();
            }

            return lResult;
        }

        private void EndSerial()
        {
            if( HasRemoteDevice() )
            {
                m_SerialPort.Close();
                m_SerialPort = null;
            }

            CloseSerialPort();
        }

        public bool HasRemoteDevice()
        {
            return m_Initialised && m_SerialPort != null && m_SerialPort.IsOpen;
        }

        private void OnEnemyDespawned()
        {
            if( HasRemoteDevice() )
            {
                ++m_PayoutStep;
                if( m_PayoutStep % m_PayoutInterval == 0)
                {
                    m_PayoutStep = 0;
                    m_SerialPort.Write( new byte[]{0xFF}, 0, 1 );
                }
            }
        }
	}
}