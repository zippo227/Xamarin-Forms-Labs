using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Sample.Data;
using XLabs.Serialization;

namespace XLabs.Sample.Pages.Controls
{
    public class UsPresidentList : ContentPage
    {
        public UsPresidentList()
        {
            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new ActivityIndicator
                    {
                        Color = Color.Red,
                        IsRunning = true,
                        IsVisible = true
                    }
                }
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var source = new JsonImageListSource(Resolver.Resolve<IJsonSerializer>());
            var items = await source.GetListItems();

            var list = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = items,
                ItemTemplate = new DataTemplate(() =>
                {
                    var image = new CircleImage
                    {
                        Aspect = Aspect.AspectFill,
                        HeightRequest = Device.OnPlatform(50, 75, 75),
                        WidthRequest = Device.OnPlatform(50, 75, 75),
                    };
                    image.SetBinding(Image.SourceProperty, "Image");
                    var label = new Label
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof (Label)),
                        YAlign = TextAlignment.Center,
                        XAlign = TextAlignment.Center
                    };
                    label.SetBinding(Label.TextProperty, "Title");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(20),
                            Spacing = 10,
                            Orientation = StackOrientation.Horizontal,
                            Children = { image, label }
                        }
                    };
                })
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    new Label { Text = Device.OnPlatform(
                        "Works on iOS", 
                        "Scrolling back and forth will cause Out of memory error on Android", 
                        "Works on WP") },
                    list
                }
            };
        }
    }

    public class UsPresidentListSquare : ContentPage
    {
        public UsPresidentListSquare()
        {
            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new ActivityIndicator
                    {
                        Color = Color.Red,
                        IsRunning = true,
                        IsVisible = true
                    }
                }
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var source = new JsonImageListSource(Resolver.Resolve<IJsonSerializer>());
            var items = await source.GetListItems();

            this.Content = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = items,
                ItemTemplate = new DataTemplate(() =>
                {
                    var image = new Image
                    {
                        Aspect = Aspect.AspectFill,
                        HeightRequest = Device.OnPlatform(50, 75, 75),
                        WidthRequest = Device.OnPlatform(50, 75, 75),
                    };
                    image.SetBinding(Image.SourceProperty, "Image");
                    var label = new Label
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        YAlign = TextAlignment.Center,
                        XAlign = TextAlignment.Center
                    };
                    label.SetBinding(Label.TextProperty, "Title");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(20),
                            Spacing = 10,
                            Orientation = StackOrientation.Horizontal,
                            Children = { image, label }
                        }
                    };
                })
            };
        }
    }
}