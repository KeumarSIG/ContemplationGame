using UnityEngine;
using System.Collections;

public static class TextureGenerator
{
	public static Texture2D TextureFromColorMap(Color[] ColorMap, int Width, int Height)
	{
		Texture2D _Texture = new Texture2D(Width, Height);
		_Texture.filterMode = FilterMode.Point;
		_Texture.wrapMode = TextureWrapMode.Clamp;
		_Texture.SetPixels(ColorMap);
		_Texture.Apply();

		return _Texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] HeightMap)
	{
		// Get lenght of width noise map
		int _Width = HeightMap.GetLength(0); 
		// Get lenght of height noise map
		int _Height = HeightMap.GetLength(1);

		// Set the size of the texture to the size of the map
		//Texture2D _Texture = new Texture2D(_Width, _Height);

		// Set the color of the map
		Color[] _ColorMap = new Color[_Width * _Height];

		for (int y = 0 ; y < _Height ; y++)
		{
			for (int x = 0 ; x < _Width ; x++)
			{
				_ColorMap[y * _Width + x] = Color.Lerp(Color.black, Color.white, HeightMap[x, y]);
			}
		}

		return TextureFromColorMap(_ColorMap, _Width, _Height);
	}
}