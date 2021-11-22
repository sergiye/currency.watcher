using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sergiy.Informer {

  public class Privat24HistoryResponse {
    public Privat24HistoryData Data { get; set; }
  }

  public class Privat24HistoryData {
    public Privat24HistoryItem[] History { get; set; }
  }

  public class Privat24HistoryItem {
    public string Date { get; set; }
    public DateTime DateParsed { get; set; }
    public string CurrencyCode { get; set; }
    public decimal NbuRate { get; set; }
    public decimal Rate_S { get; set; }
    public decimal Rate_S_Delta { get; set; }
    public decimal Rate_B { get; set; }
    public decimal Rate_B_Delta { get; set; }
  }

  public partial class MinfinItem {
    public string Currency { get; set; }
    //public long City_Id { get; set; }
    public string Rate_Type { get; set; }
    public DateTime Date { get; set; }
    public double Rate_Sell { get; set; }
    public double Rate_Buy { get; set; }
    //public double Trend_Sell { get; set; }
    //public long Trend_Sell_Count { get; set; }
    //public double Trend_Buy { get; set; }
    //public long Trend_Buy_Count { get; set; }
    //public long Trade_Buy_Count { get; set; }
    //public long Trade_Sell_Count { get; set; }
    //public double Trend_Trade_Sell { get; set; }
    //public double Trend_Trade_Buy { get; set; }
    //public long Trend_Trade_Buy_Count { get; set; }
    //public long Trend_Trade_Sell_Count { get; set; }
    //public double Trade_Sell { get; set; }
    //public double Trade_Buy { get; set; }
    public long Count_Sell { get; set; }
    public long Count_Buy { get; set; }
    //public object Organization { get; set; }
  }

  public class NbuRateItem {
    
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    
    public decimal Rate { get; set; }
  }

  public class DateFormatConverter : IsoDateTimeConverter {
    public DateFormatConverter(string format) {
      DateTimeFormat = format;
    }
  }
}
