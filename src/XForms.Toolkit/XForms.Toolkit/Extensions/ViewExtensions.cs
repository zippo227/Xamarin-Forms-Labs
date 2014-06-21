using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Extensions
{
    public static class ViewExtensions
    {
        private static IDisplay Display
        {
            get
            {
                return Resolver.Resolve<IDisplay>() ?? 
                    Resolver.Resolve<IDevice>().Display;
            }
        }

        private static double? widthInInches;
        private static double? heightInInches;

        private static double WidthInInches
        {
            get
            {
                if (widthInInches.HasValue)
                {
                    return widthInInches.Value;
                }
                
                widthInInches = Display.WidthRequestInInches(1);
                return widthInInches.Value;
            }
        }

        private static double HeightInInches
        {
            get
            {
                if (widthInInches.HasValue)
                {
                    return heightInInches.Value;
                }

                heightInInches = Display.WidthRequestInInches(1);
                return heightInInches.Value;
            }
        }

        public static double GetWidthRequestInInches(this View view)
        {
            return view.WidthRequest / WidthInInches;
        }

        public static void SetWidthRequestInInches(this View view, double inches)
        {
            view.WidthRequest = inches * WidthInInches;
        }

        public static double GetHeightRequestInInches(this View view)
        {
            return view.HeightRequest / HeightInInches;
        }

        public static void SetHeightRequestInInches(this View view, double inches)
        {
            view.HeightRequest = inches * HeightInInches;
        }
    }
}
