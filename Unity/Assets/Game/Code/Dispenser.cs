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
        private static extern bool CreateRemoteDevice( string pAddress );
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern bool BeginDeviceIdentification();
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern bool TriggerDevice();
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern bool IsDeviceIdentified();
        [System.Runtime.InteropServices.DllImport("ArduinoPlugin")]
        private static extern void DestroyDevice();

		[SerializeField]
		private ObjectLocator m_ObjectLocator;
        [SerializeField]
        private int m_PayoutInterval;

        private int m_PayoutStep;
	
		void Start()
		{
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
            if( !IsDeviceIdentified() )
            {
                StartCoroutine( InitRemoteDeviceCR() );
            }
		}

        private void DestroyRemoteDevice()
        {
            DestroyDevice();
        }

        private IEnumerator InitRemoteDeviceCR()
        {
            const float WAIT_TIME = 2f;
            const int IDENTIFY_RETRIES = 3;
            bool lComplete = false;

            string[] lPorts = FetchAvailablePorts();
            if( lPorts != null && lPorts.Length > 0 )
            {
                WaitForSeconds lDelay = new WaitForSeconds( WAIT_TIME );
    
                for( int i = 0; i < lPorts.Length && !lComplete; ++i )
                {
                    CreateRemoteDevice( lPorts[i] );
                    BeginDeviceIdentification();

                    yield return lDelay;

                    for( int r = 0; r < IDENTIFY_RETRIES; ++r )
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
                    }
                }
            }
        }

        public bool HasRemoteDevice()
        {
            return IsDeviceIdentified();
        }

        private void OnEnemyDespawned()
        {
            if( IsDeviceIdentified() )
            {
                ++m_PayoutStep;
                if( m_PayoutStep % m_PayoutInterval == 0)
                {
                    m_PayoutStep = 0;
                    TriggerDevice();
                }
            }
        }
	}
}