using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Xamarin.Forms.Labs.WP8.Controls.SensorBar
{
    public partial class SensorBarControl : UserControl
    {
        private System.Windows.Media.Color positiveColor = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
        private System.Windows.Media.Color negativeColor = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
        private double limit = 1;
        private double currentValue = 0;

        public SensorBarControl()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public double CurrentValue
        {
            get { return this.currentValue; }
            set
            {
                if (Math.Abs(value) <= this.Limit)
                {
                    this.currentValue = value;

                }
            }
        }

        [Browsable(true)]
        public double Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }

        [Browsable(true)]
        public System.Windows.Media.Color PositiveColor
        {
            get { return this.negativeColor; }
            set 
            {
                this.negativeColor = value;
                this.positiveRectangle.Fill = new SolidColorBrush(value); 
            }
        }

        [ Browsable(true)]
        public System.Windows.Media.Color NegativeColor
        {
            get { return this.negativeColor; }
            set 
            { 
                this.negativeColor = value;
                this.negativeRectangle.Fill = new SolidColorBrush(value); 
            }
        }
    }
}
