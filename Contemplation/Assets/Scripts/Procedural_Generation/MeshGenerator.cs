using UnityEngine;
using System.Collections;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] HeightMap, float HeightMultiplier, AnimationCurve MeshHeightCurve, int LevelOfDetail)
	{
		int _Width = HeightMap.GetLength(0);
		int _Height = HeightMap.GetLength(1);

		float _TopLeftX = (_Width - 1) / -2f;
		float _TopLeftZ = (_Height - 1) / 2f;

		int _MeshSimplificationIncrement = (LevelOfDetail == 0) ? 1 : LevelOfDetail * 2;
		int _VerticesPerLine = ((_Width - 1) / _MeshSimplificationIncrement) + 1;

		MeshData _MeshData = new MeshData(_VerticesPerLine, _VerticesPerLine);
		int _VertexIndex = 0;

		for (int y = 0 ; y < _Height ; y += _MeshSimplificationIncrement)
		{
			for (int x = 0 ; x < _Width ; x += _MeshSimplificationIncrement)
			{
				_MeshData.m_Vertices[_VertexIndex] = new Vector3(_TopLeftX + x, MeshHeightCurve.Evaluate(HeightMap[x, y]) * HeightMultiplier, _TopLeftZ - y);
				_MeshData.m_Uvs[_VertexIndex] = new Vector2(x/(float)_Width, y/(float)_Height);

				if (x < (_Width-1) && y < (_Height -1))
				{
					_MeshData.AddTriangles(_VertexIndex, _VertexIndex + _VerticesPerLine + 1, _VertexIndex + _VerticesPerLine);
					_MeshData.AddTriangles(_VertexIndex + _VerticesPerLine + 1, _VertexIndex, _VertexIndex + 1);
				}

				_VertexIndex++;
			}
		}

		return _MeshData;
	}
}

public class MeshData
{
	public Vector3[] m_Vertices;
	public int[] m_Triangles;
	public Vector2[] m_Uvs;

	private int m_TriangleIndex;

	public MeshData(int MeshWidth, int MeshHeight)
	{
		m_Vertices = new Vector3[MeshWidth * MeshHeight];
		m_Uvs = new Vector2[MeshWidth * MeshHeight];
		m_Triangles = new int[(MeshWidth-1) * (MeshHeight-1) * 6];
	}

	public void AddTriangles(int a, int b, int c)
	{
		m_Triangles[m_TriangleIndex] = a;
		m_Triangles[m_TriangleIndex+1] = b;
		m_Triangles[m_TriangleIndex+2] = c;

		m_TriangleIndex += 3;
	}

	public Mesh CreateMesh()
	{
		Mesh _Mesh = new Mesh();
		_Mesh.vertices = m_Vertices;
		_Mesh.triangles = m_Triangles;
		_Mesh.uv = m_Uvs;
		_Mesh.RecalculateNormals();

		return _Mesh;
	}
}