using System;
using Android;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using BleRedux.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BleRedux.Droid.Permissions))]
namespace BleRedux.Droid
{
    [Preserve(AllMembers = true)]
    public class Permissions: IPermissions
    {
        //https://stackoverflow.com/questions/36784663/requesting-multiple-bluetooth-permissions-in-android-marshmallow
        public void RequestPermissions()
        {
            var mainActivity = (MainActivity)Platform.CurrentActivity;

            if (ContextCompat.CheckSelfPermission(mainActivity, Manifest.Permission.Bluetooth) == (int)Permission.Granted) return;

            ActivityCompat.RequestPermissions(mainActivity, new String[] { Manifest.Permission.Bluetooth }, 1);

            if (ContextCompat.CheckSelfPermission(mainActivity, Manifest.Permission.AccessCoarseLocation) == (int)Permission.Granted) return;
            
            ActivityCompat.RequestPermissions(mainActivity, new String[] { Manifest.Permission.AccessCoarseLocation }, 2);
        }
    }
}
