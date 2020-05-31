using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class HelloConnectMessage : Message
    {
        public const uint Id = 1;

        public override uint MessageId => Id;

        public HelloConnectMessage()
        {

        }

        public override void Serialize(IDataWriter writer)
        {
            
        }

        public override void Deserialize(IDataReader reader)
        {
        }

    }
}
