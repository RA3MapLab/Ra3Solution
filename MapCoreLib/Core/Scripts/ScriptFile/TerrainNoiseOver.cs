using System.Drawing;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class TerrainNoiseOver: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            var heightMapData = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData);

            Bitmap bitmap = new Bitmap("noise.png");
            for (int i = 0; i < context.mapHeight; i++)
            {
                for (int j = 0; j < context.mapWidth; j++)
                {
                    if (heightMapData.elevations[j, i] == 210)
                    {
                        Color oc = bitmap.GetPixel(j, i);
                        int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                        float diff = grayScale * 1.0f / 255 * 40;
                        heightMapData.elevations[j, i] += diff;
                    }
                    
                }
            }
        }
        
        
    }
}