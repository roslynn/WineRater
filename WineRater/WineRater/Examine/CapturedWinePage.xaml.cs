using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineRater.Examine
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CapturedWinePage : ContentPage
  {
    public CapturedWinePage(byte[] image, string description)
    {
      InitializeComponent();
      WinePicture.Source = ImageSource.FromStream(() => new MemoryStream(image));
      ReviewLabel.LabelContent = description;
    }

    public async void ReviewButtonClicked(object sender, System.EventArgs e)
    {
      await Navigation.PushAsync(new ReviewPage());
    }

    private async void SaveButtonClicked(object sender, System.EventArgs e)
    {
      await Navigation.PushAsync(new SavePage());
    }
  }
}