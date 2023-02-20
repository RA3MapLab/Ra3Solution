using System;
using System.Collections.Generic;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class BlendTileData : MajorAsset
    {
        public int mapWidth;

        public int mapHeight;

        public int area;

        public ushort[,] tiles;

        private int blendsCount;

        private ushort[,] blends;

        public List<BlendInfo> blendInfo = new List<BlendInfo>();

        private ushort[,] singleEdgeBlends;

        private int cliffBlendsCount;

        private ushort[,] cliffBlends;

        public Passability[,] passability;

        private bool[,] passageWidth;

        public bool[,] visibility;

        private bool[,] buildability;

        private bool[,] tiberiumGrowability;

        private byte[,] dynamicShrubbery;

        private int textureCellCount;
        
        public List<Texture> textures = new List<Texture>();
        
        private int magic1;
        private int magic2;

        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            if (context.mapWidth == Int32.MinValue || context.mapHeight == Int32.MinValue)
            {
                throw new Exception("illegal mapWidth or mapHeight");
            }

            mapWidth = context.mapWidth;
            mapHeight = context.mapHeight;
            area = binaryReader.ReadInt32();
            tiles = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            blends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            singleEdgeBlends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            cliffBlends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);

            passability = new Passability[mapWidth, mapHeight];
            bool[,] impassable = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            bool[,] impassableToPlayers = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            passageWidth = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            bool[,] extraPassable = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            visibility = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            buildability = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            bool[,] impassableToAirUnits = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            tiberiumGrowability = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x2 = 0; x2 < mapWidth; x2++)
                {
                    if (impassable[x2, y])
                    {
                        passability[x2, y] = Passability.Impassable;
                    }
                    else if (impassableToPlayers[x2, y])
                    {
                        passability[x2, y] = Passability.ImpassableToPlayers;
                    }
                    else if (impassableToAirUnits[x2, y])
                    {
                        passability[x2, y] = Passability.ImpassableToAirUnits;
                    }
                    else if (extraPassable[x2, y])
                    {
                        passability[x2, y] = Passability.ExtraPassable;
                    }
                    else
                    {
                        passability[x2, y] = Passability.Passable;
                    }
                }
            }

            dynamicShrubbery = binaryReader.ReadArray<byte>(mapWidth, mapHeight);
            textureCellCount = binaryReader.ReadInt32();
            blendsCount = binaryReader.ReadInt32() - 1;
            cliffBlendsCount = binaryReader.ReadInt32() - 1;

            var textureCount = binaryReader.ReadInt32();
            for (int i = 0; i < textureCount; i++)
            {
                textures.Add(new Texture().fromStream(binaryReader, context));
            }
            
            magic1 = binaryReader.ReadInt32();
            magic2 = binaryReader.ReadInt32();

            for (int i = 0; i < blendsCount; i++)
            {
                blendInfo.Add(new BlendInfo().fromStream(binaryReader, context));
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(area);
            binaryWriter.WriteArray(tiles);
            binaryWriter.WriteArray(blends);
            binaryWriter.WriteArray(singleEdgeBlends);
            binaryWriter.WriteArray(cliffBlends);
            
            bool[,] impassable = new bool[mapWidth, mapHeight];
            bool[,] impassableToPlayers = new bool[mapWidth, mapHeight];
            bool[,] impassableToAirUnits = new bool[mapWidth, mapHeight];
            bool[,] extraPassable = new bool[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (passability[x, y])
                    {
                        case Passability.Impassable:
                            impassable[x, y] = true;
                            break;
                        case Passability.ImpassableToAirUnits:
                            impassableToAirUnits[x, y] = true;
                            break;
                        case Passability.ImpassableToPlayers:
                            impassableToPlayers[x, y] = true;
                            break;
                        case Passability.ExtraPassable:
                            extraPassable[x, y] = true;
                            break;
                    }
                }
            }
            
            binaryWriter.WriteArray(impassable);
            binaryWriter.WriteArray(impassableToPlayers);
            binaryWriter.WriteArray(extraPassable);
            binaryWriter.WriteArray(passageWidth);
            binaryWriter.WriteArray(visibility);
            binaryWriter.WriteArray(buildability);
            binaryWriter.WriteArray(impassableToAirUnits);
            binaryWriter.WriteArray(tiberiumGrowability);
            binaryWriter.WriteArray(dynamicShrubbery);
            binaryWriter.Write(textureCellCount);
            binaryWriter.Write(blendsCount + 1);
            binaryWriter.Write(cliffBlendsCount + 1);
            binaryWriter.Write(textures.Count);
            foreach (var texture in textures)
            {
                texture.saveData(binaryWriter, context);
            }
            binaryWriter.Write(magic1);
            binaryWriter.Write(magic2);
            foreach (var info in blendInfo)
            {
                info.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_BlendTileData;
        }
    }
}