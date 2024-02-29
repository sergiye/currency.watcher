using System;

namespace currency.watcher {

  public class NbuRateItem {
    
    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    
    public decimal Rate { get; set; }
  }
}