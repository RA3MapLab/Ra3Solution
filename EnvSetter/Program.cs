using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Runtime.InteropServices;

namespace EnvSetter
{
    internal class Program
    {
        [DllImport("msi.dll")]
        private static extern INSTALLSTATE MsiQueryProductState(string product);
        public static INSTALLSTATE GetProcuct(string product)
        {
            INSTALLSTATE state = MsiQueryProductState(product);
            return state;
        }
        
        public static int Main(string[] args)
        {
            try
            {
                var state = MsiQueryProductState("{7299052B-02A4-4627-81F2-1818DA5D550D}");
                if (state != INSTALLSTATE.INSTALLSTATE_DEFAULT)
                {
                    var processStartInfo = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(Directory.GetCurrentDirectory(), "vcredist_x86.exe"),
                        WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory()),
                        Arguments = @"/q:a /c:\""msiexec /i vcredist.msi /qn /l*v %temp%\\vcredist_x86.log\""",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    var process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    state = MsiQueryProductState("{7299052B-02A4-4627-81F2-1818DA5D550D}");
                    if (state != INSTALLSTATE.INSTALLSTATE_DEFAULT)
                    {
                        return -1;
                    }

                    return 0;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

            return -1;
        }
        
        public enum INSTALLSTATE
        {
            INSTALLSTATE_NOTUSED = -7,  // component disabled
            INSTALLSTATE_BADCONFIG = -6,  // configuration data corrupt
            INSTALLSTATE_INCOMPLETE = -5,  // installation suspended or in progress
            INSTALLSTATE_SOURCEABSENT = -4,  // run from source, source is unavailable
            INSTALLSTATE_MOREDATA = -3,  // return buffer overflow
            INSTALLSTATE_INVALIDARG = -2,  // invalid function argument
            INSTALLSTATE_UNKNOWN = -1,  // unrecognized product or feature
            INSTALLSTATE_BROKEN = 0,  // broken
            INSTALLSTATE_ADVERTISED = 1,  // advertised feature
            INSTALLSTATE_REMOVED = 1,  // component being removed (action state, not settable)
            INSTALLSTATE_ABSENT = 2,  // uninstalled (or action state absent but clients remain)
            INSTALLSTATE_LOCAL = 3,  // installed on local drive
            INSTALLSTATE_SOURCE = 4,  // run from source, CD or net
            INSTALLSTATE_DEFAULT = 5,  // use default, local or source
        }
    }
}