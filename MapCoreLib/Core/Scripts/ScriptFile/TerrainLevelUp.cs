using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class TerrainLevelUp: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            var heightMapData = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData);
            float min = float.MaxValue;
            for (int i = 0; i < context.mapHeight; i++)
            {
                for (int j = 0; j < context.mapWidth; j++)
                {
                    if (heightMapData.elevations[j, i] - min < 0.0001)
                    {
                        min = heightMapData.elevations[j, i];
                    }
                }
            }
            var diff = 210 - min;
            for (int i = 0; i < context.mapHeight; i++)
            {
                for (int j = 0; j < context.mapWidth; j++)
                {
                    heightMapData.elevations[j, i] += diff;
                }
            }
        }
    }
}