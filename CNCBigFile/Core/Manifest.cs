using System.Collections.Generic;
using System.IO;

namespace CNCBigFile.Core
{
    public class Manifest
    {
        public ManifestHeader manifestHeader;
        public List<AssetEntry> assetEntries = new List<AssetEntry>();

        public Manifest(string path)
        {
            //open file as stream and read header
            using (var stream = File.OpenRead(path))
            {
                manifestHeader = ManifestHeader.fromStream(stream);
                for (int i = 0; i < manifestHeader.header.AssetCount; i++)
                {
                    assetEntries.Add(AssetEntry.fromStream(stream));
                }
            }
        }
    }
}