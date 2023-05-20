using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Core;

public sealed class Aria2BTDownload : Aria2DownloadBase
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    protected override string Aria2Arguments => GenerateArguments();
    public bool FollowTorrent { get; }
    public string TargetFile { get; }

    private Aria2BTDownload(string toolPath,
                            string url,
                            string directoryName,
                            string targetFile,
                            IProgress<int?> downloadProgress,
                            IProgress<string?> speedProgress,
                            string logFolderPath,
                            bool followTorrent) :
        base(toolPath, url, directoryName, downloadProgress, speedProgress, logFolderPath)
    {
        FollowTorrent = followTorrent;
        TargetFile = targetFile;
    }

    public static async Task DownloadAsync(string link,
                                           string outputDirectory,
                                           string targetFile,
                                           IProgress<int?> downloadProgress,
                                           IProgress<string?> speedProgress,
                                           string logFolderPath,
                                           CancellationToken cancel)
    {
        var torrentFileName = Path.GetFileName(new Uri(link).AbsolutePath);
        _logger.Info("torrent file download begin");
        await DoDownloadAsync(link,
                              outputDirectory,
                              torrentFileName,
                              ProgressUtils.NoOp<int?>(), // 下载种子文件就不用进度条了
                              speedProgress,
                              logFolderPath,
                              followTorrent: false,
                              cancel);
        _logger.Info("torrent file download end");
        var torrentFilePath = Path.Combine(outputDirectory, torrentFileName);
        if (!File.Exists(torrentFilePath))
        {
            throw new FileNotFoundException("Downloaded torrent file not found", torrentFilePath);
        }
        _logger.Info("actual content download begin");
        await DoDownloadAsync(Path.GetFullPath(torrentFilePath),
                              outputDirectory,
                              targetFile,
                              downloadProgress,
                              speedProgress,
                              logFolderPath,
                              followTorrent: true,
                              cancel);
        _logger.Info("actual content download end");
    }

    private static async Task DoDownloadAsync(string link,
                                              string outputDirectory,
                                              string targetFile,
                                              IProgress<int?> downloadProgress,
                                              IProgress<string?> speedProgress,
                                              string logFolderPath,
                                              bool followTorrent,
                                              CancellationToken cancel)
    {
        var toolPath = Path.Combine("aria2c.exe");
        using var process = new Aria2BTDownload(toolPath,
                                                link,
                                                outputDirectory,
                                                targetFile,
                                                downloadProgress,
                                                speedProgress,
                                                logFolderPath,
                                                followTorrent);
        _logger.Info($"Launching Aria2 Process (follow torrent: {followTorrent}) for BT download...");
        await process.ExecuteProcessParseProcessOutput(cancel);
        _logger.Info("Download complete!");
    }

    protected override void VerifyProcessResult()
    {
        base.VerifyProcessResult();

        var folderPath = DirectoryName;
        var fileInfo = new FileInfo(Path.Combine(folderPath, TargetFile));
        var isDownloadCompleted = fileInfo.Exists && fileInfo.Length > 0 && !File.Exists($"{folderPath}.aria2");
        if (!isDownloadCompleted)
        {
            throw new Exception("Download not completed");
        }
    }

    private string GenerateArguments()
    {
        using var writer = new StringWriter();
        writer.Write(" -d \"{0}\"", DirectoryName);
        writer.Write(" --file-allocation=none --check-certificate=false --seed-time=0");
        writer.Write(" --bt-enable-lpd=true --allow-overwrite=true --dht-listen-port=36881-36999 --listen-port=26881-26999 ");
        writer.Write(" --follow-torrent={0}", FollowTorrent ? "true" : "false");
        writer.Write(" \"{0}\"", Url);
        return writer.ToString();
    }
}