using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Discord.Manager;
using UiC.Network.Protocol.Types;
using UiC.NetworkServer.Network;
using UiC.NetworkServer.Records;
using UiC.NetworkServer.WebAPI.Results;

namespace UiC.Discord.Modules
{
    public class ServerModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public Task Help()
        {
            string reply = "`!servers // Display every server online`\n";
            reply += "`!server <serverId> // Display every player on the server`\n";
            reply += "`!player <method> <value> // Search a player, method available: id, name, hwid, ip`\n";
            reply += "`!announce <text> // Send text to every servers online`\n";
            reply += "`!announceto <serverId> <text> // Send text to the server`\n";
            reply += "`!kick <playerId> <reason> // Kick a player with his uID`\n";
            reply += "`!kickto <serverId> <entRef> <reason> // Kick a player from a server with his entRef`\n";
            reply += "`!ban <playerId> <reason> // Ban a player from every server with his uId`\n";
            reply += "`!banto <serverId> <playerId> <reason> // Ban a player from a server (global ban) with his entRef`\n";
            reply += "`!findplayer <serverId> <playerId> // Find a player with his entRef`\n";

            return ReplyAsync(reply);
        }

        [Command("status")]
        public Task Status()
        {
            var result = WebRequestManager.Instance.Get("Servers/online");
            var servers = JsonConvert.DeserializeObject<Dictionary<int, TeknoServer>>(result);
            var players = servers.Select(x => x.Value.Players);

            string reply = string.Empty;

            var top3 = servers.Values.OrderByDescending(x => x.Players.Count).Take(3);

            int i = 1;
            foreach (var server in top3)
            {
                reply += ($"`#{i++} {server.Hostname} | Players: {server.Players.Count}/18`.\n");
            }

            var footer = new EmbedFooterBuilder();
            footer.Text = $"Informations provided by UiC-System.";

            var builder = new EmbedBuilder();

            builder.WithTitle("TOP 3 SERVER");
            builder.Description = reply;
            builder.Footer = footer;
            builder.AddInlineField("Servers Online", servers.Count);
            builder.AddInlineField("Players Online", players.Sum(x => x.Count()));

            builder.WithColor(Color.Red);
            Context.Channel.SendMessageAsync("", false, builder).Wait();
            Context.Client.SetGameAsync($"{servers.Count} servers online. {players.Sum(x => x.Count())} Players");

            return Task.CompletedTask;
        }


        [Command("servers")]
        public Task Servers()
        {
            var result = WebRequestManager.Instance.Get("Servers/online");
            var servers = JsonConvert.DeserializeObject<Dictionary<int,TeknoServer>>(result);
            var players = servers.Select(x => x.Value.Players);

            string reply = string.Empty;

            foreach(var server in servers.Values.OrderBy(x => x.Record.Id))
            {
                reply += ($"`#{server.Record.Id} {server.Hostname} | IP: {server.IP} | Players: {server.Players.Count}/18`.\n");
            }

            var footer = new EmbedFooterBuilder();
            footer.Text = $"{servers.Count} servers online. A total of {players.Sum(x => x.Count())} players online.";

            var builder = new EmbedBuilder();

            builder.WithTitle("Servers Online");
            builder.Description = reply;
            builder.Footer = footer;

            builder.WithColor(Color.Red);
            Context.Channel.SendMessageAsync("", false, builder).Wait();
            Context.Client.SetGameAsync($"{servers.Count} servers online. {players.Sum(x => x.Count())} Players");

            return Task.CompletedTask;
            
        }

        [Command("server")]
        public Task Server([Remainder]string serverId)
        {
            var result = WebRequestManager.Instance.Get("Server/" + serverId);
            var server = JsonConvert.DeserializeObject<TeknoServer>(result);

            string reply = string.Empty;

            foreach (var player in server.Players.Take(12).OrderBy(x => x.EntRef))
            {
                reply += ($"`#{player.EntRef} {player.Name} | Hwid: {player.Hwid} | Guid: {player.Guid} | XnAddr: {player.XnAddr} | IP: {player.IP}.`\n");
            }

            ReplyAsync(reply).Wait();

            reply = string.Empty;

            var restPlayers = server.Players.Skip(12).Take(16);

            foreach (var player in restPlayers.OrderBy(x => x.EntRef))
            {
                reply += ($"`#{player.EntRef} {player.Name} | Hwid: {player.Hwid} | Guid: {player.Guid} | XnAddr: {player.XnAddr} | IP: {player.IP}.`\n");
            }

            return ReplyAsync(reply);
        }

        [Command("player")]
        public Task Player(string method, string value)
        {
            string result = string.Empty;

            switch (method.ToLower())
            {
                case "hwid":
                    result = WebRequestManager.Instance.Get($"Player/Hwid/" + value);
                    break;
                case "ip":
                case "name":
                    result = WebRequestManager.Instance.Get($"Player/Name/" + value);
                    var ps = JsonConvert.DeserializeObject<List<PlayerRecord>>(result);
                    foreach (var p in ps)
                    {
                        ReplyAsync($"#{p.Id} {p.Name} | {p.XnAddr} | {p.Guid} | {p.Hwid} | {p.NewHwid} [ {p.IP} | Alias: {string.Join(",", p.Aliases)}").Wait();
                    }
                    return Task.CompletedTask;
                case "alias":
                    result = WebRequestManager.Instance.Get($"Player/Alias/" + value);
                    var pas = JsonConvert.DeserializeObject<List<PlayerRecord>>(result);
                    foreach (var p in pas)
                    {
                        ReplyAsync($"#{p.Id} {p.Name} | {p.XnAddr} | {p.Guid} | {p.Hwid} | {p.NewHwid} [ {p.IP} | Alias: {string.Join(",", p.Aliases)}").Wait();
                    }
                    return Task.CompletedTask;
            }

            var resultObj = JsonConvert.DeserializeObject<PlayerRecord>(result);
            return ReplyAsync($"#{resultObj.Id} {resultObj.Name} | {resultObj.Hwid} | {resultObj.Guid} | {resultObj.XnAddr} | {resultObj.IP} | Alias: {resultObj.AliasesCSV}");
        }

        [Command("announce")]
        public Task Announce([Remainder]string text)
        {
            var result = WebRequestManager.Instance.Put($"Announce/[Ui^3C]: " + text);
            var resultObj = JsonConvert.DeserializeObject<Result>(result);

            if (resultObj.Success)
            {
                return ReplyAsync("Text send !");
            }
            else
            {
                return ReplyAsync("Error.");
            }
        }

        [Command("announceto")]
        public Task AnnounceToServer(int serverId, [Remainder]string text)
        {
            var result = WebRequestManager.Instance.Put($"Announce/{serverId}/[Ui^3C]: {text}");
            var resultObj = JsonConvert.DeserializeObject<Result>(result);

            if (resultObj.Success)
            {
                return ReplyAsync("Text send !");
            }
            else
            {
                return ReplyAsync("Error.");
            }
        }

        [Command("kick")]
        public Task KickToServer(int playerId, [Remainder]string reason)
        {
            var result = WebRequestManager.Instance.Post($"Servers/Kick/{playerId}/{reason}");
            var resultObj = JsonConvert.DeserializeObject<Player>(result);

            return ReplyAsync($"Player {resultObj.Name} kicked !");
        }

        [Command("kickto")]
        public Task KickToServer(int serverId, int playerId, [Remainder]string reason)
        {
            var result = WebRequestManager.Instance.Post($"Server/{serverId}/Kick/{playerId}/{reason}");
            var resultObj = JsonConvert.DeserializeObject<Player>(result);

            return ReplyAsync($"Player {resultObj.Name} kicked !");
        }

        [Command("ban")]
        public Task Ban(string playerId, [Remainder]string reason)
        {
            var result = WebRequestManager.Instance.Put($"Ban/{playerId}/{reason}");
            var resultObj = JsonConvert.DeserializeObject<PlayerRecord>(result);

            return ReplyAsync($"Player {resultObj.Name} banned !");
        }

        [Command("banto")]
        public Task BanTo(int serverId, int playerId, [Remainder]string reason)
        {
            var result = WebRequestManager.Instance.Put($"Server/{serverId}/Ban/{playerId}/{reason}");
            var resultObj = JsonConvert.DeserializeObject<Player>(result);

            return ReplyAsync($"Player {resultObj.Name} banned !");
        }

        [Command("findplayer")]
        public Task FindPlayer(int serverId, int playerId)
        {
            var result = WebRequestManager.Instance.Get($"Server/{serverId}/Player/{playerId}");
            var resultObj = JsonConvert.DeserializeObject<PlayerRecord>(result);

            var footer = new EmbedFooterBuilder();
            footer.Text = $"Found by UiC-System.";

            var builder = new EmbedBuilder();

            builder.WithTitle("Player Informations");
            builder.Description = $"{resultObj.Id} - {resultObj.Name} - {resultObj.AliasesCSV} - {resultObj.XnAddr} - {resultObj.Guid} - {resultObj.Hwid} - {resultObj.IP} - {resultObj.NewHwid}";
            builder.Footer = footer;
            builder.AddInlineField("Id", resultObj.Id);
            builder.AddInlineField("Name", resultObj.Name);
            builder.AddInlineField("Alias", resultObj.AliasesCSV);
            builder.AddInlineField("XnAddr", resultObj.XnAddr);
            builder.AddInlineField("Guid", resultObj.Guid);
            builder.AddInlineField("Hwid", resultObj.Hwid);
            builder.AddInlineField("IP", resultObj.IP);
            builder.AddInlineField("NewHwid", resultObj.NewHwid);
            builder.WithColor(Color.DarkOrange);

            Context.Channel.SendMessageAsync("", false, builder).Wait();

            return ReplyAsync("Command executed.");
        }
    }
}
