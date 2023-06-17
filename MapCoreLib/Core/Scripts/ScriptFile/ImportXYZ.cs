using System.IO;
using System.Linq;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class ImportXYZ: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            var heightMapData = context.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData);
            foreach (var line in File.ReadLines("heightmap.xyz"))
            {
                var numbers = line.Split(' ');
                heightMapData.elevations[int.Parse(numbers[0]), int.Parse(numbers[1])] = float.Parse(numbers[2]);
            }
        }
    }
}