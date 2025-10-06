using System;
using System.IO;

namespace GamePatcher
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Info("=== Game Patcher ===");

			if (args.Length == 0)
			{
				Log.Warn("Usage: GamePatcher <path to Assembly-CSharp.dll>");
				return;
			}

			string dllPath = args[0];

			if (!File.Exists(dllPath))
			{
				Log.Error($"File not found: {dllPath}");
				return;
			}

			try
			{
				// Backup file
				string backupPath = dllPath + ".bak";
				if (!File.Exists(backupPath))
				{
					File.Copy(dllPath, backupPath);
					Log.Info($"Backup created at {backupPath}");
				}

				DllPatcher patcher = new DllPatcher(dllPath);
				patcher.AddPatch(new LegacyLauncherCallPatch());

				int patches = patcher.ApplyPatches();
				Log.Success($"Finished. {patches} patch(es) applied.");
			}
			catch (Exception ex)
			{
				Log.Error("An error occured during the patching process: " + ex.Message);
			}
		}
	}
}
