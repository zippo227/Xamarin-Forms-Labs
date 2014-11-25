using System.Diagnostics;
using System.Linq;
#if __UNIFIED__
using UIKit;
#elif __IOS__
using MonoTouch.UIKit;
#endif
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
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var page = (ExtendedTabbedPage)Element;

            TabBar.TintColor = page.TintColor.ToUIColor();
            TabBar.BarTintColor = page.BarTintColor.ToUIColor();
            TabBar.BackgroundColor = page.BackgroundColor.ToUIColor();
            

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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var page = (ExtendedTabbedPage)Element;

            if (!string.IsNullOrEmpty(page.TabBarSelectedImage))
            {
                TabBar.SelectionIndicatorImage = UIImage.FromFile(page.TabBarSelectedImage).CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0), UIImageResizingMode.Stretch);
            }

            if (!string.IsNullOrEmpty(page.TabBarBackgroundImage))
            {
                TabBar.BackgroundImage = UIImage.FromFile(page.TabBarBackgroundImage).CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0), UIImageResizingMode.Stretch);
            }

            if (page.Badges != null && page.Badges.Count != 0)
            {
                var items = TabBar.Items;

                for (var i = 0; i < page.Badges.Count; i++)
                {
                    if (i >= items.Count())
                    {
                        continue;
                    }

                    items[i].BadgeValue = page.Badges[i];
                }
            }
        }
    }
}
