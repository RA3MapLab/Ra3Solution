using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core.Asset;
using MapCoreLib.Util;

namespace MapCoreLib.Core
{
    /**
     * 应该把这结构传给每个MajorAsset
     */
    public class Ra3MapStruct
    {
        private Dictionary<string, int> stringPool = new Dictionary<string, int>();

        private List<MajorAsset> assets = new List<MajorAsset>();

        public int RegisterString(string str)
        {
            if (stringPool.ContainsKey(str))
            {
                return stringPool[str];
            }
            return RegisterString(str, stringPool.Count);
        }
        
        public int RegisterString(string str, int index)
        {
            stringPool.AddIfAbsent(str, index);
            return index;
        }

        public int getStringIndex(string str)
        {
            if (stringPool.ContainsKey(str))
            {
                return stringPool[str];
            }

            return -1;
        }

        public string findStringByIndex(int index)
        {
            var resList = stringPool.Where(pair => pair.Value == index).ToList();
            if (resList.Count > 0)
            {
                return resList[0].Key;
            }

            return null;
        }

        public void addAsset(MajorAsset asset)
        {
            assets.Add(asset);
        }

        public List<MajorAsset> getAssets()
        {
            return assets;
        }

        public void save(BinaryWriter bw, MapDataContext context)
        {
            bw.Write(stringPool.Count);
            foreach (var pair in stringPool)
            {
                bw.Write(pair.Key);
                bw.Write(pair.Value);
            }
            
            assets.ForEach(asset =>
            {
                asset.Save(bw, context);
            });
        }

        public void replaceAsset(string name, MajorAsset newAsset, MapDataContext context)
        {
            
            var majorAsset = assets.Find(asset => asset.getAssetName() == name);
            newAsset.registerSelf(context);
            var index = assets.IndexOf(majorAsset);
            assets.Remove(majorAsset);
            assets.Insert(index, newAsset);
        }
    }
}