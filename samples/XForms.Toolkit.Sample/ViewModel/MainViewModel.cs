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
			Items = new ObservableCollection<string> ();
			for (int i = 0; i < 10; i++) {
				Items.Add(string.Format("item {0}",i));
			}
		}

		private ObservableCollection<string> _items= null;
		public ObservableCollection<string> Items{
			get{
				return _items;
			}
			 set{ 
				_items = value;
				NotifyPropertyChanged ("Items");
			}
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

		private RelayCommand<string> _searchCommand;
		public RelayCommand<string> SearchCommand {
			get{ return _searchCommand ?? new RelayCommand<string> (
				obj => {


				}, obj => !string.IsNullOrEmpty (obj)); }
		}


	}
}

