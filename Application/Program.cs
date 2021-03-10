using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sirtatji.Application.Command;
using Sirtatji.Application.Startup;
using Sirtatji.Application.VoiceNotifier;

namespace Sirtatji.Application
{
    internal static class Program
    {
        public static Task Main(string[] args) => CreateHostBuilder(args).RunConsoleAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                    config.AddJsonFile("appsettings.json", false)
                )
                .ConfigureServices(services =>
                    services
                        .AddSingleton(provider => new DiscordSocketClient(new DiscordSocketConfig
                        {
                            LogLevel = GetDiscordLogSeverity(provider),
                            MessageCacheSize = 1000
                        }))
                        .AddSingleton(provider => new CommandService(new CommandServiceConfig
                        {
                            LogLevel = GetDiscordLogSeverity(provider),
                            DefaultRunMode = RunMode.Async,
                        }))
                        .AddSingleton<IChannelHolder, ChannelHolder>()
                        .AddScoped<IVoiceSender, VoiceSender>()
                        .AddScoped<Random>()
                        .AddScoped<IRandomVoiceGenerator, RandomVoiceGenerator>()
                        .AddSingleton<IChannelVoiceNotifier, ChannelVoiceNotifier>()
                        .AddHostedService<DiscordStartupService>()
                        .AddHostedService<CommandHandler>()
                        .AddHostedService<ChannelWatcherService>()
                )
                .ConfigureLogging((_, logging) =>
                    logging.AddConsole()
                );

        private static LogSeverity GetDiscordLogSeverity(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logSeverityFromConfig = configuration["discord:logseverity"];
            var isLogSeverityValid = Enum.TryParse(typeof(LogSeverity), logSeverityFromConfig, out var logSeverity);
            return isLogSeverityValid
                ? (LogSeverity) logSeverity
                : throw new InvalidOperationException($"Invalid Log Severity ${logSeverityFromConfig}");
        }
    }
}