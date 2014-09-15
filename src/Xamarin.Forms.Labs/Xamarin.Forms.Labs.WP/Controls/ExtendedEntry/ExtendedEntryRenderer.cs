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
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;

            SetFont(view);
            SetPlaceholderTextColor(view);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont((ExtendedEntry)Element);

			if(e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor((ExtendedEntry)Element);

        }

        private void SetFont(ExtendedEntry view)
        {
            if (view.Font != Font.Default)
                if (view.IsPassword)
                {
                    var passwordBox = (PasswordBox)Control.Children.FirstOrDefault(c => c is PasswordBox);
                    if (passwordBox != null)
                    {
                        passwordBox.FontSize = view.Font.GetHeight();
                    }
                }
                else
                {
                    var phoneTextBox = (PhoneTextBox)Control.Children.FirstOrDefault(c => c is PhoneTextBox);
                    if (phoneTextBox != null)
                    {
                        phoneTextBox.FontSize = view.Font.GetHeight();
                    }
                }
        }

        private void SetPlaceholderTextColor(ExtendedEntry view)
        {
            //the EntryRenderer renders two child controls. A PhoneTextBox or PasswordBox
            // depending on the IsPassword property of the Xamarin form control

            if (view.IsPassword)
            {
                //PasswordBox passwordBox = (PasswordBox)control.Children.FirstOrDefault(c => c is PasswordBox);
                //if (view.PlaceholderTextColor != Color.Default)
                //    passwordBox.
                //    editText.SetHintTextColor(e.PlaceholderTextColor.ToAndroid());

            }
            else
            {
                var phoneTextBox = (PhoneTextBox)Control.Children.FirstOrDefault(c => c is PhoneTextBox);
                if (view.PlaceholderTextColor != Color.Default && phoneTextBox != null)
                {
                    var hintStyle = new Style(typeof(PhoneTextBox));
                    hintStyle.Setters.Add(
                        new Setter(
                            System.Windows.Controls.Control.ForegroundProperty,
                            view.PlaceholderTextColor.ToBrush())
                        );
                    phoneTextBox.HintStyle = hintStyle;
                }
            }        
        }
    }
}