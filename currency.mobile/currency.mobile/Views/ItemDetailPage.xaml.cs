using currency.mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace currency.mobile.Views {
  public partial class ItemDetailPage : ContentPage {
    public ItemDetailPage() {
      InitializeComponent();
      BindingContext = new ItemDetailViewModel();
    }
  }
}