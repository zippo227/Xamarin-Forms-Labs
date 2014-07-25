//http://forums.xamarin.com/discussion/19351/how-to-achieve-synchronized-scroll-views
//using and extending on msmith implementation

using System;

namespace Xamarin.Forms.Labs.Controls
{
    public class ExtendedScrollView : ScrollView
    {
        public event Action<ScrollView, Rectangle> Scrolled;

        public void UpdateBounds(Rectangle bounds)
        {
            Position = bounds.Location;
            if (Scrolled != null)
                Scrolled (this, bounds);
        }

        public static readonly BindableProperty PositionProperty = 
            BindableProperty.Create<ExtendedScrollView,Point>(
                p => p.Position, default(Point));

        public Point Position {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly BindableProperty AnimateScrollProperty = 
            BindableProperty.Create<ExtendedScrollView,bool>(
                p => p.AnimateScroll,true);

        public bool AnimateScroll {
            get { return (bool)GetValue (AnimateScrollProperty); }
            set { SetValue (AnimateScrollProperty, value); }
        }

    }
}

