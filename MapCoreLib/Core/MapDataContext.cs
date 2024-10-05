using System;
using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core
{
    public class MapDataContext
    {
        public Ra3MapStruct MapStruct { get; }
        public Dictionary<object, object> extra { get; }

        public int mapWidth = Int32.MinValue;
        public int mapHeight = Int32.MinValue;
        public int border = Int32.MinValue;
        public string mapName = "";

        // public SidesList sideList
        // {
        //     get
        //     {
        //         MapStruct.getAssets().Find(asset =>
        //         {
        //             asset.getName().Equals(Ra3MapConst.ASSET_SidesList);
        //         })
        //         return null;
        //     }
        // }
        
        public T getAsset<T>(string name) where T : MajorAsset
        {
            var res = MapStruct.getAssets().Find(asset =>
            {
                return asset.getAssetName().Equals(name);
            });
            return (T)res;
        }
        
        public T? getAsset<T>() where T : MajorAsset
        {
            var res = MapStruct.getAssets().Find(asset => asset is T);
            return (T?)res;
        }
        
        public MapDataContext(Ra3MapStruct mapStruct)
        {
            MapStruct = mapStruct;
        }
    }
}