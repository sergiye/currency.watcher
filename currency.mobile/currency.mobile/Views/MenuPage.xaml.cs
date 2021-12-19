using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace currency.mobile.Views {

  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class MenuPage : ContentPage {

    public MenuPage() {
      InitializeComponent();
      //this.BindingContext = new MenuViewModel();
    }

    private async void Button_Clicked(object sender, EventArgs e) {
      await Shell.Current.GoToAsync($"{nameof(NbuPage)}");
    }

    private async void AboutButton_Clicked(object sender, EventArgs e) {
      // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
      await Shell.Current.GoToAsync($"{nameof(AboutPage)}");
    }
  }
}