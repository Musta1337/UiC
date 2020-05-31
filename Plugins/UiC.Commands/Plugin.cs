using InfinityScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UiC.Commands.Commands;
using UiC.Commands.Commands.Triggers;
using UiC.Loader.Plugins;

namespace UiC.Commands
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

        public override string Name => "Commands Plugin";

        public override string Description => "Commands Plugin for UiC";

        public override string Author => "Alpa";

        public override Version Version => new Version(1, 0);


        public override void OnPlayerConnecting(Entity player)
        {

        }

        public override void Initialize()
        {
            return;

            CommandManager.Instance.RegisterAll(Assembly.GetExecutingAssembly());
        }


        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            return BaseScript.EventEat.EatNone;

            string delimiter = message.Substring(0, 1);
            string command = message.Substring(1);

            if (command.StartsWith("uic "))
            {
                CommandManager.Instance.HandleCommand(new GameTrigger(command.Substring(4), player), delimiter);
                return BaseScript.EventEat.EatGame;
            }
            else if(command == "uic")
            {
                CommandManager.Instance.HandleCommand(new GameTrigger(command.Substring(3) + "discord", player), delimiter);
                return BaseScript.EventEat.EatGame;
            }
            
            return BaseScript.EventEat.EatNone;
        }

        public override void Dispose()
        {

        }
    }
}
