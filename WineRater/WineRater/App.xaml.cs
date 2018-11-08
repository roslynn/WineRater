using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WineRater
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      StartPage = new MainPage();
      MainPage = new NavigationPage(StartPage);
    }

    public MainPage StartPage { get; }

    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
