using InfinityScript;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UiC.Core.Discord.Objects;

namespace UiC.Core.Discord
{
    [JsonObject]
    public class Webhook
    {
        private readonly string _webhookUrl;

        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();

        public Webhook(string webhookUrl)
        {
            _webhookUrl = webhookUrl;
        }

        public Webhook(ulong id, string token) : this($"https://discordapp.com/api/webhooks/{id}/{token}")
        {
        }


        public void Send()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(this._webhookUrl);
                var postData = JsonConvert.SerializeObject(this, Formatting.Indented);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Timeout = 10000;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.Info, "Discord error: " + ex);
            }
        }

    }
}
