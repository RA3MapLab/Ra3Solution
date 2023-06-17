using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    
    public class StandingWaterAreas: MajorAsset
    {
        
        public List<StandingWaterArea> areas = new List<StandingWaterArea>();
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            var length = binaryReader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                areas.Add(new StandingWaterArea().fromStream(binaryReader, context));
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(areas.Count);
            foreach (var standingWaterArea in areas)
            {
                standingWaterArea.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_StandingWaterAreas;
        }

        public override short getVersion()
        {
            return 2;
        }

        public static StandingWaterAreas newInstance(MapDataContext context, int width, int height)
        {
            var standingWaterAreas = new StandingWaterAreas();
            standingWaterAreas.name = Ra3MapConst.ASSET_StandingWaterAreas;
            standingWaterAreas.id = context.MapStruct.RegisterString(standingWaterAreas.name);
            standingWaterAreas.version = standingWaterAreas.getVersion();
            standingWaterAreas.areas.Add(StandingWaterArea.newInstance(context, width, height));
            return standingWaterAreas;
        }
    }
}