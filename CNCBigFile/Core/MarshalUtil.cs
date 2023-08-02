using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace CNCBigFile.Core
{
    public static class MarshalUtil
    {
        private const string _memDll = "msvcrt.dll";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf(Type type)
        {
            Debug.Assert(!(type is null), $"{nameof(type)} is null");
            return Marshal.SizeOf(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>() where T : struct
        {
            return Marshal.SizeOf<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>(T[] array) where T : struct
        {
            return array is null ? 0 : array.Length * SizeOf<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToStruct<T>(IntPtr ptr) where T : struct
        {
            return Marshal.PtrToStructure<T>(ptr);
        }

        [DllImport(_memDll, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr CopyMemoryInt(IntPtr dest, IntPtr src, ulong count);

        [Obsolete("Use CopyMemory instead.")]
        public static void CopyMemoryInt(IntPtr dest, IntPtr src, int count)
        {
            CopyMemoryInt(dest, src, (ulong)count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(IntPtr dest, IntPtr src, int count)
        {
            unsafe { Buffer.MemoryCopy((void*)src, (void*)dest, count, count); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(IntPtr dest, IntPtr src, long count)
        {
            unsafe { Buffer.MemoryCopy((void*)src, (void*)dest, count, count); }
        }

        [DllImport(_memDll, EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr ClearMemory(IntPtr ptr, byte value, ulong count);

        public static void ClearMemory(IntPtr ptr, byte value, int count)
        {
            ClearMemory(ptr, value, (ulong)count);
        }

        public static unsafe IntPtr AllocateMemory(int sizeInBytes, int align = 16)
        {
            ulong mask = (ulong)(align - 1);
            if ((align & (int)mask) != 0) throw new ArgumentException("Not power of two.", nameof(align));
            IntPtr ptr = Marshal.AllocHGlobal(sizeInBytes + (int)mask + sizeof(void*));
            byte* result = (byte*)(((ulong)ptr + (ulong)sizeof(void*) + mask) & ~mask);
            ((IntPtr*)result)[-1] = ptr;
            return new IntPtr(result);
        }

        public static unsafe IntPtr AllocateMemory(uint sizeInBytes, int align = 16)
        {
            ulong mask = (ulong)(align - 1);
            if ((align & (int)mask) != 0) throw new ArgumentException("Not power of two.", nameof(align));
            IntPtr ptr = Marshal.AllocHGlobal((IntPtr)(sizeInBytes + (int)mask + sizeof(void*)));
            byte* result = (byte*)(((ulong)ptr + (ulong)sizeof(void*) + mask) & ~mask);
            ((IntPtr*)result)[-1] = ptr;
            return new IntPtr(result);
        }

        public static IntPtr AllocateClearedMemory(int sizeInBytes, byte value = 0, int align = 16)
        {
            IntPtr result = AllocateMemory(sizeInBytes, align);
            ClearMemory(result, value, sizeInBytes);
            return result;
        }

        public static IntPtr AllocateClearedMemory(uint sizeInBytes, byte value = 0, int align = 16)
        {
            IntPtr result = AllocateMemory(sizeInBytes, align);
            ClearMemory(result, value, sizeInBytes);
            return result;
        }

        public static bool IsMemoryAligned(IntPtr ptr, int align = 16)
        {
            return (ptr.ToInt64() & (align - 1)) == 0;
        }

        public static unsafe void FreeMemory(IntPtr ptr)
        {
            Marshal.FreeHGlobal(((IntPtr*)ptr)[-1]);
        }
    }
}