using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class MPPositionInfo: MajorAsset
    {
        //TODO 这个需要解解析看一下情况
        
        public bool isHuman;

        public bool isComputer;

        public bool loadAIScript;

        public uint team;

        public string[] sideRestriction;

        protected override void parseData(BinaryReader br, MapDataContext context)
        {
            isHuman = br.ReadBoolean();
            isComputer = br.ReadBoolean();
            loadAIScript = br.ReadBoolean();
            team = br.ReadUInt32();
            sideRestriction = new string[br.ReadInt32()];
            for (int i = 0; i < sideRestriction.Length; i++)
            {
                sideRestriction[i] = br.readDefaultString();
            }
        }

        protected override void saveData(BinaryWriter bw, MapDataContext context)
        {
            bw.Write(isHuman);
            bw.Write(isComputer);
            bw.Write(loadAIScript);
            bw.Write(team);
            bw.Write(sideRestriction.Length);
            for (int i = 0; i < sideRestriction.Length; i++)
            {
                bw.writeDefaultString(sideRestriction[i]);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_MPPositionInfo;
        }

        public override short getVersion()
        {
            return 0;
        }

        public static MPPositionInfo newInstance(MapDataContext context)
        {
            var mpPositionInfo = new MPPositionInfo();
            mpPositionInfo.name = Ra3MapConst.ASSET_MPPositionInfo;
            mpPositionInfo.id = context.MapStruct.RegisterString(mpPositionInfo.name);
            mpPositionInfo.isHuman = (mpPositionInfo.isComputer = (mpPositionInfo.loadAIScript = true));
            mpPositionInfo.team = uint.MaxValue;
            mpPositionInfo.sideRestriction = new string[0];
            return mpPositionInfo;
        }
    }
}