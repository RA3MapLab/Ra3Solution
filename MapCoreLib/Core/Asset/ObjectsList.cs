using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class ObjectsList: MajorAsset
    {
        public List<MapObject> mapObjects = new List<MapObject>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            while (binaryReader.BaseStream.Position - dataStartPos < dataSize)
            {
                mapObjects.Add((MapObject) new MapObject().fromStream(binaryReader, context));
            }

            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            foreach (var mapObject in mapObjects)
            {
                mapObject.Save(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_ObjectsList;
        }
    }
}