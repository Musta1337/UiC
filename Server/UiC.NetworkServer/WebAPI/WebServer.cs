using Microsoft.Owin.Hosting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace UiC.NetworkServer.WebAPI
{
    public class WebServer
    {
        public static int WebAPIPort = 9251;

        public static string WebAPIKey = "Key";

        public static void Initialize()
        {
            // Start OWIN host
            WebApp.Start<Startup>(url: $"http://x.x.x.x:{WebAPIPort}/");
        }

        public class ErrorMessageResult : IHttpActionResult
        {
            private readonly string Message;
            private readonly HttpStatusCode StatusCode;

            public ErrorMessageResult(string message, HttpStatusCode statusCode)
            {
                Message = message;
                StatusCode = statusCode;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(StatusCode)
                {
                    Content = new StringContent(Message)
                };

                return Task.FromResult(response);
            }
        }
    }
}