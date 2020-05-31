using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Pool;
using UiC.Core.Reflection;
using UiC.Core.Threading;
using UiC.Network.Handlers;
using UiC.Network.Protocol;
using UiC.NetworkServer.Extensions;
using UiC.NetworkServer.Handlers;
using UiC.NetworkServer.Logging;
using UiC.NetworkServer.Network;
using UiC.NetworkServer.WebAPI;
using UiC.ORM;

namespace UiC.NetworkServer
{
    public class Server : Singleton<Server>
    {
        public ClientPacketHandler Handler
        {
            get;
            private set;
        }

        public SelfRunningTaskPool IOTaskPool
        {
            get;
            protected set;
        }

        public ServerPacketHandler HandlerManager
        {
            get;
            protected set;
        }

        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration {
            Host = "x.x.x.x",
            Port = "3306",
            DbName = "uic",
            User = "uic",
            Password = "UiCSystem",
            ProviderName = "MySql.Data.MySqlClient"
        };

        public DatabaseAccessor DBAccessor
        {
            get;
            protected set;
        }

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Initialize()
        {
            Instance = this;

            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;

            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();

            logger.Info("Initializing Database...");
            DBAccessor = new DatabaseAccessor(DatabaseConfiguration);
            DBAccessor.RegisterMappingAssembly(Assembly.GetExecutingAssembly());
            DBAccessor.Initialize();

            logger.Info("Opening Database...");
            DBAccessor.OpenConnection();
            DBAccessor.Database.ExecutingCommand += OnExecutingDBCommand;

            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            HandlerManager = ServerPacketHandler.Instance;
            HandlerManager.RegisterAll(Assembly.GetExecutingAssembly());

            ClientManager.Instance.Initialize(CreateClient);
            ClientManager.Instance.Start("x.x.x.x", 9250);

            WebServer.Initialize();

            IOTaskPool = new SelfRunningTaskPool(50, "Main TaskPool");
            IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(10).TotalMilliseconds, KeepSQLConnectionAlive);
            IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(10).TotalMilliseconds, WriteBufferLogs);

            IOTaskPool.Start();

            logger.Info("Server listening at port 9250");
            logger.Info("WebServer listening at port 9251");


            //     TeknoServer server = new TeknoServer("test", "test");
        }
        private void WriteBufferLogs()
        {
            var path = string.Format("./buffer/bufferleaks {0}.txt", DateTime.Now.ToString("dd-MM hh-mm-ss"));

            using (var file = File.OpenWrite(path))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine("Connected : {0}", ClientManager.Instance.Count);

                    PrintBufferInformations(writer, BufferManager.Default);
                    PrintBufferInformations(writer, BufferManager.Tiny);
                    PrintBufferInformations(writer, BufferManager.Small);
                    PrintBufferInformations(writer, BufferManager.Large);
                    PrintBufferInformations(writer, BufferManager.ExtraLarge);
                    PrintBufferInformations(writer, BufferManager.SuperSized);
                }
            }

        }

        private static void PrintBufferInformations(StreamWriter file, BufferManager manager)
        {
            var leaks = manager.GetSegmentsInUse();

            file.WriteLine("------------- {0} kB {1}/{2} used segments -------------", manager.SegmentSize / 1024, manager.UsedSegmentCount, manager.TotalSegmentCount);
            foreach (var leak in leaks.OrderByDescending(x => DateTime.Now - x.LastUsage))
            {
                var client = leak.Token as BaseClient;
                if (client != null && !client.Connected)
                {
                    file.WriteLine("Client {0} Connected {1}", client, client.Connected);
                }
                if (client == null || !client.Connected)
                {
                    file.WriteLine("Buffer #{0} Size:{1} LastUsage:{2}ago Uses:{4}  Stack:{3}", leak.Number, leak.Length,
                        (DateTime.Now - leak.LastUsage).ToPrettyFormat(), leak.LastUserTrace, leak.Uses);

                    file.WriteLine("");
                }
            }
        }
        private void OnExecutingDBCommand(Database arg1, IDbCommand arg2)
        {
           // logger.Info("Database query: " + arg2.CommandText);

        }

        public BaseClient CreateClient(Socket s)
        {
            return new BaseClient(s);
        }

        protected virtual void KeepSQLConnectionAlive()
        {
            try
            {
                DBAccessor.Database.Execute("DO 1");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Cannot ping SQL connection : {0}", ex);
                logger.Warn("Try to Re-open the connection");
                try
                {
                    DBAccessor.CloseConnection();
                    DBAccessor.OpenConnection();
                }
                catch (Exception ex2)
                {
                    logger.Error(ex, "Cannot reopen the SQL connection : {0}", ex2);
                }
            }
        }
    }
}
