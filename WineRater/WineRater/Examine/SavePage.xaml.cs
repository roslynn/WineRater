using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineRater.Examine
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SavePage : ContentPage
	{
		public SavePage ()
		{
			InitializeComponent ();
		}

    protected override bool OnBackButtonPressed()
    {
      Navigation.PopToRootAsync();
      return true;
    }

  }
}