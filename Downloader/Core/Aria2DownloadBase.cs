using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Downloader.Core;

public abstract class Aria2DownloadBase : ProcessWrapper
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    private static readonly Regex _progressRegex = new("[(](.*?)[)]");
    private static readonly Regex _speedRegex = new("[a-zA-Z0-9.,]*");
    private readonly IProgress<int?> _downloadProgress;
    private readonly IProgress<string?> _speedProgress;
    private readonly string _logFolderPath;
    private string Aria2LogFilePath => Path.Combine(_logFolderPath, "aria2c.exe.log.txt");
    private string Aria2LogArgument => $"-l \"{Aria2LogFilePath}\"";
    private string Aria2CurrentExtraArgument => true ? Aria2LogArgument : string.Empty;
    protected abstract string Aria2Arguments { get; }
    public override sealed string Arguments => $"{Aria2CurrentExtraArgument} {Aria2Arguments}";
    public override string ProcessPath { get; }
    public string Url { get; }
    public string DirectoryName { get; }

    protected Aria2DownloadBase(string toolPath,
                                string url,
                                string directoryName,
                                IProgress<int?> downloadProgress,
                                IProgress<string?> speedProgress,
                                string logFolderPath)
    {
        ProcessPath = toolPath;
        Url = url;
        DirectoryName = directoryName.TrimEnd('\\');
        _downloadProgress = downloadProgress;
        _speedProgress = speedProgress;
        _logFolderPath = logFolderPath;
    }

    public static string GetAria2TemporaryFile(string originalFile)
    {
        return $"{originalFile}.aria2";
    }

    protected override void ParseProcessOutput(string? output)
    {
        ShowInfo(output);
    }

    private void ShowInfo(string? a)
    {
        if (a == null)
        {
            return;
        }

        int? newProgress = null;
        string? newSpeed = null;
        try
        {
            DoParse(a, out newProgress, out newSpeed);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to parse info.");
        }
        if (newProgress is null && newSpeed is null)
        {
            if (!string.IsNullOrWhiteSpace(a))
            {
                _logger.Error("Unparsed output: {0}", a);
            }
            return;
        }
        _downloadProgress.Report(newProgress);
        _speedProgress.Report(newSpeed);
    }

    private static void DoParse(string text, out int? newProgress, out string? newSpeed)
    {
        newProgress = null;
        newSpeed = null;
        foreach (var splitted in text.Split())
        {
            if (splitted.Contains("%"))
            {
                if (_progressRegex.Match(splitted) is { Success: true } m && m.Groups.Count > 1)
                {
                    if (float.TryParse(m.Groups[1].Value.TrimEnd('%'), out var candidate))
                    {
                        if (candidate >= 0 && candidate <= 100)
                        {
                            newProgress = (int)candidate;
                        }
                    }
                }
            }
            if (splitted.Contains("DL"))
            {
                if (splitted.Split(':') is { Length: > 1 } parts)
                {
                    if (_speedRegex.Match(parts[1]) is { Success: true } m)
                    {
                        newSpeed = m.Value + "/s";
                    }
                }
            }
        }
    }
}