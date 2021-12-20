using System;
using Newtonsoft.Json;

namespace currency {

  public class NbuRateItem {
    
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    
    public decimal Rate { get; set; }
  }
}