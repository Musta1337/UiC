using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
using UiC.Loader.Plugins;
using System.Drawing;
using UiC.Core.Misc;
using UiC.Core.Discord;
using UiC.Core.Discord.Objects;
using UiC.Core.Extensions;
using UiC.Loader;
using UiC.Core;
using System.Timers;
using UiC.Core.Threading;
using System.Diagnostics;

namespace UIC.AntiCheat
{
    public class UICAntiCheat : PluginBase
    {
        public UICAntiCheat(PluginContext context)
    : base(context)
        {
            CurrentPlugin = this;
        }

        public static UICAntiCheat CurrentPlugin
        {
            get;
            private set;
        }

        public override string Name => "AntiCheat Plugin";

        public override string Description => "AntiCheat for protect the server against hackers";

        public override string Author => "astro & Alpa";

        public override Version Version => new Version(1, 0);
        public override void Initialize()
        {
            Server.Hostname = GSCFunctions.GetDvar("sv_hostname");
            return;

            UiC_Loader.AfterDelay(2000, AntiForceClass.Initialize);
        }

        public override void OnPlayerConnecting(Entity player)
        {
            CheckIntegrity(player);
        }

        public override void OnPlayerConnected(Entity player)
        {
            return;

            player.SpawnedPlayer += () =>
            {
                AntiForceClass.CheckClass(player);
                CheckTeam(player);
            };

            player.OnNotify("weapon_fired", (ent, weapon) =>
            {
                // var norecoil = new AntiNoRecoil(player);
               // norecoil.AutoCheck();

            });


            base.OnPlayerConnected(player);
        }

        public void CheckTeam(Entity entity)
        {
            if (entity.SessionTeam == "spectator")
            {
                WebhookManager.SendWebhookSuspect(entity, "**Player Spawned as Spectator**", Server.Hostname.RemoveColors(), $"**Player Suspected for Spectator GODMODE **", "UiC Anti-Cheat", AntiForceClass.DsrName, Color.Blue);
            }
        }

        internal static void AntiCheatBanClient(Entity obj, string v)
        {
            XnAddr ad = new XnAddr(obj, obj.GetEntityNumber());
            WebhookManager.SendWebhookSuspect(obj, "**Player Suspected**", Server.Hostname.RemoveColors(), $"**Player Suspected for {v} **", "UiC Anti-Cheat", AntiForceClass.DsrName, Color.Blue);
        }

        public void CheckIntegrity(Entity entity)
        {
            if (entity.GUID.ToString().Length != 17 && entity.GUID.ToString().Length != 17)
            {
                UiC_Loader.AfterDelay(2000, () =>
                    Utilities.ExecuteCommand($"dropclient {entity.EntRef} ^0[^5UiC Anti Cheat^0] ^7Cheat Detected. Discord: uic.elitesnipers.pw/discord"));

                Utilities.RawSayAll($"Player ^3{entity.Name} ^7kicked by ^3UiC^7, Reason: Contact Musta. Discord: uic.elitesnipers.pw/discord");
                WebhookManager.SendWebhookSuspect(entity, "HWID Length is bad format.", Server.Hostname.RemoveColors(), "**Player kicked for bad HWID**", "UiC Anti-Cheat", AntiForceClass.DsrName, Color.Aqua);
            }
        }

        public override void Dispose()
        {
        }
        
    }
}
