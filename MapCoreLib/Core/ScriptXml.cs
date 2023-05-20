using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core
{
    public class ScriptXml
    {

        //TODO 序列化策略控制，根据修改时间
        //opensage的参数类型数字 似乎可以使用
        //要点一遍所有的脚本
        //TODO XmlSerializer不好用，自己用XmlDocument写一套序列化，利用反射和已有的注解。分散到每个类
        //暂时没想到办法往XmlReader里放入context

        //TODO 需要命名空间区分版本号
        //TODO 写进地图需要验证输入值合法性以及哪些值必须要有

        public static string ra3MapNsUri = "uri:wu.com:ra3map";
        public static XmlNamespaceManager ra3Ns = new XmlNamespaceManager(new NameTable());
        private static bool validXml = true;
        private static string validErrorMsg = "";

        static ScriptXml()
        {
            ra3Ns.AddNamespace("ra3Ns", ra3MapNsUri);
        }
        public static void serialize(string mapPath)
        {
            ScriptSpec.initScriptSpec();
            var ra3Map = new Ra3Map(mapPath);
            ra3Map.parse();

            // doSerializeScript(ra3Map);
            serializeScript2(ra3Map);
        }

        private static void serializeScript2(Ra3Map ra3Map)
        {
            MapDataContext context = ra3Map.getContext();
            
            XmlDocument ra3MapDoc = new XmlDocument();
            XmlElement root = ra3MapDoc.CreateRa3MapElement("Ra3Map");
            // root.SetAttribute("xmlns", "uri:wu.com:ra3map");
            // root.SetAttribute("Name", "test");
            ra3MapDoc.AppendChild(root);
            var playerScriptsList = ra3Map
                .getAsset<PlayerScriptsList>(Ra3MapConst.ASSET_PlayerScriptsList);

            MapXmlUtil.obj2Xml(ra3MapDoc, root, context, playerScriptsList);

            // var a = 2;
            
            var writerSettings = new XmlWriterSettings();
            writerSettings.NewLineOnAttributes = true;
            writerSettings.Indent = true;
            writerSettings.IndentChars = "    ";
            string mapXml = Path.Combine(Path.GetDirectoryName(ra3Map.mapPath),
                ra3Map.getContext().mapName + ".edit.xml");
            using (var writer = XmlWriter.Create(mapXml, writerSettings))
            {
                ra3MapDoc.Save(writer);
            }
        }

        public static void deserialize(string mapPath)
        {
            ScriptSpec.initScriptSpec();
            var ra3Map = new Ra3Map(mapPath);
            ra3Map.parse();

            string xmlPath = Path.Combine(Path.GetDirectoryName(ra3Map.mapPath),
                ra3Map.getContext().mapName + ".edit.xml");
            deserializeScript2(ra3Map, xmlPath);
        }

        private static void deserializeScript2(Ra3Map ra3Map, string xmlPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("uri:wu.com:ra3map", Path.Combine(PathUtil.configDir, "Ra3MapSchema.xsd"));
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            validXml = true;
            validErrorMsg = "";
            //验证xml
            XmlReader reader = XmlReader.Create(xmlPath, settings);
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                if (!validXml)
                {
                    throw new Exception($"xml文件有错误\n{validErrorMsg}");
                }

                //TODO 用schema验证合法性
                XmlNode MapScriptNode = document.SelectSingleNode($"//ra3Ns:{Ra3MapConst.ELEM_NAME_MapScript}", ra3Ns);
                var playerScriptsList = new PlayerScriptsList();
                MapXmlUtil.xml2obj(document, MapScriptNode as XmlElement, ra3Map.getContext(), playerScriptsList);
                ra3Map.replaceMajorAsset(Ra3MapConst.ASSET_PlayerScriptsList, playerScriptsList);

                // ra3Map.save("Test", ra3Map.getContext().mapName);
                ra3Map.modifyMap();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    var msg = $"Error: {e.Message} |  行：{e.Exception.LineNumber} ， 列：{e.Exception.LinePosition}\n";
                    // LogUtil.log(msg);
                    validXml = false;
                    validErrorMsg += msg;
                    //不要马上抛异常，检查所有文件内容
                    // throw new XmlSchemaValidationException(msg);
                    break;
                case XmlSeverityType.Warning:
                    LogUtil.log($"Warning {e.Message} |  行：{e.Exception.LineNumber} ， 列：{e.Exception.LinePosition}");
                    break;
            }
        }
    }
}