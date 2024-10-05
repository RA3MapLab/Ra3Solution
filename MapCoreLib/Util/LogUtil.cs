using System;
using System.IO;
using System.Reflection;

namespace MapCoreLib.Util
{
    public static class LogUtil
    {
        private static FileStream fs;      
        private static StreamWriter sw;
        // public static string workingDir = ""
        static LogUtil()
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // var logFilePath = Path.Combine(assemblyFolder, $"logs-{DateTime.Now.ToString("yyyyMMdd")}");
            //
            // if (File.Exists(logFilePath))
            //     //验证文件是否存在，有则追加，无则创建
            // {
            //     fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
            // }
            // else
            // {
            //     fs = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
            // }
            // sw = new StreamWriter(fs);
        }
        
        public static void log(string msg)
        {
            sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff")} | | {msg}");
            sw.Flush();
        }
        
        public static void log(string tag, string msg)
        {
            // sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff")} | {tag} | {msg}");
            // sw.Flush();
        }

        public static void close()
        {
            sw.Close();
            fs.Close();    
        }

        public static void init(string workingDir)
        {
            // string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var logFilePath = Path.Combine(workingDir, "data", "logs", $"logs-{DateTime.Now.ToString("yyyyMMdd")}.log");
            
            if (File.Exists(logFilePath))
                //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
        }
    }
}