using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace RMGlib.Core.Utility
{
    public static class ScriptSpec
    {
        public static string INT = "Int32";
        public static string FLOAT = "Double";
        public static string STRING = "String";
        public static string POSITION = "Vec3D";
        public static List<string> compatibleTypes = new List<string> {INT, FLOAT};
        public static Dictionary<string, ScriptModel> actionsSpec = new Dictionary<string, ScriptModel>();
        
        public static Dictionary<string, ScriptModel> conditionsSpec = new Dictionary<string, ScriptModel>();
        
        public static void initScriptSpec()
        {
            if (actionsSpec.Count == 0)
            {
                actionsSpec = JsonConvert.DeserializeObject<List<ScriptModel>>(File.ReadAllText("ScriptActionNew.json"))
                    .ToDictionary(item => item.commandWord, item => item);
            }
            
            if (conditionsSpec.Count == 0)
            {
                conditionsSpec = JsonConvert.DeserializeObject<List<ScriptModel>>(File.ReadAllText("ScriptConditonNew.json"))
                    .ToDictionary(item => item.commandWord, item => item);
            }
        }


        public static bool checkScriptAction(string commandWord, params object[] args)
        {
            return checkScriptInternal(actionsSpec, commandWord, args);
        }
        
        public static bool checkScriptCondition(string commandWord, params object[] args)
        {
            return checkScriptInternal(conditionsSpec, commandWord, args);
        }

        public static bool checkScriptInternal(Dictionary<string, ScriptModel> dict, string commandWord, params object[] args)
        {
            if (dict.ContainsKey(commandWord))
            {
                ScriptModel scriptModel = dict[commandWord];
                for (int i = 0; i < args.Length; i++)
                {
                    var argi = args[i];
                    if (args[i].GetType().Name == "Boolean")
                    {
                        argi = (bool)args[i] ? 1 : 0;
                    }
                    if (argi.GetType().Name != scriptModel.argumentModel[i].realType)
                    {
                        if (compatibleTypes.Contains(argi.GetType().Name) && compatibleTypes.Contains(scriptModel.argumentModel[i].realType))
                        {
                            continue;
                        }
                        else
                        {
                            throw new Exception($"{commandWord} | wrong argument type {argi.GetType().Name}, should be {scriptModel.argumentModel[i].realType}");
                        }
                    }
                }

                return true;
            }

            throw new Exception($"unknown commandWord {commandWord}");
        }
    }
}