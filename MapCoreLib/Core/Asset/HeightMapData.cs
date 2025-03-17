using System.Collections.Generic;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class HeightMapData: MajorAsset
    {
        public float[,] elevations;
        // public ushort[,] elevations;

        public List<HeightMapBorder> borders = new List<HeightMapBorder>();

        public int mapWidth;

        public int mapHeight;

        public int borderWidth;

        public int playableWidth;

        public int playableHeight;

        public int area;
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            context.mapWidth = mapWidth = binaryReader.ReadInt32();
            context.mapHeight = mapHeight = binaryReader.ReadInt32();
            context.border = borderWidth = binaryReader.ReadInt32();
            playableWidth = mapWidth - 2 * borderWidth;
            playableHeight = mapHeight - 2 * borderWidth;
            
            int borderCount = binaryReader.ReadInt32();
            for (int i = 0; i < borderCount; i++)
            {
                borders.Add(new HeightMapBorder().fromStream(binaryReader, context));
            }
            
            area = binaryReader.ReadInt32();
            
            elevations = new float[mapWidth, mapHeight];
            // elevations = new ushort[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // elevations[x, y] = binaryReader.ReadUInt16();
                    elevations[x, y] = StreamExtension.FromSageFloat16(binaryReader.ReadUInt16());
                }
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(mapWidth);
            binaryWriter.Write(mapHeight);
            binaryWriter.Write(borderWidth);
            binaryWriter.Write(borders.Count);
            foreach (var border in borders)
            {
                border.saveData(binaryWriter, context);
            }
            binaryWriter.Write(area);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // binaryWriter.Write(elevations[x, y]);
                    binaryWriter.Write(StreamExtension.ToSageFloat16(elevations[x, y]));
                }
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_HeightMapData;
        }

        public override short getVersion()
        {
            return 6;
        }

        public static HeightMapData newInstance(MapDataContext context, NewMapConfig config)
        {
            var heightMapData = new HeightMapData();
            heightMapData.name = Ra3MapConst.ASSET_HeightMapData;
            heightMapData.id = context.MapStruct.RegisterString(heightMapData.name);
            heightMapData.version = heightMapData.getVersion();
            context.mapWidth = heightMapData.mapWidth = config.width + 2 * config.border;
            context.mapHeight = heightMapData.mapHeight = config.height + 2 * config.border;
            context.border = heightMapData.borderWidth = config.border;
            heightMapData.playableHeight = config.height;
            heightMapData.playableWidth = config.width;
            heightMapData.area = heightMapData.mapWidth * heightMapData.mapHeight;
            heightMapData.elevations = new float[heightMapData.mapWidth, heightMapData.mapHeight];
            for (int y = 0; y < heightMapData.mapHeight; y++)
            {
                for (int x = 0; x < heightMapData.mapWidth; x++)
                {
                    heightMapData.elevations[x, y] = 210;
                }
            }
            heightMapData.borders.Add(HeightMapBorder.newInstance(0, 0, 
                config.width, config.height));
            return heightMapData;
        }
    }
}