using System;
using System.Runtime.CompilerServices;

[assembly: 
    InternalsVisibleTo("Xamarin.Forms.Labs.Droid"),
    InternalsVisibleTo("Xamarin.Forms.Labs.iOS"),
    InternalsVisibleTo("Xamarin.Forms.Labs.WP8")]

namespace Xamarin.Forms.Labs.Controls
{
	public class ExtendedEditor : Editor
	{
		public ExtendedEditor ()
		{
		}

		public static readonly BindableProperty FontProperty = BindableProperty.Create<ExtendedEditor, Font> (p => p.Font, default(Font));

		public Font Font 
        {
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}

        public EventHandler LeftSwipe;
        public EventHandler RightSwipe;

        internal void OnLeftSwipe(object sender, EventArgs e)
        {
            var handler = this.LeftSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void OnRightSwipe(object sender, EventArgs e)
        {
            var handler = this.RightSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }
	}
}

