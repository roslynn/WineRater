using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Support.V4.App;
using Android;

namespace WineRater.Droid
{
  [Activity(Label = "WineRater", Icon = "@mipmap/icon", RoundIcon = "@mipmap/round_icon",
    Theme = "@style/MainTheme",
    MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    private App _instance;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;
      CrossCurrentActivity.Current.Init(this, savedInstanceState);

      base.OnCreate(savedInstanceState);
      global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

      _instance = new App();
      LoadApplication(_instance);

      Initialize();
    }

    private void Initialize()
    {
      CheckCameraPermissions();
    }

    private void CheckCameraPermissions()
    {
      var cameraPermission = Manifest.Permission.Camera;
      if (ActivityCompat.CheckSelfPermission(this, cameraPermission) == Permission.Denied)
      {
        ActivityCompat.RequestPermissions(this, new[] { cameraPermission }, 1);
      }
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
      Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }


    protected override void OnDestroy()
    {
      base.OnDestroy();
    }
  }
}