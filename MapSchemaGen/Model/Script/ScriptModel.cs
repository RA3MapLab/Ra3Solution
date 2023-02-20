using System.Collections.Generic;

namespace RMGlib.Core.Utility
{
    public class ScriptModel
    {
        public string commandWord { get; set; }
        public int editorNumber { get; set; }
        //脚本条目
        public string scriptName = "";
        //帮助文本
        public string scriptDesc = "";
        //参数位置参考
        public string scriptArg = "";
        public List<ArgumentModel> argumentModel { get; set; }

        public ScriptModel()
        {
        }

        public ScriptModel(string commandWord, int editorNumber, List<ArgumentModel> argumentModel)
        {
            this.commandWord = commandWord;
            this.editorNumber = editorNumber;
            this.argumentModel = argumentModel;
        }
    }
}