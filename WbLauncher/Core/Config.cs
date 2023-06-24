using System;
using System.Globalization;
using System.IO;

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