using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    public class WebImage : Image
    {
        public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create<WebImage, string>(p => p.ImageUrl, default(string));

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }
    }
}
