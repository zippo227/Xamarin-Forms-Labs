using System;

namespace Xamarin.Forms.Labs.Controls
{
    public class SegmentControl : ContentView
    {
        private StackLayout layout;

        private Color tintColor = Color.Black;

        public Color TintColor
        {
            get { return tintColor; }
            set
            {
                tintColor = value;

                if (layout == null)
                {
                    return;
                }

                layout.BackgroundColor = tintColor;

                for (var iBtn = 0; iBtn < layout.Children.Count; iBtn++)
                {
                    SetSelectedState(iBtn, iBtn == selectedSegment, true);
                }
            }
        }

        private int selectedSegment;

        public int SelectedSegment
        {
            get
            {
                return selectedSegment;
            }
            set
            {
                //reset the original selected segment
                if (value == selectedSegment)
                {
                    return;
                }

                SetSelectedState(selectedSegment, false);
                selectedSegment = value;

                if (value < 0 || value >= layout.Children.Count)
                {
                    return;
                }

                SetSelectedState(selectedSegment, true);
            }
        }

        public event EventHandler<int> SelectedSegmentChanged;

        private Command ClickedCommand;

        public SegmentControl()
        {
            layout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(1),
                Spacing = 1
            };

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Start;
            Padding = new Thickness(0, 0);
            Content = layout;

            selectedSegment = 0;
            ClickedCommand = new Command(SetSelectedSegment);
        }

        public void AddSegment(string segmentText)
        {
            // TODO: TextColor needs to be a bound property
            var button = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderColor = TintColor,
                BorderRadius = 0,
                BorderWidth = 0,
                Text = segmentText,
                TextColor = TintColor,
                BackgroundColor = Color.White,
                Command = ClickedCommand,
                CommandParameter = layout.Children.Count,
            };

            layout.BackgroundColor = TintColor;
            layout.Children.Add(button);

            SetSelectedState(layout.Children.Count - 1, layout.Children.Count - 1 == selectedSegment);
        }

        private void SetSelectedSegment(object o)
        {
            var selectedIndex = (int)o;

            SelectedSegment = selectedIndex;

            if (SelectedSegmentChanged != null)
            {
                SelectedSegmentChanged(this, selectedIndex);
            }
        }

        public void SetSegmentText(int iSegment, string segmentText)
        {
            if (iSegment >= layout.Children.Count || iSegment < 0)
            {
                throw new IndexOutOfRangeException("SetSegmentText: Attempted to change segment text for a segment doesn't exist.");
            }

            ((Button)layout.Children[iSegment]).Text = segmentText;
        }

        private void SetSelectedState(int indexer, bool isSelected, bool setBorderColor = false)
        {
            if (layout.Children.Count <= indexer)
            {
                return; //Out of bounds
            }

            var button = (Button)layout.Children[indexer];

            // TODO: TextColor needs to be a bound property
            if (isSelected)
            {
                button.BackgroundColor = TintColor;
                button.TextColor = Color.White;
            }
            else
            {
                button.BackgroundColor = Color.White;
                button.TextColor = TintColor;
            }

            if (setBorderColor)
            {
                button.BorderColor = TintColor;
            }
        }
    }
}