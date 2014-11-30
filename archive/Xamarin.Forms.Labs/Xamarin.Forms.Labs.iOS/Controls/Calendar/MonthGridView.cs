
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

namespace escoz
{

	public class MonthGridView : UIView
	{
		private CalendarMonthView _calendarMonthView;

		public DateTime CurrentDate {get;set;}
		private DateTime _currentMonth;

		protected readonly IList<CalendarDayView> _dayTiles = new List<CalendarDayView>();
		public int Lines { get; set; }
		protected CalendarDayView SelectedDayView {get;set;}
		public int weekdayOfFirst;
		public IList<DateTime> Marks { get; set; }

		public MonthGridView(CalendarMonthView calendarMonthView, DateTime month)
		{
			_calendarMonthView = calendarMonthView;
			_currentMonth = month.Date;

			var tapped = new UITapGestureRecognizer(p_Tapped);
			this.AddGestureRecognizer(tapped);
		}

		void p_Tapped(UITapGestureRecognizer tapRecg)
		{
			var loc = tapRecg.LocationInView(this);
			if (SelectDayView(loc)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag));
		}

		public void Update(){
			foreach (var v in _dayTiles)
				updateDayView(v);

			this.SetNeedsDisplay();
		}

		public void updateDayView(CalendarDayView dayView){
			dayView.Marked = _calendarMonthView.IsDayMarkedDelegate == null ? 
				false : _calendarMonthView.IsDayMarkedDelegate(dayView.Date);
			dayView.Available = _calendarMonthView.IsDateAvailable == null ? 
				true : _calendarMonthView.IsDateAvailable(dayView.Date);
			dayView.Highlighted = _calendarMonthView.HighlightedDaysOfWeek[(int)dayView.Date.DayOfWeek];
		}

		public void BuildGrid ()
		{
			//DateTime dt = DateTime.Now;
			DateTime previousMonth = _currentMonth.AddMonths (-1);
			DateTime nextMonth = _currentMonth.AddMonths (1);
			var daysInPreviousMonth = DateTime.DaysInMonth (previousMonth.Year, previousMonth.Month);
			var daysInMonth = DateTime.DaysInMonth (_currentMonth.Year, _currentMonth.Month);
			var firstDayOfWeek = (int) CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			weekdayOfFirst = (int)_currentMonth.DayOfWeek;
			//var lead = daysInPreviousMonth  - ((weekdayOfFirst+firstDayOfWeek) - 1);

			int boxWidth = _calendarMonthView.BoxWidth;
			int boxHeight = _calendarMonthView.BoxHeight;

			var numberOfLastMonthDays = (weekdayOfFirst - firstDayOfWeek);
			if(numberOfLastMonthDays < 0){
				numberOfLastMonthDays = 7 - (weekdayOfFirst + firstDayOfWeek);
			}
			var lead = daysInPreviousMonth - numberOfLastMonthDays + 1;
			// build last month's days
			for (int i = 1; i <= numberOfLastMonthDays; i++)
			{
				var viewDay = new DateTime (previousMonth.Year, previousMonth.Month, lead);
				var dayView = new CalendarDayView (_calendarMonthView);
				dayView.Frame = new RectangleF ((i - 1) * boxWidth, 0, boxWidth, boxHeight);
				dayView.Date = viewDay;
				dayView.Text = lead.ToString ();
				updateDayView (dayView);
				AddSubview (dayView);
				_dayTiles.Add (dayView);
				lead++;
			}

			var position = weekdayOfFirst-firstDayOfWeek + 1;
			if(position == 0){
				position = 7;
			}
			var line = 0;

			// current month
			for (int i = 1; i <= daysInMonth; i++)
			{
				var viewDay = new DateTime (_currentMonth.Year, _currentMonth.Month, i);
				var dayView = new CalendarDayView(_calendarMonthView)
				{
					Frame = new RectangleF((position - 1) * boxWidth, line * boxHeight, boxWidth, boxHeight),
					Today = (CurrentDate.Date==viewDay.Date),
					Text = i.ToString(),

					Active = true,
					Tag = i,
					Selected = (viewDay.Date == _calendarMonthView.CurrentSelectedDate.Date)
				};

				dayView.Date = viewDay;
				updateDayView (dayView);

				if (dayView.Selected)
					SelectedDayView = dayView;

				AddSubview (dayView);
				_dayTiles.Add (dayView);

				position++;
				if (position > 7)
				{
					position = 1;
					line++;
				}
			}

			//next month
			int dayCounter = 1;
			if (position != 1)
			{
				for (int i = position; i < 8; i++)
				{
					var viewDay = new DateTime (nextMonth.Year, nextMonth.Month, dayCounter);
					var dayView = new CalendarDayView (_calendarMonthView)
					{
						Frame = new RectangleF((i - 1) * boxWidth, line * boxHeight, boxWidth, boxHeight),
						Text = dayCounter.ToString(),
					};
					dayView.Date = viewDay;
					updateDayView (dayView);

					AddSubview (dayView);
					_dayTiles.Add (dayView);
					dayCounter++;
				}
			}

//Why to add unnecesarry unclickable dates of next month?
//			while (line < 5)
//			{
//				line++;
//				for (int i = 1; i < 8; i++)
//				{
//					var viewDay = new DateTime (_currentMonth.Year, _currentMonth.Month, i);
//					var dayView = new CalendarDayView (_calendarMonthView)
//					{
//						Frame = new RectangleF((i - 1) * boxWidth -1, line * boxHeight, boxWidth, boxHeight),
//						Text = dayCounter.ToString(),
//					};
//					dayView.Date = viewDay;
//					updateDayView (dayView);
//
//					AddSubview (dayView);
//					_dayTiles.Add (dayView);
//					dayCounter++;
//				}
//			}

			Frame = new RectangleF(Frame.Location, new SizeF(Frame.Width, (line + 1) * boxHeight));

			Lines = (position == 1 ? line - 1 : line);

			if (SelectedDayView!=null)
				this.BringSubviewToFront(SelectedDayView);
			//Console.WriteLine("Building the grid took {0} msecs",(DateTime.Now-dt).TotalMilliseconds);
		}

		/*public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			if (SelectDayView((UITouch)touches.AnyObject)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag));
		}
		
		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			if (SelectDayView((UITouch)touches.AnyObject)&& _calendarMonthView.OnDateSelected!=null)
				_calendarMonthView.OnDateSelected(new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag));
		}
		
		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			if (_calendarMonthView.OnFinishedDateSelection==null) return;
			var date = new DateTime(_currentMonth.Year, _currentMonth.Month, SelectedDayView.Tag);
			if (_calendarMonthView.IsDateAvailable == null || _calendarMonthView.IsDateAvailable(date))
				_calendarMonthView.OnFinishedDateSelection(date);
		}*/

		private bool SelectDayView(PointF p){

			int index = ((int)p.Y / _calendarMonthView.BoxHeight) * 7 + ((int)p.X / _calendarMonthView.BoxWidth);
			if(index<0 || index >= _dayTiles.Count) return false;

			var newSelectedDayView = _dayTiles[index];
			if (newSelectedDayView == SelectedDayView) 
				return false;

			if (!newSelectedDayView.Active){
				var day = int.Parse(newSelectedDayView.Text);
				if (day > 15)
					_calendarMonthView.MoveCalendarMonths(false, true);
				else
					_calendarMonthView.MoveCalendarMonths(true, true);
				return false;
			} else if (!newSelectedDayView.Active && !newSelectedDayView.Available){
				return false;
			}

			if (SelectedDayView!=null)
				SelectedDayView.Selected = false;

			this.BringSubviewToFront(newSelectedDayView);
			newSelectedDayView.Selected = true;

			SelectedDayView = newSelectedDayView;
			_calendarMonthView.CurrentSelectedDate =  SelectedDayView.Date;
			SetNeedsDisplay();
			return true;
		}

		public void DeselectDayView(){
			if (SelectedDayView==null) return;
			SelectedDayView.Selected= false;
			SelectedDayView = null;
			SetNeedsDisplay();
		}
	}

}
