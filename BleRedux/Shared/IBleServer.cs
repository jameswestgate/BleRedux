using System;
using System.Threading.Tasks;
using Plugin.BluetoothLE.Server;

namespace BleRedux.Shared
{
    public interface IBleServer
    {
        void Initialise();

        Plugin.BluetoothLE.AdapterStatus GetStatus();
        event EventHandler<Plugin.BluetoothLE.AdapterStatus> StatusChanged;

        IGattService CreateService(Guid uuid, bool primary);
        void AddService(IGattService service);
        void StartAdvertiser(AdvertisementData advertisingData);
        void StopAdvertiser();
    }
}
