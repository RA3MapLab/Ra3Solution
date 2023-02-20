namespace Compress
{
    public static class RefpackHelper
    {
        public static bool RefPackCompress(this byte[] input, out byte[] output, CompressionLevel level)
        {
            return Compression.Compress(input, out output, level);
        }

        public static bool RefPackCompress(this byte[] input, out byte[] output)
        {
            // Stopwatch stopwatch = Stopwatch.StartNew();
            var res =  Compression.Compress(input, out output, CompressionLevel.Max);
            // LogUtil.WriteLine("Compress map in {0}ms, result: {1}", stopwatch.ElapsedMilliseconds, res);
            return res;
        }
    }
}
