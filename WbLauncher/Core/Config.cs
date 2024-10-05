using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WbLauncher.Core
{
    public static class Config
    {
        
        public static void init()
        {
            
        }
        
        public static bool isChinese()
        {
            return CultureInfo.CurrentCulture.Name.StartsWith("zh");
        }
        
        
        public static string FindGame()
        {
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\RA3ModLauncher");
                if (registryKey != null)
                {
                    string path = (string)registryKey.GetValue("preferred_game_path");
                    if (MyRegistry.IsGamePathValid(path))
                    {
                        return path;
                    }
                }

                RegistryKey localMachine = Registry.LocalMachine;
                registryKey =
                    localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\RA3.exe");
                if (registryKey != null)
                {
                    string path2 = (string)registryKey.GetValue("path");
                    if (MyRegistry.IsGamePathValid(path2))
                    {
                        return path2;
                    }
                }

                registryKey = localMachine.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3");
                if (registryKey != null)
                {
                    string path3 = (string)registryKey.GetValue("Install Dir");
                    if (MyRegistry.IsGamePathValid(path3))
                    {
                        return path3;
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
            
            return "";
        }

        public static string getCoronaRootPath()
        {
            var coronaRoot = Path.Combine(Directory.GetCurrentDirectory(), "data", "config", "corona.txt");
            if (!File.Exists(coronaRoot))
            {
                return null;
            }
            return File.ReadAllText(coronaRoot);
        }
    }

    public class LauncherStr
    {
        public class launcherDialog
        {
            public static string title;
            public static string selectLanguage;
            public static string selectMod;
            public static string launchButton;
            public static string selectGamePath;
            public static string selectGamePathError;
            public static string waitingInit;
        }
    }
}