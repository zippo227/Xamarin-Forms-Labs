using System;
using System.Collections.ObjectModel;

namespace Xamarin.Forms.Labs.Controls
{
	public class EditableListView<T> : View
	{
		public static readonly BindableProperty SourceProperty = BindableProperty.Create<EditableListView<T>, ObservableCollection<T>>(p => p.Source, default(ObservableCollection<T>));
		public ObservableCollection<T> Source
		{
			get { return (ObservableCollection<T>)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly BindableProperty AddRowCommandProperty = BindableProperty.Create<EditableListView<T>, Command>(p => p.AddRowCommand, default(Command));
		public Command AddRowCommand
		{
			get { return (Command)GetValue(AddRowCommandProperty); }
			set { SetValue(AddRowCommandProperty, value); }
		}

		public float CellHeight { get; set; }
		public Type ViewType { get; set; }

		public EditableListView ()
		{
		}

		public void ExecuteAddRow()
		{
			var addRowCommand = AddRowCommand;
			if (addRowCommand != null)
				addRowCommand.Execute(null);
		}
	}
}

