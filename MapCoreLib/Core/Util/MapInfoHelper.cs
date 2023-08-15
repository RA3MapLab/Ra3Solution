using System;
using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Util
{
    public class MapInfoHelper
    {
        //可能会没有出生点
        public static List<Position> getStartPos(string mapPath)
        {
            var ra3Map = new Ra3Map(mapPath);
            ra3Map.parse();
            List<Position> startPos = new List<Position>();
            var objectsList = ra3Map.getAsset<ObjectsList>(Ra3MapConst.ASSET_ObjectsList);
            objectsList.mapObjects.ForEach(obj =>
            {
                string uniqueId = obj.assetPropertyCollection.getProperty("uniqueID").data.ToString();
                if (Ra3MapConst.startPos.Contains(uniqueId))
                {
                    //add obj.position to startPos
                    startPos.Add(new Position()
                    {
                        x = (int)(obj.position.x / 10),
                        y = (int)(obj.position.y / 10)
                    });
                }
            });
            return startPos;
        }
        
        public static List<Position> getStartPos(Ra3Map ra3Map)
        {
            List<Position> startPos = new List<Position>()
            {
                Position.InvalidPosition(),
                Position.InvalidPosition(),
                Position.InvalidPosition(),
                Position.InvalidPosition(),
                Position.InvalidPosition(),
                Position.InvalidPosition(),
            };
            var objectsList = ra3Map.getAsset<ObjectsList>(Ra3MapConst.ASSET_ObjectsList);
            objectsList.mapObjects.ForEach(obj =>
            {
                string uniqueId = obj.assetPropertyCollection.getProperty("uniqueID").data.ToString();
                if (Ra3MapConst.startPos.Contains(uniqueId))
                {
                    var index = Int32.Parse(uniqueId.Split('_')[1]) - 1;
                    startPos[index] = new Position()
                    {
                        x = (int)(obj.position.x / 10),
                        y = (int)(obj.position.y / 10)
                    };
                }
            });
            return startPos;
        }
        
        
    }
    
    public class MapInfo {
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public List<Position> StartPos { get; set; }
        public bool Valid { get; set; }
        public int maxPlayerCount { get; set; }
    }

    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public static Position InvalidPosition()
        {
            return new Position()
            {
                x = Int32.MinValue,
                y = Int32.MinValue
            };
        }
    }
}