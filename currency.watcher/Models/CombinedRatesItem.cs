using System;

namespace currency.watcher {
  public class CombinedRatesItem {
    
    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public decimal NbuRateUsd { get; set; }
    public decimal NbuRateEur { get; set; }

    public decimal PbRateUsdB { get; set; }
    public decimal PbRateUsdS { get; set; }
    public decimal PbRateEurB { get; set; }
    public decimal PbRateEurS { get; set; }
  }
}