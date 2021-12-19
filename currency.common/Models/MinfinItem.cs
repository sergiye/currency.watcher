using System;

namespace currency {

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
}