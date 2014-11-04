using Xamarin.Forms;

[assembly:ExportRenderer(typeof(Xamarin.Forms.Labs.Controls.GesturesContentView),typeof(Xamarin.Forms.Labs.Droid.Controls.GesturesContentView.GestureContentViewRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls.GesturesContentView
{
    using Android.Views;
    using Xamarin.Forms.Labs.Controls;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// Android renderer for the GestureContentView
    /// This class detects gestures and sends them
    /// to the GestureContentView for processing
    /// It implments:
    /// <see cref="GestureDetector.IOnGestureListener"/>
    /// <see cref="GestureDetector.IOnDoubleTapListener"/>
    /// </summary>
    public class GestureContentViewRenderer  : ViewRenderer<GesturesContentView,View>  , GestureDetector.IOnGestureListener ,GestureDetector.IOnDoubleTapListener
    {
        /// <summary>
        /// Standard Android detector
        /// </summary>
        private readonly GestureDetector detector;

        /// <summary>
        /// Initialize the detector with a listener(this)
        /// </summary>
        public GestureContentViewRenderer()
        {
            detector=new GestureDetector(this);    
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
            detector.OnTouchEvent(e.Event);
        }

        /// <summary>
        /// Forward motion events to the detector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            detector.OnTouchEvent(e.Event);
        }


        #region Gesture Events
        /// <summary>
        /// Do noting
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Return false to indicate the the event has not been handled</returns>
        public bool OnDown(MotionEvent e)
        {
            return false;
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
            return Element.VectorGesture(RawGestures.Swipe, new VectorGestureLoci
            {
                RelativeX = e1.GetX(),
                RelativeY = e1.GetY(),
                RelativeX2 = e2.GetX(),
                RelativeY2 = e2.GetY()
            });
        }

        /// <summary>
        /// Pass the longpress to the <see cref="GesturesContentView"/>
        /// </summary>
        /// <param name="e"></param>
        public void OnLongPress(MotionEvent e)
        {
            Element.NonVectorGesture(RawGestures.LongPress, new NonVectorGestureLoci { RelativeX = e.GetX(),RelativeY = e.GetY() });
        }

        /// <summary>
        /// Ignored
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="distanceX"></param>
        /// <param name="distanceY"></param>
        /// <returns></returns>
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY){return false;}
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
        public bool OnSingleTapUp(MotionEvent e){return false;}

        /// <summary>
        /// Ignored
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnDoubleTap(MotionEvent e){return false;}

        /// <summary>
        /// Send double tap to the <see cref="GesturesContentView"/> for processing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnDoubleTapEvent(MotionEvent e)
        {
            return Element.NonVectorGesture(RawGestures.LongPress, new NonVectorGestureLoci { RelativeX = e.GetX(), RelativeY = e.GetY() });
        }

        /// <summary>
        /// Send the Single tap to the <see cref="GesturesContentView"/> for processing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnSingleTapConfirmed(MotionEvent e)
        {
            return Element.NonVectorGesture(RawGestures.LongPress, new NonVectorGestureLoci { RelativeX = e.GetX(), RelativeY = e.GetY() });
        }
        #endregion
    }
}