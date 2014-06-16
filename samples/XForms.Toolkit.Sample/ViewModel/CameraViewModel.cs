using System.Threading;
using System.Threading.Tasks;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Media;
using XForms.Toolkit.Mvvm;
using Xamarin.Forms;

namespace XForms.Toolkit.Sample
{
	/// <summary>
	/// Class CameraViewModel.
	/// </summary>
	[ViewType(typeof(CameraPage))]
	public class CameraViewModel : ViewModel
	{
		/// <summary>
		/// The _picture chooser
		/// </summary>
		private IMediaPicker _mediaPicker;
		/// <summary>
		/// The _image source
		/// </summary>
		private ImageSource _imageSource;
		/// <summary>
		/// The _take picture command
		/// </summary>
		private RelayCommand _takePictureCommand;

		private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		//private CancellationTokenSource cancelSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraViewModel"/> class.
		/// </summary>
		public CameraViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the image source.
		/// </summary>
		/// <value>The image source.</value>
		public ImageSource ImageSource
		{
			get
			{
				return _imageSource;
			}
			set
			{
				this.ChangeAndNotify(ref _imageSource, value);
				base.NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets the take picture command.
		/// </summary>
		/// <value>The take picture command.</value>
		public RelayCommand TakePictureCommand 
		{
			get
			{ 
				return _takePictureCommand ?? (_takePictureCommand = new RelayCommand (
					() => TakePicture(),

					() => true)); 
			}
		}

		/// <summary>
		/// Setups this instance.
		/// </summary>
		void Setup(){
			if (_mediaPicker != null)
			{
				return;
			}

			var device = Resolver.Resolve<IDevice>();
			_mediaPicker = device.MediaPicker;
		}
		/// <summary>
		/// Takes the picture.
		/// </summary>
		private async Task TakePicture ()
		{
			Setup();

			ImageSource = null;

			await this._mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400}).ContinueWith(t =>
			{
				if (t.IsFaulted)
				{
					var s = t.Exception.InnerException.ToString();
				}
				else if (t.IsCanceled)
				{
					var canceled = true;
				}
				else
				{
					var mediaFile = t.Result;

					ImageSource = ImageSource.FromStream(() => mediaFile.Source);

					return mediaFile;
				}

				return null;
			}, _scheduler);
		}
	}
}

