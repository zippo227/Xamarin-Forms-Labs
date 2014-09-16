
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
using System.Globalization;
using MonoTouch.Foundation;
using Xamarin.Forms.Labs.Controls;

namespace escoz
{

	public delegate void DateSelected(DateTime date);
	public delegate void MonthChanged(DateTime monthSelected);

	public class CalendarMonthView : UIView
	{
		public int BoxHeight = 30;
		public int BoxWidth = 46;
		int headerHeight = 0;

		public Action<DateTime> OnDateSelected;
		public Action<DateTime> OnFinishedDateSelection;
		public Func<DateTime, bool> IsDayMarkedDelegate;
		public Func<DateTime, bool> IsDateAvailable;
		public Action<DateTime> MonthChanged;
		Dictionary<int, bool> _hlighlightedDaysOfWeek;
		public Dictionary<int, bool> HighlightedDaysOfWeek{
			get{
				return _hlighlightedDaysOfWeek;
			}
		}
		public Action SwipedUp;

		public DateTime CurrentSelectedDate;
		public DateTime CurrentMonthYear;
		protected DateTime CurrentDate { get; set; }



		private UIScrollView _scrollView;
		private bool calendarIsLoaded;
		private DateTime? _minDateTime;
		private DateTime? _maxDateTime;
		private MonthGridView _monthGridView;
		bool _showHeader;
		private bool _showNavArrows;

		CalendarArrowView _leftArrow;

		CalendarArrowView _rightArrow;

		private StyleDescriptor _styleDescriptor;
		public StyleDescriptor StyleDescriptor{
			get{
				return _styleDescriptor;
			}
		}



		public CalendarMonthView(DateTime selectedDate, bool showHeader,bool showNavArrows, float width = 320) 
		{

			_showHeader = showHeader;
			_showNavArrows = showNavArrows;
			if(_showNavArrows){
				_showHeader = true;
			}
			_styleDescriptor = new StyleDescriptor();
			HighlightDaysOfWeeks(new DayOfWeek[]{});
			if (_showHeader && headerHeight == 0){
				if(showNavArrows){
					headerHeight = 40;
				}else{
					headerHeight = 20;
				}
			}
				

			if (_showHeader)
				this.Frame = new RectangleF(0, 0, width, 198+headerHeight);
			else 
				this.Frame = new RectangleF(0, 0, width, 198);

			BoxWidth = Convert.ToInt32(Math.Ceiling( width / 7 ));

			BackgroundColor = UIColor.White;

			ClipsToBounds = true;
			CurrentDate = DateTime.Now.Date;
			CurrentMonthYear = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);

			CurrentSelectedDate = selectedDate;



			var swipeLeft = new UISwipeGestureRecognizer(p_monthViewSwipedLeft);
			swipeLeft.Direction = UISwipeGestureRecognizerDirection.Left;
			this.AddGestureRecognizer(swipeLeft);

			var swipeRight = new UISwipeGestureRecognizer(p_monthViewSwipedRight);
			swipeRight.Direction = UISwipeGestureRecognizerDirection.Right;
			this.AddGestureRecognizer(swipeRight);

			var swipeUp = new UISwipeGestureRecognizer(p_monthViewSwipedUp);
			swipeUp.Direction = UISwipeGestureRecognizerDirection.Up;
			this.AddGestureRecognizer(swipeUp);
		}

		public void SetDate (DateTime newDate,bool animated)
		{
			bool right = true;

			CurrentSelectedDate = newDate;

			var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
			if (monthsDiff != 0)
			{
				if (monthsDiff < 0)
				{
					right = false;
					monthsDiff = -monthsDiff;
				}

				for (int i=0; i<monthsDiff; i++)
				{
					MoveCalendarMonths (right, animated);
				}
			} 
			else
			{
				//If we have created the layout already
				if(_scrollView != null) {
					RebuildGrid(right, animated);
				}
			}

		}

		public void SetMaxAllowedDate(DateTime? maxDate){
			_maxDateTime = maxDate;
		}
		public void SetMinAllowedDate(DateTime? minDate){
			_minDateTime = minDate;
		}


		public void HighlightDaysOfWeeks(DayOfWeek[] daysOfWeeks)
		{
			_hlighlightedDaysOfWeek = new Dictionary<int,bool>();
			for(int i = 0; i <= 6; i++){
				_hlighlightedDaysOfWeek[i] = false;
			}
			foreach(var dOW in daysOfWeeks){
				_hlighlightedDaysOfWeek[(int)dOW] = true;
			}
		}

		public void SetDisplayedMonthYear(DateTime newDate, bool animated){
			bool right = true;
			var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
			if (monthsDiff != 0)
			{
				if (monthsDiff < 0)
				{
					right = false;
					monthsDiff = -monthsDiff;
				}

				for (int i=0; i<monthsDiff; i++)
				{
					MoveCalendarMonths (right, animated);
				}
			} 
			else
			{
				//If we have created the layout already
				if(_scrollView != null) {
					RebuildGrid(right, animated);
				}
			}
		}

		void p_monthViewSwipedUp (UISwipeGestureRecognizer ges)
		{
			if (SwipedUp != null)
				SwipedUp ();
		}

		void p_monthViewSwipedRight (UISwipeGestureRecognizer ges)
		{
			MoveCalendarMonths(false, true);
		}

		void p_monthViewSwipedLeft (UISwipeGestureRecognizer ges)
		{
			MoveCalendarMonths(true, true);
		}

		public override void SetNeedsDisplay ()
		{
			base.SetNeedsDisplay();
			if (_monthGridView!=null)
				_monthGridView.Update();
		}

		public override void LayoutSubviews ()
		{
			if (calendarIsLoaded) return;

			_scrollView = new UIScrollView()
			{
				ContentSize = new SizeF(320, 260),
				ScrollEnabled = false,
				Frame = new RectangleF(0, 16 + headerHeight, 320, this.Frame.Height - 16),
				BackgroundColor = _styleDescriptor.BackgroundColor
			};

			//_shadow = new UIImageView(UIImage.FromBundle("Images/Calendar/shadow.png"));

			//LoadButtons();

			LoadNavArrows();
			SetNavigationArrows(false);
			LoadInitialGrids();

			BackgroundColor = UIColor.Clear;

			AddSubview(_scrollView);

			//AddSubview(_shadow);

			_scrollView.AddSubview(_monthGridView);

			calendarIsLoaded = true;
		}

		public void DeselectDate(){
			if (_monthGridView!=null)
				_monthGridView.DeselectDayView();
		}

		void LoadNavArrows()
		{
			_leftArrow = new CalendarArrowView(new RectangleF(10, 9, 18, 22));
			_leftArrow.Color = StyleDescriptor.TitleForegroundColor;
			_leftArrow.TouchUpInside += HandlePreviousMonthTouch;
			_leftArrow.Direction = CalendarArrowView.ArrowDirection.LEFT;
			this.AddSubview(_leftArrow);
			_rightArrow = new CalendarArrowView(new RectangleF(320-22-10, 9, 18, 22));
			_rightArrow.Color = StyleDescriptor.TitleForegroundColor;
			_rightArrow.TouchUpInside += HandleNextMonthTouch;
			_rightArrow.Direction = CalendarArrowView.ArrowDirection.RIGHT;
			this.AddSubview(_rightArrow);
		}

		/*private void LoadButtons()
		{
			_leftButton = UIButton.FromType(UIButtonType.Custom);
			_leftButton.TouchUpInside += HandlePreviousMonthTouch;
			_leftButton.SetImage(UIImage.FromBundle("Images/Calendar/leftarrow.png"), UIControlState.Normal);
			AddSubview(_leftButton);
			_leftButton.Frame = new RectangleF(10, 0, 44, 42);
			
			_rightButton = UIButton.FromType(UIButtonType.Custom);
			_rightButton.TouchUpInside += HandleNextMonthTouch;
			_rightButton.SetImage(UIImage.FromBundle("Images/Calendar/rightarrow.png"), UIControlState.Normal);
			AddSubview(_rightButton);
			_rightButton.Frame = new RectangleF(320 - 56, 0, 44, 42);
		}*/

		private void HandlePreviousMonthTouch(object sender, EventArgs e)
		{
			MoveCalendarMonths(false, true);
		}
		private void HandleNextMonthTouch(object sender, EventArgs e)
		{
			MoveCalendarMonths(true, true);
		}

		public void MoveCalendarMonths (bool right, bool animated)
		{
			var newDate = CurrentMonthYear.AddMonths(right? 1 : -1);
			if((_minDateTime != null && newDate < _minDateTime.Value.Date) || (_maxDateTime != null && newDate > _maxDateTime.Value.Date)){
				if (animated){
					var oldX = _monthGridView.Center.X;

					_monthGridView.Center = new PointF(oldX, _monthGridView.Center.Y);
					UIView.Animate(0.25, () => {
						if(right) {
							_monthGridView.Center = new PointF(_monthGridView.Center.X - 40, _monthGridView.Center.Y);
						} else {
							_monthGridView.Center = new PointF(_monthGridView.Center.X + 40, _monthGridView.Center.Y);
						}
					}, () => {
						UIView.Animate(0.25, () => {
							_monthGridView.Center = new PointF(oldX, _monthGridView.Center.Y);
						});
					});

				}
				return;
			}

			CurrentMonthYear = newDate;
			SetNavigationArrows(animated);
			//If we have created the layout already
			if(_scrollView != null) {
				RebuildGrid(right, animated);
			}
		}

		private void SetNavigationArrows(bool animated){

			bool isMin = false;
			bool isMax = false;
			if(_minDateTime != null){
				isMin = CurrentMonthYear.Month == _minDateTime.Value.Month && CurrentMonthYear.Year == _minDateTime.Value.Year;
			}
			if(_maxDateTime != null){
				isMax = CurrentMonthYear.Month == _maxDateTime.Value.Month && CurrentMonthYear.Year == _maxDateTime.Value.Year;
			}

			if(_showNavArrows){
				if(animated){
					UIView.Animate(0.250, () => {
						if(isMin && _leftArrow.Enabled){
							_leftArrow.Enabled = false;
							_leftArrow.Alpha = 0;
						}else{
							_leftArrow.Enabled = true;
							_leftArrow.Alpha = 1;
						}
						if(isMax && _rightArrow.Enabled){
							_rightArrow.Enabled = false;
							_rightArrow.Alpha = 0;
						}else{
							_rightArrow.Enabled = true;
							_rightArrow.Alpha = 1;
						}
					});

				}else{
					if(isMin && _leftArrow.Enabled){
						_leftArrow.Enabled = false;
						_leftArrow.Alpha = 0;
					}else{
						_leftArrow.Enabled = true;
						_leftArrow.Alpha = 1;
					}

					if(isMax && _rightArrow.Enabled){
						_rightArrow.Enabled = false;
						_rightArrow.Alpha = 0;
					}else{
						_rightArrow.Enabled = true;
						_rightArrow.Alpha = 1;
					}


				}
			}
		}

		public void RebuildGrid(bool right, bool animated)
		{
			UserInteractionEnabled = false;

			var gridToMove = CreateNewGrid(CurrentMonthYear);
			var pointsToMove = (right? Frame.Width : -Frame.Width);

			/*if (left && gridToMove.weekdayOfFirst==0)
				pointsToMove += 44;
			if (!left && _monthGridView.weekdayOfFirst==0)
				pointsToMove -= 44;*/

			gridToMove.Frame = new RectangleF(new PointF(pointsToMove, 0), gridToMove.Frame.Size);

			_scrollView.AddSubview(gridToMove);


			if (animated){
				UIView.BeginAnimations("changeMonth");
				UIView.SetAnimationDuration(0.4);
				UIView.SetAnimationDelay(0.1);
				UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			}

			_monthGridView.Center = new PointF(_monthGridView.Center.X - pointsToMove, _monthGridView.Center.Y);
			gridToMove.Center = new PointF(gridToMove.Center.X  - pointsToMove, gridToMove.Center.Y);

			_monthGridView.Alpha = 0;

			/*_scrollView.Frame = new RectangleF(
				_scrollView.Frame.Location,
				new SizeF(_scrollView.Frame.Width, this.Frame.Height-16));
			
			_scrollView.ContentSize = _scrollView.Frame.Size;*/

			SetNeedsDisplay();

			if (animated)
				UIView.CommitAnimations();

			_monthGridView = gridToMove;

			UserInteractionEnabled = true;

			if (MonthChanged != null)
				MonthChanged(CurrentMonthYear);
		}

		private MonthGridView CreateNewGrid(DateTime date){
			var grid = new MonthGridView(this, date);
			grid.CurrentDate = CurrentDate;
			grid.BuildGrid();
			grid.Frame = new RectangleF(0, 0, 320, this.Frame.Height-16);
			return grid;
		}

		private void LoadInitialGrids()
		{
			_monthGridView = CreateNewGrid(CurrentMonthYear);

			/*var rect = _scrollView.Frame;
			rect.Size = new SizeF { Height = (_monthGridView.Lines + 1) * 44, Width = rect.Size.Width };
			_scrollView.Frame = rect;*/

			//Frame = new RectangleF(Frame.X, Frame.Y, _scrollView.Frame.Size.Width, _scrollView.Frame.Size.Height+16);

			/*var imgRect = _shadow.Frame;
			imgRect.Y = rect.Size.Height - 132;
			_shadow.Frame = imgRect;*/
		}

		public override void Draw(RectangleF rect)
		{
			using(var context = UIGraphics.GetCurrentContext())
			{
				context.SetFillColor (_styleDescriptor.TitleBackgroundColor.CGColor);
				//Console.WriteLine("Title background color is {0}",_styleDescriptor.TitleBackgroundColor.ToString());
				context.FillRect (new RectangleF (0, 0, 320, 18 + headerHeight));
			}

			DrawDayLabels(rect);

			if (_showHeader)
				DrawMonthLabel(rect);
		}

		private void DrawMonthLabel(RectangleF rect)
		{
			var r = new RectangleF(new PointF(0, 2), new SizeF {Width = 320, Height = headerHeight});
//			_styleDescriptor.TitleForegroundColor.SetColor();
//			DrawString(CurrentMonthYear.ToString("MMMM yyyy"), 
//				r, _styleDescriptor.MonthTitleFont,
//				UILineBreakMode.WordWrap, UITextAlignment.Center);
			DrawCenteredString((NSString)CurrentMonthYear.ToString("MMMM yyyy"), _styleDescriptor.TitleForegroundColor, r,StyleDescriptor.MonthTitleFont);
		}

		private void DrawCenteredString(NSString text,UIColor color, RectangleF rect,UIFont font){

			var paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
			paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
			paragraphStyle.Alignment = UITextAlignment.Center;
			var attrs = new UIStringAttributes() {
				Font = font,
				ForegroundColor = color,
				ParagraphStyle = paragraphStyle
			};
			var size = text.GetSizeUsingAttributes(attrs);
			RectangleF targetRect = new RectangleF(
				rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
				rect.Y +(float) Math.Floor((rect.Height - size.Height) / 2f),
				size.Width,
				size.Height
			);
			text.DrawString(targetRect, attrs);

		}


		private void DrawDayLabels(RectangleF rect)
		{
			var font = _styleDescriptor.DateLabelFont;

			var context = UIGraphics.GetCurrentContext();
			context.SaveState();
			var firstDayOfWeek = (int) CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			var today = CurrentDate;
			var originalDay = today;
			for (int i = 0; i < 7; i++) {
				var offset = firstDayOfWeek - (int) today.DayOfWeek + i;
				today = today.AddDays(offset);
				var dateRectangle = new RectangleF(i*BoxWidth, 2 + headerHeight, BoxWidth, 15);
				if(StyleDescriptor.ShouldHighlightDaysOfWeekLabel && _hlighlightedDaysOfWeek[(int)today.DayOfWeek] == true) {
					context.SetFillColorWithColor(_styleDescriptor.HighlightedDateBackgroundColor.CGColor);
				} else {
					context.SetFillColorWithColor(_styleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);

				}
				context.FillRect(dateRectangle);
				if(StyleDescriptor.ShouldHighlightDaysOfWeekLabel && _hlighlightedDaysOfWeek[(int)today.DayOfWeek] == true) {
					_styleDescriptor.HighlightedDateForegroundColor.SetColor();
				} else {
					_styleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
				}
				DrawString(today.ToString("ddd"),dateRectangle, font,
					UILineBreakMode.WordWrap, UITextAlignment.Center);
				today = originalDay;
			}

//			var i = 0;
//			foreach (var d in Enum.GetNames(typeof(DayOfWeek)))
//			{
//				var dateRectangle = new RectangleF(i*BoxWidth, 2 + headerHeight, BoxWidth, 10);
//				context.SetFillColorWithColor(_styleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);
//				context.FillRect(dateRectangle);
//				_styleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
//				DrawString(d.Substring(0, 3),dateRectangle, font,
//					UILineBreakMode.WordWrap, UITextAlignment.Center);
//				i++;
//			}
			context.RestoreState();
		}
	}


}

