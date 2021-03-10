using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Sirtatji.Application.VoiceNotifier
{
    public class RandomVoiceGenerator : IRandomVoiceGenerator
    {
        private readonly Random _random;

        private readonly Lazy<string> _arrivalsPath;

        private readonly Lazy<string> _departuresPath;

        public RandomVoiceGenerator(Random random, IConfiguration configuration)
        {
            _random = random;
            _arrivalsPath = new Lazy<string>(() => configuration["voices:arrivals"]);
            _departuresPath = new Lazy<string>(() => configuration["voices:departures"]);
        }

        public string GetNextVoicePath(bool joined)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var voicesPath = joined ? _arrivalsPath.Value : _departuresPath.Value;
            var directoryInfo = new DirectoryInfo(Path.GetFullPath(
                Path.Combine(currentDirectory, voicesPath)
            ));
            var files = directoryInfo.GetFiles("*.wav");
            if (!files.Any())
            {
                var type = joined ? "arrivals" : "departures";
                throw new InvalidOperationException($"Could not find any file for {type}");
            }

            var file = files[_random.Next(files.Length)];
            return file.FullName;
        }
    }
}