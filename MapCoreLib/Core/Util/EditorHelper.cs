using System;
using System.Diagnostics;

namespace MapCoreLib.Core.Util
{
    public static class EditorHelper
    {
        public static void openEditor4XmlScript(string mapPath)
        {
            // 文件路径
            string filePath = PathUtil.CorrespondScriptXmlPath(mapPath);


            Process fileopener = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filePath;
            startInfo.UseShellExecute = true;

            Process process = new Process();
            process.StartInfo = startInfo;

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        static bool CheckVsCodeInstalled()
        {
            // 检查是否已安装 VS Code
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "code";
            startInfo.Arguments = "--version";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.StartsWith("1.");
        }

        static void OpenFileWithVsCode(string filePath)
        {
            // 使用 VS Code 打开文件
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "code";
            startInfo.Arguments = filePath;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }

        static void OpenFileWithNotepad(string filePath)
        {
            // 使用记事本打开文件
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "notepad";
            startInfo.Arguments = filePath;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}