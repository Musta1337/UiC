using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Enums;

namespace UiC.Core.Extensions
{
    public static class CharExtensions
    {
        public static bool CharIsHex(this char c)
        {
            return (c >= '0' && c <= '9') ||
                     (c >= 'a' && c <= 'f') ||
                     (c >= 'A' && c <= 'F');
        }

        public static string RemoveColors(this string str)
        {
            foreach (var enu in Enum.GetValues(typeof(ColorEnum)))
            {
                str = str.Replace("^" + (int)enu, "");
            }
            str = str.Replace("^" + ";", "");
            str = str.Replace("^" + ":", "");

            return str;
        }
    }


}
