using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	using XLabs.Platform;

	/// <summary>
	/// Class BindablePicker.
	/// </summary>
	public class BindablePicker : View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BindablePicker"/> class.
		/// </summary>
		public BindablePicker()
		{
			this.Items = new ObservableCollection<string>();
			((ObservableCollection<string>)this.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnItemsCollectionChanged);
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		/// <summary>
		/// Gets or sets the souce item label converter.
		/// </summary>
		/// <value>The souce item label converter.</value>
		public Func<object, string> SouceItemLabelConverter { get; set; }

		/// <summary>
		/// The items source property
		/// </summary>
		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindablePicker, IList>(o => o.ItemsSource, default(IList), propertyChanged: new BindableProperty.BindingPropertyChangedDelegate<IList> (BindablePicker.OnItemsSourceChanged));

		/// <summary>
		/// The selected item property
		/// </summary>
		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object),BindingMode.TwoWay,propertyChanged: new BindableProperty.BindingPropertyChangedDelegate<object> (BindablePicker.OnSelectedItemChanged));

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		/// <summary>
		/// Called when [items source changed].
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldvalue">The oldvalue.</param>
		/// <param name="newvalue">The newvalue.</param>
		private static void OnItemsSourceChanged(BindableObject bindable, IList oldvalue, IList newvalue)
		{
			var picker = bindable as BindablePicker;
			picker.Items.Clear();
			if (newvalue != null)
			{
				foreach (var item in newvalue)
				{
					if (picker.SouceItemLabelConverter != null)
						picker.Items.Add (picker.SouceItemLabelConverter (item));
					else 
						picker.Items.Add (item.ToString());
				}
			}
		}

		/// <summary>
		/// Handles the <see cref="E:SelectedIndexChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = null;
			}
			else
			{

				SelectedItem = ItemsSource[SelectedIndex];
			}
		}

		/// <summary>
		/// Called when [selected item changed].
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldvalue">The oldvalue.</param>
		/// <param name="newvalue">The newvalue.</param>
		private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as BindablePicker;
			if (newvalue != null) {
				string title = string.Empty;
				if (picker.SouceItemLabelConverter != null)
					title = picker.SouceItemLabelConverter (newvalue);
				else
					title = newvalue.ToString ();

				picker.SelectedIndex = picker.Items.IndexOf (title);
			} else {
				picker.SelectedIndex = -1;
			}
		}

		/// <summary>
		/// The title property
		/// </summary>
		public static readonly BindableProperty TitleProperty = BindableProperty.Create<BindablePicker, string>((BindablePicker w) => w.Title, null, BindingMode.OneWay, null, null, null, null);
		/// <summary>
		/// The selected index property
		/// </summary>
		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create<BindablePicker, int>((BindablePicker w) => w.SelectedIndex, -1, BindingMode.TwoWay, null, delegate(BindableObject bindable, int oldvalue, int newvalue)
			{
				EventHandler selectedIndexChanged = ((BindablePicker)bindable).SelectedIndexChanged;
				if (selectedIndexChanged != null)
				{
					selectedIndexChanged(bindable, EventArgs.Empty);
				}
			}, null, new BindableProperty.CoerceValueDelegate<int>(BindablePicker.CoerceSelectedIndex));

		/// <summary>
		/// Occurs when [selected index changed].
		/// </summary>
		public event EventHandler SelectedIndexChanged;

		/// <summary>
		/// Coerces the index of the selected.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="value">The value.</param>
		/// <returns>System.Int32.</returns>
		private static int CoerceSelectedIndex(BindableObject bindable, int value)
		{
			BindablePicker picker = (BindablePicker)bindable;
			if (picker.Items != null)
			{
				return value.Clamp(-1, picker.Items.Count-1);
			}
			return -1;
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get
			{
				return (string)base.GetValue(BindablePicker.TitleProperty);
			}
			set
			{
				base.SetValue(BindablePicker.TitleProperty, value);
			}
		}

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>The items.</value>
		public ObservableCollection<string> Items
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the index of the selected.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(BindablePicker.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(BindablePicker.SelectedIndexProperty, value);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ItemsCollectionChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (SelectedItem != null) {
				string title = string.Empty;
				if (SouceItemLabelConverter != null)
					title = SouceItemLabelConverter (SelectedItem);
				else
					title = SelectedItem.ToString ();

				SelectedIndex = Items.IndexOf (title);
			}
		}

		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedEntry), true);

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		/// <value><c>true</c> if this instance has border; otherwise, <c>false</c>.</value>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}	
	}
}

