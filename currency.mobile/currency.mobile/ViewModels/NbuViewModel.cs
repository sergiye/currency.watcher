using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace currency.mobile.ViewModels {

  public class NbuViewModel : BaseViewModel {

    const int index = 0; //todo: currencyIndex - show both USD & EUR on diff columns

    public ObservableCollection<NbuRateModel> Items { get; }
    public Command LoadItemsCommand { get; }
    public Command<NbuRateModel> ItemTapped { get; }
    // public Command AddItemCommand { get; }

    private readonly ICurrencyProvider<NbuRateItem> nbuProvider;

    public NbuViewModel() {
      Items = new ObservableCollection<NbuRateModel>();
      LoadItemsCommand = new Command(ExecuteLoadItemsCommand);

      ItemTapped = new Command<NbuRateModel>(item => {
        if (item == null) return;
        // await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Date}");
      });

      // AddItemCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(NewItemPage)));

      nbuProvider = DependencyService.Get<ICurrencyProvider<NbuRateItem>>();
      ExecuteLoadItemsCommand(); //todo: auto-load data
    }
    
    public void OnAppearing() {
      IsBusy = true;
    }

    private async void ExecuteLoadItemsCommand() {
      IsBusy = true;

      try {

        var today = DateTime.Today;
        if (nbuProvider.IsEmpty(index) || nbuProvider.GetLastItem(index)?.Date < today.Date) {
          await nbuProvider.Refresh(index);
        }

        if (nbuProvider.IsEmpty(index))
          return;

        if (Items.Count > 0 && nbuProvider.GetLastItem(index).Date == Items[0].Date)
          return;

        Items.Clear();
        Items.Add(new NbuRateModel(nbuProvider.GetLastItem(index)));
        for (var i = 0; i < 30; i++) {
          var item = nbuProvider.GetByDate(index, Items[i].Date.AddDays(-1));
          if (item == null) break;
          var newItem = new NbuRateModel(item);
          if (newItem.Rate > Items[i].Rate) {
            Items[i].Color = "OrangeRed";
            Items[i].Direction = "↓";
          }
          else if (newItem.Rate == Items[i].Rate) {
            Items[i].Color = "White";
            Items[i].Direction = "-";
          }
          else {
            Items[i].Color = "LimeGreen";
            Items[i].Direction = "↑";
          }
          Items.Add(newItem);
        }
      }
      catch (Exception ex) {
        Debug.WriteLine(ex);
      }
      finally {
        IsBusy = false;
      }
    }
  }
}