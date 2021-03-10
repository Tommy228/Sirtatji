namespace Sirtatji.Application.VoiceNotifier
{
    public interface IRandomVoiceGenerator
    {
        public string GetNextVoicePath(bool joined);
    }
}