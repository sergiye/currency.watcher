using System;
using Newtonsoft.Json;

namespace currency {

  public class NbuRateItem {
    
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    
    public decimal Rate { get; set; }
  }

  public class NbuRateModel : NbuRateItem {
    
    public string DateFormatted => Date.ToString("M");
    public string RateFormatted => Rate.ToString("n3");
    public string Color { get; set; }
    public string Direction { get; set; }

    public NbuRateModel(NbuRateItem value) {
      this.Date = value.Date;
      Direction = "";
      Color = "White";
      this.Rate = value.Rate;
    }
  }
}