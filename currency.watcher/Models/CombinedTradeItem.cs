using System.ComponentModel;

namespace currency.watcher {

  public class CombinedTradeItem {
    
    public string Date { get; set; }
    
    [DefaultValue(0)]
    public decimal UsdB { get; set; }
    
    [DefaultValue(0)]
    public decimal UsdS { get; set; }
    
    [DefaultValue(0)]
    public decimal EurB { get; set; }

    [DefaultValue(0)]
    public decimal EurS { get; set; }

    public bool RatesEquals(CombinedTradeItem other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return UsdB == other.UsdB && UsdS == other.UsdS && EurB == other.EurB && EurS == other.EurS;
    }
  }
}