
namespace XLabs.Platform.Services
{
	using System.Collections.Generic;

	/// <summary>
	/// The Text To Speech Service interface.
	/// </summary>
	public interface ITextToSpeechService
	{
		/// <summary>
		/// The speak method.
		/// </summary>
		/// <param name="text">
		/// The text to speak.
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
