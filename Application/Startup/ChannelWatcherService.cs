using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Sirtatji.Application.VoiceNotifier;

namespace Sirtatji.Application.Startup
{
    public class ChannelWatcherService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly IChannelHolder _channelHolder;
        private readonly IChannelVoiceNotifier _channelVoiceNotifier;

        public ChannelWatcherService(IChannelHolder channelHolder, DiscordSocketClient client,
            IChannelVoiceNotifier channelVoiceNotifier)
        {
            _channelHolder = channelHolder;
            _client = client;
            _channelVoiceNotifier = channelVoiceNotifier;
        }

        private Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState previousState,
            SocketVoiceState newState)
        {
            if (user.IsBot)
                return Task.CompletedTask;

            if (previousState.VoiceChannel == newState.VoiceChannel)
                return Task.CompletedTask;

            var channel = _channelHolder.GetChannel();
            if (!channel.HasValue)
                return Task.CompletedTask;

            Task.Run(async () =>
            {
                if (previousState.VoiceChannel == channel.Value)
                    await _channelVoiceNotifier.SendVoice(previousState.VoiceChannel, false);

                if (newState.VoiceChannel == channel.Value)
                    await _channelVoiceNotifier.SendVoice(newState.VoiceChannel, true);
            });

            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client.UserVoiceStateUpdated += UserVoiceStateUpdated;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}