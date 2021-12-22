using currency.mobile.Views;
using Xamarin.Forms;

namespace currency.mobile {
  public partial class AppShell : Shell {
    public AppShell() {
      InitializeComponent();

      //main pages
      Routing.RegisterRoute(nameof(NbuPage), typeof(NbuPage));
      Routing.RegisterRoute(nameof(PrivatPage), typeof(PrivatPage));
      Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
      Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));

      //main tabs
      mainTab.Children.Add(new PrivatPage());
      mainTab.Children.Add(new NbuPage());
      mainTab.Children.Add(new AboutPage());
      // mainTab.Children.Add(new MenuPage());
    }
  }
}
