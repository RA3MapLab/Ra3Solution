using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core.Util;

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
            public uint typeId { get; set; }

            public uint instanceId { get; set; }

            public AssetBlock fromStream(BinaryReader binaryReader, MapDataContext context)
            {
                typeId = binaryReader.ReadUInt32();
                instanceId = binaryReader.ReadUInt32();
                return this;
            }

            public void saveData(BinaryWriter binaryWriter, MapDataContext context)
            {
                binaryWriter.Write(typeId);
                binaryWriter.Write(instanceId);
            }
        }

        public void addInstance(string typeName)
        {
            var instanceId = FashHash.GetHashCode(typeName);
            if (assetBlocks.Find(block => block.instanceId == instanceId) == null)
            {
                assetBlocks.Add(new AssetBlock()
                {
                    typeId = 2486173485u,   //默认都是GameObject
                    instanceId = instanceId
                });
            }
        }
    }
}