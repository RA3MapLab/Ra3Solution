using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class NamedCameras: MajorAsset
    {
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_NamedCameras;
        }
    }
}