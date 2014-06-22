using System;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace XForms.Toolkit
{
    public class SensorBarDroidView : View
    {
        private Color positiveColor = Color.Green;
        private Color negativeColor = Color.Red;
        private double limit = 1;
        private double currentValue = 0;

        public SensorBarDroidView (Context context) :
        base (context)
        {
            Initialize ();
        }

        public SensorBarDroidView (Context context, IAttributeSet attrs) :
        base (context, attrs)
        {
            Initialize ();
        }

        public SensorBarDroidView (Context context, IAttributeSet attrs, int defStyle) :
        base (context, attrs, defStyle)
        {
            Initialize ();
        }

        public double CurrentValue
        {
            get { return this.currentValue; }
            set 
            { 
                if (Math.Abs(value) <= this.Limit)
                {
                    this.currentValue = value;
                }
            }
        }

        public double Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }
            
        public Color PositiveColor
        {
            get { return this.positiveColor; }
            set { this.positiveColor = value; }
        }

        public Color NegativeColor
        {
            get { return this.negativeColor; }
            set { this.negativeColor = value; }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw (canvas);

            var r = new Rect ();
            this.GetLocalVisibleRect (r);

            var half = r.Width() / 2;
            var height = r.Height();

            var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;


            var paint = new Paint()
            {
                Color = this.CurrentValue < 0 ? this.negativeColor : this.positiveColor,
                StrokeWidth = 5
            };

            paint.SetStyle(Paint.Style.Fill);

            if (this.CurrentValue < 0)
            {
                var start = (float)percentage * half;
                var size = half - start;
                canvas.DrawRect (new Rect ((int)start, 0, (int)(start + size), height), paint);
            }
            else
            {
                canvas.DrawRect (new Rect((int)half, 0, (int)(half + percentage * half), height), paint);
            }
        }

        private void Initialize()
        {

        }
    }
}

