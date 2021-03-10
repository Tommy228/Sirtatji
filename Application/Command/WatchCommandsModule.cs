using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Sirtatji.Application.VoiceNotifier;

namespace Sirtatji.Application.Command
{
    public class WatchCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IChannelHolder _channelHolder;

        public WatchCommandsModule(IChannelHolder channelHolder)
        {
            _channelHolder = channelHolder;
        }

        [Command("watch", RunMode = RunMode.Async)]
        public async Task WatchChannel()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync("You're not on a voice channel, dumbass.");
                return;
            }

            await ReplyAsync($"Watching the channel {channel.Name}.");
            _channelHolder.SetChannel(channel);
        }
        
        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopWatching()
        {
            _channelHolder.ClearChannel();
            await ReplyAsync("Not watching any channel now");
        }
    }
}