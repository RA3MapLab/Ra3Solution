using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NewMapParser.Core
{
    public class ObjectListTrans
    {
        public static void Run()
        {
            var map = new Dictionary<string, string>();
            foreach (var line in File.ReadLines("objectListTrans.txt"))
            {
                var tokens = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                map.Add(tokens[0], tokens[1]);
            }

            File.WriteAllText("objectList.json", JsonConvert.SerializeObject(map));
        }
    }
}