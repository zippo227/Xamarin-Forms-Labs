using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]

namespace XLabs.Forms.Controls
{
    using System.ComponentModel;
    using Extensions;

    public class ExtendedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var element = this.Element;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.VerticalAlignment = this.Element.VerticalAlignment.ToContentVerticalAlignment();
            this.Control.HorizontalAlignment = this.Element.HorizontalAlignment.ToContentHorizontalAlignment();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "VerticalAlignment":
                    this.Control.VerticalAlignment = this.Element.VerticalAlignment.ToContentVerticalAlignment();
                    break;
                case "HorizontalAlignment":
                    this.Control.HorizontalAlignment = this.Element.HorizontalAlignment.ToContentHorizontalAlignment();
                    break;
                default:
                    base.OnElementPropertyChanged(sender, e);
                    break;
            }
        }

        public new ExtendedButton Element
        {
            get
            {
                return base.Element as ExtendedButton;
            }
        }
    }
}