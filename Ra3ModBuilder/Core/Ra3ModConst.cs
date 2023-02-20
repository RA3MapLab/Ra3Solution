using System.Xml.Serialization;
using Ra3ModBuilder.Core.Util;

namespace Ra3ModBuilder.Core
{
    public static class Ra3ModConst
    {
        public const string xmlns = "uri:ea.com:eala:asset";
        
        public static XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces().also(ns =>
        {
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        });
    }
}