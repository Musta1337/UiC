using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class PlayerDisconnectedMessage : Message
    {
        public const uint Id = 4;

        public override uint MessageId => Id;

        public int entRef;

        public PlayerDisconnectedMessage()
        {

        }

        public PlayerDisconnectedMessage(int entRef)
        {
            this.entRef = entRef;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entRef);
        }

        public override void Deserialize(IDataReader reader)
        {
            entRef = reader.ReadInt();
        }

    }
}
