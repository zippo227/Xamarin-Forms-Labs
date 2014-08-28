using System.Linq;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace Xamarin.Forms.Labs
{
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