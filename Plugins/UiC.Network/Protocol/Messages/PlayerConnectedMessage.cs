using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.IO;
using UiC.Network.Protocol.Types;

namespace UiC.Network.Protocol.Messages
{
    public class PlayerConnectedMessage : Message
    {
        public const uint Id = 3;

        public override uint MessageId => Id;

        public Player Player;

        public PlayerConnectedMessage()
        {

        }

        public PlayerConnectedMessage(Player player)
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