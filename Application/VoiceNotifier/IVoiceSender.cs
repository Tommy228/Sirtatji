using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;

namespace Sirtatji.Application.VoiceNotifier
{
    public interface IVoiceSender
    {
        /// <summary>
        /// Send a voice
        /// </summary>
        /// <param name="client">The Discord audio client</param>
        /// <param name="path">The path to the audio file of the voice to send</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendAsync([NotNull] IAudioClient client, [NotNull] string path, CancellationToken cancellationToken);
    }
}