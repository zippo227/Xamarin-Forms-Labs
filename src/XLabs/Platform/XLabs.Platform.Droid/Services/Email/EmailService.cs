//using System;

using XLabs.Platform.Droid.Services.Email;

[assembly: Dependency(typeof(EmailService))]

namespace XLabs.Platform.Droid.Services.Email
{
	using System.Collections.Generic;

	using Android.Content;

	using XLabs.Platform.Droid.Extensions;
	using XLabs.Platform.Services.Email;

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

            if (html)
            {
                intent.PutExtra(Intent.ExtraText, Android.Text.Html.FromHtml(body));
            }
            else
            {
                intent.PutExtra(Intent.ExtraText, body ?? string.Empty);
            }

            intent.AddAttachments(attachments);

            this.StartActivity(intent);
        }

        public void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType(html ? "text/html" : "text/plain");
            intent.PutExtra(Intent.ExtraEmail, new string[]{ to });
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
