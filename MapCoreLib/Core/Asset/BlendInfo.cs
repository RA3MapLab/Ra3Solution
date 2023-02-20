using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class BlendInfo
    {
        public int secondaryTextureTile;

        private uint i3;

        private uint i4;

        public BlendDirection blendDirection;

        public BlendInfo fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            secondaryTextureTile = binaryReader.ReadInt32();
            blendDirection = ToBlendDirection(binaryReader.ReadBytes(6));
            i3 = binaryReader.ReadUInt32();
            i4 = binaryReader.ReadUInt32();
            return this;
        }
        
        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(secondaryTextureTile);
            binaryWriter.Write(FromBlendDirection(blendDirection));
            binaryWriter.Write(i3);
            binaryWriter.Write(i4);
        }
        
        private BlendDirection ToBlendDirection(byte[] bytes)
        {
            int x = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 1)
                {
                    x |= 1 << i;
                }
            }
            return (BlendDirection)x;
        }
        
        public byte[] FromBlendDirection(BlendDirection bd)
        {
            byte[] bytes = new byte[6];
            for (int i = 0; i < bytes.Length; i++)
            {
                if (((uint)bd & (uint)(1 << i)) != 0)
                {
                    bytes[i] = 1;
                }
            }
            return bytes;
        }
    }
}