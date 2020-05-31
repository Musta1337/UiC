using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Threading;
using Message = UiC.Network.Protocol.Message;

namespace UiC.Network.Handlers
{
    public class HandledMessage<T> : Message3<object, T, Message>
        where T : IClient
    {

        public HandledMessage(Action<object, T, Message> callback, T client, Message message)
            : base(null, client, message, callback)
        {

        }

        public override void Execute()
        {
            try
            {
                base.Execute();
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.Info, "[Handler : {0}] Force disconnection of client {1} : {2}", Parameter3, Parameter2, ex);
                Parameter2.Disconnect();
            }
        }

        public override string ToString()
        {
            return Parameter3.ToString();
        }
    }
}
