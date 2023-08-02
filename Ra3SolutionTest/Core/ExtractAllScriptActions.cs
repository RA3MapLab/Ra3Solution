using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core.Util;
using Newtonsoft.Json;
using RMGlib.Core.Utility;

namespace NewMapParser.Core
{
    public class ExtractAllScriptActions
    {
        public static void Run()
        {
            var actionsSpec =
                JsonConvert.DeserializeObject<List<ScriptModel>>(
                    File.ReadAllText(Path.Combine(PathUtil.configDir, "ScriptConditonNew.json")));
            var actionsSpec2 = actionsSpec.OrderBy(item => item.editorNumber);
            foreach (var action in actionsSpec2)
            {
                Console.WriteLine($"{action.commandWord}={action.editorNumber},");
            }
        }
    }
}