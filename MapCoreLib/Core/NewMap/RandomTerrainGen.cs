using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Util;

namespace MapCoreLib.Core.NewMap
{
    public class RandomTerrainGen: NewMapOptionHandler
    {
        private float[] heightKeys = new float[5] { 0f, 0.4167f, 0.5167f, 0.6333f, 1f };
        private float[] heightVals = new float[5] { 130, 210, 280, 350f, 420f };
        private string[] heightTextures = new string[5] { "Dirt_Yucatan01", "Grass_Yucatan02", "Grass_Yucatan05", "Grass_Hawaii04", "Grass_Heidelberg05" };
        
        public void handle(MapDataContext context, NewMapConfig config)
        {
            var option = config.randomTerrainOption;
            var noiseMap = PerlineNoise.NoiseMap(context.mapWidth, context.mapHeight, option.seed, option.passCount);
            var heightMapData = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData);
            var blendTileData = context.getAsset<BlendTileData>(Ra3MapConst.ASSET_BlendTileData);
            foreach (var heightTexture in heightTextures)
            {
                blendTileData.addTexture(context, heightTexture);
            }
            for (int y = 0; y < context.mapHeight; y++)
            {
                for (int x = 0; x < context.mapWidth; x++)
                {
                    for (int i = 1; i < heightKeys.Length; i++)
                    {
                        if (noiseMap[x, y] < heightKeys[i])
                        {
                            float h = heightVals[i - 1];
                            // if (heightSmooth[i - 1] >= 0.1f)
                            // {
                            //     double r = (noiseMap[x, y] - heightKeys[i - 1]) / (heightKeys[i] - heightKeys[i - 1]);
                            //     r = ((!(r < 0.5))
                            //         ? (1.0 - Math.Pow(1.0 - r, heightSmooth[i - 1]))
                            //         : Math.Pow(r, heightSmooth[i - 1]));
                            //     h = heightVals[i - 1] + (float) r * (heightVals[i] - heightVals[i - 1]);
                            // }

                            heightMapData.elevations[x, y] = h;
                            //TODO 贴图要放到worldInfo里
                            blendTileData.tiles[x, y] = blendTileData.GetTile(x, y, i);
                            break;
                        }
                    }
                }
            }
            
            blendTileData.UpdatePassabilityMap(context);
        }

        public string optionName()
        {
            return "RandomTerrain";
        }
    }
}