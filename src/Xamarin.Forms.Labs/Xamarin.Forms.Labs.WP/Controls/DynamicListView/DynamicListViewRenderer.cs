using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Platform.WinPhone;

namespace Xamarin.Forms.Labs.WP8.Controls
{
    public class DynamicListViewRenderer<T> : ViewRenderer<DynamicListView<T>, LongListSelector>
    {
        private LongListSelector tableView;

        private const string Xaml = @"<DataTemplate
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                    <Grid>
                        <TextBlock Text='{Binding}' FontSize='40' />
                    </Grid>        
                </DataTemplate>";

        protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
        {
            base.OnElementChanged(e);

            if (this.tableView == null)
            {
                //var source = new DataSource(this);
                //this.tableView.ItemTemplate = new DataSource(this).ContentTemplate;
                
                this.tableView = new LongListSelector();
                this.tableView.SelectionChanged += (sender, args) =>
                    {
                        foreach (var item in args.AddedItems.OfType<T>())
                        {
                            this.Element.InvokeItemSelectedEvent(this, item);
                        }
                    };

                this.tableView.ItemTemplate = Template;

                this.SetNativeControl(this.tableView);
            }

            this.Unbind(e.OldElement);
            this.Bind(e.NewElement);
        }

        protected virtual System.Windows.DataTemplate Template
        {
            get
            {
                return (System.Windows.DataTemplate)XamlReader.Load(Xaml);
            }
        }

        protected virtual System.Windows.DataTemplate TemplateForItem(object item)
        {
            return this.Template;
        }

        private void Unbind(DynamicListView<T> oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= ElementPropertyChanged;
            }

            this.tableView.ItemsSource = null;
        }

        private void Bind(DynamicListView<T> newElement)
        {
            if (newElement != null)
            {
                this.tableView.ItemsSource = this.Element.Data;
                newElement.PropertyChanged += ElementPropertyChanged;
            }
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.tableView.ItemsSource = this.Element.Data;
            }
        }

        public class DataSource : ContentControl
        {
            private DynamicListViewRenderer<T> parent;
 
            public DataSource(DynamicListViewRenderer<T> parent)
            {
                this.parent = parent;
            }

            protected override void OnContentChanged(object oldContent, object newContent)
            {
                base.OnContentChanged(oldContent, newContent);

                if (newContent != null && (oldContent == null || oldContent.GetType() != newContent.GetType()))
                {
                    this.ContentTemplate = this.parent.TemplateForItem(newContent);
                }
            }
        }

        public class MyDataTemplate : System.Windows.DataTemplate
        {
            
        }
    }
}
