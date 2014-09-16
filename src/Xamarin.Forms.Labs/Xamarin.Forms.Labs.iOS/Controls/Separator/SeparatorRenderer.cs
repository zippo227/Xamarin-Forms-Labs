using System;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]
namespace Xamarin.Forms.Labs.iOS
{
	public class SeparatorRenderer : ViewRenderer<Separator,UISeparator>
	{
		public SeparatorRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
		{
			base.OnElementChanged(e);
			var separator = new UISeparator(this.Bounds);
			this.BackgroundColor = Color.Transparent.ToUIColor();
			this.SetNativeControl(separator);
			this.SetProperties ();
			this.Element.PropertyChanged += this.OnPropertyChanged;


		}

	

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetProperties ();
		}

		private void SetProperties()
		{
			var separator = Control;
			separator.Thickness = Element.Thickness;
			separator.StrokeColor = Element.Color.ToUIColor();
			separator.StrokeType = Element.StrokeType;
			separator.Orientation = Element.Orientation;
			separator.SpacingBefore = Element.SpacingBefore;
			separator.SpacingAfter = Element.SpacingAfter;
		}
	}
}

