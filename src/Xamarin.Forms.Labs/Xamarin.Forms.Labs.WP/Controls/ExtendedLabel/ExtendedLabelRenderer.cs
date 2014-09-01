using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedLabel)Element;

            UpdateUi(view, Control);
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        private void UpdateUi(ExtendedLabel view, TextBlock control)
        {
            if (!string.IsNullOrEmpty(view.FontName))
            {
                string filename = view.FontName;
                //if no extension given then assume and add .ttf
                if (filename.LastIndexOf(".", System.StringComparison.Ordinal) != filename.Length - 4)
                {
                    filename = string.Format("{0}.ttf", filename);
                }

                control.FontFamily = new FontFamily(string.Format(@"\Assets\Fonts\{0}#{1}",filename, string.IsNullOrEmpty(view.FriendlyFontName) ? filename.Substring(0,filename.Length-4) : view.FriendlyFontName ));
                control.FontSize = (view.FontSize > 0) ? (float)view.FontSize : 12.0f;
            }

            if (view.IsUnderline)
            {
                control.TextDecorations = TextDecorations.Underline;
            }

            if (view.IsStrikeThrough)
            {
                
                //isn't perfect, but it's a start 
                var border = new Border { Height = 1, Width = this.Control.ActualWidth, Background = control.Foreground, HorizontalAlignment = HorizontalAlignment.Center };
                Canvas.SetTop(border, (this.Control.ActualHeight / 2) - 0.5);
                ((Xamarin.Forms.Platform.WinPhone.VisualElementRenderer<Xamarin.Forms.Label, System.Windows.Controls.TextBlock>)(this)).Children.Add(border);



                ////alternative single line method - STILL IN DEVELOPMENT
                
                //var strikeThroughGrid = new System.Windows.Controls.Grid();
                //strikeThroughGrid.VerticalAlignment = VerticalAlignment.Top;
                //strikeThroughGrid.HorizontalAlignment = HorizontalAlignment.Left;
                ////define rows
                //var colDef1 = new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) };
                //var colDef2 = new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) };
                //strikeThroughGrid.RowDefinitions.Add(colDef1);
                //strikeThroughGrid.RowDefinitions.Add(colDef2);

                ////add textblock
                //strikeThroughGrid.Children.Add(control);
                //System.Windows.Controls.Grid.SetRowSpan(control,2);

                ////add border
                //var strikethroughBorder = new Border{BorderThickness = new System.Windows.Thickness(0,0,0,2) , BorderBrush = control.Foreground , Padding = new System.Windows.Thickness(0,0,0,control.FontSize*0.3)};
                //strikeThroughGrid.Children.Add(strikethroughBorder);

            }
        }
    }
}

