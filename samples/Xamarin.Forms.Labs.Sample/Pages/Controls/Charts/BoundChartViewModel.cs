using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Labs.Charting.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.Charts
{
    public class BoundChartViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeColorYellowCommand { get; set; }
        public ICommand ChangeColorGreenCommand { get; set; }
        public ICommand ChangeColorWhiteCommand { get; set; }
        public ICommand ChangeSourceCommand { get; set; }

        public BoundChartViewModel()
        {
            ChangeColorYellowCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.Yellow;
            },
            (color) =>
            {
                return Color != Color.Yellow;
            });
            ChangeColorGreenCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.Green;
            },
            (color) =>
            {
                return Color != Color.Green;
            });
            ChangeColorWhiteCommand = new Command<string>((color) =>
            {
                this.Color = Xamarin.Forms.Color.White;
            },
            (color) =>
            {
                return Color != Color.White;
            });
            Color = Color.Yellow;

            ChangeSourceCommand = new Command(() =>
            {
                this.ChartData = getChartData();
            },
            () =>
            {
                return true;
            });

            this.ChartData = getChartData();
            
        }

        private ChartDataSet getChartData()
        {
            Random rnd = new Random();

            ChartDataTable table1 = new ChartDataTable();
            table1.Columns.Add(new ChartColumn("Month"));
            table1.Rows.Add(new object[] { "Jan", rnd.Next(0,100) });
            table1.Rows.Add(new object[] { "Feb", rnd.Next(0, 100) });
            table1.Rows.Add(new object[] { "March", rnd.Next(0, 100) });

            ChartDataTable table2 = new ChartDataTable();
            table2.Columns.Add(new ChartColumn("Month"));
            table2.Rows.Add(new object[] { "Jan", rnd.Next(0, 100) });
            table2.Rows.Add(new object[] { "Feb", rnd.Next(0, 100) });
            table2.Rows.Add(new object[] { "March", rnd.Next(0, 100) });

            ChartDataTable table3 = new ChartDataTable();
            table3.Columns.Add(new ChartColumn("Month"));
            table3.Rows.Add(new object[] { "Jan", rnd.Next(0, 100) });
            table3.Rows.Add(new object[] { "Feb", rnd.Next(0, 100) });
            table3.Rows.Add(new object[] { "March", rnd.Next(0, 100) });

            return new ChartDataSet()
            {
                Tables = new List<ChartDataTable>() { table1, table2, table3 }
            };
        }

        private Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if(_color != value)
                {
                    _color = value;
                    ((Command)this.ChangeColorYellowCommand).ChangeCanExecute();
                    ((Command)this.ChangeColorGreenCommand).ChangeCanExecute();
                    ((Command)this.ChangeColorWhiteCommand).ChangeCanExecute();
                    OnPropertyChanged("Color");
                }
            }
        }
        
        private ChartDataSet _chartData;
        public ChartDataSet ChartData
        {
            get
            {
                return _chartData;
            }
            set
            {
                _chartData = value;
                OnPropertyChanged("ChartData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }
    }
}
