using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core
{
    public class Ra3MapVisitImpl
    {
        public void visit(MapDataContext context, MapListener mapListener)
        {
            var interestList = mapListener.interestedIn();
            context.MapStruct.getAssets().ForEach(asset =>
            {
                if (!interestList.Contains(asset.getAssetName()))
                {
                    return;
                }
                switch (asset.getAssetName())
                {
                    case Ra3MapConst.ASSET_ObjectsList:
                        visitObjectsList(context, asset as ObjectsList, mapListener);
                        break;
                    case Ra3MapConst.ASSET_AssetList:

                        break;
                    case Ra3MapConst.ASSET_SidesList:
                        visitSideList(context, asset as SidesList, mapListener);
                        break;
                    case Ra3MapConst.ASSET_PlayerScriptsList:
                        visitPlayerScriptsList(context, asset as PlayerScriptsList, mapListener);
                        break;
                    case Ra3MapConst.ASSET_Teams:

                        break;
                    case Ra3MapConst.ASSET_HeightMapData:

                        break;
                    case Ra3MapConst.ASSET_BlendTileData:

                        break;
                    case Ra3MapConst.ASSET_WorldInfo:

                        break;
                    default:
                        break;
                }
            });
        }

        private void visitSideList(MapDataContext context, SidesList sidesList, MapListener mapListener)
        {
            sidesList.players.ForEach(player =>
            {
                mapListener.visitPlayer(context, player);
            });
        }

        private void visitObjectsList(MapDataContext context, ObjectsList objectsList, MapListener mapListener)
        {
            mapListener.visitMO_Start(context, objectsList);
            objectsList.mapObjects.ForEach(mapObject =>
            {
                
            });
        }

        private void visitPlayerScriptsList(MapDataContext context, PlayerScriptsList playerScriptsList,
            MapListener mapListener)
        {
            for (int i = 0; i < playerScriptsList.scriptLists.Count; i++)
            {
                var scriptList = playerScriptsList.scriptLists[i];
                mapListener.visitScriptList(context, scriptList, i);
                scriptList.scriptGroups.ForEach(scriptGroup => { visitScriptGroup(context, mapListener, scriptGroup); });
                scriptList.scripts.ForEach(script => { visitScript(context, mapListener, script); });
            }
            // playerScriptsList.scriptLists.ForEach(scriptList =>
            // {
            //     mapListener.visitScriptList(context, scriptList);
            //     
            //     scriptList.scriptGroups.ForEach(scriptGroup => { visitScriptGroup(context, mapListener, scriptGroup); });
            //     
            //     scriptList.scripts.ForEach(script => { visitScript(context, mapListener, script); });
            // });
        }

        private static void visitScriptGroup(MapDataContext context, MapListener mapListener, ScriptGroup scriptGroup)
        {
            mapListener.visitScriptGroup(context, scriptGroup);

            scriptGroup.scriptGroups.ForEach(scriptGroupChild =>
            {
                visitScriptGroup(context, mapListener, scriptGroupChild);
            });

            scriptGroup.scripts.ForEach(script => { visitScript(context, mapListener, script); });
        }

        private static void visitScript(MapDataContext context, MapListener mapListener, Script script)
        {
            mapListener.visitScript(context, script);
            
            script.scriptOrConditions.ForEach(orCondition =>
            {
                orCondition.conditions.ForEach(condition =>
                {
                    mapListener.visitScriptCondition(context, condition);
                });
            });
            
            script.ScriptActionOnTrue.ForEach(action =>
            {
                mapListener.visitScriptAction(context, action);
            });
            
            script.ScriptActionOnFalse.ForEach(actionFalse =>
            {
                mapListener.visitScriptActionFalse(context, actionFalse);
            });
        }
    }
}