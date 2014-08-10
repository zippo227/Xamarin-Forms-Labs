Xamarin Forms Labs
=====================

Xamarin Forms Labs is a open source project that aims to provide a powerful and cross platform set of controls tailored to work with Xamarin Forms.

Call for action for all Xamarin Developers, embrace this project and share your controls and services with the community, add your own control to the toolkit.

**Available controls**

 - Calendar Control (beta)  
 - ExtendedTabbedPage  
 - ImageButton (beta)
 - ExtendedLabel (beta)
 - ExtendedViewCell (beta)
 - ExtendedTextCell (beta)
 - AutoComplete (beta)
 - HybridWebView (alpha)
 - Web Image (beta)
 - GridView (IOS beta)
 - RepeaterView (beta)
 - SegmentedControlView (IOS beta)
 - ExtendedScrollView (IOS beta)
 - DynamicListView (beta)
 - ExtendedContentView (beta)

**Available services (Beta)**

 - Text To Speech 
 - Device (battery info, device info, sensors, accelerometers)
 - Phone Service (cellular network info, make phonecalls)
 - Geolocator
 - Camera (Picture and Video picker, Take Picture, Take Video)
 - Accelerometer
 - Display
 - Cache
 - SoundService


**Available Mvvm helpers (Beta)**

 - ViewModel (navigation, isbusy)
 - ViewFactory
 - IOC
 - IXFormsApp (application events)

**Available Plugins (Beta)**
    

 - Serialization (ServiceStackV3, ProtoBuf, JSON.Net)
 - Caching (SQLLiteSimpleCache)
 - Dependency Injection containers (TinyIOC, Autofac, NInject, SimpleInjector, Unity)
 - Web (RestClient)
 
   
_________________



**HOW-TO**
======

We are working in a great [wiki][1] on how to use the controls and services. 


https://github.com/XForms/Xamarin-Forms-Labs/wiki


**Xamarin Forms Labs Framework Helper**
======

[Website - Xamarin Forms Labs Framework Helper for online use](http://htmlpreview.github.io/?https://github.com/XForms/Xamarin-Forms-Labs/blob/master/Help/Web/Index.html)

[Xamarin.Forms.Labs.chm file for offline use](https://github.com/XForms/Xamarin-Forms-Labs/blob/master/Help/Xamarin.Forms.Labs.chm)



------------------------------------------------------------------------

Using the MVVM Helpers
-----------

**ViewFactory**
Coming soon


_________________

Using the controls
-----------


Add Xamarin.Forms.Labs.Controls reference to your projects , main pcl, ios, android, and wp.

Xaml :

Reference the assembly namespace 

     xmlns:controls="clr-namespace:Xamarin.Forms.Labs.Controls;assembly=Xamarin.Forms.Labs"

Render your control:

     <controls:ImageButton Text="Twitter" BackgroundColor="#01abdf" TextColor="#ffffff" HeightRequest="75" WidthRequest="175" Image="icon_twitter" Orientation="ImageToLeft"  ImageHeightRequest="50" ImageWidthRequest="50" />
      
Or from your codebehind:


	var button = new ImageButton() {
				ImageHeightRequest = 50,
				ImageWidthRequest = 50,
				Orientation = Orientation.ImageToLeft,
				Source = "icon_twitter.png",
				Text = "Twitter"
			};
	stacker.Children.Add (button);
	
_________________

Using the Services
-----------
**TextToSpeechService** 

	DependencyService.Get<ITextToSpeechService>().Speak(TextToSpeak);
	
**Device** 

		var device = Resolver.Resolve<IDevice>();
		device.Display; //display information
		device.Battery; //battery information

	
**PhoneService** 

	 	var device = Resolver.Resolve<IDevice>();
		// not all devices have phone service, f.e. iPod and Android tablets
		// so we need to check if phone service is available
		if (device.PhoneService != null)
		{
			device.PhoneService.DialNumber("+1 (855) 926-2746");
		}

_______________

Nuget
--------------
**Main Packages:**
 - [Xamarin.Forms.Labs](https://www.nuget.org/packages/Xamarin.Forms.Labs/)

**Plugins:**

Caching 

 - [Xamarin.Forms.Labs.Caching.SQLiteNet](https://www.nuget.org/packages/Xamarin.Forms.Labs.Caching.SQLiteNet/)

DI 

 - [Xamarin.Forms.Labs.Services.SimpleContainer](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.SimpleContainer/)
 - [Xamarin.Forms.Labs.Services.Ninject](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.Ninject/)
 - [Xamarin.Forms.Labs.Services.Autofac](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.Autofac/)
 - [Xamarin.Forms.Labs.Services.TinyIOC](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.TinyIOC/)

Serialization

 - [Xamarin.Forms.Labs.Services.Serialization.ProtoBuf](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.Serialization.ProtoBuf/)
 - [Xamarin.Forms.Labs.Serialization.JsonNET](https://www.nuget.org/packages/Xamarin.Forms.Labs.Services.Serialization.JsonNET/)

Cryptography

 - [Xamarin.Forms.Labs.Cryptography](https://www.nuget.org/packages/Xamarin.Forms.Labs.Cryptography/)
_________________

Build the project
--------------

To develop on this project, just clone the project to your computer, package restore is enable so build the solution first, if you get any errors try to build each project independently .


_________________

Screenshots
-----------
Coming soon..

__________________

Contribute
-----------

Everbody is welcome to contribute with any kind of controls or features at this time. Since there's no oficial releases feel free to submit your playground controls even if they aren't perfect. 

Twitter hashtag : [#xflabs](https://twitter.com/search?q=xflabs)
__________________

License
-----------

License Apache 2.0 more about that in the [LICENSE][2] file. 
__________________

**Contributions:**
 - Michael Ridland [@rid00z ](https://twitter.com/rid00z)
 - [Rui Marinho](http://ruimarinho.net/)  [@ruiespinho](https://twitter.com/ruiespinho)
 - Filip De Vos  [@foxtricks](https://twitter.com/foxtricks)
 - ThomasLebrun 
 - Sami M. Kallio 
 - [Kevin E. Ford](http://windingroadway.blogspot.com/) [@Bowman74](https://twitter.com/Bowman74)
 - Jason Smith [@jassmith87](https://twitter.com/jassmith87)
 - Shawn Anderson
 - [Sara Silva](saramgsilva.com) [@saramgsilva](https://twitter.com/saramgsilva)
 - Ben Ishiyama-Levy [@mrbrl](http://www.monovo.io)
 - Ryan Wischkaemper
 - [Eric Grover](http://www.ericgrover.com) [@bluechiperic](https://twitter.com/bluechiperic)
 - [Mitch Milam] [@mitchmilam](https://twitter.com/mitchmilam)
 - Kazuki Yasufuku
 - Petr Klíma

 **Other Project Contributions:**
 	Xamarin.Mobile


  [1]: https://github.com/XForms/Xamarin-Forms-Labs/wiki
  [2]: https://github.com/XForms/XForms-Toolkit/blob/master/LICENSE
