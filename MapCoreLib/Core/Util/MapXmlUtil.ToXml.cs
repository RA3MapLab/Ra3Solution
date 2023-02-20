using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using MapCoreLib.Core.Interface;

namespace MapCoreLib.Core.Util
{
    public static partial class MapXmlUtil
    {
        public static void obj2Xml(XmlDocument document, XmlElement root, MapDataContext context, Object asset)
        {
            if (asset == null)
            {
                return;
            }

            var type = asset.GetType();

            if (isICustomXml(type))
            {
                (asset as ICustomXml).toXml(document, root, context, asset);
                return;
            }

            obj2XmlDeFault(document, root, context, asset);
        }

        public static void obj2XmlDeFault(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            var type = asset.GetType();
            string tagName = type.Name;
            XmlRootAttribute xmlRootAttribute = type.GetCustomAttribute<XmlRootAttribute>();
            if (xmlRootAttribute != null && !string.IsNullOrEmpty(xmlRootAttribute.ElementName))
            {
                tagName = xmlRootAttribute.ElementName;
            }

            var newElement = document.CreateRa3MapElement(tagName);
            //普通字段默认为属性
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(asset);
                if (fieldValue == null)
                {
                    continue;
                }

                var xmlIgnoreAttribute = field.GetCustomAttribute<XmlIgnoreAttribute>();
                if (xmlIgnoreAttribute != null)
                {
                    //标记了忽略的字段
                    continue;
                }

                var defaultValueAttribute = field.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttribute != null && fieldValue.Equals(defaultValueAttribute.Value))
                {
                    //默认值不用序列化
                    continue;
                }

                if (isDirectAttr(field.FieldType))
                {
                    string attrName = field.Name;
                    var fieldXmlAttribute = field.GetCustomAttribute<XmlAttributeAttribute>();
                    if (fieldXmlAttribute != null && !string.IsNullOrEmpty(fieldXmlAttribute.AttributeName))
                    {
                        attrName = fieldXmlAttribute.AttributeName;
                    }

                    newElement.SetAttribute(attrName,
                        field.FieldType.Equals(typeof(bool)) ? fieldValue.ToString().ToLower() : fieldValue.ToString());
                }
                //先只考虑List
                else if(isListType(field))
                {
                    var xmlArrayAttribute = field.GetCustomAttribute<XmlArrayAttribute>();
                    if (xmlArrayAttribute == null || string.IsNullOrEmpty(xmlArrayAttribute.ElementName))
                    {
                        //平铺序列化
                        IList enumerable = fieldValue as IList;
                        for (int i = 0; i < enumerable.Count; i++)
                        {
                            var item = enumerable[i];
                            //这里直接假定列表里的都不是基础类型，后面如果有需要在拓展
                            obj2Xml(document, newElement, context, item);
                        }
                    }
                    else
                    {
                        var arrayElement = document.CreateRa3MapElement(xmlArrayAttribute.ElementName);
                        IList enumerable = fieldValue as IList;
                        for (int i = 0; i < enumerable.Count; i++)
                        {
                            var item = enumerable[i];
                            //这里直接假定列表里的都不是基础类型，后面如果有需要在拓展
                            obj2Xml(document, arrayElement, context, item);
                        }

                        newElement.AppendChild(arrayElement);
                    }
                    
                }
                else
                {
                    //这里假定剩余情况都是类类型
                    obj2Xml(document, newElement, context, fieldValue);
                }
            }

            root.AppendChild(newElement);
        }

        private static bool isListType(FieldInfo field)
        {
            return field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>);
        }

        private static bool isICustomXml(Type type)
        {
            return typeof(ICustomXml).IsAssignableFrom(type);
        }

        private static bool IsEnumerableType(Type type)
        {
            return (type.GetInterface(nameof(IEnumerable)) != null);
        }

        private static bool isDirectAttr(Type type)
        {
            return type.IsPrimitive || type == typeof(String);
        }
    }
}