using System;
using System.Threading.Tasks;
#if __UNIFIED__
using UIKit;
using Foundation;
#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.iOS.Services.Media
{
	public sealed class MediaPickerController
		: UIImagePickerController
	{
		internal MediaPickerController (MediaPickerDelegate mpDelegate)
		{
			base.Delegate = mpDelegate;
		}

		public override NSObject Delegate
		{
			get { return base.Delegate; }
			set { throw new NotSupportedException(); }
		}

		public Task<MediaFile> GetResultAsync()
		{
			return ((MediaPickerDelegate)Delegate).Task;
		}
	}
}

