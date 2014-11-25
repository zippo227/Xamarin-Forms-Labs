using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#if __UNIFIED__
using Foundation;
using MessageUI;
using UIKit;
#elif __IOS__
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Email;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.iOS.Services.Email.EmailService))]
namespace Xamarin.Forms.Labs.iOS.Services.Email
{
    public class EmailService : IEmailService
    {
        #region IEmailService Members
        public bool CanSend
        {
            get { return MFMailComposeViewController.CanSendMail; }
        }

        public void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc, IEnumerable<string> attachments)
        {
            var mailer = new MFMailComposeViewController();

            mailer.SetMessageBody(body ?? string.Empty, html);
            mailer.SetSubject(subject ?? string.Empty);
            mailer.SetCcRecipients(cc);
            mailer.SetToRecipients(to);
            mailer.Finished += (s, e) => ((MFMailComposeViewController)s).DismissViewController(true, () => { });

            foreach (var attachment in attachments)
            {
                mailer.AddAttachmentData(NSData.FromFile(attachment), GetMimeType(attachment), Path.GetFileName(attachment));
            }

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailer, true, null);
        }

        public void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments)
        {
            ShowDraft(subject, body, html, new[] { to }, new string[] { }, new string[] { }, attachments);
        }

        // TODO: make this more robust
        private string GetMimeType(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            var extension = Path.GetExtension(filename.ToLowerInvariant());

            switch (extension)
            {
                case "png":
                    return "image/png";
                case "doc":
                    return "application/msword";
                case "pdf":
                    return "application/pdf";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "zip":
                case "docx":
                case "xlsx":
                case "pptx":
                    return "application/zip";
                case "htm":
                case "html":
                    return "text/html";
            }

            return "application/octet-stream";
        }

        #endregion
    }
}