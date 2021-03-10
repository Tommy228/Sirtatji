using System.Diagnostics.CodeAnalysis;
using Discord;
using Functional.Maybe;

namespace Sirtatji.Application.VoiceNotifier
{
    public interface IChannelHolder
    {
        public void SetChannel([NotNull] IVoiceChannel channel);

        public void ClearChannel();

        public Maybe<IVoiceChannel> GetChannel();
    }
}