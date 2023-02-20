using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using MapCoreLib.Core.Interface;
using MapCoreLib.Core.Util;

namespace MapCoreLib.Core.Asset
{
    [XmlRoot(Ra3MapConst.ELEM_NAME_MapScript)]
    public class PlayerScriptsList : MajorAsset, ICustomXml
    {
        [XmlElement]
        public List<ScriptList> scriptLists = new List<ScriptList>();
        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);
            while (binaryReader.BaseStream.Position - dataStartPos < dataSize)
            {
                scriptLists.Add((ScriptList) new ScriptList().fromStream(binaryReader, context));
            }
            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            foreach (var scriptList in scriptLists)
            {
                scriptList.Save(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_PlayerScriptsList;
        }

        public void toXml(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            List<string> playerNames = context
                .getAsset<SidesList>(Ra3MapConst.ASSET_SidesList)
                .players
                .Select(player => player.assetPropertyCollection.getProperty("playerName").data as string).ToList();

            MapXmlUtil.obj2XmlDeFault(document, root, context, asset);
            for (int i = 0; i < root.FirstChild.ChildNodes.Count; i++)
            {
                var childNode = root.FirstChild.ChildNodes[i];
                (childNode as XmlElement).SetAttribute("Name", playerNames[i]);
            }
        }

        public void toObj(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            List<string> playerNames = context
                .getAsset<SidesList>(Ra3MapConst.ASSET_SidesList)
                .players
                .Select(player => player.assetPropertyCollection.getProperty("playerName").data as string).ToList();
            
            MapXmlUtil.xml2ObjDeFault(document, root, context, asset);
            //TODO 按照玩家名字顺序排序列表
        }

        public override void registerSelf(MapDataContext context)
        {
            base.registerSelf(context);
            foreach (var scriptList in scriptLists)
            {
                scriptList.registerSelf(context);
            }
        }

        public override short getVersion()
        {
            return 1;
        }
    }
}