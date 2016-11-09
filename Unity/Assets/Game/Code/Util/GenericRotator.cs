using UnityEngine;
using System.Collections;

public class GenericRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Speed;

    private Vector3 m_Rotation;

    void Start()
    {
        m_Rotation = transform.localRotation.eulerAngles;
    }

	void Update()
    {
        m_Rotation.x += m_Speed.x * Time.deltaTime;
        m_Rotation.y += m_Speed.y * Time.deltaTime;
        m_Rotation.z += m_Speed.z * Time.deltaTime;

        transform.localRotation = Quaternion.Euler( m_Rotation );
	}
}
