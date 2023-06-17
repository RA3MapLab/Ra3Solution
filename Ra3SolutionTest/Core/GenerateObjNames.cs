using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NewMapParser.Core
{
    public class GenerateObjNames
    {
        public static void Run()
        {
            var map = new Dictionary<string, string>();
            foreach (var line in File.ReadLines("ObjWndTrans.txt"))
            {
                var tokens = line.Split('\t');
                // var tokens = System.Text.RegularExpressions.Regex.Split( line, @"\s{2,}");
                map.Add(tokens[0].Trim(), tokens[1].Trim());
            }
            File.WriteAllText("ObjWndTrans.json", JsonConvert.SerializeObject(map));
        }
    }
}