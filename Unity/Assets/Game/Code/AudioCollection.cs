using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioCollection
{
    [SerializeField]
    private AudioClip[] m_AudioClips;
    [SerializeField]
    private string m_Name;

    public AudioClip GetRandom()
    {
        return m_AudioClips[ Random.Range(0, m_AudioClips.Length) ];
    }

    public AudioClip GetIndex( int lIndex )
    {
        if( lIndex >= 0 && lIndex < m_AudioClips.Length )
        {
            return m_AudioClips[ lIndex ];
        }

        return null;
    }

    public string GetName()
    {
        return m_Name;
    }
}
