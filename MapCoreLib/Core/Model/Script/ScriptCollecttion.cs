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
    public static class ScriptCollection
    {
        public static Dictionary<string, ScriptModel> actions = new Dictionary<string, ScriptModel>();
        
        public static Dictionary<string, ScriptModel> conditions = new Dictionary<string, ScriptModel>();

        public static bool collectScript = true;

        public static void collect()
        {
            var ra3Map = new Ra3Map("./allScript/allScript.map");
            ra3Map.parse();
            PlayerScriptsList playerScriptsList = ra3Map.getAsset<PlayerScriptsList>(Ra3MapConst.ASSET_PlayerScriptsList);
            foreach (var scriptList in playerScriptsList.scriptLists)
            {
                foreach (var scriptGroup in scriptList.scriptGroups)
                {
                    traverseGroup(scriptGroup);
                }

                foreach (var script in scriptList.scripts)
                {
                    traverseScript(script);
                }
            }

            var a = 2;
            collectDoc();
            // mergeTranslation();
            mergeTranslation2();

            File.WriteAllText("ScriptConditonNew.json", JsonConvert.SerializeObject(conditions.Values));
            File.WriteAllText("ScriptActionNew.json", JsonConvert.SerializeObject(actions.Values));
        }
        
        private static void collectDoc()
        {
            var scripDocs = File.ReadAllText("RA3WBScriptIDListStripped.txt").Split(new string[] { "\r\n\r\n" }, 
                StringSplitOptions.RemoveEmptyEntries);

            var docMap = new Dictionary<string, DocItem>();
            foreach (var scripDoc in scripDocs)
            {
                var tokens = scripDoc.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                docMap.Add(tokens[0].Substring("!ACTION:".Length, tokens[0].Length- "!ACTION:".Length).Trim(),
                    new DocItem()
                    {
                        scriptName = tokens[1].Substring("!NAME:".Length, tokens[1].Length- "!NAME:".Length),
                        scriptDesc = tokens[2].Substring("!DESC:".Length, tokens[2].Length- "!DESC:".Length),
                        scriptArg = tokens[3].Substring("!ARG:".Length, tokens[3].Length- "!ARG:".Length),
                    });
            }
            foreach (var action in actions.Values)
            {
                if (docMap.ContainsKey(action.commandWord))
                {
                    action.scriptName = docMap[action.commandWord].scriptName;
                    action.scriptDesc = docMap[action.commandWord].scriptDesc;
                    action.scriptArg = docMap[action.commandWord].scriptArg;
                }
                else
                {
                    LogUtil.debug($"{action.commandWord}  not doc");
                }
                
            }
            
            foreach (var condition in conditions.Values)
            {
                if (docMap.ContainsKey(condition.commandWord))
                {
                    condition.scriptName = docMap[condition.commandWord].scriptName;
                    condition.scriptDesc = docMap[condition.commandWord].scriptDesc;
                    condition.scriptArg = docMap[condition.commandWord].scriptArg;
                }
                else
                {
                    LogUtil.debug($"{condition.commandWord}  not doc");
                }
                
            }
            var a = 2;
        }

        private static void mergeTranslation()
        {
            
            LogUtil.debug($"--------------------mergeTranslation------------------");
            var scripDocs = File.ReadAllText("脚本翻译.txt").Split(new string[] { "\r\n\r\n" }, 
                StringSplitOptions.RemoveEmptyEntries);

            var docMap = new Dictionary<string, TransItem>();
            foreach (var scripDoc in scripDocs)
            {
                var tokens = scripDoc.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                docMap.Add(tokens[0].Substring("!ACTION:".Length, tokens[0].Length- "!ACTION:".Length).Trim(),
                    new TransItem()
                    {
                        trans = tokens[2].Substring("翻译名称:".Length, tokens[2].Length- "翻译名称:".Length),
                    });
            }
            foreach (var action in actions.Values)
            {
                if (docMap.ContainsKey(action.commandWord))
                {
                    action.scriptTrans = docMap[action.commandWord].trans;
                }
                else
                {
                    LogUtil.debug($"{action.commandWord}  not doc");
                }
                
            }
            
            foreach (var condition in conditions.Values)
            {
                if (docMap.ContainsKey(condition.commandWord))
                {
                    condition.scriptTrans = docMap[condition.commandWord].trans;
                }
                else
                {
                    LogUtil.debug($"{condition.commandWord}  not doc");
                }
                
            }
        }
        
        private static void mergeTranslation2()
        {
            
            LogUtil.debug($"--------------------mergeTranslation2------------------");
            var actionTrans = File.ReadAllText("脚本动作翻译.txt").Split(new string[] { "\r\n" }, 
                StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Select(line =>
                {
                    var left = line.IndexOf('[');
                    var right = line.IndexOf(']');
                    if (left < 0 || right < 0)
                    {
                        LogUtil.debug($"invalid line: {line}");
                        throw new Exception();
                    }
                    else
                    {
                        int editorNum = Convert.ToInt32(line.Substring(left + 1, right - left - 1));
                        var trsnslation = line.Substring(right + 1, line.Length - (right + 1));
                        return new TransItem2()
                        {
                            editorNumber = editorNum,
                            trans = trsnslation
                        };
                    }
                })
                .ToDictionary(item => item.editorNumber);
            
            var conditionTrans = File.ReadAllText("脚本状态翻译.txt").Split(new string[] { "\r\n" }, 
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Select(line =>
                {
                    var left = line.IndexOf('[');
                    var right = line.IndexOf(']');
                    if (left < 0 || right < 0)
                    {
                        LogUtil.debug($"invalid line: {line}");
                        throw new Exception();
                    }
                    else
                    {
                        int editorNum = Convert.ToInt32(line.Substring(left + 1, right - left - 1));
                        var trsnslation = line.Substring(right + 1, line.Length - (right + 1));
                        return new TransItem2()
                        {
                            editorNumber = editorNum,
                            trans = trsnslation
                        };
                    }
                })
                .ToDictionary(item => item.editorNumber);

            
            foreach (var action in actions.Values)
            {
                action.scriptTrans = actionTrans[action.editorNumber].trans;

            }
            
            foreach (var condition in conditions.Values)
            {
                if (conditionTrans.ContainsKey(condition.editorNumber))
                {
                    condition.scriptTrans = conditionTrans[condition.editorNumber].trans;
                }
                else
                {
                    LogUtil.debug($"mergeTranslation2 | editornumber: {condition.editorNumber} not found");
                }
                
                
            }

        }
        
        public class DocItem
        {
            //脚本条目
            public string scriptName { get; set; }
            //帮助文本
            public string scriptDesc { get; set; }
            //参数位置参考
            public string scriptArg { get; set; }
        }
        
        public class TransItem
        {
            //脚本翻译
            public string trans { get; set; }
        }
        
        public class TransItem2
        {
            public int editorNumber { get; set; }

            //脚本翻译
            public string trans { get; set; }
        }

        private static void traverseScript(Script script)
        {
            foreach (OrCondition orCondition in script.scriptOrConditions)
            {
                foreach (var scriptCondition in orCondition.conditions)
                {
                    addConditions(scriptCondition.scriptContent);
                }
            }

            foreach (var scriptAction in script.ScriptActionOnTrue)
            {
                addActions(scriptAction);
            }
        }

        private static void traverseGroup(ScriptGroup scriptGroup)
        {
            foreach (var subGroup in scriptGroup.scriptGroups)
            {
                traverseGroup(subGroup);
            }
            foreach (var script in scriptGroup.scripts)
            {
                traverseScript(script);
            }
            
        }

        public static void addActions(ScriptContent scriptContent)
        {
            if (!collectScript)
            {
                return;
            }
            if (!actions.ContainsKey(scriptContent.contentName))
            {
                bool accept = true;
                foreach (ScriptArgument scriptArgument in scriptContent.arguments)
                {
                    if (scriptArgument.intValue == 0 && scriptArgument.floatValue.Equals(0f) && scriptArgument.stringValue.Equals("") && scriptArgument.position.Equals(new Vec3D(0f,0f,0f)))
                    {
                        accept = false;
                        break;
                    }
                }

                if (accept)
                {
                    actions.Add(scriptContent.contentName, getScriptModel(scriptContent));
                }
            }
        }

        private static ScriptModel getScriptModel(ScriptContent scriptContent)
        {
            List<ScriptArgument> arguments = scriptContent.arguments;
            List<ArgumentModel> argumentModel = new List<ArgumentModel>();
            foreach (var argument in arguments)
            {
                string realType = "";
                string exampleData = "";
                if (argument.intValue != 0)
                {
                    realType = ScriptSpec.INT;
                    exampleData = argument.intValue.ToString();
                }
                else if (!argument.floatValue.Equals(0f))
                {
                    realType = ScriptSpec.FLOAT;
                    exampleData = argument.floatValue.ToString();
                }
                else if (!argument.stringValue.Equals(""))
                {
                    realType = ScriptSpec.STRING;
                    exampleData = argument.stringValue.ToString();
                }
                else
                {
                    realType = ScriptSpec.POSITION;
                    exampleData = argument.position.ToString();
                }

                argumentModel.Add(new ArgumentModel(argument.argumentType, realType, exampleData));
            }
            return new ScriptModel(scriptContent.contentName, scriptContent.contentType, argumentModel);
        }

        public static void addConditions(ScriptContent scriptContent)
        {
            if (!collectScript)
            {
                return;
            }
            if (!conditions.ContainsKey(scriptContent.contentName))
            {
                bool accept = true;
                foreach (ScriptArgument scriptArgument in scriptContent.arguments)
                {
                    if (scriptArgument.intValue == 0 && scriptArgument.floatValue.Equals(0f) && scriptArgument.stringValue.Equals("") && scriptArgument.position.Equals(new Vec3D(0f,0f,0f)))
                    {
                        accept = false;
                        break;
                    }
                }

                if (accept)
                {
                    conditions.Add(scriptContent.contentName, getScriptModel(scriptContent));
                }
            }
        }
    }
}