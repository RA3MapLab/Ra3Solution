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
    }
}