using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Extensions;

namespace UiC.Core.Misc
{
    public class NewHWID
    {
        public static int HWIDOffset = 0x4A30335;
        public static int HWIDDataSize = 0x78688;

        public string Value
        {
            get; private set;
        }

        public NewHWID(Entity player)
            :this(player.EntRef)
        {

        }

        public NewHWID(int entRef)
        {
            int address = HWIDDataSize * entRef + HWIDOffset;
            string formattedhwid = "";
            unsafe
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i % 4 == 0 && i != 0)
                        formattedhwid += "-";
                    formattedhwid += (*(byte*)(address + i)).ToString("x2");
                }
            }
            Value = formattedhwid;
        }

        private NewHWID(string value)
        {
            Value = value;
        }

        public bool IsBadHWID()
        {
            return string.IsNullOrWhiteSpace(Value) || Value == "00000000-00000000-00000000";
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string str, out NewHWID parsedhwid)
        {
            str = str.ToLowerInvariant();
            if (str.Length != 26)
            {
                parsedhwid = new NewHWID((string)null);
                return false;
            }
            for (int i = 0; i < 26; i++)
            {
                if (i == 8 || i == 17)
                {
                    if (str[i] != '-')
                    {
                        parsedhwid = new NewHWID((string)null);
                        return false;
                    }
                    continue;
                }
                if (!str[i].CharIsHex())
                {
                    parsedhwid = new NewHWID((string)null);
                    return false;
                }
            }
            parsedhwid = new NewHWID(str);
            return true;
        }
    }
}
