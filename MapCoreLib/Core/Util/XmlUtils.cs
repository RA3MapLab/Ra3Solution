using System.Xml;

namespace MapCoreLib.Core.Util
{
    public static class XmlUtils
    {
        public static void SetAttributeDefault(this XmlElement xmlElement, string name, string value, string defaultValue)
        {
            if (value != defaultValue)
            {
                xmlElement.SetAttribute(name, value);
            }
        }


        public static string GetAttributeDefault(this XmlElement xmlElement, string name, string defaultvalue)
        {
            if (xmlElement.HasAttribute(name))
            {
                return xmlElement.GetAttribute(name);
            }

            return defaultvalue;
        }

        public static XmlElement CreateRa3MapElement(this XmlDocument document, string name)
        {
            return document.CreateElement(name, ScriptXml.ra3MapNsUri);
        }
    }
}