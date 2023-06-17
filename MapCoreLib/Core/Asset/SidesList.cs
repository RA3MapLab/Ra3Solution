using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class SidesList : MajorAsset
    {
        public List<Player> players = new List<Player>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            binaryReader.ReadByte();   // 必定是1
            var playerCount = binaryReader.ReadInt32();
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new Player().fromStream(binaryReader, context));
            }
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write((byte)1);
            binaryWriter.Write(players.Count);
            foreach (var player in players)
            {
                player.saveData(binaryWriter, context);
            }
        }

        public override void initDefault(MapDataContext context)
        {
            players.Add(Player.of(context));
            players.Add(Player.of(context, "Player_1"));
            players.Add(Player.of(context, "Player_2"));
            players.Add(Player.of(context, "Player_3"));
            players.Add(Player.of(context, "Player_4"));
            players.Add(Player.of(context, "Player_5"));
            players.Add(Player.of(context, "Player_6"));
            players.Add(Player.of(context, "PlyrCivilian"));
            players.Add(Player.of(context, "PlyrCreeps"));
            players.Add(Player.of(context, "PlyrNeutral"));
			
            players.Add(Player.of(context, "SkirmishCivilian"));
            players.Add(Player.of(context, "SkirmishRandom"));
            players.Add(Player.of(context, "SkirmishSoviet"));
            players.Add(Player.of(context, "SkirmishAllies"));
            players.Add(Player.of(context, "SkirmishJapan"));
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_SidesList;
        }

        public override short getVersion()
        {
            return 6;
        }

        public int findPlayerIndex(string playerName)
        {
            Player p = players.Find(player => (player.assetPropertyCollection.getProperty("playerName").data as string) == playerName);
            if (p == null)
            {
                return -1;
            }

            return players.IndexOf(p);
        }

        public static SidesList newInstance(MapDataContext context)
        {
            var sidesList = new SidesList();
            sidesList.name = Ra3MapConst.ASSET_SidesList;
            sidesList.id = context.MapStruct.RegisterString(sidesList.name);
            sidesList.version = sidesList.getVersion();
            sidesList.players.Add(Player.of(context));
            sidesList.players.Add(Player.of(context, "Player_1"));
            sidesList.players.Add(Player.of(context, "Player_2"));
            sidesList.players.Add(Player.of(context, "Player_3"));
            sidesList.players.Add(Player.of(context, "Player_4"));
            sidesList.players.Add(Player.of(context, "Player_5"));
            sidesList.players.Add(Player.of(context, "Player_6"));
            sidesList.players.Add(Player.of(context, "PlyrCivilian"));
            sidesList.players.Add(Player.of(context, "PlyrCreeps"));
            sidesList.players.Add(Player.of(context, "PlyrNeutral"));
            //TODO 根据不同mod的阵营添加新增加的Skirmish玩家
            sidesList.players.Add(Player.of(context, "SkirmishCivilian"));
            sidesList.players.Add(Player.of(context, "SkirmishRandom"));
            sidesList.players.Add(Player.of(context, "SkirmishSoviet"));
            sidesList.players.Add(Player.of(context, "SkirmishAllies"));
            sidesList.players.Add(Player.of(context, "SkirmishJapan"));
            sidesList.players.Add(Player.of(context, "SkirmishNull"));
            sidesList.players.Add(Player.of(context, "SkirmishObserver"));

            return sidesList;
        }
    }
}