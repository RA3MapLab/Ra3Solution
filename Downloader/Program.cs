using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandLine;
using Downloader.Core;
using Downloader.Core.UI;

namespace Downloader
{
    //下载器默认在根目录的tool文件夹里
    internal class Program
    {
        private static Updater updater;

        public class Options
        {
            /**
             * 0: download wb big files
             * 1: download terrainFix.big
             */
            [Option('t', "type", Required = true, HelpText = "set download task type")]
            public int taskType { get; set; }
        }
        
        [STAThread]
        public static async Task<int> Main(string[] args)
        {
            Action<Progress<int?>?, Progress<string?>?>? action = null;
            string title = "";
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    switch (o.taskType)
                    {
                        case 0:
                            action = (progress, speed) => updater.DownloadWbBigsAsync(progress, speed, default);
                            title = "Downloading WB Big Files";
                            break;
                        case 1:
                            action = (progress, speed) => updater.DownloadTerrainFixAsync(progress, speed, default);
                            title = "Downloading TerrainFix.big";
                            break;
                        default:
                            break;
                    }
                });

            if (action == null)
            {
                return -1001;
            }
            
            var WbRootPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var gameDataPath = Path.Combine(File.ReadAllText(Path.Combine(WbRootPath, "data", "config", "_config")), "Data");
            updater = new Updater(
                Path.Combine(WbRootPath, "data", "logs"),
                Path.Combine(WbRootPath, "data", "temp"),
                Path.Combine(WbRootPath),
                gameDataPath,
                new Version(1, 0)
            );
            
            var progressDialog = new ProgressDialog(title, action);
            try
            {
                progressDialog.ShowDialog();
            }
            catch (Exception e)
            {
                progressDialog.Close();
                MessageBox.Show("Download Fail!");
                return -1;
            }

            return 0;
        }
    }
}