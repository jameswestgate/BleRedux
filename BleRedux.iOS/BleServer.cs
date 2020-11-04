using System;
using BleRedux.Shared;
using Xamarin.Forms;

using Foundation;

using Plugin.BluetoothLE.Server;
using CoreBluetooth;
using System.Reactive.Linq;
using System.Text;

[assembly: Dependency(typeof(BleRedux.iOS.BleServer))]
namespace BleRedux.iOS
{
    [Preserve(AllMembers = true)]
    public class BleServer: IBleServer
    {
        private CBPeripheralManager _manager;
        private GattServer _server;
        private Advertiser _advertiser;

        public event EventHandler<Plugin.BluetoothLE.AdapterStatus> StatusChanged;

        public void Initialise()
        {
            _manager = new CBPeripheralManager();

            _manager.StateUpdated += (object sender, EventArgs e) =>
            {
                var result = Plugin.BluetoothLE.AdapterStatus.Unknown;

                Enum.TryParse(_manager.State.ToString(), true, out result);

                StatusChanged?.Invoke(this, result);
            };
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
            //advertisingData.ManufacturerData = new ManufacturerData();
            if (_advertiser != null) StopAdvertiser();

            _advertiser = new Advertiser(_manager);

            _advertiser.Start(advertisingData);
        }

        public void StopAdvertiser()
        {
            if (_advertiser == null) return;
            _advertiser.Stop();
        }
    }
}