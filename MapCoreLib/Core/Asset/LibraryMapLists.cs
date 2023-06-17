using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class LibraryMapLists: MajorAsset
    {
        public List<LibraryMaps> libraryMaps = new List<LibraryMaps>();

        protected override void saveData(BinaryWriter bw, MapDataContext context)
        {
            foreach (var libraryMap in libraryMaps)
            {
                libraryMap.Save(bw, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_LibraryMapLists;
        }

        public override short getVersion()
        {
            return 1;
        }

        public static LibraryMapLists newInstance(MapDataContext context)
        {
            var libraryMapLists = new LibraryMapLists();
            libraryMapLists.name = Ra3MapConst.ASSET_LibraryMapLists;
            libraryMapLists.id = context.MapStruct.RegisterString(libraryMapLists.name);
            libraryMapLists.version = libraryMapLists.getVersion();
            return libraryMapLists;
        }
    }
}