using System.Threading.Tasks;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface ITextToSpeechService
    {
        Task SpeakAsync(string text);
    }
}