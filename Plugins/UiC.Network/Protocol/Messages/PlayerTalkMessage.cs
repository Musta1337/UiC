using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class PlayerTalkMessage : Message
    {
        public const uint Id = 10;

        public override uint MessageId => Id;

        public int entRef;
        public string message;

        public PlayerTalkMessage()
        {

        }

        public PlayerTalkMessage(int entRef, string message)
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
