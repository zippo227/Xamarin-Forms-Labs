namespace XLabs.Platform.WP8.Device
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Windows.Networking.Proximity;

	using Microsoft.Phone.Tasks;

	public class BluetoothHub : IBluetoothHub
    {
        public BluetoothHub()
        {
            this.OpenSettings = new Command(
                o => new ConnectionSettingsTask() { ConnectionSettingsType = ConnectionSettingsType.Bluetooth }.Show(),
                o => true
                );
        }

        public bool Enabled
        {
            get { return PeerFinder.AllowBluetooth; }
        }

        public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
        {
            PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = string.Empty;
            var devices = await PeerFinder.FindAllPeersAsync();
            return devices.Select(a => new BluetoothDevice(a)).ToList();
        }

        public System.Windows.Input.ICommand OpenSettings
        {
            get; 
            private set;
        }
    }
}