using System;
using Xamarin.Forms.Labs.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample
{
	public class MainViewModel : ViewModel
	{
        private IDevice device;

		public MainViewModel ()
		{
			SpeakCommand = new Command (() => 
            {
					DependencyService.Get<ITextToSpeechService>().Speak(TextToSpeak);
			});

			
            Items = new ObservableCollection<string> ();
			
            for (int i = 0; i < 10; i++) 
            {
				Items.Add(string.Format("item {0}",i));
			}

            this.device = Resolver.Resolve<IDevice>();
		}

		public void StartTimer()
        {
			Device.StartTimer (new TimeSpan(6000), () => 
            {
				DeviceTimerInfo ="This text was updated using the Device Timer";
				return true;
			});
		}


        public string DeviceManufacturer
        {
            get
            {
                return string.Format("Device was manufactured by {0}", this.device.Manufacturer);
            }
        }

        public string DeviceName
        {
            get
            {
                return string.Format("Device is called {0}", this.device.Name);
            }
        }

		private string _numberToCall ="+1 (855) 926-2746";
		public string NumberToCall
		{
			get
			{
				return _numberToCall;
			}
			set
			{
				this.ChangeAndNotify(ref _numberToCall, value);
			}
		}


		private string _textToSpeak ="Hello from Xamarin Forms Labs";
		public string TextToSpeak
		{
			get
			{
				return _textToSpeak;
			}
			set
			{
				this.ChangeAndNotify(ref _textToSpeak, value);
			}
		}


        private bool batteryLevel;
        public bool BatteryLevel
        {
            get
            {
                return batteryLevel;
            }
            private set
            {
                this.ChangeAndNotify(ref batteryLevel, value);
            }
        }

		private string _deviceUIThreadInfo = string.Empty;
		public string DeviceUIThreadInfo
        {
			get
            {
				return _deviceUIThreadInfo;
			}
			set
            { 
				this.ChangeAndNotify(ref _deviceUIThreadInfo, value);
			}
		}

		private string _deviceTimerInfo = string.Empty;
		public string DeviceTimerInfo
        {
            get
            {
				return _deviceTimerInfo;
			}
			
            set
            { 
				this.ChangeAndNotify(ref _deviceTimerInfo, value);
			}
		}

		private ObservableCollection<string> _items= null;
		public ObservableCollection<string> Items
        {
            get
            {
                return _items;
            }
            set
            { 
                _items = value;
				this.ChangeAndNotify(ref _items, value);
            }
		}

		private Command _speakCommand= null;
		public Command SpeakCommand
        {
			get
            {
				return _speakCommand;
			}
			private set
            { 
				_speakCommand = value;
			}
		}

		private Command<string> _searchCommand;
		public Command<string> SearchCommand 
        {
			get
            { 
                return _searchCommand ?? new Command<string> (
				    obj => {}, 
                    obj => !string.IsNullOrEmpty (obj)); 
            }
        }

		private Command _callCommand;
		public Command CallCommand 
		{
			get
			{ 
				return _callCommand ?? new Command (
					() => {this.device .PhoneService.DialNumber(NumberToCall);}, 
					() => true); 
			}
		}
	}
}

