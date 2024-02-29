using System;

namespace currency.watcher {

  public class MinfinHistoryResponse {
    public string Start { get; set; }
    public string End { get; set; }
    public string Group { get; set; }
    public MinfinHistorys Items { get; set; }
  }

  public class MinfinHistorys {
    public MinfinHistory[] Usd { get; set; }
    public MinfinHistory[] Eur { get; set; }
  }

  public class MinfinHistory {
    public DateTime Date { get; set; }
    public string Currency { get; set; }
    public float Sell { get; set; }
    public float Sell_dx { get; set; }
    public float Sell_n { get; set; }
    public float Sell_n_dx { get; set; }
    public float Buy { get; set; }
    public float Buy_dx { get; set; }
    public float Buy_n { get; set; }
    public float Buy_n_dx { get; set; }
  }
}