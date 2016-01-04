using System;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// See http://stackoverflow.com/questions/6612316/how-set-spannable-object-font-with-custom-font
    /// </summary>
    public class CustomTypefaceSpan : TypefaceSpan
    {
        private readonly Typeface _newType;
        private readonly Android.Graphics.Color _fontColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypefaceSpan"/> class.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="type">The type.</param>
        /// <param name="color">The color.</param>
        public CustomTypefaceSpan(String family, Typeface type, Android.Graphics.Color color)
            : base(family)
        {

            _newType = type;
            _fontColor = color;
        }


        /// <summary>
        /// Handles the Update Draw State request
        /// </summary>
        /// <param name="ds">The ds.</param>
        public override void UpdateDrawState(TextPaint ds)
        {
            ApplyCustomTypeFace(ds, _newType, _fontColor);
        }


        /// <summary>
        /// Handles the update measure state request
        /// </summary>
        /// <param name="paint">The paint.</param>
        public override void UpdateMeasureState(TextPaint paint)
        {

            ApplyCustomTypeFace(paint, _newType, _fontColor);
        }

        /// <summary>
        /// Applies the custom type face.
        /// </summary>
        /// <param name="paint">The paint.</param>
        /// <param name="tf">The tf.</param>
        /// <param name="color">The color.</param>
        private static void ApplyCustomTypeFace(Paint paint, Typeface tf, Android.Graphics.Color color)
        {
            int oldStyle;
            Typeface old = paint.Typeface;
            if (old == null)
            {
                oldStyle = 0;
            }
            else
            {
                oldStyle = (int)old.Style;
            }

            int fake = oldStyle & ~(int)tf.Style;
            if ((fake & (int)TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fake & (int)TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }


            paint.SetARGB(color.A, color.R, color.G, color.B);

            paint.SetTypeface(tf);
        }
    }
}