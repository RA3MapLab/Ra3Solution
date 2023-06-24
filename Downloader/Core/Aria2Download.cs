using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Core;

public class Aria2Download : Aria2DownloadBase
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    protected override string Aria2Arguments =>
        $" -c -d \"{DirectoryName}\" -o \"{FileName}\" --file-allocation=none --check-certificate=false \"{Url}\"";

    public string FileName { get; }

    protected Aria2Download(string toolPath,
                            string url,
                            string directoryName,
                            string fileName,
                            IProgress<int?> downloadProgress,
                            IProgress<string?> speedProgress,
                            string logFolderPath) :
        base(toolPath, url, directoryName, downloadProgress, speedProgress, logFolderPath)
    {
        FileName = fileName;
    }

    public static async Task DownloadAsync(string link,
                                           string outputDirectory,
                                           string fileName,
                                           IProgress<int?> downloadProgress,
                                           IProgress<string?> speedProgress,
                                           string logFolderPath,
                                           CancellationToken cancel)
    {
        var toolPath = Path.Combine(Directory.GetCurrentDirectory(), "aria2c.exe");
        using var process = new Aria2Download(toolPath,
                                              link,
                                              outputDirectory,
                                              fileName,
                                              downloadProgress,
                                              speedProgress,
                                              logFolderPath);
        _logger.Info("Launching Aria2 Process...");
        await process.ExecuteProcessParseProcessOutput(cancel);
        _logger.Info("Download completed!");
    }

    protected override void VerifyProcessResult()
    {
        base.VerifyProcessResult();
        var finalPath = Path.Combine(DirectoryName, FileName);
        var fileInfo = new FileInfo(finalPath);
        var isDownloadCompleted = fileInfo.Exists && fileInfo.Length > 0 && !File.Exists($"{finalPath}.aria2");
        if (!isDownloadCompleted)
        {
            throw new Exception("Download not completed");
        }
    }
}