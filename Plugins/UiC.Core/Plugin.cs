using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Managers;
using UiC.Loader.Plugins;

namespace UiC.Core
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

        public override string Name => "Core Plugin";

        public override string Description => "Core Plugin for UiC";

        public override string Author => "Alpa";

        public override Version Version => new Version(1, 0);

        public override void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Write(LogLevel.Info, e.ExceptionObject.ToString());
        }

        public override void OnPlayerConnected(Entity player)
        {
            PlayerManager.Instance.Players.Add(player);
        }

        public override void OnPlayerDisconnected(Entity player)
        {
            var entity = PlayerManager.Instance.Players.FirstOrDefault(x => x.EntRef == player.EntRef);

            if (entity != null)
                PlayerManager.Instance.Players.Remove(entity);
        }

        public override void Dispose()
        {

        }
    }
}
