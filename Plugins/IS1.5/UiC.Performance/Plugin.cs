using InfinityScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UiC.Core.Misc;
using UiC.Loader.Plugins;

namespace UiC.Performance
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

        public override string Name => "Performance Plugin";

        public override string Description => "Performance Plugin for UiC";

        public override string Author => "Musta";

        public override Version Version => new Version(1, 0);

        public PerformancePlugin PerformancePlugin
        {
            get;
            private set;
        }

        public override void OnPlayerConnected(Entity player)
        {
            //PerformancePlugin.SetDvars(player);

            NewHWID addr = new NewHWID(player);

            if (addr.Value == "d20a94c1-1dd7f4db-0c41d13d" || addr.Value == "514bcef1-3443efe0-dd60b830")
            {
                PerformancePlugin.SetIcon(player);

                player.SpawnedPlayer += () =>
                {
                    PerformancePlugin.SetIcon(player);
                };
            }

            player.OnNotify("joined_team", (entity) =>
            {
                PerformancePlugin.SetSpawnDvar(player);
            });
            return;

        }
        
        public override void Initialize()
        {

            GSCFunctions.PreCacheStatusIcon("cardicon_iwlogo");

            PerformancePlugin = new PerformancePlugin();

            PerformancePlugin.SetServerDvars();
            return;
        }


        public override void Dispose()
        {

        }
    }
}
