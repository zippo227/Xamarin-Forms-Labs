using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(PhoneMasterDetailRenderer), UIUserInterfaceIdiom.Phone)]
[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedTabletMasterDetailPageRenderer), UIUserInterfaceIdiom.Pad)]

namespace Xamarin.Forms.Labs.iOS
{
    public class ExtendedTabletMasterDetailPageRenderer : TabletMasterDetailRenderer
    {
        public static Func<bool> ShouldHideMenu;

        public ExtendedTabletMasterDetailPageRenderer()
        {
            this.Delegate = new ExtendedDelegate();
        }

        private class ExtendedDelegate : UISplitViewControllerDelegate
        {
            public UIBarButtonItem PresentButton { get; set; }

            public override void WillShowViewController(UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
            {
                this.PresentButton = (UIBarButtonItem) null;
            }

            public override void WillHideViewController(UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
            {
                this.PresentButton = barButtonItem;
            }

            public override bool ShouldHideViewController(UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
            {
                return true;
            }
        }
    }

    public class ExtendedPhoneMasterDetailPageRenderer : PhoneMasterDetailRenderer
    {
        public ExtendedPhoneMasterDetailPageRenderer()
        {
        }
    }
}

