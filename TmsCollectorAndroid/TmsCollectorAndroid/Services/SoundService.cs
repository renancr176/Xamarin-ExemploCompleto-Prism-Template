using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Services
{
    public class SoundService : ISoundService
    {
        public ISimpleAudioPlayer Player { get; private set; }

        public SoundService()
        {
            Player = CrossSimpleAudioPlayer.Current;
        }

        public Stream GetAudioStream(SoundEnum sound)
        {
            try
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                return assembly.GetManifestResourceStream($"TmsCollectorAndroid.Assets.Sounds.{sound.GetDescription()}");
            }
            catch (Exception) { }

            return null;
        }

        public async Task PlaySoundAsync(SoundEnum sound)
        {
            var audioStream = GetAudioStream(sound);
            if (audioStream != null && Player.Load(audioStream))
            {
                Player.Play();
            }
        }
    }
}