using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    using Xamarin.Forms.Labs.Sample.ViewModel;

    public partial class GestureSample  : ContentPage
    {
        public GestureSample()
        {
            InitializeComponent();
            BindingContext = new GestureSampleVM();
        }
    }
}
