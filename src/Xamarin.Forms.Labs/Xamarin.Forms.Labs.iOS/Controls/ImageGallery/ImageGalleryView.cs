using System;
using MonoTouch.UIKit;
using System.Collections.ObjectModel;
using System.Drawing;
using MonoTouch.Foundation;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.iOS.Controls.ImageGallery
{
    public class ImageGalleryView : UIView
    {
        public bool FadeImages {
            get;
            set;
        }
        private ObservableCollection<string> _images; 
        public ObservableCollection<string> Images {
            get{ 
                return _images;
            }
            set{ 
                _images = value;
            }
        }

        UIPageControl pageControl;
        UIScrollView scroller;

        public ImageGalleryView (ObservableCollection<string> images ) : this(default(RectangleF),images)
        {
        }
        public ImageGalleryView (RectangleF frame, ObservableCollection<string> images = null) : base (frame)
        {
            this.AutoresizingMask = UIViewAutoresizing.All;
            this.ContentMode = UIViewContentMode.ScaleToFill;
            FadeImages = true;
            this.BackgroundColor = UIColor.White;
            if (frame == default(RectangleF))
                this.Frame = UIScreen.MainScreen.Bounds;
            else
                this.Frame = frame;

            if (images == null)
                Images = new ObservableCollection<string> ();
            else
                Images = images;

            pageControl = new UIPageControl ();
            pageControl.AutoresizingMask = UIViewAutoresizing.All;
            pageControl.ContentMode = UIViewContentMode.ScaleToFill;
            pageControl.ValueChanged += (object sender, EventArgs e) => UpdateScrollPositionBasedOnPageControl();

            scroller = new UIScrollView ();
            scroller.AutoresizingMask = UIViewAutoresizing.All;
            scroller.ShowsHorizontalScrollIndicator = scroller.ShowsVerticalScrollIndicator = false;
            scroller.ContentMode = UIViewContentMode.ScaleToFill;
            scroller.PagingEnabled = true;
            scroller.Bounces = false;


            this.Add (scroller);
            this.Add (pageControl);


        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            scroller.Scrolled -= ScrollChanged;
            Images.CollectionChanged -= HandleCollectionChanged;
            Images.Clear ();
        }
        public  override void Draw (System.Drawing.RectangleF rect)
        {
            Images.CollectionChanged+= HandleCollectionChanged;
            scroller.Scrolled += ScrollChanged;

            //TODO: need to remove observer if using the lambda?
            NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.DidChangeStatusBarOrientationNotification, not => {

                var orientation = UIDevice.CurrentDevice.Orientation;
                if ((UIDeviceOrientation.LandscapeLeft == orientation || UIDeviceOrientation.LandscapeRight == orientation))
                {
                    scroller.ContentSize = new System.Drawing.SizeF (Frame.Height * Images.Count-1, Frame.Width);

                }
                else{
                    scroller.ContentSize = new System.Drawing.SizeF (rect.Width * Images.Count-1, rect.Height);

                }
                UpdateScrollPositionBasedOnPageControl();
            });
            pageControl.Frame = new System.Drawing.RectangleF (rect.Left,rect.Height-40, rect.Width,40);
            scroller.Frame = new System.Drawing.RectangleF (rect.Left,rect.Top, rect.Width, rect.Height);
            var curr = 0;
            foreach (var im in  Images) {
                try {
                    AddImage (rect,curr,im);
                    curr++;
                } catch (Exception ex) {

                }

            }
            scroller.ContentSize = new System.Drawing.SizeF (scroller.Frame.Width * curr-1, scroller.Frame.Height);
            pageControl.Pages = curr;

            base.Draw (rect);
        }

        private void AddImage (RectangleF rect, int position, string im)
        {
            var img = new UIImage ();
            var isRemote = Helpers.IsValidUrl (im);
            if (isRemote)
                //dont await , fire and forget
                LoadImageAsync (position, im);
            else
                img = UIImage.FromFile (im);
            var imgView = new UIImageView (img);
            imgView.AutoresizingMask = UIViewAutoresizing.All;
            imgView.ContentMode = UIViewContentMode.ScaleToFill;
            if (FadeImages)
                imgView.Alpha = 0;
            //if first image is local, fade it in
            if (position == 0 && !isRemote)
                FadeImageViewIn (imgView);
            imgView.Frame = new System.Drawing.RectangleF (rect.Width * position, rect.Top, rect.Width, rect.Height);
            scroller.AddSubview (imgView);

        }


        private Task LoadImageAsync(int position, string url)
        {
            return Task.Run (() => {
                var img = Helpers.LoadFromUrl(url);

                InvokeOnMainThread( () => {

                    var imgView = scroller.Subviews[position] as UIImageView;
                    if(pageControl.CurrentPage == position && FadeImages)
                        FadeImageViewIn(imgView,img);
                    else
                        imgView.Image = img;
                });
            });

        }
        void ScrollChanged (object sender, EventArgs e)
        {
            var pageWidth = double.Parse(scroller.Bounds.Width.ToString());
            var oof = double.Parse(scroller.ContentOffset.X.ToString());
            int pageNumber = int.Parse(( Math.Floor((oof - pageWidth / 2) / pageWidth) + 1).ToString());
            var imgView = scroller.Subviews[pageNumber] as UIImageView;
            FadeImageViewIn (imgView);
            pageControl.CurrentPage = pageNumber;
        }

        void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (var newImage in e.NewItems) {
                    try {
                        this.BeginInvokeOnMainThread (() => {
                            AddImage (Frame, pageControl.Pages, newImage as string);
                            scroller.ContentSize = new System.Drawing.SizeF (Frame.Width * (pageControl.Pages + 1), scroller.Frame.Height);
                            pageControl.Pages = pageControl.Pages + 1;
                        });
                    } catch (Exception ex) {

                    }
                }
            }
        }

        private void SetImage (UIImageView imgView, UIImage img)
        {
            if (img != null) {
                imgView.Image = img;
            }
            imgView.Alpha = 1;
        }
        private void UpdateScrollPositionBasedOnPageControl()
        {
            var off = this.pageControl.CurrentPage  * this.scroller.Frame.Width;
            scroller.SetContentOffset(new PointF(off, 0), true);
        }
        private void FadeImageViewIn (UIImageView imgView, UIImage img = null)
        {

            if (FadeImages)
                UIView.Animate (0.3, 0, UIViewAnimationOptions.TransitionCrossDissolve, () => {
                    SetImage (imgView, img);
                }, () => {
                });
            else {
                SetImage (imgView, img);
            }
        }
    }

    public class Helpers
    {
        public static bool IsValidUrl(string urlString)
        {
            Uri uri;
            return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                    || uri.Scheme == Uri.UriSchemeHttps
                    || uri.Scheme == Uri.UriSchemeFtp
                    || uri.Scheme == Uri.UriSchemeMailto);
        }
        public static UIImage LoadFromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
    }
}

