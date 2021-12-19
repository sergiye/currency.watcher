using System;
using currency.mobile.Services;
using Xamarin.Forms;

namespace currency.mobile {
  public partial class App : Application {

    public App() {
      InitializeComponent();

      DependencyService.Register<DataStore>();

      var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var nbuProvider = new NbuProvider(appDataPath);
      DependencyService.RegisterSingleton<ICurrencyProvider<NbuRateItem>>(nbuProvider);
      DependencyService.RegisterSingleton(nbuProvider);

      MainPage = new AppShell();
    }

    protected override void OnStart() {
    }

    protected override void OnSleep() {
    }

    protected override void OnResume() {
    }
  }
}
