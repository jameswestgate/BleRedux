using System;
using System.Threading.Tasks;
using BleRedux.Shared;
using CoreBluetooth;
using Foundation;
using Plugin.BluetoothLE.Server;
using Xamarin.Forms;

[assembly: Dependency(typeof(BleRedux.iOS.BleServer))]
namespace BleRedux.iOS
{
    [Preserve(AllMembers = true)]
    public class BleServer: IBleServer
    {
        private CBPeripheralManager _manager;
        private GattServer _server;
        private Advertiser _advertiser;
        private Plugin.BluetoothLE.AdapterStatus _status;

        public event EventHandler<Plugin.BluetoothLE.AdapterStatus> StatusChanged;

        public void Initialise()
        {
            _manager = new CBPeripheralManager();
            _status = Plugin.BluetoothLE.AdapterStatus.Unknown;

            _manager.StateUpdated += (object sender, EventArgs e) =>
            {
                var result = Plugin.BluetoothLE.AdapterStatus.Unknown;

                Enum.TryParse(_manager.State.ToString(), true, out result);

                _status = result;

                StatusChanged?.Invoke(this, result);
            };
        }

        public Plugin.BluetoothLE.AdapterStatus GetStatus()
        {
            return _status;
        }

        public IGattService CreateService(Guid uuid, bool primary)
        {
            if (_server == null) _server = new GattServer(_manager);

            return new GattService(_manager, _server, uuid, primary);
        }

        public void AddService(IGattService service)
        {
            _server.AddService(service);
        }

        public void StartAdvertiser(Plugin.BluetoothLE.Server.AdvertisementData advertisingData)
        {
            if (_advertiser != null) StopAdvertiser();

            _advertiser = new Advertiser(_manager);

            _advertiser.Start(advertisingData);
        }

        public void StopAdvertiser()
        {
            if (_advertiser == null) return;
            _advertiser.Stop();
        }

        public bool RequestPermissions()
        {
            throw new NotImplementedException();
        }
    }
}