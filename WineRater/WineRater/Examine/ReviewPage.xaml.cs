using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineRater.Examine
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReviewPage : ContentPage
	{
		public ReviewPage ()
		{
			InitializeComponent ();
      //todo save to the database
		}
	}
}