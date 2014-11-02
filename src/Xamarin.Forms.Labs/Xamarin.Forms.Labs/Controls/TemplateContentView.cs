using Xamarin.Forms.Labs.Exceptions;

namespace Xamarin.Forms.Labs.Controls
{
    public class TemplateContentView<T> : ContentView
    {
        #region Bindable Properties
        public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create<TemplateContentView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector));
        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create<TemplateContentView<T>, T>(x => x.ViewModel,default(T),BindingMode.OneWay,null,ViewModelChanged);

        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        public T ViewModel
        {
            get { return (T)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty,value);}
        }

        private static void ViewModelChanged(BindableObject bindable, T oldValue, T newValue)
        {
            var layout = bindable as TemplateContentView<T>;
            if(layout==null)
                throw new InvalidBindableException(bindable,typeof(TemplateContentView<T>));
            layout.ViewModelChangedImpl(newValue);
        }
        #endregion


        private void ViewModelChangedImpl(T newvalue)
        {
            var newchild = TemplateSelector.ViewFor(newvalue);
            //Verify that newchild is a contentview
            Content = newchild;
            InvalidateLayout();
        }
    }
}