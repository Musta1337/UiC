using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Loader;
using UiC.Core.Misc;
using UiC.Core;

namespace UiC.Performance
{
    public class PerformancePlugin
    {
        public PerformancePlugin()
        {

        }



        public void SetIcon(Entity player, string icon)
        {
            player.SetField("statusicon", new Parameter(icon));

            player.SpawnedPlayer += () =>
            {
                UiC_Loader.Instance.AfterDelay(1000, () =>
                {
                    player.SetField("statusicon", new Parameter(icon));
                });
            };
        }

        public void SetDvars(Entity player)
        {
            player.SetClientDvar( "cg_objectiveText", (string)"^1Powered by ^7Ui^3Community"); //Mainly based on Atlas Admin Script
            player.SetClientDvar( "rate", "60000");
            player.SetClientDvar( "sys_lockThreads", "all");
            player.SetClientDvar( "com_maxFrameTime", "1000");
            player.SetClientDvar("r_distortion", "0");
            player.SetClientDvar( "r_drawSun", "0");
            player.SetClientDvar( "r_drawWater", "0");
            player.SetClientDvar( "cg_brass", "0");
            player.SetClientDvar( "cl_demo_client", "1"); // could cause demos to not record.
            player.SetClientDvar( "g_motd", (string)"This server is protected by United iSnipe ^3Community."); //Just making sure.
            player.SetClientDvar( "didyouknow", (string)"This server is protected by United iSnipe ^3Community.");
            player.SetClientDvar( "motd", (string)"This server is protected by United iSnipe ^3Community.");
            player.SetClientDvar( "cl_demo_enabled", "1");// could cause demos to not record.
            player.SetClientDvar("r_zfar", "3500"); //Idk what I added this.
        }

        public void SetSpawnDvar(Entity player)
        {
            player.SetClientDvar("cg_objectiveText", (string)"^1Powered by ^7Ui^3Community");
        }

        public void SetServerDvars()
        {
            Function.Call("setdvar", "sv_network_fps", "200");
            Function.Call("makedvarserverinfo", "didyouknow", "This server is protected by United iSnipe ^3Community.");//Just making sure.
            Function.Call("makedvarserverinfo", "g_motd", "This server is protected by United iSnipe ^3Community.");
            Function.Call("makedvarserverinfo", "motd", "This server is protected by United iSnipe ^3Community.");
            Function.Call("setdvar", "sv_network_fps", "200");
            Function.Call("setdvar", "sv_hugeSnapshotSize", "10000");
            Function.Call("setdvar", "sv_hugeSnapshotDelay", "100");
            Function.Call("setdvar", "sv_pingDegradation", "0");
            Function.Call("setdvar", "sv_pingDegradationLimit", "9999");
            Function.Call("setdvar", "sv_acceptableRateThrottle", "9999");
            Function.Call("setdvar", "sv_newRateThrottling", "2");
            Function.Call("setdvar", "sv_minPingClamp", "50");
            Function.Call("setdvar", "sv_cumulThinkTime", "1000");
            Function.Call("setdvar", "sys_lockThreads", "all");
        }
    }
}
