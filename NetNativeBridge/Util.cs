using System.Text.RegularExpressions;

namespace NetNativeBridge
{
    public class Util
    {
        private static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
        
        public static bool IsChinese(char c)
        {
            return cjkCharRegex.IsMatch(c.ToString());
        }
    }
}