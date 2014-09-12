using System;
using System.Collections;

namespace Xamarin.Forms.Labs.Controls
{
    public class ImageGallery : View
    {
        public ImageGallery ()
        {

        }
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create ("ItemsSource", typeof(IEnumerable), typeof(ImageGallery), null, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create ("ItemTemplate", typeof(DataTemplate), typeof(ImageGallery), null, BindingMode.OneWay, null, null, null, null);

		public static readonly BindableProperty AspectProperty = BindableProperty.Create ("AspectProperty", typeof(Aspect), typeof(ImageGallery), Aspect.AspectFill, BindingMode.OneWay, null, null, null, null);


        // Properties
        //
        public IEnumerable ItemsSource {
            get {
                return (IEnumerable)base.GetValue (ImageGallery.ItemsSourceProperty);
            }
            set {
                base.SetValue (ImageGallery.ItemsSourceProperty, value);
            }
        }

        public DataTemplate ItemTemplate {
            get {
                return (DataTemplate)base.GetValue (ImageGallery.ItemTemplateProperty);
            }
            set {
                base.SetValue (ImageGallery.ItemTemplateProperty, value);
            }
        }


		public Aspect Aspect {
			get {
				return (Aspect)base.GetValue(Image.AspectProperty);
			}
			set {
				base.SetValue(Image.AspectProperty, value);
			}
		}

    }
}

