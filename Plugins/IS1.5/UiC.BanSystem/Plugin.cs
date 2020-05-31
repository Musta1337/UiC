using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InfinityScript;
using UiC.Loader.Plugins;

namespace BanSystem
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

        public override string Name => "BanSystem Plugin";

        public override string Description => "BanSystem Plugin for UiC";

        public override string Author => "Alpa";

        public override Version Version => new Version(1, 1);

        public IS1_5.BanSystem BanSystem
        {
            get;
            private set;
        }

        public override void OnPlayerConnecting(Entity player)
        {
            //BanSystem.OnPlayerConnecting(player);
        }

        public override void OnPlayerConnected(Entity player)
        {

        }

        public override void Initialize()
        {

        }


        public override void Dispose()
        {

        }
    }
}
