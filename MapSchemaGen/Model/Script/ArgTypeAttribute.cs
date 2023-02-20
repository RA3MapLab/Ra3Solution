using System;

namespace MapSchemaGen.Model.Script
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgTypeAttribute : Attribute
    {
        public string type { get; set; }

        public ArgTypeAttribute(string type)
        {
            this.type = type;
        }
    }
}