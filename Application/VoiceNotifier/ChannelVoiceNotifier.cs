using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Sirtatji.Application.VoiceNotifier
{
    public class ChannelVoiceNotifier : IChannelVoiceNotifier
    {
        private readonly IVoiceSender _voiceSender;
        private readonly IRandomVoiceGenerator _randomVoiceGenerator;

        private CancellationTokenSource _cancellationTokenSource;

        public ChannelVoiceNotifier(IVoiceSender voiceSender, IRandomVoiceGenerator randomVoiceGenerator)
        {
            _voiceSender = voiceSender;
            _randomVoiceGenerator = randomVoiceGenerator;
        }

        public async Task SendVoice(IAudioChannel channel, bool joined)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            var audioClient = await channel.ConnectAsync();

            await _voiceSender.SendAsync(audioClient, _randomVoiceGenerator.GetNextVoicePath(joined),
                _cancellationTokenSource.Token);

            await audioClient.StopAsync();
        }
    }
}