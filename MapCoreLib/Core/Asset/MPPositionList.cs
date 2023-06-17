using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class MPPositionList: MajorAsset
    {
        public MPPositionInfo[] positionInfo;

        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            positionInfo = new MPPositionInfo[6];
            for (int i = 0; i < 6; i++)
            {
                positionInfo[i] = new MPPositionInfo().fromStream(binaryReader, context) as MPPositionInfo;
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_MPPositionList;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            base.saveData(binaryWriter, context);
        }

        public override short getVersion()
        {
            return base.getVersion();
        }

        public static MPPositionList newInstance(MapDataContext context)
        {
            var mpPositionList = new MPPositionList();
            mpPositionList.name = Ra3MapConst.ASSET_MPPositionList;
            mpPositionList.id = context.MapStruct.RegisterString(mpPositionList.name);
            mpPositionList.version = mpPositionList.getVersion();
            mpPositionList.positionInfo = new MPPositionInfo[6];
            for (int i = 0; i < mpPositionList.positionInfo.Length; i++)
            {
                mpPositionList.positionInfo[i] = MPPositionInfo.newInstance(context);
            }
            return mpPositionList;
        }
    }
}