using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WApplication = System.Windows.Application;
using Windows.Storage;
using Microsoft.Phone.Shell;
using Xamarin.Forms.Labs.Mvvm;
using Microsoft.Phone.Controls;

namespace Xamarin.Forms.Labs.WP8
{
    /// <summary>
    /// The Xamarin Forms Labs Windows Phone Application.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class XFormsAppWP : XFormsApp<WApplication>
    {
        public XFormsAppWP() { }

        public XFormsAppWP(WApplication application) : base(application) { }

        public void RaiseBackPress()
        {
            this.OnBackPress();
        }

        public void SetOrientation(PageOrientation orientation)
        {
            this.Orientation = orientation.ToOrientation();
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="app">The native application.</param>
        protected override void OnInit(WApplication app)
        {
            this.AppContext.Startup += (o, e) => this.OnStartup();
            this.AppContext.Exit += (o, e) => this.OnClosing();
            this.AppContext.UnhandledException += (o, e) => this.OnError(e.ExceptionObject);
            this.AppDataDirectory = ApplicationData.Current.LocalFolder.Path;

            foreach (var svc in app.ApplicationLifetimeObjects.OfType<PhoneApplicationService>())
            {
                svc.Activated += (o, e) => this.OnResumed();
                svc.Deactivated += (o, e) => this.OnSuspended();
            }

            base.OnInit(app);
        }
    }
}