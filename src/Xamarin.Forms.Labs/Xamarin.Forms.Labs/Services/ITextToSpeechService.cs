
using System.Collections.Generic;

namespace Xamarin.Forms.Labs.Services
{
    /// <summary>
    /// The Text To Speech Service interface.
    /// </summary>
    public interface ITextToSpeechService
	{
	    /// <summary>
	    /// The speak.
	    /// </summary>
	    /// <param name="text">
	    /// The text.
	    /// </param>
	    void Speak(string text);

	    /// <summary>
	    /// Get installed languages.
	    /// </summary>
	    /// <returns>
	    /// The installed language names.
	    /// </returns>
	    IEnumerable<string> GetInstalledLanguages();
	}
}
