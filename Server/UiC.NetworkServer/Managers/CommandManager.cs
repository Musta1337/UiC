using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Reflection;
using UiC.Network.Protocol.Messages;
using UiC.Network.Protocol.Types;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer.Managers
{
    public class CommandManager : Singleton<CommandManager>
    {
        public void HandleCommand(TeknoServer server, Player sender, string message)
        {
            string[] args = message.Split(' ');

            var command = args[0];

            switch (command.ToLower())
            {
                case "whois":
                    var client = ClientManager.Instance.Clients.FirstOrDefault(x => x.TeknoServer == server);

                    var playerArg = args[1];



                    break;
            }

        }

        public bool GetPlayer(BaseClient client, Player sender, string playerArg, out Player target)
        {
            target = null;

            if (playerArg.StartsWith("@"))
            {
                var player = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef.ToString() == playerArg.Substring(1));

                if (player == null)
                {
                    client.Send(new SayToPlayerMessage(sender.EntRef, "[UiC] Entref not found"));
                    return false;
                }

                var playerRecord = PlayerManager.Instance.FindPlayerRecordByNewHwid(player.NewHwid);

                //client.Send(new SayToPlayerMessage(sender.EntRef, "[UiC] Player alias: " + playerRecord.AliasesCSV));
                
                return true;
            }

            var players = client.TeknoServer.Players.Where(x => x.Name.Contains(playerArg));

            if (players.Count() > 1)
            {
                client.Send(new SayToPlayerMessage(sender.EntRef, "[UiC] Multiple players have this name, be more specific or use @entRef"));

                foreach (var player in players)
                {
                    client.Send(new SayToPlayerMessage(sender.EntRef, "[UiC] @" + player.EntRef + " " + player.Name));
                }

                return false;
            }
            else
            {

            }

            return false;
        }
    }
}
