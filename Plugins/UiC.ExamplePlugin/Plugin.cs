using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Loader.Plugins;

namespace ExemplePlugin
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

        public override string Name => "Example Plugin";

        public override string Description => "Example Plugin for UiC";

        public override string Author => "Alpa";

        public override Version Version => new Version(1, 0);

        public ExamplePlugin ExamplePlugin
        {
            get;
            private set;
        }

        public override void OnPlayerConnecting(Entity player)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} Connecting.");
        }

        public override void OnPlayerConnected(Entity player)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} Connected.");
        }

        public override void OnPlayerDisconnected(Entity player)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} disconnected.");
        }

        public override void OnPlayerNotified(string str, Parameter[] parameters)
        {
            Log.Write(LogLevel.Info, $"Notified: " + str);
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} damage taken {damage} by {attacker.Name} with <{weapon}> at <{hitLoc}> ");
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} killed {damage} by {attacker.Name} with <{weapon}> at <{hitLoc}> ");
        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            Log.Write(LogLevel.Info, $"Player {player.Name} say {message}");

            return base.OnSay3(player, type, name, ref message);
        }
        
        public override void Initialize()
        {
            base.Initialize();

            ExamplePlugin = new ExamplePlugin();
            ExamplePlugin.Initialize();
        }


        public override void Dispose()
        {

        }
    }
}
