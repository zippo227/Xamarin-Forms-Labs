using Xamarin.Forms;

[assembly:ExportRenderer(typeof(Xamarin.Forms.Labs.Controls.GesturesContentView),typeof(Xamarin.Forms.Labs.Droid.Controls.GesturesContentView.GesturesContentViewRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls.GesturesContentView
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Android.Graphics;
    using Android.Views;

    using Xamarin.Forms.Labs.Behaviors;
    using Xamarin.Forms.Labs.Controls;
    using Xamarin.Forms.Platform.Android;

    using Point = Xamarin.Forms.Point;
    using View = Xamarin.Forms.View;

    /// <summary>
    /// Android renderer for the GestureContentView
    /// This class detects gestures and sends them
    /// to the GestureContentView for processing
    /// It implments:
    /// <see cref="GestureDetector.IOnGestureListener"/>
    /// <see cref="GestureDetector.IOnDoubleTapListener"/>
    /// </summary>
    public class GesturesContentViewRenderer  : ViewRenderer<GesturesContentView,Android.Views.View>  , GestureDetector.IOnGestureListener ,GestureDetector.IOnDoubleTapListener 
    {
        /// <summary>
        /// Standard Android detector
        /// </summary>
        private readonly GestureDetector _detector;

        /// <summary>
        /// Initialize the detector with a listener(this)
        /// </summary>
        public GesturesContentViewRenderer()
        {
            _detector=new GestureDetector(this);   
        }

        
        /// <summary>
        /// Follow the xamarin rules for element changing.
        /// </summary>
        /// <param name="e">Change event parameter</param>
        protected override void OnElementChanged(ElementChangedEventArgs<GesturesContentView> e)
        {
            
            base.OnElementChanged(e);
            if (e.NewElement == null)
            {
                GenericMotion -= HandleGenericMotion;
                Touch -= HandleTouch;
            }
            if (e.OldElement != null)return;
            Element.GestureRecognizers.Clear();            
            GenericMotion += HandleGenericMotion;
            Touch += HandleTouch;
        }
        
        /// <summary>
        /// Forward touch events to the detector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTouch(object sender, TouchEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        /// <summary>
        /// Forward motion events to the detector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        private List<View> ViewsContaing(Point pt)
        {
            var ret = new List<View>();
            return ViewsContainingImpl(pt,ret,ViewGroup).OrderBy(x=>x.Bounds.Width*x.Bounds.Height).ToList();
        }

        private IEnumerable<View> ViewsContainingImpl(Point pt,List<View>views,ViewGroup root )
        {
            for (var i = 0; i <root.ChildCount; i++)
            {
                var child = root.GetChildAt(i);
                var bounds = new Rect();
                child.GetHitRect(bounds);
                var ver = child as IVisualElementRenderer;
                if (ver == null || !(ver.Element is View) || !bounds.Contains(Convert.ToInt32(pt.X), Convert.ToInt32(pt.Y)))
                    continue;
                views.Add(ver.Element as View);
                //check to see if this containing child has any other containing children
                ViewsContainingImpl(pt, views,ver.ViewGroup);
            }
            return views;
        }
        #region Gesture Events
        /// <summary>
        /// Do noting
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Return false to indicate the the event has not been handled</returns>
        public bool OnDown(MotionEvent e)
        {
            return true;
        }

        /// <summary>
        /// Pass the Fling(aka swipe) to the <see cref="GesturesContentView"/> for processing
        /// 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="velocityX"></param>
        /// <param name="velocityY"></param>
        /// <returns></returns>
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {

            var e1X = ConvertPixelsToDp(e1.GetX());
            var e2X = ConvertPixelsToDp(e2.GetX());
            var e1Y=ConvertPixelsToDp(e1.GetY());
            var e2Y=ConvertPixelsToDp(e2.GetY());

            var distance =Math.Abs(Math.Sqrt(Math.Pow(e1X - e2X, 2) + Math.Pow(e1Y - e2Y, 2)));
            if (distance < Element.MinimumSwipeLength) return true;
            
            Element.ProcessGesture(new GestureResult
                                {
                                        GestureType=GestureType.Swipe,
                                        Direction=(Math.Abs(e1X - e2X) < 3 ? Directionality.None : e1X < e2X ? Directionality.Right : Directionality.Left)
                                                | (Math.Abs(e1Y-e2Y)<3 ? Directionality.None: e1Y < e2Y ? Directionality.Down : Directionality.Up),
                                        Origin= new Point(e1X,e1Y),
                                        VerticalDistance = Math.Abs(e1Y - e2Y),
                                        HorizontalDistance = Math.Abs(e1X - e2X),
                                        Length=distance,
                                        ViewStack = Element.ExcludeChildren ? ViewsContaing(new Point(e1.GetX(), e1.GetY())) : null
                                    });
            return false;
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        /// <summary>
        /// Pass the longpress to the <see cref="GesturesContentView"/>
        /// </summary>
        /// <param name="e"></param>
        public void OnLongPress(MotionEvent e)
        {
            Element.ProcessGesture(new GestureResult
                                       {
                                           ViewStack = Element.ExcludeChildren ? ViewsContaing(new Point(e.GetX(), e.GetY())) : null, 
                                           GestureType = GestureType.LongPress, 
                                           Direction = Directionality.None, 
                                           Origin = new Point(ConvertPixelsToDp(e.GetX()), ConvertPixelsToDp(e.GetY()))
                                       });
        }

        
        /// <summary>
        /// Ignored
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="distanceX"></param>
        /// <param name="distanceY"></param>
        /// <returns></returns>
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY){return true;}
         /// <summary>
         /// Ignored
         /// </summary>
         /// <param name="e"></param>
        public void OnShowPress(MotionEvent e){}

        /// <summary>
        /// Ignored
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnSingleTapUp(MotionEvent e){return true;}

        /// <summary>
        /// Ignored
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnDoubleTap(MotionEvent e)
        {
            return !Element.ProcessGesture(new GestureResult
                                               {
                                                   ViewStack = Element.ExcludeChildren ? ViewsContaing(new Point(e.GetX(), e.GetY())) : null, 
                                                   GestureType = GestureType.DoubleTap, 
                                                   Direction = Directionality.None, 
                                                   Origin = new Point(ConvertPixelsToDp(e.GetX()), ConvertPixelsToDp(e.GetY()))
                                               });
        }

        /// <summary>
        /// Send double tap to the <see cref="GesturesContentView"/> for processing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnDoubleTapEvent(MotionEvent e){return true;}

        /// <summary>
        /// Send the Single tap to the <see cref="GesturesContentView"/> for processing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnSingleTapConfirmed(MotionEvent e)
        {
                return !Element.ProcessGesture(new GestureResult
                                                   {
                                                       ViewStack = Element.ExcludeChildren ? ViewsContaing(new Point(e.GetX(), e.GetY())) : null,
                                                       GestureType = GestureType.SingleTap,
                                                       Direction = Directionality.None,
                                                       Origin = new Point(ConvertPixelsToDp(e.GetX()), ConvertPixelsToDp(e.GetY()))
                                                   });
        }
        #endregion

    }

}