using System.Collections.Generic;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.NewMap;

namespace MapCoreLib.Core
{
    public class Ra3MapBuilder
    {
        private Ra3Map ra3Map;
        private NewMapConfig newMapConfig;

        private List<NewMapOptionHandler> handlers = new List<NewMapOptionHandler>()
        {
            new RandomTerrainGen()
        };

        public Ra3MapBuilder(Ra3Map ra3Map, NewMapConfig newMapConfig)
        {
            this.ra3Map = ra3Map;
            this.newMapConfig = newMapConfig;
        }
        
        public void build()
        {
            var context = ra3Map.getContext();
            var mapStruct = context.MapStruct;
            buildBaseMap(mapStruct, context);
            //TODO 根据选项添加handler执行额外功能
            // foreach (var option in newMapConfig.options)
            // {
            //     handlers.Find(handler => handler.optionName() == option.Key)?.handle(context, newMapConfig);
            // }

            if (newMapConfig.enableRandomTerrainOption)
            {
                new RandomTerrainGen().handle(context, newMapConfig);
            }
        }

        private void buildBaseMap(Ra3MapStruct mapStruct, MapDataContext context)
        {
            mapStruct.addAsset(AssetList.newInstance(context));
            mapStruct.addAsset(GlobalVersion.newInstance(context));
            mapStruct.addAsset(HeightMapData.newInstance(context, newMapConfig.width, newMapConfig.height));
            mapStruct.addAsset(BlendTileData.newInstance(context, newMapConfig));
            mapStruct.addAsset(WorldInfo.newInstance(context, newMapConfig));
            mapStruct.addAsset(MPPositionList.newInstance(context));
            var sidesList = SidesList.newInstance(context);
            mapStruct.addAsset(sidesList);
            mapStruct.addAsset(LibraryMapLists.newInstance(context));
            mapStruct.addAsset(Teams.newInstance(context));
            mapStruct.addAsset(PlayerScriptsList.newInstance(context, sidesList.players.Count));
            mapStruct.addAsset(ObjectsList.newInstance(context));
            mapStruct.addAsset(StandingWaterAreas.newInstance(context, newMapConfig.width, newMapConfig.height));
            mapStruct.addAsset(GlobalWaterSettings.newInstance(context));
            mapStruct.addAsset(PostEffectsChunk.newInstance(context));
            mapStruct.addAsset(GlobalLighting.newInstance(context));
        }
    }
}