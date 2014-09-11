using System;
using Xamarin.Forms;
namespace Xamarin.Forms.Labs.Controls
{
	public enum StrokeType{
		Solid,
		Dotted,
		Dashed
	}
	public enum SeparatorOrientation{
		Vertical,
		Horizontal
	}

	public class Separator : View
	{

		/**
		 * Orientation property
		 */
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(SeparatorOrientation), typeof(Separator), SeparatorOrientation.Horizontal, BindingMode.OneWay, null, null, null, null);

		//Sorry I've implemented just horizontal implementaiton for now
		public SeparatorOrientation Orientation {
			get {
				return (SeparatorOrientation)base.GetValue(Separator.OrientationProperty);
			}

			private set {
				base.SetValue(Separator.OrientationProperty, value);
			}
		}

		/**
		 * Color property
		 */
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Separator), Color.Default, BindingMode.OneWay, null, null, null, null);


		public Color Color {
			get {
				return (Color)base.GetValue(Separator.ColorProperty);
			}
			set {
				base.SetValue(Separator.ColorProperty, value);
			}
		}


		/**
		 * Spacing property
		 */

		public static readonly BindableProperty SpacingBeforeProperty = BindableProperty.Create("SpacingBefore", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);

		/**
		 * Spacing property
		 */
		public double SpacingBefore {
			get {
				return (double)base.GetValue(Separator.SpacingBeforeProperty);
			}
			set {
				base.SetValue(Separator.SpacingBeforeProperty, value);
			}
		}

		/**
		 * Spacing property
		 */
		public static readonly BindableProperty SpacingAfterProperty = BindableProperty.Create("SpacingAfter", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);

		/**
		 * Spacing property
		 */
		public double SpacingAfter {
			get {
				return (double)base.GetValue(Separator.SpacingAfterProperty);
			}
			set {
				base.SetValue(Separator.SpacingAfterProperty, value);
			}
		}

		/**
		 * Spacing property
		 */
		public static readonly BindableProperty ThicknessProperty = BindableProperty.Create("Thickness", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);


		/**
		 * Spacing property
		 */

		public double Thickness {
			get {
				return (double)base.GetValue(Separator.ThicknessProperty);
			}
			set {
				base.SetValue(Separator.ThicknessProperty, value);
			}
		}


		/**
		 * Spacing property
		 */
		public static readonly BindableProperty StrokeTypeProperty = BindableProperty.Create("StrokeType", typeof(StrokeType), typeof(Separator), StrokeType.Solid, BindingMode.OneWay, null, null, null, null);

		/**
		 * Spacing property
		 */
		public StrokeType StrokeType {
			get {
				return (StrokeType)base.GetValue(Separator.StrokeTypeProperty);
			}
			set {
				base.SetValue(Separator.StrokeTypeProperty, value);
			}
		}

		public Separator()
		{

			UpdateRequestedSize();


		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			UpdateRequestedSize();
		}


		private void UpdateRequestedSize(){
			var minSize = Thickness;
			var optimalSize = SpacingBefore + Thickness + SpacingAfter;
			if(Orientation == SeparatorOrientation.Horizontal){
				MinimumHeightRequest = minSize;
				HeightRequest = optimalSize;
				HorizontalOptions = LayoutOptions.FillAndExpand;
			}else{
				MinimumWidthRequest = minSize;
				WidthRequest = optimalSize;
				VerticalOptions = LayoutOptions.FillAndExpand;
			}
		}
	}
}

