using System;
using System.Diagnostics;
using System.IO;
using MapCoreLib.Core;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using NewMapParser.Core;
using RMGlib.Core.Utility;

namespace NewMapParser
{
    internal class Program
    {
        private const string userName = "29972";
        public static void Main(string[] args)
        {
            LogUtil.init(Directory.GetCurrentDirectory());
            // new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\Mission2\\Mission2.map").parse();
            // new Ra3Map("chuangguan_xq/chuangguan_xq.map").parse();
            
            // ScriptXml.serialize($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\the_last_tial\\the_last_tial.map");
            // ScriptXml.deserialize($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\the_last_tial\\the_last_tial.map");
            
            // MapXmlUtil.buildSchema();
            
            // ScriptCollection.collect();
            // GenScriptsMd.GenAllScriptList();

            // printTrans();
            // ScriptTranslation.GenTranslationMap();
            // ObjectListTrans.Run();
            
            // GetObjectCatogory.Run();
            // GenScriptConfig.genScriptTransMap();
            ImageToXyz.Run();
        }

        private static void printTrans()
        {
            ScriptSpec.initScriptSpec();
            
            using (var file = new StreamWriter(File.OpenWrite("脚本条件原文.txt")))
            {
                foreach (var condition in ScriptSpec.conditionsSpec.Values)
                {
                    file.WriteLine($"{condition.scriptName}");
                }
            }
        }
    }
}