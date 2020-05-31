using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class SayToPlayerMessage : Message
    {
        public const uint Id = 8;

        public override uint MessageId => Id;

        public int entRef;
        public string message;

        public SayToPlayerMessage()
        {

        }

        public SayToPlayerMessage(int entRef, string message)
        {
            this.entRef = entRef;
            this.message = message;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entRef);
            writer.WriteUTF(message);
        }

        public override void Deserialize(IDataReader reader)
        {
            entRef = reader.ReadInt();
            message = reader.ReadUTF();
        }

    }
}
