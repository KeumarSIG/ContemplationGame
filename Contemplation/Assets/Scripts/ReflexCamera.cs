using UnityEngine;
using System.Collections;

public class ReflexCamera : MonoBehaviour 
{
	public GameObject m_CurrentFocus;
	public Shader m_CurrentFocusShader;

	void Update ()
	{
		if (Input.GetButtonDown("Mouse 1"))
		{
			Ray _Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit _Hit;

			if (Physics.Raycast(_Ray, out _Hit, Mathf.Infinity))
			{
				m_CurrentFocus = _Hit.collider.gameObject;
				m_CurrentFocus.GetComponentInChildren<MeshRenderer>().material.shader = m_CurrentFocusShader;
			}
		}
	}
}