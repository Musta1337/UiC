using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Discord;
using UiC.Core.Reflection;
using UiC.Network.Protocol.Messages;
using UiC.Network.Protocol.Types;
using UiC.NetworkServer.Network;
using UiC.NetworkServer.Records;
using UiC.ORM;

namespace UiC.NetworkServer.Managers
{
    public class ServerManager : Singleton<ServerManager>
    {
        public Database Database
        {
            get => Server.Instance.DBAccessor.Database;
        }

        public ServerManager()
        {

        }

        public void SendMessageToAll(string message)
        {
            foreach(var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                client.Send(new SayToEveryoneMessage( message));
            }
        }

        public void SendMessageToServer(int serverId, string message)
        {
            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                if(client.TeknoServer.Record.Id == serverId)
                    client.Send(new SayToEveryoneMessage(message));
            }
        }

        public void KickPlayer(int serverId, string hwid, string reason)
        {
            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                if (client.TeknoServer.Record.Id != serverId)
                    continue;

                var kickPlayer = client.TeknoServer.Players.FirstOrDefault(x => x.NewHwid == hwid);

                if (kickPlayer != null)
                {
                    client.Send(new DropClientMessage(kickPlayer.EntRef, reason));
                    client.Send(new SayToEveryoneMessage($"[UiC] Player ^3{kickPlayer.Name} ^7kicked ! Reason: ^3{reason}"));
                }
            }
        }

        public Player KickPlayer(int serverId, int entRef, string reason)
        {
            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                if (client.TeknoServer.Record.Id != serverId)
                    continue;

                var kickPlayer = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef == entRef);

                if (kickPlayer != null)
                {
                    client.Send(new DropClientMessage(kickPlayer.EntRef, reason));
                    client.Send(new SayToEveryoneMessage($"[UiC] Player ^3{kickPlayer.Name} ^7kicked ! Reason: ^3{reason}"));

                    return kickPlayer;
                }
            }

            return null;
        }

        public PlayerRecord FindPlayer(int serverId, int entRef)
        {
            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                if (client.TeknoServer.Record.Id != serverId)
                    continue;

                var player = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef == entRef);

                if(player != null)
                {
                    return PlayerManager.Instance.FindPlayerRecordByNewHwid(player.NewHwid);
                }
            }

            return null;
        }

        public bool KickPlayer(string hwid, string reason, out string hostname)
        {
            hostname = string.Empty;

            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                var kickPlayer = client.TeknoServer.Players.FirstOrDefault(x => x.NewHwid == hwid);

                if (kickPlayer != null)
                {
                    hostname = client.TeknoServer.Hostname;

                    client.Send(new DropClientMessage(kickPlayer.EntRef, reason));
                    client.Send(new SayToEveryoneMessage($"[UiC] Player ^3{kickPlayer.Name} ^7kicked ! Reason: ^3{reason}"));

                    return true;
                }
            }

            return false;
        }

        public PlayerRecord AddBan(int playerId, string reason, string reporter, bool sendMessage = true)
        {
            var playerRecord = new PlayerBannedRecord() { PlayerId = playerId, Reason = reason, Reporter = reporter, RowAdded = DateTime.Now, Server = "All" };
            Database.Insert(playerRecord);

            if (sendMessage)
            {
                ServerManager.Instance.SendMessageToAll($"[UiC] Player ^3{playerRecord.Player.Name} ^7banned ^1definitively by {reporter} ! Reason: ^3{reason}");

                if (ServerManager.Instance.KickPlayer(playerRecord.Player.NewHwid, reason, out string hostname))
                {
                    WebhookManager.SendWebhook(playerRecord.Player.NewHwid, reason, hostname, "Player banned", "UiC-Ban", System.Drawing.Color.Orange);
                }
                else
                {
                    WebhookManager.SendWebhook(playerRecord.Player.NewHwid, reason, "Was not in a server", "Player banned", "UiC-Ban", System.Drawing.Color.Orange);
                }
            }


            return playerRecord.Player;
        }

        public PlayerRecord AddBan(int serverId, int playerId, string reason, string reporter, bool sendMessage = true)
        {

            var player = FindPlayer(serverId, playerId);
            var playerRecord = new PlayerBannedRecord() { PlayerId = player.Id, Reason = reason, Reporter = reporter, RowAdded = DateTime.Now, Server = "All" };
            Database.Insert(playerRecord);

            if (sendMessage)
            {
                ServerManager.Instance.SendMessageToAll($"[UiC] Player ^3{player.Name} ^7banned ^1definitively by {reporter} ! Reason: ^3{reason}");
                if (ServerManager.Instance.KickPlayer(playerRecord.Player.NewHwid, reason, out string hostname))
                {
                    WebhookManager.SendWebhook(playerRecord.Player.NewHwid, reason, hostname, "Player banned", "UiC-Ban", System.Drawing.Color.Orange);
                }
                else
                {
                    WebhookManager.SendWebhook(playerRecord.Player.NewHwid, reason, "Was not in a server", "Player banned", "UiC-Ban", System.Drawing.Color.Orange);
                }
            }


            return player;
        }

        public void KickPlayer(int entRef, string reason)
        {
            foreach (var client in ClientManager.Instance.Clients)
            {
                if (client.TeknoServer == null)
                    continue;

                var kickPlayer = client.TeknoServer.Players.FirstOrDefault(x => x.EntRef == entRef);

                if (kickPlayer != null)
                {
                    client.Send(new DropClientMessage(kickPlayer.EntRef, reason));
                    client.Send(new SayToEveryoneMessage($"[UiC] Player ^3{kickPlayer.Name} ^7kicked ! Reason: ^3{reason}"));
                }
            }
        }

        public ServerRecord FindServerRecord(string hostname, string ip)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Server.Instance.DBAccessor.Database.Query<ServerRecord>(ServerRelator.FetchQueryByHostnameAndIp, hostname, ip).FirstOrDefault();
        }

        public ServerRecord FindServerRecord(int id)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Server.Instance.DBAccessor.Database.Query<ServerRecord>(ServerRelator.FetchQueryById, id).FirstOrDefault();
        }


        public List<ServerRecord> FindServersRecord()
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Server.Instance.DBAccessor.Database.Query<ServerRecord>(ServerRelator.FetchQuery).ToList();
        }
    }
}
