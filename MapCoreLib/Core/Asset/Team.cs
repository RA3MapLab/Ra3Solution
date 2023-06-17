using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class Team
    {
        public AssetPropertyCollection propertyCollection = new AssetPropertyCollection();
        
        public Team fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            propertyCollection = new AssetPropertyCollection().fromStream(binaryReader, context);
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            propertyCollection.saveData(binaryWriter, context);
        }

        public static Team of(MapDataContext context, string teamName, string ownerName)
        {
            var team = new Team();
            team.propertyCollection.addProperty("teamName", teamName, context);
            team.propertyCollection.addProperty("teamOwner", ownerName, context);
            team.propertyCollection.addProperty("teamIsSingleton", true, context);
            return team;
        }
    }
}