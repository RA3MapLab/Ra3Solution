using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WbHelper.UI;
using wbInject;
using wbInject.Core;

namespace WbHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WbStarter.ICommandListener
    {
        private WbStarter starter;
        private HttpClient client;

        public MainWindow()
        {
            InitializeComponent();
            LaunchBtn.IsEnabled = false;
            CheckUpdate();
            // initWbHelper();

            this.Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
        }

        private void initWbHelper()
        {
            LaunchBtn.Content = "启动";
            LaunchBtn.IsEnabled = true;
            starter = new WbStarter();
            starter.commandListener = this;
        }

        private async void CheckUpdate()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            using (client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(2);
                    LaunchBtn.Content = "Check Version..";
                    HttpResponseMessage response = await client.GetAsync("http://pengion.top:8080/version");
                    response.EnsureSuccessStatusCode();
                    string newVersion = await response.Content.ReadAsStringAsync();
                    if (!currentVersion.ToString().Equals(newVersion))
                    {
                        try
                        {
                            LaunchBtn.Content = "DownLoad Zip..";
                            response = await client.GetAsync("http://pengion.top:8080/static/WorldBuilderHelper.zip");
                            response.EnsureSuccessStatusCode();
                            var zipPath = Path.Combine(Environment.CurrentDirectory, "WorldBuilderHelper.zip");
                            if (File.Exists(zipPath))
                            {
                                File.Delete(zipPath);
                            }

                            using (var fs = new FileStream(zipPath, FileMode.CreateNew))
                            {
                                await response.Content.CopyToAsync(fs);
                            }

                            MessageBox.Show("新版本压缩包下载完毕，请手动解压更新", "提示", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            string argument = "/select, \"" + zipPath + "\"";

                            System.Diagnostics.Process.Start("explorer.exe", argument);
                            Application.Current.Shutdown();
                        }
                        catch (Exception e)
                        {
                            Logger.log(e.Message);
                            var messageBoxResult = MessageBox.Show("有新版本，点击确定跳转", "提示", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            if (messageBoxResult == MessageBoxResult.OK)
                            {
                                System.Diagnostics.Process.Start(
                                    "https://pan.baidu.com/s/1a6v6pYFKdEyP-kqelk0TxA?pwd=wuwu ");
                            }

                            System.Windows.Application.Current.Shutdown();
                        }
                    }

                    initWbHelper();
                }
                catch (Exception e)
                {
                    Logger.log(e.Message);
                    initWbHelper();
                    return;
                }

                initWbHelper();
            }

            
            
        }
        
        public static void CopyDirectory(string sourceDir, string destDir)
        {
            // Create the directory if it doesn't exist
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Copy each file into the new directory
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destDir, fileName);
                File.Copy(filePath, destFilePath, true); // overwrite if file already exists
            }

            // Recursively copy subdirectories
            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(destDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath);
            }
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            starter.closeInjector();
            client.Dispose();
            Logger.close();
        }

        private void LoadPlugin(object sender, RoutedEventArgs e)
        {
            try
            { 
                client.Dispose();
                starter.initWbInjector();
            }
            catch (Exception exception)
            {
                Logger.log("LoadPlugin", exception.Message + "  " + exception.StackTrace);
                MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public string onSelectScript()
        {
            return (string)Application.Current.Dispatcher.Invoke(new Func<string>(() =>
            {
                var chooseScriptDialog = new ChooseScriptDialog();
                if (chooseScriptDialog.ShowDialog() == true)
                {
                    return chooseScriptDialog.selectScriptName;
                }

                return "";
            }));
            
            
        }

        public void onClose()
        {
            Close();
        }

        public void showErrorBox(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void JoinQQ(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://jq.qq.com/?_wv=1027&k=o50VegvC");
        }

        private void UpdateLink(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "https://pan.baidu.com/s/1a6v6pYFKdEyP-kqelk0TxA?pwd=wuwu");
        }
    }
}