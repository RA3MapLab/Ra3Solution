namespace MapCoreLib.Core.Util
{
    public static class FNVHash
    {
        const uint FNV_PRIME_32 = 0x1000193u;
        const uint FNV_OFFSET_32 = 0x811C9DC5u;
        
        public static uint FNV_1_Hash(string input)
        {
            uint hash = FNV_OFFSET_32;
            for (var i = 0; i < input.Length; i++)
            {
                hash *= FNV_PRIME_32;
                hash ^= input[i];
            }
            return hash;
        }
        
        
    }
}