using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using currency.mobile.Models;
using Xamarin.Forms;

namespace currency.mobile.ViewModels {

  public class NbuViewModel : BaseViewModel {

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
        if (nbuProvider.IsEmpty(0) || nbuProvider.GetLastItem(0)?.Date < today.Date) 
          await nbuProvider.Refresh(0);
        if (nbuProvider.IsEmpty(1) || nbuProvider.GetLastItem(1)?.Date < today.Date) 
          await nbuProvider.Refresh(1);

        if (nbuProvider.IsEmpty(0) && nbuProvider.IsEmpty(1))
          return;

        if (Items.Count > 0 && 
            nbuProvider.GetLastItem(0).Date == Items[0].Date && 
            nbuProvider.GetLastItem(1).Date == Items[0].Date)
          return;

        if (Items.Count > 0) {
          var lastItem = Items[0];
          while (true) {
            var nextItem = nbuProvider.GetByDate(0, lastItem.Date.AddDays(1));
            var nextItem2 = nbuProvider.GetByDate(1, lastItem.Date.AddDays(1));
            if (nextItem == null && nextItem2 == null)
              break;
            var newItem = new NbuRateModel(nextItem?.Date ?? nextItem2.Date, nextItem?.Rate ?? 0, nextItem2?.Rate ?? 0);
            newItem.Items[0].UpdateDelta(lastItem.Items[0].Rate);
            newItem.Items[1].UpdateDelta(lastItem.Items[1].Rate);
            Items.Insert(0, newItem);
          }
        }
        else {
          var last = nbuProvider.GetLastItem(0);
          var last2 = nbuProvider.GetByDate(1, last.Date);
          Items.Add(new NbuRateModel(last.Date, last.Rate, last2?.Rate ?? 0));
          while (Items.Count < 365) {
            var prevIndex = Items.Count - 1;
            var item = nbuProvider.GetByDate(0, Items[prevIndex].Date.AddDays(-1));
            var item2 = nbuProvider.GetByDate(1, Items[prevIndex].Date.AddDays(-1));
            if (item == null && item2 == null) break;
            var newItem = new NbuRateModel(item.Date, item?.Rate ?? 0, item2?.Rate ?? 0);
            Items[prevIndex].Items[0].UpdateDelta(newItem.Items[0].Rate);
            Items[prevIndex].Items[1].UpdateDelta(newItem.Items[1].Rate);
            Items.Add(newItem);
          }
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