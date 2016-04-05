using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class EditorMapGeneration : Editor 
{
	public override void OnInspectorGUI()
	{
		// Ref to MapGenerator
		MapGenerator _RefMapGenerator = (MapGenerator) target;

		// If inspector is rendered and auto-update == true, then auto-update
		if (DrawDefaultInspector())
		{
			if (_RefMapGenerator.m_AutoUpdate)
			{
				_RefMapGenerator.GenerateMap();
			}
		}

		// Generate map if "Generate" is clicked
		if (GUILayout.Button("Generate"))
		{
			_RefMapGenerator.GenerateMap();
		}
	}
}