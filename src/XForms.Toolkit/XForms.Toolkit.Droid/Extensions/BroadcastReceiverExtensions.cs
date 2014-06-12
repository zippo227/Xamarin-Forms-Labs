using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XForms.Toolkit
{
    public static class BroadcastReceiverExtensions
    {
        public static Intent RegisterReceiver(this BroadcastReceiver receiver, IntentFilter intentFilter)
        {
            return Application.Context.RegisterReceiver(receiver, intentFilter);
        }

        public static void UnregisterReceiver(this BroadcastReceiver receiver)
        {
            Application.Context.UnregisterReceiver(receiver);
        }
    }
}