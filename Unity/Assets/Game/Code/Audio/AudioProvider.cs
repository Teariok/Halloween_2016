using UnityEngine;
using System.Collections;

namespace Teario.Util
{
    public class AudioProvider : MonoBehaviour
    {
        [SerializeField]
        private AudioCollection[] m_Collections;
    
        public AudioClip GetRandom( string lCollectionName )
        {
            AudioCollection lCollection = FindCollection( lCollectionName );
            Debug.Assert( lCollection != null );
    
            return lCollection.GetRandom();
        }
    
        public AudioClip GetIndex( string lCollectionName, int lIndex )
        {
            AudioCollection lCollection = FindCollection( lCollectionName );
            Debug.Assert( lCollection != null );
    
            return lCollection.GetIndex( lIndex );
        }
    
        private AudioCollection FindCollection( string lName )
        {
            for( int i = 0; i < m_Collections.Length; ++i )
            {
                if( m_Collections[i].GetName().Equals( lName ) )
                {
                    return m_Collections[i];
                }
            }
    
            return null;
        }
    }
}