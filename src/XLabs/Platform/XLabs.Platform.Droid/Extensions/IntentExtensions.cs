namespace XLabs.Platform.Droid.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	using Android.Content;
	using Android.Net;

	public static class IntentExtensions
    {
        public static void AddAttachments(this Intent intent, IEnumerable<string> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                Android.Util.Log.Warn("Intent.AddAttachments", "No attachments to attach.");
                return;
            }

			foreach (var attachment in attachments) {
				var file = new Java.IO.File(attachment);
				// File existence check
				if (file.Exists()) {
					intent.PutExtra(Intent.ExtraStream, Uri.FromFile(file));
				}
				else {
                    Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because it doesn't exist.", attachment);
				}
			}
        }
    }
}
