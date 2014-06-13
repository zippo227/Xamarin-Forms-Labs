using System;
using Android.App;

namespace XForms.Toolkit
{
    public class Display : IDisplay
    {
        public Display()
        {
            var dm = Application.Context.Resources.DisplayMetrics;
            this.Height = dm.HeightPixels;
            this.Width = dm.WidthPixels;
            this.Xdpi = dm.Xdpi;
            this.Ydpi = dm.Ydpi;
        }

        #region IScreen implementation

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public double Xdpi
        {
            get;
            private set;
        }

        public double Ydpi
        {
            get;
            private set;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
        }
    }
}

