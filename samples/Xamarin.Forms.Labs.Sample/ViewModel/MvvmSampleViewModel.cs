using System;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample
{
	[ViewType(typeof(MvvmSamplePage))]
	public class MvvmSampleViewModel : ViewModel
	{
		public MvvmSampleViewModel ()
		{
		
		}


		private Command _navigateToViewModel;
		public Command NavigateToViewModel 
		{
			get
			{ 
				return _navigateToViewModel ?? new Command (
					async () => await Navigation.PushAsync<NewPageViewModel>() , 
					() => true); 
			}
		}

		private string _navigateToViewModelButtonText = "Navigate to another view model";
		public string NavigateToViewModelButtonText
		{
			get
			{
				return _navigateToViewModelButtonText;
			}
			set
			{ 
				this.ChangeAndNotify(ref _navigateToViewModelButtonText, value);
			}
		}

	}
}

