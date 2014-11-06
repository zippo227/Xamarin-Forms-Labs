using System;
using System.Runtime.CompilerServices;

[assembly: 
    InternalsVisibleTo("Xamarin.Forms.Labs.Droid"),
    InternalsVisibleTo("Xamarin.Forms.Labs.iOS"),
    InternalsVisibleTo("Xamarin.Forms.Labs.WP8")]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// An extended entry cell control that allows set IsPassword
    /// </summary>
    public class ExtendedEntryCell : EntryCell
    {

        /// <summary>
        /// The IsPassword property
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create<ExtendedEntryCell,bool> ( p => p.IsPassword, false);

        /// <summary>
        /// Gets or sets IsPassword 
        /// </summary>
        public bool IsPassword 
        {
            get { return (bool)GetValue (IsPasswordProperty); }
            set { SetValue (IsPasswordProperty,value); }
        }
    }
}

