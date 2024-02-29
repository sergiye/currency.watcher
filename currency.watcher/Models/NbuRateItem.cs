using System;

namespace currency {

  public class NbuRateItem {
    
    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    
    public decimal Rate { get; set; }
  }

  public class NbuRateCombinedItem {
    
    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public decimal RateUsd { get; set; }
    public decimal RateEur { get; set; }
  }
}