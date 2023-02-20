using System.IO;

namespace MapCoreLib.Core.Asset
{
    
    public class StandingWaterAreas: MajorAsset
    {
        
        public StandingWaterArea[] areas;
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            areas = new StandingWaterArea[binaryReader.ReadInt32()];
            for (int i = 0; i < areas.Length; i++)
            {
                areas[i] = new StandingWaterArea().fromStream(binaryReader, context);
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(areas.Length);
            foreach (var standingWaterArea in areas)
            {
                standingWaterArea.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_StandingWaterAreas;
        }
    }
}