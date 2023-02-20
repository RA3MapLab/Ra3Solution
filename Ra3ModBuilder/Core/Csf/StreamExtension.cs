using System.IO;
using System.Text;

namespace Ra3ModBuilder.Core
{
    public static class StreamExtension
    {

        public static Encoding ansiEncoding = Encoding.GetEncoding(1252);
        public static string readUInt32PrefixedAsciiString(this BinaryReader reader)
        {
            var length = reader.ReadUInt32();
            return ansiEncoding.GetString(reader.ReadBytes((int) length));
        }
        
        public static string readUInt32PrefixedNegatedUnicodeString(this BinaryReader reader)
        {
            var length = reader.ReadUInt32();
            var bytes = reader.ReadBytes((int) length * 2);
            var negatedBytes = new byte[bytes.Length];
            for (var i = 0; i < negatedBytes.Length; i++)
            {
                negatedBytes[i] = (byte) ~bytes[i];
            }
            return Encoding.Unicode.GetString(negatedBytes);
        }
        
        public static void writeUInt32PrefixedAsciiString(this BinaryWriter writer, string str)
        {
            writer.Write(str.Length);
            writer.Write(ansiEncoding.GetBytes(str));
        }
        
        public static void writeUInt32PrefixedNegatedUnicodeString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.Unicode.GetBytes(str);
            var negatedBytes = new byte[bytes.Length];
            for (var i = 0; i < negatedBytes.Length; i++)
            {
                negatedBytes[i] = (byte) ~bytes[i];
            }
            writer.Write(bytes.Length / 2);
            writer.Write(negatedBytes);
        }
    }
}