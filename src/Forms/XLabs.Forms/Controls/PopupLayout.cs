using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	///     Class PopupLayout.
	/// </summary>
	public class PopupLayout : RelativeLayout
	{
		/// <summary>
		///     Popup location options when relative to another view
		/// </summary>
		public enum PopupLocation
		{
			/// <summary>
			///     Will show popup on top of the specified view
			/// </summary>
			Top,

			/// <summary>
			///     Will show popup below of the specified view
			/// </summary>
			Bottom
			/// <summary>
			/// Will show popup left to the specified view
			/// </summary>
			//Left,
			/// <summary>
			/// Will show popup right of the specified view
			/// </summary>
			//Right
		}

		/// <summary>
		///     The content
		/// </summary>
		private View _content;

		/// <summary>
		///     The popup
		/// </summary>
		private View _popup;

		/// <summary>
		///     Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public View Content
		{
			get { return _content; }
			set
			{
				if (_content != null)
				{
					Children.Remove(_content);
				}

				_content = value;
				Children.Add(_content, () => Bounds);
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance is popup active.
		/// </summary>
		/// <value><c>true</c> if this instance is popup active; otherwise, <c>false</c>.</value>
		public bool IsPopupActive
		{
			get { return _popup != null; }
		}

		/// <summary>
		///     Shows the popup.
		/// </summary>
		/// <param name="popupView">The popup view.</param>
		public void ShowPopup(View popupView)
		{
			DismissPopup();
			_popup = popupView;

			_content.InputTransparent = true;
			Children.Add(
				_popup,
				Constraint.RelativeToParent(p => (Width/2) - (_popup.WidthRequest/2)),
				Constraint.RelativeToParent(p => (Height/2) - (_popup.HeightRequest/2)),
				Constraint.RelativeToParent(p => _popup.WidthRequest),
				Constraint.RelativeToParent(p => _popup.HeightRequest)
				);

			if (Device.OS == TargetPlatform.Android)
			{
				LowerChild(_popup);
			}

			UpdateChildrenLayout();
		}

		/// <summary>
		///     Shows the popup.
		/// </summary>
		/// <param name="popupView">The popup view.</param>
		/// <param name="presenter">The presenter.</param>
		/// <param name="location">The location.</param>
		/// <param name="paddingX">The padding x.</param>
		/// <param name="paddingY">The padding y.</param>
		public void ShowPopup(View popupView, View presenter, PopupLocation location, float paddingX = 0, float paddingY = 0)
		{
			DismissPopup();
			_popup = popupView;

			Constraint constraintX = null, constraintY = null, constraintW = null, constraintH = null;

			switch (location)
			{
				case PopupLocation.Bottom:
					constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - _popup.WidthRequest)/2);
					constraintY = Constraint.RelativeToParent(parent => parent.Y + presenter.Y + presenter.Height + paddingY);
					break;
				case PopupLocation.Top:
					constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - _popup.WidthRequest)/2);
					constraintY = Constraint.RelativeToParent(parent =>
						parent.Y + presenter.Y - _popup.HeightRequest/2 - paddingY);
					break;
				//case PopupLocation.Left:
				//    constraintX = Constraint.RelativeToView(presenter, (parent, view) => ((view.X + view.Height / 2) - parent.X) + this.popup.HeightRequest / 2);
				//    constraintY = Constraint.RelativeToView(presenter, (parent, view) => parent.Y + view.Y + view.Width + paddingY);
				//    break;
				//case PopupLocation.Right:
				//    constraintX = Constraint.RelativeToView(presenter, (parent, view) => ((view.X + view.Height / 2) - parent.X) + this.popup.HeightRequest / 2);
				//    constraintY = Constraint.RelativeToView(presenter, (parent, view) => parent.Y + view.Y - this.popup.WidthRequest - paddingY);
				//    break;
			}

			constraintW = Constraint.RelativeToParent(p => _popup.WidthRequest);
			constraintH = Constraint.RelativeToParent(p => _popup.HeightRequest);

			_content.InputTransparent = true;
			Children.Add(
				_popup,
				constraintX,
				constraintY,
				constraintW,
				constraintH
				);

			if (Device.OS == TargetPlatform.Android)
			{
				LowerChild(_popup);
			}

			UpdateChildrenLayout();
		}

		/// <summary>
		///     Dismisses the popup.
		/// </summary>
		public void DismissPopup()
		{
			if (_popup != null)
			{
				Children.Remove(_popup);
				_popup = null;
			}

			_content.InputTransparent = false;
		}
	}
}