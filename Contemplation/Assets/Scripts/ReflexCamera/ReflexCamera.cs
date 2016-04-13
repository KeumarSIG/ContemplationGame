using UnityEngine;
using System.Collections;

public class ReflexCamera : MonoBehaviour 
{
	public GameObject m_CurrentFocus;
	private ObjectOfInterest m_CurrentFocusObjectOfInterest;
	public Shader m_FocusShader;

	void Update ()
	{
		// Focus an item
		if (Input.GetButtonDown("Mouse 2"))
		{
			ResetCurrentTarget();

			// Targeting a new object
			Ray _Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit _Hit;

			if (Physics.Raycast(_Ray, out _Hit, Mathf.Infinity))
			{
				m_CurrentFocus = _Hit.collider.gameObject;
				m_CurrentFocusObjectOfInterest = m_CurrentFocus.GetComponentInParent<ObjectOfInterest>();
				m_CurrentFocusObjectOfInterest.m_IsFocused = true;
				m_CurrentFocusObjectOfInterest.CheckCurrentState(m_FocusShader);
				print("PARENT: " + m_CurrentFocus.transform.parent.name);
			}
		}
	}

	// If the player already has a target, reset it so that we can target something new
	void ResetCurrentTarget()
	{
		if (m_CurrentFocus != null)
		{
			m_CurrentFocusObjectOfInterest.m_IsFocused = false;
			m_CurrentFocusObjectOfInterest.CheckCurrentState(null);

			m_CurrentFocus = null; 
			m_CurrentFocusObjectOfInterest = null;
		}
	}
}