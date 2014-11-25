#if __UNIFIED__
using Foundation;
using UIKit;
using CoreGraphics;
#elif __IOS__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif
using System.ComponentModel;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridViewCell : UICollectionViewCell
    {
        public const string Key = "GridViewCell";

        private ViewCell viewCell;
        private UIView view;

        public ViewCell ViewCell {
            get {
                return this.viewCell;
            }
            set {
                if (this.viewCell == value) {
                    return;
                }
                this.UpdateCell (value);
            }
        }

        [Export ("initWithFrame:")]
        public GridViewCell (System.Drawing.RectangleF frame) : base (frame)
        {
            // SelectedBackgroundView = new GridItemSelectedViewOverlay (frame);
            // this.BringSubviewToFront (SelectedBackgroundView);

        }

        private void UpdateCell (ViewCell cell)
        {
            if (this.viewCell != null) {
                //this.viewCell.SendDisappearing ();
                this.viewCell.PropertyChanged -= new PropertyChangedEventHandler (this.HandlePropertyChanged);
            }
            this.viewCell = cell;
            this.viewCell.PropertyChanged += new PropertyChangedEventHandler (this.HandlePropertyChanged);
            //this.viewCell.SendAppearing ();
            this.UpdateView ();
        }

        private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            this.UpdateView ();
        }

        private void UpdateView ()
        {

            if (this.view != null) {
                this.view.RemoveFromSuperview ();
            }
            this.view = RendererFactory.GetRenderer (this.viewCell.View).NativeView;
            this.view.AutoresizingMask = UIViewAutoresizing.All;
            this.view.ContentMode = UIViewContentMode.ScaleToFill;

            this.AddSubview (this.view);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            var frame = this.ContentView.Frame;
            frame.X = (this.Bounds.Width - frame.Width) / 2;
            frame.Y = (this.Bounds.Height - frame.Height) / 2;
            this.ViewCell.View.Layout(frame.ToRectangle());
            this.view.Frame = frame;
        }
    }

    //SelectedView Overlay Windows8 style
    public class GridItemSelectedViewOverlay : UIView
    {

        public GridItemSelectedViewOverlay (RectangleF frame) : base (frame)
        {
            BackgroundColor = UIColor.Clear;
        }

#if __UNIFIED__
        public override void Draw(CGRect rect)
#elif __IOS__
        public override void Draw (RectangleF rect)
#endif
        {
            using (var g = UIGraphics.GetCurrentContext ()) {
                g.SetLineWidth (10);
                UIColor.FromRGB (64, 30, 168).SetStroke ();
                UIColor.Clear.SetFill ();

                //create geometry
                var path = new CGPath ();
                path.AddRect (rect);
                path.CloseSubpath ();

                //add geometry to graphics context and draw it
                g.AddPath (path);
                g.DrawPath (CGPathDrawingMode.Stroke);
            }
        }
    }
}

