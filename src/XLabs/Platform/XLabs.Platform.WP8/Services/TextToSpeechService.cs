using XLabs.Platform.WP8.Services;

[assembly: Dependency(typeof(TextToSpeechService))]
namespace XLabs.Platform.WP8.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Windows.Phone.Speech.Synthesis;

	/// <summary>
    /// The text to speech service implements <see cref="ITextToSpeechService"/> for Windows Phone.
    /// </summary>
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechSynthesizer synth;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextToSpeechService"/> class.
        /// </summary>
        public TextToSpeechService()
        {
            this.synth = new SpeechSynthesizer();
        }

        /// <summary>
        /// The speak.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public async void Speak(string text)
        {
            await this.synth.SpeakTextAsync(text);
        }

        /// <summary>
        /// Get installed languages.
        /// </summary>
        /// <returns>
        /// The installed language names.
        /// </returns>
        public IEnumerable<string> GetInstalledLanguages()
        {
            return InstalledVoices.All.Select(a => a.Language).Distinct();
        }
    }
}