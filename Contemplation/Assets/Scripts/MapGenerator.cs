using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour 
{
	// How do we display the terrain ? 
	public enum DrawMode
	{
		NoiseMap,
		ColorMap,
		MeshMap
	};

	[Header("Other")]
	#region AutoUpdate
	public bool m_AutoUpdate;
	public DrawMode m_DrawMode;
	public int m_Seed;
	#endregion Auto-Update



	#region MapParameters
	[Header("Map parameters")]
	[HideInInspector]
	public const int m_MapChunkSize = 241;

	[Range(0f, 6f)] 
	public int m_LevelOfDetail;

	[Range(0.001f, 128f)] 	
	public float m_NoiseScale;

	[Range(1f, 10f)] 		
	public int m_Octaves;

	[Range(0f, 5f)] 		
	public float m_Persistance;

	[Range(1f, 10f)]		
	public float m_Lacunarity;

	public float m_MeshHeightMultiplier;
	public AnimationCurve m_MeshHeightCurve;
	public Vector2 m_Offset;
	public TerrainType[] m_Regions;
	#endregion MapParameters



	// Generate the noise map
	public void GenerateMap()
	{
		// Get the generated map
		float[,] _NoiseMap = Noise.GenerateNoiseMap(m_Seed, m_MapChunkSize, m_MapChunkSize, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity, m_Offset);

		Color[] _ColorMap = new Color[m_MapChunkSize * m_MapChunkSize];

		for (int y = 0; y < m_MapChunkSize ; y++) 
		{
			for (int x = 0; x < m_MapChunkSize ; x++) 
			{
				float _CurrentHeight = _NoiseMap[x, y];		

				for (int i = 0 ; i < m_Regions.Length ; i++)
				{
					if (_CurrentHeight <= m_Regions[i].m_Height)
					{
						_ColorMap[y * m_MapChunkSize + x] = m_Regions[i].m_Color;
						break;
					}
				}
			}
		}

		// Get MapDisplay to display map
		MapDisplay _RefMapDisplay = FindObjectOfType<MapDisplay>();

		// Noise map mode
		if (m_DrawMode == DrawMode.NoiseMap)
		{
			
			_RefMapDisplay.DrawTexure(TextureGenerator.TextureFromHeightMap(_NoiseMap));
		}

		// Color mode
		else if (m_DrawMode == DrawMode.ColorMap)
		{
			_RefMapDisplay.DrawTexure(TextureGenerator.TextureFromColorMap(_ColorMap, m_MapChunkSize, m_MapChunkSize));
		}

		else if (m_DrawMode == DrawMode.MeshMap)
		{
			_RefMapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(_NoiseMap, m_MeshHeightMultiplier, m_MeshHeightCurve, m_LevelOfDetail), TextureGenerator.TextureFromColorMap(_ColorMap, m_MapChunkSize, m_MapChunkSize));
		}
	}

	// When changing variables in the inspector, makes sure values are clamped
	// Pas opti, je pense...
	void OnValidate()
	{
		if (m_Lacunarity < 1)
		{
			m_Lacunarity = 1;
		}

		if (m_Octaves < 0)
		{
			m_Octaves = 0;
		}
	}

	// Show up in the inspector
	[System.Serializable]
	public struct TerrainType
	{
		public string m_Name;
		public float m_Height;
		public Color m_Color;
	}
}