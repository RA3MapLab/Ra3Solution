using System.Collections.Generic;
using MapCoreLib.Core.NewMap;

namespace MapCoreLib.Core
{
    public class NewMapConfig
    {
        public string mapPath;
        public int width;
        public int height;
        public int border;
        public string defaultTexture = "Dirt_Yucatan03";
        public bool enableRandomTerrainOption;
        public RandomTerrainOption randomTerrainOption;
    }
}