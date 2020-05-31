using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UiC.NetworkServer.Pool;

namespace UiC.NetworkServer.Network
{
    public class PoolableSocketArgs : SocketAsyncEventArgs, IPooledObject
    {
        public void Cleanup()
        {
            UserToken = null;
        }
    }
}
