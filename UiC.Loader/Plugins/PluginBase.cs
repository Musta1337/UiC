using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfinityScript.BaseScript;

namespace UiC.Loader.Plugins {
    public abstract class PluginBase : IPlugin {
        protected PluginBase(PluginContext context) {
            Context = context;
        }

        public PluginContext Context {
            get;
            protected set;
        }
        #region IPlugin Members

        public abstract string Name {
            get;
        }

        public abstract string Description {
            get;
        }

        public abstract string Author {
            get;
        }

        public abstract Version Version {
            get;
        }

        public virtual void Initialize() {

        }

        public virtual void OnPlayerConnected(Entity player)
        {

        }

        public virtual void OnPlayerConnecting(Entity player)
        {

        }

        public virtual void OnPlayerDisconnected(Entity player)
        {

        }

        public virtual EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            return EventEat.EatNone;
        }

        public virtual void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {

        }

        public virtual void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {

        }


        public virtual void Shutdown() {

        }

        public abstract void Dispose();

        #endregion

        public string GetPluginDirectory() {
            return Path.GetDirectoryName(Context.AssemblyPath);
        }

        public virtual void OnPlayerNotified(string str, Parameter[] parameters)
        {

        }
    }
}
