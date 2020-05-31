using InfinityScript;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using UiC.Loader.Plugins;
using UiC.Loader.Records;

namespace UiC.Loader
{
    public class UiC_Loader : BaseScript
    {
        public ManualResetEvent PluginLoadedEvent = new ManualResetEvent(false);

        public static UiC_Loader Instance
        {
            get;
            protected set;
        }

        public UiC_Loader()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
              delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                  return true;
              };

            Directory.CreateDirectory("./scripts/UiC/Logs/Players");

            Instance = this;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            CheckUpdates();
            PluginManager.Instance.LoadAllPlugins();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //base.Notified += UiC_Loader_Notified;
            base.PlayerConnecting += UiC_Loader_PlayerConnecting; ;
            base.PlayerConnected += UiC_Loader_PlayerConnected;
            base.PlayerDisconnected += UiC_Loader_PlayerDisconnected;
        }

        private void UiC_Loader_Notified(string str, Parameter[] parameters)
        {
            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                plugin.Plugin.OnPlayerNotified(str, parameters);
            }
        }

        private void UiC_Loader_PlayerDisconnected(Entity obj)
        {
            PluginLoadedEvent.WaitOne(10000);

            foreach(var plugin in PluginManager.Instance.GetPlugins())
            {
                plugin.Plugin.OnPlayerDisconnected(obj);
            }
        }

        private void UiC_Loader_PlayerConnecting(Entity obj)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            PluginLoadedEvent.WaitOne(10000);

            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                Log.Write(LogLevel.Info, $"Process {plugin.Plugin.Name}...");
                plugin.Plugin.OnPlayerConnecting(obj);
            }

            watch.Stop();
            Log.Write(LogLevel.Info, "Player connecting in " + watch.ElapsedMilliseconds + "ms");
        }

        private void UiC_Loader_PlayerConnected(Entity obj)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            PluginLoadedEvent.WaitOne(10000);

            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                Log.Write(LogLevel.Info, $"Process {plugin.Plugin.Name}...");
                plugin.Plugin.OnPlayerConnected(obj);
            }

            watch.Stop();
            Log.Write(LogLevel.Info, "Player connected in " + watch.ElapsedMilliseconds + "ms");
        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            var result = EventEat.EatNone;

            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                var eventEat = plugin.Plugin.OnSay3(player, type, name, ref message);
                if (eventEat != EventEat.EatNone)
                    result = eventEat;                    
            }

            return result;
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                plugin.Plugin.OnPlayerDamage(player, inflictor, attacker, damage, dFlags, mod, weapon, point, dir, hitLoc);
            }
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            foreach (var plugin in PluginManager.Instance.GetPlugins())
            {
                plugin.Plugin.OnPlayerKilled(player, inflictor, attacker, damage, mod, weapon, dir, hitLoc);
            }
        }


        private void CheckUpdates()
        {
            var useLatestIS = IsLatestIS();

            Log.Write(LogLevel.Info, "Use InfinityScript 1.5: " + useLatestIS);

            string hashs = new WebClient().DownloadString("http://uic.elitesnipers.pw/downloads/UiC.hashs");

            var files = JsonConvert.DeserializeObject<List<FileRecord>>(hashs);

            foreach (var file in files)
            {
                string version = file.Name.Split('/')[0];

                if (version == "IS1" && useLatestIS)
                    continue;

                if (version == "IS1.5" && !useLatestIS)
                    continue;

                var fileName = Path.GetFileName(file.Name);
                var filePath = "./scripts/UiC/" + fileName;

                if (File.Exists(filePath))
                {
                    var hash = GetHash(filePath);

                    if (hash != file.Hash)
                    {
                        File.Delete(filePath);

                        Log.Write(LogLevel.Info, "Updating: " + file.Name);

                        var data = new WebClient().DownloadData("http://uic.elitesnipers.pw/downloads/" + file.Name);
                        File.WriteAllBytes(filePath, data);
                    }
                }
                else
                {
                    Log.Write(LogLevel.Info, "Downloading: " + file.Name);

                    var data = new WebClient().DownloadData("http://uic.elitesnipers.pw/downloads/" + file.Name);
                    File.WriteAllBytes(filePath, data);
                }
            }
            
            foreach(var path in PluginManager.PluginsPath)
            {
                foreach (var file in Directory.GetFiles(path, "*.dll").OrderByDescending(x => x.Contains("UiC.Core.dll")))
                {
                    try
                    {
                        var fileName = Path.GetFileName(file.Substring(2));
                        var fileCompletePath = Path.Combine(path, fileName);


                        if (!files.Any(x => x.Name.Split('/')[1] == fileName))
                        {
                            Log.Write(LogLevel.Info, "Delete file: " + fileName);
                            File.Delete(fileCompletePath);
                        }
                    }
                    catch(Exception e)
                    {
                        Log.Write(LogLevel.Info, "Error:" + e.ToString());
                    }
                }
            }

        }

        private string GetHash(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                MD5 md5Algorithm = MD5.Create();
                byte[] hashBytes = md5Algorithm.ComputeHash(fs);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash;
            }
        }

        public static bool IsLatestIS()
        {
            try
            {
                var assembly = Assembly.GetAssembly(typeof(BaseScript));
                var type = assembly.GetType("InfinityScript.BaseScript", true);
                var methods = type.GetMethods();

                if (methods.Any(x => x.Name == "Call"))
                    return false;

                return true;
            }
            catch
            {
                return true;
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyFile = (args.Name.Contains(','))
                ? args.Name.Substring(0, args.Name.IndexOf(','))
                : args.Name;

            assemblyFile += ".dll";

             Log.Write(LogLevel.Info, "[UiC.Loaader] Load library: " + assemblyFile);

            var path = $"scripts/{assemblyFile}";

            if (!File.Exists(path))
                return null;

            return Assembly.LoadFile(path);
        }
    }
}
