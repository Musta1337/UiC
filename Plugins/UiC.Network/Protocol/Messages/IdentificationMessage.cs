using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;

namespace UiC.Network.Protocol.Messages
{
    public class IdentificationMessage : Message
    {
        public const uint Id = 2;

        public override uint MessageId => Id;

        public string hostname;

        public IdentificationMessage()
        {

        }

        public IdentificationMessage(string hostname)
        {
            this.hostname = hostname;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(hostname);
        }

        public override void Deserialize(IDataReader reader)
        {
            hostname = reader.ReadUTF();
        }

    }
}
