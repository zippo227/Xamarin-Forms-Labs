using System;
using System.ComponentModel;
using Xamarin.Forms;
//using PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    ///     Delegate CurrentPageChangingEventHandler.
    /// </summary>
    public delegate void CurrentPageChangingEventHandler();

    /// <summary>
    ///     Delegate CurrentPageChangedEventHandler.
    /// </summary>
    public delegate void CurrentPageChangedEventHandler();

    public delegate void SwipeLeftEventHandler();
    public delegate void SwipeRightEventHandler();

    /// <summary>
    ///     Class ExtendedTabbedPage.
    /// </summary>
    public class ExtendedTabbedPage : TabbedPage
    {
        public bool SwipeEnabled;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtendedTabbedPage" /> class.
        /// </summary>
        public ExtendedTabbedPage()
        {
            PropertyChanging += OnPropertyChanging;
            PropertyChanged += OnPropertyChanged;
            OnSwipeLeft += SwipeLeft;
            OnSwipeRight += SwipeRight;

            SwipeEnabled = false;
        }

        /// <summary>
        ///     Occurs when [current page changing].
        /// </summary>
        public event CurrentPageChangingEventHandler CurrentPageChanging;

        /// <summary>
        ///     Occurs when [current page changed].
        /// </summary>
        public event CurrentPageChangedEventHandler CurrentPageChanged;

        /// <summary>
        /// Occurs when the TabbedPage is swipped Right
        /// </summary>
        public event EventHandler OnSwipeRight;

        /// <summary>
        /// Occurs when the TabbedPage is swipped Left
        /// </summary>
        public event EventHandler OnSwipeLeft;

        /// <summary>
        /// Invokes the item SwipeRight event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item">Item.</param>
        public void InvokeSwipeRightEvent(object sender, object item)
        {
            if (OnSwipeRight != null)
            {
                OnSwipeRight.Invoke(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Invokes the SwipeLeft event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item">Item.</param>
        public void InvokeSwipeLeftEvent(object sender, object item)
        {
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft.Invoke(sender, new EventArgs());
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:PropertyChanging" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangingEventArgs" /> instance containing the event data.</param>
        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                RaiseCurrentPageChanging();
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPage")
            {
                RaiseCurrentPageChanged();
            }
        }

        /// <summary>
        /// Move to the previous Tabbed Page
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        private void SwipeLeft(object a, EventArgs e)
        {
            if (SwipeEnabled)
            {
                PreviousPage();
            }
        }

        /// <summary>
        /// Move to the next Tabbed Page
        /// </summary>
        /// <param name="a"></param>
        /// <param name="e"></param>
        private void SwipeRight(object a, EventArgs e)
        {
            if (SwipeEnabled)
            {
                NextPage();
            }
        }
        /// <summary>
        ///     Raises the current page changing.
        /// </summary>
        private void RaiseCurrentPageChanging()
        {
            var handler = CurrentPageChanging;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///     Raises the current page changed.
        /// </summary>
        private void RaiseCurrentPageChanged()
        {
            var handler = CurrentPageChanged;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Calculate the 
        /// </summary>
        private void NextPage()
        {
            var currentPage = Children.IndexOf(CurrentPage);

            currentPage++;

            if (currentPage > Children.Count - 1)
            {
                currentPage = 0;
            }

            CurrentPage = Children[currentPage];
        }

        private void PreviousPage()
        {
            var currentPage = Children.IndexOf(CurrentPage);

            currentPage--;

            if (currentPage < 0)
            {
                currentPage = Children.Count - 1;
            }

            CurrentPage = Children[currentPage];
        }
    }
}