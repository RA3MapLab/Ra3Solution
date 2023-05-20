using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Core;

public abstract class ProcessWrapper: IDisposable
{
    public abstract string ProcessPath { get; }
    public abstract string Arguments { get; }
    public Process? Process { get; private set; }
    public bool KillAfterExit { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void ParseProcessOutput(string? output);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            // process will be killed by the ProcessTracker anyway
            return;
        }

        try
        {
            if (KillAfterExit && Process?.HasExited == false)
            {
                //App.Log($"Trying to kill process {ProcessPath ?? "null"}...");
                Process?.KillTree();
            }
        }
        catch (InvalidOperationException) when (Process?.HasExited is true)
        {
            /* ignore */
        }
        finally
        {
            Process?.Dispose();
            Process = null;
        }
    }

    ~ProcessWrapper()
    {
        Dispose(false);
    }

    protected virtual Process CreateAndStartProcess()
    {
        //App.Log($"Process [{ProcessPath}] Commandline: [{Arguments}]");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ProcessPath,
                Arguments = Arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            }
        };

        try
        {
            process.Start(); //启动进程
            return process;
        }
        catch
        {
            process.Dispose(); // 只在出错时进行清理，否则由调用方来清理
            throw;
        }
    }

    public async Task ExecuteProcessParseProcessOutput(CancellationToken cancel)
    {
        if (Process != null)
        {
            throw new InvalidOperationException("Process already started");
        }

        Process = CreateAndStartProcess();
        // 输出信息重定向
        IProgress<string?> progress = new Progress<string?>(ParseProcessOutput);
        Process.OutputDataReceived += (_, e) => progress.Report(e.Data);
        Process.ErrorDataReceived += (_, e) => progress.Report(e.Data);

        //if (!AppData.LauncherConfig.DownloadAfterExit)
        //{
        //    KillAfterExit = true;
        //    App.TrackChildProcess(Process); // 开始监测进程
        //}

        Process.BeginOutputReadLine();
        Process.BeginErrorReadLine();
        try
        {
            await Process.WaitForExitAsync(cancel); // 等待进程结束
        }
        catch (OperationCanceledException)
        {
            // 假如是被取消了，那么终止进程
            try
            {
                Process.KillTree();
            }
            catch
            {
                //App.Log("Process Kill failed: " + e);
                if (Process?.HasExited is not true)
                {
                    throw;
                }
                // 假如进程已经退出了，那就不用管了（
            }
        }
        VerifyProcessResult(); // 检查结果，一旦失败，抛出异常
    }

    protected virtual void ParseErrorOutput(string? data)
    {
        ParseProcessOutput(data);
    }

    protected virtual void VerifyProcessResult()
    {
        if (Process?.ExitCode != 0)
        {
            throw new Exception($"Process {ProcessPath} exited with non zero result: {Process?.ExitCode}");
        }
    }
}