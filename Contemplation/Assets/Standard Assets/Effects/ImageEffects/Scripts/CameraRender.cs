using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/CameraRender")]
	public class CameraRender : ImageEffectBase
	{
		// Called by camera to apply image effect
		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit (source, destination, police);
		}
	}
}