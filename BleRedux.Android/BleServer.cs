using System;

using Android.Bluetooth;
using Android.Content.PM;
using Android.Runtime;

using BleRedux.Shared;

using Plugin.BluetoothLE.Server;
using Plugin.BluetoothLE.Server.Internals;

using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BleRedux.Droid.BleServer))]
namespace BleRedux.Droid
{
    [Preserve(AllMembers = true)]
    public class BleServer: IBleServer
    {
        private GattContext _context;
        private BluetoothAdapter _adapter;
        private GattServer _server;
        private Advertiser _advertiser;

        public event EventHandler<Plugin.BluetoothLE.AdapterStatus> StatusChanged;

        public void Initialise()
        {
            _context = new GattContext();
            _adapter = _context.Manager.Adapter;

            //Check if bluetooth is enabled
            if (_adapter == null || !_adapter.IsEnabled)
            {
                Console.WriteLine($"PLEASE ENABLE BLUETOOTH.");
            }

            var activity = Platform.CurrentActivity;
            var hasBle = activity.PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe);

            if (!hasBle)
            {
                Console.WriteLine($"DEVICE DOES NOT SUPPORT BLE.");
            }
        }

        public Plugin.BluetoothLE.AdapterStatus GetStatus()
        {
            return (_adapter.State == State.On) ? Plugin.BluetoothLE.AdapterStatus.PoweredOn : Plugin.BluetoothLE.AdapterStatus.Unknown;
        }

        //https://developer.android.com/guide/topics/connectivity/bluetooth-le
        public IGattService CreateService(Guid uuid, bool primary)
        {
            if (!MainThread.IsMainThread)
            {
                Console.WriteLine("CALL CreateService ON THE MAIN THREAD.");
                return null;
            }

            //This can return null in the Ble Plugin
            //var gattServer = _context.Manager.OpenGattServer(activity, _context.Callbacks);

            //if (gattServer == null)
            //{
            //    Console.WriteLine("A GATT SERVER OBJECT COULD NOT BE CREATED.");
            //    return null;
            //}

            if (_server == null) _server = new GattServer();

            return new GattService(_context, _server, uuid, primary);
        }

        public void AddService(IGattService service)
        {
            _server.AddService(service);
        }

        public void StartAdvertiser(AdvertisementData advertisingData)
        {
            if (_advertiser != null) StopAdvertiser();

            _advertiser = new Advertiser();
            _advertiser.Start(advertisingData);
        }

        public void StopAdvertiser()
        {
            if (_advertiser == null) return;
            _advertiser.Stop();
        }
    }
}
