using System;

namespace currency {

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
}
