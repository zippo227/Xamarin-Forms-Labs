

using Android.Content;
using AApplication = Android.App.Application;

// Analysis disable CheckNamespace
namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// Broadcast receiver extensions.
    /// </summary>
    public static class BroadcastReceiverExtensions
    {
        /// <summary>
        /// Registers the receiver using <see cref="Application.Context"/>.
        /// </summary>
        /// <returns>The receiver intent.</returns>
        /// <param name="receiver">Receiver.</param>
        /// <param name="intentFilter">Intent filter.</param>
        public static Intent RegisterReceiver(this BroadcastReceiver receiver, IntentFilter intentFilter)
        {
            return AApplication.Context.RegisterReceiver(receiver, intentFilter);
        }

        /// <summary>
        /// Unregisters the receiver using <see cref="Application.Context"/>.
        /// </summary>
        /// <param name="receiver">Receiver to unregister.</param>
        public static void UnregisterReceiver(this BroadcastReceiver receiver)
        {
            AApplication.Context.UnregisterReceiver(receiver);
        }
    }
}