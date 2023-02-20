using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core
{
    internal class Ra3MapVisitImpl2 : MapListener
    {
        private int num = 1;
        public override List<string> interestedIn()
        {
            return new List<string>()
            {
                Ra3MapConst.ASSET_SidesList,
                Ra3MapConst.ASSET_ObjectsList
            };
        }

        public override void visitPlayer(MapDataContext context, Player player)
        {
            if (num == 1)
            {
                var buildListItem = new BuildListItem();
                buildListItem.inits();
                player.buildListItems.Add(buildListItem);
                num++;
            }
            
            // var buildListItem = new BuildListItem();
            // buildListItem.inits();
            // player.buildListItems.Add(buildListItem);
        }

        public override void visitMO_Start(MapDataContext context, ObjectsList objectsList)
        {
            // objectsList.mapObjects.Add(
            //     MapObject.of("JapanEmperorsRageEffect_Small", context)
            // );
        }
    }
}