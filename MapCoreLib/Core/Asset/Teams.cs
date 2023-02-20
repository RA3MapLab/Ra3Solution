using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class Teams: MajorAsset
    {
        public List<Team> teamList = new List<Team>();

        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            var count = binaryReader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                teamList.Add(new Team().fromStream(binaryReader, context));
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(teamList.Count);
            foreach (var team in teamList)
            {
                team.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_Teams;
        }
    }
}