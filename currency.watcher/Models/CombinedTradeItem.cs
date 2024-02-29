namespace currency.watcher {

  public class CombinedTradeItem {
    
    public string Date { get; set; }
    public decimal UsdB { get; set; }
    public decimal UsdS { get; set; }
    public decimal EurB { get; set; }
    public decimal EurS { get; set; }

    public bool RatesEquals(CombinedTradeItem other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return UsdB == other.UsdB && UsdS == other.UsdS && EurB == other.EurB && EurS == other.EurS;
    }
  }
}