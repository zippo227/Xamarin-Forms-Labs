//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.Droid.Services.Email.EmailService))]

namespace Xamarin.Forms.Labs.Droid.Services.Email
{
	using Android.Net;
	using Xamarin.Forms.Labs.Services.Email;

	public class EmailService : IEmailService
	{
		#region IEmailService Members

		// TODO: Check if there is a way to check this on Android
		public bool CanSend
		{
			get { return true; }
		}

		public void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc, IEnumerable<string> attachments)
		{
			var intent = new Intent(Intent.ActionSend);

			intent.SetType(html ? "text/html" : "text/plain");
			intent.PutExtra(Intent.ExtraEmail, to);
			intent.PutExtra(Intent.ExtraCc, cc);
			intent.PutExtra(Intent.ExtraBcc, bcc);
			intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

			if (html) {
				intent.PutExtra (Intent.ExtraText, Android.Text.Html.FromHtml (body));
			} else {
				intent.PutExtra (Intent.ExtraText, body ?? string.Empty);
			}

			intent.AddAttachments(attachments);

			this.StartActivity(intent);
		}

		public void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments)
		{
			var intent = new Intent(Intent.ActionSend);
			intent.SetType(html ? "text/html" : "text/plain");
			intent.PutExtra(Intent.ExtraEmail, to);
			intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

			if (html)
			{
				intent.PutExtra(Intent.ExtraText, body);
			}
			else
			{
				intent.PutExtra(Intent.ExtraText, body ?? string.Empty);
			}

			intent.AddAttachments(attachments);

			this.StartActivity(intent);
		}

		#endregion
	}
}