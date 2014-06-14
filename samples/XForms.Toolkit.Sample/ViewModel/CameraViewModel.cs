using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Camera;
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
		private IPictureChooser _pictureChooser;
		/// <summary>
		/// The _image source
		/// </summary>
		private ImageSource _imageSource;
		/// <summary>
		/// The _take picture command
		/// </summary>
		private RelayCommand _takePictureCommand;

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
					TakePicture, 
					() => true)); 
			}
		}

		/// <summary>
		/// Setups this instance.
		/// </summary>
		void Setup(){
			if (_pictureChooser != null)
			{
				return;
			}

			var device = Resolver.Resolve<IDevice>();
			_pictureChooser = device.PictureChooser;
		}
		/// <summary>
		/// Takes the picture.
		/// </summary>
		private void TakePicture ()
		{
			Setup();

			ImageSource = null;

			this._pictureChooser.TakePicture(1, 100,
				(s) => {
					_imageSource = ImageSource.FromStream(() => s);
				},
				() => {

				});
		}
	}
}

