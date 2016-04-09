/*

using UnityEngine;
using System.Collections;

public class InfiniteTerrain : MonoBehaviour 
{
	public const float m_MaxViewDistance = 300;
	public Transform m_Viewer;

	public static Vector2 m_ViewerPosition;
	private int m_ChunkSize;
	private int m_ChunkVisibleDistance;

	void Start()
	{
		m_ChunkSize = MapGenerator.m_MapChunkSize - 1;
		m_ChunkVisibleDistance = Mathf.RoundToInt(m_MaxViewDistance / m_ChunkSize);
	}

	void UpdateVisibleChunks()
	{
		int _CurrentChunkCoordX = Mathf.RoundToInt(m_ViewerPosition.x / m_ChunkSize);
		int _CurrentChunkCoordY = Mathf.RoundToInt(m_ViewerPosition.y / m_ChunkSize);
	}
}

*/