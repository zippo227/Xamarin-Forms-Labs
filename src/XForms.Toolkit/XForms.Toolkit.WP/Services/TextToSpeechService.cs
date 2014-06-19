using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Phone.Speech.Synthesis;
using Xamarin.Forms;
using XForms.Toolkit.Services;

[assembly: Dependency(typeof(XForms.Toolkit.WP.Services.TextToSpeechService))]

namespace XForms.Toolkit.WP.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechSynthesizer synth;

        public TextToSpeechService()
        {
            synth = new SpeechSynthesizer();
        }

        public async void Speak(string text)
        {
            await synth.SpeakTextAsync(text);
        }

        public IEnumerable<string> GetInstalledLanguages()
        {
            return InstalledVoices.All.Select(a => a.Language).Distinct();
        }
    }
}