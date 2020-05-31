using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.Network;

namespace UiC.NetworkServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Player/{playerId:int}")]
    public class PlayerController : ApiController
    {
        public IHttpActionResult Get(int playerId)
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

                var players = PlayerManager.Instance.FindPlayerRecordById(playerId);

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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
        [Route("Players")]
        public IHttpActionResult GetPlayers()
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

                var players = PlayerManager.Instance.GetPlayerRecords();

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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
        [Route("Player/Name/{playerName}")]
        public IHttpActionResult GetPlayersByName(string playerName)
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

                var players = PlayerManager.Instance.FindPlayerRecordByName(playerName);

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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
        [Route("Player/Ip/{ip}")]
        public IHttpActionResult GetPlayersByIP(string ip)
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

                var players = PlayerManager.Instance.FindPlayerRecordByIP(ip);

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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
        [Route("Player/Hwid/{hwid}")]
        public IHttpActionResult GetPlayersByHwid(string hwid)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }

                var players = PlayerManager.Instance.FindPlayerRecordByHwid(hwid);

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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
        [Route("Player/NewHwid/{hwid}")]
        public IHttpActionResult GetPlayersByNewHwid(string hwid)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }

                var players = PlayerManager.Instance.FindPlayerRecordByNewHwid(hwid);

                if (players == null)
                {
                    result = NotFound();

                    resetEvent.Set();
                    return;
                }

                result = Json(players);

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

        [HttpPost]
        [Route("Servers/Kick/{playerId:int}/{reason}")]
        public IHttpActionResult KickPlayer(int playerId, string reason)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }

                ServerManager.Instance.KickPlayer(playerId, reason);

                result = Ok();

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

        [HttpPut]
        [Route("Ban/{playerId:int}/{reason}")]
        public IHttpActionResult AddBan(int playerId, string reason)
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

                result = Json(ServerManager.Instance.AddBan(playerId, reason, "WebAPI"));

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

        [HttpPut]
        [Route("Server/{serverId:int}/Ban/{playerId:int}/{reason}")]
        public IHttpActionResult AddBan(int serverId, int playerId, string reason)
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

                result = Json(ServerManager.Instance.AddBan(serverId, playerId, reason, "UiC Staff"));

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

        [HttpPost]
        [Route("Server/{serverId:int}/Kick/{playerId:int}/{reason}")]
        public IHttpActionResult KickPlayer(int serverId, int playerId, string reason)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }

                result = Json(ServerManager.Instance.KickPlayer(serverId, playerId, reason));

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

        [HttpPost]
        [Route("Server/{serverId:int}/Ban/{playerId:int}/{reason}")]
        public IHttpActionResult BanPlayer(int serverId, int playerId, string reason)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }

                result = Json(ServerManager.Instance.AddBan(serverId, playerId, reason, "UiC Staff"));

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
        [Route("Server/{serverId:int}/Player/{playerId:int}")]
        public IHttpActionResult FindPlayer(int serverId, int playerId)
        {
            IHttpActionResult result = null;
            bool timeout = false;
            var resetEvent = new ManualResetEventSlim();

            Server.Instance.IOTaskPool.AddMessage(() =>
            {
                if (timeout)
                {
                    return;
                }
                var player = ServerManager.Instance.FindPlayer(serverId, playerId);

                result = Json(player);

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