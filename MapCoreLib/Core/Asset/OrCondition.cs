using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MapCoreLib.Core.Interface;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core.Asset
{
    public class OrCondition : MajorAsset, ICustomXml
    {
        [XmlElement("ScriptCondition")]
        public List<ScriptCondition> conditions = new List<ScriptCondition>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            
            while (binaryReader.BaseStream.Position - dataStartPos < dataSize)
            {
                conditions.Add(new ScriptCondition().fromStream(binaryReader, context));
            }
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            foreach (var condition in conditions)
            {
                condition.saveData(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_OrCondition;
        }

        public void toXml(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            MapXmlUtil.obj2XmlDeFault(document, root, context, asset);
        }
        
        public void toObj(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            foreach (XmlElement childNode in root.ChildNodes)
            {
                var condition = new ScriptCondition();
                var Name = childNode.LocalName;
                condition.IsInverted = Convert.ToBoolean(childNode.GetAttributeDefault("IsInverted", false.ToString()));
                var scriptModel = ScriptSpec.conditionsSpec.getOrNull(Name);
                if (scriptModel == null)
                {
                    LogUtil.log($"OrCondition | unknown name ${Name}");
                    continue;
                }
        
                condition.scriptContent.toObj(document, childNode, context, scriptModel);
                conditions.Add(condition);
            }
        }

        public override short getVersion()
        {
            return 1;
        }

        public override void registerSelf(MapDataContext context)
        {
            base.registerSelf(context);
            conditions.ForEach(item => item.registerSelf(context));
        }
    }
}