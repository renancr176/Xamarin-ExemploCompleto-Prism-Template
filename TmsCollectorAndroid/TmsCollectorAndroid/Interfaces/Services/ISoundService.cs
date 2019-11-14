using System.IO;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using TmsCollectorAndroid.Enums;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface ISoundService
    {
        ISimpleAudioPlayer Player { get; }

        Stream GetAudioStream(SoundEnum sound);

        Task PlaySoundAsync(SoundEnum sound);
    }
}