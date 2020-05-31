using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Core.Records
{
    public class PlayerRecord
    {
        public string Name
        {
            get;
            set;
        }

        public List<string> Aliases
        {
            get;
            set;
        } = new List<string>();

        public string IP
        {
            get;
            set;
        }

        public string XnAddr
        {
            get;
            set;
        }

        public string Hwid
        {
            get;
            set;
        }

        public string NewHwid
        {
            get;
            set;
        }

        public string Guid
        {
            get;
            set;
        }


        public DateTime RowAdded
        {
            get;
            set;
        }

        public DateTime RowUpdated
        {
            get;
            set;
        }
    }
}
