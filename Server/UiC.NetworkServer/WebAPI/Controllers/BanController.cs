using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.WebAPI.Results;

namespace UiC.NetworkServer.WebAPI.Controllers
{
    [Route("IsBan/{playerId:int}")]
    public class BanController : ApiController
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

                var player = BanManager.Instance.FindBanRecord(playerId);

                var res = new Result() {
                    Success = false
                };

                if (player != null)
                {
                    res.Success = true;
                }

                result = Json(res);

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
        [Route("IsBan/Ip/{ip}")]
        public IHttpActionResult GetBanRecordByName(string ip)
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

                var res = new Result() {
                    Success = false
                };

                foreach (var record in players)
                {
                    var banRecord = BanManager.Instance.FindBanRecord(record.Id);

                    if (banRecord != null)
                    {
                        res.Success = true;
                    }
                }

                result = Json(res);

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
        [Route("IsBan/XnAddr/{xnAddr}")]
        public IHttpActionResult GetBanRecordByXnAddr(string xnAddr)
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

                var player = PlayerManager.Instance.FindPlayerRecord(xnAddr);

                var res = new Result() {
                    Success = false
                };

                if (player != null)
                {
                    res.Success = true;
                }

                result = Json(res);

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
        [Route("BanList")]
        public IHttpActionResult GetBanList()
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

                result = Json(BanManager.Instance.GetBanRecords().Where(x => x.Player != null).ToDictionary(x => x.Player.XnAddr));

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
