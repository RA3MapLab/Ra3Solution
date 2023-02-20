using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class HeightMapBorder
    {
        public int Corner1X;
        public int Corner1Y;

        public int Corner2X;
        public int Corner2Y;

        public HeightMapBorder fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            Corner1X = binaryReader.ReadInt32();
            Corner1Y = binaryReader.ReadInt32();
            Corner2X = binaryReader.ReadInt32();
            Corner2Y = binaryReader.ReadInt32();
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(Corner1X);
            binaryWriter.Write(Corner1Y);
            binaryWriter.Write(Corner2X);
            binaryWriter.Write(Corner2Y);
        }
    }
}