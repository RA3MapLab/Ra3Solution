using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MapCoreLib.Core.Asset;
using RMGlib.Core.Utility;

namespace MapSchemaGen
{
    internal class Program
    {
        private static Dictionary<string, string> xsdTypeMap = new Dictionary<string, string>()
        {
            {ScriptSpec.INT, "xs:int"},
            {ScriptSpec.FLOAT, "xs:float"},
            {ScriptSpec.STRING, "xs:string"},
            {ScriptSpec.POSITION, "position"},
        };
        
        //TODO position类型有问题
        public static void Main(string[] args)
        {
            ScriptSpec.initScriptSpec();
            var actions = ScriptSpec.actionsSpec.Values.Select(item => new
            {
                Name = item.commandWord,
                args = item.argumentModel.Select(arg => ((ScriptArgumentType) arg.typeNumber).ToString()).ToList(),
        //脚本条目
         scriptName = item.scriptName,
        //帮助文本
        scriptDesc = item.scriptDesc,
        //参数位置参
        scriptArg = item.scriptArg,
            }).ToList();

            var conditions = ScriptSpec.conditionsSpec.Values.Select(item => new
            {
                Name = item.commandWord,
                args = item.argumentModel.Select(arg => ((ScriptArgumentType) arg.typeNumber).ToString()).ToList(),
                //脚本条目
                scriptName = item.scriptName,
                //帮助文本
                scriptDesc = item.scriptDesc,
                //参数位置
                scriptArg = item.scriptArg,
            }).ToList();

            SortedDictionary<int, string> argsSet = new SortedDictionary<int, string>();
            foreach (var action in ScriptSpec.actionsSpec.Values)
            {
                foreach (var argumentModel in action.argumentModel)
                {
                    if (!argsSet.ContainsKey(argumentModel.typeNumber))
                    {
                        argsSet.Add(argumentModel.typeNumber, argumentModel.realType);
                    }
                }
            }

            foreach (var condition in ScriptSpec.conditionsSpec.Values)
            {
                foreach (var argumentModel in condition.argumentModel)
                {
                    if (!argsSet.ContainsKey(argumentModel.typeNumber))
                    {
                        argsSet.Add(argumentModel.typeNumber, argumentModel.realType);
                    }
                }
            }

            argsSet.OrderBy(item => item.Key);

            if (File.Exists("scriptContent.xsd"))
            {
                File.Delete("scriptContent.xsd");
            }
            
            using (var file = new StreamWriter(File.OpenWrite("scriptContent.xsd")))
            {
                file.WriteLine(
                    @"<?xml version=""1.0"" encoding=""utf-8""?><xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns=""uri:wu.com:ra3map"" targetNamespace=""uri:wu.com:ra3map"" elementFormDefault=""qualified"">");

                //所有动作
                file.WriteLine(@" <xs:complexType name=""Action""> <xs:choice maxOccurs=""unbounded"">");
                foreach (var action in actions)
                {
                    file.WriteLine(
                        $@"<xs:element name=""{action.Name}"" type=""{action.Name}"" maxOccurs=""unbounded"" minOccurs=""0""/>");
                }

                file.WriteLine(@"</xs:choice>  </xs:complexType>");

                //每个动作类型
                foreach (var action in actions)
                {
                    file.WriteLine($@" <xs:complexType name=""{action.Name}""> ");
                    file.WriteLine($@"
<xs:annotation>
            <xs:documentation>
<br/>
                脚本条目 : {HttpUtility.HtmlEncode(action.scriptName)} <br/> 
                参数位置参考 : {HttpUtility.HtmlEncode(action.scriptArg)} <br/> 
            </xs:documentation>
        </xs:annotation>
");
                    if (action.args.Count > 0)
                    {
                        file.WriteLine(@"<xs:sequence>");
                        int index = 0;
                        foreach (var arg in action.args)
                        {
                            file.WriteLine(
                                $@"<xs:element name=""{arg}_{index}"" type=""{arg}"" maxOccurs=""1"" minOccurs=""1""/>");
                            index++;
                        }

                        file.WriteLine(@"</xs:sequence>");
                    }


                    file.WriteLine(@"
<xs:attribute name=""enable"" type=""xs:boolean"" use=""optional"" default=""true""/>
</xs:complexType>");
                }


                //条件

                file.WriteLine(@" <xs:complexType name=""OrCondition""> <xs:choice maxOccurs=""unbounded"">");
                foreach (var condition in conditions)
                {
                    file.WriteLine(
                        $@"<xs:element name=""{condition.Name}"" type=""{condition.Name}"" maxOccurs=""unbounded"" minOccurs=""0""/>");
                }

                file.WriteLine(@"</xs:choice> </xs:complexType>");


                foreach (var condition in conditions)
                {
                    file.WriteLine($@" <xs:complexType name=""{condition.Name}""> ");
                    file.WriteLine($@"
<xs:annotation>
            <xs:documentation>
<br/>
                脚本条目 : {HttpUtility.HtmlEncode(condition.scriptName)}  <br/> 
                参数位置参考 : {HttpUtility.HtmlEncode(condition.scriptArg)} <br/> 
            </xs:documentation>
        </xs:annotation>
");
                    if (condition.args.Count > 0)
                    {
                        file.WriteLine(@"<xs:sequence>");
                        int index = 0;
                        foreach (var arg in condition.args)
                        {
                            file.WriteLine(
                                $@"<xs:element name=""{arg}_{index}"" type=""{arg}"" maxOccurs=""1"" minOccurs=""1""/>");
                            index++;
                        }

                        file.WriteLine(@"</xs:sequence>");
                    }


                    file.WriteLine(@"
<xs:attribute name=""enable"" type=""xs:boolean"" use=""optional"" default=""true""/>
<xs:attribute name=""IsInverted"" type=""xs:boolean"" use=""optional"" default=""false""/>
</xs:complexType>");
                }

                //参数
                foreach (var argItem in argsSet)
                {
                    file.WriteLine($@" <xs:complexType name=""{((ScriptArgumentType) argItem.Key).ToString()}""> ");
                    file.WriteLine(
                        $@"<xs:attribute name=""value"" type=""{xsdTypeMap[argItem.Value]}"" use=""required""/>");
                    file.WriteLine(@"</xs:complexType>");
                }

                file.WriteLine(@"

<xs:simpleType name=""position"">
         <xs:restriction base=""xs:string"">
<xs:pattern value=""\([+-]?([0-9]*[.])?[0-9]+,[+-]?([0-9]*[.])?[0-9]+,[+-]?([0-9]*[.])?[0-9]+\)""/>
</xs:restriction>
</xs:simpleType>
");

                file.WriteLine("</xs:schema>");
            }
        }
    }
}