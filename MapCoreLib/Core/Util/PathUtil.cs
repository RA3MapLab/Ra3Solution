using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MapCoreLib.Core.Util
{
    public class PathUtil
    {
        public static string RA3MapFolder = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Red Alert 3", "Maps");
        public static string WorkingDir = Directory.GetCurrentDirectory();
        public static string configDir => Path.Combine(WorkingDir, "data", "config");

        public static string defaultMapPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + ".map");
        }
        public static string defaultScriptXmlPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + ".edit.xml");
        }
        
        public static string defaulTempMiniMapPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + "_temp.png");
        }
        
        public static string defaulMiniMapPath(string mapName)
        {
            return Path.Combine(RA3MapFolder, mapName, mapName + "_art.tga");
        }
        
        public static List<string> defaultScriptCsSources(string scriptName)
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var scriptCsDir = Path.Combine(WorkingDir, "data", "scripts", scriptName, "Source");
            return Directory.EnumerateFiles(scriptCsDir, "*.cs", SearchOption.AllDirectories).ToList();
        }

        public static string defaultScriptCsDir()
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(WorkingDir, "data", "scripts");
        }

        public static string defaultNameScriptCsir(string scriptName)
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(WorkingDir, "data", "scripts", scriptName);
        }
    }
}