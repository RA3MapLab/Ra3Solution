using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class ScriptArgument
    {
        public int argumentType = 0;
        public Vec3D position = new Vec3D(0f, 0f, 0f);

        public int intValue = 0;
        public float floatValue = 0f;
        public string stringValue = "";
        
        public ScriptArgument fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            argumentType = (int) binaryReader.ReadUInt32();
            if (argumentType == 16)
            {
                position = binaryReader.readVec3D();
            } else
            {
                intValue = binaryReader.ReadInt32();
                floatValue = binaryReader.ReadSingle();
                stringValue = binaryReader.readDefaultString();
            }
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(argumentType);
            if (argumentType == 16)
            {
                binaryWriter.Write(position.x);
                binaryWriter.Write(position.y);
                binaryWriter.Write(position.z);
            }
            else
            {
                binaryWriter.Write(intValue);
                binaryWriter.Write(floatValue);
                binaryWriter.writeDefaultString(stringValue);
            }
        }
    }
}