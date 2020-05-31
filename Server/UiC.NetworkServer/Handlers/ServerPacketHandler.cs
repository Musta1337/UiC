using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Threading;
using UiC.Network;
using UiC.Network.Handlers;
using UiC.Network.Managers;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer.Handlers
{
    public class ServerPacketHandler : HandlerManager<ServerPacketHandler, HandlerAttribute, ClientHandlerContainer, BaseClient>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private SelfRunningTaskPool m_taskPool;
        private SelfRunningTaskPool m_taskPoolSecondary;

        public ServerPacketHandler()
        {
            m_taskPool = new SelfRunningTaskPool(100, "Client task pool");
            m_taskPool.Start();

            m_taskPoolSecondary = new SelfRunningTaskPool(100, "Secondary Client task pool");
            m_taskPoolSecondary.Start();
        }

        public override void Dispatch(BaseClient client, UiC.Network.Protocol.Message message)
        {
            List<MessageHandler> handlers;
            if (m_handlers.TryGetValue(message.MessageId, out handlers))
            {
                try
                {
                    foreach (var handler in handlers)
                    {
                        //logger.Info($"TaskPool: {m_taskPool.MessageQueue.Count} / { m_taskPoolSecondary.MessageQueue.Count}");

                        if (m_taskPool.MessageQueue.Count < m_taskPoolSecondary.MessageQueue.Count)
                        {
                            m_taskPool.AddMessage(new SelfRunningMessage(new HandledMessage<BaseClient>(handler.Action, client, message)));
                        }
                        else
                        {
                            m_taskPoolSecondary.AddMessage(new SelfRunningMessage(new HandledMessage<BaseClient>(handler.Action, client, message)));
                        }

                    }
                }
                catch (Exception ex)
                {
                    logger.Info(string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", message, client, ex));
                    client.Disconnect();
                }
            }
        }
    }
}
