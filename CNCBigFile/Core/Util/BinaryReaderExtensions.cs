using System;
using System.IO;
using System.Text;

namespace CNCBigFile.Core.Util
{
    public static class BinaryReaderExtensions
    {
        public static string ReadFourCc(this BinaryReader reader, bool bigEndian = false)
        {
            var a = (char) reader.ReadByte();
            var b = (char) reader.ReadByte();
            var c = (char) reader.ReadByte();
            var d = (char) reader.ReadByte();

            return bigEndian
                ? new string(new[] { d, c, b, a })
                : new string(new[] { a, b, c, d });
        }
        
        public static int ReadBigEndianInt32(this BinaryReader reader)
        {
            var array = reader.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array);
            return BitConverter.ToInt32(array, 0);
        }
        
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var sb = new StringBuilder();

            char c;
            while ((c = reader.ReadChar()) != '\0')
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}