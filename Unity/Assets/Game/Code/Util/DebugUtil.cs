#if UNITY_EDITOR || DEVELOPMENT_BUILD
#define ENABLE_DEBUG_LOGS
#endif

using UnityEngine;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace Teario.Util
{
    public class DebugUtil
    {
        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void Assert( bool lCondition )
        {
            Debug.Assert( lCondition );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void Assert( bool lCondition, string lMessage )
        {
            Debug.Assert( lCondition, lMessage );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void Log( object lMessage, Object lContext )
        {
            Debug.Log( lMessage, lContext );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void LogError( object lMessage)
        {
            Debug.LogError( lMessage );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void LogError( object lMessage, Object lContext )
        {
            Debug.LogError( lMessage, lContext );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void LogError( System.Exception lException )
        {
            Debug.LogException( lException );
        }

        [Conditional("ENABLE_DEBUG_LOGS")]
        public static void LogError( System.Exception lException, Object lContext )
        {
            Debug.LogException( lException, lContext );
        }
    }
}