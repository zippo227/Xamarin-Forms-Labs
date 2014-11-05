
namespace Xamarin.Forms.Labs.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms.Labs.Behaviors;
    using Xamarin.Forms.Labs.Exceptions;

    /// <summary>
    /// Provides a View that uses swipe left and swipe right to change between displays
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CarouselView<T> : ContentView
    {
        #region Bindables
        /// <summary>
        /// Property defnition for the <see cref="ViewModels"/> property
        /// </summary>
        public static BindableProperty ViewModelsProperty = BindableProperty.Create<CarouselView<T>, ObservableCollection<T>>(x => x.ViewModels, default(ObservableCollection<T>),BindingMode.OneWay,null,ViewModelsChanged);
        /// <summary>
        /// Property definition for the <see cref="TemplateSelector"/> property
        /// </summary>
        public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create<CarouselView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector),BindingMode.OneWay,null,TemplateSelectorChanged);
        /// <summary>
        /// Property definition for the <see cref="TickColor"/> property
        /// </summary>
        public static readonly BindableProperty TickColorProperty =BindableProperty.Create<CarouselView<T>, Color>(x => x.TickColor, Color.Lime);
        /// <summary>
        /// Property definition for the <see cref="ShowTick"/>
        /// </summary>
        public static readonly BindableProperty ShowTickProperty =BindableProperty.Create<CarouselView<T>, bool>(x => x.ShowTick, true,BindingMode.OneWay,null,ShowTickchanged);


        private static void ShowTickchanged(BindableObject bo, bool oldval, bool newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));
            cv.ShowTickChanged(newval);            
        }
        private static void ViewModelsChanged(BindableObject bo, ObservableCollection<T> oldval, ObservableCollection<T> newval)
        {
            var cv = bo as CarouselView<T>;
            if (cv == null)
                throw new InvalidBindableException(bo, typeof(CarouselView<T>));
            cv.ViewModelsChanged(oldval, newval);
        }
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
        public bool ShowTick
        {
            get { return (bool)GetValue(ShowTickProperty); }
            set { SetValue(ShowTickProperty,value);}
        }
        /// <summary>
        /// The color for the Ticks
        /// </summary>
        public Color TickColor
        {
            get { return (Color)GetValue(TickColorProperty); }
            set { SetValue(TickColorProperty,value);}
        }
        /// <summary>
        /// The collection of viewmodels to display in the carousel
        /// </summary>
        public ObservableCollection<T> ViewModels
        {
            get { return (ObservableCollection<T>)GetValue(ViewModelsProperty); }
            set { SetValue(ViewModelsProperty, value); }
        }

        private void ShowTickChanged(bool newval)
        {
            myGrid.IsVisible = newval;
        }
        private void SelectorChanged(TemplateSelector newval)
        {
            if(contentView != null)//may be constructing
                contentView.TemplateSelector = newval;

        }

        private void ViewModelsChanged(ObservableCollection<T> oldval, ObservableCollection<T> newval)
        {
            currentview = -1;
            if (contentView != null)
            {
                SetupTickBoard();
                SwitchView(true);
            }
            if (oldval != null) oldval.CollectionChanged -= ViewModelCollectionContentsChanged;
            newval.CollectionChanged += ViewModelCollectionContentsChanged;
        }

        private void ViewModelCollectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetupTickBoard();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    currentview = -1;
                    contentView.Content = null;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == currentview) //Well damn
                        SwitchView(currentview == 0);
                    break;
                case NotifyCollectionChangedAction.Add:
                    if(e.NewStartingIndex==currentview)
                        SwitchView(currentview<ViewModels.Count);
                    break;
            }
        }

        
        /// <summary>
        /// Used to match a type with a datatemplate
        /// <see cref="TemplateSelector"/>
        /// </summary>
        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        private readonly GesturesContentView gestureView;
        private readonly TemplateContentView<object> contentView;

        private readonly List<GestureInterest> interests=new List<GestureInterest>();

        private readonly Grid myGrid,marker;
        private int currentview;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();            
            foreach (var i in interests) i.BindingContext = BindingContext;
            gestureView.BindingContext = BindingContext;
            contentView.BindingContext = BindingContext;
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
            gestureView=new GesturesContentView
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            };

            interests.Add(new GestureInterest { Direction = Directionality.Right, GestureType = GestureType.Swipe,
                GestureCommand = new RelayGesture((g, x) => SwitchView(false),(g,x)=>currentview > 0)});
            interests.Add(new GestureInterest{Direction = Directionality.Left,GestureType = GestureType.Swipe,
                GestureCommand = new RelayGesture((g,x)=>SwitchView(true),(g,x)=>currentview < ViewModels.Count)});


           //Setup the TemplateContentView
            contentView = new TemplateContentView<object>
                              {
                                  HorizontalOptions = LayoutOptions.CenterAndExpand,
                                  VerticalOptions = LayoutOptions.CenterAndExpand,
                                  TemplateSelector = TemplateSelector
                              };
            Content = gestureView;
            marker = new Grid { BackgroundColor = TickColor };
            var s = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 5, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            myGrid = new Grid { BackgroundColor = BackgroundColor, ColumnSpacing = 25, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, Padding = new Thickness(35, 2.5) };
            myGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Absolute) });
            s.Children.Add(contentView);
            s.Children.Add(myGrid);
            gestureView.Content = s;
            gestureView.RegisterInterests(s, interests);
            
        }

        private void SetupTickBoard()
        {
            myGrid.ColumnDefinitions.Clear();
            for(var i=0;i<ViewModels.Count;i++)
                myGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1,GridUnitType.Star)});

        }

        private void SwitchView(bool increment)
        {
            var newval = currentview + (increment ? 1 : -1);
            
            if (newval < 0 || newval > ViewModels.Count() - 1) return;
            if(myGrid.Children.Count > 1)
                myGrid.Children.Remove(marker);//Should be the only other child of the grid
            currentview = newval;
            if (contentView.Content != null)
                gestureView.RemoveInterestsFor(contentView.Content);
            contentView.ViewModel = ViewModels[currentview];
            if (contentView.Content != null)
                gestureView.RegisterInterests(contentView.Content, interests);
            myGrid.Children.Add(marker, currentview, 0);
        }

    }
}
