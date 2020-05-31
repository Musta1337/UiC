using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UiC.Core.Threading;
using UiC.Network.Protocol;
using UiC.NetworkServer.Logging;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer
{
    public class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server();
            server.Initialize();

            GC.Collect();

            while (true)
            {
                Thread.Sleep(5000);
            }
        }


    }
}
