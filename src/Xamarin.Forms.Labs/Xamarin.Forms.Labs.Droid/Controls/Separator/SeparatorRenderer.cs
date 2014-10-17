using System;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Droid;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]

namespace Xamarin.Forms.Labs.Droid
{
	public class SeparatorRenderer : ViewRenderer<Separator, SeparatorDroidView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement == null)
			{
				return;
			}

			if (this.Control == null)
			{
				this.SetNativeControl(new SeparatorDroidView(this.Context));
			}

			this.SetProperties();
		}


		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			this.SetProperties();
		}

		private void SetProperties()
		{
			Control.SpacingBefore = Element.SpacingBefore;
			Control.SpacingAfter = Element.SpacingAfter;
			Control.Thickness = Element.Thickness;
			Control.StrokeColor = Element.Color.ToAndroid();
			Control.StrokeType = Element.StrokeType;
			Control.Orientation = Element.Orientation;

			this.Control.Invalidate();
		}
	}
}

