using currency.mobile.ViewModels;
using Xamarin.Forms;

namespace currency.mobile.Views {
  
  public partial class NbuPage : ContentPage {
    
    private readonly NbuViewModel viewModel;

    public NbuPage() {
      InitializeComponent();

      BindingContext = viewModel = new NbuViewModel();
    }

    protected override void OnAppearing() {
      base.OnAppearing();
      viewModel.OnAppearing();
    }
  }
}