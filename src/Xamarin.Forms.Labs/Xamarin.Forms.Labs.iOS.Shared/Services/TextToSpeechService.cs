using System.Collections.Generic;
using System.Linq;
#if __UNIFIED__
using AVFoundation;
#elif __IOS__
using MonoTouch.AVFoundation;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.iOS.Services.TextToSpeechService))]
namespace Xamarin.Forms.Labs.iOS.Services
{
    /// <summary>
    /// The text to speech service for iOS.
    /// </summary>
    public class TextToSpeechService : ITextToSpeechService
	{
	    /// <summary>
	    /// The speak.
	    /// </summary>
	    /// <param name="text">
	    /// The text.
	    /// </param>
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


	    /// <summary>
	    /// Get installed languages.
	    /// </summary>
	    /// <returns>
	    /// The installed language names.
	    /// </returns>
	    public IEnumerable<string> GetInstalledLanguages()
        {
            return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => a.Language).Distinct();
        }
    }
}

