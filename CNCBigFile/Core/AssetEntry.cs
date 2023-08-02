using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CNCBigFile.Core
{
    public class AssetEntry
    {
        private interface IAssetEntry : IDisposable
        {
            uint TypeId { get; set; }
            uint InstanceId { get; set; }
            uint TypeHash { get; set; }
            uint InstanceHash { get; set; }
            uint AssetReferenceOffset { get; set; }
            int AssetReferenceCount { get; set; }
            uint AssetNameOffset { get; set; }
            uint SourceFileOffset { get; set; }
            int InstanceDataSize { get; set; }
            int RelocationsDataSize { get; set; }
            int ImportsDataSize { get; set; }
            bool IsTokenized { get; set; }
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AssetEntry7Struct
        {
            public uint TypeId;
            public uint InstanceId;
            public uint TypeHash;
            public uint InstanceHash;
            public uint AssetReferenceOffset;
            public int AssetReferenceCount;
            public uint AssetNameOffset;
            public uint SourceFileOffset;
            public int InstanceDataSize;
            public int RelocationsDataSize;
            public int ImportsDataSize;
            [MarshalAs(UnmanagedType.Bool)] public bool IsTokenized;
        }
        
        public class AssetEntry7 : AStructWrapper<AssetEntry7Struct>, IAssetEntry
        {
            protected new unsafe AssetEntry7Struct* Data => (AssetEntry7Struct*)base.Data;

            public unsafe uint TypeId { get => Data->TypeId; set => Data->TypeId = value; }
            public unsafe uint InstanceId { get => Data->InstanceId; set => Data->InstanceId = value; }
            public unsafe uint TypeHash { get => Data->TypeHash; set => Data->TypeHash = value; }
            public unsafe uint InstanceHash { get => Data->InstanceHash; set => Data->InstanceHash = value; }
            public unsafe uint AssetReferenceOffset { get => Data->AssetReferenceOffset; set => Data->AssetReferenceOffset = value; }
            public unsafe int AssetReferenceCount { get => Data->AssetReferenceCount; set => Data->AssetReferenceCount = value; }
            public unsafe uint AssetNameOffset { get => Data->AssetNameOffset; set => Data->AssetNameOffset = value; }
            public unsafe uint SourceFileOffset { get => Data->SourceFileOffset; set => Data->SourceFileOffset = value; }
            public unsafe int InstanceDataSize { get => Data->InstanceDataSize; set => Data->InstanceDataSize = value; }
            public unsafe int RelocationsDataSize { get => Data->RelocationsDataSize; set => Data->RelocationsDataSize = value; }
            public unsafe int ImportsDataSize { get => Data->ImportsDataSize; set => Data->ImportsDataSize = value; }
            public unsafe bool IsTokenized { get => Data->IsTokenized; set => Data->IsTokenized = value; }

            internal unsafe override void Swap()
            {
                Swap(&Data->TypeId);
                Swap(&Data->InstanceId);
                Swap(&Data->TypeHash);
                Swap(&Data->InstanceHash);
                Swap(&Data->AssetReferenceOffset);
                Swap(&Data->AssetReferenceCount);
                Swap(&Data->AssetNameOffset);
                Swap(&Data->SourceFileOffset);
                Swap(&Data->InstanceDataSize);
                Swap(&Data->RelocationsDataSize);
                Swap(&Data->ImportsDataSize);
            }
        }

        public AssetEntry7 _assetEntry;

        public static AssetEntry fromStream(Stream stream)
        {
            var assetEntry = new AssetEntry();
            AssetEntry7 assetEntry7 = new AssetEntry7();
            assetEntry7.LoadFromStream(stream, false);
            assetEntry._assetEntry = assetEntry7;
            return assetEntry;
        }
    }
}