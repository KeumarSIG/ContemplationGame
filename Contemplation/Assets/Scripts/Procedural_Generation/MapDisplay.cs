using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour 
{
	[HideInInspector] public Renderer m_TextureRenderer;
	public MeshFilter m_MeshFilter;
	public MeshRenderer m_MeshRenderer;

	public void DrawTexure(Texture2D Texture)
	{
		m_TextureRenderer.sharedMaterial.mainTexture = Texture;
		m_TextureRenderer.transform.localScale = new Vector3(Texture.width, 1, Texture.height);
	}

	public void DrawMesh(MeshData MeshDataComponent, Texture2D TextureComponent)
	{
		m_MeshFilter.sharedMesh = MeshDataComponent.CreateMesh();
		m_MeshRenderer.sharedMaterial.mainTexture = TextureComponent;
	}
}