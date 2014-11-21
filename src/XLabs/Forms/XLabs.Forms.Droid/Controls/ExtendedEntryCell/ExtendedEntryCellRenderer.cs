using Android.Content;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls.ExtendedEntryCell;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace XLabs.Forms.Controls.ExtendedEntryCell
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

