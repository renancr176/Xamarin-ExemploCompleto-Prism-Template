using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Speech.Tts;
using Plugin.CurrentActivity;
using TmsCollectorAndroid.Interfaces.Services;
using Debug = System.Diagnostics.Debug;

namespace TmsCollectorAndroid.Droid.Service
{
    public class TextToSpeechService : Java.Lang.Object, ITextToSpeechService, TextToSpeech.IOnInitListener
    {
        private TextToSpeech speaker;
        private string toSpeak;
        private readonly ICurrentActivity _currentActivity;

        public TextToSpeechService(ICurrentActivity currentActivity)
        {
            _currentActivity = currentActivity;
        }

        public async Task SpeakAsync(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(_currentActivity.Activity, this);
            }
            else
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
                Debug.WriteLine("spoke " + toSpeak);
            }
        }

        #region IOnInitListener implementation

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                Debug.WriteLine("speaker init");
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }
            else
            {
                Debug.WriteLine("was quiet");
            }
        }

        #endregion
    }
}