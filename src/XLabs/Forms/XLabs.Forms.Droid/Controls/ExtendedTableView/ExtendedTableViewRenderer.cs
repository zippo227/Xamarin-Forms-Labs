using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls.ExtendedTableView;

[assembly: ExportRenderer(typeof(ExtendedTableView), typeof(ExtendedTableViewRenderer))]
namespace XLabs.Forms.Controls.ExtendedTableView
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