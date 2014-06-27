using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using Xamarin.Forms;
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

        public void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc)
        {
            var mailer = new MFMailComposeViewController();
            mailer.SetMessageBody(body ?? string.Empty, html);
            mailer.SetSubject(subject ?? string.Empty);
            mailer.SetCcRecipients(cc);
            mailer.SetToRecipients(to);
            mailer.Finished += (s, e) =>
            {
                ((MFMailComposeViewController)s).DismissViewController(true, () => { });
            };
        }

        public void ShowDraft(string subject, string body, bool html, string to)
        {
            ShowDraft(subject, body, html, new[] { to }, new string[] { }, new string[] { });
        }

        #endregion
    }
}