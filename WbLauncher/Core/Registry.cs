using System.IO;
using Microsoft.Win32;

namespace WbLauncher.Core
{
    internal static class Registry
    {
         public static string GetGamePath()
        {
            var view32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3");
            return ra3?.GetValue("Install Dir") as string ?? string.Empty;
        }
         
         public static string GetGameDataPath()
         {
             var gamePath = GetGamePath();
             if (string.IsNullOrEmpty(gamePath))
             {
                 return "";
             }

             return Path.Combine(gamePath, "Data");
         }

        public static string GetKey()
        {
            var view32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3\\ergc");
            return ra3?.GetValue(null) as string ?? string.Empty;
        }

        public static bool IsGamePathValid(string gamePath)
        {
            if (gamePath == null)
            {
                return false;
            }
            try
            {
                return File.Exists(Path.Combine(gamePath, "RA3.exe"));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsGameRegistryPathValid()
        {
            var view32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3", true);
            if (ra3 == null)
            {
                return false;
            }
            var path = ra3.GetValue("Install Dir") as string;
            return IsGamePathValid(path);
        }

        public static bool IsRegistryValid()
        {
            if (!IsGameRegistryPathValid())
            {
                return false;
            }
            if (string.IsNullOrEmpty(GetKey()))
            {
                return false;
            }
            var view32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3", true);
            if (ra3 == null)
            {
                return false;
            }
            return true;
        }

        // private static bool checkPath(string ra3Root, string currentDirectory)
        // {
        //     if (ContainChinese(ra3root))
        //     {
        //         MessageBox.Show("游戏路径包含中文字符，请修改！");
        //         return false;
        //     }
        //     if (ContainChinese(currentDirectory))
        //     {
        //         MessageBox.Show("新地编客户端路径包含中文字符，请修改！");
        //         return false;
        //     }
        //     return true;
        // }
    }
}