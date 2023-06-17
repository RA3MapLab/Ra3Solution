using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Util
{
    public static class ScriptHelper
    {
        public static List<OrCondition> ConditionTrue(MapDataContext context)
        {
            return new List<OrCondition>()
            {
                new OrCondition()
                {
                    conditions = new List<ScriptCondition>()
                    {
                        ScriptCondition.of(context, "CONDITION_TRUE")
                    }
                }
            };
        }

        public static void addScript(this MapDataContext context, string path, Script script)
        {
            Script.addTo(context, script, path);
        }
        
        public static void addScripts(this MapDataContext context, string path, List<Script> scripts)
        {
            scripts.ForEach( script => Script.addTo(context, script, path));
        }

        public static void enableScriptOneByOne(this MapDataContext context, List<Script> scripts)
        {
            for (var i = 1; i < scripts.Count; i++)
            {
                scripts[i].isActive = false;
            }

            for (var i = 0; i < scripts.Count - 1; i++)
            {
                var script = scripts[i];
                script.ScriptActionOnTrue.Add(
                    ScriptAction.of(context, "ENABLE_SCRIPT", scripts[i + 1].Name)
                    );
            }
            
            for (var i = 1; i < scripts.Count; i++)
            {
                var script = scripts[i];
                script.ScriptActionOnTrue.Insert(0, 
                    ScriptAction.of(context, "DISABLE_SCRIPT", scripts[i - 1].Name)
                );
            }
            
        }
    }
}