
using Android.App;
using Android.Content;
using AApplication = Android.App.Application;

namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// Intent filter extensions.
    /// </summary>
    public static class IntentFilterExtensions
    {
        /// <summary>
        /// Gets a single result for the intent filter using <see cref="Application.Context"/>
        /// </summary>
        /// <param name="intentFilter">Intent filter</param>
        /// <returns>An intent result, null if not successful</returns>
        public static Intent RegisterReceiver(this IntentFilter intentFilter)
        {
            return AApplication.Context.RegisterReceiver(null, intentFilter);
        }
    }
}