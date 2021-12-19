using Xamarin.Essentials;
using Xamarin.Forms;

namespace currency.mobile.Views {

  public partial class AboutPage : ContentPage {

    public AboutPage() {
      InitializeComponent();
    }

    private async void Button_Clicked(object sender, System.EventArgs e) {
      await Browser.OpenAsync("https://github.com/sergiye/currency.watcher");
    }
  }
}