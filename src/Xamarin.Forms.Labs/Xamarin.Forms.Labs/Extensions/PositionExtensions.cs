using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services.Geolocation;

namespace Xamarin.Forms.Labs
{
    public static class PositionExtensions
    {
        public static Uri ToUri(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "geo:{0},{1}", position.Latitude, position.Longitude));
        }

        public static Uri ToBingMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://www.bing.com/maps/?q={0},{1}", position.Latitude, position.Longitude));
            //return new Uri(string.Format(new CultureInfo("en-US"), "maps:{0} {1}", position.Latitude, position.Longitude));
        }

        public static Uri ToGoogleMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://maps.google.com/?q={0},{1}", position.Latitude, position.Longitude));
        }

        public static Uri ToAppleMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://maps.apple.com/?q={0},{1}", position.Latitude, position.Longitude));
        }

        public static Uri DriveToLink(this Position position, string destination = "Driving instructions")
        {
            return new Uri(string.Format(
                new CultureInfo("en-US"), 
                "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                position.Latitude,
                position.Longitude,
                destination
                ));
        }
    }
}
