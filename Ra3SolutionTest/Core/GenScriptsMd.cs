using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ra3MapWiki.Core;
using RMGlib.Core.Utility;

namespace NewMapParser.Core
{
    public class GenScriptsMd
    {
        private const string docRoot = "docs";

        public static void GenAllScriptList()
        {
            ScriptSpec.initScriptSpec();

            List<FileItem> conditionItems = new List<FileItem>();
            foreach (var condition in ScriptSpec.conditionsSpec.Values)
            {
                var nameToken = condition.scriptName.Split('/');
                insertFileItem(nameToken.ToList(), conditionItems, condition);
            }

            List<FileItem> actionItems = new List<FileItem>();
            foreach (var action in ScriptSpec.actionsSpec.Values)
            {
                var tokens = action.scriptName.Split('/');
                insertFileItem(tokens.ToList(), actionItems, action);
            }
            
            var scriptactionMd = Path.Combine(docRoot, "ScriptAction.md");
            var scriptconditionMd = Path.Combine(docRoot, "ScriptCondition.md");
            
            if (File.Exists(scriptactionMd))
            {
                File.Delete(scriptactionMd);
            }

            if (File.Exists(scriptconditionMd))
            {
                File.Delete(scriptactionMd);
            }

            using (var file = new StreamWriter(File.OpenWrite(scriptconditionMd)))
            {
                file.WriteLine("# 地编脚本条件列表    ");
                foreach (var conditionItem in conditionItems)
                {
                    printScriptItem(file, conditionItem, 2);
                }
            }
            
            using (var file = new StreamWriter(File.OpenWrite(scriptactionMd)))
            {
                file.WriteLine("# 地编脚本动作列表    ");
                foreach (var actionItem in actionItems)
                {
                    printScriptItem(file, actionItem, 2);
                }
            }

        }

        private static void printScriptItem(StreamWriter sw, FileItem fileItem, int depth)
        {
            //注意转义，否则文档会无法显示
            if (fileItem.isLeaf)
            {
                sw.WriteLine($"{new string('#', depth)} {HttpUtility.HtmlEncode(fileItem.name)}[{fileItem.ScriptModel.editorNumber}]");
                
                
                sw.WriteLine($" 翻译：{HttpUtility.HtmlEncode(fileItem.transName)}               ");
                sw.WriteLine($" 命令字：{fileItem.ScriptModel.commandWord}                  ");
            }
            else
            {
                // var transSuffix = fileItem.transName.Length > 0 ? $"({HttpUtility.HtmlEncode(fileItem.transName)})" : "";
                var transSuffix = "";
                sw.WriteLine($"{new string('#', depth)} {fileItem.name}{transSuffix}");
                foreach (var itemSubFile in fileItem.subFiles)
                {
                    printScriptItem(sw, itemSubFile, depth + 1);
                }
            }
        }


        private static void insertFileItem(List<string> nameToken, List<FileItem> items, ScriptModel scriptModel)
        {
            if (nameToken.Count == 0)
            {
                return;
            }
            var findItem = items.Find(item => item.name.Equals(nameToken[0]));
            if (findItem == null)
            {
                findItem = new FileItem()
                {
                    isLeaf = nameToken.Count == 1,
                    name = nameToken[0],
                    transName = nameToken.Count == 1 ? scriptModel.scriptTrans : "",
                    ScriptModel = nameToken.Count == 1 ? scriptModel : null
                };
                items.Add(findItem);
            }

            nameToken.RemoveAt(0);
            insertFileItem(nameToken, findItem.subFiles, scriptModel);
        }
    }
}