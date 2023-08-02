using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CNCBigFile.Core
{
    public class ManifestHeader
    {
        private interface IManifestHeader : IDisposable
        {
            bool IsBigEndian { get; set; }
            bool IsLinked { get; set; }
            ushort Version { get; set; }
            uint StreamChecksum { get; set; }
            uint AllTypesHash { get; set; }
            int AssetCount { get; set; }
            int TotalInstanceDataSize { get; set; }
            int MaxInstanceChunkSize { get; set; }
            int MaxRelocationChunkSize { get; set; }
            int MaxImportsChunkSize { get; set; }
            int AssetReferenceBufferSize { get; set; }
            int ReferenceManifestNameBufferSize { get; set; }
            int AssetNameBufferSize { get; set; }
            int SourceFileNameBufferSize { get; set; }
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ManifestHeader6Struct
        {
            [MarshalAs(UnmanagedType.I1)] public bool IsBigEndian;
            [MarshalAs(UnmanagedType.I1)] public bool IsLinked;
            public ushort Version;
            public uint StreamChecksum;
            public uint AllTypesHash;
            public int AssetCount;
            public int TotalInstanceDataSize;
            public int MaxInstanceChunkSize;
            public int MaxRelocationChunkSize;
            public int MaxImportsChunkSize;
            public int AssetReferenceBufferSize;
            public int ReferenceManifestNameBufferSize;
            public int AssetNameBufferSize;
            public int SourceFileNameBufferSize;
        }
        
        public class ManifestHeader6 : AStructWrapper<ManifestHeader6Struct>, IManifestHeader
        {
            protected new unsafe ManifestHeader6Struct* Data => (ManifestHeader6Struct*)base.Data;

            public unsafe bool IsBigEndian { get => Data->IsBigEndian; set => Data->IsBigEndian = value; }
            public unsafe bool IsLinked { get => Data->IsLinked; set => Data->IsLinked = value; }
            public unsafe ushort Version { get => Data->Version; set => Data->Version = value; }
            public unsafe uint StreamChecksum { get => Data->StreamChecksum; set => Data->StreamChecksum = value; }
            public unsafe uint AllTypesHash { get => Data->AllTypesHash; set => Data->AllTypesHash = value; }
            public unsafe int AssetCount { get => Data->AssetCount; set => Data->AssetCount = value; }
            public unsafe int TotalInstanceDataSize { get => Data->TotalInstanceDataSize; set => Data->TotalInstanceDataSize = value; }
            public unsafe int MaxInstanceChunkSize { get => Data->MaxInstanceChunkSize; set => Data->MaxInstanceChunkSize = value; }
            public unsafe int MaxRelocationChunkSize { get => Data->MaxRelocationChunkSize; set => Data->MaxRelocationChunkSize = value; }
            public unsafe int MaxImportsChunkSize { get => Data->MaxImportsChunkSize; set => Data->MaxImportsChunkSize = value; }
            public unsafe int AssetReferenceBufferSize { get => Data->AssetReferenceBufferSize; set => Data->AssetReferenceBufferSize = value; }
            public unsafe int ReferenceManifestNameBufferSize { get => Data->ReferenceManifestNameBufferSize; set => Data->ReferenceManifestNameBufferSize = value; }
            public unsafe int AssetNameBufferSize { get => Data->AssetNameBufferSize; set => Data->AssetNameBufferSize = value; }
            public unsafe int SourceFileNameBufferSize { get => Data->SourceFileNameBufferSize; set => Data->SourceFileNameBufferSize = value; }

            internal unsafe override void Swap()
            {
                Swap(&Data->Version);
                Swap(&Data->StreamChecksum);
                Swap(&Data->AllTypesHash);
                Swap(&Data->AssetCount);
                Swap(&Data->TotalInstanceDataSize);
                Swap(&Data->MaxInstanceChunkSize);
                Swap(&Data->MaxRelocationChunkSize);
                Swap(&Data->MaxImportsChunkSize);
                Swap(&Data->AssetReferenceBufferSize);
                Swap(&Data->ReferenceManifestNameBufferSize);
                Swap(&Data->AssetNameBufferSize);
                Swap(&Data->SourceFileNameBufferSize);
            }
        }
        
        public ManifestHeader6 header;

        public static ManifestHeader fromStream(Stream stream)
        {
            var manifestHeader = new ManifestHeader();
            long streamPosition = stream.Position;
            ManifestHeader6 header6 = new ManifestHeader6();
            header6.LoadFromStream(stream, false);
            manifestHeader.header = header6;
            return manifestHeader;
        }
        
        
    }
}