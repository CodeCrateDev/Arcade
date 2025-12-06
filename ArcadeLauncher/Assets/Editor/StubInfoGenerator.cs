using UnityEditor;
using UnityEngine;
using System.IO;

public class StubInfoGenerator
{
	private const string OutputPath = "Assets/StubInfo.cs";

	public static void UpdateExeName(string exeName)
	{
		string content =
		$@"// AUTO-GENERATED
// DO NOT EDIT
public static class ExternalExeInfo 
{{
    public const string ExeFileName = ""{exeName}"";
}}";

		File.WriteAllText(OutputPath, content);
		AssetDatabase.Refresh();
		Debug.Log("Updated StubInfo.cs with EXE name: " + exeName);
	}
}
