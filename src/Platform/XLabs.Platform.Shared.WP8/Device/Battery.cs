namespace XLabs.Platform.Device
{
    using System;

    /// <summary>
    /// Windows Phone Battery class.
    /// </summary>
    public partial class Battery
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level in percentage 0-100.</value>
        public int Level
        {
            get
            {
#if !NETFX_CORE
				return Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercent;
#else
				return 0;
#endif
            }
        }

        /// <summary>
        /// Called when [remaining charge percent changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="o">The o.</param>
        private void OnRemainingChargePercentChanged(object sender, object o)
        {
            onLevelChange?.Invoke(sender, Level);
        }

        /// <summary>
        /// Handles the <see cref="E:PowerSourceChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnPowerSourceChanged(object sender, EventArgs eventArgs)
        {
            onChargerStatusChanged?.Invoke(sender, Charging);
        }

        #region partial implementations

        /// <summary>
        /// Starts the level monitoring.
        /// </summary>
        partial void StartLevelMonitoring()
        {
#if !NETFX_CORE
			Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged += OnRemainingChargePercentChanged;
#else
#endif
        }

        /// <summary>
        /// Stops the level monitoring.
        /// </summary>
        partial void StopLevelMonitoring()
        {
#if !NETFX_CORE
			Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged -= OnRemainingChargePercentChanged;
#else
#endif
		}

		#endregion
	}
}