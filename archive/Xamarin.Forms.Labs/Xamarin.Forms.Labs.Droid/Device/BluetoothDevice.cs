using System.IO;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Util;
using Java.Util;

namespace Xamarin.Forms.Labs
{
    public class AndroidBluetoothDevice : IBluetoothDevice
    {
        private const string BtUuid = "00001101-0000-1000-8000-00805F9B34FB";

        private readonly BluetoothDevice device;
        private BluetoothSocket socket;
        private readonly UUID uuid;

        public AndroidBluetoothDevice(BluetoothDevice device)
        {
            this.device = device;
            this.uuid = UUID.RandomUUID();
        }

        #region IBluetoothDevice implementation

        public async Task Connect()
        {
            if (this.socket == null)
            {
                this.socket = this.device.CreateRfcommSocketToServiceRecord(this.uuid);
            }
            
            await this.socket.ConnectAsync();
        }

        public void Disconnect()
        {
            if (this.socket != null)
            {
                try
                {
                    this.socket.Close();
                    this.socket = null;
                }
                catch (Java.IO.IOException ex)
                {
                    Log.Error("BluetoothSocket.Close()", ex.Message);
                }
            }
        }

        public string Name
        {
            get { return this.device.Name; }
        }

        public string Address
        {
            get { return this.device.Address; }
        }

        public Stream InputStream 
        { 
            get 
            { 
                return (this.socket == null) ? null : this.socket.InputStream; 
            } 
        }

        public Stream OutputStream 
        { 
            get 
            { 
                return (this.socket == null) ? null : this.socket.OutputStream; 
            } 
        }

        #endregion
        
    }
}