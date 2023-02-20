namespace RMGlib.Core.Utility
{
    public class ArgumentModel
    {
        public int typeNumber { get; set; }
        public string realType { get; set; }
        public string exampleData { get; set; }

        public ArgumentModel()
        {
        }

        public ArgumentModel(int typeNumber, string realType, string exampleData)
        {
            this.typeNumber = typeNumber;
            this.realType = realType;
            this.exampleData = exampleData;
        }
    }
}