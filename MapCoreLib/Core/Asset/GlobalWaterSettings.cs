using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class GlobalWaterSettings: MajorAsset
    {
        public bool reflection = true;

        public float reflectionPlaneZ = 200;

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(reflection ? 1 : 0);
            binaryWriter.Write(reflectionPlaneZ);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_GlobalWaterSettings;
        }

        public override short getVersion()
        {
            return 1;
        }

        public static GlobalWaterSettings newInstance(MapDataContext context)
        {
            var globalWaterSettings = new GlobalWaterSettings();
            globalWaterSettings.name = Ra3MapConst.ASSET_GlobalWaterSettings;
            globalWaterSettings.id = context.MapStruct.RegisterString(globalWaterSettings.name);
            globalWaterSettings.version = globalWaterSettings.getVersion();
            return globalWaterSettings;
        }
    }
}