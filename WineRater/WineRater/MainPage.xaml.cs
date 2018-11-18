using System;
using System.Reflection;
using Xamarin.Forms;

namespace WineRater
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
#if DEBUG
      CheckResourcesFound();
#endif
      InitializeComponent();

      //todo unhook this in some kind of dispose, if exists
      ExamineWineButton.Clicked += async (sender, args) =>
      {
        try
        {
          var examinePage = new ExamineWinePage();
          await examinePage.Init();
          await Navigation.PushAsync(examinePage);
        }
        catch (Exception e)
        {
          Console.WriteLine("Examine page initialization went wrong");
          Console.WriteLine(e.Message);
        }
      };
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
