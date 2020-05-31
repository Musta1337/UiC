using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace UiC.Core.Extensions
{
    public static class ColorExtensions
    {
        public static int ColorToUInt(this Color color)
        {
            return (int)((color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }
    }
}
