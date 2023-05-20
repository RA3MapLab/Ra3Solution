using System.Linq;

namespace CNCBigFile.Core
{
    public class BigArchiveEntry
    {
        public int Offset;
        public BigArchive Archive;
        public string FullName;
        public string Name => FullName.Split('\\').Last();
        public int Length;
        
        public BigArchiveEntry(BigArchive archive, string name, int offset, int size)
        {
            Archive = archive;
            FullName = name;
            Offset = offset;
            Length = size;
        }
    }
}