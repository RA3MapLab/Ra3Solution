using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class ScriptGroup : MajorAsset
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        [DefaultValue(true)]
        public bool IsActive;
        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsSubroutine;
        [XmlElement("Script")]
        public List<Script> scripts = new List<Script>();
        [XmlElement("ScriptGroup")]
        public List<ScriptGroup> scriptGroups = new List<ScriptGroup>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            Name = binaryReader.readDefaultString();
            IsActive = binaryReader.ReadBoolean();
            IsSubroutine = binaryReader.ReadBoolean();
            while (binaryReader.BaseStream.Position - dataStartPos < dataSize)
            {
                var id = binaryReader.ReadInt32();
                var assetName = context.MapStruct.findStringByIndex(id);
                binaryReader.BaseStream.Position -= 4;
                switch (assetName)
                {
                    case Ra3MapConst.ASSET_Script:
                        scripts.Add((Script) new Script().fromStream(binaryReader, context));
                        break;
                    case Ra3MapConst.ASSET_ScriptGroup:
                        scriptGroups.Add((ScriptGroup) new ScriptGroup().fromStream(binaryReader, context));
                        break;
                    default:
                        LogUtil.log($"unknown assetName: {assetName}");
                        break;
                }
            }
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.writeDefaultString(Name);
            binaryWriter.Write(IsActive);
            binaryWriter.Write(IsSubroutine);
            foreach (var script in scripts)
            {
                script.Save(binaryWriter, context);
            }

            foreach (var scriptGroup in scriptGroups)
            {
                scriptGroup.Save(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_ScriptGroup;
        }

        public override void registerSelf(MapDataContext context)
        {
            base.registerSelf(context);
            scriptGroups.ForEach(scriptGroup => scriptGroup.registerSelf(context));
            scripts.ForEach(script => script.registerSelf(context));
        }

        public override short getVersion()
        {
            return 3;
        }
    }
}