using UnityEngine;
using UnityEditor;
using System.IO;

public class ArcadeStubSettingsWindow : EditorWindow
{
	private const string EXE_PATH_KEY = "ArcadeStubPath";
	private static bool checkedStartup = false;

	[InitializeOnLoadMethod]
	private static void AutoPopupIfNeeded()
	{
		if (checkedStartup) { return; }
		checkedStartup = true;

		string path = EditorPrefs.GetString(EXE_PATH_KEY, "");

		// Delay so Unity can finish loading
		EditorApplication.delayCall += () =>
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				ShowWindow();
			}
		};
	}

	[MenuItem("Tools/Stub Settings")]
	public static void ShowWindow()
	{
		ArcadeStubSettingsWindow instance = GetWindow<ArcadeStubSettingsWindow>("Stub Settings");
		instance.minSize = new Vector2(450, 260);
		instance.maxSize = new Vector2(450, 260);
		instance.Repaint();
	}

	private void OnGUI()
	{
		GUILayout.Space(10);
		GUILayout.Label("External EXE Configuration", EditorStyles.boldLabel);
		GUILayout.Space(5);

		EditorGUILayout.HelpBox("Specify the location of the external EXE file.\nIt will be copied into the build folder automatically.", MessageType.Info);

		GUILayout.Space(10);

		string path = EditorPrefs.GetString(EXE_PATH_KEY, "");

		EditorGUILayout.BeginVertical("box");
		{
			GUILayout.Label("Selected EXE Path:", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.SelectableLabel(path, GUILayout.Height(18));

			if (GUILayout.Button("Browse...", GUILayout.Width(100)))
			{
				string selected = EditorUtility.OpenFilePanel("Select External EXE", "", "exe");
				if (!string.IsNullOrEmpty(selected))
				{
					EditorPrefs.SetString(EXE_PATH_KEY, selected);
					Repaint();
				}
			}
			EditorGUILayout.EndHorizontal();

			if (string.IsNullOrEmpty(path))
			{
				EditorGUILayout.HelpBox("No EXE path selected.", MessageType.Warning);
			}
			else if (!File.Exists(path))
			{
				EditorGUILayout.HelpBox("File does not exist at this location!", MessageType.Error);
			}
			else
			{
				EditorGUILayout.HelpBox("EXE path is valid!\nIt is recommended to regenerate variables.", MessageType.Info);
			}
		}
		EditorGUILayout.EndVertical();

		GUILayout.Space(25);

		// Manual refresh button
		if (GUILayout.Button("Refresh Variables", GUILayout.Height(30)))
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				string exeName = Path.GetFileName(path);
				ExternalStubVariableGenerator.UpdateExeName(exeName);
				EditorUtility.DisplayDialog("Success", "EXE filename refreshed!", "OK");
			}
			else
			{
				EditorUtility.DisplayDialog("Error", "Invalid EXE path.", "OK");
			}
		}

		if (GUILayout.Button("Clear Path", GUILayout.Height(30)))
		{
			EditorPrefs.DeleteKey(EXE_PATH_KEY);
			Repaint();
		}
	}

	public static string GetExternalExePath()
	{
		return EditorPrefs.GetString(EXE_PATH_KEY, "");
	}
}
