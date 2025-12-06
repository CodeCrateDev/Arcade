using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class StubBuildValidator
{
	static StubBuildValidator()
	{
		BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayer);
	}

	private static void BuildPlayer(BuildPlayerOptions options)
	{
		string exePath = ArcadeStubSettingsWindow.GetExternalExePath();

		if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
		{
			EditorUtility.DisplayDialog(
				"Build Blocked",
				"Cannot build: External stub EXE path is not set or the file does not exist.\n\n" +
				"Please configure it in Tools > Stub Settings.",
				"OK"
			);

			return;
		}

		BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
	}
}