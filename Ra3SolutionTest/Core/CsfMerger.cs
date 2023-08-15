using System;
using Ra3ModBuilder.Core;

namespace NewMapParser.Core
{
    public class CsfMerger
    {
        public static void Run()
        {
            new CSFUtil().mergeCsv2Csf("mission2.csv", "gamestrings.csf", "gamestrings2.csf");
            // var csfUtil = new CSFUtil();
            // csfUtil.parse("gamestrings.csf");
            // //write all csfUtil.entries to txt
            // var writer = new System.IO.StreamWriter("gamestrings.txt");
            // foreach (var entry in csfUtil.entries)
            // {
            //     writer.WriteLine(entry.Key);
            //     writer.WriteLine(entry.Value);
            //     writer.WriteLine();
            // }
            // writer.Close();
        }
    }
}