using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Network.Protocol.Types;
using UiC.NetworkServer.Extensions;
using UiC.NetworkServer.Managers;
using UiC.NetworkServer.Records;

namespace UiC.NetworkServer.Network
{
    public class TeknoServer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ServerRecord Record
        {
            get;
            set;
        }

        public string Hostname
        {
            get
            {
                return Record.Name;
            }
            set
            {
                Record.Name = value;
            }
        }

        public string IP
        {
            get => Record.IP;
        }

        private List<Player> m_players = new List<Player>();

        public List<Player> Players
        {
            get => m_players;
        }

        public TeknoServer(string hostname, string ip)
        {

        }

        public void LoadRecord(string hostname, string ip)
        {
            Server.Instance.IOTaskPool.EnsureContext();

            Record = ServerManager.Instance.FindServerRecord(hostname, ip);

            if (Record == null)
            {
                Record = new ServerRecord() {
                    Name = hostname,
                    IP = ip,
                    IsOnline = true,
                    RowAdded = DateTime.Now,
                    RowUpdated = DateTime.Now
                };

                ServerManager.Instance.Database.Insert(Record);
            }
            else
            {
                Record.RowUpdated = DateTime.Now;
                Record.IsOnline = true;

                ServerManager.Instance.Database.Update(Record);
            }
        }

        public void AddPlayer(Player player)
        {
            Server.Instance.IOTaskPool.EnsureContext();

            var playerRecord = PlayerManager.Instance.FindPlayerRecordByHwid(player.Hwid);

            if (playerRecord == null)
            {
                playerRecord = new PlayerRecord() {
                    Name = player.Name,
                    Guid = player.Guid.ToString(),
                    Aliases = new List<string>(),
                    Hwid = player.Hwid,
                    NewHwid = player.NewHwid,
                    XnAddr = player.XnAddr,
                    IP = player.IP,
                    RowAdded = DateTime.Now,
                    RowUpdated = DateTime.Now
                };

                PlayerManager.Instance.Database.Insert(playerRecord);
            }
            else
            {
                playerRecord.RowUpdated = DateTime.Now;

                if (playerRecord.Name != player.Name && !playerRecord.Aliases.Contains(player.Name))
                {
                    playerRecord.Aliases.Add(player.Name);
                    playerRecord.AliasesCSV = playerRecord.Aliases.ToCSV(",");
                }

                if (string.IsNullOrEmpty(playerRecord.NewHwid))
                {
                    playerRecord.NewHwid = player.NewHwid;
                }

                PlayerManager.Instance.Database.Update(playerRecord);
            }

            Players.Add(player);

        }

    

        public void RemovePlayer(int entef)
        {
            var player = Players.FirstOrDefault(x => x.EntRef == entef);

            if(player != null)
            {
                m_players.Remove(player);
            }
        }

    }
}
