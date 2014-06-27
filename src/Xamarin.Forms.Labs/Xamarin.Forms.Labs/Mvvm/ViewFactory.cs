using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Mvvm
{
    using Xamarin.Forms.Labs.Services;

    /// <summary>
    /// Class ViewTypeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the view.
        /// </summary>
        /// <value>The type of the view.</value>
        public Type ViewType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTypeAttribute"/> class.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        public ViewTypeAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }

    // Can be replaced by all sorts of complexity and auto loading BS but this keeps it simple and loose
    /// <summary>
    /// Class ViewFactory.
    /// </summary>
    public static class ViewFactory
    {
        /// <summary>
        /// The type dictionary
        /// </summary>
        private static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type>();

        /// <summary>
        /// The page cache
        /// </summary>
        private static readonly Dictionary<string, Tuple<ViewModel, Page>> PageCache =
            new Dictionary<string, Tuple<ViewModel, Page>>();

        /// <summary>
        /// Gets or sets a value indicating whether [enable cache].
        /// </summary>
        /// <value><c>true</c> if [enable cache]; otherwise, <c>false</c>.</value>
        public static bool EnableCache { get; set; }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TView">The type of the t view.</typeparam>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        public static void Register<TView, TViewModel>() where TView : Page where TViewModel : ViewModel
        {
            TypeDictionary[typeof(TViewModel)] = typeof(TView);

            var container = Resolver.Resolve<IDependencyContainer>();

            // check if we have DI container
            if (container != null) // register viewmodel with DI to enable non default vm constructors / service locator
                container.Register<TViewModel, TViewModel>();
        }

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <param name="initialiser">
        /// The create action.
        /// </param>
        /// <returns>
        /// Page.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Unknown View for ViewModel.
        /// </exception>
        public static Page CreatePage<TViewModel>(Action<TViewModel, Page> initialiser = null)
            where TViewModel : ViewModel
        {
            Type viewType = null;
            Type viewModelType = typeof(TViewModel);

            if (TypeDictionary.ContainsKey(viewModelType))
            {
                viewType = TypeDictionary[viewModelType];
            }
            else
            {
                throw new InvalidOperationException("Unknown View for ViewModel");
            }

            Page page = null;
            TViewModel viewModel;
            var pageCacheKey = string.Format("{0}:{1}", viewModelType.Name, viewType.Name);

            if (EnableCache && PageCache.ContainsKey(pageCacheKey))
            {
                var cache = PageCache[pageCacheKey];
                viewModel = cache.Item1 as TViewModel;
                page = cache.Item2;
            }
            else
            {
                page = (Page)Activator.CreateInstance(viewType);

                try
                {
                    viewModel = (TViewModel)Resolver.Resolve(viewModelType);
                }
                catch
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "ViewModel {0} cannot be resolved - please make sure you've added it to the ViewFactory by calling Register<TView, TViewModel>().",
                            viewModelType));
                }

                //this is the real fallback :)
                if (viewModel == null) viewModel = (TViewModel)Activator.CreateInstance(viewModelType);

                viewModel.Navigation = new ViewModelNavigation(page.Navigation);

                if (EnableCache) PageCache[pageCacheKey] = new Tuple<ViewModel, Page>(viewModel, page);
            }

            if (initialiser != null) initialiser(viewModel, page);

            // forcing break reference on viewmodel in order to allow initializer to do its work
            page.BindingContext = null;
            page.BindingContext = viewModel;

            return page;
        }
    }
}