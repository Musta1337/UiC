using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Network.Protocol.Messages;

namespace UiC.Network.Handlers.Game
{
    public class PlayerHandler : ClientHandlerContainer
    {
        [Handler(DropClientMessage.Id)]
        public static void HandleDropClientMessage(NetworkClient client, DropClientMessage message)
        {
            Utilities.ExecuteCommand($"dropclient {message.entRef} \"{message.reason}\"");
        }

        [Handler(SayToEveryoneMessage.Id)]
        public static void HandleSayToEveryoneMessage(NetworkClient client, SayToEveryoneMessage message)
        {
            Utilities.RawSayAll(message.message);
        }

        [Handler(SayToPlayerMessage.Id)]
        public static void HandleSayToPlayerMessage(NetworkClient client, SayToPlayerMessage message)
        {
            Utilities.RawSayTo(message.entRef, message.message);
        }
    }
}
