using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using Microsoft.Phone.Shell;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.WP
{
	public class XFormsAppWP : XFormsApp<Application>
	{	
		protected override void OnInit(Application app)
		{
            this.AppContext.Startup += (o, e) => { this.OnStartup(); };
            this.AppContext.Exit += (o, e) => { this.OnClosing(); };
            this.AppContext.UnhandledException += (o, e) => { this.OnError(e.ExceptionObject); };
            this.AppDataDirectory = ApplicationData.Current.LocalFolder.Path;
	
            foreach (var a in app.ApplicationLifetimeObjects)
            {
	            var svc = a as PhoneApplicationService;
	            if (svc != null)
	            {
		            svc.Activated += (o, e) => { this.OnResumed(); };
		            svc.Deactivated += (o, e) => { this.OnSuspended(); };
	            }
            }
			
            base.OnInit(app);
		}
	}
}
