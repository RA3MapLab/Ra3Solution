using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CNCBigFile.Core
{
    public abstract class AStructWrapper<T> : IDisposable where T : struct
    {
        public static int Size { get; } = MarshalUtil.SizeOf<T>();

        private readonly bool _isDataAllocator;

        protected unsafe byte* Data;

        public unsafe AStructWrapper()
        {
            _isDataAllocator = true;
            Data = (byte*)MarshalUtil.AllocateClearedMemory(Size);
        }

        public unsafe AStructWrapper(byte* data)
        {
            _isDataAllocator = false;
            Data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(ushort* pValue)
        {
            ushort value = *pValue;
            *pValue = (ushort)((value >> 8) | (value << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(short* pValue)
        {
            ushort value = (ushort)*pValue;
            *pValue = (short)((value >> 8) | (value << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(uint* pValue)
        {
            uint value = *pValue;
            value = (value >> 16) | (value << 16);
            *pValue = ((value & 0xFF00FF00u) >> 8) | ((value & 0x00FF00FFu) << 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(int* pValue)
        {
            uint value = (uint)*pValue;
            value = (value >> 16) | (value << 16);
            *pValue = (int)(((value & 0xFF00FF00u) >> 8) | ((value & 0x00FF00FFu) << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(ulong* pValue)
        {
            ulong value = *pValue;
            value = (value >> 32) | (value << 32);
            value = ((value & 0xFFFF0000FFFF0000uL) >> 16) | ((value & 0x0000FFFF0000FFFFuL) << 16);
            *pValue = ((value & 0xFF00FF00FF00FF00uL) >> 8) | ((value & 0x00FF00FF00FF00FFuL) << 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe void Swap(long* pValue)
        {
            ulong value = (ulong)*pValue;
            value = (value >> 32) | (value << 32);
            value = ((value & 0xFFFF0000FFFF0000uL) >> 16) | ((value & 0x0000FFFF0000FFFFuL) << 16);
            *pValue = (long)(((value & 0xFF00FF00FF00FF00uL) >> 8) | ((value & 0x00FF00FF00FF00FFuL) << 8));
        }

        internal abstract void Swap();

        public virtual unsafe void LoadFromBuffer(byte[] buffer, bool isBigEndian)
        {
            fixed (byte* pBuffer = &buffer[0])
            {
                MarshalUtil.CopyMemory((IntPtr)Data, (IntPtr)pBuffer, Size);
            }
            if (isBigEndian)
            {
                Swap();
            }
        }

        public virtual unsafe void LoadFromStream(Stream stream, bool isBigEndian)
        {
            byte[] buffer = new byte[Size];
            stream.Read(buffer, 0, Size);
            LoadFromBuffer(buffer, isBigEndian);
        }

        public virtual unsafe void SaveToStream(Stream stream, bool isBigEndian)
        {
            if (isBigEndian)
            {
                Swap();
            }
            byte[] buffer = new byte[Size];
            using (Stream dataStream = new UnmanagedMemoryStream(Data, Size))
            {
                dataStream.Read(buffer, 0, Size);
            }
            stream.Write(buffer, 0, Size);
            if (isBigEndian)
            {
                Swap();
            }
        }

        protected virtual unsafe void Dispose(bool isDisposing)
        {
            if (_isDataAllocator && (IntPtr)Data != IntPtr.Zero)
            {
                MarshalUtil.FreeMemory((IntPtr)Data);
                Data = (byte*)IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AStructWrapper()
        {
            Dispose(false);
        }
    }
}