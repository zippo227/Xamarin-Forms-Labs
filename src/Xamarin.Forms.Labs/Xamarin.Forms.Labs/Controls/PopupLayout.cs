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

        public void DismissPopup()
        {
            if (this.popup != null)
            {
                this.Children.Remove(popup);
                this.popup = null;
            }

            this.content.InputTransparent = false;
        }
    }
}
