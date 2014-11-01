using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Services.Email
{
    public interface IEmailService
    {
        bool CanSend { get; }
        void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments);
        void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc, IEnumerable<string> attachments);
    }
}
