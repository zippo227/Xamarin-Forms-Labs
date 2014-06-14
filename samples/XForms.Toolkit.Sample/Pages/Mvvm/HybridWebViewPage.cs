using System;
using Xamarin.Forms;
using XForms.Toolkit.Mvvm;

namespace XForms.Toolkit.Sample
{
    public class HybridWebViewPage : BaseView
    {
        public HybridWebViewPage()
        {
            var viewModel = ChartViewModel.Dummy;

            this.BindingContext = viewModel;
        }
    }
}

