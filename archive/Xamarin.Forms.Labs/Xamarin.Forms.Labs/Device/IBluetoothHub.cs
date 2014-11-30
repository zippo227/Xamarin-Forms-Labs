using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Forms.Labs
{
    public interface IBluetoothHub
    {
        bool Enabled { get; }

        Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices();

        ICommand OpenSettings { get; }
    }
}