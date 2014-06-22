// ***********************************************************************
// Assembly         : XFormsToolkitSampleiOS
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-17-2014
// ***********************************************************************
// <copyright file="AppDelegate.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS;
using Xamarin.Forms.Labs.iOS.Controls.Calendar;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Serialization;
using Xamarin.Forms.Labs.Caching.SQLiteNet;
using SQLite.Net.Platform.XamarinIOS;
using System;
using System.IO;

namespace Xamarin.Forms.Labs.Sample.iOS
{
	/// <summary>
	/// Class AppDelegate.
	/// </summary>
	/// <remarks>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the
    /// User Interface of the application, as well as listening (and optionally responding) to
    /// application events from iOS.
	/// </remarks>
	[Register ("AppDelegate")]
	public partial class AppDelegate : XFormsApplicationDelegate
	{
		/// <summary>
		/// The window
		/// </summary>
		private UIWindow _window;

		/// <summary>
		/// Finished the launching.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		/// <remarks>
        /// This method is invoked when the application has loaded and is ready to run. In this
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        ///
        /// You have 17 seconds to return from this method, or iOS will terminate your application.
		/// </remarks>
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            this.SetIoc();

			new CalendarViewRenderer(); //added so the assembly is included

			Forms.Init();

			App.Init();

            this._window = new UIWindow(UIScreen.MainScreen.Bounds) { RootViewController = App.GetMainPage().CreateViewController() };

            this._window.MakeKeyAndVisible();

			base.FinishedLaunching(app, options);

			return true;
		}

		/// <summary>
		/// Sets the IoC.
		/// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

			var app = new XFormsAppiOS();
			app.Init(this);

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var pathToDatabase = "xforms.db";

			resolverContainer.Register<IDevice> (t => AppleDevice.CurrentDevice)
                .Register<IDisplay> (t => t.Resolve<IDevice> ().Display)
				.Register<IJsonSerializer, Services.Serialization.ServiceStackV3.JsonSerializer> ()
				.Register<IXFormsApp> (app)
				.Register<ISimpleCache> (t => new SQLiteSimpleCache(new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS(),
					new SQLite.Net.SQLiteConnectionString(pathToDatabase,true), t.Resolve<IJsonSerializer> () ));
				

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
	}
}

