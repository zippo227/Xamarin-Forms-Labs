using System;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.ComponentModel;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.iOS
{
	public class UISeparator : MonoTouch.UIKit.UIView
	{


		private double _thickness;

		public double Thickness{
			set{
				_thickness = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _thickness;
			}
		}

		private double _spacingBefore;
	
		public double SpacingBefore{
			set{
				_spacingBefore = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _spacingBefore;
			}
		}

		private double _spacingAfter;
	
		public double SpacingAfter{
			set{
				_spacingAfter = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _spacingAfter;
			}
		}

		private UIColor _strokeColor;

		public UIColor StrokeColor{
			set{
				_strokeColor = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _strokeColor;
			}
		}


		private StrokeType _strokeType;

		public StrokeType StrokeType{
			set{
				_strokeType = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _strokeType;
			}
		}

		private Xamarin.Forms.Labs.Controls.SeparatorOrientation _orientation;

		public SeparatorOrientation Orientation{
			set{
				_orientation = value;
				this.SetNeedsDisplayInRect (this.Bounds);
			}
			get{
				return _orientation;
			}
		}

		void Initialize()
		{
			this.BackgroundColor = UIColor.Clear;
			this.Opaque = false;
		}

		public UISeparator(System.Drawing.RectangleF bounds) : base(bounds)
		{
			Initialize();
		}

		public UISeparator(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		public UISeparator()
		{
			Initialize();
		}

		public override void Draw(System.Drawing.RectangleF rect)
		{
			base.Draw(rect);


			var height = this.Bounds.Size.Height;
			//var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;

			var context = UIGraphics.GetCurrentContext();

			context.ClearRect(rect);
			//context.SetFillColor(UIColor.Clear.CGColor);
			//context.FillRect(rect);
			context.SetStrokeColor(this.StrokeColor.CGColor);
			switch(StrokeType) {
			case StrokeType.Dashed:
				context.SetLineDash(0, new float[]{ 6, 2 });
				break;
			case StrokeType.Dotted:
				context.SetLineDash(0, new float[]{ (float)this.Thickness, (float)this.Thickness});
				break;
			default:

				break;
			}
			context.SetLineWidth((float)this.Thickness);
			var desiredTotalSpacing = SpacingAfter + SpacingBefore;
			float leftForSpacing = 0;
			float actualSpacingBefore = 0;
			float actualSpacingAfter = 0;
			if(Orientation == SeparatorOrientation.Horizontal){
				leftForSpacing = (float)this.Bounds.Size.Height - (float)Thickness;
			}else{
				leftForSpacing = (float)this.Bounds.Size.Width - (float)Thickness;
			}
			if(desiredTotalSpacing > 0) {
				float spacingCompressionRatio = (float)(leftForSpacing / desiredTotalSpacing) ;
				actualSpacingBefore = (float)SpacingBefore * spacingCompressionRatio;
				actualSpacingAfter = (float)SpacingAfter * spacingCompressionRatio;
			}else{
				actualSpacingBefore = 0;
				actualSpacingAfter = 0;
			}
			float thicknessOffset = (float)Thickness/2.0f;




			if(Orientation == SeparatorOrientation.Horizontal){



				var half = this.Bounds.Size.Height / 2.0f;
				context.MoveTo(0, actualSpacingBefore+thicknessOffset);
				context.AddLineToPoint(rect.Width, actualSpacingBefore+thicknessOffset);
			}else{
				var half = this.Bounds.Size.Width / 2.0f;
				context.MoveTo(actualSpacingBefore+thicknessOffset, 0);
				context.AddLineToPoint(actualSpacingBefore+thicknessOffset, rect.Height);
			}
			context.StrokePath();
		}
	}

}

