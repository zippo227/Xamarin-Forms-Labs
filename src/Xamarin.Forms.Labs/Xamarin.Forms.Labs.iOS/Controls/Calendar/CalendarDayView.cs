
//
//  CalendarMonthView.cs
//
//  Converted to MonoTouch on 1/22/09 - Eduardo Scoz || http://escoz.com
//  Originally reated by Devin Ross on 7/28/09  - tapku.com || http://github.com/devinross/tapkulibrary
//  Adapted for Xamarin.Forms.Labs project by Vratislav Kalenda on 22/09/14 http://www.applifting.cz
/*
 
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
 
 */

using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using Xamarin.Forms.Labs.iOS.Controls.Calendar;
using Xamarin.Forms.Labs.Controls;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;


namespace escoz
{

	public class CalendarDayView : UIView
	{
		string _text;
		private static NSMutableParagraphStyle paragraphStyle;
		private UIColor _oldBackgorundColor;
		public DateTime Date {get;set;}
		bool _active, _today, _selected, _marked, _available,_highlighted;
		public bool Available {get {return _available; } set {_available = value; SetNeedsDisplay(); }}
		public string Text {get { return _text; } set { _text = value; SetNeedsDisplay(); } }
		public bool Active {get { return _active; } set { _active = value; SetNeedsDisplay();  } }
		public bool Today {get { return _today; } set { _today = value; SetNeedsDisplay(); } }
		public bool Selected {get { return _selected; } set { _selected = value; SetNeedsDisplay(); } }
		public bool Marked {get { return _marked; } set { _marked = value; SetNeedsDisplay(); }  }
		public bool Highlighted {get { return _highlighted; } set { _highlighted = value; SetNeedsDisplay(); }  }

		CalendarMonthView _mv;

		public CalendarDayView (CalendarMonthView mv)
		{
			_mv = mv;
			BackgroundColor = mv.StyleDescriptor.DateBackgroundColor;
		}

		public override void Draw(RectangleF rect)
		{
			DateTime dt = DateTime.Now;
			UIImage img = null;
			var color = _mv.StyleDescriptor.InactiveDateForegroundColor;
			BackgroundColor = _mv.StyleDescriptor.InactiveDateBackgroundColor;
			CalendarView.BackgroundStyle backgroundStyle = CalendarView.BackgroundStyle.Fill;


			if (!Active || !Available)
			{
				if(Highlighted){
					BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
				}
				//color = UIColor.FromRGBA(0.576f, 0.608f, 0.647f, 1f);
				//img = UIImage.FromBundle("Images/Calendar/datecell.png");
			} 
			else if (Today && Selected)
			{
				color = _mv.StyleDescriptor.SelectedDateForegroundColor;
				BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
				backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
				//img = UIImage.FromBundle("Images/Calendar/todayselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
			} 
			else if (Today)
			{
				color = _mv.StyleDescriptor.TodayForegroundColor;
				BackgroundColor = _mv.StyleDescriptor.TodayBackgroundColor;
				backgroundStyle = _mv.StyleDescriptor.TodayBackgroundStyle;
				//img = UIImage.FromBundle("Images/Calendar/today.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
			} 
			else if (Selected || Marked)
			{
				//color = UIColor.White;
				color = _mv.StyleDescriptor.SelectedDateForegroundColor;
				BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
				backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
				//img = UIImage.FromBundle("Images/Calendar/datecellselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
			}else if(Highlighted){
				color = _mv.StyleDescriptor.HighlightedDateForegroundColor;
				BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
			}
			else
			{
				color = _mv.StyleDescriptor.DateForegroundColor;
				BackgroundColor = _mv.StyleDescriptor.DateBackgroundColor;
				//img = UIImage.FromBundle("Images/Calendar/datecell.png");
			}

			//if (img != null)
				//img.Draw(new RectangleF(0, 0, _mv.BoxWidth, _mv.BoxHeight));
			var context = UIGraphics.GetCurrentContext();
			if(_oldBackgorundColor != BackgroundColor) {
				if(backgroundStyle == CalendarView.BackgroundStyle.Fill) {
					context.SetFillColor(BackgroundColor.CGColor);
					context.FillRect(new RectangleF(0, 0, _mv.BoxWidth, _mv.BoxHeight));
				}else{
					if(Highlighted) {
						context.SetFillColor(_mv.StyleDescriptor.HighlightedDateBackgroundColor.CGColor);
					} else {
						context.SetFillColor(_mv.StyleDescriptor.DateBackgroundColor.CGColor);
					}
					context.FillRect(new RectangleF(0, 0, _mv.BoxWidth, _mv.BoxHeight));

					var smallerSide = Math.Min(_mv.BoxWidth, _mv.BoxHeight);
					var center = new PointF(_mv.BoxWidth / 2, _mv.BoxHeight / 2);
					var circleArea = new  RectangleF(center.X-smallerSide/2,center.Y-smallerSide/2,smallerSide,smallerSide);

					if(backgroundStyle == CalendarView.BackgroundStyle.CircleFill){
						context.SetFillColor(BackgroundColor.CGColor);
						context.FillEllipseInRect(circleArea.Inset(1,1));
					}else{
						context.SetStrokeColor(BackgroundColor.CGColor);
						context.StrokeEllipseInRect(circleArea.Inset(2,2));
					}
				}
			}


			color.SetColor();
			var inflated = new RectangleF(0, 0, Bounds.Width, Bounds.Height);
//			var attrs = new UIStringAttributes() {
//				Font = _mv.StyleDescriptor.DateLabelFont,
//				ForegroundColor = color,
//				ParagraphStyle = 
//
//			};
			//((NSString)Text).DrawString(inflated,attrs);
			//DrawString(Text, inflated,_mv.StyleDescriptor.DateLabelFont,UILineBreakMode.WordWrap, UITextAlignment.Center);
			this.DrawDateString((NSString)Text, color, inflated);

			//            if (Marked)
			//            {
			//                var context = UIGraphics.GetCurrentContext();
			//                if (Selected || Today)
			//                    context.SetRGBFillColor(1, 1, 1, 1);
			//                else if (!Active || !Available)
			//					UIColor.LightGray.SetColor();
			//				else
			//                    context.SetRGBFillColor(75/255f, 92/255f, 111/255f, 1);
			//                context.SetLineWidth(0);
			//                context.AddEllipseInRect(new RectangleF(Frame.Size.Width/2 - 2, 45-10, 4, 4));
			//                context.FillPath();
			//
			//            }
			_oldBackgorundColor = BackgroundColor;
			//Console.WriteLine("Drawing of cell took {0} msecs",(DateTime.Now-dt).TotalMilliseconds);
		}

		private void DrawDateString(NSString dateString,UIColor color, RectangleF rect){
			if(paragraphStyle == null){
				paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
				paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
				paragraphStyle.Alignment = UITextAlignment.Center;

			}
			var attrs = new UIStringAttributes() {
				Font = _mv.StyleDescriptor.DateLabelFont,
				ForegroundColor = color,
				ParagraphStyle = paragraphStyle
			};
			var size = dateString.GetSizeUsingAttributes(attrs);
			RectangleF targetRect = new RectangleF(
				rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
				rect.Y +(float) Math.Floor((rect.Height - size.Height) / 2f),
				                        size.Width,
				                        size.Height
			                        );
			dateString.DrawString(targetRect, attrs);
		}

//		/*
//      (void) drawString: (NSString*) s
//           withFont: (UIFont*) font
//             inRect: (CGRect) contextRect {
//
//    /// Make a copy of the default paragraph style
//    NSMutableParagraphStyle *paragraphStyle = [[NSParagraphStyle defaultParagraphStyle] mutableCopy];
//    /// Set line break mode
//    paragraphStyle.lineBreakMode = NSLineBreakByTruncatingTail;
//    /// Set text alignment
//    paragraphStyle.alignment = NSTextAlignmentCenter;
//
//    NSDictionary *attributes = @{ NSFontAttributeName: font,
//                                  NSForegroundColorAttributeName: [UIColor whiteColor],
//                                  NSParagraphStyleAttributeName: paragraphStyle };
//
//    CGSize size = [s sizeWithAttributes:attributes];
//
//    CGRect textRect = CGRectMake(contextRect.origin.x + floorf((contextRect.size.width - size.width) / 2),
//                                 contextRect.origin.y + floorf((contextRect.size.height - size.height) / 2),
//                                 size.width,
//                                 size.height);
//
//    [s drawInRect:textRect withAttributes:attributes];
//}
//
//
//		*/
	}
}
