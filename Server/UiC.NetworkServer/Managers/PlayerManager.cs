using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Reflection;
using UiC.NetworkServer.Records;
using UiC.ORM;

namespace UiC.NetworkServer.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public Database Database
        {
            get => Server.Instance.DBAccessor.Database;
        }

        public PlayerManager()
        {

        }

        public List<PlayerRecord> GetPlayerRecords()
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchQuery).ToList();
        }

        public PlayerRecord FindPlayerRecord(string xnAddr)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchByXnAddr, xnAddr).FirstOrDefault();
        }

        public List<PlayerRecord> FindPlayerRecordByName(string name)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchQuery).Where(x => x.Name.Contains(name)).ToList();
        }

        public List<PlayerRecord> FindPlayerRecordByIP(string ip)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchQueryByIP, ip).ToList();
        }

        public PlayerRecord FindPlayerRecordByHwid(string hwid)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchQueryByHwid, hwid).FirstOrDefault();
        }

        public PlayerRecord FindPlayerRecordByNewHwid(string hwid)
        {
            Server.Instance.IOTaskPool.EnsureContext();
            return Database.Query<PlayerRecord>(PlayerRelator.FetchQueryByNewHwid, hwid).FirstOrDefault();
        }

        public PlayerRecord FindPlayerRecordById(int id)
        {
            return Server.Instance.DBAccessor.Database.Query<PlayerRecord>(PlayerRelator.FetchQueryById, id).FirstOrDefault();
        }
    }
}
