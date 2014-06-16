// ***********************************************************************
// Assembly         : XForms.Toolkit.Sample
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="CameraViewModel.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

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

		/// <summary>
		/// The _scheduler
		/// </summary>
		private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		//private CancellationTokenSource cancelSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraViewModel" /> class.
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
		/// <returns>Task.</returns>
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

