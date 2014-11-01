using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Labs.Controls
{
	public class SelectCell : ExtendedTextCell
	{
		public static readonly BindableProperty ItemsProperty = BindableProperty.Create<SelectCell, List<string>>(p => p.Items, default(List<string>));
		public List<string> Items
		{
			get { return (List<string>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<SelectCell, string>(p => p.SelectedItem, default(string));
		public string SelectedItem
		{
			get { return (string)GetValue(SelectedItemProperty);  }
			set { SetValue(SelectedItemProperty, value); Detail = value; }
		}

		public Func<INavigation> Navigation { get; set; }
		List<CheckboxCell> cells = new List<CheckboxCell>();
		TableView selectionTableView;
		public event Action SelectedItemChanged;

		public SelectCell(Func<INavigation> navigation)
		{
			Navigation = navigation;
			ShowDisclousure = true;
			DetailLocation = Xamarin.Forms.Labs.Enums.TextCellDetailLocation.Right;
			this.DetailColor = Color.Gray;

			BackgroundColor = Color.White;
			this.Tapped += HandleTapped;
		}

		protected virtual ContentPage CreatePage()
		{
			return new ContentPage();
		}

		void CheckboxChanged(object sender, EventArgs<bool> args)
		{
			foreach (var cell in cells) {
				if (cell == sender)
					SelectedItem = cell.Text;
				else
					cell.Checked = false;
			}

			if (SelectedItemChanged != null)
				SelectedItemChanged();

			selectionTableView.Root = new TableRoot { new TableSection () { cells } };
		}

		void HandleTapped (object sender, EventArgs e)
		{
			var page = CreatePage();

			cells.Clear ();
			foreach (string curItem in Items)
			{
				var checkboxCell = new CheckboxCell(true) { Text = curItem, Checked = curItem == SelectedItem };
				checkboxCell.CheckedChanged += CheckboxChanged;
				cells.Add(checkboxCell);
			}

			selectionTableView = new TableView
			{
				Intent = TableIntent.Settings,
				Root = new TableRoot
				{
					new TableSection()
					{
						cells  
					}
				}
			};

			page.Content = selectionTableView;

			Navigation().PushAsync(page);
		}
	}
}

