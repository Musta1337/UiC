using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Network.Protocol;

namespace UiC.Network
{
    public interface IClient
    {
        void Send(Message message);
        void Disconnect();
    }
}
