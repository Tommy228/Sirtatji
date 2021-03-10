using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Microsoft.Extensions.Logging;

namespace Sirtatji.Application.VoiceNotifier
{
    public class VoiceSender : IVoiceSender
    {
        private readonly ILogger<VoiceSender> _logger;

        public VoiceSender(ILogger<VoiceSender> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SendAsync(IAudioClient client, string path, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Sending voice {path}");
            
            using var ffmpeg = CreateStream(path);
            await using var output = ffmpeg.StandardOutput.BaseStream;
            await using var audioStream = client.CreatePCMStream(AudioApplication.Mixed);
            try
            {
                await output.CopyToAsync(audioStream, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                _logger.LogError("Voice send failure", e);
            }
            finally
            {
                await audioStream.FlushAsync(cancellationToken);
            }
        }

        private static Process CreateStream(string path) =>
            Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
    }
}