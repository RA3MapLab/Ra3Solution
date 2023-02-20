using System.IO;

namespace MapCoreLib.Core.Asset
{
    public abstract class MajorAsset
    {
        protected int id;
        protected short version;
        protected string name;
        protected int dataSize;
        protected long dataStartPos;

        public virtual MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            id = binaryReader.ReadInt32();
            version = binaryReader.ReadInt16();
            dataSize = binaryReader.ReadInt32();
            dataStartPos = binaryReader.BaseStream.Position;
            name = context.MapStruct.findStringByIndex(id);
            parseData(binaryReader, context);
            return this;
        }

        protected virtual void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            
        }

        public virtual void Save(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(id);
            binaryWriter.Write(version);
            long pos = binaryWriter.BaseStream.Position;
            binaryWriter.Write(dataSize);
            saveData(binaryWriter, context);
            dataSize = (int)(binaryWriter.BaseStream.Position - pos - 4);
            binaryWriter.BaseStream.Position = pos;
            binaryWriter.Write(dataSize);
            binaryWriter.BaseStream.Seek(0L, SeekOrigin.End);
        }
        
        protected virtual void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            
        }

        public virtual void initDefault(MapDataContext context)
        {
            
        }


        public abstract string getAssetName();

        public virtual short getVersion()
        {
            return 0;
        }

        public void replaceBasicInfo(MajorAsset newAsset)
        {
            newAsset.id = id;
            newAsset.version = version;
        }

        /**
         * 注册id，填充id，填充version
         * <param name="mapDataContext"></param>
         */
        public virtual void registerSelf(MapDataContext context)
        {
            id = context.MapStruct.RegisterString(getAssetName());
            version = getVersion();
        }
    }
}