using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Mvvm
{
    public class NavigationView : NavigationPage
    {
        private const string CurrentPagePropertyName = "CurrentPage";

        private Page _previousPage;

        public NavigationView()
        {
        }

        public NavigationView(Page root)
            : base(root)
        {
        }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (propertyName == CurrentPagePropertyName)
            {
                _previousPage = CurrentPage;
            }

            base.OnPropertyChanging(propertyName);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == CurrentPagePropertyName)
            {
                if (CurrentPage != null)
                {
                    CurrentPage.Appearing += OnCurrentPageAppearing;
                    CurrentPage.Disappearing += OnCurrentPageDisappearing;
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        private void OnCurrentPageAppearing(object sender, EventArgs e)
        {
            Page view = (Page)sender;

            view.Appearing -= OnCurrentPageAppearing;

            var navigationAware = AsNavigationAware(view);
            if (navigationAware != null)
            {
                navigationAware.OnNavigatingTo(_previousPage);
            }

            _previousPage = null;
        }

        private void OnCurrentPageDisappearing(object sender, EventArgs e)
        {
            Page view = (Page)sender;

            view.Disappearing -= OnCurrentPageDisappearing;

            var navigationAware = AsNavigationAware(view);
            if (navigationAware != null)
            {
                navigationAware.OnNavigatingFrom(CurrentPage);
            }
        }

        private INavigationAware AsNavigationAware(VisualElement element)
        {
            var navigationAware = element.BindingContext as INavigationAware;
            if (navigationAware == null)
            {
                navigationAware = element as INavigationAware;
            }

            return navigationAware;
        }
    }
}
