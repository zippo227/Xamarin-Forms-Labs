using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    ///     Class PopupLayout.
    /// </summary>
    public class PopupLayout : RelativeLayout
    {
        /// <summary>
        /// Popup location options when relative to another view
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
        /// The content
        /// </summary>
        private View content;

        /// <summary>
        /// The popup
        /// </summary>
        private View popup;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public View Content
        {
            get { return this.content; }
            set
            {
                if (this.content != null)
                {
                    this.Children.Remove(this.content);
                }

                this.content = value;
                this.Children.Add(this.content, () => this.Bounds);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is popup active.
        /// </summary>
        /// <value><c>true</c> if this instance is popup active; otherwise, <c>false</c>.</value>
        public bool IsPopupActive
        {
            get { return this.popup != null; }
        }

        /// <summary>
        /// Shows the popup.
        /// </summary>
        /// <param name="popupView">The popup view.</param>
        public void ShowPopup(View popupView)
        {
            DismissPopup();
            this.popup = popupView;

            this.content.InputTransparent = true;
            this.Children.Add(
                this.popup,
                Constraint.RelativeToParent(p => (this.Width/2) - (this.popup.WidthRequest/2)),
                Constraint.RelativeToParent(p => (this.Height/2) - (this.popup.HeightRequest/2)),
                Constraint.RelativeToParent(p => this.popup.WidthRequest),
                Constraint.RelativeToParent(p => this.popup.HeightRequest)
                );

            UpdateChildrenLayout();
        }

        /// <summary>
        /// Shows the popup.
        /// </summary>
        /// <param name="popupView">The popup view.</param>
        /// <param name="presenter">The presenter.</param>
        /// <param name="location">The location.</param>
        /// <param name="paddingX">The padding x.</param>
        /// <param name="paddingY">The padding y.</param>
        public void ShowPopup(View popupView, View presenter, PopupLocation location, float paddingX = 0, float paddingY = 0)
        {
            DismissPopup();
            this.popup = popupView;

            Constraint constraintX = null, constraintY = null;

            var constraintW = Constraint.RelativeToParent(p => this.popup.WidthRequest);
            var constraintH = Constraint.RelativeToParent(p => this.popup.HeightRequest);

            switch (location)
            {
                case PopupLocation.Bottom:
                    constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - this.popup.WidthRequest)/2);
                    constraintY = Constraint.RelativeToParent(parent => parent.Y + presenter.Y + presenter.Height + paddingY);
                    break;
                case PopupLocation.Top:
                    constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - this.popup.WidthRequest)/2);
                    constraintY = Constraint.RelativeToParent(parent =>
                        parent.Y + presenter.Y - this.popup.HeightRequest/2 - paddingY);
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

            this.content.InputTransparent = true;
            this.Children.Add(this.popup, constraintX, constraintY, constraintW, constraintH);
            UpdateChildrenLayout();
        }

        /// <summary>
        /// Dismisses the popup.
        /// </summary>
        public void DismissPopup()
        {
            if (this.popup != null)
            {
                this.Children.Remove(this.popup);
                this.popup = null;
            }

            this.content.InputTransparent = false;
        }
    }
}