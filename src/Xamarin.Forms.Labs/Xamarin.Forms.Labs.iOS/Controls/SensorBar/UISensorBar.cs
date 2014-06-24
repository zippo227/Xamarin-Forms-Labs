using System;
using System.ComponentModel;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The UI sensor bar view.
    /// </summary>
    [Register("SensorBarView")]
    public class UISensorBar : UIView
    {
        private UIColor positiveColor = UIColor.Green;
        private UIColor negativeColor = UIColor.Red;
        private double limit = 1;
        private double currentValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UISensorBar"/> class.
        /// </summary>
        public UISensorBar()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UISensorBar"/> class.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        public UISensorBar(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UISensorBar"/> class.
        /// </summary>
        /// <param name="handle">
        /// The handle.
        /// </param>
        public UISensorBar(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        [Export, Browsable(true)]
        public double CurrentValue
        {
            get
            {
                return this.currentValue;
            }

            set
            { 
                if (Math.Abs(value) <= this.Limit)
                {
                    this.currentValue = value;
                    this.SetNeedsDisplayInRect (this.Bounds);
                }
            }
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        [Export, Browsable(true)]
        public double Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }

        /// <summary>
        /// Gets or sets the positive color.
        /// </summary>
        [Export, Browsable(true)]
        public UIColor PositiveColor
        {
            get { return this.positiveColor; }
            set { this.positiveColor = value; }
        }

        /// <summary>
        /// Gets or sets the negative color.
        /// </summary>
        [Export, Browsable(true)]
        public UIColor NegativeColor
        {
            get { return this.negativeColor; }
            set { this.negativeColor = value; }
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="rect">
        /// The rectangle for draw.
        /// </param>
        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);
            var half = this.Bounds.Size.Width / 2.0f;
            var height = this.Bounds.Size.Height;
            var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;

            var context = UIGraphics.GetCurrentContext();

            context.ClearRect(rect);

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

        private static void Initialize()
        {
        }
    }
}