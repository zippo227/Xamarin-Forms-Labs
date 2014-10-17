using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs
{
    public interface IBluetoothDevice
    {
        string Name { get; }
        string Address { get; }

        Stream InputStream { get; }
        Stream OutputStream { get; }

        Task Connect();
        void Disconnect(); 
    }
}