using System;
using System.Globalization;
using System.Linq;
using currency.mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace currency.mobile.Views {

  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PrivatPage : ContentPage {

    private DateTime lastHistoryGet;

    public PrivatPage() {

      InitializeComponent();

      var grid = new Grid {
        Padding = 5,
        RowDefinitions = new RowDefinitionCollection(), //todo: in loop
        ColumnDefinitions = new ColumnDefinitionCollection {
          new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
          new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)},
          new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)},
          new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)},
          new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)},
        },
      };
      
      var refreshView = new RefreshView();

      async void ExecuteRefresh() {

        try {
          // IsRefreshing is true
          var today = DateTime.Today.Date;
          if (lastHistoryGet == today)
            return;

          var response = await Helper.GetJsonData("https://otp24.privatbank.ua/v3/api/1/info/currency/history", 30, "POST");
          if (string.IsNullOrEmpty(response)) return;

          var historyData = response.FromJson<Privat24HistoryResponse>();
          if (historyData?.Data?.History == null) return;
          if (historyData.Data.History.Length <= 0) return;

          // Refresh data here
          grid.Children.Clear();
          grid.RowDefinitions.Clear();

          grid.Children.Add(Utils.GetLabel("Date", 14, Color.LightCyan), 0, grid.RowDefinitions.Count);
          grid.Children.Add(Utils.GetLabel("Buy", 14, Color.LightCyan), 1, grid.RowDefinitions.Count);
          grid.Children.Add(Utils.GetLabel("B delta", 14, Color.LightCyan), 2, grid.RowDefinitions.Count);
          grid.Children.Add(Utils.GetLabel("Sell", 14, Color.LightCyan), 3, grid.RowDefinitions.Count);
          grid.Children.Add(Utils.GetLabel("S delta", 14, Color.LightCyan), 4, grid.RowDefinitions.Count);
          grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

          for (var currencyIndex = 0; currencyIndex < 2; currencyIndex++) {
            var currencyCode = Helper.GetCurrencyName(currencyIndex);
            grid.Children.Add(Utils.GetLabel(currencyCode.ToUpper(), 16, Color.LightCyan), 2, grid.RowDefinitions.Count);
            grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            var data = historyData.Data.History.Where(i => i.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
              .OrderByDescending(x => {
                DateTime.TryParseExact(x.Date, "dd-MM-yyyy", null, DateTimeStyles.AllowWhiteSpaces, out var dt);
                x.DateParsed = dt;
                return dt;
              }).ToArray();
            if (data.Length == 0) continue;

            foreach (var item in data) {
              if (item.DateParsed.Date == today)
                lastHistoryGet = today;
              grid.Children.Add(Utils.GetLabel(item.Date, 15, Color.LightSkyBlue), 0, grid.RowDefinitions.Count);
              grid.Children.Add(Utils.GetLabel($"{item.Rate_B:n3}", 17, Color.White), 1, grid.RowDefinitions.Count);
              grid.Children.Add(Utils.GetLabel($"{Utils.GetDirection(item.Rate_B_Delta)} {item.Rate_B_Delta:n3}", 16,
                  Utils.GetDiffColor(item.Rate_B_Delta)), 2, grid.RowDefinitions.Count);
              grid.Children.Add(Utils.GetLabel($"{item.Rate_S:n3}", 17, Color.White), 3, grid.RowDefinitions.Count);
              grid.Children.Add(Utils.GetLabel($"{Utils.GetDirection(item.Rate_S_Delta)} {item.Rate_S_Delta:n3}", 16,
                  Utils.GetDiffColor(item.Rate_S_Delta)), 4, grid.RowDefinitions.Count);
              grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
            }
          }
        }
        finally {
          refreshView.IsRefreshing = false;
        }
      }

      refreshView.Command = new Command(ExecuteRefresh);

      refreshView.Content = new ScrollView {
        BackgroundColor = Color.Black,
        Content = grid
      };
      Privat24Page.Content = refreshView;

      ExecuteRefresh();
    }
  }
}