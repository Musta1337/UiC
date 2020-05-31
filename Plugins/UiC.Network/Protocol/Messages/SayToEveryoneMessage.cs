using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class SayToEveryoneMessage : Message
    {
        public const uint Id = 7;

        public override uint MessageId => Id;

        public string message;

        public SayToEveryoneMessage()
        {

        }

        public SayToEveryoneMessage(string message)
        {
            this.message = message;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(message);
        }

        public override void Deserialize(IDataReader reader)
        {
            message = reader.ReadUTF();
        }

    }
}
