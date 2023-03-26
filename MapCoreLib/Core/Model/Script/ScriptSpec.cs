using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core;
using MapCoreLib.Core.Asset;
using MapCoreLib.Util;
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

        static ScriptSpec()
        {
            initScriptSpec();
        }
        
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


        public static bool checkScriptAction(string commandWord, List<object> args)
        {
            return checkScriptInternal(actionsSpec, commandWord, args);
        }
        
        public static bool checkScriptCondition(string commandWord, List<object> args)
        {
            return checkScriptInternal(conditionsSpec, commandWord, args);
        }

        public static bool checkScriptInternal(Dictionary<string, ScriptModel> dict, string commandWord, List<object> args)
        {
            if (dict.ContainsKey(commandWord))
            {
                ScriptModel scriptModel = dict[commandWord];
                if (scriptModel.argumentModel.Count > 0 && args == null)
                {
                    throw new Exception($"ScriptCondition of | 命令字{commandWord} 需要参数，但未提供");
                }
                if (scriptModel.argumentModel.Count == 0 && args == null)
                {
                    return true;
                }
                
                if (args != null && scriptModel.argumentModel.Count != args.Count)
                {
                    throw new Exception($"ScriptCondition of | 命令字{commandWord} 提供的参数不足");
                }
                for (int i = 0; i < args.Count; i++)
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
        
        public static ScriptContent generateScriptContent(MapDataContext mapDataContext, Dictionary<string, ScriptModel> dict, string commandWord, List<object> args)
        {
            ScriptModel scriptModel = dict[commandWord];
            var scriptContent = new ScriptContent();
            scriptContent.contentType = scriptModel.editorNumber;
            scriptContent.nameIndex = mapDataContext.MapStruct.RegisterString(commandWord);
            scriptContent.assetPropertyType = AssetPropertyType.stringType;
            scriptContent.contentName = commandWord;
            if (args == null)
            {
                return scriptContent;
            }
            for (var i = 0; i < args.Count; i++)
            {
                var scriptArgument = new ScriptArgument();
                if (scriptModel.argumentModel[i].realType == INT)
                {
                    if (args[i].GetType().Name == "Boolean")
                    {
                        scriptArgument.intValue = (bool)args[i] ? 1 : 0;
                    }
                    else
                    {
                        scriptArgument.intValue = (int) Math.Floor(float.Parse(args[i].ToString()));
                    }
                } else if (scriptModel.argumentModel[i].realType == FLOAT)
                {
                    scriptArgument.floatValue = float.Parse(args[i].ToString());
                } else if (scriptModel.argumentModel[i].realType == STRING)
                {
                    scriptArgument.stringValue = (string) args[i];
                } else if (scriptModel.argumentModel[i].realType == POSITION)
                {
                    scriptArgument.position = (Vec3D) args[i];
                }

                scriptArgument.argumentType = scriptModel.argumentModel[i].typeNumber;
                scriptContent.arguments.Add(scriptArgument);
            }

            return scriptContent;
        }
    }
}