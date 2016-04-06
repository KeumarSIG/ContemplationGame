using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
    public class SepiaTone : ImageEffectBase
	{
        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            Graphics.Blit (source, destination, material);
        }


		protected override void Start()
		{
			/*
			if (!SystemInfo.supportsImageEffects)
			{
				enabled = false;
				return;
			}

			// Disable the image effect if the shader can't
			// run on the users graphics card
			if (!shader || !shader.isSupported)
			{
				enabled = false;
			}
			*/
			print("Override");
		}

    }
}
