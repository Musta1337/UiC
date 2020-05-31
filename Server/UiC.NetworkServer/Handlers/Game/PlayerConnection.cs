using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Discord;
using UiC.Network.Handlers;
using UiC.Network.Protocol.Messages;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer.Handlers.Game
{
    public class PlayerConnectionHandler : ClientHandlerContainer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        [Handler(PlayerConnectingMessage.Id)]
        public static void HandlePlayerConnectingMessage(BaseClient client, PlayerConnectingMessage message)
        {

        }

        [Handler(PlayerConnectedMessage.Id)]
        public static void HandlePlayerConnectedMessage(BaseClient client, PlayerConnectedMessage message)
        {
            logger.Info("Player connected: " + message.Player.Name);

            if (client.TeknoServer == null)
                return;

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                client.TeknoServer.AddPlayer(message.Player);

                var record = BanManager.Instance.Bans.FirstOrDefault(x => x.Player != null && (x.Player.IP == message.Player.IP || x.Player.NewHwid == message.Player.NewHwid));

                if (record != null)
                {
                    WebhookManager.SendWebhook(message.Player.Name, record.Reason, client.TeknoServer.Hostname, "**Player rejected**", "UiC-BanSystem", Color.Red);
                    client.Send(new DropClientMessage(message.Player.EntRef, "[UiC] Banned, Reason: " + record.Reason + " Discord: uic.elitesnipers.pw/discord"));
                    client.Send(new SayToEveryoneMessage($"[UiC] Player ^3{message.Player.Name} ^7Banned ^1permanently^7 ! Reason: ^3{record.Reason}"));

                // ServerManager.Instance.KickPlayer(message.Player.NewHwid, record.Reason);

                logger.Info("Player banned: " + message.Player.Name + " " + message.Player.NewHwid);
                }
            });
        }
    

        [Handler(PlayerCommandMessage.Id)]
        public static void HandlePlayerCommandMessage(BaseClient client, PlayerCommandMessage message)
        {
            if (client.TeknoServer == null)
                return;

            var player = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef == message.entRef);

            if(player != null)
            {
                if(player.NewHwid == "d20a123c1-141234523b-031324d" || player.NewHwid == "514bcef1-32320-dd63230" || player.NewHwid == "a65d323-4c4332ec6d")
                {
                    CommandManager.Instance.HandleCommand(client.TeknoServer, player, message.message.Substring('1'));
                }
                else
                {
                    logger.Info($"Command {message.message} trying to be executed by {player.Name}");
                }
            }
        }

        [Handler(PlayerDisconnectedMessage.Id)]
        public static void HandlePlayerDisconnectedMessage(BaseClient client, PlayerDisconnectedMessage message)
        {
            if (client.TeknoServer == null)
                return;

            var player = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef == message.entRef);
            if (player != null)
            {
                logger.Info("Player disconnected: " + player.Name);
                client.TeknoServer.RemovePlayer(message.entRef);
            }
        }

    }
}
