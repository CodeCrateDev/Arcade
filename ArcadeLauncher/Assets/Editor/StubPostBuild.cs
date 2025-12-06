using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class StubPostBuild : MonoBehaviour
{
	[PostProcessBuild]
	public static void CopyExternalExe(BuildTarget target, string buildPath)
	{
		string sourceExe = StubSettingsWindow.GetExternalExePath();

		if (string.IsNullOrEmpty(sourceExe) || !File.Exists(sourceExe))
		{
			Debug.LogWarning("External EXE not found or not set. Skipping copy.");
			return;
		}

		string buildDir = Path.GetDirectoryName(buildPath);
		string fileName = Path.GetFileName(sourceExe);
		string destination = Path.Combine(buildDir, fileName);

		File.Copy(sourceExe, destination, true);
		Debug.Log($"External EXE copied to build: {destination}");

		// Update variable file
		StubInfoGenerator.UpdateExeName(fileName);
	}
}
