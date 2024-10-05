using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MapCoreLib.Core.Scripts.ScriptFile;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using Microsoft.CSharp;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core.Scripts
{
    public class ScriptHandler
    {
        public static void runScript(string mapPath, string scriptName)
        {
            ScriptSpec.initScriptSpec();
            var ra3Map = new Ra3Map(mapPath);
            ra3Map.parse();
            // testRunScript(ra3Map.getContext());
            doRunScript(ra3Map, scriptName);
            ra3Map.doSaveMap(ra3Map.mapPath);
        }

        private static void testRunScript(MapDataContext mapDataContext)
        {
            new OnlyInfantry().Apply(mapDataContext);
        }

        public static List<CsScriptDesc> getScriptDescs()
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogUtil.log($"{PathUtil.defaultScriptCsDir()}");
            var scriptDirs = Directory.GetDirectories(PathUtil.defaultScriptCsDir());
            var csScriptDescs = new List<CsScriptDesc>();
            foreach (var scriptDir in scriptDirs)
            {
                //不检查了，编译Source下的所有文件
                // if (!File.Exists(Path.Combine(scriptDir, "Main.cs")))
                // {
                //     //检查必须有对应的代码文件
                //     continue;
                // }
                var helpTextFile = Path.Combine(scriptDir, "readme.txt");
                csScriptDescs.Add(new CsScriptDesc
                {
                    label = new DirectoryInfo(scriptDir).Name,
                    helpText = File.Exists(helpTextFile) ? File.ReadAllText(helpTextFile) : ""
                });
            }

            return csScriptDescs;
        }
            
        public class CsScriptDesc
        {
            public string label { get; set; }
            public string helpText { get; set; }
        }

        private static void doRunScript(Ra3Map ra3Map, string scriptName)
        {
            var scriptSources = PathUtil.defaultScriptCsSources(scriptName);
            
            // 创建编译器参数
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateInMemory = true; // 将程序集保存在内存中而不是磁盘上

            // 添加需要引用的程序集
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParams.ReferencedAssemblies.Add("System.Core.dll");
            compilerParams.ReferencedAssemblies.Add("MapCoreLib.dll");
            compilerParams.ReferencedAssemblies.Add("Newtonsoft.Json.dll");

            // 创建CSharpCodeProvider实例
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // 编译外部.cs代码
            CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, scriptSources.ToArray());
            

            if (results.Errors.HasErrors)
            {
                string msg = "";
                msg = "编译错误: ";
                foreach (CompilerError error in results.Errors)
                {
                    msg += $"\t{error.ErrorNumber}: {error.ErrorText}";
                }

                LogUtil.log(msg);
                throw new Exception(msg);
            }
            else
            {
                // 获取编译后的程序集
                Assembly assembly = results.CompiledAssembly;

                // 获取实现了ScriptInterfacee接口的类的类型
                Type[] types = assembly.GetTypes();
                Type myType = null;
                foreach (Type type in types)
                {
                    if (typeof(ScriptInterface).IsAssignableFrom(type))
                    {
                        myType = type;
                        break;
                    }
                }

                if (myType != null)
                {
                    
                    var oldWorkingDirectory = Environment.CurrentDirectory;
                    try
                    {
                        // 创建实例并执行接口方法
                        object instance = Activator.CreateInstance(myType);
                        ScriptInterface myObject = (ScriptInterface)instance;
                        //切换工作目录
                        Environment.CurrentDirectory = PathUtil.defaultNameScriptCsir(scriptName);
                        myObject.Apply(ra3Map.getContext());
                    }
                    catch (Exception e)
                    {
                        var msg = $"脚本代码运行错误: {e.Message} | {e.StackTrace}";
                        throw new Exception(msg);
                    }
                    finally
                    {
                        Environment.CurrentDirectory = oldWorkingDirectory;
                    }
                    
                }
                else
                {
                    throw new Exception("未找到实现了ScriptInterface接口的类");
                }
            }
        }
    }
}