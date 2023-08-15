using System;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class P2AsGiles: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            var sidesList = context.getAsset<SidesList>(Ra3MapConst.ASSET_SidesList);
            sidesList.players[11].assetPropertyCollection.setProperty("aiPersonality", "AIPersonalityDefinition:2AlliedSquadronLeader");
        }
    }
}