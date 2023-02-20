
using System.IO;

namespace Compress
{
    public static class StreamHelper
    {
        public static bool RefPackCompress(this Stream input, int length, out byte[] output, CompressionLevel level)
        {
            var data = new byte[length];
            if (input.Read(data, 0, data.Length) != data.Length)
            {
                throw new EndOfStreamException();
            }
            return Compression.Compress(data, out output, level);
        }

        public static bool RefPackCompress(this Stream input, int length, out byte[] output)
        {
            var data = new byte[length];
            if (input.Read(data, 0, data.Length) != data.Length)
            {
                throw new EndOfStreamException();
            }
            return Compression.Compress(data, out output, CompressionLevel.Max);
        }
    }
}
