using UnityEngine;
using System.Collections;

public static class Noise 
{
	// Generates a noise map ; Returns an array[a, b] of float values from 0 to 1
	public static float[,] GenerateNoiseMap(int Seed, int MapWidth, int MapHeight, float Scale, int Octaves, float Persistance, float Lacunarity, Vector2 Offset) // Width of the map ; Height of the map ; 
	{
		// The actual noise map
		float[,] _NoiseMap = new float[MapWidth, MapHeight]; 

		System.Random _Prng = new System.Random(Seed);
		Vector2[] _OctavesOffset = new Vector2[Octaves];

		for (int i = 0; i < Octaves; i++) 
		{
			float _OffsetX = _Prng.Next(-100000, 100000) + Offset.x;	
			float _OffsetY = _Prng.Next(-100000, 100000) + Offset.y;
			_OctavesOffset[i] = new Vector2(_OffsetX, _OffsetY);
		}

		// Makes sure _Scale is not < or == to 0 (avoid errors)
		if (Scale <= 0) 
		{
			Scale = 0.0001f;
		}

		float _MinNoiseHeight = float.MaxValue;
		float _MaxNoiseHeight = float.MinValue;

		float _HalfMapWidth = MapWidth * 0.5f;
		float _HalfMapHeight = MapHeight * 0.5f;

		for (int y = 0 ; y < MapHeight ; y++)
		{
			for (int x = 0 ; x < MapWidth ; x++)
			{
				float _Amplitude = 1;
				float _Frequency = 1;
				float _NoiseHeight = 0;

				// Octave processing
				for (int i = 0 ; i < Octaves ; i++)
				{
					float _SampleX = (x - _HalfMapWidth) / Scale * _Frequency + _OctavesOffset[i].x; // Height value change faster
					float _SampleY = (y - _HalfMapHeight) / Scale * _Frequency + _OctavesOffset[i].y;

					float _PerlinValue = ((Mathf.PerlinNoise(_SampleX, _SampleY) * 2) - 1); // Will return random floats from -1 to 1
					_NoiseHeight += _PerlinValue * _Amplitude;

					_Amplitude *= Persistance; // Range 0 to 1 so decreases octave
					_Frequency *= Lacunarity; // Increases each octave
				}

				// Check NoiseHeights
				if (_NoiseHeight > _MaxNoiseHeight)
				{
					_MaxNoiseHeight = _NoiseHeight;
				}

				else if (_NoiseHeight < _MinNoiseHeight)
				{
					_MinNoiseHeight = _NoiseHeight;
				}

				_NoiseMap[x, y] = _NoiseHeight;
			}
		}
			
		for (int y = 0 ; y < MapHeight ; y++)
		{
			for (int x = 0 ; x < MapWidth ; x++)
			{
				_NoiseMap[x, y] = Mathf.InverseLerp(_MinNoiseHeight, _MaxNoiseHeight, _NoiseMap[x, y]);
			}
		}
			
		return _NoiseMap;
	}
}