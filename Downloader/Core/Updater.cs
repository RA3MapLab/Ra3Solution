using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeHollow.FeedReader;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;

namespace Downloader.Core;

public class Updater
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    private static string FeedUrl = "https://hw.file.rmrts.com/srv/feed.xml";
    
    public VersionData VersionData;
    public string LogFolderPath { get; set; }
    public string TempFolderPath { get; set; }
    public string CurrentAppRoot { get; set; }
    
    public string GameDataPath { get; set; }
    public Version? ProgramVersion { get; set; }
    
    public Updater(
        string logFolderPath,
        string tempFolderPath,
        string currentAppRoot,
        string gameDataPath,
        Version programVersion)
    {
        LogFolderPath = logFolderPath;
        TempFolderPath = tempFolderPath;
        CurrentAppRoot = currentAppRoot;
        GameDataPath = gameDataPath;
        ProgramVersion = programVersion;
    }
    
    public async Task DownloadWbBigsAsync(IProgress<int?> downloadProgress,
        IProgress<string?> speedProgress,
        CancellationToken cancel)
    {
        try
        {
            var zipFileName = "WorldBuilderBig.zip";
            speedProgress.Report("downloading config...");
            var rssData = await ReadRSSAsync(FeedUrl,
                "RandomMapGenerator",
                zipFileName,
                downloadProgress,
                speedProgress,
                LogFolderPath,
                TempFolderPath,
                cancel);
            if (rssData is null)
            {
                throw new FileNotFoundException("Failed to retrieve RSS mod data for live content");
            }

            var downloadedFile = Path.Combine(TempFolderPath, zipFileName);

            if (File.Exists(downloadedFile))
            {
                File.Delete(downloadedFile);
            }

            await Aria2BTDownload.DownloadAsync($"{rssData.DirectLink}.torrent",
                TempFolderPath,
                zipFileName,
                downloadProgress,
                speedProgress,
                LogFolderPath,
                cancel);

            await Task.Delay(1500);
            speedProgress.Report("unzipping...");
            await Task.Run(new Action(() =>
            { 
                UnpackZip(downloadedFile, GameDataPath);
            }));
            speedProgress.Report("Done");
            
        }
        catch (Exception e)
        {
            _logger.Error(e);
            MessageBox.Show($"Fail to download WB Big Files: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw;
        }
        
    }
    
    public async Task DownloadTerrainFixAsync(IProgress<int?> downloadProgress,
        IProgress<string?> speedProgress,
        CancellationToken cancel)
    {
        try
        {
            var targetFileName = "TerrainFix.big";
            speedProgress.Report("downloading config...");
            var rssData = await ReadRSSAsync(FeedUrl,
                "RandomMapGenerator",
                targetFileName,
                downloadProgress,
                speedProgress,
                LogFolderPath,
                TempFolderPath,
                cancel);
            if (rssData is null)
            {
                throw new FileNotFoundException("Failed to retrieve RSS mod data for live content");
            }

            var downloadedFile = Path.Combine(TempFolderPath, targetFileName);

            if (File.Exists(downloadedFile))
            {
                File.Delete(downloadedFile);
            }

            await Aria2BTDownload.DownloadAsync($"{rssData.DirectLink}.torrent",
                TempFolderPath,
                targetFileName,
                downloadProgress,
                speedProgress,
                LogFolderPath,
                cancel);
            
            
            File.Copy(downloadedFile, Path.Combine(GameDataPath, targetFileName), true);
            speedProgress.Report("Done");
            
        }
        catch (Exception e)
        {
            _logger.Error(e);
            MessageBox.Show($"Fail to download WB Big Files: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw;
        }
        
    }
    
    private void UnpackZip(string temporaryZipPath, string targetPath)
    {
        //unzip temporaryZipPath to targetPath recursively
        using var archive = ZipArchive.Open(temporaryZipPath);
        foreach (var entry in archive.Entries)
        {
            if (!entry.IsDirectory)
            {
                _logger.Info("UnpackZip: entry");
                entry.WriteToDirectory(targetPath, new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });
            }
        }
        //TODO check files and delete temporaryZipPath in WbLauncher
    }

    // public async Task UpdateLauncherAsync(IProgress<int?> downloadProgress,
    //                                       IProgress<string?> speedProgress,
    //                                       CancellationToken cancel)
    // {
    //
    //     var launcherFileName = string.Format(LauncherRemoteName, VersionData.LauncherVersion);
    //     var rssData = await ReadRSSAsync(FeedUrl,
    //                                      "ra3battlenet",
    //                                      $"{LauncherRemoteParentPath}/{launcherFileName}",
    //                                      downloadProgress,
    //                                      speedProgress,
    //                                      LogFolderPath,
    //                                      TempFolderPath,
    //                                      cancel);
    //     if (rssData is null)
    //     {
    //         throw new FileNotFoundException("Failed to retrieve RSS mod data for live content");
    //     }
    //     var downloadedFile = Path.Combine(TempFolderPath, launcherFileName);
    //
    //     if (File.Exists(downloadedFile))
    //     {
    //         File.Delete(downloadedFile);
    //     }
    //     await Aria2BTDownload.DownloadAsync($"{rssData.DirectLink}.torrent",
    //                                         TempFolderPath,
    //                                         launcherFileName,
    //                                         downloadProgress,
    //                                         speedProgress,
    //                                         LogFolderPath,
    //                                         cancel);
    //
    //     UnpackZip(downloadedFile, CurrentAppRoot);
    // }
    

    public static async Task<RssData?> ReadRSSAsync(string feedUrl,
        string id,
        string targetPath,
        IProgress<int?> downloadProgress,
        IProgress<string?> speedProgress,
        string logFolderPath,
        string tempFolderPath,
        CancellationToken cancel)
    {
        var feedFileName = Path.GetFileName(new Uri(feedUrl).AbsolutePath);
        var feedXmlLocalPath = Path.Combine(tempFolderPath, feedFileName);
        var aria2ControlPath = Aria2Download.GetAria2TemporaryFile(feedXmlLocalPath);
        if (File.Exists(feedXmlLocalPath))
        {
            File.Delete(feedXmlLocalPath);
        }
        if (File.Exists(aria2ControlPath))
        {
            File.Delete(aria2ControlPath);
        }
        _logger.Info("RSS download begin");
        await Aria2Download.DownloadAsync(feedUrl,
            tempFolderPath,
            feedFileName,
            downloadProgress,
            speedProgress,
            logFolderPath,
            cancel);
        _logger.Info("Rss download end");
        downloadProgress.Report(null);
        var feed = await FeedReader.ReadFromFileAsync(feedXmlLocalPath);
        return RssData.Parse(feed, id, targetPath);
    }
}