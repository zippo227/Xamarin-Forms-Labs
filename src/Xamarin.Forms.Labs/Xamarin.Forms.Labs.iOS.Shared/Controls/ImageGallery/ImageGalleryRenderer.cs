using System;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.iOS.Controls.ImageGallery;
using System.Collections.ObjectModel;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class ImageGalleryRenderer : ViewRenderer<Xamarin.Forms.Labs.Controls.ImageGallery,ImageGalleryView>
    {
        public ImageGalleryRenderer ()
        {


        }

        protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Labs.Controls.ImageGallery> e)
        {
            base.OnElementChanged (e);

            var imageGalleryView = new ImageGalleryView (e.NewElement.ItemsSource as ObservableCollection<string>);
            this.Bind (e.NewElement);
            base.SetNativeControl(imageGalleryView);

        }
        private void Bind(Xamarin.Forms.Labs.Controls.ImageGallery newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;

            }
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {

            }
        }
        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {

            }
        }

    }
}