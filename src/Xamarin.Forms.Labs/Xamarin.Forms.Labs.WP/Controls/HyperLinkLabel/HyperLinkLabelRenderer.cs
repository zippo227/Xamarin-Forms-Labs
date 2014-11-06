
using Xamarin.Forms.Labs.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    public class HyperLinkLabelRenderer : ViewRenderer<HyperLinkLabel, System.Windows.Controls.HyperlinkButton>
    {
        private bool fontApplied;

        protected override void OnElementChanged(ElementChangedEventArgs<HyperLinkLabel> e)
        {
            base.OnElementChanged(e);
            var element = new System.Windows.Controls.HyperlinkButton();
            element.Click += (sender, args)=> 
            {
                if (base.Element.NavigateUri.Contains("@"))
                {
                    EmailComposeTask emailComposeTask = new EmailComposeTask() { Subject = base.Element.Subject, To = "mailto:" + base.Element.NavigateUri};
                    emailComposeTask.Show();
                }
                else if (base.Element.NavigateUri.Contains("www.") || base.Element.NavigateUri.Contains("http:"))
                {
                    Uri uri = base.Element.NavigateUri.StartsWith("http:") ? new Uri(base.Element.NavigateUri) : new Uri(@"http://" + base.Element.NavigateUri);

                    WebBrowserTask webBrowserTask = new WebBrowserTask() { Uri = uri };
                    webBrowserTask.Show();
                }
            };
         
            base.SetNativeControl(element);
            this.UpdateContent();
            if (base.Element.BackgroundColor != Color.Default)
            {
                this.UpdateBackground();
            }
            if (base.Element.TextColor != Color.Default)
            {
                this.UpdateTextColor();
            }

            this.UpdateFont();
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if ((e.PropertyName == HyperLinkLabel.TextProperty.PropertyName))
            {
                this.UpdateContent();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                this.UpdateBackground();
            }
            else if (e.PropertyName == HyperLinkLabel.TextColorProperty.PropertyName)
            {
                this.UpdateTextColor();
            }
            else if (e.PropertyName == HyperLinkLabel.FontProperty.PropertyName)
            {
                this.UpdateFont();
            }           
        }
        private void UpdateBackground()
        {
          //  base.Control.Background = (base.Element.BackgroundColor != Color.Default) ? new System.Windows.Media.SolidColorBrush(base.Element.BackgroundColor.ToMediaColor(): ((System.Windows.Media.Brush)Application.Current.Resources["PhoneBackgroundBrush"]);
        }

        private void UpdateContent()
        {
            base.Control.Content = base.Element.Text;
        }

        private void UpdateFont()
        {
            if (((base.Control != null) && (base.Element != null)) && ((base.Element.Font != Font.Default) || this.fontApplied))
            {
                Font font = (base.Element.Font == Font.Default) ? Font.SystemFontOfSize(NamedSize.Medium) : base.Element.Font;
                base.Control.ApplyFont(font);
                this.fontApplied = true;
            }
        }

        private void UpdateTextColor()
        {
       //     base.Control.Foreground = (base.Element.TextColor != Color.Default) ? new System.Windows.Media.SolidColorBrush(base.Element.TextColor.ToMediaColor(): ((System.Windows.Media.Brush)Application.Current.Resources["PhoneForegroundBrush"]);         
        }
    }
}

