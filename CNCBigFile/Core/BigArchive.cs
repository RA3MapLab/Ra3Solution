using System.Collections.Generic;
using System.IO;
using CNCBigFile.Core.Util;

namespace CNCBigFile.Core
{
    public class BigArchive
    {
        public string fileName = "";
        public List<BigArchiveEntry> bigFileEntries = new List<BigArchiveEntry>();
        public BigArchiveVersion Version;

        public BigArchive parseFromStream(BinaryReader reader)
        {
            var fourCc = reader.ReadFourCc();
            switch (fourCc)
            {
                case "BIGF":
                    Version = BigArchiveVersion.BigF;
                    break;

                case "BIG4":
                    Version = BigArchiveVersion.Big4;
                    break;

                default:
                    throw new InvalidDataException($"Not a supported BIG format: {fourCc}");
            }
            
            reader.ReadBigEndianInt32(); // Archive Size
            var numEntries = reader.ReadBigEndianInt32();
            reader.ReadBigEndianInt32(); // First File Offset

            for (var i = 0; i < numEntries; i++)
            {
                var entryOffset = reader.ReadBigEndianInt32();
                var entrySize = reader.ReadBigEndianInt32();
                var entryName = reader.ReadNullTerminatedString();

                var entry = new BigArchiveEntry(this, entryName, entryOffset, entrySize);

                bigFileEntries.Add(entry);
            }

            //TODO 
            return null;
        }
    }
    
    public enum BigArchiveVersion
    {
        BigF,
        Big4
    }
}