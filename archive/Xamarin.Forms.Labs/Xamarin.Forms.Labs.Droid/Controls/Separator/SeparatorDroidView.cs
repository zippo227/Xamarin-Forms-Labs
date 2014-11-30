using System;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Xamarin.Forms.Labs.Controls;
using Android.App;

namespace Xamarin.Forms.Labs
{
	public class SeparatorDroidView :Android.Views.View
	{



		public SeparatorDroidView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public SeparatorDroidView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public SeparatorDroidView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		private double _thickness;

		public double Thickness{
			set{
				_thickness = value;
				//this.Invalidate();
			}
			get{
				return _thickness;
			}
		}

		private double _spacingBefore;

		public double SpacingBefore{
			set{
				_spacingBefore = value;
				//this.Invalidate();
			}
			get{
				return _spacingBefore;
			}
		}

		private double _spacingAfter;

		public double SpacingAfter{
			set{
				_spacingAfter = value;
				//this.Invalidate();
			}
			get{
				return _spacingAfter;
			}
		}

		private Android.Graphics.Color _strokeColor;

		public Android.Graphics.Color StrokeColor{
			set{
				_strokeColor = value;
				//this.Invalidate();
			}
			get{
				return _strokeColor;
			}
		}


		private StrokeType _strokeType;

		public StrokeType StrokeType{
			set{
				_strokeType = value;
				//this.Invalidate();
			}
			get{
				return _strokeType;
			}
		}

		private Xamarin.Forms.Labs.Controls.SeparatorOrientation _orientation;

		public SeparatorOrientation Orientation{
			set{
				_orientation = value;
				this.Invalidate();
			}
			get{
				return _orientation;
			}
		}
		//Density measure
		float dm;

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw (canvas);

			var r = new Rect (0,0,canvas.Width,canvas.Height);
			float dAdjustedThicnkess = (float)this.Thickness * dm;
		

			var paint = new Paint()
			{
				Color = this.StrokeColor,
				StrokeWidth = dAdjustedThicnkess,
				AntiAlias = true
			
			};
			paint.SetStyle(Paint.Style.Stroke);
			switch(StrokeType) {
			case StrokeType.Dashed:
				paint.SetPathEffect(new DashPathEffect(new float[]{ 6*dm, 2*dm}, 0));
				break;
			case StrokeType.Dotted:
				paint.SetPathEffect(new DashPathEffect(new float[]{ dAdjustedThicnkess, dAdjustedThicnkess}, 0));
				break;
			default:

				break;
			}

			var desiredTotalSpacing = (SpacingAfter + SpacingBefore)*dm;
			float leftForSpacing = 0;
			float actualSpacingBefore = 0;
			float actualSpacingAfter = 0;
			if(Orientation == SeparatorOrientation.Horizontal){
				leftForSpacing = r.Height() - dAdjustedThicnkess;
			}else{
				leftForSpacing = r.Width() - dAdjustedThicnkess;
			}
			if(desiredTotalSpacing > 0) {
				float spacingCompressionRatio = (float)(leftForSpacing / desiredTotalSpacing) ;
				actualSpacingBefore = (float)SpacingBefore*dm * spacingCompressionRatio;
				actualSpacingAfter = (float)SpacingAfter*dm * spacingCompressionRatio;
			}else{
				actualSpacingBefore = 0;
				actualSpacingAfter = 0;
			}
			float thicknessOffset = (dAdjustedThicnkess)/2.0f;



			Path p = new Path();
			if(Orientation == SeparatorOrientation.Horizontal){
				p.MoveTo(0, actualSpacingBefore + thicknessOffset);
				p.LineTo(r.Width(), actualSpacingBefore + thicknessOffset);
			}else{
				p.MoveTo(actualSpacingBefore+thicknessOffset, 0);
				p.LineTo(actualSpacingBefore+thicknessOffset, r.Height());
			}
			canvas.DrawPath(p, paint);
		}



		private void Initialize()
		{
			dm = Application.Context.Resources.DisplayMetrics.Density;
		}
	}
}

