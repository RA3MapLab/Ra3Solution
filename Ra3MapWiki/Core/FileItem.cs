using System.Collections.Generic;
using RMGlib.Core.Utility;

namespace Ra3MapWiki.Core
{
    public class FileItem
    {
        public string name { get; set; }
        public bool isLeaf { get; set; }
        public ScriptModel ScriptModel { get; set; }
        public List<FileItem> subFiles = new List<FileItem>();
    }
}