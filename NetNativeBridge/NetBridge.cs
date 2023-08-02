using System;
using System.Diagnostics;
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
        public static IntPtr createNewMap(IntPtr newMapConfigPtr)
        {
            string newMapConfigStr = (string)(CharPtr)newMapConfigPtr;
            LogUtil.log($"createNewMap {newMapConfigStr}");
            try
            {
                var newMapConfig = JsonConvert.DeserializeObject<NewMapConfig>(newMapConfigStr);
                var mapDir = Path.GetDirectoryName(newMapConfig.mapPath);
                if (Directory.Exists(mapDir))
                {
                    Directory.Delete(mapDir, true);
                }
                Ra3Map.newMap(newMapConfig)
                    .save(newMapConfig.mapPath);
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

        [DllExport]
        public static IntPtr launchGameDebugger()
        {
            var rootDir = PathUtil.WorkingDir;
            var gameDebuggerPath = Path.Combine(rootDir, "game", "WuRa3GameDebug.exe");
            // var ra3RootDir = Environment.GetEnvironmentVariable("RA3_Root_Dir");
            //check if gameDebugger exist
            if (!File.Exists(gameDebuggerPath))
            {
                return CppStr(Result.defaultErrorJson($"未找到游戏调试器"));
            }
            //check if gameDebugger is running
            var gameDebuggerProcess = Process.GetProcessesByName("WuRa3GameDebug");
            if (gameDebuggerProcess.Length != 0)
            {
                LogUtil.log("gameLauncher started");
                // 如果游戏调试器已经启动
                // 检测是否有红警3,如果有红警3，切换到红警3窗口
                if (FindIfRa3Running())
                {
                    var ra3Windows = FindRa3Windows();
                    var ra3 = FindFirstRa3Process(ra3Windows);
                    SetForegroundWindow(ra3.MainWindowHandle);
                    //已经切换到游戏了，不要再问是否立即启动游戏了
                    return CppStr(Result.errorJson(-2, ""));
                }
                else
                {
                    //成功代表可以继续启动游戏，得先在cpp那边询问是否要立即启动游戏
                    return CppStr(Result.successJson());
                }
            }
            else
            {
                LogUtil.log("gameLauncher not started");
                //游戏调试器没启动
                //检测是否有红警3
                if (FindIfRa3Running())
                {
                    return CppStr(Result.defaultErrorJson("检测到你已经启动了红警3，必须先启动插件，再启动游戏"));
                }

                //启动调试器
                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(rootDir, "game", "WuRa3GameDebug.exe"),
                    WorkingDirectory = Path.Combine(rootDir, "game"),
                };
                
                LogUtil.log("launch WuRa3GameDebug");
                var process = Process.Start(processStartInfo);

                return CppStr(Result.successJson());
            }

        }

        private static bool FindIfRa3Running()
        {
            var ra3Windows = FindRa3Windows();
            if (ra3Windows.Length != 0)
            { 
                return true;
            }

            var ra3 = FindFirstRa3Process(ra3Windows);
            if (ra3 != null)
            {
                return true;
            }

            return false;
        }

        [DllExport]
        public static IntPtr launchRa3()
        {
            var ra3RootDir = Environment.GetEnvironmentVariable("RA3_Root_Dir");
            if (!File.Exists(Path.Combine(ra3RootDir, "RA3.exe")))
            {
                return CppStr(Result.defaultErrorJson("找不到红警3, 请手动开启游戏"));
            }
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(ra3RootDir, "RA3.exe"),
                WorkingDirectory = ra3RootDir,
                Arguments = " -ui",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            return CppStr(Result.successJson());
        }

        public const string Ra3WindowClass = "41DAF790-16F5-4881-8754-59FD8CF3B8D2";
        private static IntPtr[] FindRa3Windows()
        {
            var ra3Window = FindWindowW(Ra3WindowClass, null);
            if (ra3Window == IntPtr.Zero)
            {
                // 岚依的魔改版红警3（
                ra3Window = FindWindowW("7" + Ra3WindowClass.Substring(1), null);
            }

            if (ra3Window != IntPtr.Zero)
            {
                return new[] { ra3Window };
            }

            return Array.Empty<IntPtr>();
        }
        
        private static Process FindFirstRa3Process(IntPtr[] ra3Windows)
        {
            foreach (var window in ra3Windows)
            {
                var found = GetRa3Process(window);
                if (found is null)
                {
                    continue;
                }

                return found;
            }

            return null;
        }
        
        private static Process GetRa3Process(IntPtr window)
        {
            // 根据窗口句柄找到进程 ID
            _ = GetWindowThreadProcessId(window, out var processId);
            Process process = null;
            try
            {
                process = Process.GetProcessById(processId);
                // 万一人家开的是 KW 呢
                if (process.ProcessName.ToLower().Contains("ra3_"))
                {
                    // 现在确定是 RA3 了
                    var returnValue = process;
                    process = null; // 避免返回值被 dispose
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                process?.Dispose();
            }

            return null;
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
        
        [DllImport("User32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindowW(string className, string windowName);
        
        [DllImport("User32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr window, out int processId);
        
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow (IntPtr hWnd);
    }
}