using System.IO;

namespace Ra3ModBuilder.Core
{
    //为防止转义字符,content统一必须用双引号包裹
    public class CSFEntry
    {
        public string name { get; set; }
        public string content { get; set; }


        public CSFEntry parse(BinaryReader reader)
        {
            var csfEntry = new CSFEntry();
            reader.ReadChars(4);  //魔数 LBL
            reader.ReadUInt32();  // string数量，固定为1
            csfEntry.name = reader.readUInt32PrefixedAsciiString();
            reader.ReadChars(4);  //魔数 RTS
            csfEntry.content = reader.readUInt32PrefixedNegatedUnicodeString();
            return csfEntry;
        }
    }
}