using System;
using System.Collections.Generic;

namespace wbInject.Core
{
    
    public class IpcData
    {
        public string command { get; set; }
        public List<string> param { get; set; }

        public IpcData()
        {
            param = new List<string>();
        }

        public override string ToString()
        {
            return $"command: {command} | param: {string.Join(", ", param)}";
        }
    }
}