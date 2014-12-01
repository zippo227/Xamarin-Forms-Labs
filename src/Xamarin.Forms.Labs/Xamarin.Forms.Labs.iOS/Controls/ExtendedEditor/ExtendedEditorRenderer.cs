using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs.Controls;
using MonoTouch.UIKit;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class ExtendedEditorRenderer : EditorRenderer
	{
        private UISwipeGestureRecognizer leftSwipeGestureRecognizer;
        private UISwipeGestureRecognizer rightSwipeGestureRecognizer;

		public ExtendedEditorRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged (e);

			var view = (ExtendedEditor)Element;
            Control.Font = view.Font.ToUIFont ();

            if (e.OldElement == null)
            {
                this.leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => view.OnLeftSwipe(this, EventArgs.Empty))
                    {
                        Direction = UISwipeGestureRecognizerDirection.Left
                    };

                this.rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(()=> view.OnRightSwipe(this, EventArgs.Empty))
                    {
                        Direction = UISwipeGestureRecognizerDirection.Right
                    };

                this.Control.AddGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.AddGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            if (e.NewElement == null)
            {
                this.Control.RemoveGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.RemoveGestureRecognizer(this.rightSwipeGestureRecognizer);
            }
		}
	}
}

