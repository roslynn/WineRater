using System;
using System.Reflection;
using Xamarin.Forms;
using WineRater.Examine;
using WineRater.CommonTypes;
using Ninject;
using System.IO;

namespace WineRater
{
  public partial class MainPage : ContentPage
  {
    private ISaveToLocalStorage _fileSave;

    public MainPage()
    {
#if DEBUG
      CheckResourcesFound();
#endif
      InitializeComponent();

      NavigationPage.SetHasBackButton(this, false);
      NavigationPage.SetHasNavigationBar(this, false);

      //todo unhook this in some kind of dispose, if exists
      ExamineWineButton.Clicked += async (sender, args) =>
      {
        try
        {
          var examinePage = new ExamineWinePage();
          await examinePage.Init();
          await Navigation.PushAsync(examinePage);
          NavigationPage.SetHasBackButton(examinePage, false);
          NavigationPage.SetHasNavigationBar(examinePage, false);
        }
        catch (Exception e)
        {
          Console.WriteLine("Examine page initialization went wrong");
          Console.WriteLine(e.Message);
        }
      };
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      _fileSave = Bootstrap.IoC.Get<ISaveToLocalStorage>();
      var picPath = Path.Combine(_fileSave.DocumentsFolder, Bootstrap.Temp_File_Name);

      ReviewedWineButton.Image =/* Path.Combine(picPath,  @"WineRater.Resources.*/"wine_database_pic.png";
      PendingWineButton.Image = /*  Path.Combine(picPath, @"WineRater.Resources.*/"wine_to_review.png";
      YourProfileButton.Image = /*  Path.Combine(picPath, @"WineRater.Resources.*/"persona_picture.png";
      ExamineWineButton.Image = /*  Path.Combine(picPath, @"WineRater.Resources.*/"camera_pic.png";
      FriendsButton.Image =     /* Path.Combine(picPath,  @"WineRater.Resources.*/"friends_picture.png";
    }

    private void CheckResourcesFound()
    {
      // ...
      // NOTE: use for debugging, not in released app code!
      var assembly = typeof(MainPage).GetTypeInfo().Assembly;
      foreach (var res in assembly.GetManifestResourceNames())
      {
        System.Diagnostics.Debug.WriteLine("found resource: " + res);
      }
    }
  }
}
