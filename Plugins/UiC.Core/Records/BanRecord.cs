using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Core.Records
{
    public class BanRecord
    {
        public BanRecord()
        {

        }

        public int Id
        {
            get;
            set;
        }

        public PlayerRecord Player
        {
            get;
            set;
        }


        public string Reason
        {
            get;
            set;
        }

        public string Server
        {
            get;
            set;
        }

        public string Reporter
        {
            get;
            set;
        }

        public DateTime RowAdded
        {
            get;
            set;
        }


    }
}
