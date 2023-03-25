using System;
using System.IO;
using Compress;
using MapCoreLib.Core.Asset;
using MapCoreLib.Log;

namespace MapCoreLib.Core
{
    public class Ra3MapParseImpl
    {
        public MapDataContext parse(string mapPath)
        {
            using (FileStream fs = File.OpenRead(mapPath))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    BinaryReader br2;
                    switch (br.ReadUInt32())
                    {
                        case Ra3MapConst.UnCompressorFlag:
                            br2 = br;
                            break;
                        case Ra3MapConst.CompressorFlag:
                        {
                            BinaryWriter bWriter = new BinaryWriter(new MemoryStream((int) br.BaseStream.Length));
                            br.BaseStream.Position = 8L;
                            PrefUtil.start("decompress");
                            RefpackComrpessor.Decompress(br, bWriter);
                            PrefUtil.stop("decompress");
                            br2 = new BinaryReader(bWriter.BaseStream);
                            br2.BaseStream.Position = 4L;
                            break;
                        }
                        default:
                            throw new Exception("unknown map start flag");
                    }

                    return doParseMap(br2);
                }
            }
            // LogUtil.WriteLine("*\t Done parsing \"{0}\"", file);
        }

        private MapDataContext doParseMap(BinaryReader br)
        {
            PrefUtil.start("doParseMap");
            Ra3MapStruct mapStruct = new Ra3MapStruct();
            var dataContext = new MapDataContext(mapStruct);
            parseStringPool(br, mapStruct);
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                var index = br.ReadInt32();
                var majorAssetName = dataContext.MapStruct.findStringByIndex(index);
                br.BaseStream.Position -= 4;
                switch (majorAssetName)
                {
                    case Ra3MapConst.ASSET_ObjectsList:
                        mapStruct.addAsset(new ObjectsList().fromStream(br, dataContext));
                        break;
                    case Ra3MapConst.ASSET_AssetList:
                        mapStruct.addAsset(new AssetList().fromStream(br, dataContext));
                        break;
                    case Ra3MapConst.ASSET_SidesList:
                        mapStruct.addAsset(new SidesList().fromStream(br, dataContext));
                        break;
                    case Ra3MapConst.ASSET_PlayerScriptsList:
                        mapStruct.addAsset(new PlayerScriptsList().fromStream(br, dataContext));
                        break;
                    case Ra3MapConst.ASSET_Teams:
                        mapStruct.addAsset(new Teams().fromStream(br, dataContext));
                        break;
                    case Ra3MapConst.ASSET_HeightMapData:
                        mapStruct.addAsset(new HeightMapData().fromStream(br, dataContext));
                        break;
                    // case Ra3MapConst.ASSET_BlendTileData:
                    //     mapStruct.addAsset(new BlendTileData().fromStream(br, dataContext));
                    //     break;
                    case Ra3MapConst.ASSET_WorldInfo:
                        mapStruct.addAsset(new WorldInfo().fromStream(br, dataContext));
                        break;
                    // case Ra3MapConst.ASSET_StandingWaterAreas:
                    //     mapStruct.addAsset(new StandingWaterAreas().fromStream(br, dataContext));
                    //     break;
                    case Ra3MapConst.ASSET_GlobalLighting:
                        mapStruct.addAsset(new GlobalLighting().fromStream(br, dataContext));
                        break;
                    default:
                        mapStruct.addAsset(new DefaultMajorAsset().fromStream(br, dataContext));
                        break;
                }
            }
            PrefUtil.stop("doParseMap");
            return dataContext;
        }

        private static void parseStringPool(BinaryReader br, Ra3MapStruct mapStruct)
        {
            var stringPoolSize = br.ReadInt32();
            for (int i = 0; i < stringPoolSize; i++)
            {
                mapStruct.RegisterString(br.ReadString(), br.ReadInt32());
            }
        }
    }
}