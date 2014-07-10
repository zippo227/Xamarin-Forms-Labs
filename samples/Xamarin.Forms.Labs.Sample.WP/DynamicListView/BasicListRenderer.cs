using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Sample.WP.DynamicListView;
using Xamarin.Forms.Labs.WP8.Controls;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace Xamarin.Forms.Labs.Sample.WP.DynamicListView
{
    public class BasicListRenderer : DynamicListViewRenderer<object>
    {
    }
}
