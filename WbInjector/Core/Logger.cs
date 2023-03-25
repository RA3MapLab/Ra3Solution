using System;
using MapCoreLib.Util;

namespace wbInject.Core
{
    public static class Logger
    {
        
        public static void log(string tag, string msg)
        {
            LogUtil.log(tag, msg);
        }
        
        public static void log(string msg)
        {
            LogUtil.log(msg);
        }
        
        public static void close()
        {
            LogUtil.close();
        }
    }
}