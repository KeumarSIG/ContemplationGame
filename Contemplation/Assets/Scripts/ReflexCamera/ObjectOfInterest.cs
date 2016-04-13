using UnityEngine;
using System.Collections;

public class ObjectOfInterest : MonoBehaviour
{
	[HideInInspector] public bool m_IsFocused;
	private Shader m_OriginalShader;
	private Renderer m_Renderer;

	// Sets the object to "not currently being focused" && get the renderer && get original shader
	void Awake()
	{
		m_IsFocused = false;
		m_Renderer = GetComponentInChildren<Renderer>();
		m_OriginalShader = m_Renderer.material.shader;
	}

	public void CheckCurrentState(Shader FocusShader)
	{
		if (m_IsFocused == true)
		{
			m_Renderer.material.shader = FocusShader;
		}

		else 
		{
			m_Renderer.material.shader = m_OriginalShader;
		}
	}
}