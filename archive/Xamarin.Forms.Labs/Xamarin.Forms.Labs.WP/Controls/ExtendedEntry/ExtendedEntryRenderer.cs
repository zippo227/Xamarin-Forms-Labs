using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP8;
using Xamarin.Forms.Platform.WinPhone;


[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Xamarin.Forms.Labs.WP8
{
    public class ExtendedEntryRenderer :  EntryRenderer
    {
        private PasswordBox _thisPasswordBox;
        private PhoneTextBox _thisPhoneTextBox;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;

            //Because Xamarin EntryRenderer switches the type of control we need to find the right one
            if (view.IsPassword)
            {
                _thisPasswordBox = (PasswordBox) Control.Children.FirstOrDefault(c => c is PasswordBox);
            }
            else
            {
                _thisPhoneTextBox = (PhoneTextBox) Control.Children.FirstOrDefault(c => c is PhoneTextBox);
            }

            SetFont(view);
            SetTextAlignment(view);
            SetBorder(view);
            SetPlaceholderTextColor(view);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if(e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);

        }

        private void SetBorder(ExtendedEntry view)
        {
            if (view.IsPassword && _thisPasswordBox != null)
            {
                _thisPasswordBox.BorderThickness = view.HasBorder ? new System.Windows.Thickness(2) :  new System.Windows.Thickness(0);
            }
            else if (!view.IsPassword && _thisPhoneTextBox != null)
            {
                _thisPhoneTextBox.BorderThickness = view.HasBorder ? new System.Windows.Thickness(2) : new System.Windows.Thickness(0);
            }
        }

        private void SetTextAlignment(ExtendedEntry view)
        {
            if (view.IsPassword && _thisPasswordBox != null)
            {
                switch (view.XAlign)
                {
                    //NotCurrentlySupported: Text alaignement not available on Windows Phone for Password Entry
                }                
            }
            else if (!view.IsPassword && _thisPhoneTextBox != null)
            {
                switch (view.XAlign)
                {
                    case TextAlignment.Center:
                        _thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Center;
                        break;
                    case TextAlignment.End:
                        _thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Right;
                        break;
                    case TextAlignment.Start:
                        _thisPhoneTextBox.TextAlignment = System.Windows.TextAlignment.Left;
                        break;
                }              
            }
        }

        private void SetFont(ExtendedEntry view)
        {
            if (view.Font != Font.Default)
                if (view.IsPassword)
                {
                    if (_thisPasswordBox != null)
                    {
                        _thisPasswordBox.FontSize = view.Font.GetHeight();
                    }
                }
                else
                {
                    if (_thisPhoneTextBox != null)
                    {
                        _thisPhoneTextBox.FontSize = view.Font.GetHeight();
                    }
                }
        }

        private void SetPlaceholderTextColor(ExtendedEntry view)
        {
            //the EntryRenderer renders two child controls. A PhoneTextBox or PasswordBox
            // depending on the IsPassword property of the Xamarin form control

            if (view.IsPassword)
            {
                //NotCurrentlySupported: Placeholder text color is not supported on Windows Phone Password control
            }
            else
            {
                if (view.PlaceholderTextColor != Color.Default && _thisPhoneTextBox != null)
                {
                    var hintStyle = new Style(typeof(ContentControl));
                    hintStyle.Setters.Add(
                        new Setter(
                            System.Windows.Controls.Control.ForegroundProperty,
                            view.PlaceholderTextColor.ToBrush())
                        );
                    _thisPhoneTextBox.HintStyle = hintStyle;
                }
            }        
        }
    }
}