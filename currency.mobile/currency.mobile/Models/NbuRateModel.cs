using System;
using currency.mobile.Services;
using Xamarin.Forms;

namespace currency.mobile.Models {

  public class NbuRateItemModel {

    public decimal Rate { get; private set; }
    public decimal Delta { get; private set; }
    public string RateFormatted { get; private set; }
    public string DeltaFormatted { get; private set; }
    public Color Color { get; private set; }

    public NbuRateItemModel(decimal rate) {
      Rate = rate;
      RateFormatted =  Rate.ToString("n3");
      Color = Color.White;
    }

    public void UpdateDelta(decimal rate) {
      Delta = Rate - rate;
      Color = Utils.GetDiffColor(Delta);
      DeltaFormatted = $"{Utils.GetDirection(Delta)} {Delta:n3}";
    }
  }

  public class NbuRateModel {
    
    public DateTime Date { get; private set; }
    public string DateFormatted { get; private set; }
    public NbuRateItemModel[] Items { get; private set; }

    public string Rate1 => Items[0].RateFormatted;
    public string Delta1 => Items[0].DeltaFormatted;
    public Color Color1 => Items[0].Color;
    public string Rate2 => Items[1].RateFormatted;
    public string Delta2 => Items[1].DeltaFormatted;
    public Color Color2 => Items[1].Color;

    public NbuRateModel(DateTime date, decimal rate, decimal rate2) {
      Date = date;
      DateFormatted = Date.ToString("dd-MM-yyyy");
      Items = new NbuRateItemModel[2];
      Items[0] = new NbuRateItemModel(rate);
      Items[1] = new NbuRateItemModel(rate2);
    }
  }
}