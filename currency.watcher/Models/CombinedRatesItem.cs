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

    public bool DataEquals(CombinedRatesItem item) {
      if (item == null) return false;
      return NbuRateUsd == item.NbuRateUsd && NbuRateEur == item.NbuRateEur &&
             PbRateUsdB == item.PbRateUsdB && PbRateUsdS == item.PbRateUsdS &&
             PbRateEurB == item.PbRateEurB && PbRateEurS == item.PbRateEurS;
    }

    public bool DataEqualsOrNoNewValues(CombinedRatesItem item) {
      if (item == null) return false;
      return (NbuRateUsd == item.NbuRateUsd || item.NbuRateUsd == 0) && 
             (NbuRateEur == item.NbuRateEur || item.NbuRateEur == 0) &&
             (PbRateUsdB == item.PbRateUsdB || item.PbRateUsdB == 0) &&
             (PbRateUsdS == item.PbRateUsdS || item.PbRateUsdS == 0) &&
             (PbRateEurB == item.PbRateEurB || item.PbRateEurB == 0) &&
             (PbRateEurS == item.PbRateEurS || item.PbRateEurS == 0);
    }

    public bool UpdateFrom(CombinedRatesItem item) {
      if (item == null) return false;

      var dataChanged = false;
      if (NbuRateUsd != item.NbuRateUsd && item.NbuRateUsd != 0) {
        NbuRateUsd = item.NbuRateUsd;
        dataChanged = true;
      }
      if (NbuRateEur != item.NbuRateEur && item.NbuRateEur != 0) {
        NbuRateEur = item.NbuRateEur;
        dataChanged = true;
      }
      if (PbRateUsdB != item.PbRateUsdB && item.PbRateUsdB != 0) {
        PbRateUsdB = item.PbRateUsdB;
        dataChanged = true;
      }
      if (PbRateUsdS != item.PbRateUsdS && item.PbRateUsdS != 0) {
        PbRateUsdS = item.PbRateUsdS;
        dataChanged = true;
      }
      if (PbRateEurB != item.PbRateEurB && item.PbRateEurB != 0) {
        PbRateEurB = item.PbRateEurB;
        dataChanged = true;
      }
      if (PbRateEurS != item.PbRateEurS && item.PbRateEurS != 0) {
        PbRateEurS = item.PbRateEurS;
        dataChanged = true;
      }
      return dataChanged;
    }
  }
}