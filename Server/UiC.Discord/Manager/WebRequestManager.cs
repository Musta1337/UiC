using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Reflection;

namespace UiC.Discord.Manager
{
    public class WebRequestManager : Singleton<WebRequestManager>
    {
        public const string API_URL = "http://x.x.x.x:9251/";
        public const string API_KEY = "KEY";

        public string Get(string path)
        {
            var request = (HttpWebRequest)WebRequest.Create(API_URL + path);
            request.Headers.Add("APIKey", API_KEY);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public string Put(string path)
        {
            var request = (HttpWebRequest)WebRequest.Create(API_URL + path);

            var data = new byte[0];

            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("APIKey", API_KEY);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public string Post(string path)
        {
            var request = (HttpWebRequest)WebRequest.Create(API_URL + path);

            var data = new byte[0];

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("APIKey", API_KEY);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }

}
