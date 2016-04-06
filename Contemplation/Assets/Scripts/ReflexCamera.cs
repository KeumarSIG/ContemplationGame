using UnityEngine;
using System.Collections;

public class ReflexCamera : MonoBehaviour 
{
	private GameObject m_CurrentFocus;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Mouse 1"))
		{
			RaycastHit _Raycast;

			Ray _Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(_Ray))
			{
				print("Grosse");
			}
		}
	}
}