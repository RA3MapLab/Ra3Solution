using System.Collections.Generic;
using System.IO;
using MapCoreLib.Util;

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
        
        public void AddObject(MapDataContext context, string typeName, Vec3D pos, float angle, string objName = "")
        {
            MapObject o = MapObject.ofObj(typeName, pos, angle, objName, context);
            o.assetPropertyCollection.addProperty("uniqueID", typeName + mapObjects.Count + 1000, context);
            mapObjects.Add(o);
            var assetList = context.getAsset<AssetList>(Ra3MapConst.ASSET_AssetList);
            assetList.addInstance(typeName);
        }
        
        public void AddWaypoint(MapDataContext context, string name, Vec3D pos)
        {
            MapObject o = MapObject.ofWaypoint(pos, context);
            o.assetPropertyCollection.addProperty("uniqueID", name, context);
            o.assetPropertyCollection.addProperty("waypointID", mapObjects.Count + 1000, context);
            o.assetPropertyCollection.addProperty("waypointName", name, context);
            o.assetPropertyCollection.addProperty("waypointTypeOption", "", context);
            mapObjects.Add(o);
            var assetList = context.getAsset<AssetList>(Ra3MapConst.ASSET_AssetList);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_ObjectsList;
        }

        public override short getVersion()
        {
            return 3;
        }

        public static ObjectsList newInstance(MapDataContext context)
        {
            var objectsList = new ObjectsList();
            objectsList.name = Ra3MapConst.ASSET_ObjectsList;
            objectsList.id = context.MapStruct.RegisterString(objectsList.name);
            objectsList.version = objectsList.getVersion();
            return objectsList;
        }
    }
}