 using System;
using System.Collections.Generic;
using System.Diagnostics;
 using System.Globalization;
 using System.IO;
 using System.Linq;
 using System.Reflection;
 using System.Text;
 using MapCoreLib.Core;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.NewMap;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using NewMapParser.Core;
using Newtonsoft.Json;
 using Ra3ModBuilder.Core;
 using RMGlib.Core.Utility;

namespace NewMapParser
{
    internal class Program
    {
        private const string userName = "29972";
        public static int Main(string[] args)
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

            // var ra3Map = new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\[COR]a_little_field_corona_2.04_MH\\[COR]a_little_field_corona_2.04_MH.map");
            // // var ra3Map = new Ra3Map(Path.Combine(Directory.GetCurrentDirectory(), "dump.dat"));
            // ra3Map.parse();
            // var a = 2;
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

            // MiniMap.genMiniMap("The_Battle_of_New_York", 0, false);


            var ra3Map = new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\AlliesA_v1.31\\AlliesA_v1.31.map");
            ra3Map.parse();
            // var startPos = MapInfoHelper.getStartPos(ra3Map);
            var sidesList = ra3Map.getAsset<SidesList>(Ra3MapConst.ASSET_SidesList);
            sidesList.players[5].assetPropertyCollection.setProperty("aiPersonality", "AIPersonalityDefinition:01S");
            ra3Map.save(Path.Combine(Directory.GetCurrentDirectory(), "test"), "MAP");
            // var a = 2;
            // foreach (var file in Directory.EnumerateFiles(
            //              @"D:\file\yule\RA3_MODSDK-X_1.3\source\GenEvo_WB_Expansion\WBfiles\ColorCorrection",
            //              "*.tga"))
            // {
            //     Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            // }

            // ExtractAllScriptActions.Run();
            
            // var ra3Folder = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Red Alert 3");
            // var currentProfile = Path.Combine(Path.Combine(ra3Folder, "Profiles"), "directory.ini");
            // readEAUtf16LeContent(currentProfile);

            // var res = FNVHash.FNV_1_Hash("MN");
            // Console.WriteLine(res);

            
            //enumarate all .h and .cpp files in Duilib folder recursively and convert them to utf8 and save
            // Directory.EnumerateFiles("./Duilib", "*.*", SearchOption.AllDirectories)
            //     .Where(file => file.EndsWith(".h") || file.EndsWith(".cpp"))
            //     .ToList()
            //     .ForEach(file =>
            //     {
            //         var content = File.ReadAllText(file, Encoding.GetEncoding("gb2312"));
            //         File.WriteAllText(file, content, Encoding.UTF8);
            //     });
            
            // var hash = 

            // var modMaker = new ModMaker("YRre", "1.0");
            // modMaker.configMaker("");
            // modMaker.buildMod("YRre", "1.0");
            
            
            // var ra3Map = new Ra3Map($"C:\\Users\\{userName}\\AppData\\Roaming\\Red Alert 3\\Maps\\NewMapTest_mix\\NewMapTest_mix.map");
            // ra3Map.parse();
            // ra3Map.getContext().MapStruct.getAssets().ForEach(asset =>
            // {
            //     var defaultMajorAsset = asset as DefaultMajorAsset;
            //     // save asset data to file
            //     File.WriteAllBytes($"./mapSplit2/{defaultMajorAsset.getAssetName()}.bin", defaultMajorAsset.data);
            // });

            // return doMix(args);
            
            // IncludePathTest.Run();
            // TestVCRedist.Run();
            // LoadLIbTest.Run();
            return 0;
        }

        public static string readEAUtf16LeContent(string path)
        {
            var text = File.ReadAllText(path);
            string res;
            using (var ms = new MemoryStream())
            {
                ms.Seek(0, SeekOrigin.Begin);
                for (var i = 0; i < text.Length; i++)
                {
                    if (text[i] == '_')
                    {
                        ms.WriteByte(byte.Parse(text.Substring(i + 1, 2), NumberStyles.HexNumber));
                        i += 2;
                    }
                    else if (text[i] == '\n')
                    {
                        var bytes = System.Text.Encoding.Unicode.GetBytes("\n");
                        ms.Write(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        ms.WriteByte((byte)text[i]);
                    }
                }

                res = System.Text.Encoding.Unicode.GetString(ms.ToArray());
            }

            return res;
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