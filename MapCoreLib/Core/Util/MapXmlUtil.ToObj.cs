using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Interface;

namespace MapCoreLib.Core.Util
{
    public partial class MapXmlUtil
    {
        //TODO 命名空间,schema验证，xsd文档标注
        public static void xml2obj(XmlDocument document, XmlElement root, MapDataContext context, Object asset)
        {
            if (asset == null)
            {
                return;
            }

            var type = asset.GetType();

            if (isICustomXml(type))
            {
                (asset as ICustomXml).toObj(document, root, context, asset);
                return;
            }

            xml2ObjDeFault(document, root, context, asset);
        }

        public static void xml2ObjDeFault(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            var type = asset.GetType();
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                //注意默认值，注意名字，忽略的字段
                var xmlIgnoreAttribute = field.GetCustomAttribute<XmlIgnoreAttribute>();
                if (xmlIgnoreAttribute != null)
                {
                    //标记了忽略的字段
                    continue;
                }

                if (isDirectAttr(field.FieldType))
                {
                    string attrName = field.Name;
                    var fieldXmlAttribute = field.FieldType.GetCustomAttribute<XmlAttributeAttribute>();
                    if (fieldXmlAttribute != null && !string.IsNullOrEmpty(fieldXmlAttribute.AttributeName))
                    {
                        attrName = fieldXmlAttribute.AttributeName;
                    }

                    var defaultValueAttribute = field.GetCustomAttribute<DefaultValueAttribute>();
                    string attrValue = root.GetAttributeDefault(attrName,
                        defaultValueAttribute == null ? null : defaultValueAttribute.Value.ToString());
                    field.SetValue(asset, Convert.ChangeType(attrValue, field.FieldType));
                }
                else if (isListType(field))
                {
                    var xmlArrayAttribute = field.GetCustomAttribute<XmlArrayAttribute>();
                    if (xmlArrayAttribute == null || string.IsNullOrEmpty(xmlArrayAttribute.ElementName))
                    {
                        //平铺序列化
                        Type itemType = field.FieldType.GetGenericArguments()[0];
                        var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                        var fieldTagName = itemType.Name;
                        var fieldXmlRootAttribute = itemType.GetCustomAttribute<XmlRootAttribute>();
                        if (fieldXmlRootAttribute != null && string.IsNullOrEmpty(fieldXmlRootAttribute.ElementName))
                        {
                            fieldTagName = fieldXmlRootAttribute.ElementName;
                        }

                        var itemNodeList = root.SelectNodes($"./ra3Ns:{fieldTagName}", ScriptXml.ra3Ns);
                        for (int i = 0; i < itemNodeList.Count; i++)
                        {
                            var itemNode = itemNodeList[i];
                            //这里直接假定列表里的都不是基础类型，后面如果有需要在拓展
                            var item = Activator.CreateInstance(itemType);
                            xml2obj(document, itemNode as XmlElement, context, item);
                            field.FieldType.GetMethod("Add").Invoke(list, new[] {item});
                        }
                        field.SetValue(asset, list);
                    }
                    else
                    {
                        Type itemType = field.FieldType.GetGenericArguments()[0];
                        var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                        var fieldTagName = itemType.Name;
                        var fieldXmlRootAttribute = itemType.GetCustomAttribute<XmlRootAttribute>();
                        if (fieldXmlRootAttribute != null && string.IsNullOrEmpty(fieldXmlRootAttribute.ElementName))
                        {
                            fieldTagName = fieldXmlRootAttribute.ElementName;
                        }


                        //多一层tag
                        var arrayRootNode = root.SelectSingleNode($"./ra3Ns:{xmlArrayAttribute.ElementName}", ScriptXml.ra3Ns);
                        if (isICustomXml(itemType))
                        {
                            var itemNodeList = arrayRootNode.SelectNodes($"./*");
                            for (int i = 0; i < itemNodeList.Count; i++)
                            {
                                var itemNode = itemNodeList[i];
                                //这里直接假定列表里的都不是基础类型，后面如果有需要在拓展
                                var item = Activator.CreateInstance(itemType);
                                xml2obj(document, itemNode as XmlElement, context, item);
                                field.FieldType.GetMethod("Add").Invoke(list, new[] { item });
                            }

                            field.SetValue(asset, list);
                        }
                        else
                        {
                            var itemNodeList = arrayRootNode.SelectNodes($"./ra3Ns:{fieldTagName}", ScriptXml.ra3Ns);
                            for (int i = 0; i < itemNodeList.Count; i++)
                            {
                                var itemNode = itemNodeList[i];
                                //这里直接假定列表里的都不是基础类型，后面如果有需要在拓展
                                var item = Activator.CreateInstance(itemType);
                                xml2obj(document, itemNode as XmlElement, context, item);
                                field.FieldType.GetMethod("Add").Invoke(list, new[] { item });
                            }

                            field.SetValue(asset, list);
                        }
                        
                        
                    }
                }
                else
                {
                    string attrName = field.FieldType.Name;
                    var fieldXmlAttribute = field.FieldType.GetCustomAttribute<XmlAttributeAttribute>();
                    if (fieldXmlAttribute != null && !string.IsNullOrEmpty(fieldXmlAttribute.AttributeName))
                    {
                        attrName = fieldXmlAttribute.AttributeName;
                    }
                    obj2Xml(document, root.SelectSingleNode($"./ra3Ns:{attrName}", ScriptXml.ra3Ns) as XmlElement, context, Activator.CreateInstance(field.FieldType));
                }
            }
        }
    }
}