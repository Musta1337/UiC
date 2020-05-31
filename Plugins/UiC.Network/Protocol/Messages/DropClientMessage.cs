using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class DropClientMessage : Message
    {
        public const uint Id = 6;

        public override uint MessageId => Id;

        public int entRef;
        public string reason;

        public DropClientMessage()
        {

        }

        public DropClientMessage(int entRef, string reason)
        {
            this.entRef = entRef;
            this.reason = reason;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entRef);
            writer.WriteUTF(reason);
        }

        public override void Deserialize(IDataReader reader)
        {
            entRef = reader.ReadInt();
            reason = reader.ReadUTF();
        }

    }
}