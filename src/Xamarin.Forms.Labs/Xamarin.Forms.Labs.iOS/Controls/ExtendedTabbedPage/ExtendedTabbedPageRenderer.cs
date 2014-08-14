using System.Diagnostics;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedTabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class ExtendedTabbedPageRenderer : TabbedRenderer
    {
        public ExtendedTabbedPageRenderer()
        {
            //TabBar.TintColor = MonoTouch.UIKit.UIColor.Black;
            // TabBar.BarTintColor = MonoTouch.UIKit.UIColor.Blue;
            // TabBar.BackgroundColor = MonoTouch.UIKit.UIColor.Green;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var page = (ExtendedTabbedPage)Element;

            // TODO: Need to figure out why this variable is null.
            //if (!page.SwipeEnabled)
            //{
            //    return;
            //}

            View.AddGestureRecognizer(new UISwipeGestureRecognizer(sw =>
            {
                // TODO: For demo only
                sw.ShouldReceiveTouch += (recognizer, touch) => (true);     //!(touch.View is UITableView) && !(touch.View is UITableViewCell));

                if (sw.Direction == UISwipeGestureRecognizerDirection.Left)
                {
                    page.InvokeSwipeLeftEvent(null, null);
                }

                if (sw.Direction == UISwipeGestureRecognizerDirection.Right)
                {
                    ((ExtendedTabbedPage)Element).InvokeSwipeRightEvent(null, null);
                }

                Debug.WriteLine("Swipe Tab.");
            }));
        }
    }
}
