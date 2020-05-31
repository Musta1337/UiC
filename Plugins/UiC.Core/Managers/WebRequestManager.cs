using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Discord;
using UiC.Core.Threading;

namespace UiC.Core.Managers
{
    public static class WebRequestManager
    {
        private static SelfRunningTaskPool m_taskpool;
        private static bool isInitialized;

        public static void Initialize()
        {
            if (isInitialized)
                return;

            m_taskpool = new SelfRunningTaskPool(500, "Web TaskPool");
            m_taskpool.Start();

            isInitialized = true;
        }

        public static void SendWebhook(Webhook webhook)
        {
            Initialize();

            if (webhook.Embeds.Count > 0)
                Log.Write(LogLevel.Info, "Send Webhook: " + webhook.Embeds[0].Description);

            m_taskpool.AddMessage(webhook.Send);
        }
    }
}
