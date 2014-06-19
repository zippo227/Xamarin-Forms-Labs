using System.Collections.Generic;
using System.Linq;
using MonoTouch.AVFoundation;
using Xamarin.Forms;
using XForms.Toolkit.Services;

[assembly: Dependency(typeof(XForms.Toolkit.iOS.Services.TextToSpeechService))]
namespace XForms.Toolkit.iOS.Services
{
	public class TextToSpeechService : ITextToSpeechService
	{
	    public void Speak(string text)
		{
			var speechSynthesizer = new AVSpeechSynthesizer();

			var speechUtterance = new AVSpeechUtterance(text)
			{
				Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
				Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
				Volume = 0.5f,
				PitchMultiplier = 1.0f
			};

			speechSynthesizer.SpeakUtterance(speechUtterance);
		}


        public IEnumerable<string> GetInstalledLanguages()
        {
            return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => a.Language).Distinct();
        }
    }
}

