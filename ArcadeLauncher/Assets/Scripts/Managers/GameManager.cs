using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ArcadeLauncher.Models;
using ArcadeLauncher.Utils;

namespace ArcadeLauncher.Managers
{
	// This name is confusing, it manages ALL the games, not A game
	public class GameManager : Singleton<GameManager>
	{
		private List<GameManifest> manifests = new List<GameManifest>();
		private List<Sprite> images = new List<Sprite>();

		public Action FinishedLoadingGames;

		[SerializeField] private TextMeshProUGUI path;

		public override void Awake()
		{
			base.Awake();

			manifests = GameManifestLoader.LoadAllGameManifests();

			// Load all images with the same ID as manifests
			for (int i = 0; i < manifests.Count; i++)
			{
				string path = ExecutableHelper.GetRootPath() + "\\" + manifests[i].imagePath;

				images.Insert(i, ImageLoader.LoadPNGSprite(path));
			}

			FinishedLoadingGames?.Invoke();
		}

		private void Start()
		{
			path.text = "CORE PATHS";
			path.text += "\nStub: " + ExecutableHelper.GetStubPath();
			path.text += "\nGames: " + ExecutableHelper.GetRootPath() + "\\" + "Games";

			path.text += "\n\nGAMES";
			string[] gameDirs = System.IO.Directory.GetDirectories(ExecutableHelper.GetRootPath() + "\\" + "Games");
			foreach (string dir in gameDirs)
			{
				string manifestPath = dir + "\\" +"manifest.json";
				path.text += "\n" + manifestPath;
			}

			path.text += "\n\nGAME MANIFESTS";
			foreach (GameManifest manifest in manifests)
			{
				path.text += "\n" + manifest.gameName + ": " + ExecutableHelper.GetGamePath(manifest.gamePath);
			}
		}

		public int GetGameCount()
		{
			return manifests.Count;
		}

		public GameManifest GetGameManifest(int index)
		{
			return manifests[index];
		}

		public Sprite GetGameImage(int index)
		{
			return images[index];
		}
	}
}
