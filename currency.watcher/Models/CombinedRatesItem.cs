using Newtonsoft.Json;
using sergiye.Common;
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

    [JsonIgnore]
    public bool Modified { get; private set; }

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

      if (NbuRateUsd.Differs(item.NbuRateUsd) && item.NbuRateUsd != 0) {
        NbuRateUsd = item.NbuRateUsd;
        Modified = true;
      }
      if (NbuRateEur.Differs(item.NbuRateEur) && item.NbuRateEur != 0) {
        NbuRateEur = item.NbuRateEur;
        Modified = true;
      }
      if (PbRateUsdB.Differs(item.PbRateUsdB) && item.PbRateUsdB != 0) {
        PbRateUsdB = item.PbRateUsdB;
        Modified = true;
      }
      if (PbRateUsdS.Differs(item.PbRateUsdS) && item.PbRateUsdS != 0) {
        PbRateUsdS = item.PbRateUsdS;
        Modified = true;
      }
      if (PbRateEurB.Differs(item.PbRateEurB) && item.PbRateEurB != 0) {
        PbRateEurB = item.PbRateEurB;
        Modified = true;
      }
      if (PbRateEurS.Differs(item.PbRateEurS) && item.PbRateEurS != 0) {
        PbRateEurS = item.PbRateEurS;
        Modified = true;
      }
      return Modified;
    }
  }
}