using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;
using System.Linq;

namespace Xamarin.Forms.Labs.Sample
{	
    public partial class CanvasWebHybrid : BaseView
	{	
		public CanvasWebHybrid ()
		{
			InitializeComponent ();

            this.hybridWebView.RegisterCallback("dataCallback", t =>
                System.Diagnostics.Debug.WriteLine(t)
            );

            var model = ChartViewModel.Dummy;


            this.BindingContext = model;

            model.PropertyChanged += HandlePropertyChanged;

            model.DataPoints.CollectionChanged += HandleCollectionChanged;

            foreach (var datapoint in model.DataPoints)
            {
                datapoint.PropertyChanged += HandlePropertyChanged;
            }
		}

        void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
//            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
//            {
                foreach (var datapoint in e.NewItems.OfType<DataPoint>())
                {
                    datapoint.PropertyChanged += HandlePropertyChanged;
                }
//            } 
//            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
//            {
                foreach (var datapoint in e.OldItems.OfType<DataPoint>())
                {
                    datapoint.PropertyChanged -= HandlePropertyChanged;
                }
//            }
            
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.hybridWebView.CallJsFunction ("onViewModelData", this.BindingContext);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.hybridWebView.LoadFromContent("HTML/home.html");
        }
	}
}

