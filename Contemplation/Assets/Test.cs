using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
	public float m_RotationSpeed;
	public bool m_GoesUpwards;

	// Update is called once per frame
	void Update () 
	{
		if (m_GoesUpwards == true)
		{
			transform.Rotate(Vector3.up * Time.deltaTime * m_RotationSpeed);
		}

		else 
		{
			transform.Rotate(Vector3.right * Time.deltaTime * m_RotationSpeed);
		}
	}

	void OnDrawGizmos()
	{
		if (m_GoesUpwards == true)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * 200);
		}

		else
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + transform.forward * 200);
		}
	}
}
