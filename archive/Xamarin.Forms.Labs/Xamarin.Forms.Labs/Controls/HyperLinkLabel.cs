using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
    public class HyperLinkLabel : Label
    {
        public static readonly BindableProperty SubjectProperty;
        public static readonly BindableProperty NavigateUriProperty;
        
        static HyperLinkLabel()
        {
            SubjectProperty = BindableProperty.Create("Subject", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);
            NavigateUriProperty = BindableProperty.Create("NavigateUri", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);          
        }
        public string Subject
        {
            get { return (string)base.GetValue(SubjectProperty); }
            set { base.SetValue(SubjectProperty, value); }
        }

        public string NavigateUri
        {
            get { return (string)base.GetValue(NavigateUriProperty); }
            set { base.SetValue(NavigateUriProperty, value); }
        }
    }
}
