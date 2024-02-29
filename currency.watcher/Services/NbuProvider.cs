using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace currency {

  public class NbuProvider {

    private readonly string nbuRatesFile;
    private List<NbuRateCombinedItem> nbuRates;

    static NbuProvider() {
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }

    public NbuProvider(string appPath) {

      if (!string.IsNullOrEmpty(appPath)) {
        nbuRatesFile= $"{Path.GetDirectoryName(appPath)}\\nbu.rates.json";
      }

      LoadNbuRates();
    }

    public bool IsEmpty() {
      lock(nbuRates)
        return nbuRates.Count == 0;
    }

    public NbuRateCombinedItem GetByDate(DateTime date) {
      lock (nbuRates)
        return IsEmpty() ? null : nbuRates.FirstOrDefault(i => i.Date.Date == date.Date);
    }

    public NbuRateCombinedItem[] Take(int count) {
      lock (nbuRates)
        return IsEmpty() ? null : nbuRates
        .Skip(Math.Max(0, nbuRates.Count - count)).ToArray();
    }

    public event Action OnDataChanged;

    private void LoadNbuRates() {
      if (!string.IsNullOrEmpty(nbuRatesFile) && File.Exists(nbuRatesFile))
        nbuRates = File.ReadAllText(nbuRatesFile).FromJson<List<NbuRateCombinedItem>>();
      
      if (nbuRates == null)
        nbuRates = new List<NbuRateCombinedItem>();
    }

    private void SaveNbuRates() {
      if (!string.IsNullOrEmpty(nbuRatesFile)) {
        lock (nbuRates) {
          var data = nbuRates.ToJson();
          File.WriteAllText(nbuRatesFile, data);
        }
      }
    }

    public async Task Refresh() {

      var dataChanged = false;

      var dataUsd = await Helper.GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.usd.stock.json")
        .ConfigureAwait(false);
      if (!string.IsNullOrEmpty(dataUsd)) {
        var newRatesUsd = dataUsd.FromJson<NbuRateItem[]>();
        if (newRatesUsd != null && newRatesUsd.Length > 0) {
          for (var i = 0; i < newRatesUsd.Length; i++) {
            var old = GetByDate(newRatesUsd[i].Date);
            if (old == null) {
              lock (nbuRates) {
                nbuRates.Add(new NbuRateCombinedItem {
                  Date = newRatesUsd[i].Date,
                  RateUsd = newRatesUsd[i].Rate,
                });
              }
              dataChanged = true;
            }
            else if (old.RateUsd != newRatesUsd[i].Rate) {
              old.RateUsd = newRatesUsd[i].Rate;
              dataChanged = true;
            }
          }
        }
      }

      var dataEur = await Helper.GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.eur.stock.json")
        .ConfigureAwait(false);
      if (!string.IsNullOrEmpty(dataEur)) {
        var newRatesEur = dataEur.FromJson<NbuRateItem[]>();
        if (newRatesEur != null && newRatesEur.Length > 0) {
          for (var i = 0; i < newRatesEur.Length; i++) {
            var old = GetByDate(newRatesEur[i].Date);
            if (old == null) {
              lock (nbuRates) {
                nbuRates.Add(new NbuRateCombinedItem {
                  Date = newRatesEur[i].Date,
                  RateEur = newRatesEur[i].Rate,
                });
              }
              dataChanged = true;
            }
            else if (old.RateEur != newRatesEur[i].Rate) {
              old.RateEur = newRatesEur[i].Rate;
              dataChanged = true;
            }
          }
        }
      }

      if (dataChanged) {
        SaveNbuRates();
        OnDataChanged?.Invoke();
      }
    }
  }
}
