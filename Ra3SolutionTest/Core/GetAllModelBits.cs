using System;
using System.IO;
using System.Xml;

namespace NewMapParser.Core
{
    public class GetAllModelBits
    {
        public static void Run()
        {
            //load ModelState.xsd file and get all enumeration nodes
            XmlDocument doc = new XmlDocument();
            doc.Load("ObjectStatus.xsd");

            foreach (var node in doc.SelectNodes("//enumeration"))
            {
                var value = (node as XmlNode).Attributes["value"].Value;
                Console.WriteLine($"else if (name == \"{value}\") {{ \n return static_cast<int>(ObjectStatus::{value}); \n}}");
                // Console.WriteLine($"\"{value}\",");
            }
        }
    }
}