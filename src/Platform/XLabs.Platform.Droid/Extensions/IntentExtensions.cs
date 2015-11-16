namespace XLabs.Platform
{
	using System.Collections.Generic;
	using System.Linq;

	using Android.Content;
	using Android.Net;
  using Android.OS;

	/// <summary>
	/// Class IntentExtensions.
	/// </summary>
	public static class IntentExtensions
  {
		/// <summary>
		/// Adds the attachments.
		/// </summary>
		/// <param name="intent">The intent.</param>
		/// <param name="attachments">The attachments.</param>
    public static void AddAttachments(this Intent intent, IEnumerable<string> attachments)
    {
      if (attachments == null || !attachments.Any())
      {
        Android.Util.Log.Warn("Intent.AddAttachments", "No attachments to attach.");
        return;
      }

      IList<IParcelable> uris = new List<IParcelable>();
      foreach (var attachment in attachments)
      {
        //convert from paths to Android friendly Parcelable Uri's
        var file = new Java.IO.File(attachment);
        if (file.Exists()) 
        {
          Uri u = Uri.FromFile(file);
          uris.Add(u);
        } else {
            Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because it doesn't exist.", attachment);
        }
      }

      intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
    }
  }
}
