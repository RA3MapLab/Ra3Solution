using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class WorldInfo: MajorAsset
    {
        
        public AssetPropertyCollection properties;
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            properties = new AssetPropertyCollection().fromStream(binaryReader, context);
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            properties.saveData(binaryWriter, context);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_WorldInfo;
        }
    }
}