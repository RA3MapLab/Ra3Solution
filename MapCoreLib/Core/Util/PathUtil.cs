using System;
using System.IO;
using System.Reflection;

namespace MapCoreLib.Core.Util
{
    public class PathUtil
    {
        public static string RA3MapFolder = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Red Alert 3", "Maps");

        public static string defaultMapPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + ".map");
        }
        public static string defaultScriptXmlPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + ".edit.xml");
        }
        
        public static string defaultScriptCsPath(string scriptName)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(assemblyFolder, "Data", "Scripts", scriptName, "Main.cs");
        }
        
        public static string defaultScriptCsDir()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(assemblyFolder, "Data", "Scripts");
        }

    }
}