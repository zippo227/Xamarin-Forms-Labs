namespace XLabs.Platform.Droid.Device
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Android.Bluetooth;

	using XLabs.Platform.Device;

	public class BluetoothHub : IBluetoothHub
    {
        readonly BluetoothAdapter adapter;

        public bool Enabled
        {
            get
            {
                return this.adapter.IsEnabled;
            }
        }

        public BluetoothHub() : this(BluetoothAdapter.DefaultAdapter) { }

        public BluetoothHub(BluetoothAdapter adapter)
        {
            this.adapter = adapter;

            this.OpenSettings = new Command(
                o => o.StartActivityForResult(new Android.Content.Intent(BluetoothAdapter.ActionRequestEnable)),
                o => true
                );
        }

        #region IBluetoothHub implementation
        public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
        {
            return await Task.Factory.StartNew(() => this.adapter.BondedDevices.Select(a => new AndroidBluetoothDevice(a)).ToList());
        }

        public System.Windows.Input.ICommand OpenSettings
        {
            get;
            private set;
        }
        #endregion
    }
}