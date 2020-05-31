using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Threading;
using UiC.Network;
using UiC.Network.Managers;
using UiC.Network.Protocol;

namespace UiC.Network.Handlers
{
    public class ClientPacketHandler : HandlerManager<ClientPacketHandler, HandlerAttribute, ClientHandlerContainer, NetworkClient>
    {
        private SelfRunningTaskPool m_taskPool;

        public ClientPacketHandler()
        {
            m_taskPool = new SelfRunningTaskPool(100, "Client task pool");
            m_taskPool.Start();
        }

        public override void Dispatch(NetworkClient client, Protocol.Message message)
        {
            List<MessageHandler> handlers;
            if (m_handlers.TryGetValue(message.MessageId, out handlers))
            {
                try
                {
                    foreach (var handler in handlers)
                    {
                        m_taskPool.AddMessage(new SelfRunningMessage(new HandledMessage<NetworkClient>(handler.Action, client, message)));
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(LogLevel.Info, string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", message, client, ex));
                    client.Disconnect();
                }
            }
        }
    }
}
