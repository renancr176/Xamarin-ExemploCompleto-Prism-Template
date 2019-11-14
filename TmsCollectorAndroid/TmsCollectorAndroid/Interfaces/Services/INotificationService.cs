using System.Threading.Tasks;
using TmsCollectorAndroid.Enums;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string message, string title = "Atençaõ", bool textToSpeech = false);
        Task NotifyAsync(string message, SoundEnum sound, string title = "Atenção");

        Task<bool> AskQuestionAsync(string message, string title = "Atenção", string acceptBunttonText = "SIM",
            string cancelButtonText = "NÃO", bool textToSpeech = false);

        Task<bool> AskQuestionAsync(string message, SoundEnum sound, string title = "Atenção",
            string acceptBunttonText = "SIM", string cancelButtonText = "NÃO");
    }
}