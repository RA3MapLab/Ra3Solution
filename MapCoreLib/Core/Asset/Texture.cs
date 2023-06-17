using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class Texture
    {
        public int cellStart;

        public int cellCount;

        public int cellSize;

        public int magicValue;  //必定是0

        public string name;

        public Texture fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            cellStart = binaryReader.ReadInt32();
            cellCount = binaryReader.ReadInt32();
            cellSize = binaryReader.ReadInt32();
            magicValue = binaryReader.ReadInt32();
            name = binaryReader.readDefaultString();
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(cellStart);
            binaryWriter.Write(cellCount);
            binaryWriter.Write(cellSize);
            binaryWriter.Write(magicValue);
            binaryWriter.writeDefaultString(name);
        }

        public static Texture newInstance(int start, string name)
        {
            var texture = new Texture();
            texture.cellStart = start;
            texture.cellCount = 16;
            texture.cellSize = 4;
            texture.magicValue = 0;
            texture.name = name;
            return texture;
        }
    }
}