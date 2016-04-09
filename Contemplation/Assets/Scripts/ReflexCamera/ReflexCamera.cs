using UnityEngine;
using System.Collections;

public class ReflexCamera : MonoBehaviour 
{
	public GameObject m_CurrentFocus;
	public Shader m_FocusShader;

	void Update ()
	{
		if (Input.GetButtonDown("Mouse 2"))
		{
			ResetCurrentTarget();

			Ray _Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit _Hit;

			if (Physics.Raycast(_Ray, out _Hit, Mathf.Infinity))
			{
				m_CurrentFocus = _Hit.collider.gameObject;
				ObjectOfInterest _CurrentFocus = m_CurrentFocus.GetComponentInParent<ObjectOfInterest>();

				_CurrentFocus.m_IsFocused = true;
				_CurrentFocus.CheckCurrentState(m_FocusShader);
			}
		}
	}

	void ResetCurrentTarget()
	{
		if (m_CurrentFocus != null)
		{
			m_CurrentFocus = null; 

			ObjectOfInterest _CurrentFocus = m_CurrentFocus.GetComponentInParent<ObjectOfInterest>();

			_CurrentFocus.m_IsFocused = false;
			_CurrentFocus.CheckCurrentState(null);
		}
	}
}