using Newtonsoft.Json;
using System;

namespace currency.watcher {
  
  public class CombinedRatesItem {
    
    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    [JsonProperty(PropertyName = "Usd")] 
    public decimal NbuRateUsd { get; set; }
    [JsonProperty(PropertyName = "Eur")]
    public decimal NbuRateEur { get; set; }

    [JsonProperty(PropertyName = "UsdB")]
    public decimal PbRateUsdB { get; set; }
    [JsonProperty(PropertyName = "UsdS")]
    public decimal PbRateUsdS { get; set; }
    [JsonProperty(PropertyName = "EurB")]
    public decimal PbRateEurB { get; set; }
    [JsonProperty(PropertyName = "EurS")]
    public decimal PbRateEurS { get; set; }
  }
}