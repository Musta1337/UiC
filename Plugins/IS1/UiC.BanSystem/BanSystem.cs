using BanSystem.IS1.Extensions;
using InfinityScript;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UiC.Core;
using UiC.Core.Discord;
using UiC.Core.Extensions;
using UiC.Core.Misc;
using UiC.Core.Records;
using UiC.Loader;

namespace BanSystem.IS1
{
    public class BanSystem
    {
        private const string FILE_PATH = @"scripts\UiC\Logs\BanList.json";

        private static Uri m_banListUrl = new Uri("http://x.x.x.x:9251/BanList");
        private static Dictionary<string, BanRecord> m_bans = new Dictionary<string, BanRecord>();
        private static ManualResetEvent m_waitList = new ManualResetEvent(false);

        public BanSystem()
        {

        }

        public void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
               delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) {
                   return true;
               };

            var webClient = new WebClient();

            webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            webClient.DownloadStringAsync(m_banListUrl);

            //Uic_Loader.Instance.PlayerConnecting += BanSystem_PlayerConnecting;

        }

        public void OnPlayerConnecting(Entity player)
        {
            m_waitList.WaitOne(10000);

            var xnAddr = new XnAddr(player, player.GetEntityNumber());

            if(!FastSearchBanEntry(player, xnAddr))
                LongSearchBanEntry(player);
        }

        private bool FastSearchBanEntry(Entity player, XnAddr xnAddr)
        {
            if (m_bans.TryGetValue(xnAddr.Value, out var record))
            {
                Kick(player, record);

                return true;
            }

            return false;
        }

        private void LongSearchBanEntry(Entity player)
        {
            var record = m_bans.Values.FirstOrDefault(x => x.Player.Hwid == player.HWID || x.Player.IP == player.IP.Address.ToString());

            if (record != null)
            {
                Kick(player, record);
            }
        }

        private void Kick(Entity player, BanRecord record)
        {
            LogIt(record);
            Log.Write(LogLevel.Info, "Player Kicked by HwId: " + player.Name + ", Hwid: " + player.HWID + ", IP: " + player.IP.Address.ToString());

            UiC_Loader.Instance.AfterDelay(2000, () =>
            {
                player.Kick("Banned for " + record.Reason + " By ^3UiC^7. Discord: uic.elitesnipers.pw/discord");
            });

            Utilities.RawSayAll($"Player ^3{player.Name} ^7kicked by ^3UiC^7, Reason: {record.Reason}");
            return;
        }

        private static void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                m_bans = JsonConvert.DeserializeObject<Dictionary<string, BanRecord>>(e.Result);

                //Log.Write(LogLevel.Info, "BanList downloaded.");

                using (StreamWriter writer = new StreamWriter(FILE_PATH))
                {
                    writer.Write(e.Result);
                }
            }
            catch (Exception ex)
            {
                //Log.Write(LogLevel.Info, ex.ToString());
                Log.Write(LogLevel.Info, "BanList can't be download, get the local file.");

                if (File.Exists(FILE_PATH))
                {
                    StreamReader reader = new StreamReader(FILE_PATH);
                    var content = reader.ReadToEnd();

                    m_bans = JsonConvert.DeserializeObject<Dictionary<string, BanRecord>>(content);

                }
                else
                {
                    Log.Write(LogLevel.Info, "There is no local BanList, script canceled.");
                }
            }

            m_waitList.Set();
        }

        private void LogIt(BanRecord record)
        {
            WebhookManager.SendWebhook(record.Player.Name, record.Reason, Server.Hostname.RemoveColors(), "**[UiC Ban] Player Banned**", "UiC Ban System", Color.Red);
        }
    }

}
