using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// A view that renders its content based on a data template. Typical usage is to either set an explicit 
    /// <see cref="BindableObject.BindingContext"/> on this element or use an inhereted one, then set a display.
    /// </summary>
    public class ContentPresenter : ContentView
    {
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(ContentPresenter), null, propertyChanged: OnItemTemplateChanged);

        private static void OnItemTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var cp = (ContentPresenter)bindable;

            var template = cp.ItemTemplate;
            if (template != null)
            {
                var content = (View)template.CreateContent();
                cp.Content = content;
            }
            else
            {
                cp.Content = null;
            }
        }

        /// <summary>
        /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

    }
}
