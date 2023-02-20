using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class Team
    {
        public AssetPropertyCollection propertyCollection;
        
        public Team fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            propertyCollection = new AssetPropertyCollection().fromStream(binaryReader, context);
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            propertyCollection.saveData(binaryWriter, context);
        }
    }
}