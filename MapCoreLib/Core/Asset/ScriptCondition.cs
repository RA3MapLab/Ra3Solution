using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MapCoreLib.Core.Interface;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core.Asset
{
    public class ScriptCondition : ICustomXml
    {
        [XmlIgnore] public ScriptContent scriptContent = new ScriptContent();

        [XmlAttribute("IsInverted")] [DefaultValue(false)]
        public bool IsInverted = false;

        public ScriptCondition fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            scriptContent = (ScriptContent) new ScriptContent().fromStream(binaryReader, context);
            IsInverted = binaryReader.ReadInt32() == 1;
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            scriptContent.Save(binaryWriter, context);
            binaryWriter.Write(IsInverted ? 1 : 0);
        }


        public void toXml(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            var newElement = document.CreateRa3MapElement(scriptContent.contentName);
            newElement.SetAttributeDefault("IsInverted", IsInverted.ToString().ToLower(), false.ToString().ToLower());
            var scriptModel = ScriptSpec.conditionsSpec.getOrNull(scriptContent.contentName);
            if (scriptModel == null)
            {
                // root.AppendChild(newElement);
                return;
            }

            scriptContent.toXml(document, newElement, context, scriptModel);
            root.AppendChild(newElement);
        }

        public void toObj(XmlDocument document, XmlElement childNode, MapDataContext context, object asset)
        {
            // var condition = new ScriptCondition();
            // var Name = childNode.LocalName;
            // condition.IsInverted = Convert.ToBoolean(childNode.GetAttributeDefault("IsInverted", false.ToString()));
            // var scriptModel = ScriptSpec.conditionsSpec.getOrNull(Name);
            // if (scriptModel == null)
            // {
            //     LogUtil.debug($"OrCondition | unknown name ${Name}");
            //     throw new Exception($"非法的脚本条件标签 {Name}");
            // }
            //
            // condition.scriptContent.toObj(document, childNode, context, scriptModel);
        }

        public void registerSelf(MapDataContext context)
        {
            scriptContent.registerSelf2(context, 6, "Condition");
        }
    }
}