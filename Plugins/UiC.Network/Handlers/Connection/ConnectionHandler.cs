using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core;
using UiC.Core.Extensions;
using UiC.Network.Protocol.Messages;

namespace UiC.Network.Handlers.Connection
{
    public class ConnectionHandler : ClientHandlerContainer
    {
        [Handler(HelloConnectMessage.Id)]
        public static void HandleHelloConnectMessage(NetworkClient client, HelloConnectMessage message)
        {
            Log.Write(LogLevel.Info, "Connected.");
            client.Send(new IdentificationMessage(Server.Hostname.RemoveColors()));

            client.Ready();
        }
    }
}
