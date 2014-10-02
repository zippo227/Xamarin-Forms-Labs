using System;

namespace Xamarin.Forms.Labs
{
    public class ExtendedTableView : TableView
    {
        public event EventHandler<EventArgs> DataChanged;

        public void OnDataChanged()
        {
            var handler = this.DataChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}