using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Extensions;

namespace UiC.Core.Misc
{
    public class XnAddr
    {
        private const int XNADDROffset = 0x049EBD00;
        private const int XNADDRDataSize = 0x78688;

        public string Value
        {
            get; private set;
        }

        public XnAddr(Entity player, int ent)
        {
            if (player == null || !player.IsPlayer)
            {
                Value = null;
                return;
            }
            string connectionstring = Memory.ReadString(XNADDRDataSize * ent + XNADDROffset, XNADDRDataSize);
            string[] parts = connectionstring.Split('\\');
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i - 1] == "xnaddr")
                {
                    Value = parts[i].Substring(0, 12);
                    return;
                }
            }
            Value = null;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string str, out XnAddr parsedxnaddr)
        {
            str = str.ToLowerInvariant();
            if (str.Length != 12)
            {
                parsedxnaddr = null;
                return false;
            }
            for (int i = 0; i < 12; i++)
            {
                if (!str[i].CharIsHex())
                {
                    parsedxnaddr = null;
                    return false;
                }
            }
            parsedxnaddr = null;
            return true;
        }
    }
}
