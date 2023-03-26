using System;
using System.Runtime.InteropServices;
using System.Text;

namespace wbInject.Core
{
    public static class NativeMethod
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, int lparam);
        
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllPath);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr dllHandle, string procName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType dllHandle, IntPtr procAddress, IntPtr dllhandle,
            uint threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr hookHandle);
        
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
}