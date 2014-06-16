using System;
using System.ComponentModel;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace XForms.Toolkit.Controls
{
    [Register("SensorBarView")]
    public class UISensorBar : UIView
    {
        private UIColor positiveColor = UIColor.Green;
        private UIColor negativeColor = UIColor.Red;
        private double limit = 1;
        private double currentValue = 0;
        
        public UISensorBar()
        {
            Initialize();
        }

        public UISensorBar(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        public UISensorBar(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        void Initialize()
        {
        }

        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);
            var half = this.Bounds.Size.Width / 2.0f;
            var height = this.Bounds.Size.Height;
            //var origin = new PointF(, this.Bounds.Size.Height / 2.0f);
            var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;

            var context = UIGraphics.GetCurrentContext();

            context.ClearRect (rect);

            context.SetFillColor(this.CurrentValue < 0 ? this.negativeColor.CGColor : this.positiveColor.CGColor);
            if (this.CurrentValue < 0)
            {
                var start = (float)percentage * half;
                var size = half - start;
                context.FillRect(new RectangleF(start, 0, size, height));
            }
            else
            {
                context.FillRect(new RectangleF(half, 0, (float)percentage * half, height));
            }
        }

        [Export, Browsable(true)]
        public double CurrentValue
        {
            get { return this.currentValue; }
            set 
            { 
                if (Math.Abs(value) <= this.Limit)
                {
                    this.currentValue = value;
                    this.SetNeedsDisplayInRect (this.Bounds);
                }
            }
        }

        [Export, Browsable(true)]
        public double Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }

        [Export, Browsable(true)]
        public UIColor PositiveColor
        {
            get { return this.positiveColor; }
            set { this.positiveColor = value; }
        }

        [Export, Browsable(true)]
        public UIColor NegativeColor
        {
            get { return this.negativeColor; }
            set { this.negativeColor = value; }
        }
    }
}