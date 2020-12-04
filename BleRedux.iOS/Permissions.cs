using System;
using BleRedux.Shared;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(BleRedux.iOS.Permissions))]
namespace BleRedux.iOS
{
    [Preserve(AllMembers = true)]
    public class Permissions: IPermissions
    {
        public void RequestPermissions()
        {
            throw new NotImplementedException();
        }
    }
}