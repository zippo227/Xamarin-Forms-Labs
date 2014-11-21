using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Platform.Exceptions;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class RepeaterView.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RepeaterView<T> : StackLayout
	{
		/// <summary>
		/// Class CollectionChangedHandle.
		/// </summary>
		private class CollectionChangedHandle : IDisposable
		{
			/// <summary>
			/// The _items source
			/// </summary>
			private readonly INotifyCollectionChanged _itemsSource;
			/// <summary>
			/// The _event handler
			/// </summary>
			private NotifyCollectionChangedEventHandler _eventHandler;

			/// <summary>
			/// Initializes a new instance of the <see cref="CollectionChangedHandle"/> class.
			/// </summary>
			/// <param name="repeater">The repeater.</param>
			/// <param name="itemsSource">The items source.</param>
			public CollectionChangedHandle(RepeaterView<T> repeater, IEnumerable<T> itemsSource)
			{
				_itemsSource = itemsSource as INotifyCollectionChanged;

				if (_itemsSource != null)
				{
					_eventHandler = repeater.ItemsSource_CollectionChanged;
					_itemsSource.CollectionChanged += _eventHandler;
				}
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				if (_eventHandler != null)
				{
					_itemsSource.CollectionChanged -= _eventHandler;
					_eventHandler = null;
				}
			}
		}

		/// <summary>
		/// The item template property
		/// </summary>
		public static readonly BindableProperty ItemTemplateProperty =
		BindableProperty.Create<RepeaterView<T>, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

		/// <summary>
		/// The items source property
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<RepeaterView<T>, IEnumerable<T>>(p => p.ItemsSource, Enumerable.Empty<T>(), BindingMode.OneWay, null, ItemsChanged);

		/// <summary>
		/// The item click command property
		/// </summary>
		public static BindableProperty ItemClickCommandProperty =
			BindableProperty.Create<RepeaterView<T>, ICommand>(x => x.ItemClickCommand, null);

		/// <summary>
		/// The template selector property
		/// </summary>
		public static readonly BindableProperty TemplateSelectorProperty =
			BindableProperty.Create<RepeaterView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector));

		/// <summary>
		/// Delegate RepeaterViewItemAddedEventHandler
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="RepeaterViewItemAddedEventArgs"/> instance containing the event data.</param>
		public delegate void RepeaterViewItemAddedEventHandler(object sender, RepeaterViewItemAddedEventArgs args);

		/// <summary>
		/// Occurs when [item created].
		/// </summary>
		public event RepeaterViewItemAddedEventHandler ItemCreated;

		/// <summary>
		/// The _collection changed handle
		/// </summary>
		private IDisposable _collectionChangedHandle;

		/// <summary>
		/// Initializes a new instance of the <see cref="RepeaterView{T}"/> class.
		/// </summary>
		public RepeaterView()
		{
			Spacing = 0;
		}

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IEnumerable<T> ItemsSource
		{
			get { return (IEnumerable<T>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the template selector.
		/// </summary>
		/// <value>The template selector.</value>
		public TemplateSelector TemplateSelector
		{
			get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
			set { SetValue(TemplateSelectorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the item click command.
		/// </summary>
		/// <value>The item click command.</value>
		public ICommand ItemClickCommand
		{
			get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
			set { SetValue(ItemClickCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// Gives codebehind a chance to play with the
		/// newly created view object :D
		/// </summary>
		/// <param name="view">The visual view object</param>
		/// <param name="model">The item being added</param>
		protected virtual void NotifyItemAdded(View view, T model)
		{
			if (ItemCreated != null)
			{
				ItemCreated(this, new RepeaterViewItemAddedEventArgs(view, model));
			}
		}

		/// <summary>
		/// Select a datatemplate dynamically
		/// Prefer the TemplateSelector then the DataTemplate
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>DataTemplate.</returns>
		protected virtual DataTemplate GetTemplateFor(Type type)
		{
			DataTemplate retTemplate = null;
			if (TemplateSelector != null)
				retTemplate = TemplateSelector.TemplateFor(type);
			return retTemplate ?? ItemTemplate;
		}

		/// <summary>
		/// Creates a view based on the items type
		/// While we do have T, T could very well be
		/// a common superclass or an interface by
		/// using the items actual type we support
		/// both inheritance based polymorphism
		/// and shape based polymorphism
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>A View that has been initialized with <see cref="item" /> as it's BindingContext</returns>
		/// <exception cref="InvalidVisualObjectException"></exception>
		/// Thrown when the matched datatemplate inflates to an object not derived from either
		/// <see cref="Xamarin.Forms.View" /> or <see cref="Xamarin.Forms.ViewCell" />
		protected virtual View ViewFor(T item)
		{
			var template = GetTemplateFor(item.GetType());
			var content = template.CreateContent();

			if (!(content is View) && !(content is ViewCell))
				throw new InvalidVisualObjectException(content.GetType());
			var view = (content is View) ? content as View : ((ViewCell)content).View;
			view.BindingContext = item;
			view.GestureRecognizers.Add(new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item });
			return view;
		}

		/// <summary>
		/// Reset the collection of bound objects
		/// Remove the old collection changed eventhandler (if any)
		/// Create new cells for each new item
		/// </summary>
		/// <param name="bindable">The control</param>
		/// <param name="oldValue">Previous bound collection</param>
		/// <param name="newValue">New bound collection</param>
		/// <exception cref="System.Exception">Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a  + bindable.GetType().Name</exception>
		private static void ItemsChanged(BindableObject bindable, IEnumerable<T> oldValue, IEnumerable<T> newValue)
		{
			var control = bindable as RepeaterView<T>;
			if (control == null)
				throw new Exception("Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a " + bindable.GetType().Name);

			if (control._collectionChangedHandle != null)
			{
				control._collectionChangedHandle.Dispose();
			}

			control._collectionChangedHandle = new CollectionChangedHandle(control, newValue);
			control.Children.Clear();

			foreach (var item in newValue)
			{
				var view = control.ViewFor(item);
				control.Children.Add(view);
				control.NotifyItemAdded(view, item);
			}
		}

		// ReSharper disable once InconsistentNaming
		/// <summary>
		/// Update visible controls based on changes in the bound collection
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				Children.Clear();
			}
			else
			{
				if (e.OldItems != null)
				{
					Children.RemoveAt(e.OldStartingIndex);
				}

				if (e.NewItems != null)
				{
					foreach (T item in e.NewItems)
					{
						var comparer = EqualityComparer<T>.Default;
						var index = -1;
						var i = 0;
						foreach (var t in ItemsSource)
						{
							if (comparer.Equals(t, item))
							{
								index = i;
								break;
							}
							i++;
						}
						var view = ViewFor(item);
						Children.Insert(index, view);
						NotifyItemAdded(view, item);
					}
				}
			}
			UpdateChildrenLayout();
			InvalidateLayout();
		}
	}

	public class RepeaterViewItemAddedEventArgs : EventArgs
	{
		public RepeaterViewItemAddedEventArgs(View view, object model)
		{
			View = view;
			Model = model;
		}

		public View View { get; set; }

		public object Model { get; set; }
	}
}