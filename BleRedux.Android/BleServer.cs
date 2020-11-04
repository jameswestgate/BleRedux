using System;
using Android.Runtime;
using Xamarin.Forms;
using BleRedux.Shared;
using Plugin.BluetoothLE.Server;

[assembly: Dependency(typeof(BleRedux.Droid.BleServer))]
namespace BleRedux.Droid
{
    [Preserve(AllMembers = true)]
    public class BleServer: IBleServer
    {
        public event EventHandler<Plugin.BluetoothLE.AdapterStatus> StatusChanged;

        public void Initialise()
        {

        }

        public IGattService CreateService(Guid uuid, bool primary)
        {
            throw new NotImplementedException();
        }

        public void AddService(IGattService service)
        {
            throw new NotImplementedException();
        }

        public void StartAdvertiser(AdvertisementData advertisingData)
        {
            throw new NotImplementedException();
        }

        public void StopAdvertiser()
        {
            throw new NotImplementedException();
        }
    }
}
