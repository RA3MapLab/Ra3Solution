using System;
using System.Text;

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
                throw new Exception("线程获取失败");
            }

            IntPtr dllHandle = NativeMethod.LoadLibrary(dllPath);

            if (dllHandle == IntPtr.Zero)
            {
                throw new Exception("dll加载失败");
            }

            IntPtr procAddress = NativeMethod.GetProcAddress(dllHandle, MouseHookFunc);
            if (procAddress == IntPtr.Zero)
            {
                throw new Exception("dll鼠标钩子函数获取失败");
            }

            // hookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_MSGFILTER, procAddress, dllHandle, threadId);
            MouseHookHandle = NativeMethod.SetWindowsHookEx(HookType.WH_GETMESSAGE, procAddress, dllHandle, threadId);
            if (MouseHookHandle == IntPtr.Zero)
            {
                throw new Exception("dll鼠标钩子函数注入失败");
            }
            else
            {
                Logger.log("dll鼠标钩子函数注入成功");
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

    }
}