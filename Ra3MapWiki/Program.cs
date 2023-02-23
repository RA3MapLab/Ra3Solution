using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ra3MapWiki.Core;
using RMGlib.Core.Utility;

namespace Ra3MapWiki
{
    internal class Program
    {
        private const string docRoot = "docs";
        
        public static void Main(string[] args)
        {
            GenAllScriptList();
        }

        private static void GenAllScriptList()
        {
            ScriptSpec.initScriptSpec();

            List<FileItem> conditionItems = new List<FileItem>();
            foreach (var condition in ScriptSpec.conditionsSpec.Values)
            {
                var tokens = condition.scriptName.Split('/');
                insertFileItem(tokens.ToList(), conditionItems, condition);
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
                foreach (var conditionItem in conditionItems)
                {
                    printScriptItem(file, conditionItem, 2);
                }
            }
            
            using (var file = new StreamWriter(File.OpenWrite(scriptactionMd)))
            {
                foreach (var actionItem in actionItems)
                {
                    printScriptItem(file, actionItem, 2);
                }
            }

        }

        private static void printScriptItem(StreamWriter sw, FileItem fileItem, int depth)
        {
            if (fileItem.isLeaf)
            {
                sw.WriteLine($"{new string('#', depth)} {fileItem.name}[{fileItem.ScriptModel.editorNumber}]");
                sw.WriteLine($"{fileItem.ScriptModel.commandWord}");
            }
            else
            {
                sw.WriteLine($"{new string('#', depth)} {fileItem.name}");
                foreach (var itemSubFile in fileItem.subFiles)
                {
                    printScriptItem(sw, itemSubFile, depth + 1);
                }
            }
        }


        private static void insertFileItem(List<string> tokens, List<FileItem> items, ScriptModel scriptModel)
        {
            if (tokens.Count == 0)
            {
                return;
            }
            var findItem = items.Find(item => item.name.Equals(tokens[0]));
            if (findItem == null)
            {
                findItem = new FileItem()
                {
                    isLeaf = tokens.Count == 1,
                    name = tokens[0],
                    ScriptModel = tokens.Count == 1 ? scriptModel : null
                };
                items.Add(findItem);
            }

            tokens.RemoveAt(0);
            insertFileItem(tokens, findItem.subFiles, scriptModel);
        }
    }
}