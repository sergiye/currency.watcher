using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace currency {

  public class NbuProvider : ICurrencyProvider<NbuRateItem> {

    private readonly List<string> nbuRatesFile = new List<string>();
    private List<NbuRateItem[]> nbuRates;

    static NbuProvider() {
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }

    public NbuProvider(string appPath) {

      if (!string.IsNullOrEmpty(appPath)) {
        nbuRatesFile.Add($"{Path.GetDirectoryName(appPath)}\\nbu.rates.{Helper.GetCurrencyName(0)}.json");
        nbuRatesFile.Add($"{Path.GetDirectoryName(appPath)}\\nbu.rates.{Helper.GetCurrencyName(1)}.json");
      }

      LoadNbuRates();
    }

    public bool IsEmpty(int index) {
      return index < 0 || index >= nbuRates.Count || nbuRates[index]?.Length == 0;
    }

    public NbuRateItem GetLastItem(int index) {
      return IsEmpty(index) ? null : nbuRates[index][nbuRates[index].Length - 1];
    }

    public NbuRateItem GetByDate(int index, DateTime date) {
      return IsEmpty(index) ? null : nbuRates[index].FirstOrDefault(i => i.Date.Date == date.Date);
    }

    public NbuRateItem[] GetFirstItems(int index, int count) {
      return IsEmpty(index) ? null : nbuRates[index].Take(count).ToArray();
    }

    public NbuRateItem[] GetLastItems(int index, int count) {
      return IsEmpty(index) ? null : nbuRates[index]
        .Skip(Math.Max(0, nbuRates[index].Length - count)).ToArray();
    }

    public event EventHandler<int> OnDataChanged;

    private void LoadNbuRates() {
      nbuRates = new List<NbuRateItem[]> {
        Array.Empty<NbuRateItem>(),
        Array.Empty<NbuRateItem>()
      };
      for (var i = 0; i < nbuRatesFile.Count; i++) {
        if (!File.Exists(nbuRatesFile[i])) continue;
        nbuRates[i] = File.ReadAllText(nbuRatesFile[i]).FromJson<NbuRateItem[]>();
      }
    }

    private void SaveNbuRates(int index) {
      if (0 > index || index >= nbuRatesFile.Count) return;
      var data = nbuRates[index].ToJson();
      File.WriteAllText(nbuRatesFile[index], data);
    }

    public async Task Refresh(int index) {

      var data = await Helper.GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.{Helper.GetCurrencyName(index)}.stock.json")
        .ConfigureAwait(false);
      if (string.IsNullOrEmpty(data)) return;
      var newRates = data.FromJson<NbuRateItem[]>();
      if (newRates == null || newRates.Length == 0)
        return;

      if (nbuRates[index] == null || nbuRates[index].Length == 0) {
        nbuRates[index] = newRates;
        SaveNbuRates(index);
        OnDataChanged?.Invoke(this, index);
      }
      else {
        if (newRates[newRates.Length - 1].Date.Date > nbuRates[index][nbuRates[index].Length - 1].Date.Date) {
          var newItems = new List<NbuRateItem>();
          newItems.AddRange(nbuRates[index]);
          newItems.AddRange(newRates.Where(i => i.Date.Date > nbuRates[index][nbuRates[index].Length - 1].Date.Date)
            .ToList());
          nbuRates[index] = newItems.ToArray();
          SaveNbuRates(index);
          OnDataChanged?.Invoke(this, index);
        }
      }
    }
  }
}
