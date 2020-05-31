using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;
using UiC.Network.Protocol.Types;

namespace UiC.Network.Protocol.Messages
{
    public class PlayerConnectingMessage : Message
    {
        public const uint Id = 5;

        public override uint MessageId => Id;

        public Player Player;

        public PlayerConnectingMessage()
        {

        }

        public PlayerConnectingMessage(Player player)
        {
            this.Player = player;
        }

        public override void Serialize(IDataWriter writer)
        {
            Player.Serialize(writer);
        }

        public override void Deserialize(IDataReader reader)
        {
            Player = new Player();
            Player.Deserialize(reader);
        }

    }
}
