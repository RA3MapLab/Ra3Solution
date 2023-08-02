using System.Collections.Generic;
using System.IO;
using CNCBigFile.Core;
using Newtonsoft.Json;

namespace CNCBigFile
{
    public class WorldBuilderManifestCollector
    {
        public class manifestGameObject
        {
            public uint instanceid;
            public uint instanceHash;
        }
        
        public static void Run()
        {
            var manifest = new Manifest(Path.Combine(Directory.GetCurrentDirectory(), "mod.manifest"));
            List<manifestGameObject> manifestGameObjects = new List<manifestGameObject>();
            string res = "";
            foreach (var manifestAssetEntry in manifest.assetEntries)
            {
                if (manifestAssetEntry._assetEntry.TypeId == 2486173485u)
                {
                    manifestGameObjects.Add(new manifestGameObject()
                    {
                        instanceid = manifestAssetEntry._assetEntry.InstanceId,
                        instanceHash = manifestAssetEntry._assetEntry.InstanceHash
                    });
                    res += $"{manifestAssetEntry._assetEntry.TypeId},{manifestAssetEntry._assetEntry.InstanceId},{manifestAssetEntry._assetEntry.InstanceHash}\n";
                }
            }

            File.WriteAllText("test.txt", res);
        }
    }
}