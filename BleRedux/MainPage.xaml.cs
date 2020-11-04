using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using BleRedux.Shared;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using System.Reactive.Linq;

namespace BleRedux
{
    public partial class MainPage : ContentPage
    {
        IBleServer _server;
        IDisposable _notifyBroadcast = null;
        Plugin.BluetoothLE.Server.IGattService _service;

        public MainPage()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (_server == null)
            {
                Console.WriteLine("CREATING SERVER");
                _server = DependencyService.Get<IBleServer>();
                _server.Initialise();

                _server.StatusChanged += Peripheral_StatusChanged;
            }
        }

        private void Peripheral_StatusChanged(object sender, AdapterStatus status)
        {
            try
            {
                Console.WriteLine($"GOT STATUS CHANGED: {status}");

                if (status != AdapterStatus.PoweredOn) return;
                if (_service != null) return;

                Console.WriteLine($"CREATING SERVICE");
                _service = _server.CreateService(new Guid(BluetoothConstants.kFidoServiceUUID), true);

                Console.WriteLine($"ADDING CHARACTERISTICS");
                var characteristic = _service.AddCharacteristic(
                    Guid.NewGuid(),
                    CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse,
                    GattPermissions.Read | GattPermissions.Write
                );

                var notifyCharacteristic = _service.AddCharacteristic
                (
                    Guid.NewGuid(),
                    CharacteristicProperties.Indicate | CharacteristicProperties.Notify,
                    GattPermissions.Read | GattPermissions.Write
                );

                Console.WriteLine($"SUBSCRIBING TO DEVICE SUBS");

                notifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
                {
                    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";

                    if (_notifyBroadcast == null)
                    {
                        this._notifyBroadcast = Observable
                            .Interval(TimeSpan.FromSeconds(1))
                            .Where(x => notifyCharacteristic.SubscribedDevices.Count > 0)
                            .Subscribe(_ =>
                            {
                                Console.WriteLine("Sending Broadcast");
                                var dt = DateTime.Now.ToString("g");
                                var bytes = Encoding.UTF8.GetBytes(dt);
                                notifyCharacteristic.Broadcast(bytes);
                            });
                    }
                });

                Console.WriteLine($"SUBSCRIBING TO READ");
                characteristic.WhenReadReceived().Subscribe(x =>
                {
                    Console.WriteLine($"READ RECEIVED");
                    var write = "HELLO";

                    // you must set a reply value
                    x.Value = Encoding.UTF8.GetBytes(write);
                    x.Status = GattStatus.Success; // you can optionally set a status, but it defaults to Success
                });

                Console.WriteLine($"SUBSCRIBING TO WRITE");
                characteristic.WhenWriteReceived().Subscribe(x =>
                {
                    var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);

                    Console.WriteLine($"WRITE RECEIVED: {write}");
                });

                //Also start advertiser (on ios)
                var advertisingData = new AdvertisementData
                {
                    LocalName = "FIDO Test Server",
                    ServiceUuids = new List<Guid> { new Guid(BluetoothConstants.kFidoServiceUUID) } //new Guid(DeviceInformationService),
                };

                //Now add the service
                Console.WriteLine($"ADDING SERVICE");
                _server.AddService(_service);

                Console.WriteLine($"STARTING ADVERTISER");
                _server.StartAdvertiser(advertisingData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex}");
            }
        }
    }
}
