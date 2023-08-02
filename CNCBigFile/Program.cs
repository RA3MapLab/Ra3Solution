using System.Collections.Generic;
using System.IO;
using CNCBigFile.Core;

namespace CNCBigFile
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var manifest = new Manifest(Path.Combine(Directory.GetCurrentDirectory(), "Worldbuilder.manifest"));
            List<AssetEntry> assetEntries = new List<AssetEntry>();
            foreach (var manifestAssetEntry in manifest.assetEntries)
            {
                if (manifestAssetEntry._assetEntry.TypeId == 2486173485u)
                {
                    assetEntries.Add(manifestAssetEntry);
                }
            }
            var a = 2;
        }
    }
}