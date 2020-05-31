using InfinityScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UiC.Core;
using UiC.Core.Discord;
using UiC.Core.Extensions;
using UiC.Core.Misc;
using UiC.Loader.Plugins;
using UiC.Network.Handlers;
using UiC.Network.Protocol;
using UiC.Network.Protocol.Messages;
using UiC.Network.Protocol.Types;

namespace UiC.Network
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
            CurrentPlugin = this;
        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }

        public override string Name => "Network Plugin";

        public override string Description => "Network Plugin for UiC";

        public override string Author => "Alpa";

        public override Version Version => new Version(1, 0);

        public NetworkClient NetworkClient
        {
            get;
            private set;
        }

        public override void OnPlayerConnecting(Entity player)
        {
            try
            {
                //   Log.Write(LogLevel.Info, $"Player {player.Name} connecting");

                //   XnAddr xnAddr = new XnAddr(player, player.EntRef);
                //   NetworkClient.SendWhenReady(new PlayerConnectingMessage(new Player(player.Name, player.EntRef, xnAddr.Value, player.HWID, player.GUID, player.IP.Address.ToString())));
            } catch (Exception e) {

            }
        }

        public override void OnPlayerConnected(Entity player)
        {
            try
            {
                Log.Write(LogLevel.Info, $"Player {player.Name} connected");

                XnAddr xnAddr = new XnAddr(player, player.EntRef);
                NetworkClient.SendWhenReady(new PlayerConnectedMessage(new Player(player.Name, player.EntRef, xnAddr.Value, player.HWID, new NewHWID(player).Value, player.GUID, player.IP.Address.ToString())));
            }
            catch { }
        }

        public override void OnPlayerDisconnected(Entity player)
        {
            try
            {
                Log.Write(LogLevel.Info, $"Player {player.Name} disconnected");
                NetworkClient.SendWhenReady(new PlayerDisconnectedMessage(player.EntRef));
            }
            catch { }
        }

        public override void OnPlayerNotified(string str, Parameter[] parameters)
        {
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {

        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {

        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            try
            {
                //Log.Write(LogLevel.Info, $"Player {player.Name} disconnected");
                if (message.StartsWith("/"))
                {
                    NetworkClient.SendWhenReady(new PlayerCommandMessage(player.EntRef, message));
                    return BaseScript.EventEat.EatGame;
                }
                else
                {
                    WebhookManager.SendMessageLog(player.Name, message, Server.Hostname.RemoveColors(), "New message", "By UiC-Spy", Color.FloralWhite);
                }

                return BaseScript.EventEat.EatGame;
            }
            catch
            {
                return BaseScript.EventEat.EatScript;
            }
        }
        
        public override void Initialize()
        {
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            Handler = new ClientPacketHandler();
            Handler.RegisterAll(Context.PluginAssembly);

            NetworkClient = new NetworkClient();
            NetworkClient.Initialize();
        }

        public ClientPacketHandler Handler
        {
            get;
            private set;
        }

        public override void Dispose()
        {

        }
    }
}
