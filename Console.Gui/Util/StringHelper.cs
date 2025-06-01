using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
