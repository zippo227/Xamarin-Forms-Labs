// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-19-2014
// ***********************************************************************
// <copyright file="ViewFactory.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Mvvm
{
	/// <summary>
	/// Class ViewTypeAttribute.
	/// </summary>
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
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
		public ViewTypeAttribute (Type viewType)
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
		static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type> ();
		/// <summary>
		/// The page cache
		/// </summary>
		private static readonly Dictionary<string, Tuple<ViewModel, Page>> PageCache = new Dictionary<string, Tuple<ViewModel, Page>>();

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
		public static void Register<TView, TViewModel> ()
			where TView : Page
			where TViewModel : ViewModel
		{
			TypeDictionary[typeof (TViewModel)] = typeof (TView);
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <param name="createAction">The create action.</param>
		/// <returns>Page.</returns>
		/// <exception cref="System.InvalidOperationException">Unknown View for ViewModel</exception>
		public static Page CreatePage(Type viewModelType, Action<ViewModel, Page> createAction = null)
		{
			Type viewType = null;
			if (TypeDictionary.ContainsKey (viewModelType)) {
				viewType = TypeDictionary[viewModelType];
			} else {
				throw new InvalidOperationException ("Unknown View for ViewModel");
			}

			var pageCacheKey = string.Format("{0}:{1}", viewModelType.Name, viewType.Name);
			if (EnableCache)
			{
				if (PageCache.ContainsKey(pageCacheKey))
				{
					var cache = PageCache[pageCacheKey];

					if (createAction != null)
					{
						createAction(cache.Item1, cache.Item2);
					}
				}
			}

			var page = (Page)Activator.CreateInstance (viewType);
			var viewModel = (ViewModel)Activator.CreateInstance(viewModelType);

			viewModel.Navigation = new ViewModelNavigation(page.Navigation);

			page.BindingContext = viewModel;

			if (createAction != null)
			{
				createAction(viewModel, page);
			}

			if (EnableCache)
			{
				PageCache[pageCacheKey] = new Tuple<ViewModel, Page>(viewModel, page);
			}

			return page;
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="createAction">The create action.</param>
		/// <returns>Page.</returns>
		public static Page CreatePage<TViewModel>(Action<ViewModel, Page> createAction = null)
			where TViewModel : ViewModel
		{
			var viewModelType = typeof (TViewModel);
			return CreatePage (viewModelType, createAction);
		}
	}
}
