using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core.Asset
{
    public class ScriptContent : MajorAsset
    {
        // [XmlIgnore]
        public int contentType;

        // [XmlIgnore]
        public AssetPropertyType assetPropertyType;
        public string contentName;
        [XmlIgnore] public int nameIndex;
        public bool enable = true;
        public List<ScriptArgument> arguments = new List<ScriptArgument>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);

            contentType = binaryReader.ReadInt32();
            assetPropertyType = (AssetPropertyType) binaryReader.ReadByte();
            nameIndex = binaryReader.ReadUInt24();
            contentName = context.MapStruct.findStringByIndex(nameIndex);
            var argNums = binaryReader.ReadInt32();
            for (int i = 0; i < argNums; i++)
            {
                arguments.Add(new ScriptArgument().fromStream(binaryReader, context));
            }

            enable = binaryReader.ReadInt32() == 1;
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(contentType);
            binaryWriter.Write((byte) assetPropertyType);
            binaryWriter.WriteUInt24((uint) nameIndex);
            binaryWriter.Write(arguments.Count);
            // foreach (var argument in arguments)
            // {
            //     argument.saveData(binaryWriter, context);
            // }

            for (int i = 0; i < arguments.Count; i++)
            {
                arguments[i].saveData(binaryWriter, context);
            }

            binaryWriter.Write(enable ? 1 : 0);
        }

        public override string getAssetName()
        {
            throw new System.NotImplementedException();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", contentName);
            writer.WriteAttributeString("enable", enable.ToString());
            var scriptConModel = ScriptSpec.conditionsSpec.getOrNull(contentName);
            var scriptActModel = ScriptSpec.actionsSpec.getOrNull(contentName);
            if (scriptConModel != null && scriptActModel != null)
            {
                LogUtil.log("ScriptContent | WriteXml find two spec");
            }

            var model = scriptConModel == null ? scriptActModel : scriptConModel;
            if (model == null)
            {
                LogUtil.log("unknown arg model");
                return;
            }

            // writer.WriteStartElement("ScriptArgument");

            var argumentModel = model.argumentModel;
            for (int i = 0; i < argumentModel.Count; i++)
            {
                var argModel = argumentModel[i];
                writer.WriteStartElement("Arg");
                var value = "";
                if (argModel.realType == ScriptSpec.INT)
                {
                    value = arguments[i].intValue.ToString();
                }
                else if (argModel.realType == ScriptSpec.FLOAT)
                {
                    value = arguments[i].floatValue.ToString();
                }
                else if (argModel.realType == ScriptSpec.STRING)
                {
                    value = arguments[i].stringValue;
                }
                else if (argModel.realType == ScriptSpec.POSITION)
                {
                    value = arguments[i].position.ToString();
                }

                writer.WriteAttributeString("value", value);
                writer.WriteEndElement();
            }

            // writer.WriteEndElement();
        }

        public class ScriptContentElement
        {
            [XmlAttribute] public string Name;
            [XmlAttribute] public bool enable;

            [XmlElement] public List<Argument> Arg;

            public class Argument
            {
                [XmlAttribute] public string value { get; set; }
            }
        }



        public void toXml(XmlDocument document, XmlElement newElement, MapDataContext context, ScriptModel scriptModel)
        {
            newElement.SetAttributeDefault("enable", enable.ToString().ToLower(), true.ToString().ToLower());

            var argumentModel = scriptModel.argumentModel;
            for (int i = 0; i < argumentModel.Count; i++)
            {
                var argModel = argumentModel[i];

                var tagName = $"{((ScriptArgumentType) argModel.typeNumber).ToString()}_{i}";

                var argElement = document.CreateRa3MapElement(tagName);
                var value = "";
                if (argModel.realType == ScriptSpec.INT)
                {
                    value = arguments[i].intValue.ToString();
                }
                else if (argModel.realType == ScriptSpec.FLOAT)
                {
                    value = arguments[i].floatValue.ToString();
                }
                else if (argModel.realType == ScriptSpec.STRING)
                {
                    value = arguments[i].stringValue;
                }
                else if (argModel.realType == ScriptSpec.POSITION)
                {
                    value = arguments[i].position.ToString();
                }

                argElement.SetAttribute("value", value);

                newElement.AppendChild(argElement);
            }
        }

        public void toObj(XmlDocument document, XmlElement node, MapDataContext context, ScriptModel scriptModel)
        {
            contentName = node.LocalName;
            enable = Convert.ToBoolean(node.GetAttributeDefault("enable", false.ToString()));
            contentType = scriptModel.editorNumber;
            assetPropertyType = AssetPropertyType.stringType;
            nameIndex = context.MapStruct.RegisterString(contentName);

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var argNode = node.ChildNodes[i] as XmlElement;
                var scriptArgument = new ScriptArgument();
                var argModel = scriptModel.argumentModel[i];
                scriptArgument.argumentType = argModel.typeNumber;
                if (argModel.realType == ScriptSpec.INT)
                {
                    scriptArgument.intValue = Convert.ToInt32(argNode.GetAttribute("value"));
                }
                else if (argModel.realType == ScriptSpec.FLOAT)
                {
                    scriptArgument.floatValue = Convert.ToSingle(argNode.GetAttribute("value"));
                }
                else if (argModel.realType == ScriptSpec.STRING)
                {
                    scriptArgument.stringValue = argNode.GetAttribute("value");
                }
                else if (argModel.realType == ScriptSpec.POSITION)
                {
                    scriptArgument.position = Vec3D.fromXmlStr(argNode.GetAttribute("value"));
                }
                arguments.Add(scriptArgument);
            }
        }

        public override short getVersion()
        {
            return base.getVersion();
        }

        public override void registerSelf(MapDataContext context)
        {
            base.registerSelf(context);
        }

        public void registerSelf2(MapDataContext context, short ver, string realname)
        {
            id = context.MapStruct.RegisterString(realname);
            base.version = ver;
        }
    }
}