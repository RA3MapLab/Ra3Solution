using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WbLauncher.Core;


namespace WbLauncher
{
    internal class Program
    {
        private static string ra3root = "";
        private static string modConfigPath = "";

        [STAThread]
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "config", "_config");
            var ra3root = File.ReadAllText(configPath);
            if (string.IsNullOrEmpty(ra3root) || !Registry.IsGamePathValid(Path.Combine(ra3root)))
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "选择游戏安装目录下的RA3.exe";
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "RA3 Game |RA3.exe";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ra3root = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 8);
                        if (!Registry.IsGamePathValid(ra3root))
                        {
                            MessageBox.Show("这不是一个正确的路径！");
                            return;
                        }
                    }
                }
            }
            File.WriteAllText(configPath, ra3root);
            var environmentVariable = Environment.GetEnvironmentVariable("RA3_NEW_WB_DIR");
            if (string.IsNullOrEmpty(environmentVariable) || Directory.GetCurrentDirectory() != environmentVariable)
            {
                Environment.SetEnvironmentVariable("NEW_RA3_WB_DIR", Directory.GetCurrentDirectory(), EnvironmentVariableTarget.User);

            }

            var needWaitEvent = true;
            MessageBox.Show("test2");
            if (needWaitEvent)
            {
                var loading = new LoadingDialog("等待初始化中");
                Task.Run(() =>
                {
                    bool res = WaitEvent();
                    loading.Invoke((MethodInvoker)delegate
                    {
                        loading.Close();
                        if (!res)
                        {
                            MessageBox.Show("在等待初始化期间出现错误");
                            Application.Exit();
                        }
                        else
                        {
                            launcherWb(Path.Combine(ra3root, "Data"));
                        }
                    });
                });
                
                loading.ShowDialog();
                
                
            }
            else
            {
                var selectModDialog = new SelectModDialog(path => { modConfigPath = path; });
                var dialogResult = selectModDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                launcherWb(Path.Combine(ra3root, "Data"));
            }

        }
        
        class CommandArg {
            string rootDir;
            string gameDataDir;
        };
        private static void launcherWb(string ra3Root)
        {
            // var gameDataPath = Registry.GetGameDataPath();
            var gameDataPath = ra3Root;
            var currentDirectory = Directory.GetCurrentDirectory();
            
            string pathvar = System.Environment.GetEnvironmentVariable("PATH");
            var newEnvPath = pathvar + $";{Path.Combine(currentDirectory, "bin")};";
            

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(currentDirectory, "bin", "WorldBuilder_Mod_1.12.exe"),
                WorkingDirectory = gameDataPath,
                Arguments = $@"--rootDir=""{currentDirectory}"" --gameDataPath=""{ra3Root}"" --modConfig=""{modConfigPath.Trim()}""",
                UseShellExecute = false
            };
            // Environment.SetEnvironmentVariable("PATH", newEnvPath, EnvironmentVariableTarget.Process);
            processStartInfo.EnvironmentVariables["PATH"] = newEnvPath;
            Process.Start(processStartInfo);
        }
        
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void StartAsAdmin()
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, 
                    UseShellExecute = true, 
                    Verb = "runas"
                }
            };

            proc.Start();
        }
        
        private static bool WaitEvent()
        {
            var security = new EventWaitHandleSecurity();
            // 为了尽可能的避免权限问题，这个事件被设为任何人都有设置的权限
            var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var rule = new EventWaitHandleAccessRule(everyone, EventWaitHandleRights.FullControl, AccessControlType.Allow);
            security.AddAccessRule(rule);
            // var notificationEvent = EventWaitHandleAcl.Create(false, EventResetMode.AutoReset, eventName, out _, security);
            var res1 = EventWaitHandle.TryOpenExisting("ra3_wu_wb_wait_start", EventWaitHandleRights.Modify, out var startEvent);
            
            
            if (res1)
            {
                // startEvent.Set();
                var res2 = EventWaitHandle.TryOpenExisting("ra3_wu_wb_wait_complete", EventWaitHandleRights.Synchronize, out var notificationEvent);
                if (res2)
                {
                    startEvent.Set();
                    notificationEvent.WaitOne();
                    startEvent.Dispose();
                    notificationEvent.Dispose();
                    return true;
                }
                else
                {
                    startEvent.Dispose();
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
}