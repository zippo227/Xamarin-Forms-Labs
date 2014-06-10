Xamarin-Forms-Toolkit
=====================

Xamarin Forms Toolkit is a open source project that aims to provide a powerfull and cross platform set of controls tailored to work with Xamarin Forms.

Call for action for all Xamarin Developers, embrace this project and share your controls and services with the community, add your own control to the toolkit.

**Available controls **

 - Calendar Control (beta)  
 - ExtendedTabbedPage  
 - ImageButton (beta)
 - ExtendedLabel (alpha)
 - AutoComplete (alpha)

**Available services (Beta)**

 - TextToSpeech 
 - Device

**Available Mvvm helpers (Beta)**

 - ViewModelBase 
 - RelayCommand ; RelayCommand< T >

_______________

Nuget
--------------

At the time there's no public nuget packages , the solution includes a build action to create nuget packages.


_________________

Build the project
--------------

To develop on this project, run the build.bat or build.sh file first so the required Xamarin nuget packages can be downloaded.


_________________

Screenshots
-----------
Coming soon..

__________________

_________________

Using the controls
-----------
Xaml :

Reference the assembly namespace 

     xmlns:controls="clr-namespace:XForms.Toolkit.Controls;assembly=XForms.Toolkit"

Render your control:

     <controls:ImageButton Text="Twitter" BackgroundColor="#01abdf" TextColor="#ffffff" HeightRequest="75" WidthRequest="175" Image="icon_twitter" Orientation="ImageToLeft"  ImageHeightRequest="50" ImageWidthRequest="50" />
      
Or from your codebehind:


	var button = new ImageButton() {
				ImageHeightRequest = 50,
				ImageWidthRequest = 50,
				Orientation= Orientation.ImageToLeft",
				Image = "icon_twitter"
			};
	stacker.Children.Add (button);
	
_________________

Contribute
-----------

Everbody is welcome to contribute with any kind of controls or features at this time. Since there's no oficial releases feel free to submit your playground controls even if they aren't perfect. 
__________________

License
-----------

License MIT more about that in the [LICENSE][1] file. 
__________________

**Contributions:**
Michael Ridland @rid00z
Rui Marinho @ruiespinho
Filip De Vos @foxtricks
ThomasLebrun
Sami M. Kallio
Kevin E. Ford @Bowman74


  [1]: https://github.com/XForms/XForms-Toolkit/blob/master/LICENSE
