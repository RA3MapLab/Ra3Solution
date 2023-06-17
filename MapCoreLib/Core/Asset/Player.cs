using System;
using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class Player
    {
        
        public AssetPropertyCollection assetPropertyCollection = new AssetPropertyCollection();
        public List<BuildListItem> buildListItems = new List<BuildListItem>();
        public Player fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            assetPropertyCollection = new AssetPropertyCollection().fromStream(binaryReader, context);
            var buildListItemCount = binaryReader.ReadInt32();
            for (int i = 0; i < buildListItemCount; i++)
            {
                buildListItems.Add(new BuildListItem().fromStream(binaryReader, context));
            }
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            assetPropertyCollection.saveData(binaryWriter, context);
            binaryWriter.Write(buildListItems.Count);
            foreach (var buildListItem in buildListItems)
            {
                buildListItem.saveData(binaryWriter, context);
            }
        }

        public static Player of(MapDataContext context, string name = "")
        {
            string faction = "PlayerTemplate:";
            faction = ((!name.Contains("Skirmish")) ? (faction + "FactionCivilian") : (faction + name.Substring("Skirmish".Length)));
            AIDifficulty ai = AIDifficulty.Easy | AIDifficulty.Normal | AIDifficulty.Hard | AIDifficulty.Brutal;
            
            var player = new Player();
            player.assetPropertyCollection.addProperty("playerName", name, context);
            player.assetPropertyCollection.addProperty("playerIsHuman", false, context);
            player.assetPropertyCollection.addProperty("playerDisplayName", name == "" ? "Neutral" : name, context);
            player.assetPropertyCollection.addProperty("playerFaction", name == ""? "" : faction, context);
            player.assetPropertyCollection.addProperty("playerAllies", "", context);
            player.assetPropertyCollection.addProperty("playerEnemies", "", context);

            if (name != "")
            {
                
            }
            return player;
        }
        
        [Flags]
        public enum AIDifficulty
        {
            Easy = 0x1,
            Normal = 0x2,
            Hard = 0x4,
            Brutal = 0x8,
            Unused0 = 0x10,
            Unused1 = 0x20,
            Unused2 = 0x40,
            Unused3 = 0x80
        }
    }
}