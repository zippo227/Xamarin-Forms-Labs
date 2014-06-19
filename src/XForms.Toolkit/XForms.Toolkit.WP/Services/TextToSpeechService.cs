using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.Speech.Synthesis;
using Xamarin.Forms;
using XForms.Toolkit.Services;

[assembly: Dependency(typeof(XForms.Toolkit.WP.Services.TextToSpeechService))]

namespace XForms.Toolkit.WP.Services
{
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
            synth = new SpeechSynthesizer();
        }

        public async void Speak(string text)
        {
            await synth.SpeakTextAsync(text);
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