using System.Collections.Generic;
using System.Linq;
using Android.Speech.Tts;
using Java.Util;
using Xamarin.Forms;
using XForms.Toolkit.Services;

[assembly: Dependency(typeof(XForms.Toolkit.Droid.Services.TextToSpeechService))]

namespace XForms.Toolkit.Droid.Services
{
    /// <summary>
    /// The text to speech service implements <see cref="ITextToSpeechService"/> for Android.
    /// </summary>
    public class TextToSpeechService : Java.Lang.Object, ITextToSpeechService, TextToSpeech.IOnInitListener
	{
		private TextToSpeech speaker;
		private string toSpeak;

	    /// <summary>
	    /// The speak.
	    /// </summary>
	    /// <param name="text">
	    /// The text.
	    /// </param>
	    public void Speak(string text)
		{
			var ctx = Forms.Context; // useful for many Android SDK features
			toSpeak = text;
			if (speaker == null)
			{
				speaker = new TextToSpeech(ctx, this);
			}
			else
			{
				var p = new Dictionary<string, string>();
				speaker.Speak(toSpeak, QueueMode.Flush, p);
			}
		}

		#region IOnInitListener implementation

	    /// <summary>
        /// Implementation for <see cref="TextToSpeech.IOnInitListener.OnInit"/>.
	    /// </summary>
	    /// <param name="status">
	    /// The status.
	    /// </param>
	    public void OnInit(OperationResult status)
		{
			if (status.Equals(OperationResult.Success))
			{
				var p = new Dictionary<string, string>();
                this.speaker.Speak(this.toSpeak, QueueMode.Flush, p);
			}
		}
		#endregion

	    /// <summary>
	    /// Get installed languages.
	    /// </summary>
	    /// <returns>
	    /// The installed language names.
	    /// </returns>
	    public IEnumerable<string> GetInstalledLanguages()
        {
            return Locale.GetAvailableLocales().Select(a => a.Language).Distinct();
        }
    }
}