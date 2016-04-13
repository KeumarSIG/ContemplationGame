using UnityEngine;
using System.Collections;

public class RaycastTest : MonoBehaviour 
{
	private float m_DistanceForward;
	private float m_DistanceBottom;
	private Vector3 m_HitBottom;
	private Vector3 m_HitForward;
	public Transform obj;

	public float m_MinDistanceBottom;
	public float m_MinDistanceForward;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			print("BOT");
			Ray _Ray = new Ray(transform.position, Vector3.down);
			RaycastHit _Hit = new RaycastHit();

			if (Physics.Raycast(_Ray, out _Hit, m_MinDistanceBottom))
			{
				print("I hit the '" + _Hit.transform.parent.name + "'");
				m_HitBottom = _Hit.point;
				m_DistanceBottom = Vector3.Distance(transform.position, m_HitBottom);
			}
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			print("FOR");
			Ray _Ray = new Ray(transform.position, Vector3.forward);
			RaycastHit _Hit = new RaycastHit();

			if (Physics.Raycast(_Ray, out _Hit, m_MinDistanceForward))
			{
				print("I hit the '" + _Hit.transform.parent.name + "'");
				m_HitForward = _Hit.point;
				m_DistanceForward = Vector3.Distance(transform.position, m_HitForward);
			}
		}
	}

	void OnDrawGizmos()
	{
		// Bottom
		if (m_DistanceBottom != 0)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, m_HitBottom);
		}

		// Front
		if (m_DistanceForward != 0)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, m_HitForward);
		}
	}
}