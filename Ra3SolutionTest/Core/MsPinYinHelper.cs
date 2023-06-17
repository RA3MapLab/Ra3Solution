using System;
using System.IO;
using Microsoft.International.Converters.PinYinConverter;

namespace NewMapParser.Core
{
    public class MsPinYinHelper
{

    public static void Run()
    {
        var text = File.ReadAllText(@"C:\Users\29972\AppData\Roaming\Red Alert 3\Maps\[MRZYQH\[MRZYQH.edit.xml");
        File.WriteAllText(@"C:\Users\29972\AppData\Roaming\Red Alert 3\Maps\[MRZYQH\[MRZYQH22.edit.xml", PinYin(text));
    }
    
    
    #region 汉子转拼音
    /// <summary>
    /// 汉字转全拼
    /// </summary>
    /// <param name="chinese">汉字</param>
    /// <returns></returns>
    public static string PinYin(string chinese)
    {
        string retValue = string.Empty;
 
        foreach (char chr in chinese)
        {
            try
            {
                if (ChineseChar.IsValidChar(chr))
                {
                    ChineseChar chineseChar = new ChineseChar(chr);
                    string t = chineseChar.Pinyins[0].ToString();
                    retValue += t.Substring(0, t.Length - 1);
                }
                else
                {
                    retValue += chr.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("全拼转化出错！" + e.Message);
                return string.Empty;
            }
        }
 
        return retValue;
    }
    /// <summary>
    /// 汉字转首拼
    /// </summary>
    /// <param name="chinese">汉字</param>
    /// <returns></returns>
    public static string FirstPinYin(string chinese)
    {
        string retValue = string.Empty;
 
        foreach (char chr in chinese)
        {
            try
            {
                if (ChineseChar.IsValidChar(chr))
                {
                    ChineseChar chineseChar = new ChineseChar(chr);
                    string t = chineseChar.Pinyins[0].ToString();
                    retValue += t.Substring(0, 1);
                }
                else
                {
                    retValue += chr.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("首字母转化出错！" + e.Message);
                return string.Empty;
            }
        }
 
        return retValue;
    }
    #endregion
 
    #region 判断多音字
    /// <summary>
    /// 判断汉字是否是多音字
    /// </summary>
    /// <param name="chr">单个汉字字符</param>
    /// <returns></returns>
    public static bool IsPolyphone(char chr)
    {
        if (ChineseChar.IsValidChar(chr))
        {
            ChineseChar chineseChar = new ChineseChar(chr);
            return chineseChar.IsPolyphone;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 判断汉字是否是多音字
    /// </summary>
    /// <param name="chinese">字符串形式的汉字，如果是多个，只判断首字</param>
    /// <returns></returns>
    public static bool IsPolyphone(string chinese)
    {
        if (ChineseChar.IsValidChar(chinese[0]))
        {
            ChineseChar chineseChar = new ChineseChar(chinese[0]);
            return chineseChar.IsPolyphone;
        }
        else
        {
            return false;
        }
    }
    #endregion
 
    #region 获取拼音个数
    /// <summary>
    /// 取得汉字拼音个数
    /// </summary>
    /// <param name="chinese">汉字字符串，非汉字字符不算长度</param>
    /// <returns></returns>
    public static int PinYinCount(string chinese)
    {
        int retCount = 0;
 
        foreach (char chr in chinese)
        {
            if (ChineseChar.IsValidChar(chr))
            {
                ChineseChar chineseChar = new ChineseChar(chr);
                retCount += chineseChar.PinyinCount;
            }
        }
 
        return retCount;
    }
    /// <summary>
    /// 取得汉字拼音个数
    /// </summary>
    /// <param name="chr">单个汉字字符，非汉字返回0</param>
    /// <returns></returns>
    public static int PinYinCount(char chr)
    {
        if (ChineseChar.IsValidChar(chr))
        {
            ChineseChar chineseChar = new ChineseChar(chr);
            return chineseChar.PinyinCount;
        }
        else
        {
            return 0;
        }
    }
    #endregion
 
    #region 获取笔画数
    /// <summary>
    /// 取得汉字笔画数
    /// </summary>
    /// <param name="chinese">汉字字符串，非汉字的字符不算笔画</param>
    /// <returns></returns>
    public static int StrokeNumber(string chinese)
    {
        int retCount = 0;
 
        foreach (char chr in chinese)
        {
            if (ChineseChar.IsValidChar(chr))
            {
                ChineseChar chineseChar = new ChineseChar(chr);
                retCount += chineseChar.StrokeNumber;
            }
        }
 
        return retCount;
    }
    /// <summary>
    /// 取得汉字笔画数
    /// </summary>
    /// <param name="chr">单个汉字字符，非汉字返回0</param>
    /// <returns></returns>
    public static int StrokeNumber(char chr)
    {
        if (ChineseChar.IsValidChar(chr))
        {
            ChineseChar chineseChar = new ChineseChar(chr);
            return chineseChar.StrokeNumber;
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
}