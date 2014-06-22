using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
	/// <summary>
	/// Delegate CurrentPageChangingEventHandler
	/// </summary>
	public delegate void CurrentPageChangingEventHandler();
	/// <summary>
	/// Delegate CurrentPageChangedEventHandler
	/// </summary>
	public delegate void CurrentPageChangedEventHandler();

	/// <summary>
	/// Class ExtendedTabbedPage.
	/// </summary>
	public class ExtendedTabbedPage : TabbedPage
	{
		/// <summary>
		/// Occurs when [current page changing].
		/// </summary>
		public event CurrentPageChangingEventHandler CurrentPageChanging;
		/// <summary>
		/// Occurs when [current page changed].
		/// </summary>
		public event CurrentPageChangedEventHandler CurrentPageChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTabbedPage"/> class.
		/// </summary>
		public ExtendedTabbedPage()
		{
			this.PropertyChanging += this.OnPropertyChanging;
			this.PropertyChanged += this.OnPropertyChanged;
		}

		/// <summary>
		/// Handles the <see cref="E:PropertyChanging" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "CurrentPage")
			{
				this.RaiseCurrentPageChanging();
			}
		}

		/// <summary>
		/// Handles the <see cref="E:PropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentPage")
			{
				this.RaiseCurrentPageChanged();
			}
		}

		/// <summary>
		/// Raises the current page changing.
		/// </summary>
		private void RaiseCurrentPageChanging()
		{
			var handler = this.CurrentPageChanging;
			if (handler != null)
			{
				handler();
			}
		}

		/// <summary>
		/// Raises the current page changed.
		/// </summary>
		private void RaiseCurrentPageChanged()
		{
			var handler = this.CurrentPageChanged;
			if (handler != null)
			{
				handler();
			}
		}
	}
}