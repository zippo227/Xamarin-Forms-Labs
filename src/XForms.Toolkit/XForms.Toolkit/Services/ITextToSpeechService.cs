
using System.Collections.Generic;

namespace XForms.Toolkit.Services
{
	//http://developer.xamarin.com/guides/cross-platform/xamarin-forms/dependency-service/
	public interface ITextToSpeechService
	{
		void Speak(string text);

	    IEnumerable<string> GetInstalledLanguages();
	}
}
