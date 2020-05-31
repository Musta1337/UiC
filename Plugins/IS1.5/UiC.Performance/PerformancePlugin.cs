using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Loader;
using UiC.Core.Misc;
using UiC.Core;
using System.Diagnostics;

namespace UiC.Performance
{
    public class PerformancePlugin
    {
        public PerformancePlugin()
        {

        }

        public void SetDvars(Entity player)
        {
            player.SetClientDvar("rate", 60000);
            player.SetClientDvar("sv_network_fps", 200);
            player.SetClientDvar("sys_lockThreads", "all");
            player.SetClientDvar("r_distortion", 0);
            player.SetClientDvar("r_drawSun", 0);
            player.SetClientDvar("r_drawWater", 0);
            player.SetClientDvar("cg_brass", 0);
            player.SetClientDvar("cl_demo_client", 1);
            player.SetClientDvar("g_motd", (Parameter)"This server is protected by United iSnipe ^3Community.");
            player.SetClientDvar("didyouknow", (Parameter)"This server is protected by United iSnipe ^3Community.");
            player.SetClientDvar("cl_demo_enabled", (Parameter)1);
            player.SetClientDvar("r_zfar", (Parameter)3500);
        }

        public void SetIcon(Entity player)
        {
            UiC_Loader.AfterDelay(1000, () =>
            {
                player.StatusIcon = "cardicon_iwlogo";
            });
        }

        public void SetIcon(Entity player, string icon)
        {

        }

        public void SetSpawnDvar(Entity player)
        {
            player.SetClientDvar("cg_objectiveText", (Parameter)"^1Powered by ^7Ui^3Community");
        }

        public void SetServerDvars()
        {
            GSCFunctions.MakeDvarServerInfo("didyouknow", (Parameter)"This server is protected by United iSnipe ^3Community.");
            GSCFunctions.MakeDvarServerInfo("g_motd", (Parameter)"This server is protected by United iSnipe ^3Community.");
            GSCFunctions.MakeDvarServerInfo("motd", (Parameter)"This server is protected by United iSnipe ^3Community.");
            GSCFunctions.SetDvar("sv_network_fps", 200);
            GSCFunctions.SetDvar("sv_hugeSnapshotSize", 10000);
            GSCFunctions.SetDvar("sv_hugeSnapshotDelay", 100);
            GSCFunctions.SetDvar("sv_pingDegradation", 0);
            GSCFunctions.SetDvar("sv_pingDegradationLimit", 9999);
            GSCFunctions.SetDvar("sv_acceptableRateThrottle", 9999);
            GSCFunctions.SetDvar("sv_newRateThrottling", 2);
            GSCFunctions.SetDvar("sv_minPingClamp", 50);
            GSCFunctions.SetDvar("sv_cumulThinkTime", 1000);
            GSCFunctions.SetDvar("sys_lockThreads", "all");
        }
    }
}
