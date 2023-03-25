using System;
using System.Text;

namespace MapCoreLib.Core.Util
{
    public class FashHash
    {
        public unsafe static uint GetHashCode(string content)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(content.ToLower());
            fixed (byte* pBuffer = &buffer[0])
            {
                return FastHashFn((uint)buffer.Length, pBuffer, (uint)buffer.Length);
            }
        }
        private unsafe static uint FastHashFn(uint hash, byte* pBuffer, uint length)
        {
            if (length == 0 || (IntPtr)pBuffer == IntPtr.Zero)
            {
                return 0u;
            }
            uint num = length & 3u;
            for (length >>= 2; length != 0; length--)
            {
                hash += *(ushort*)pBuffer;
                hash ^= (*(ushort*)(pBuffer + 2) ^ (hash << 5)) << 11;
                hash += hash >> 11;
                pBuffer += 4;
            }
            switch (num)
            {
                case 1u:
                    hash += *pBuffer;
                    hash ^= hash << 10;
                    hash += hash >> 1;
                    break;
                case 2u:
                    hash += *(ushort*)pBuffer;
                    hash ^= hash << 11;
                    hash += hash >> 17;
                    break;
                case 3u:
                    hash += *(ushort*)pBuffer;
                    hash ^= hash << 16;
                    hash ^= (uint)(pBuffer[2] << 18);
                    hash += hash >> 11;
                    break;
            }
            hash ^= hash << 3;
            hash += hash >> 5;
            hash ^= hash << 2;
            hash += hash >> 15;
            hash ^= hash << 10;
            return hash;
        }
    }
}