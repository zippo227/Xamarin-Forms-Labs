using System.Collections.Specialized;
using System.Net;
using Android.Graphics;
using Android.Webkit;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls.ImageGallery;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace XLabs.Forms.Controls.ImageGallery
{
	public class ImageGalleryRenderer : ViewRenderer<Xamarin.Forms.Labs.Controls.ImageGallery,Gallery>
	{

		private Gallery gallery;
		private DataSource source;

		private DataSource Source
		{
			get
			{
				return source ?? (source = new DataSource(this));
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ImageGallery> e)
		{
			base.OnElementChanged(e);

			if(e.OldElement == null)
			{
				gallery = new Gallery(Context);
				SetNativeControl(gallery);
			}
			Bind(e.NewElement);
			gallery.Adapter = Source;
		}

		protected virtual Android.Views.View GetView(string item, Android.Views.View convertView, Android.Views.ViewGroup parent, int position)
		{
			var imageView = convertView as ImageView ?? new ImageView(parent.Context);

			if (IsValidUrl(item))
				imageView.SetImageBitmap(GetBitmapFromUrl(item));
			else
				imageView.SetImageResource(Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(item), "drawable", Context.PackageName));

			imageView.SetScaleType(ImageView.ScaleType.FitXy);
			return imageView;
		}


		private void Bind(ImageGallery newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;
				((INotifyCollectionChanged)newElement.ItemsSource).CollectionChanged += DataCollectionChanged;
			}
		}

		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged -= DataCollectionChanged;
		}

		private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.Source.NotifyDataSetChanged();
		}

		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged += DataCollectionChanged;
		}

		private bool IsValidUrl(string urlString)
		{
			return URLUtil.IsValidUrl(urlString);
		}

		private Bitmap GetBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
			}

			return imageBitmap;
		}

		private class DataSource : BaseAdapter
		{
			ImageGalleryRenderer galleryRenderer;

			public DataSource(ImageGalleryRenderer galleryRenderer)
			{
				this.galleryRenderer = galleryRenderer;
			}

			#region abstract members of BaseAdapter
			public override Java.Lang.Object GetItem(int position)
			{
				return position;
			}

			public override long GetItemId(int position)
			{
				return position;
			}

			public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
			{
				return galleryRenderer.GetView(galleryRenderer.Element.ItemsSource.Cast<string>().ToArray()[position], convertView, parent, position);
			}

			public override int Count
			{
				get
				{
					return galleryRenderer.Element.ItemsSource.Cast<string>().Count();
				}
			}
			#endregion
		}
	}
}