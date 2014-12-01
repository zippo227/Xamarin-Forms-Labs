using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedTableView), typeof(ExtendedTableViewRenderer))]
namespace Xamarin.Forms.Labs.Droid
{
    public class ExtendedTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                ((ExtendedTableView)e.NewElement).DataChanged += (object sender, EventArgs args) =>
                {
                    ((TableViewModelRenderer)Control.Adapter).NotifyDataSetChanged();
                };
            }
        }
    }
}