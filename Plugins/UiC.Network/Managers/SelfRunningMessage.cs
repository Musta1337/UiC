using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Threading;

namespace UiC.Network
{
    public class SelfRunningMessage : IMessage
    {
        private readonly IMessage m_message;

        public SelfRunningMessage(IMessage message)
        {
            m_message = message;
        }


        public void Execute()
        {
            m_message.Execute();
        }

    }
}
