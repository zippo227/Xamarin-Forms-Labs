using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Mvvm
{
	/// <summary>
	/// Class ViewModelNavigation.
	/// </summary>
	public class ViewModelNavigation
	{
		/// <summary>
		/// The _implementor
		/// </summary>
		readonly INavigation _implementor;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelNavigation"/> class.
		/// </summary>
		/// <param name="implementor">The implementor.</param>
		public ViewModelNavigation(INavigation implementor)
		{
			_implementor = implementor;
		}

		// This method can be considered unclean in the pure MVVM sense, 
		// however it has been handy to me at times
		/// <summary>
		/// Pushes the asynchronous.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Task.</returns>
		public Task PushAsync(Page page)
		{
			return _implementor.PushAsync(page);
		}

		/// <summary>
		/// Pushes the asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The activate action.</param>
		/// <returns>Task.</returns>
		public Task PushAsync<TViewModel>(Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushAsync(ViewFactory.CreatePage<TViewModel>(activateAction));
		}

		/// <summary>
		/// Pops the asynchronous.
		/// </summary>
		/// <returns>Task.</returns>
		public Task PopAsync()
		{
			return _implementor.PopAsync();
		}

		/// <summary>
		/// Pops to root asynchronous.
		/// </summary>
		/// <returns>Task.</returns>
		public Task PopToRootAsync()
		{
			return _implementor.PopToRootAsync();
		}

		// This method can be considered unclean in the pure MVVM sense, 
		// however it has been handy to me at times
		/// <summary>
		/// Pushes the modal asynchronous.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Task.</returns>
		public Task PushModalAsync(Page page)
		{
			return _implementor.PushModalAsync(page);
		}


		/// <summary>
		/// Pushes the modal asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The create action.</param>
		/// <returns>Task.</returns>
		public Task PushModalAsync<TViewModel>(Action<TViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushModalAsync(ViewFactory.CreatePage<TViewModel>(activateAction));
		}

		/// <summary>
		/// Pops the modal asynchronous.
		/// </summary>
		/// <returns>Task.</returns>
		public Task PopModalAsync()
		{
			return _implementor.PopModalAsync();
		}
	}
}