using System;


using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Locations;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Gms.Common;
using Android.Support.V4.Content;
using Android;


namespace TapThis.Droid
{

    /// <summary>
    /// Handles connecting to Google Services. Disconnectiong. Checking user permission and requesting if denied. 
    /// </summary>
    /// 
    [Activity(Label = "TapThis", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,
        GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {

        private LocationRequest Location_Request;
        private GoogleApiClient GoogleApi_Client;

        //Might not need
        readonly string[] PermissionsLocation =
            {
            Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
             };

        const int Request_LocationId = 0;



        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);

            RequestPermissions(PermissionsLocation, Request_LocationId);

            GoogleApi_Client = new GoogleApiClient.Builder(Application.Context, this, this)
                                .AddApi(Android.Gms.Location.LocationServices.API)
                                .Build();


            /*
            if (GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this) == ConnectionResult.Success)
            {
                Log.Info("MainActivity", "Google Play Services is installed on this device.");
            }
            */
            

            LoadApplication(new TapThis.App());

        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("OnResume", "OnResume");

            //Checking for location permission.
            //If allowed, connect to the Google API and begin recieving location updates
            //If not, request permission and handle the response.
            if (PermissionChecker.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                Log.Debug("OnResume", "Shit");
                GoogleApi_Client.Connect();
            }
        }

        protected override async void OnPause()
        {
            base.OnPause();
            Log.Debug("OnPause", "OnPause");

            if (GoogleApi_Client.IsConnected)
            {
                Log.Debug("OnPause", "Fuck");
                // stop location updates, passing in the LocationListener
                await LocationServices.FusedLocationApi.RemoveLocationUpdates(GoogleApi_Client, this);

                GoogleApi_Client.Disconnect();
            }
        }


        ////Interface methods
        public async void OnConnected(Bundle bundle)
        {
            Log.Info("LocationClient", "OnConnected");
            Location_Request = new LocationRequest();
            Location_Request.SetPriority(100);
            Location_Request.SetFastestInterval(500);
            Location_Request.SetInterval(1000);

            await LocationServices.FusedLocationApi.RequestLocationUpdates(GoogleApi_Client, Location_Request, this);
            Location Last_Location = LocationServices.FusedLocationApi.GetLastLocation(GoogleApi_Client);

            TapThis.App.User_Location.Latitude = Last_Location.Latitude;
            TapThis.App.User_Location.Longitude = Last_Location.Longitude;
            TapThis.App.User_Location.Is_Enabled = true;
            TapThis.App.User_Location.Connected();
            OnLocationChanged(Last_Location);

            Log.Info("LocationClient", "Now connected to client");
        }



        public void OnDisconnected()
        {
            // This method is called when we disconnect from the LocationClient.
            TapThis.App.User_Location.Is_Enabled = false;
            Log.Info("LocationClient", "Disconnected");
        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {
            // This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
            // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            Log.Info("LocationClient", "Connection failed, attempting to reach google play services");
        }

        public void OnLocationChanged(Location location)
        {
            // This method returns changes in the user's location if they've been requested
            //TapThis.App.User_Latitude = location.Latitude;
            // TapThis.App.User_Longitude = location.Longitude;
            TapThis.App.User_Location.Latitude = location.Latitude;
            TapThis.App.User_Location.Longitude = location.Longitude;

            Log.Debug("LocationClient", "Location updated");
        }

        public void OnConnectionSuspended(int i)
        {

        }

    }
}

