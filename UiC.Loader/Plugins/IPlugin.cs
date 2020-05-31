using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfinityScript.BaseScript;

namespace UiC.Loader.Plugins
{
    public static class PluginExtensions
    {
        public static string GetDefaultDescription(this IPlugin plugin)
        {
            return string.Format("'{0}' v{1} by {2}", plugin.Name, plugin.GetType().Assembly.GetName().Version, plugin.Author);
        }
    }

    public interface IPlugin
    {
        PluginContext Context
        {
            get;
        }

        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        string Author
        {
            get;
        }

        Version Version
        {
            get;
        }

        void Initialize();
        void OnPlayerConnecting(Entity player);
        void OnPlayerConnected(Entity player);
        void OnPlayerDisconnected(Entity player);
        void OnPlayerNotified(string str, Parameter[] parameters);

        EventEat OnSay3(Entity player, ChatType type, string name, ref string message);
        void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc);
        void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc);
        void Shutdown();

        void Dispose();
    }
}
