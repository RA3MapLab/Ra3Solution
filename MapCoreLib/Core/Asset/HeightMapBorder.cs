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

        public static HeightMapBorder newInstance(int Corner1X, int Corner1Y, int Corner2X, int Corner2Y)
        {
            var heightMapBorder = new HeightMapBorder();
            heightMapBorder.Corner1X = Corner1X;
            heightMapBorder.Corner1Y = Corner1Y;
            heightMapBorder.Corner2X = Corner2X;
            heightMapBorder.Corner2Y = Corner2Y;
            return heightMapBorder;
        }
    }
}