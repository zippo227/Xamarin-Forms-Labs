using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Extensions;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Helper class for rendering TextView like controls.
    /// 
    /// </summary>
    internal class TextViewRenderHelper
    {
        public Context Context { get; private set; }

        public TextViewRenderHelper(Context context)
        {
            Context = context;
        }

        public Android.Graphics.Color GetSpanColor(Xamarin.Forms.Color color, ColorStateList defaultColors)
        {
            if (color == Xamarin.Forms.Color.Default)
                return new Android.Graphics.Color(defaultColors.DefaultColor);

            return color.ToAndroid();
        }

        /// <summary>
        /// Load the font from assets
        /// </summary>
        /// <param name="fontName">name of the font</param>
        /// <returns></returns>
        private Typeface LoadFontFromAsset(string fontName)
        {
            try
            {
                return Typeface.CreateFromAsset(Context.Assets, "fonts/" + fontName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("not found in assets. Exception: {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// Load the font from file
        /// </summary>
        /// <param name="fontName">name of the font</param>
        /// <returns></returns>
        private Typeface LoadFontFromFile(string fontName)
        {
            try
            {
                return Typeface.CreateFromFile("fonts/" + fontName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("not found by file. Exception: {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// Load the font
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public Typeface TrySetFont(string fontName)
        {
            var tp = TypefaceCache.SharedCache.RetrieveTypeface(fontName);
            if (tp == null)
                tp = LoadFontFromAsset(fontName);
            if (tp == null)
                tp = LoadFontFromFile(fontName);
            if (tp == null)
                tp = Typeface.Default;
            
            TypefaceCache.SharedCache.StoreTypeface(fontName, tp);

            return tp;
        }
    }
}
