using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace ArcadeLauncher.Utils
{
    public static class ExecutableHelper
    {
        private const string STUB_EXECUTABLE_NAME = "launcherstub.exe";
        private const string LAUNCHER_EXECUTABLE_NAME = "ArcadeLauncher.exe";

        public static bool LaunchGame(string gamePath)
        {
            Process stub = Process.Start(GetStubPath(), CreateArguments(gamePath, GetLauncherPath()));

            // Check if the process was started
            if (stub == null)
            {
                return false;
            }

            Application.Quit();

            // This in theory will never be called
            return true;
        }

        public static bool DoesGameExist(string gamePath)
        {
            FileAttributes attrib = File.GetAttributes(gamePath);

            switch (attrib)
            {
                case FileAttributes.Directory:
                    return Directory.Exists(gamePath);
                default:
                    return File.Exists(gamePath);
            }
        }
        public static string GetRootPath()
        {
            return Directory.GetParent(Application.dataPath).FullName;
        }

        public static string GetGamePath(string gamePath)
        {
            return GetRootPath() + "\\" + gamePath;
        }

        public static string GetStubPath()
        {
            return GetRootPath() + "\\" + STUB_EXECUTABLE_NAME;
        }

        public static string GetLauncherPath()
        {
            return GetRootPath() + "\\" + LAUNCHER_EXECUTABLE_NAME;
        }

        private static string CreateArguments(string gamePath, string launcherPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            sb.Append(gamePath);
            sb.Append("\" \"");
            sb.Append(launcherPath);
            sb.Append("\"");

            return sb.ToString();
        }
    }
}
