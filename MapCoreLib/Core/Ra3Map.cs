using System.IO;
using System.Linq;
using Compress;
using MapCoreLib.Core.Asset;
using MapCoreLib.Log;
using MapCoreLib.Util;
using Newtonsoft.Json;

namespace MapCoreLib.Core
{
    public class Ra3Map
    {
        public readonly string mapPath;
        private readonly Ra3MapParseImpl mapParseImpl = new Ra3MapParseImpl();
        public readonly Ra3MapVisitImpl mapVisitImpl = new Ra3MapVisitImpl();
        private MapDataContext context;

        public Ra3Map(string mapPath)
        {
            this.mapPath = mapPath;
        }

        public static Ra3Map newMap(NewMapConfig newMapConfig)
        {
            // var newMapConfig = JsonConvert.DeserializeObject<NewMapConfig>(config);
            LogUtil.log("Ra3Map newMap");
            var ra3Map = new Ra3Map(newMapConfig.mapPath);
            ra3Map.context = new MapDataContext(new Ra3MapStruct());
            new Ra3MapBuilder(ra3Map, newMapConfig).build();

            return ra3Map;
        }

        public void parse()
        {
            context = mapParseImpl.parse(mapPath);
            context.mapName = Path.GetFileNameWithoutExtension(mapPath);
        }

        public void visit(MapListener mapListener)
        {
            mapVisitImpl.visit(context, mapListener);
        }

        public void DeCompress()
        {
            using (FileStream fs = File.OpenRead(mapPath))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    switch (br.ReadUInt32())
                    {
                        case Ra3MapConst.UnCompressorFlag:
                            // 解压过了
                            break;
                        case Ra3MapConst.CompressorFlag:
                        {
                            BinaryWriter bWriter = new BinaryWriter(new MemoryStream((int)br.BaseStream.Length));
                            br.BaseStream.Position = 8L;
                            RefpackComrpessor.Decompress(br, bWriter);
                            // save decompressed map
                            using (var resFile = File.OpenWrite(Path.Combine("map", Path.GetFileNameWithoutExtension(mapPath) + "_un.map")))
                            {
                                bWriter.BaseStream.Position = 0;
                                bWriter.BaseStream.CopyTo(resFile);
                            }
                            
                            // using (var resFile = File.OpenWrite(Path.Combine("map", Path.GetFileNameWithoutExtension(mapPath) + "_un.map")))
                            // {
                            //     resFile.CopyTo(File.OpenWrite($"map/{Path.GetFileNameWithoutExtension(mapPath)}_un.map"));
                            // }
                            break;
                        }
                        default:
                            return;
                    }
                    
                }
            }
        }

        public void save(string outputDir, string mapName)
        {
            PrefUtil.start("saveMap");
            var mapDir = Path.Combine(outputDir, mapName);
            var mapFile = Path.Combine(mapDir, $"{mapName}.map");
            if (Directory.Exists(mapDir))
            {
                new DirectoryInfo(mapDir).Delete(true);
            }

            Directory.CreateDirectory(mapDir);

            doSaveMap(mapFile);
            PrefUtil.stop("saveMap");
        }
        
        public void saveMix()
        {
            var oldMapName = Path.GetFileNameWithoutExtension(mapPath);
            var newMapName = $"{oldMapName}_mix";
            var outputDir = Path.GetDirectoryName(Path.GetDirectoryName(mapPath));
            var newmapDir = Path.Combine(outputDir, newMapName);
            var newmapFile = Path.Combine(newmapDir, $"{newMapName}.map");
            if (newmapDir.Length == 0)
            {
                return;
            }
            if (Directory.Exists(newmapDir))
            {
                new DirectoryInfo(newmapDir).Delete(true);
            }
            Directory.CreateDirectory(newmapDir);

            doSaveMap(newmapFile);
            var minimapFile = Path.Combine(outputDir, oldMapName, $"{oldMapName}_art.tga");
            if (File.Exists(minimapFile))
            {
                File.Copy(minimapFile, Path.Combine(outputDir, newMapName, $"{newMapName}_art.tga"));
            }
            var minimapFile2 = Path.Combine(outputDir, oldMapName, $"{oldMapName}.tga");
            if (File.Exists(minimapFile2))
            {
                File.Copy(minimapFile2, Path.Combine(outputDir, newMapName, $"{newMapName}.tga"));
            }
            var strFile = Path.Combine(outputDir, oldMapName, "map.str");
            if (File.Exists(strFile))
            {
                File.Copy(strFile, Path.Combine(outputDir, newMapName, "map.str"));
            }
            var uploadInfoFile = Path.Combine(outputDir, oldMapName, $"{oldMapName}_info.json");
            if (File.Exists(uploadInfoFile))
            {
                File.Copy(uploadInfoFile, Path.Combine(outputDir, newMapName, $"{newMapName}_info.json"));
            }
        }

        public void save(string mapPath)
        {
            PrefUtil.start("saveMap");
            var mapDir = Path.GetDirectoryName(this.mapPath);
            if (!Directory.Exists(mapDir))
            {
                Directory.CreateDirectory(mapDir);
            }
            doSaveMap(mapPath);
            PrefUtil.stop("saveMap");
        }

        public void doSaveMap(string mapFile)
        {
            using (var memoryStream = new MemoryStream(2048 * 1024))
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    bw.Write(Ra3MapConst.UnCompressorFlag);

                    context.MapStruct.save(bw, context);

                    // 压缩
                    PrefUtil.start("compress");
                    byte[] output;
                    memoryStream.GetBuffer().Skip(0).Take((int) memoryStream.Length).ToArray().RefPackCompress(out output);
                    // output = memoryStream.GetBuffer().Skip(0).Take((int) memoryStream.Length).ToArray();
                    PrefUtil.stop("compress");
                    using (var fileStream = File.OpenWrite(mapFile))
                    {
                        fileStream.Write(output, 0, output.Length);
                    }
                }
            }
        }

        /**
         * 应该只是暂时的，暂时不好用visitor模式
         */
        public T getAsset<T>(string name) where T: MajorAsset
        {
            return context.getAsset<T>(name);
        }

        public int registerString(string name)
        {
            return context.MapStruct.RegisterString(name);
        }

        public MapDataContext getContext()
        {
            return context;
        }

        public void replaceMajorAsset(string name, MajorAsset newAsset)
        {
            getContext().MapStruct.replaceAsset(name, newAsset, context);
        }

        /**
         * 备份原来的地图并且替换原来的地图
         */
        public void modifyMap()
        {
            string backMap = Path.Combine(Path.GetDirectoryName(mapPath), context.mapName + ".backup");
            //2层备份，使用编辑器之前的备份
            string deepackMap = Path.Combine(Path.GetDirectoryName(mapPath), context.mapName + ".deepbackup");
            if (!File.Exists(deepackMap))
            {
                //保留一份最初的,并且永远不修改
                File.Copy(mapPath, deepackMap);
            }
            if (File.Exists(backMap))
            {
                File.Delete(backMap);
            }
            
            File.Copy(mapPath, backMap);

            if (File.Exists(mapPath))
            {
                File.Delete(mapPath);
            }

            doSaveMap(mapPath);
        }
    }
}