using System.IO;
using System.Linq;
using Compress;
using MapCoreLib.Core.Asset;
using MapCoreLib.Log;

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
                            using (var resFile = File.OpenWrite(Path.Combine("map", Path.GetFileNameWithoutExtension(mapPath) + "_un.map")))
                            {
                                resFile.CopyTo(File.OpenWrite($"map/{Path.GetFileNameWithoutExtension(mapPath)}_un.map"));
                            }
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

        private void doSaveMap(string mapFile)
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