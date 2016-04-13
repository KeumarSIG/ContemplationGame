using UnityEngine;
using System.Collections;

public class RaycastTest : MonoBehaviour 
{
	private float m_DistanceForward;
	private float m_DistanceBottom;
	private Vector3 m_HitBottom;
	private Vector3 m_HitForward;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			print("RayBOTTOM");
			Ray _Ray = new Ray (this.transform.position, Vector3.down);
			RaycastHit _Hit = new RaycastHit();

			if (Physics.Raycast(_Ray, out _Hit, Mathf.Infinity))
			{
				m_HitBottom = _Hit.transform.position;
				m_DistanceBottom = Vector3.Distance(transform.position, m_HitBottom);
				print("Bot.Dist: " + m_DistanceBottom);
			}
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			print("RayFORWARD");
			Ray _Ray = new Ray (this.transform.position, Vector3.up);
			RaycastHit _Hit = new RaycastHit();

			if (Physics.Raycast(_Ray, out _Hit, Mathf.Infinity))
			{
				m_HitForward = _Hit.transform.position;
				m_DistanceForward = Vector3.Distance(transform.position, m_HitForward);
				print("For.Dist: " + m_DistanceForward);
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