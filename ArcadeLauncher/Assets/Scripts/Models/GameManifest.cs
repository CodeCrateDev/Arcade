using System;
using UnityEngine;

namespace ArcadeLauncher.Models
{
	[Serializable]
	public struct GameManifest
	{
		public int manifestVersion;
		public string gamePath;
		public string gameName;
		public string gameVersion;
		public string author;
		public string imagePath;
	}
}
