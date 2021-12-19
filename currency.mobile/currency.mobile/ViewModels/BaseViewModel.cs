using currency.mobile.Models;
using currency.mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace currency.mobile.ViewModels {
  public class BaseViewModel : INotifyPropertyChanged {

    public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

    private bool isBusy;
    public bool IsBusy {
      get => isBusy;
      set => SetProperty(ref isBusy, value);
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null) {
      
      if (EqualityComparer<T>.Default.Equals(backingStore, value))
        return false;

      backingStore = value;
      onChanged?.Invoke();
      OnPropertyChanged(propertyName);
      return true;
    }

    #region INotifyPropertyChanged
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
