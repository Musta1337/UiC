using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Network.Handlers;
using UiC.Network.Protocol.Messages;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer.Handlers.Connection
{
    public class ConnectionHandler : ClientHandlerContainer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Handler(IdentificationMessage.Id)]
        public static void HandleIdentificationMessage(BaseClient client, IdentificationMessage message)
        {
            logger.Info("Server connected: " + message.hostname);

            if (client.TeknoServer == null)
            {
                Server.Instance.IOTaskPool.AddMessage(() =>
                {
                    client.TeknoServer = new TeknoServer(message.hostname, client.IP);
                    client.TeknoServer.LoadRecord(message.hostname, client.IP);
                });
            }
        }
    }
}

