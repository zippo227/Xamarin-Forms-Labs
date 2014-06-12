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
    public static class IntentFilterExtensions
    {
        /// <summary>
        /// Gets a single result for the intent filter
        /// </summary>
        /// <param name="intentFilter">Intent filter</param>
        /// <returns>An intent result, null if not successful</returns>
        public static Intent RegisterReceiver(this IntentFilter intentFilter)
        {
            return Application.Context.RegisterReceiver(null, intentFilter);
        }
    }
}