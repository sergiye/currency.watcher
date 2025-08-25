using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace currency.watcher {

  public class DataProvider {

    private readonly string nbuRatesFile;
    private List<CombinedRatesItem> rates;

    static DataProvider() {
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }

    public DataProvider(string appPath) {

      if (!string.IsNullOrEmpty(appPath)) {
        nbuRatesFile= $"{Path.GetDirectoryName(appPath)}\\nbu.rates.json";
      }

      LoadNbuRates();
    }

    public bool IsEmpty() {
      lock(rates)
        return rates.Count == 0;
    }

    public CombinedRatesItem GetByDate(DateTime date) {
      lock (rates)
        return IsEmpty() ? null : rates.FirstOrDefault(i => i.Date.Date == date.Date);
    }

    public CombinedRatesItem[] Take(int count) {
      lock (rates)
        return IsEmpty() ? null : rates
        .Skip(Math.Max(0, rates.Count - count)).ToArray();
    }

    public event Action OnDataChanged;

    private bool AddOrUpdate(CombinedRatesItem item) {

      lock (rates) {
        var oldIndex = rates.FindLastIndex(i => i.Date.Date <= item.Date);
        if (oldIndex == -1) {
          rates.Insert(0, item);
          return true;
        }
        else {
          var old = rates[oldIndex];
          if (old.Date == item.Date) {
            if (old.UpdateFrom(item))
              return true;
          }
          else {
            if (old.DataEqualsOrNoNewValues(item))
              return false;
            rates.Insert(oldIndex + 1, item);
          }
        }
      }
      return false;
    }

    private void LoadNbuRates() {
      if (!string.IsNullOrEmpty(nbuRatesFile) && File.Exists(nbuRatesFile))
        rates = File.ReadAllText(nbuRatesFile).FromJson<List<CombinedRatesItem>>();
      
      if (rates == null)
        rates = new List<CombinedRatesItem>();
    }

    private bool SaveNbuRates() {
      if (string.IsNullOrEmpty(nbuRatesFile)) return false;
      if (OptimizeRates() == false)
        return false;
      lock (rates) {
        var data = rates.ToJson();
        File.WriteAllText(nbuRatesFile, data);
      }
      return true;
    }

    private bool OptimizeRates() {

      var hasModifiedItems = false;
      lock (rates) {
        var i = 1;
        while (i < rates.Count) {
          var prev = rates[i - 1];
          var item = rates[i];
          if (prev.DataEquals(item)) {
            rates.RemoveAt(i);
          }
          else {
            i++;
            hasModifiedItems = item.Modified;
          }
        }
      }
      return hasModifiedItems;
    }

    public void Refresh() {

      Task.Factory.StartNew(async () => {
        var dataChanged = false;

        var dataUsd = await GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.usd.stock.json")
          .ConfigureAwait(false);
        if (!string.IsNullOrEmpty(dataUsd)) {
          var newRatesUsd = dataUsd.FromJson<NbuRateItem[]>();
          if (newRatesUsd != null && newRatesUsd.Length > 0) {
            for (var i = 0; i < newRatesUsd.Length; i++) {
              var date = DateTime.SpecifyKind(newRatesUsd[i].Date, DateTimeKind.Utc);
              if (AddOrUpdate(new CombinedRatesItem {
                Date = date,
                NbuRateUsd = newRatesUsd[i].Rate,
              })) {
                dataChanged = true;
              }
            }
          }
        }

        var dataEur = await GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.eur.stock.json")
  .ConfigureAwait(false);
        if (!string.IsNullOrEmpty(dataEur)) {
          var newRatesEur = dataEur.FromJson<NbuRateItem[]>();
          if (newRatesEur != null && newRatesEur.Length > 0) {
            for (var i = 0; i < newRatesEur.Length; i++) {
              var date = DateTime.SpecifyKind(newRatesEur[i].Date, DateTimeKind.Utc);
              if (AddOrUpdate(new CombinedRatesItem {
                Date = date,
                NbuRateEur = newRatesEur[i].Rate,
              })) {
                dataChanged = true;
              }
            }
          }
        }

        var dataPrivat24 = await GetJsonData("https://otp24.privatbank.ua/v3/api/1/info/currency/history", 30, "POST");
        if (!string.IsNullOrEmpty(dataPrivat24)) {
          var historyData = dataPrivat24.FromJson<Privat24HistoryResponse>();
          if (historyData?.Data?.History != null && historyData.Data.History.Length > 0) {
            var currencyCodes = new[] { "USD", "EUR" };
            var filteredItems = historyData.Data.History.Where(i => currencyCodes.Contains(i.CurrencyCode))
              .OrderByDescending(x => {
                DateTime.TryParseExact(x.Date, "dd-MM-yyyy", null, DateTimeStyles.AllowWhiteSpaces, out var dt);
                x.DateParsed = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                return dt;
              }).ToArray();
            if (filteredItems.Length != 0) {
              for (var i = 0; i < filteredItems.Length; i += 2) {
                var itemEur = filteredItems[i]; //todo: get by Date & CurrencyCode
                var itemUsd = filteredItems[i + 1]; //todo: get by Date & CurrencyCode
                if (AddOrUpdate(new CombinedRatesItem {
                  Date = itemUsd.DateParsed.Date,
                  PbRateUsdB = itemUsd.Rate_B,
                  PbRateUsdS = itemUsd.Rate_S,
                  PbRateEurB = itemEur.Rate_B,
                  PbRateEurS = itemEur.Rate_S,
                })) {
                  dataChanged = true;
                }
              }
            }
          }
        }

        if (dataChanged) {

          //fix missed pb dates
          lock (rates) {
            CombinedRatesItem current = null;
            for (int i = 0; i < rates.Count; i++) {
              if (rates[i].PbRateUsdB == 0) {
                if (current != null) {
                  rates[i].PbRateUsdB = current.PbRateUsdB;
                  rates[i].PbRateUsdS = current.PbRateUsdS;
                  rates[i].PbRateEurB = current.PbRateEurB;
                  rates[i].PbRateEurS = current.PbRateEurS;
                }
              }
              else {
                current = rates[i];
              }
            }
          }

          if (SaveNbuRates())
            OnDataChanged?.Invoke();
        }

      });

    }

    public static async Task<string> GetJsonData(string uri, int timeout = 10, string method = "GET") {
      var request = (HttpWebRequest)WebRequest.Create(uri);
      request.Method = method;
      request.Timeout = timeout * 1000;
      request.UserAgent =
        "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.500.27 Safari/537.36";
      //request.Accept = "text/xml,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
      request.ContentType = "application/json; charset=utf-8";
      try {
        using (var webResp = await request.GetResponseAsync().ConfigureAwait(false)) {
          using (var stream = webResp.GetResponseStream()) {
            if (stream == null) return null;
            var answer = new StreamReader(stream, Encoding.UTF8);
            var result = await answer.ReadToEndAsync().ConfigureAwait(false);
            return result;
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex);
      }

      return null;
    }

  }
}
