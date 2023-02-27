using System.IO;
using System.Linq;

namespace NewMapParser.Core
{
    public class ScriptTranslation
    {
        public static void GenTranslationMap()
        {
            using (var file = new StreamWriter(File.OpenWrite("chatgpt脚本动作翻译对照.csv")))
            {
                // foreach (var origLine in File.ReadLines("脚本动作翻译原文.txt"))
                // {
                //     foreach (var transLine in File.ReadLines("chatgpt脚本动作翻译.txt"))
                //     {
                //         file.WriteLine($"{origLine}, {transLine}");
                //     }
                // }


                var origText = File.ReadLines("脚本动作翻译原文.txt").ToList();
                var translateText = File.ReadLines("chatgpt脚本动作翻译.txt").ToList();
                for (var i = 0; i < origText.Count; i++)
                {
                    // file.WriteLine($"{origText[i]}");
                    // file.WriteLine($"{translateText[i]}");
                    // file.WriteLine();
                    file.WriteLine($"{origText[i]}, {translateText[i]}");
                }
                
            }

            
        }
    }
}