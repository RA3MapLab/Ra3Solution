using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class DefaultMajorAsset: MajorAsset
    {
        public byte[] data;
        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            data = binaryReader.ReadBytes(dataSize);
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(data);
        }

        public override string getAssetName()
        {
            return "DefaultMajorAsset";
        }
    }
}