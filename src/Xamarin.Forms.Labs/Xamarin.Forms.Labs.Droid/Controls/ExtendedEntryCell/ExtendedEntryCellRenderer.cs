using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls.ExtendedEntryCell;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Text.Method;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.ExtendedEntryCell
{
    public class ExtendedEntryCellRenderer : EntryCellRenderer
    {
        protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore (item, convertView, parent, context);
            if (cell != null) {
            
                var textField = (cell as EntryCellView).EditText as TextView;
                
                if (textField != null && textField.TransformationMethod != PasswordTransformationMethod.Instance) {
                    textField.TransformationMethod = PasswordTransformationMethod.Instance;
                }
            }
            return cell;
        }
    }
}

