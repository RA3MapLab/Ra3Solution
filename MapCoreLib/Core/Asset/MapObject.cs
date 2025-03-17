using System;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class MapObject: MajorAsset
    {
        public Vec3D position;
        /**
         * 角度
         */
        public float angle;
        public int roadOption;
        public string typeName;
        public AssetPropertyCollection assetPropertyCollection = new AssetPropertyCollection();
        
        public string? originalOwner => assetPropertyCollection.getProperty("originalOwner")?.data.ToString();
        public string? uniqueID => assetPropertyCollection.getProperty("uniqueID")?.data.ToString();

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

        public override short getVersion()
        {
            return 3;
        }

        public static MapObject ofObj(string typeName, Vec3D vec3D, float angle, string objName, MapDataContext context)
        {
            var mapObject = new MapObject();
            mapObject.registerSelf(context);
            mapObject.position = vec3D;
            mapObject.angle = angle;
            mapObject.roadOption = 0;
            mapObject.typeName = typeName;
            mapObject.assetPropertyCollection.addProperty("originalOwner", "PlyrNeutral/teamPlyrNeutral", context);
            mapObject.assetPropertyCollection.addProperty("objectName", objName, context);

            mapObject.assetPropertyCollection.addProperty("objectInitialHealth", 100, context);
            mapObject.assetPropertyCollection.addProperty("objectEnabled", true, context);
            mapObject.assetPropertyCollection.addProperty("objectIndestructible", false, context);
            mapObject.assetPropertyCollection.addProperty("objectUnsellable", false, context);
            mapObject.assetPropertyCollection.addProperty("objectPowered", true, context);
            mapObject.assetPropertyCollection.addProperty("objectRecruitableAI", true, context);
            mapObject.assetPropertyCollection.addProperty("objectTargetable", false, context);
            mapObject.assetPropertyCollection.addProperty("objectSleeping", false, context);
            mapObject.assetPropertyCollection.addProperty("objectBasePriority", 40, context);
            mapObject.assetPropertyCollection.addProperty("objectBasePhase", 1, context);
            mapObject.assetPropertyCollection.addProperty("objectLayer", "", context);
            mapObject.assetPropertyCollection.addProperty("objectInitialStance", 0, context);
            mapObject.assetPropertyCollection.addProperty("objectExperienceLevel", 1, context);

            return mapObject;
        }

        public static MapObject ofWaypoint(Vec3D vec3D, MapDataContext context)
        {
            var mapObject = new MapObject();
            mapObject.registerSelf(context);
            mapObject.position = vec3D;
            mapObject.angle = 0;
            mapObject.roadOption = 0;
            mapObject.typeName = "*Waypoints/Waypoint";
            mapObject.assetPropertyCollection.addProperty("originalOwner", "/team", context);

            mapObject.assetPropertyCollection.addProperty("objectInitialHealth", 100, context);
            mapObject.assetPropertyCollection.addProperty("objectEnabled", true, context);
            mapObject.assetPropertyCollection.addProperty("objectIndestructible", false, context);
            mapObject.assetPropertyCollection.addProperty("objectUnsellable", false, context);
            mapObject.assetPropertyCollection.addProperty("objectPowered", true, context);
            mapObject.assetPropertyCollection.addProperty("objectRecruitableAI", true, context);
            mapObject.assetPropertyCollection.addProperty("objectTargetable", false, context);
            mapObject.assetPropertyCollection.addProperty("objectSleeping", false, context);
            mapObject.assetPropertyCollection.addProperty("objectBasePriority", 40, context);
            mapObject.assetPropertyCollection.addProperty("objectBasePhase", 1, context);
            mapObject.assetPropertyCollection.addProperty("objectLayer", "", context);

            return mapObject;
        }
    }
}