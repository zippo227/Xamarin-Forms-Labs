using System;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Device
{
    public interface IDevice
    {
        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        IDisplay Display { get; }

        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        IPhoneService PhoneService { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        string FirmwareVersion { get; }

        string HardwareVersion { get; }

        string Manufacturer { get; }
    }
}

