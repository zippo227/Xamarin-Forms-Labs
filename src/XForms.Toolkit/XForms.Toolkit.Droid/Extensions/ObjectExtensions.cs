using System;
using Android.Content;
using Android.App;

namespace XForms.Toolkit
{
    public static class ObjectExtensions
    {
        public static void StartActivity(this object o, Intent intent)
        {
            var context = o as Context;
            if (context != null)
            {
                context.StartActivity (intent);
            } 
            else
            {
                intent.SetFlags (ActivityFlags.NewTask);
                Application.Context.StartActivity (intent);
            }
        }
    }
}

