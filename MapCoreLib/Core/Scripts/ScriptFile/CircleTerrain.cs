using System;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class CircleTerrain: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            var heightMapData = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData);
            for (int i = 0; i < context.mapHeight; i++)
            {
                for (int j = 0; j < context.mapWidth; j++)
                {
                    if (Math.Sqrt(Math.Pow(i - context.mapHeight / 2,2) + Math.Pow(j - context.mapWidth / 2,2)) < 100)
                    {
                        heightMapData.elevations[j, i] = 210;
                    }
                    else
                    {
                        heightMapData.elevations[j, i] = 130;
                    }
                }
            }
        }
    }
}