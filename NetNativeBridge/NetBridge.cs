using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MapCoreLib.Core;
using MapCoreLib.Core.Scripts;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using net.r_eg.Conari.Types;
using Newtonsoft.Json;

namespace NetNativeBridge
{
    public class NetBridge
    {
        //TODO 4.7.2
        [DllExport]
        public static IntPtr ExportXmlScript(IntPtr mpName)
        {
            string mapName = (string)(CharPtr)mpName;
            LogUtil.log($"ExportXmlScript {mapName}");
            if (!CheckMapExist(mapName))
            {
                var msg = $"map {mapName} not found";
                LogUtil.log(msg);

                return CppStr(Result.defaultErrorJson(msg));
            }

            try
            {
                ScriptXml.serialize(PathUtil.defaultMapPath(mapName));
                EditorHelper.openEditor4XmlScript(mapName);
            }
            catch (Exception e)
            {
                var msg = $"{e.Message}";
                LogUtil.log(msg);
                return CppStr(Result.defaultErrorJson(msg));
            }

            return CppStr(Result.successJson());
            ;
        }

        [DllExport]
        public static IntPtr ImportXmlScript(IntPtr mpName)
        {
            string mapName = (string)(CharPtr)mpName;
            LogUtil.log($"ImportXmlScript {mapName}");
            if (!CheckMapExist(mapName))
            {
                var msg = $"map {mapName} not found";
                LogUtil.log(msg);
                return CppStr(Result.defaultErrorJson(msg));
            }

            try
            {
                // throw new Exception("测试中文");
                ScriptXml.deserialize(PathUtil.defaultMapPath(mapName));
            }
            catch (Exception e)
            {
                var msg = $"{e.Message} \n {e.StackTrace}";
                LogUtil.log(msg);
                return CppStr(Result.defaultErrorJson(msg));
            }

            return CppStr(Result.successJson());
        }

        [DllExport]
        public static IntPtr GetCodeScriptList()
        {
            var csScriptDescs = ScriptHandler.getScriptDescs();
            return CppStr(JsonConvert.SerializeObject(csScriptDescs));
        }

        [DllExport]
        public static IntPtr executeCodeScript(IntPtr mpName, IntPtr scriptNamePtr)
        {
            string mapName = (string)(CharPtr)mpName;
            string scriptName = (string)(CharPtr)scriptNamePtr;
            LogUtil.log($"executeCodeScript {mapName} {scriptName}");
            if (!CheckMapExist(mapName))
            {
                var msg = $"map {mapName} not found";
                LogUtil.log("exeScript", msg);
                return CppStr(Result.defaultErrorJson(msg));
            }

            try
            {
                ScriptHandler.runScript(mapName, scriptName);
            }
            catch (Exception e)
            {
                var msg = $"{e.Message}";
                LogUtil.log($"{e.Message} | {e.StackTrace}");
                return CppStr(Result.defaultErrorJson(msg));
            }

            return CppStr(Result.successJson());
        }

        [DllExport]
        public static IntPtr genMiniMap(IntPtr mpName, int style, bool edge)
        {
            
            string mapName = (string)(CharPtr)mpName;
            LogUtil.log($"genMiniMap {mapName} {style} {edge}");
            if (!CheckMapExist(mapName))
            {
                var msg = $"map {mapName} not found";
                LogUtil.log("genMiniMap", msg);
                return CppStr(Result.defaultErrorJson(msg));
            }
            
            try
            {
                MiniMap.genMiniMap(mapName, style, edge);
            }
            catch (Exception e)
            {
                var msg = $"{e.Message}";
                LogUtil.log($"{e.Message} | {e.StackTrace}");
                return CppStr(Result.defaultErrorJson(msg));
            }

            return CppStr(Result.successJson());
        }

        [DllExport]
        public static void init(IntPtr ptr)
        {
            string workingDir = (string)(CharPtr)ptr;
            PathUtil.WorkingDir = workingDir;
            LogUtil.init(workingDir);
            LogUtil.log("init");
        }
        
        //TODO 释放字符串函数
        [DllExport]
        public static void releaseCppString(IntPtr ptr)
        {
            LogUtil.log($"releaseCppString");
            Marshal.FreeHGlobal(ptr);
        }
        
        private static IntPtr CppStr(string str)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(str + "\0");
            IntPtr ptr = Marshal.AllocHGlobal(utf8Bytes.Length);
            Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
            return ptr;
        }

        private static bool CheckMapExist(string mapName)
        {
            return File.Exists(PathUtil.defaultMapPath(mapName));
        }
    }
}