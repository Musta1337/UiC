using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using UiC.Discord.Manager;
using UiC.Discord.Services;
using UiC.NetworkServer.Network;

namespace UiC.Discord
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private IConfiguration _config;
        private Timer _timer;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _config = BuildConfig();

            var services = ConfigureServices();
            services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);

            var token = _config["token"];
            _client.Connected += _client_Connected;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            await Task.Delay(-1);
        }

        private Task _client_Connected()
        {
            Console.WriteLine("Connected");

            _timer = new Timer(60000);
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            return Task.CompletedTask;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = WebRequestManager.Instance.Get("Servers/online");
            var servers = JsonConvert.DeserializeObject<Dictionary<int, TeknoServer>>(result);
            var players = servers.Select(x => x.Value.Players);

            _client.SetGameAsync($"{servers.Count} servers online. {players.Sum(x => x.Count())} Players");
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                // Logging
                .AddLogging()
                .AddSingleton<LogService>()
                // Extra
                .AddSingleton(_config)
                // Add additional services here...
                .BuildServiceProvider();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}

