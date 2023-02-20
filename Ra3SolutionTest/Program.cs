using System;
using MapCoreLib.Core;
using MapCoreLib.Core.Util;
using RMGlib.Core.Utility;

namespace NewMapParser
{
    internal class Program
    {
        private const string userName = "peijin";
        public static void Main(string[] args)
        {
            // new Ra3Map("C:\\Users\\peijin\\AppData\\Roaming\\Red Alert 3\\Maps\\Brother\\Brother").parse();
            // new Ra3Map("chuangguan_xq/chuangguan_xq.map").parse();
            Convert.ToBoolean("true");
            // ScriptXml.serialize($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\the_last_tial\\the_last_tial.map");
            ScriptXml.deserialize($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\the_last_tial\\the_last_tial.map");
            
            // MapXmlUtil.buildSchema();
            
            // ScriptCollection.collect();
        }
    }
}