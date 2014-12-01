using Android.Util;

namespace Xamarin.Forms.Labs.Droid.Controls.Calendar
{
    public class Logr
    {
        public static void D(string message)
        {
#if DEBUG
            Log.Debug("Xamarin.Forms.Labs.Droid.Controls.Calendar", message);
#endif
        }

        public static void D(string message, params object[] args)
        {
#if DEBUG
            D(string.Format(message, args));
#endif
        }
    }
}