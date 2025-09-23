using ArcadeLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ArcadeLauncher.Utils
{
    public static class GameManifestLoader
    {
        private const string MANIFEST_FILE_NAME = "manifest.json";

        public static List<GameManifest> LoadAllGameManifests()
        {
            List<GameManifest> manifests = new List<GameManifest>();

            string topGamesDir = ExecutableHelper.GetRootPath() + "\\" + "Games";

            if (!Directory.Exists(topGamesDir))
            {
                Debug.LogWarning($"Games directory not found: {topGamesDir}");
                return manifests;
            }


            string[] gameDirs = Directory.GetDirectories(topGamesDir);

			foreach (string dir in gameDirs)
			{
                string manifestPath = dir + "\\" + MANIFEST_FILE_NAME;
                manifests.Add(LoadGameManifest(manifestPath));
			}

			return manifests;
        }

        public static GameManifest LoadGameManifest(string manifestPath)
        {
            if (File.Exists(manifestPath))
            {
                try
                {
                    string json = File.ReadAllText(manifestPath);
                    GameManifest manifest = JsonUtility.FromJson<GameManifest>(json);

                    // Tranforms / into \ to comply with the C++ stub, although it should also do that itself
                    manifest.gamePath = manifest.gamePath.Replace("/", "\\");

                    return manifest;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load {manifestPath}: {ex}");
                }
            }
            else
            {
                Debug.LogWarning($"No manifest.json could be found at {manifestPath}");
            }

            return new GameManifest();
        }
    }
}
