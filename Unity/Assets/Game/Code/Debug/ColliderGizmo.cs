using UnityEngine;
using System.Collections;

namespace Teario.Util
{
    public class ColliderGizmo : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            BoxCollider lCollider = GetComponent<BoxCollider>();
            if( lCollider )
            {
                Color lColour = Gizmos.color;
                Gizmos.color = Color.green;

                Gizmos.DrawWireCube( gameObject.transform.position + lCollider.center, lCollider.size );

                Gizmos.color = lColour;
            }
        }
    }
}