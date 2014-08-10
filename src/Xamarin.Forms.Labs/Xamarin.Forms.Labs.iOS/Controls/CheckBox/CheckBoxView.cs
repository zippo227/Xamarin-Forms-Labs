using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    [Register("CheckBoxView")]
    public class CheckBoxView : UIButton
    {
        public CheckBoxView()
        {
            Initialize();
        }

        public CheckBoxView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        public string CheckedTitle
        {
            set
            {
                this.SetTitle(value, UIControlState.Selected);
            }
        }

        public string UncheckedTitle
        {
            set
            {
                this.SetTitle(value, UIControlState.Normal);
            }
        }

        public bool Checked
        {
            set { this.Selected = value; }
            get { return this.Selected; }
        }

        void Initialize()
        {
            this.AdjustEdgeInsets();
            this.ApplyStyle();

            this.TouchUpInside += (sender, args) => this.Selected = !this.Selected;
        }

        void AdjustEdgeInsets()
        {
            const float inset = 8f;

            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            this.ImageEdgeInsets = new UIEdgeInsets(0f, inset, 0f, 0f);
            this.TitleEdgeInsets = new UIEdgeInsets(0f, inset * 2, 0f, 0f);
        }

        void ApplyStyle()
        {
            this.SetImage(UIImage.FromBundle("Images/CheckBox/checked_checkbox.png"), UIControlState.Selected);
            this.SetImage(UIImage.FromBundle("Images/CheckBox/unchecked_checkbox.png"), UIControlState.Normal);
        }
    }
}