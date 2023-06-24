using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WbLauncher.Core;


namespace WbLauncher
{
    internal class Program
    {
        // private static string ra3root = "";
        private static string modConfigPath = "";
        private static string language = "en";

        private static List<string> oldBigFiles = new List<string>()
        {
            "WBData_12.big",
            "WBGlobal_12.big",
            "WBStatic_12.big",
            "WBStringHashes_12.big"
        };

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
                    else
                    {
                        return;
                    }
                }
            }

            File.WriteAllText(configPath, ra3root);
            var environmentVariable = Environment.GetEnvironmentVariable("RA3_NEW_WB_DIR");
            if (string.IsNullOrEmpty(environmentVariable) || Directory.GetCurrentDirectory() != environmentVariable)
            {
                Environment.SetEnvironmentVariable("RA3_NEW_WB_DIR", Directory.GetCurrentDirectory(),
                    EnvironmentVariableTarget.User);
            }

            if (!checkPath(ra3root))
            {
                return;
            }

            if (!checkNeededFiles(ra3root))
            {
                return;
            }


            var needWaitEvent = false;
            if (args.Length > 0)
            {
                needWaitEvent = args[0] == "-needwaitevent";
            }

            if (needWaitEvent)
            {
                //TODO 日冕启动的选择语言
                var normalStartupDialog = new NormalStartupDialog((modconfig, lan) =>
                {
                    modConfigPath = modconfig;
                    language = lan;
                }, true);
                normalStartupDialog.ShowDialog();
                var loading = new LoadingDialog("等待初始化中");
                loading.Load += (object sender, EventArgs e) =>
                {
                    Task.Run(() =>
                    {
                        bool result = false;
                        try
                        {
                            result = WaitEvent();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        loading.Invoke(new Action(() =>
                        {
                            // 用于停止下面的 Application.Run
                            Application.Exit();
                            if (!result)
                            {
                                MessageBox.Show("在等待初始化期间出现错误");
                            }
                            else
                            {
                                launcherWb(Path.Combine(ra3root, "Data"));
                            }
                        }));
                    });
                };
                // 这里必须是 Application.Run 而不是 loading.ShowDialog，
                // 因为上面的代码在使用 Application.Exit 来关闭窗口。
                // 之所以不使用 loading.Close() 来关闭窗口，而是使用 Application.Exit 来关闭窗口，
                // 是因为窗口在自己被关闭的时候，需要知道为什么被关闭。
                // Close() 会被视为用户的操作，而 Application.Exit 则是被视为程序的操作。
                // 这样就能根据“谁想关闭窗口”做出区分。
                Application.Run(loading);
            }
            else
            {
                var normalStartupDialog = new NormalStartupDialog((modconfig, lan) =>
                {
                    modConfigPath = modconfig;
                    language = lan;
                });
                var dialogResult = normalStartupDialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                launcherWb(Path.Combine(ra3root, "Data"));
            }
        }

        private static bool checkPath(string ra3Root)
        {
            if (ra3Root.Any(z => IsChinese(z)))
            {
                MessageBox.Show($"游戏根目录包含中文，请修改 {ra3Root}");
                return false;
            }

            if (Directory.GetCurrentDirectory().Any(z => IsChinese(z)))
            {
                MessageBox.Show($"新地编根目录包含中文，请修改 {Directory.GetCurrentDirectory()}");
                return false;
            }

            return true;
        }

        private static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");

        public static bool IsChinese(char c)
        {
            return cjkCharRegex.IsMatch(c.ToString());
        }

        private static bool checkNeededFiles(string ra3Root)
        {
            var gameDataDir = Path.Combine(ra3Root, "Data");
            bool hasAllFiles = true;
            foreach (var file in oldBigFiles)
            {
                var bigPath = Path.Combine(gameDataDir, file);
                if (!File.Exists(bigPath))
                {
                    hasAllFiles = false;
                    break;
                }
            }

            if (hasAllFiles)
            {
                return true;
            }
            else
            {
                return downloadBigFiles(ra3Root);
            }
        }

        private static bool downloadBigFiles(string ra3Root)
        {
            //use Downloader.exe in tool directory to download and wait for it and get process exit code
            var currentDirectory = Directory.GetCurrentDirectory();
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(currentDirectory, "tool", "Downloader.exe"),
                WorkingDirectory = Path.Combine(currentDirectory, "tool"),
                CreateNoWindow = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                //check if all big files are downloaded
                var gameDataDir = Path.Combine(ra3Root, "Data");
                foreach (var file in oldBigFiles)
                {
                    var bigPath = Path.Combine(gameDataDir, file);
                    if (!File.Exists(bigPath))
                    {
                        MessageBox.Show($"下载或解压big压缩包失败，请重试");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        class CommandArg
        {
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
                Arguments =
                    $@"--lan=""{language}"" --rootDir=""{currentDirectory}"" --gameDataPath=""{ra3Root}"" --modConfig=""{modConfigPath.Trim()}""",
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
            var rule = new EventWaitHandleAccessRule(everyone, EventWaitHandleRights.FullControl,
                AccessControlType.Allow);
            security.AddAccessRule(rule);
            // var notificationEvent = EventWaitHandleAcl.Create(false, EventResetMode.AutoReset, eventName, out _, security);
            if (!EventWaitHandle.TryOpenExisting("ra3_wu_wb_wait_start", EventWaitHandleRights.Modify,
                    out var startWaitEvent))
            {
                return false;
            }

            using (startWaitEvent)
            {
                if (!EventWaitHandle.TryOpenExisting("ra3_wu_wb_wait_complete", EventWaitHandleRights.Synchronize,
                        out var notificationEvent))
                {
                    return false;
                }

                using (notificationEvent)
                {
                    startWaitEvent.Set();
                    notificationEvent.WaitOne();
                    return true;
                }
            }
        }
    }
}