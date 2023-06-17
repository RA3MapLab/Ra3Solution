using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class GlobalVersion: MajorAsset
    {
        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            base.saveData(binaryWriter, context);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_GlobalVersion;
        }

        public override short getVersion()
        {
            return 1;
        }

        public static GlobalVersion newInstance(MapDataContext context)
        {
            var globalVersion = new GlobalVersion();
            globalVersion.name = Ra3MapConst.ASSET_GlobalVersion;
            globalVersion.id = context.MapStruct.RegisterString(globalVersion.name);
            globalVersion.version = globalVersion.getVersion();
            return globalVersion;
        }
    }
}