using System.Threading.Tasks;
using Discord;

namespace Sirtatji.Application.VoiceNotifier
{
    public interface IChannelVoiceNotifier
    {
        public Task SendVoice(IAudioChannel channel, bool joined);
    }
}