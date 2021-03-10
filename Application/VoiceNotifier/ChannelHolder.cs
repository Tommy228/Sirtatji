using Discord;
using Functional.Maybe;

namespace Sirtatji.Application.VoiceNotifier
{
    internal class ChannelHolder : IChannelHolder
    {
        private Maybe<IVoiceChannel> _channel = Maybe<IVoiceChannel>.Nothing;

        public void SetChannel(IVoiceChannel channel)
        {
            _channel = channel.ToMaybe();
        }

        public void ClearChannel()
        {
            _channel = Maybe<IVoiceChannel>.Nothing;
        }

        public Maybe<IVoiceChannel> GetChannel() => _channel;
    }
}