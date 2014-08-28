using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    public class PopupLayout : RelativeLayout
    {
        private View content;
        private View popup;

        public View Content
        {
            get
            {
                return content;
            }
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

        public bool IsPopupActive
        {
            get
            {
                return this.popup != null;
            }
        }

        public void ShowPopup(View popupView)
        {
            this.DismissPopup();
            this.popup = popupView;

            this.content.InputTransparent = true;
            this.Children.Add(
                this.popup,
                Constraint.RelativeToParent(p => (this.Width / 2) - (this.popup.WidthRequest / 2)),
                Constraint.RelativeToParent(p => (this.Height / 2) - (this.popup.HeightRequest / 2)),
                Constraint.RelativeToParent(p => this.popup.WidthRequest),
                Constraint.RelativeToParent(p => this.popup.HeightRequest)
                );

            if (Device.OS == TargetPlatform.Android)
            {
                this.Children.Remove(this.content);
                this.Children.Add(this.content, () => this.Bounds);
            }

            //this.LowerChild(this.Content);

            this.UpdateChildrenLayout();
        }

        public void ShowPopup(View popupView, View presenter, PopupLocation location, float paddingX = 0, float paddingY = 0)
        {
            this.DismissPopup();
            this.popup = popupView;

            Constraint constraintX = null, constraintY = null, constraintW = null, constraintH = null;

            switch (location)
            {
                case PopupLocation.Bottom:
                    constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => parent.Y + presenter.Y + presenter.Height + paddingY);
                    break;
                case PopupLocation.Top:
                    constraintX = Constraint.RelativeToParent(parent => presenter.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => 
                        parent.Y + presenter.Y - this.popup.HeightRequest / 2 - paddingY);
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

            constraintW = Constraint.RelativeToParent(p => this.popup.WidthRequest);
            constraintH = Constraint.RelativeToParent(p => this.popup.HeightRequest);

            this.content.InputTransparent = true;
            this.Children.Add(
                this.popup,
                constraintX,
                constraintY,
                constraintW,
                constraintH
                );

            if (Device.OS == TargetPlatform.Android)
            {
                this.Children.Remove(this.content);
                this.Children.Add(this.content, () => this.Bounds);
            }

            //this.LowerChild(this.Content);

            this.UpdateChildrenLayout();
        }

        public void DismissPopup()
        {
            if (this.popup != null)
            {
                this.Children.Remove(popup);
                this.popup = null;
            }

            this.content.InputTransparent = false;
        }

        /// <summary>
        /// Popup location options when relative to another view
        /// </summary>
        public enum PopupLocation
        {
            /// <summary>
            /// Will show popup on top of the specified view
            /// </summary>
            Top,
            /// <summary>
            /// Will show popup below of the specified view
            /// </summary>
            Bottom,
            /// <summary>
            /// Will show popup left to the specified view
            /// </summary>
            //Left,
            /// <summary>
            /// Will show popup right of the specified view
            /// </summary>
            //Right
        }
    }
}
