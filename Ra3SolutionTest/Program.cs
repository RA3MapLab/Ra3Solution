using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MapCoreLib.Core;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.NewMap;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using NewMapParser.Core;
using Newtonsoft.Json;
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
            // ImageToXyz.Run();
            // GenerateObjNames.Run();

            // var ra3Map = new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\CreateMap\\CreateMap.map");
            // ra3Map.parse();
            // var majorAssets = ra3Map.getContext().MapStruct.getAssets();
            // var heightmapData = majorAssets[2] as DefaultMajorAsset;
            // var blendTitleData = majorAssets[3] as DefaultMajorAsset;
            // File.WriteAllBytes("heightmapData_2.bin", heightmapData.data);
            // File.WriteAllBytes("blendTitleData_2.bin", blendTitleData.data);
            // var a = 2;
            
            // ScriptXml.serialize(PathUtil.defaultMapPath("[MRZYQH"));
            // ScriptXml.deserialize(PathUtil.defaultMapPath("[MRZYQH"));
            // MsPinYinHelper.Run();
            
            // var mapPath = PathUtil.defaultMapPath("CreateMap");
            // Ra3Map.newMap(mapPath, new NewMapConfig()
            // {
            //     width = 500,
            //     height = 500,
            //     options = new Dictionary<string, NewMapHandlerOption>()
            //     {
            //         {"RandomTerrain", new RandomTerrainOption()
            //         {
            //             seed = 12312,
            //             passCount = 7
            //         }}
            //         
            //     }
            // }).save(PathUtil.RA3MapFolder, "CreateMap");
            
            // MiniMap.genMiniMap("City_Chaos_2.0", 0, false);
            
            
            // var ra3Map = new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\MAP\\MAP.map");
            // ra3Map.parse();
            // var sidesList = ra3Map.getAsset<SidesList>(Ra3MapConst.ASSET_SidesList);
            // sidesList.players[12].assetPropertyCollection.setProperty("aiPersonality", "AIPersonalityDefinition:1JapanBalanced");
            // ra3Map.save(Path.Combine(Directory.GetCurrentDirectory(), "test"), "MAP");
            // var a = 2;
            foreach (var file in Directory.EnumerateFiles(
                         @"D:\file\yule\RA3_MODSDK-X_1.3\source\GenEvo_WB_Expansion\WBfiles\ColorCorrection",
                         "*.tga"))
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
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