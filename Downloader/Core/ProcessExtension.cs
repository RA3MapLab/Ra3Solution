using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Core;

public static class ProcessExtension
{
    public static Task WaitForExitAsync(this Process process, 
        CancellationToken cancellationToken = default(CancellationToken))
    {
        if (process.HasExited) return Task.FromResult(process.ExitCode);

        var tcs = new TaskCompletionSource<object>();
        process.EnableRaisingEvents = true;
        process.Exited += (sender, args) => tcs.TrySetResult(null);
        if(cancellationToken != default(CancellationToken))
            cancellationToken.Register(() => tcs.SetCanceled());

        return process.HasExited ? Task.FromResult(process.ExitCode) : tcs.Task;
    }
    
    public static void KillTree(this Process process)
    {
        KillProcessAndChildren(process.Id);
    }
    
    private static void KillProcessAndChildren(int pid)
    {
        // Cannot close 'system idle process'.
        if (pid == 0)
        {
            return;
        }
        ManagementObjectSearcher searcher = new ManagementObjectSearcher
            ("Select * From Win32_Process Where ParentProcessID=" + pid);
        ManagementObjectCollection moc = searcher.Get();
        foreach (ManagementObject mo in moc)
        {
            KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
        }
        try
        {
            Process proc = Process.GetProcessById(pid);
            proc.Kill();
        }
        catch (ArgumentException)
        {
            // Process already exited.
        }
    }

}