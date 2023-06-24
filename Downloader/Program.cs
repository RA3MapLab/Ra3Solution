using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Downloader.Core;
using Downloader.Core.UI;

namespace Downloader
{
    //下载器默认在根目录的tool文件夹里
    internal class Program
    {
        private static Updater updater;

        [STAThread]
        public static async Task<int> Main(string[] args)
        {
            var WbRootPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var gameDataPath = Path.Combine(File.ReadAllText(Path.Combine(WbRootPath, "data", "config", "_config")), "Data");
            updater = new Updater(
                Path.Combine(WbRootPath, "data", "logs"),
                Path.Combine(WbRootPath, "data", "temp"),
                Path.Combine(WbRootPath),
                gameDataPath,
                new Version(1, 0)
            );
            
            var progressDialog = new ProgressDialog("Downloading WB Big Files", 
                (progress, speed) => updater.DownloadBigAsync(progress, speed, default));
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