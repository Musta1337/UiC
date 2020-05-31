using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.Network;
using UiC.NetworkServer.WebAPI.Results;

namespace UiC.NetworkServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Server/{serverId:int}")]
    public class AccountController : ApiController
    {
        public IHttpActionResult Get(int serverId)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }

                var server = ClientManager.Instance.Clients.FirstOrDefault(x => x.TeknoServer != null && x.TeknoServer.Record.Id == serverId);

                if (server == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(server.TeknoServer);

                resetEvent.Set();
                return;
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;

        }

        [HttpGet]
        [Route("Servers")]
        public IHttpActionResult GetServersList()
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }

                var server = ServerManager.Instance.FindServersRecord();

                if (server == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(server);

                resetEvent.Set();
                return;
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;


        }

        [HttpGet]
        [Route("Servers/online")]
        public IHttpActionResult GetOnlineServersList()
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }


                Dictionary<int, TeknoServer> _servers = new Dictionary<int, TeknoServer>();

                foreach(var server in ClientManager.Instance.Clients.Select(x => x.TeknoServer))
                {
                    if(server != null)
                    {
                        if(!_servers.TryGetValue(server.Record.Id, out var value))
                        {
                            _servers.Add(server.Record.Id, server);
                        }
                    }
                }

                if (_servers == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(_servers);

                resetEvent.Set();
                return;
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;
        }

        [CustomAuthorize]
        [HttpPut]
        [Route("Announce/{text}")]
        public IHttpActionResult Announce(string text)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }

                ServerManager.Instance.SendMessageToAll(text);

                result = Json(new Result() {
                    Success = true
                });

                resetEvent.Set();
                return;
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;
        }

        [CustomAuthorize]
        [HttpPut]
        [Route("Announce/{serverId:int}/{text}")]
        public IHttpActionResult AnnounceToServer(int serverId, string text)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (timeout)
                {
                    return;
                }

                ServerManager.Instance.SendMessageToServer(serverId, text);

                result = Json(new Result() {
                    Success = true
                });

                resetEvent.Set();
                return;
            });

            if (!resetEvent.Wait(15 * 1000))
            {
                timeout = true;
                return InternalServerError(new TimeoutException());
            }

            return result;
        }

    }
}
