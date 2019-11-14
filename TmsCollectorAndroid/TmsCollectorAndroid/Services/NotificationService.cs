using System.Threading.Tasks;
using Prism.Services;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService(IPageDialogService dialogService,
            ITextToSpeechService textToSpeechService,
            ISoundService soundService)
        {
            _dialogService = dialogService;
            _textToSpeechService = textToSpeechService;
            _soundService = soundService;
        }

        private readonly IPageDialogService _dialogService;
        private readonly ITextToSpeechService _textToSpeechService;
        private readonly ISoundService _soundService;

        public async Task NotifyAsync(string title, string message, bool textToSpeech)
        {
            if (textToSpeech)
                await _textToSpeechService.SpeakAsync(message);

            await _dialogService.DisplayAlertAsync(title, message, "OK");
        }

        public async Task NotifyAsync(string message, SoundEnum sound, string title = "Atenção")
        {
            await _soundService.PlaySoundAsync(sound);

            await _dialogService.DisplayAlertAsync(title, message, "OK");
        }

        public async Task<bool> AskQuestionAsync(string title, string message, string acceptBunttonText,
            string cancelButtonText, bool textToSpeech)
        {
            if (textToSpeech)
                await _textToSpeechService.SpeakAsync(message);

            return await _dialogService.DisplayAlertAsync(title,
                message,
                acceptBunttonText,
                cancelButtonText);
        }

        public async Task<bool> AskQuestionAsync(string message, SoundEnum sound, string title = "Atenção", string acceptBunttonText = "SIM",
            string cancelButtonText = "NÃO")
        {
            await _soundService.PlaySoundAsync(sound);

            return await _dialogService.DisplayAlertAsync(title,
                message,
                acceptBunttonText,
                cancelButtonText);
        }
    }
}