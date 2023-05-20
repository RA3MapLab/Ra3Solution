using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wbInject.Core
{
    public class WbInjector
    {
        
        private string WbTitle = "World Builder";
        private string dllPath = "wbCore.dll";
        private string MouseHookFunc = "stubProc";
        private string msgFilterFunc = "msgFilterProc";
        
        public IntPtr wbHWND = IntPtr.Zero;
        public IntPtr MouseHookHandle = IntPtr.Zero;

        private string WbPath = @"D:\file\yule\ra3\Data\WorldBuilder_Mod_1.12.exe";
        // public IntPtr ThreadHandle = IntPtr.Zero;
        // private uint PID = 0;
        private PROCESS_INFORMATION pi;

        public void injectDll2Wb()
        {
            NativeMethod.EnumWindows(SearchWBProc, 0);
            Logger.log($"wbHWND: {wbHWND}");

            if (wbHWND == IntPtr.Zero)
            {
                throw new Exception("找不到地图编辑器\n请检查你是否打开了地编窗口或者未使用英文版地图编辑器");
            }

            uint processId, threadId;
            threadId = NativeMethod.GetWindowThreadProcessId(wbHWND, out processId);

            if (threadId == 0)
            {
                throw new Exception($"线程获取失败 {NativeMethod.GetLastError()}");
            }

            IntPtr dllHandle = NativeMethod.LoadLibrary(dllPath);

            if (dllHandle == IntPtr.Zero)
            {
                throw new Exception($"dll加载失败 {NativeMethod.GetLastError()}");
            }

            IntPtr procAddress = NativeMethod.GetProcAddress(dllHandle, MouseHookFunc);
            if (procAddress == IntPtr.Zero)
            {
                throw new Exception($"dll钩子函数获取失败 {NativeMethod.GetLastError()}");
            }

            // hookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_MSGFILTER, procAddress, dllHandle, threadId);
            MouseHookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_GETMESSAGE, procAddress, dllHandle, threadId);
            if (MouseHookHandle == IntPtr.Zero)
            {
                throw new Exception($"dll钩子函数注入失败 {NativeMethod.GetLastError()}");
            }
            else
            {
                Logger.log("dll钩子函数注入成功");
            }

        }

        public bool SearchWBProc(IntPtr hWnd, int lparam)
        {
            StringBuilder sb = new StringBuilder(1024);
            NativeMethod.GetWindowText(hWnd, sb, sb.Capacity);
            if (sb.ToString().Contains(WbTitle))
            {
                wbHWND = hWnd;
                return false; // Found the wnd, halt enumeration
            }

            return true;
        }

        public void detachDll()
        {
            if (MouseHookHandle != IntPtr.Zero)
            {
                int res = NativeMethod.UnhookWindowsHookEx(MouseHookHandle);
                if (res == 0)
                {
                    Logger.log("dll鼠标钩子脱离失败");
                }
                else
                {
                    Logger.log("dll鼠标钩子脱离成功");
                }
                
            }
        }
        
        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;
        
        
        
        
        public async Task startWBAndInject()
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = WbPath,
                WorkingDirectory = @"D:\file\yule\ra3\Data",
            };
            Process.Start(processInfo);
            
            

            Logger.log("pi.dwProcessId : " + pi.dwProcessId);
            // NativeMethod.ResumeThread(pi.hThread);
            // await Task.Delay(2000);
            // NativeMethod.SuspendThread(pi.hThread);



            // #region dllmain
            //
            // var moduleHandle = NativeMethod.GetModuleHandle("kernel32.dll");
            //
            // if (moduleHandle == IntPtr.Zero)
            // {
            //     throw new Exception($"GetModuleHandle fail：{NativeMethod.GetLastError()}");
            //     
            // }
            //
            // IntPtr loadLibraryAddr = NativeMethod.GetProcAddress(moduleHandle, "LoadLibraryA");
            //
            // if (loadLibraryAddr == IntPtr.Zero)
            // {
            //     throw new Exception($"GetProcAddress fail：{NativeMethod.GetLastError()}");
            // }
            //
            // // alocating some memory on the target process - enough to store the name of the dll
            // // and storing its address in a pointer
            // IntPtr allocMemAddress = NativeMethod.VirtualAllocEx(pi.hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            //
            // if (allocMemAddress == IntPtr.Zero)
            // {
            //     throw new Exception($"VirtualAllocEx fail：{NativeMethod.GetLastError()}");
            // }
            //
            // // writing the name of the dll there
            // UIntPtr bytesWritten;
            // success = NativeMethod.WriteProcessMemory(pi.hProcess, allocMemAddress, Encoding.Default.GetBytes(dllPath), (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
            //
            // if (!success)
            // {
            //     throw new Exception($"WriteProcessMemory fail：{NativeMethod.GetLastError()}");
            // }
            //
            // // creating a thread that will call LoadLibraryA with allocMemAddress as argument
            // var remoteThreadHandle = NativeMethod.CreateRemoteThread(pi.hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
            // if (remoteThreadHandle == IntPtr.Zero)
            // {
            //     throw new Exception($"CreateRemoteThread fail：{NativeMethod.GetLastError()}");
            // }
            //
            // #endregion
            
            
            
            
            
            NativeMethod.ResumeThread(pi.hThread);
            
            IntPtr dllHandle = NativeMethod.LoadLibrary(dllPath);
            
            if (dllHandle == IntPtr.Zero)
            {
                throw new Exception($"dll加载失败 {NativeMethod.GetLastError()}");
            }
            
            IntPtr procAddress = NativeMethod.GetProcAddress(dllHandle, MouseHookFunc);
            if (procAddress == IntPtr.Zero)
            {
                throw new Exception($"dll钩子函数获取失败 {NativeMethod.GetLastError()}");
            }
            
            MouseHookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_MOUSE, procAddress, dllHandle, pi.dwThreadId);
            if (MouseHookHandle == IntPtr.Zero)
            {
                throw new Exception($"dll钩子函数注入失败 {NativeMethod.GetLastError()}");
            }
            else
            {
                Logger.log("dll钩子函数注入成功");
            }

        }
        
        
        
        
        

        // public async Task startWBAndInject()
        // {
        //     STARTUPINFO si = new STARTUPINFO();
        //     pi = new PROCESS_INFORMATION();
        //     bool success = NativeMethod.CreateProcess(WbPath, null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.CREATE_SUSPENDED, IntPtr.Zero, null, ref si, out pi);
        //
        //     if (!success)
        //     {
        //         throw new Exception($"CreateProcess fail：{NativeMethod.GetLastError()}");
        //     }
        //
        //     Logger.log("pi.dwProcessId : " + pi.dwProcessId);
        //     // NativeMethod.ResumeThread(pi.hThread);
        //     // await Task.Delay(2000);
        //     // NativeMethod.SuspendThread(pi.hThread);
        //
        //
        //
        //     #region dllmain
        //
        //     var moduleHandle = NativeMethod.GetModuleHandle("kernel32.dll");
        //     
        //     if (moduleHandle == IntPtr.Zero)
        //     {
        //         throw new Exception($"GetModuleHandle fail：{NativeMethod.GetLastError()}");
        //         
        //     }
        //     
        //     IntPtr loadLibraryAddr = NativeMethod.GetProcAddress(moduleHandle, "LoadLibraryA");
        //     
        //     if (loadLibraryAddr == IntPtr.Zero)
        //     {
        //         throw new Exception($"GetProcAddress fail：{NativeMethod.GetLastError()}");
        //     }
        //     
        //     // alocating some memory on the target process - enough to store the name of the dll
        //     // and storing its address in a pointer
        //     IntPtr allocMemAddress = NativeMethod.VirtualAllocEx(pi.hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
        //     
        //     if (allocMemAddress == IntPtr.Zero)
        //     {
        //         throw new Exception($"VirtualAllocEx fail：{NativeMethod.GetLastError()}");
        //     }
        //     
        //     // writing the name of the dll there
        //     UIntPtr bytesWritten;
        //     success = NativeMethod.WriteProcessMemory(pi.hProcess, allocMemAddress, Encoding.Default.GetBytes(dllPath), (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
        //     
        //     if (!success)
        //     {
        //         throw new Exception($"WriteProcessMemory fail：{NativeMethod.GetLastError()}");
        //     }
        //     
        //     // creating a thread that will call LoadLibraryA with allocMemAddress as argument
        //     var remoteThreadHandle = NativeMethod.CreateRemoteThread(pi.hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
        //     if (remoteThreadHandle == IntPtr.Zero)
        //     {
        //         throw new Exception($"CreateRemoteThread fail：{NativeMethod.GetLastError()}");
        //     }
        //
        //     #endregion
        //     
        //     
        //     
        //     
        //     
        //     NativeMethod.ResumeThread(pi.hThread);
        //     
        //     IntPtr dllHandle = NativeMethod.LoadLibrary(dllPath);
        //     
        //     if (dllHandle == IntPtr.Zero)
        //     {
        //         throw new Exception($"dll加载失败 {NativeMethod.GetLastError()}");
        //     }
        //     
        //     IntPtr procAddress = NativeMethod.GetProcAddress(dllHandle, MouseHookFunc);
        //     if (procAddress == IntPtr.Zero)
        //     {
        //         throw new Exception($"dll钩子函数获取失败 {NativeMethod.GetLastError()}");
        //     }
        //     
        //     MouseHookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_MOUSE, procAddress, dllHandle, pi.dwThreadId);
        //     if (MouseHookHandle == IntPtr.Zero)
        //     {
        //         throw new Exception($"dll钩子函数注入失败 {NativeMethod.GetLastError()}");
        //     }
        //     else
        //     {
        //         Logger.log("dll钩子函数注入成功");
        //     }
        //
        // }
    }
}