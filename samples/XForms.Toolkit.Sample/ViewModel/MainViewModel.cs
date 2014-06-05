using System;
using XForms.Toolkit.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Sample
{
	public class MainViewModel : ViewModel
	{
		public MainViewModel ()
		{
			SpeakCommand = new RelayCommand (() => {
				DependencyService.Get<ITextToSpeechService>().Speak("Hello from XForms Toolkit");
			});
		}

		private RelayCommand _speakCommand= null;
		public RelayCommand SpeakCommand{
			get{
				return _speakCommand;
			}
			private set{ 
				_speakCommand = value;
			}
		}

	}
}

