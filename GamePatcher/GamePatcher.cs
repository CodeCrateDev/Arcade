using GamePatcher.Patches;
using GamePatcher.Utils;
using System.Reflection;

namespace GamePatcher
{
	public class GamePatcher
	{
		private const string ARG_VERSION = "-v";
		private const string ARG_HELP = "-h";
		private const string ARG_SPECIFIC = "-s";
		private const string ARG_MULTIPLE = "-m";

		private const string TARGET_FILE_NAME = "Assembly-CSharp.dll";

		public static void Main(string[] args)
		{
			if (args.Length <= 0)
			{
				Log.Error("Not enough arguments");
				return;
			}

			switch (args[0])
			{
				case ARG_VERSION:
					Log.Info($"GamePatcher Version {Assembly.GetExecutingAssembly().GetName().Version}");
					break;
				case ARG_HELP:
					Log.Info("Usage: GamePatcher [options]");
					Log.Info("Options:");
					Log.Info("  -v        Show version information");
					Log.Info("  -h        Show help information");
					Log.Info("  -s        Patch a specific game assembly");
					Log.Info("  -m        Patch multiple game assemblies (automatically searched for)");
					break;
				case ARG_SPECIFIC:
					PatchSpecific(args);
					break;
				case ARG_MULTIPLE:
					PatchMultiple(args);
					break;
				default:
					Log.Error("Unknown argument. Use -h for help.");
					break;
			}
		}

		private static void PatchSpecific(string[] args)
		{
			string dllPath = args.Length > 1 ? args[1] : "";

			if (string.IsNullOrEmpty(dllPath))
			{
				Log.Error("File must be specified and not null");
				return;
			}

			if (!File.Exists(dllPath))
			{
				Log.Error($"File not found: {dllPath}");
				return;
			}

			try
			{
				BackupAssembly(dllPath);

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

		private static void PatchMultiple(string[] args)
		{
			string specifiedDirectory = args.Length > 1 ? args[1] : "";

			if (string.IsNullOrEmpty(specifiedDirectory))
			{
				Log.Error("Directory must be specified and not null");
				return;
			}

			if (!Directory.Exists(specifiedDirectory))
			{
				Log.Error($"Directory not found: {specifiedDirectory}");
				return;
			}

			List<string> assemblies = FindFilesByName(specifiedDirectory, TARGET_FILE_NAME);
			if (assemblies.Count == 0)
			{
				Log.Warn("No assemblies found to patch.");
				return;
			}

			try
			{
				int patches = 0;

				foreach (string dllPath in assemblies)
				{
					// Backup file
					BackupAssembly(dllPath);

					DllPatcher patcher = new DllPatcher(dllPath);
					patcher.AddPatch(new LegacyLauncherCallPatch());

					patches += patcher.ApplyPatches();
				}

				Log.Success($"Finished. {patches} total patch(es) applied to {assemblies.Count} assembly(ies).");
			} 
			catch (Exception ex)
			{
				Log.Error($"An error occurred during patching: {ex.Message}");
				return;
			}
		}

		private static List<string> FindFilesByName(string folderPath, string targetFileName)
		{
			List<string> foundFiles = new List<string>();

			if (!Directory.Exists(folderPath))
			{
				Console.WriteLine("Folder does not exist.");
				return foundFiles;
			}

			// Recursively get all files
			foreach (string file in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
			{
				if (Path.GetFileName(file).Equals(targetFileName, StringComparison.OrdinalIgnoreCase))
				{
					foundFiles.Add(file);
				}
			}

			return foundFiles;
		}

		private static void BackupAssembly(string dllPath)
		{
			string backupPath = dllPath + ".bak";
			if (!File.Exists(backupPath))
			{
				File.Copy(dllPath, backupPath);
				Log.Info($"Backup created at {backupPath}");
			}
		}
	}
}