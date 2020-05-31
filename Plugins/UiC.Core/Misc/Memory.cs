using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Core.Misc
{
    public static class Memory
    {
        public static unsafe string ReadString(int address, int maxlen = 0)
        {
            string ret = "";
            maxlen = (maxlen == 0) ? int.MaxValue : maxlen;
            for (; address < address + maxlen && *(byte*)address != 0; address++)
            {
                ret += Encoding.ASCII.GetString(new byte[] { *(byte*)address });
            }
            return ret;
        }

        public static unsafe void WriteString(int address, string str)
        {
            byte[] strarr = Encoding.ASCII.GetBytes(str);
            foreach (byte ch in strarr)
            {
                *(byte*)address = ch;
                address++;
            }
            *(byte*)address = 0;
        }
    }
}
