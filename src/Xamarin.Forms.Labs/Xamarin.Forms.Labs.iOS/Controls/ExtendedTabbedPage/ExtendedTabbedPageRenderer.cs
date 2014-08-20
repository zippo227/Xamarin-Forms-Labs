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

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var page = (ExtendedTabbedPage)Element;

            if (!page.SwipeEnabled)
            {
                return;
            }

            var gesture1 = new UISwipeGestureRecognizer(sw =>
            {
                sw.ShouldReceiveTouch += (recognizer, touch) => !(touch.View is UITableView) && !(touch.View is UITableViewCell);

                if (sw.Direction == UISwipeGestureRecognizerDirection.Right)
                {
                    page.InvokeSwipeLeftEvent(null, null);
                }

            }) { Direction = UISwipeGestureRecognizerDirection.Right };

            var gesture2 = new UISwipeGestureRecognizer(sw =>
            {
                sw.ShouldReceiveTouch += (recognizer, touch) => !(touch.View is UITableView) && !(touch.View is UITableViewCell);

                if (sw.Direction == UISwipeGestureRecognizerDirection.Left)
                {
                    page.InvokeSwipeRightEvent(null, null);
                }

            }) { Direction = UISwipeGestureRecognizerDirection.Left };

            View.AddGestureRecognizer(gesture1);
            View.AddGestureRecognizer(gesture2);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}
