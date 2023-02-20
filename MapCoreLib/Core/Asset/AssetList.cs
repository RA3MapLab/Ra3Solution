using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class AssetList: MajorAsset
    {
        
        public List<AssetBlock> assetBlocks = new List<AssetBlock>();
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            var assetBlockCount = binaryReader.ReadInt32();
            for (int i = 0; i < assetBlockCount; i++)
            {
                assetBlocks.Add(new AssetBlock().fromStream(binaryReader, context));
            }
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(assetBlocks.Count);
            foreach (var assetBlock in assetBlocks)
            {
                assetBlock.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_AssetList;
        }
        
        public class AssetBlock
        {
            private uint type;

            private uint id;

            public AssetBlock fromStream(BinaryReader binaryReader, MapDataContext context)
            {
                type = binaryReader.ReadUInt32();
                id = binaryReader.ReadUInt32();
                return this;
            }

            public void saveData(BinaryWriter binaryWriter, MapDataContext context)
            {
                binaryWriter.Write(type);
                binaryWriter.Write(id);
            }
        }
    }
}