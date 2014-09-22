
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xamarin.Forms.Labs.Droid
{
    public static class IntentExtensions
    {
        public static void AddAttachments(this Intent intent, IEnumerable<string> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                Android.Util.Log.Warn("Intent.AddAttachments", "No attachments to attach.");
                return;
            }

            intent.PutParcelableArrayListExtra(
                Intent.ExtraStream,
                attachments.Select(a => Uri.FromFile(new Java.IO.File(a)) as IParcelable).ToList());
        }
    }
}