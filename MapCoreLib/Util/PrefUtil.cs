using System;
using System.Collections.Generic;
using MapCoreLib.Util;

namespace MapCoreLib.Log
{
    internal static class PrefUtil
    {
        private static Dictionary<string, long> record = new Dictionary<string, long>();

        public static void start(string tag)
        {
            record.put(tag, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public static void stop(string tag)
        {
            LogUtil.debug($"{tag} | consume time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - record[tag]}ms");
        }
    }
}