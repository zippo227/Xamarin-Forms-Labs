using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace Xamarin.Forms.Labs.iOS.Controls.Calendar
{
	public class CalendarArrowView : UIButton
	{
		public enum ArrowDirection {
			RIGHT, LEFT
		}

		private ArrowDirection _arrowDirection = ArrowDirection.LEFT;
		public ArrowDirection Direction{
			get{
				return _arrowDirection;
			}
			set{
				_arrowDirection = value;
				this.SetBackgroundImage(this.GenerateImageForButton(this.Frame), UIControlState.Normal);
				//_trianglePath = GetEquilateralTriangle(this.Width, this.Height);
				SetNeedsDisplay();
			}
		}

		private UIColor _color;
		public UIColor Color{
			set{
				_color = value;
				SetNeedsDisplay();
			}
		}



		public CalendarArrowView(RectangleF frame) : base(){
			this.Frame = frame;
			this.UserInteractionEnabled = true;
			this.BackgroundColor = UIColor.Clear;
		}
	

		private UIImage GenerateImageForButton(System.Drawing.RectangleF rect){
			UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0);
			UIImage image = null;
			using(var context = UIGraphics.GetCurrentContext()){
				PointF p1 , p2 , p3 ;
				if(_arrowDirection == ArrowDirection.LEFT){

					p1 = new PointF(0, (rect.Height) / 2);
					p2 = new PointF(rect.Width, 0);
					p3 = new PointF(rect.Width, rect.Height);
				}else{
					p1 = new PointF(rect.Width, rect.Height / 2);
					p2 = new PointF(0, 0);
					p3 = new PointF(0, rect.Height);
				}
				context.SetFillColor(UIColor.Clear.CGColor);
				context.FillRect(rect);
				context.SetFillColor(_color.CGColor);
				context.MoveTo(p1.X, p1.Y);
				context.AddLineToPoint(p2.X, p2.Y);
				context.AddLineToPoint(p3.X, p3.Y);
				context.FillPath();
				image = UIGraphics.GetImageFromCurrentImageContext();
			}
			UIGraphics.EndImageContext();
			return image;
		}

		public override void Draw(System.Drawing.RectangleF rect)
		{
			base.Draw(rect);

		}
			




	}
}

