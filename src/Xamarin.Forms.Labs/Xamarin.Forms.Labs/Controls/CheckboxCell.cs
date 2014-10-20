using System;

namespace Xamarin.Forms.Labs.Controls
{
	public class CheckboxCell : ExtendedTextCell
	{
		public EventHandler<EventArgs<bool>> CheckedChanged;

		public static readonly BindableProperty CheckedProperty = BindableProperty.Create<CheckboxCell, bool> (p => p.Checked, default(bool));
		public bool Checked {
			get { return (bool)GetValue (CheckedProperty); }
			set { SetValue (CheckedProperty, value); }
		}

		bool turnOnOnly;

		public CheckboxCell (bool turnOnOnly = false)
		{
			this.turnOnOnly = turnOnOnly;
			Tapped += HandleTapped;
			BackgroundColor = Color.White;
		}

		void HandleTapped (object sender, EventArgs e)
		{
			if (turnOnOnly) {
				if (!Checked) {
					Checked = true;
					RaiseCheckedChanged (Checked);
				}
			} else {
				Checked = !Checked;
				RaiseCheckedChanged (Checked);
			}
		}

		void RaiseCheckedChanged(bool val)
		{
			if (CheckedChanged != null)
				CheckedChanged (this, new EventArgs<bool> (val));
		}
	}
}

