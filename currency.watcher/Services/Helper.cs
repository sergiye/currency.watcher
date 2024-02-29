using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace currency {

  public static class Helper {

    #region Common methods

    public static string GetCurrencyName(int currencyIndex, bool lower = true) {
      var result = currencyIndex == 0 ? "USD" : "EUR";
      if (lower)
        result = result.ToLower();
      return result;
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

    #endregion Common methods

  }
}
