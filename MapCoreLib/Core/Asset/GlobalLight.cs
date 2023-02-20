using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class GlobalLight
    {
        public Vec3D Ambient;
        public Vec3D Color;
        public Vec3D Direction;

        public GlobalLight fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            Ambient = binaryReader.readVec3D();
            Color = binaryReader.readVec3D();
            Direction = binaryReader.readVec3D();
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.writeVec3D(Ambient);
            binaryWriter.writeVec3D(Color);
            binaryWriter.writeVec3D(Direction);
        }
    }
}