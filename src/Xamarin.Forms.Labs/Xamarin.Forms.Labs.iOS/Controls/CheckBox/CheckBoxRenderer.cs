using System;
using System.ComponentModel;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    /// <summary>
    /// The check box renderer for iOS.
    /// </summary>
    public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            BackgroundColor = Element.BackgroundColor.ToUIColor();

            if (Control == null)
            {
                var checkBox = new CheckBoxView(Bounds);
                checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

                SetNativeControl(checkBox);
            }

            UpdateFont();
            

            Control.LineBreakMode = UILineBreakMode.CharacterWrap;
            Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
            Control.CheckedTitle = string.IsNullOrEmpty(e.NewElement.CheckedText) ? e.NewElement.DefaultText : e.NewElement.CheckedText;
            Control.UncheckedTitle = string.IsNullOrEmpty(e.NewElement.UncheckedText) ? e.NewElement.DefaultText : e.NewElement.UncheckedText;
            Control.Checked = e.NewElement.Checked;
            Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Normal);
            Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Selected);

            // All of the things that I have tried to do to set the height.
            //Control.TitleLabel.Lines = 0;
            //Control.TitleLabel.AdjustsFontSizeToFitWidth = true;
            //Control.SizeThatFits(Control.TitleLabel.Bounds);
            //Control.SizeToFit();
            //Control.AutosizesSubviews = true;
            //Control.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
            //Control.Bounds = Control.TitleLabel.Bounds;

            Element.PropertyChanged += ElementOnPropertyChanged;

            //ResizeText();
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    ResizeText();
                    break;
                case "TextColor":
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Normal);
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Selected);
                    break;
                case "CheckedText":
                    Control.CheckedTitle = string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText;
                    ResizeText();
                    break;
                case "UncheckedText":
                    Control.UncheckedTitle = string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;
                    ResizeText();
                    break;
                //case "Height":
                //    Control.Bounds = Control.TitleLabel.Bounds;
                //    break;
                case "FontSize":
                    UpdateFont();
                    ResizeText();
                    break;
                case "FontName":
                    UpdateFont();
                    ResizeText();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                    break;
            }
        }

        private void ResizeText()
        {
            var text = this.Element.Checked ? string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText :
                string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;

            var bounds = this.Control.Bounds;

            var width = this.Control.TitleLabel.Bounds.Width;

            var height = text.StringHeight(this.Control.Font, width);

            var minHeight = string.Empty.StringHeight(this.Control.Font, width);

            if (bounds.Height < height)
            {
                bounds.Height = height + bounds.Height - minHeight;
                this.Control.Bounds = bounds;
                this.Element.HeightRequest = bounds.Height;
            }
            //var extra = bounds.Height - minHeight;
            

            //if (Math.Abs(bounds.Height - height + extra) > .1)
            //{
            //    bounds.Height = height + extra;
            //}
            

            

            
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Control.Checked = eventArgs.Value;
            });
        }

        private void UpdateFont()
        {
            if (string.IsNullOrEmpty(Element.FontName))
            {
                return;
            }

            var font = UIFont.FromName(Element.FontName, (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f);

            if (font != null)
            {
                Control.Font = font;
            }
        }
    }
}