using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sirtatji.Application.VoiceNotifier;

namespace Sirtatji.Application.Startup
{
    internal class DiscordStartupService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly ILogger<DiscordStartupService> _logger;

        public DiscordStartupService(IServiceProvider provider, DiscordSocketClient client, CommandService commands,
            IConfiguration config, ILogger<DiscordStartupService> logger)
        {
            _provider = provider;
            _config = config;
            _logger = logger;
            _client = client;
            _commands = commands;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var discordToken = _config["discord:token"];
            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            _logger.LogInformation("Connected to Discord");
        }

        public Task StopAsync(CancellationToken cancellationToken) => _client.StopAsync();
    }
}