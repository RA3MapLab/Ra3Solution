using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace Downloader.Core;

public class Updater
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    private static string FeedUrl = "https://hw.file.rmrts.com/srv/feed.xml";
    
    public VersionData VersionData;
    public string LogFolderPath { get; set; }
    public string TempFolderPath { get; set; }
    // Must be a format string such as "launcher"
    public string LauncherRemoteParentPath { get; set; }
    // Must be a format string such as "RA3BattleNet_Setup_{0}.exe"
    public string LauncherRemoteName { get; set; }
    public string CurrentAppRoot { get; set; }
    public Version? ProgramVersion { get; set; }
    
    public Updater(
        string logFolderPath,
        string tempFolderPath,
        string launcherRemotePath,
        string launcherRemoteName,
        string currentAppRoot,
        Version programVersion)
    {
        LogFolderPath = logFolderPath;
        TempFolderPath = tempFolderPath;
        LauncherRemoteParentPath = launcherRemotePath;
        LauncherRemoteName = launcherRemoteName;
        CurrentAppRoot = currentAppRoot;
        ProgramVersion = programVersion;
    }
    
    public async Task UpdateLauncherAsync(IProgress<int?> downloadProgress,
                                          IProgress<string?> speedProgress,
                                          CancellationToken cancel)
    {

        var launcherFileName = string.Format(LauncherRemoteName, VersionData.LauncherVersion);
        var rssData = await ReadRSSAsync(FeedUrl,
                                         "ra3battlenet",
                                         $"{LauncherRemoteParentPath}/{launcherFileName}",
                                         downloadProgress,
                                         speedProgress,
                                         LogFolderPath,
                                         TempFolderPath,
                                         cancel);
        if (rssData is null)
        {
            throw new FileNotFoundException("Failed to retrieve RSS mod data for live content");
        }
        var downloadedFile = Path.Combine(TempFolderPath, launcherFileName);

        if (File.Exists(downloadedFile))
        {
            File.Delete(downloadedFile);
        }
        await Aria2BTDownload.DownloadAsync($"{rssData.DirectLink}.torrent",
                                            TempFolderPath,
                                            launcherFileName,
                                            downloadProgress,
                                            speedProgress,
                                            LogFolderPath,
                                            cancel);

        UnpackZip(downloadedFile, CurrentAppRoot);
    }

    private void UnpackZip(string temporaryZipPath, string targetPath)
    {
        ZipFile.ExtractToDirectory(temporaryZipPath, targetPath);
        File.Delete(temporaryZipPath);
    }

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