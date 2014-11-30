using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;

namespace Xamarin.Forms.Labs
{
    public class BluetoothDevice : IBluetoothDevice
    {
        private readonly PeerInformation device;
        private StreamSocket socket;

        public BluetoothDevice(PeerInformation peerInfo)
        {
            this.device = peerInfo;
        }

        public string Name
        {
            get { return this.device.DisplayName; }
        }

        public string Address
        {
            get { return this.device.HostName.DisplayName; }
        }

        public Stream InputStream
        {
            get
            {
                return this.socket == null ? null : this.socket.InputStream.AsStreamForRead();
            }
        }

        public Stream OutputStream
        {
            get
            {
                return this.socket == null ? null : this.socket.OutputStream.AsStreamForWrite();
            }
        }

        public async Task Connect()
        {
            if (this.socket != null)
            {
                this.socket.Dispose();
            }

            try
            {
                this.socket = new StreamSocket();

                await this.socket.ConnectAsync(device.HostName, device.ServiceName);

                //return true;
            }
            catch //(Exception ex)
            {
                if (this.socket != null)
                {
                    this.socket.Dispose();
                    this.socket = null;
                }

                throw;
            }
        }

        public void Disconnect()
        {
            if (this.socket != null)
            {
                this.socket.Dispose();
                this.socket = null;
            }
        }
    }
}