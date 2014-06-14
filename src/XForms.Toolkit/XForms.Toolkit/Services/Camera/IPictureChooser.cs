using System;
using System.IO;

namespace XForms.Toolkit.Services.Camera
{
	public interface IPictureChooser
	{
		/// <summary>
		/// Select a picture from library.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="actionSuccess">Action to execute when picture is selected.</param>
		/// <param name="actionCancel">Action to execute of the process is aborted/canceled.</param>
		void SelectFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> actionSuccess, Action actionCancel);

		/// <summary>
		/// Takes the picture.
		/// </summary>
		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
		/// <param name="percentQuality">The percent quality.</param>
		/// <param name="actionSuccess">Action to execute when picture is selected.</param>
		/// <param name="actionCancel">Action to execute of the process is aborted/canceled.</param>
		void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> actionSuccess, Action actionCancel);
	}
}