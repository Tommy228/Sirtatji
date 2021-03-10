using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sirtatji.Application.Command
{
    public class CommandHandler : IHostedService
    {
        private const string CommandPrefix = "!";

        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IServiceProvider provider,
            ILogger<CommandHandler> logger
        )
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _discord.MessageReceived += OnMessageReceivedAsync;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;

            var context = new SocketCommandContext(_discord, msg);

            var argPos = 0;
            if (msg.HasStringPrefix(CommandPrefix, ref argPos) ||
                msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                try
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _provider);

                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                        await context.Channel.SendMessageAsync($"Error: {result.ErrorReason}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error executing the command {msg}", e);
                    await context.Channel.SendMessageAsync("Error executing the command");
                }
            }
        }
    }
}