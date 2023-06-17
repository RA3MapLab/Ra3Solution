using System.Collections.Generic;
using MapCoreLib.Core.Asset;
using Newtonsoft.Json;

namespace MapCoreLib.Core.Scripts
{
    public class ScriptImportExport
    {
        public static string getScriptStructure(MapDataContext context)
        {
            var playerScriptsList = context.getAsset<PlayerScriptsList>(Ra3MapConst.ASSET_PlayerScriptsList);
            var sidesList = context.getAsset<SidesList>(Ra3MapConst.ASSET_SidesList);

            var scriptDirs = new List<ScriptDir>();
            var paths = new HashSet<string>();
            for (var i = 0; i < playerScriptsList.scriptLists.Count; i++)
            {
                var scriptDir = new ScriptDir()
                {
                    name = sidesList.players[i].assetPropertyCollection.getProperty("playerName").data as string
                };
                scriptDirs.Add(scriptDir);
                var scriptList = playerScriptsList.scriptLists[i];
                foreach (var scriptGroup in scriptList.scriptGroups)
                {
                    doAddScriptDir4Group(scriptDir, scriptGroup, paths);
                }
                foreach (var script in scriptList.scripts)
                {
                    doAddScriptDir4Script(scriptDir, script, paths);
                }
            }

            return JsonConvert.SerializeObject(new ScriptStructure()
            {
                scriptDirs = scriptDirs,
                paths = paths
            });
        }

        private static void doAddScriptDir4Script(ScriptDir scriptDir, Script script, HashSet<string> paths)
        {
            var path = script.Name;
            var curScriptDir = scriptDir;
            while (curScriptDir != null)
            {
                path = scriptDir.name + "/" + path;
            }

            paths.Add(path);
            scriptDir.scripts.Add(script.Name);
        }

        private static void doAddScriptDir4Group(ScriptDir scriptDir, ScriptGroup scriptGroup, HashSet<string> paths)
        {
            foreach (var scriptGroup2 in scriptGroup.scriptGroups)
            {
                var scriptDir2 = new ScriptDir()
                {
                    parent = scriptDir,
                    name = scriptGroup2.Name
                };
                scriptDir.subdirs.Add(scriptDir2);
                
                doAddScriptDir4Group(scriptDir2, scriptGroup2, paths);
            }
            foreach (var script in scriptGroup.scripts)
            {
                doAddScriptDir4Script(scriptDir, script, paths);
            }
        }
    }

    public class ScriptDir
    {
        [JsonIgnore]
        public ScriptDir parent = null;
        public string name;
        public List<ScriptDir> subdirs = new List<ScriptDir>();
        public List<string> scripts = new List<string>();
    }
    
    public class ScriptStructure
    {
        public List<ScriptDir> scriptDirs = new List<ScriptDir>();
        public HashSet<string> paths = new HashSet<string>();
    }
}