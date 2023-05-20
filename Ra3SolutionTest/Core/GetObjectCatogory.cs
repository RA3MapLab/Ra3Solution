using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCoreLib.Core;
using MapCoreLib.Core.Asset;
using Newtonsoft.Json;

namespace NewMapParser.Core
{
    public class GetObjectCatogory
    {
        public static void Run()
        {
            List<ObjectCategory> list = new List<ObjectCategory>();
            foreach (var mapFile in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "obj"), "*.map", SearchOption.AllDirectories))
            {
                var objectCategory = new ObjectCategory()
                {
                    englishName = Path.GetFileNameWithoutExtension(mapFile),
                    chineseName = Path.GetFileNameWithoutExtension(mapFile),
                    subObjects = new List<string>()
                };

                var ra3Map = new Ra3Map(mapFile);
                ra3Map.parse();
                var objectsList = ra3Map.getAsset<ObjectsList>(Ra3MapConst.ASSET_ObjectsList);
                foreach (var mapObj in objectsList.mapObjects)
                {
                    objectCategory.subObjects.Add(mapObj.typeName);
                }

                objectCategory.subObjects = objectCategory.subObjects.Distinct().OrderBy(q => q).ToList();
                list.Add(objectCategory);
            }
            File.WriteAllText("ObjectCategory.json", JsonConvert.SerializeObject(list));
        }

        class ObjectCategory
        {
            public string englishName { get; set; }
            public string chineseName { get; set; }
            public List<string> subObjects { get; set; }
        }
    }
}