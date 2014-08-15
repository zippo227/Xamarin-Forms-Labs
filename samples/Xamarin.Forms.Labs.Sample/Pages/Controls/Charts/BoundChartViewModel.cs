using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.Charts
{
    public class BoundChartViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeColorYellowCommand { get; set; }
        public ICommand ChangeColorGreenCommand { get; set; }
        public ICommand ChangeColorWhiteCommand { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }
    }
}
