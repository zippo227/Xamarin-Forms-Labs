using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
    public class ButtonGroup : ContentView
    {
        public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty ViewBackgroundColorProperty = BindableProperty.Create("ViewBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(ButtonGroup), Color.Default);
        public static readonly BindableProperty SelectedBorderColorProperty = BindableProperty.Create("SelectedBorderColor", typeof(Color), typeof(ButtonGroup), Color.Black);
        public static readonly BindableProperty SelectedFrameBackgroundColorProperty = BindableProperty.Create("SelectedFrameBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Black);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create<ButtonGroup, int>(p => p.SelectedIndex, 0, BindingMode.TwoWay);
        public static readonly BindableProperty ItemsPropertyProperty = BindableProperty.Create<ButtonGroup, List<string>>(p => p.Items, null, BindingMode.TwoWay);
        public static readonly BindableProperty FontProperty = BindableProperty.Create("Font", typeof(Font), typeof(ButtonGroup), Font.Default);
        public static readonly BindableProperty RoundedProperty = BindableProperty.Create("Rounded", typeof(bool), typeof(ButtonGroup), false);
        public static readonly BindableProperty IsNumberProperty = BindableProperty.Create("IsNumber", typeof(bool), typeof(ButtonGroup), false);

        private readonly WrapLayout ButtonLayout;
        private const int Spacing = 5;
        private const int Padding = 5;
        private const int ButtonBorderWidth = 1;
        private const int FramePadding = 1;
        private const int ButtonBorderRadius = 5;
        private const int ButtonHeight = 44;
        private const int ButtonHeightWp = 72;
        private const int ButtonHalfHeight = 22;
        private const int ButtonHalfHeightWp = 36;



        #region Properties
        public Color OutlineColor
        {
            get
            {
                return (Color)GetValue(OutlineColorProperty);
            }
            set
            {
                SetValue(OutlineColorProperty, value);
            }
        }

        public Color ViewBackgroundColor
        {
            get
            {
                return (Color)GetValue(ViewBackgroundColorProperty);
            }
            set
            {
                SetValue(ViewBackgroundColorProperty, value);
                ButtonLayout.BackgroundColor = value;
            }
        }

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set
            {
                SetValue(BackgroundColorProperty, value);

                if (ButtonLayout == null)
                {
                    return;
                }

                for (var iBtn = 0; iBtn < ButtonLayout.Children.Count; iBtn++)
                {
                    SetSelectedState(iBtn, iBtn == SelectedIndex);
                }
            }
        }

        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set
            {
                SetValue(SelectedBackgroundColorProperty, value);

                if (ButtonLayout == null)
                {
                    return;
                }

                for (var iBtn = 0; iBtn < ButtonLayout.Children.Count; iBtn++)
                {
                    SetSelectedState(iBtn, iBtn == SelectedIndex);
                }
            }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color SelectedTextColor
        {
            get { return (Color)GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public Color SelectedBorderColor
        {
            get { return (Color)GetValue(SelectedBorderColorProperty); }
            set { SetValue(SelectedBorderColorProperty, value); }
        }
        public Color SelectedFrameBackgroundColor
        {
            get { return (Color)GetValue(SelectedFrameBackgroundColorProperty); }
            set { SetValue(SelectedFrameBackgroundColorProperty, value); }
        }


        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetSelectedState(SelectedIndex, false);
                SetValue(SelectedIndexProperty, value);

                if (value < 0 || value >= ButtonLayout.Children.Count)
                {
                    return;
                }

                SetSelectedState(value, true);
            }
        }

        public List<string> Items
        {
            get { return (List<string>)GetValue(ItemsPropertyProperty); }
            set
            {
                SetValue(ItemsPropertyProperty, value);

                foreach (var item in Items)
                {
                    AddButton(item);
                }
            }
        }

        public bool Rounded
        {
            get
            {
                return (bool)GetValue(RoundedProperty);
            }
            set
            {
                SetValue(RoundedProperty, value);
            }
        }

        public bool IsNumber
        {
            get
            {
                return (bool)GetValue(IsNumberProperty);
            }
            set
            {
                SetValue(IsNumberProperty, value);
            }
        }

        #endregion

        private Command ClickedCommand;

        public ButtonGroup()
        {
            ButtonLayout = new WrapLayout
            {
                Spacing = Spacing,
                Padding = Padding,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
            //Padding = new Thickness(Spacing);
            Content = ButtonLayout;
            ClickedCommand = new Command(SetSelectedButton);
        }

        public void AddButton(string text)
        {
            var button = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = BackgroundColor,
                BorderColor = BorderColor,
                TextColor = TextColor,
                BorderWidth = ButtonBorderWidth,
                BorderRadius =
                    Rounded
                        ? Device.OnPlatform(ButtonHalfHeight, ButtonHalfHeight, ButtonHalfHeightWp)
                        : ButtonBorderRadius,
                HeightRequest = Device.OnPlatform(ButtonHeight, ButtonHeight, ButtonHeightWp),
                MinimumHeightRequest = Device.OnPlatform(ButtonHeight, ButtonHeight, ButtonHeightWp),
                Font = Font,
                Command = ClickedCommand,
                CommandParameter = ButtonLayout.Children.Count,
            };

            if (IsNumber)
            {
                button.Text = string.Format("{0}", text);
                button.WidthRequest = Device.OnPlatform(44, 44, 72);
                button.MinimumWidthRequest = Device.OnPlatform(44, 44, 72);
            }
            else
            {
                button.Text = string.Format("  {0}  ", text);
            }

            var frame = new Frame
            {
                BackgroundColor = ViewBackgroundColor,
                Padding = FramePadding,
                OutlineColor = OutlineColor,
                HasShadow = false,
                Content = button,
            };

            ButtonLayout.Children.Add(frame);

            SetSelectedState(ButtonLayout.Children.Count - 1, ButtonLayout.Children.Count - 1 == SelectedIndex);
        }

        private void SetSelectedButton(object o)
        {
            SelectedIndex = (int)o;
        }

        private void SetSelectedState(int index, bool isSelected)
        {
            if (ButtonLayout.Children.Count <= index)
            {
                return; //Out of bounds
            }

            var frame = (Frame)ButtonLayout.Children[index];

            frame.HasShadow = isSelected;

            frame.BackgroundColor = isSelected ? SelectedFrameBackgroundColor : ViewBackgroundColor;

            var button = (Button)frame.Content;

            button.BackgroundColor = isSelected ? SelectedBackgroundColor : BackgroundColor;
            button.TextColor = isSelected ? SelectedTextColor : TextColor;
            button.BorderColor = isSelected ? SelectedBorderColor : BorderColor;
        }
    }
}
