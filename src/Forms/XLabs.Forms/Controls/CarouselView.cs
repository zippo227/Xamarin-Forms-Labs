
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Behaviors;
using XLabs.Forms.Exceptions;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Provides a View that uses swipe left and swipe right to change between displays
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CarouselView<T> : ContentView
	{
		#region Bindables
		/// <summary>
		/// Property defnition for the <see cref="ViewModels" /> property
		/// </summary>
		public static BindableProperty ViewModelsProperty = BindableProperty.Create<CarouselView<T>, ObservableCollection<T>>(x => x.ViewModels, default(ObservableCollection<T>),BindingMode.OneWay,null,ViewModelsChanged);
		/// <summary>
		/// Property definition for the <see cref="TemplateSelector" /> property
		/// </summary>
		public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create<CarouselView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector),BindingMode.OneWay,null,TemplateSelectorChanged);
		/// <summary>
		/// Property definition for the <see cref="TickColor" /> property
		/// </summary>
		public static readonly BindableProperty TickColorProperty =BindableProperty.Create<CarouselView<T>, Color>(x => x.TickColor, Color.Lime);
		/// <summary>
		/// Property definition for the <see cref="ShowTick" />
		/// </summary>
		public static readonly BindableProperty ShowTickProperty =BindableProperty.Create<CarouselView<T>, bool>(x => x.ShowTick, true,BindingMode.OneWay,null,ShowTickchanged);


		/// <summary>
		/// Shows the tickchanged.
		/// </summary>
		/// <param name="bo">The bo.</param>
		/// <param name="oldval">if set to <c>true</c> [oldval].</param>
		/// <param name="newval">if set to <c>true</c> [newval].</param>
		/// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
		private static void ShowTickchanged(BindableObject bo, bool oldval, bool newval)
		{
			var cv = bo as CarouselView<T>;
			if (cv == null)
				throw new InvalidBindableException(bo, typeof(CarouselView<T>));
			cv.ShowTickChanged(newval);            
		}
		/// <summary>
		/// Views the models changed.
		/// </summary>
		/// <param name="bo">The bo.</param>
		/// <param name="oldval">The oldval.</param>
		/// <param name="newval">The newval.</param>
		/// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
		private static void ViewModelsChanged(BindableObject bo, ObservableCollection<T> oldval, ObservableCollection<T> newval)
		{
			var cv = bo as CarouselView<T>;
			if (cv == null)
				throw new InvalidBindableException(bo, typeof(CarouselView<T>));
			cv.ViewModelsChanged(oldval, newval);
		}
		/// <summary>
		/// Templates the selector changed.
		/// </summary>
		/// <param name="bo">The bo.</param>
		/// <param name="oldval">The oldval.</param>
		/// <param name="newval">The newval.</param>
		/// <exception cref="XLabs.Forms.Exceptions.InvalidBindableException"></exception>
		private static void TemplateSelectorChanged(BindableObject bo, TemplateSelector oldval, TemplateSelector newval)
		{
			var cv = bo as CarouselView<T>;
			if (cv == null)
				throw new InvalidBindableException(bo, typeof(CarouselView<T>));
			cv.SelectorChanged(newval);
		}
		#endregion
		/// <summary>
		/// Show the tickboard.  The tickboard takes up 8dp vertically when shown.
		/// </summary>
		/// <value><c>true</c> if [show tick]; otherwise, <c>false</c>.</value>
		public bool ShowTick
		{
			get { return (bool)GetValue(ShowTickProperty); }
			set { SetValue(ShowTickProperty,value);}
		}
		/// <summary>
		/// The color for the Ticks
		/// </summary>
		/// <value>The color of the tick.</value>
		public Color TickColor
		{
			get { return (Color)GetValue(TickColorProperty); }
			set { SetValue(TickColorProperty,value);}
		}
		/// <summary>
		/// The collection of viewmodels to display in the carousel
		/// </summary>
		/// <value>The view models.</value>
		public ObservableCollection<T> ViewModels
		{
			get { return (ObservableCollection<T>)GetValue(ViewModelsProperty); }
			set { SetValue(ViewModelsProperty, value); }
		}

		/// <summary>
		/// Shows the tick changed.
		/// </summary>
		/// <param name="newval">if set to <c>true</c> [newval].</param>
		private void ShowTickChanged(bool newval)
		{
			_myGrid.IsVisible = newval;
		}
		/// <summary>
		/// Selectors the changed.
		/// </summary>
		/// <param name="newval">The newval.</param>
		private void SelectorChanged(TemplateSelector newval)
		{
			if(_contentView != null)//may be constructing
				_contentView.TemplateSelector = newval;

		}

		/// <summary>
		/// Views the models changed.
		/// </summary>
		/// <param name="oldval">The oldval.</param>
		/// <param name="newval">The newval.</param>
		private void ViewModelsChanged(ObservableCollection<T> oldval, ObservableCollection<T> newval)
		{
			_currentview = -1;
			if (_contentView != null)
			{
				SetupTickBoard();
				SwitchView(true);
			}
			if (oldval != null) oldval.CollectionChanged -= ViewModelCollectionContentsChanged;
			newval.CollectionChanged += ViewModelCollectionContentsChanged;
		}

		/// <summary>
		/// Views the model collection contents changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void ViewModelCollectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SetupTickBoard();
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Reset:
					_currentview = -1;
					_contentView.Content = null;
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldStartingIndex == _currentview) //Well damn
						SwitchView(_currentview == 0);
					break;
				case NotifyCollectionChangedAction.Add:
					if(e.NewStartingIndex==_currentview)
						SwitchView(_currentview<ViewModels.Count);
					break;
			}
		}


		/// <summary>
		/// Used to match a type with a datatemplate
		/// <see cref="TemplateSelector" />
		/// </summary>
		/// <value>The template selector.</value>
		public TemplateSelector TemplateSelector
		{
			get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
			set { SetValue(TemplateSelectorProperty, value); }
		}

		/// <summary>
		/// The _gesture view
		/// </summary>
		private readonly GesturesContentView _gestureView;
		/// <summary>
		/// The _content view
		/// </summary>
		private readonly TemplateContentView<object> _contentView;

		/// <summary>
		/// The _interests
		/// </summary>
		private readonly List<GestureInterest> _interests=new List<GestureInterest>();

		/// <summary>
		/// The _my grid
		/// </summary>
		private readonly Grid _myGrid,_marker;
		/// <summary>
		/// The _currentview
		/// </summary>
		private int _currentview;

		/// <summary>
		/// Invoked whenever the binding context of the <see cref="T:Xamarin.Forms.View" /> changes. Override this method to add class handling for this event.
		/// </summary>
		/// <remarks>Overriders must call the base method.</remarks>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();            
			foreach (var i in _interests) i.BindingContext = BindingContext;
			_gestureView.BindingContext = BindingContext;
			_contentView.BindingContext = BindingContext;
		}

		/// <summary>
		/// Constructs the Carousel view and sets defaults
		/// </summary>
		public CarouselView()
		{
			//Create my children and have fun with them :D
			//ViewModels = new ObservableCollection<T>();
			//TemplateSelector = new TemplateSelector();

			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			//We need a gestureview that manages it's own gestures
			//Wrapping a contentview on the inside.
			//We will automap this TemplateSelector to the contentviews
			//Setup the gestureview
			_gestureView=new GesturesContentView
							{
								HorizontalOptions = LayoutOptions.FillAndExpand,
								VerticalOptions = LayoutOptions.FillAndExpand
							};

			_interests.Add(new GestureInterest { Direction = Directionality.Right, GestureType = GestureType.Swipe,
				GestureCommand = new RelayGesture((g, x) => SwitchView(false),(g,x)=>_currentview > 0)});
			_interests.Add(new GestureInterest{Direction = Directionality.Left,GestureType = GestureType.Swipe,
				GestureCommand = new RelayGesture((g,x)=>SwitchView(true),(g,x)=>_currentview < ViewModels.Count)});


		   //Setup the TemplateContentView
			_contentView = new TemplateContentView<object>
							  {
								  HorizontalOptions = LayoutOptions.CenterAndExpand,
								  VerticalOptions = LayoutOptions.CenterAndExpand,
								  TemplateSelector = TemplateSelector
							  };
			Content = _gestureView;
			_marker = new Grid { BackgroundColor = TickColor };
			var s = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 5, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			_myGrid = new Grid { BackgroundColor = BackgroundColor, ColumnSpacing = 25, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, Padding = new Thickness(35, 2.5) };
			_myGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Absolute) });
			s.Children.Add(_contentView);
			s.Children.Add(_myGrid);
			_gestureView.Content = s;
			_gestureView.RegisterInterests(s, _interests);
			
		}

		/// <summary>
		/// Setups the tick board.
		/// </summary>
		private void SetupTickBoard()
		{
			_myGrid.ColumnDefinitions.Clear();
			for(var i=0;i<ViewModels.Count;i++)
				_myGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star)});

		}

		/// <summary>
		/// Switches the view.
		/// </summary>
		/// <param name="increment">if set to <c>true</c> [increment].</param>
		private void SwitchView(bool increment)
		{
			var newval = _currentview + (increment ? 1 : -1);
			
			if (newval < 0 || newval > ViewModels.Count() - 1) return;
			
			_myGrid.Children.Clear();
			_currentview = newval;
			_contentView.ViewModel = ViewModels[_currentview];
			_myGrid.Children.Add(_marker, _currentview, 0);
		}

	}
}
