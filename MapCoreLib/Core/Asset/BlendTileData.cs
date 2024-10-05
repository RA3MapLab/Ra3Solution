using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core.Util;
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

        public bool[,] impassable;
        
        private bool[,] passageWidth;

        public bool[,] visibility;

        private bool[,] buildability;

        private bool[,] tiberiumGrowability;

        private byte[,] dynamicShrubbery;

        private int textureCellCount;
        
        public List<Texture> textures = new List<Texture>();
        
        private uint magic1 = 3452816845u;
        private int magic2 = 0;

        protected override void parseData(BinaryReader br, MapDataContext context)
        {
            if (context.mapWidth == Int32.MinValue || context.mapHeight == Int32.MinValue)
            {
                throw new Exception("illegal mapWidth or mapHeight");
            }

            mapWidth = context.mapWidth;
            mapHeight = context.mapHeight;
            
            int area = br.ReadInt32();
			tiles = IOUtility.ReadArray<ushort>(br, mapWidth, mapHeight);
			blends = IOUtility.ReadArray<ushort>(br, mapWidth, mapHeight);
			singleEdgeBlends = IOUtility.ReadArray<ushort>(br, mapWidth, mapHeight);
			cliffBlends = IOUtility.ReadArray<ushort>(br, mapWidth, mapHeight);
			passability = new Passability[mapWidth, mapHeight];
			impassable = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			bool[,] impassableToPlayers = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			passageWidth = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			bool[,] extraPassable = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			visibility = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			buildability = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			bool[,] impassableToAirUnits = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
			tiberiumGrowability = IOUtility.ReadArray<bool>(br, mapWidth, mapHeight);
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
			dynamicShrubbery = IOUtility.ReadArray<byte>(br, mapWidth, mapHeight);
			textureCellCount = br.ReadInt32();
			blendsCount = br.ReadInt32() - 1;
			cliffBlendsCount = br.ReadInt32() - 1;
			int textureCount = br.ReadInt32();
			// textures = new Texture[br.ReadInt32()];
			for (int j = 0; j < textureCount; j++)
			{
				textures.Add(new Texture().fromStream(br, context));
			}
			magic1 = br.ReadUInt32();
			magic2 = br.ReadInt32();
			for (int i = 0; i < blendsCount; i++)
			{
				blendInfo.Add(new BlendInfo().fromStream(br, context));
			}
            
            // area = binaryReader.ReadInt32();
            // tiles = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            // blends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            // singleEdgeBlends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            // cliffBlends = binaryReader.ReadArray<ushort>(mapWidth, mapHeight);
            //
            // passability = new Passability[mapWidth, mapHeight];
            // bool[,] impassable = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // bool[,] impassableToPlayers = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // passageWidth = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // bool[,] extraPassable = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // visibility = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // buildability = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // bool[,] impassableToAirUnits = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // tiberiumGrowability = binaryReader.ReadArray<bool>(mapWidth, mapHeight);
            // for (int y = 0; y < mapHeight; y++)
            // {
            //     for (int x2 = 0; x2 < mapWidth; x2++)
            //     {
            //         if (impassable[x2, y])
            //         {
            //             passability[x2, y] = Passability.Impassable;
            //         }
            //         else if (impassableToPlayers[x2, y])
            //         {
            //             passability[x2, y] = Passability.ImpassableToPlayers;
            //         }
            //         else if (impassableToAirUnits[x2, y])
            //         {
            //             passability[x2, y] = Passability.ImpassableToAirUnits;
            //         }
            //         else if (extraPassable[x2, y])
            //         {
            //             passability[x2, y] = Passability.ExtraPassable;
            //         }
            //         else
            //         {
            //             passability[x2, y] = Passability.Passable;
            //         }
            //     }
            // }
            //
            // dynamicShrubbery = binaryReader.ReadArray<byte>(mapWidth, mapHeight);
            // textureCellCount = binaryReader.ReadInt32();
            // blendsCount = binaryReader.ReadInt32() - 1;
            // cliffBlendsCount = binaryReader.ReadInt32() - 1;
            //
            // var textureCount = binaryReader.ReadInt32();
            // for (int i = 0; i < textureCount; i++)
            // {
            //     textures.Add(new Texture().fromStream(binaryReader, context));
            // }
            //
            // magic1 = binaryReader.ReadUInt32();
            // magic2 = binaryReader.ReadInt32();
            //
            // for (int i = 0; i < blendsCount; i++)
            // {
            //     blendInfo.Add(new BlendInfo().fromStream(binaryReader, context));
            // }
        }

        /**
         * 拓展函数WriteArray有问题，先用回原来的
         */
        protected override void saveData(BinaryWriter bw, MapDataContext context)
        {
            // binaryWriter.Write(area);
            // binaryWriter.WriteArray(tiles);
            // binaryWriter.WriteArray(blends);
            // binaryWriter.WriteArray(singleEdgeBlends);
            // binaryWriter.WriteArray(cliffBlends);
            //
            // bool[,] impassable = new bool[mapWidth, mapHeight];
            // bool[,] impassableToPlayers = new bool[mapWidth, mapHeight];
            // bool[,] impassableToAirUnits = new bool[mapWidth, mapHeight];
            // bool[,] extraPassable = new bool[mapWidth, mapHeight];
            // for (int y = 0; y < mapHeight; y++)
            // {
            //     for (int x = 0; x < mapWidth; x++)
            //     {
            //         switch (passability[x, y])
            //         {
            //             case Passability.Impassable:
            //                 impassable[x, y] = true;
            //                 break;
            //             case Passability.ImpassableToAirUnits:
            //                 impassableToAirUnits[x, y] = true;
            //                 break;
            //             case Passability.ImpassableToPlayers:
            //                 impassableToPlayers[x, y] = true;
            //                 break;
            //             case Passability.ExtraPassable:
            //                 extraPassable[x, y] = true;
            //                 break;
            //         }
            //     }
            // }
            //
            // binaryWriter.WriteArray(impassable);
            // binaryWriter.WriteArray(impassableToPlayers);
            // binaryWriter.WriteArray(extraPassable);
            // binaryWriter.WriteArray(passageWidth);
            // binaryWriter.WriteArray(visibility);
            // binaryWriter.WriteArray(buildability);
            // binaryWriter.WriteArray(impassableToAirUnits);
            // binaryWriter.WriteArray(tiberiumGrowability);
            // binaryWriter.WriteArray(dynamicShrubbery);
            // binaryWriter.Write(textureCellCount);
            // binaryWriter.Write(blendsCount + 1);
            // binaryWriter.Write(cliffBlendsCount + 1);
            // binaryWriter.Write(textures.Count);
            // foreach (var texture in textures)
            // {
            //     texture.saveData(binaryWriter, context);
            // }
            // binaryWriter.Write(magic1);
            // binaryWriter.Write(magic2);
            // foreach (var info in blendInfo)
            // {
            //     info.saveData(binaryWriter, context);
            // }
            
            bw.Write(mapHeight * mapWidth);
            IOUtility.WriteArray(bw, tiles);
            IOUtility.WriteArray(bw, blends);
            IOUtility.WriteArray(bw, singleEdgeBlends);
            bw.Write(new byte[mapHeight * mapWidth * 2]);
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
            IOUtility.WriteArray(bw, impassable);
            IOUtility.WriteArray(bw, impassableToPlayers);
            IOUtility.WriteArray(bw, extraPassable);
            IOUtility.WriteArray(bw, passageWidth);
            IOUtility.WriteArray(bw, visibility);
            IOUtility.WriteArray(bw, buildability);
            IOUtility.WriteArray(bw, impassableToAirUnits);
            IOUtility.WriteArray(bw, tiberiumGrowability);
            IOUtility.WriteArray(bw, dynamicShrubbery);
            bw.Write(textureCellCount);
            bw.Write(blendsCount + 1);
            bw.Write(cliffBlendsCount + 1);
            bw.Write(textures.Count);
            foreach (Texture t in textures)
            {
                t.saveData(bw, context);
            }
            bw.Write(magic1);
            bw.Write(0);
            for (int i = 0; i < blendsCount; i++)
            {
                blendInfo[i].saveData(bw, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_BlendTileData;
        }

        public bool[,] getImpassible()
        {
            bool[,] res = new bool[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    switch (passability[x, y])
                    {
                        case Passability.Impassable:
                            res[x, y] = true;
                            break;
                        default:
                            res[x, y] = false;
                            break;
                    }
                }
            }

            return res;
        }

        public override short getVersion()
        {
            return 27;
        }

        public ushort GetTile(int x, int y, int texture)
        {
            int rowFirst = y % 8 / 2 * 16 + y % 2 * 2;
            int current = x % 8 / 2 * 4 + x % 2 + rowFirst;
            current += 64 * texture;
            return (ushort)current;
        }

        public void addTexture(MapDataContext context, string textureName)
        {
	        foreach (var texture in textures)
	        {
		        if (texture.name == textureName)
		        {
			        //重复texture
			        return;
		        }
	        }

	        var newTexture = Texture.newInstance(textureCellCount, textureName);
	        textures.Add(newTexture);
	        textureCellCount += newTexture.cellCount;
	        var worldInfo = context.getAsset<WorldInfo>(Ra3MapConst.ASSET_WorldInfo);
	        worldInfo.properties.setProperty("terrainTextureStrings", 
		        worldInfo.properties.getProperty("terrainTextureStrings") + WorldInfo.textures[textureName]);
        }
        
        public ushort GetTexture(int x, int y)
        {
	        int rowFirst = y % 8 / 2 * 16 + y % 2 * 2;
	        int current = x % 8 / 2 * 4 + x % 2 + rowFirst;
	        return (ushort)((tiles[x, y] - current) / 64);
        }
        
        public string GetTextureName(int x, int y)
        {
	        int rowFirst = y % 8 / 2 * 16 + y % 2 * 2;
	        int current = x % 8 / 2 * 4 + x % 2 + rowFirst;
	        ushort tile =  (ushort)((tiles[x, y] - current) / 64);
	        return textures[tile].name;
        }
        
        public void UpdatePassabilityMap(MapDataContext context)
        {
	        float[,] elev = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData).elevations;
	        double a = 45;
	        float tan = (float)Math.Tan(a);
	        // impassiableCount = 0;
	        for (int y = 0; y < mapHeight; y++)
	        {
		        for (int x = 0; x < mapWidth; x++)
		        {
			        int passages = 0;
			        if (x > 0 && Math.Abs((elev[x, y] - elev[x - 1, y]) / 10f) < tan)
			        {
				        passages++;
			        }
			        if (x < mapWidth - 1 && Math.Abs((elev[x, y] - elev[x + 1, y]) / 10f) < tan)
			        {
				        passages++;
			        }
			        if (y > 0 && Math.Abs((elev[x, y] - elev[x, y - 1]) / 10f) < tan)
			        {
				        passages++;
			        }
			        if (y < mapHeight - 1 && Math.Abs((elev[x, y] - elev[x, y + 1]) / 10f) < tan)
			        {
				        passages++;
			        }
			        if (passages < 4)
			        {
				        passability[x, y] = Passability.Impassable;
				        // impassiableCount++;
				        if (x > 0)
				        {
					        passability[x - 1, y] = Passability.Impassable;
				        }
				        if (x < mapWidth - 1)
				        {
					        passability[x + 1, y] = Passability.Impassable;
				        }
				        if (y > 0)
				        {
					        passability[x, y - 1] = Passability.Impassable;
				        }
				        if (y < mapHeight - 1)
				        {
					        passability[x, y + 1] = Passability.Impassable;
				        }
			        }
			        else
			        {
				        passability[x, y] = Passability.Passable;
			        }
		        }
	        }
        }

        public static BlendTileData newInstance(MapDataContext context, NewMapConfig config)
        {
            var blendTileData = new BlendTileData();
            blendTileData.name = Ra3MapConst.ASSET_BlendTileData;
            blendTileData.id = context.MapStruct.RegisterString(blendTileData.name);
            blendTileData.version = blendTileData.getVersion();
            blendTileData.mapWidth = config.width + 2 * config.border;
            blendTileData.mapHeight = config.height + 2 * config.border;
            blendTileData.area = blendTileData.mapWidth * blendTileData.mapHeight;
            blendTileData.textureCellCount = 0;  //TODO 这里不太确定
            var texture = Texture.newInstance(blendTileData.textureCellCount, config.defaultTexture);
            blendTileData.textures.Add(texture);
            blendTileData.textureCellCount += texture.cellCount;
            
            blendTileData.tiles = new ushort[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.blends = new ushort[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.singleEdgeBlends = new ushort[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.cliffBlends = new ushort[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.passability = new Passability[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.visibility = new bool[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.passageWidth = new bool[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.buildability = new bool[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.tiberiumGrowability = new bool[blendTileData.mapWidth, blendTileData.mapHeight];
            blendTileData.dynamicShrubbery = new byte[blendTileData.mapWidth, blendTileData.mapHeight];
            for (int y = 0; y < blendTileData.mapHeight; y++)
            {
                for (int x = 0; x < blendTileData.mapWidth; x++)
                {
                    blendTileData.visibility[x, y] = true;
                    blendTileData.passability[x, y] = Passability.Passable;
                    blendTileData.tiles[x, y] = blendTileData.GetTile(x, y, 0);
                }
            }
            blendTileData.blendsCount = 0;
            blendTileData.cliffBlendsCount = 0;
            return blendTileData;
            
            // void *unk60;
            // void *unk61;
            // void *unk62;
            // void *unk63;
            // void *unk64;
            // void *unk65;
            // void *unk66;
            // void *unk67;
            // void *unk68;
            // void *unk69;



        }
    }
}