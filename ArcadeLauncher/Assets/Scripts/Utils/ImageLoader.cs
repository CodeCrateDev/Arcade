using System;
using System.IO;
using UnityEngine;

namespace ArcadeLauncher.Utils
{
	public static class ImageLoader
	{
		public static Texture2D LoadPNG(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Debug.LogError($"Couldn't find PNG file at {filePath}");
				return null;
			}

			try
			{
				byte[] fileData = File.ReadAllBytes(filePath);
				Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

				if (texture.LoadImage(fileData))
				{
					return texture;
				}
				else
				{
					Debug.LogError("Failed to load PNG data.");
					return null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Exception loading PNG: {ex}");
				return null;
			}
		}

		public static Sprite LoadPNGSprite(string filePath, float pixelsPerUnit = 100f)
		{
			Texture2D texture = LoadPNG(filePath);
			
			// Check the validity of the image
			if (texture == null) { return null; }

			return Sprite.Create(
				texture,
				new Rect(0, 0, texture.width, texture.height),
				new Vector2(0.5f, 0.5f),
				pixelsPerUnit
			);
		}
	}
}
