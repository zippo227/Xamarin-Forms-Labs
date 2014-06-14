using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using XForms.Toolkit.Services.Camera;

namespace XForms.Toolkit.WP.Services.Camera
{
	/// <summary>
	/// Class PictureChooser.
	/// </summary>
	public class PictureChooser : IPictureChooser
	{
		/// <summary>
		/// Chooses the picture from library.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension.</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="actionSuccess">Action to execute when picture is selected.</param>
		/// <param name="actionCancel">Action to execute of the process is aborted/canceled.</param>
		public void SelectFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> actionSuccess, Action actionCancel)
		{
			// No point in setting the PixelHeight = maxPixelDimension and PixelWidth = maxPixelDimension here - as that would result in square cropping
			var photoChooser = new PhotoChooserTask {ShowCamera = true};

			ChoosePictureCommon(photoChooser, maxPixelDimension, percentQuality, actionSuccess, actionCancel);
		}

		/// <summary>
		/// Takes the picture.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="actionSuccess">Action to execute when picture is selected.</param>
		/// <param name="actionCancel">Action to execute of the process is aborted/canceled.</param>
		public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> actionSuccess, Action actionCancel)
		{
			var photoChooser = new CameraCaptureTask();

			ChoosePictureCommon(photoChooser, maxPixelDimension, percentQuality, actionSuccess, actionCancel);
		}

		/// <summary>
		/// Chooses the picture common.
		/// </summary>
		/// <param name="chooser">The chooser.</param>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="pictureAvailable">Action to execute when picture is selected.</param>
		/// <param name="assumeCanceled">Action to execute of the process is aborted/canceled.</param>
		public void ChoosePictureCommon(ChooserBase<PhotoResult> chooser, int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCanceled)
		{
			chooser.Completed += (sender, args) =>
			{
				// Do we have a selected image?
				if (args.ChosenPhoto != null)
				{
					ResizeThenCallOnMainThread(maxPixelDimension,
						percentQuality,
						args.ChosenPhoto,
						pictureAvailable);
				}
				else
				{
					assumeCanceled();
				}
			};

			SafeInvoke(chooser.Show);
		}

		/// <summary>
		/// Resizes the then call on main thread.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="input">The stream that contains the image.</param>
		/// <param name="success">The action to execute on actionSuccess of resizing the image.</param>
		private static void ResizeThenCallOnMainThread(int maxPixelDimension, int percentQuality, Stream input,
			Action<Stream> success)
		{
			ResizeImageStream(maxPixelDimension, percentQuality, input, stream => SafeAsyncCall(stream, success));
		}

		/// <summary>
		/// Resizes the JPEG stream.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="input">The stream that contains the image.</param>
		/// <param name="success">The action to execute on actionSuccess of resizing the image.</param>
		private static void ResizeImageStream(int maxPixelDimension, int percentQuality, Stream input, Action<Stream> success)
		{
			int targetHeight;
			int targetWidth;

			var bitmap = new BitmapImage();
			bitmap.SetSource(input);
			var writeable = new WriteableBitmap(bitmap);

			ResizeBasedOnPixelDimension(maxPixelDimension, writeable.PixelWidth, writeable.PixelHeight, out targetWidth, out targetHeight);

			// Note: We are NOT using a "using" statementhere on purpose. It is the callers responsibility to handle the dispose of the stream correctly
			var memoryStream = new MemoryStream();
			writeable.SaveJpeg(memoryStream, targetWidth, targetHeight, 0, percentQuality);
			memoryStream.Seek(0L, SeekOrigin.Begin);

			// Execute the call back with the valid stream
			success(memoryStream);
		}

		/// <summary>
		/// Calls the asynchronous.
		/// </summary>
		/// <param name="input">The stream that contains the image.</param>
		/// <param name="success">The action to execute on actionSuccess of resizing the image.</param>
		private static void SafeAsyncCall(Stream input, Action<Stream> success)
		{
			if (Deployment.Current.Dispatcher.CheckAccess())
			{
				try
				{
					success(input);
				}
				catch
				{
				}
			}
			else
			{
				Deployment.Current.Dispatcher.BeginInvoke(() => success(input));
			}
		}

		/// <summary>
		/// Does the with invalid operation protection.
		/// </summary>
		/// <param name="action">The action.</param>
		protected void SafeInvoke(Action action)
		{			
			if (Deployment.Current.Dispatcher.CheckAccess())
			{
				try
				{
					action();
				}
				catch
				{
				}
			}
			else
			{
				Deployment.Current.Dispatcher.BeginInvoke(action);
			}
		}

		/// <summary>
		/// Calcualtes the target height and width of the image based on the max pixel dimension.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="currentWidth">Current Width of the image.</param>
		/// <param name="currentHeight">Current Height of the image.</param>
		/// <param name="targetWidth">Target Width of the image.</param>
		/// <param name="targetHeight">Target Height of the image.</param>
		public static void ResizeBasedOnPixelDimension(int maxPixelDimension, int currentWidth, int currentHeight, out int targetWidth, out int targetHeight)
		{
			double ratio;
			if (currentWidth > currentHeight)
				ratio = (maxPixelDimension) / ((double)currentWidth);
			else
				ratio = (maxPixelDimension) / ((double)currentHeight);

			targetWidth = (int)Math.Round(ratio * currentWidth);
			targetHeight = (int)Math.Round(ratio * currentHeight);
		}
	}
}
