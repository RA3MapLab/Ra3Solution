using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Ra3ModBuilder.Core.Model
{
    [XmlRoot("BinaryAssetBuilderProject")]
    public class BABProj
    {
        public Stream1 Stream;

        public BABProj()
        {
        }

        public BABProj(string modXmlPath, string sagePath)
        {
            Stream = new Stream1(modXmlPath, sagePath);
        }

        public void serialize(string modPath)
        {
            var serializer = new XmlSerializer(GetType(), "urn:xmlns:ea.com:babproject");
            using (var fs = File.OpenWrite(Path.Combine(modPath, "mod.babproj")))
            {
                serializer.Serialize(fs, this, Ra3ModConst.nameSpace);
            }
        }

        public class Stream1
        {
            [XmlAttribute]
            public string Source;
            public Configuration1 Configuration;


            public Stream1()
            {
            }

            public Stream1(string modXmlPath, string sagePath)
            {
                Source = modXmlPath;
                Configuration = new Configuration1(sagePath);
            }

            public class Configuration1
            {
                [XmlAttribute]
                public string Name = "";
                [XmlAttribute]
                public string Default = "true";
                [XmlElement(typeof(StreamReference), ElementName = "StreamReference")]
                public List<StreamReference> streamReference = new List<StreamReference>();


                public Configuration1()
                {
                }

                public Configuration1(string sagePath)
                {
                    streamReference.Add(new StreamReference()
                    {
                        ReferenceName = "audio.xml",
                        ReferenceManifest = Path.Combine(sagePath, "audio.manifest")
                    });
                    
                    streamReference.Add(new StreamReference()
                    {
                        ReferenceName = "global.xml",
                        ReferenceManifest = Path.Combine(sagePath, "global.manifest")
                    });
                    
                    streamReference.Add(new StreamReference()
                    {
                        ReferenceName = "static.xml",
                        ReferenceManifest = Path.Combine(sagePath, "static.manifest")
                    });
                }

                public class StreamReference
                {
                    [XmlAttribute] public string ReferenceName { get; set; }

                    [XmlAttribute]
                    public string ReferenceManifest { get; set; }

                    [XmlAttribute] public string ReferenceConfiguration = "";
                }
            }
        }
    }
}