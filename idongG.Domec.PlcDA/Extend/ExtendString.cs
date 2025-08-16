using System.Text;
using System.Text.RegularExpressions;

namespace idongG.Domec.PlcDA.Extend;

/// <summary>
/// 扩展字符串函数 类
/// </summary>
public static class ExtendString
{ /// <summary>
  /// 字符串转枚举
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="Name"></param>
  /// <returns></returns>
    public static T ToEnum<T>(this string Name)
    {
        return (T)Enum.Parse(typeof(T), Name, true);
    }

    /// <summary>
    /// unicode转字符串
    /// </summary>
    /// <param name="unicodeStr">\u535a\u5ba2\u56ed</param>
    /// <returns></returns>
    public static string UnicodeToChinese(this string unicodeStr)
    {
        return Regex.Unescape(unicodeStr);
    }

    /// <summary>
    /// 获取合法文件名,windows文件名不能带有< > : " / \ | * ?
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static StringBuilder GenFileName(this string name)
    {
        var sb = new StringBuilder();
        if (string.IsNullOrEmpty(name)) return sb;

        // < > : " / \ | * ?
        foreach (var item in name)
        {
            if (item != '<' && item != '>' && item != ':' && item != '"' && item != '/' && item != '\\' && item != '|' && item != '*' && item != '?')
            {
                sb.Append(item);
            }
        }
        return sb;
    }

    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

    //public static T ToEnum<T>(this string[] arys)
    //{
    //    foreach (var item in arys)
    //    {
    //    }
    //    arys.ForEach(x => { ToEnum<T>(x); });
    //}

    /// <summary>
    ///  sqlite 时间格式"yyyy-MM-dd HH:mm:ss.fff"
    /// </summary>
    /// <param name="str"></param>
    public static string ToStringDateTime(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }

    /// <summary>
    ///  sqlite 时间格式"yyyy-MM-dd HH:mm:ss.fff"
    /// </summary>
    /// <param name="str"></param>
    public static string ToDtString(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }

    /// <summary>
    ///  sqlite 时间格式"yyyy-MM-dd HH:mm:ss.fff"
    /// </summary>
    /// <param name="str"></param>
    public static string ToIntegerString(this DateTime dt)
    {
        return dt.ToString("yyyyMMddHHmmssfff");
    }

    /// <summary>
    ///  sqlite 时间格式"HH:mm:ss.fff"
    /// </summary>
    /// <param name="str"></param>
    public static string ToTimeString(this DateTime dt)
    {
        return dt.ToString("HH:mm:ss:fff");
    }

    /// <summary>
    /// 00 00 01 02 04=>0000010204
    /// </summary>
    /// <param name="strWithSpace"></param>
    /// <returns></returns>
    public static string ToStringWithOutSpace(this string strWithSpace)
    {
        return strWithSpace.Replace(" ", "");
    }

    /// <summary>
    /// 判断IP
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool IsIPAddress(string str)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;
            const string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}{1}";
            var regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string GetAddressStringPart(this string address)
    {
        var sb = new StringBuilder();
        foreach (var c in address)
        {
            if (char.IsLetter(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static int GetAddressIntPart(this string address)
    {
        var sb = new StringBuilder();
        foreach (var c in address)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
        }
        return Convert.ToInt32(sb.ToString());
    }

    private const int DBC_CHAR_START = 33; // 半角字符起始值
    private const int DBC_CHAR_END = 126; // 半角字符结束值
    private const int SBC_CHAR_START = 65281; // 全角字符起始值
    private const int SBC_CHAR_END = 65374; // 全角字符结束值
    private const int CONVERT_STEP = 65248; // 全角半角转换间隔

    public static string StrConv(this string input)
    {
        StringBuilder result = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (c >= SBC_CHAR_START && c <= SBC_CHAR_END)
            {
                result.Append((char)(c - CONVERT_STEP));
            }
            else if (c == 12288) // 全角空格转换为半角空格
            {
                result.Append((char)32);
            }
            else
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }
}