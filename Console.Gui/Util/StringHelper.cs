using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConGui.Util;
public static class StringHelper
{
    public static string StripHash(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        if (str.Contains("#"))
            return str[0..(str.IndexOf('#'))];
        return str;
    }


    public static string PadLength(this string str, int length)
    {
        string output = str;
        if (output.Length > length)
            output = output[0..length];
        else if (output.Length < length)
            output = output + new string(' ', length - output.Length);
        return output;
    }
}
