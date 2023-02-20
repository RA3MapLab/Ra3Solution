using System;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class MapObject: MajorAsset
    {
        public Vec3D position;
        public float angle;
        public int roadOption;
        public string typeName;
        public AssetPropertyCollection assetPropertyCollection = new AssetPropertyCollection();

        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            position = binaryReader.readVec3D();
            angle = binaryReader.ReadSingle() * 180f / (float)Math.PI;
            roadOption = binaryReader.ReadInt32();
            typeName = binaryReader.readDefaultString();
            assetPropertyCollection = new AssetPropertyCollection().fromStream(binaryReader, context);
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.writeVec3D(position);
            binaryWriter.Write(angle * (float)Math.PI / 180f);
            binaryWriter.Write(roadOption);
            binaryWriter.writeDefaultString(typeName);
            assetPropertyCollection.saveData(binaryWriter, context);
        }

        public static MapObject of(string typeName, MapDataContext context)
        {
            var mapObject = new MapObject();
            mapObject.position = new Vec3D(0f, 0f, 0f);
            mapObject.angle = 0f;
            mapObject.roadOption = 0;
            mapObject.typeName = typeName;
            mapObject.assetPropertyCollection.addProperty("originalOwner", "PlyrNeutral/teamPlyrNeutral", context);
            mapObject.assetPropertyCollection.addProperty("uniqueID", typeName + " " + 12312, context);
            return mapObject;
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_MapObject;
        }
    }
}