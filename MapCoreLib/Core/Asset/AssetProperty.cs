using System;
using System.IO;
using System.Reflection;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class AssetProperty
    {
        public AssetPropertyType propertyType;
        public int id;
        public string name;
        public object data;

        public AssetProperty fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            propertyType = (AssetPropertyType) binaryReader.ReadByte();
            id = binaryReader.ReadUInt24();
            name = context.MapStruct.findStringByIndex(id);
            switch (propertyType)
            {
                case AssetPropertyType.boolType:
                    data = binaryReader.ReadBoolean();
                    break;
                case AssetPropertyType.intType:
                    data = binaryReader.ReadInt32();
                    break;
                case AssetPropertyType.floatType:
                    data = binaryReader.ReadSingle();
                    break;
                case AssetPropertyType.stringType:
                    data = binaryReader.readDefaultString();
                    break;
                case AssetPropertyType.stringUnicodeType:
                    data = binaryReader.readUnicodeString();
                    break;
                case AssetPropertyType.stringNameValueType:
                    data = binaryReader.readDefaultString();
                    break;
                default:
                    LogUtil.debug($"unknown propertyType {propertyType}");
                    break;
            }

            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write((byte) propertyType);
            binaryWriter.WriteUInt24((uint) id);
            switch (propertyType)
            {
                case AssetPropertyType.boolType:
                    binaryWriter.Write((bool) data);
                    break;
                case AssetPropertyType.intType:
                    binaryWriter.Write((int) data);
                    break;
                case AssetPropertyType.floatType:
                    binaryWriter.Write((float) data);
                    break;
                case AssetPropertyType.stringType:
                    binaryWriter.writeDefaultString((string) data);
                    break;
                case AssetPropertyType.stringUnicodeType:
                    binaryWriter.writeUnicodeString((string) data);
                    break;
                case AssetPropertyType.stringNameValueType:
                    binaryWriter.writeDefaultString((string) data);
                    break;
                default:
                    LogUtil.debug($"unknown propertyType {propertyType}");
                    break;
            }
        }

        public static AssetProperty of(string name, object data, MapDataContext context)
        {
            var assetProperty = new AssetProperty();
            assetProperty.data = data;
            assetProperty.name = name;

            if (data is bool)
            {
                assetProperty.propertyType = AssetPropertyType.boolType;
            }
            else if (data is int)
            {
                assetProperty.propertyType = AssetPropertyType.intType;
            }
            else if (data is float)
            {
                assetProperty.propertyType = AssetPropertyType.floatType;
            }
            else if (data is string)
            {
                if (name == "playerDisplayName")
                {
                    assetProperty.propertyType = AssetPropertyType.stringUnicodeType;
                }
                else
                {
                    assetProperty.propertyType = AssetPropertyType.stringType;
                }
            }
            else if (data is string[])
            {
                assetProperty.propertyType = AssetPropertyType.stringNameValueType;
            }
            else
            {
                LogUtil.debug($"AssetProperty of type {data.GetType().Name}");
                assetProperty.propertyType = AssetPropertyType.intType;
            }

            assetProperty.id = context.MapStruct.RegisterString(name);

            return assetProperty;
        }
    }
}