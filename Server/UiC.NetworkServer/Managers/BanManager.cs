using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Reflection;
using UiC.Network.Protocol.Types;
using UiC.NetworkServer.Records;
using UiC.ORM;

namespace UiC.NetworkServer.Managers
{
    public class BanManager : Singleton<BanManager>
    {
        private List<PlayerBannedRecord> m_bans = new List<PlayerBannedRecord>() ;

        public List<PlayerBannedRecord> Bans
        {
            get => m_bans;
        }

        public Database Database
        {
            get => Server.Instance.DBAccessor.Database;
        }

        public BanManager()
        {
            Server.Instance.IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(20).TotalMilliseconds, UpdateBanList);
        }

        public void UpdateBanList()
        {
            m_bans = BanManager.Instance.GetBanRecords();
        }




        public void Kick(PlayerBannedRecord banRecord)
        {
            ServerManager.Instance.KickPlayer(banRecord.Player.NewHwid, banRecord.Reason, out var host);
        }

        public List<PlayerBannedRecord> GetBanRecords()
        {
            return Database.Query<PlayerBannedRecord>(PlayerBannedRelator.FetchQuery).ToList();
        }

        public PlayerBannedRecord FindBanRecord(int playerId)
        {
            return Database.Query<PlayerBannedRecord>(PlayerBannedRelator.FetchQueryById, playerId).FirstOrDefault();
        }
    }
}
