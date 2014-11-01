using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP8.Controls;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]

namespace Xamarin.Forms.Labs.WP8.Controls
{
    using System.Windows.Media;
    using System.Windows.Shapes;
    using NativeSeparator = Microsoft.Phone.Controls.Separator;

    public class SeparatorRenderer : ViewRenderer<Separator, Path>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (this.Control == null)
            {
                this.Background = Xamarin.Forms.Color.Transparent.ToBrush();
                this.SetNativeControl(new Path());
            }

            this.SetProperties(this.Control);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            this.SetProperties(this.Control);
        }

        private void SetProperties(Path line)
        {
            var myLineSegment = new LineSegment()
            {
                Point = new System.Windows.Point(this.Element.Width, 0)
            };

            var myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myLineSegment);

            var myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(new PathFigure()
            {
                StartPoint = new System.Windows.Point(0, 0),
                Segments = myPathSegmentCollection
            });

            line.Stroke = this.Element.Color.ToBrush();
            line.StrokeDashArray = new System.Windows.Media.DoubleCollection();

            if (this.Element.StrokeType != StrokeType.Solid)
            {
                if (this.Element.StrokeType == StrokeType.Dashed)
                {
                    line.StrokeDashArray.Add(10);
                }
                line.StrokeDashArray.Add(2);
            }

            line.Data = new PathGeometry()
            {
                Figures = myPathFigureCollection
            };

            line.StrokeThickness = this.Element.Thickness;
        }
    }
}
