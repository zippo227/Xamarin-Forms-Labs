using System;

namespace Xamarin.Forms.Labs.Behaviors
{
    using Xamarin.Forms.Labs.Controls;

    /// <summary>
    /// Geture result sent along with the Paramater for 
    /// in a Gesture Exectue call.
    /// Also the EventArgs type for OnGesture
    /// </summary>
    public class GestureResult
    {
        /// <summary>
        /// The gesture type
        /// </summary>
        public GestureType GestureType { get; set; }
        /// <summary>
        /// The direction (if any) of the direction
        /// </summary>
        public Directionality Direction { get; set; }
        /// <summary>
        /// The point, relative to the start view where the 
        /// gesture started
        /// </summary>
        public Point Origin { get; internal set; }
        /// <summary>
        /// The view that the gesture started in
        /// </summary>
        public View StartView { get; internal set; }
        /// <summary>
        /// The Vector Length of the gesture (if appropiate)
        /// </summary>
        public Double Length { get; internal set; }
        /// <summary>
        /// The Vertical distance the gesture travelled
        /// </summary>
        public Double VerticalDistance { get; internal set; }
        /// <summary>
        /// The horizontal distance the gesture travelled
        /// </summary>
        public Double HorizontalDistance { get; internal set; }

    }
}
