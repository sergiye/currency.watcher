using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sergiy.Informer {

  internal partial class MainForm : Form {

    private readonly Timer _timer;
    private DateTime _lastCurrencyChanged;
    private DateTime _lastMinfinStats;
    private DateTime _lastPrivat24HistoryGet;
    
    private readonly List<string> _nbuRatesFile = new List<string>();
    private List<NbuRateItem[]> _nbuRates;

    #region form decoration

    private const int SysMenuAboutId = 0x1;

    protected override void OnHandleCreated(EventArgs e) {

      base.OnHandleCreated(e);
      var hSysMenu = Common.GetSystemMenu(Handle, false);
      Common.AppendMenu(hSysMenu, Common.MfSeparator, 0, string.Empty);
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuAboutId, "&About…");
    }

    protected override void WndProc(ref Message m) {

      base.WndProc(ref m);
      if ((m.Msg == Common.WmSyscommand)) {
        switch ((int)m.WParam) {
          case SysMenuAboutId:
            MessageBox.Show(Common.GetDeveloperText(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            break;
        }
      }
    }

    private void MainForm_ResizeEnd(object sender, EventArgs e) {
      if (!AppSettings.Instance.StickToEdges) return;
      var scn = Screen.FromPoint(Location);
      if (Common.DoSnap(Left, scn.WorkingArea.Left)) Left = scn.WorkingArea.Left;
      if (Common.DoSnap(Top, scn.WorkingArea.Top)) Top = scn.WorkingArea.Top;
      if (Common.DoSnap(scn.WorkingArea.Right, Right)) Left = scn.WorkingArea.Right - Width;
      if (Common.DoSnap(scn.WorkingArea.Bottom, Bottom)) Top = scn.WorkingArea.Bottom - Height;
    }

    private void btnTop_Click(object sender, EventArgs e) {

      if (btnTop.BackColor == Color.GreenYellow) {
        TopMost = false;
        btnTop.BackColor = BackColor;
      }
      else {
        TopMost = true;
        btnTop.BackColor = Color.GreenYellow;
      }
    }

    #endregion form decoration

    public MainForm() {

      InitializeComponent();

      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

      Icon = Icon.ExtractAssociatedIcon(AppSettings.GetAppPath());

      //configure chart
      var chartArea = new ChartArea();
      chartArea.AxisX.MajorGrid.Enabled = false;
      chartArea.AxisY.MajorGrid.Enabled = false;
      chartArea.AxisX.LabelStyle.Enabled = false;
      chartArea.AxisY.LabelStyle.Enabled = false;
      chartArea.AxisY.IsStartedFromZero = false;
      chartArea.AxisX.IsStartedFromZero = false;
      //chartArea.AxisX.LabelStyle.Format = "dd-MM"; // "yyyy-MM-dd";
      //chart.ChartAreas[0].AxisY.MaximumAutoSize = 15;
      chartArea.Name = "ChartArea1";
      this.chart.ChartAreas.Add(chartArea);

      var seriesColors = new[]{ Color.Green, Color.Blue, Color.Orange };
      for(var i = 0; i < seriesColors.Length; i++) {
        chart.Series.Add(new Series {
          ChartArea = chart.ChartAreas[0].Name,
          ChartType = SeriesChartType.Spline,
          Color = seriesColors[i],
          MarkerStyle = MarkerStyle.Circle,
          Name = $"Series{i}",
          //XValueType = ChartValueType.Date,
          //YValueType = ChartValueType.Double
        });
      }

      _timer = new Timer();
      _timer.Tick += (sender, args) => UpdateData();
      _timer.Interval = 10 * 60 * 1000;
      _timer.Enabled = true;

      btnRefresh.Click += btnRefresh_Click;

      cmbCurrency.SelectedIndexChanged += (sender, args) => {
        _lastCurrencyChanged = DateTime.Now;
        UpdateData();
        numTaxesSource_ValueChanged(sender, args);
      };

      panStatus.MouseWheel += (sender, e) => {
        if (e.Button != MouseButtons.None) return;
        var delta = e.Delta > 0 ? 0.05 : -0.05;
        Opacity += delta;
        if (Opacity < 0.2)
          Opacity = 0.2;
      };

      cmbChartMode.SelectedIndexChanged += (sender, args) => {
        UpdateMinfinChartData();
      };

      this.Load += (sender, args) => {
        
        LoadNbuRates();

        Left = AppSettings.Instance.Left;
        Top = AppSettings.Instance.Top;
        Height = AppSettings.Instance.Height;
        Width = AppSettings.Instance.Width;
        Opacity = AppSettings.Instance.Opacity;
        cmbCurrency.SelectedIndex = AppSettings.Instance.CurrencyIndex;
        _timer.Interval = AppSettings.Instance.RefreshInterval * 60 * 1000;
        cmbChartMode.SelectedIndex = AppSettings.Instance.ChartViewMode;
        cbxChartGridMode.Checked = AppSettings.Instance.ChartLines;
        cbxStickEdges.Checked = AppSettings.Instance.StickToEdges;
        cbxWeather.Checked = AppSettings.Instance.Weather;
      };

      this.Closing += (sender, args) => {
        AppSettings.Instance.Left = Left;
        AppSettings.Instance.Top = Top;
        AppSettings.Instance.Height = Height;
        AppSettings.Instance.Width = Width;
        AppSettings.Instance.Opacity = Opacity;
        AppSettings.Instance.CurrencyIndex = cmbCurrency.SelectedIndex;
        AppSettings.Instance.Weather = cbxWeather.Checked;

        AppSettings.Instance.ChartViewMode = cmbChartMode.SelectedIndex;
        AppSettings.Instance.ChartLines = cbxChartGridMode.Checked;

        AppSettings.Instance.Save();
      };

      chart.Annotations.Add(new CalloutAnnotation{
        CalloutStyle = CalloutStyle.RoundedRectangle,
        Font = new Font(this.Font.FontFamily, this.Font.Size + 2, FontStyle.Bold),
        TextStyle = TextStyle.Shadow,
      });

      cbxChartGridMode.CheckedChanged += (s, e) => {
        var checkState = cbxChartGridMode.Checked;
        //chart.ChartAreas[0].AxisX.MajorGrid.Enabled = checkState;
        //chart.ChartAreas[0].AxisX.LabelStyle.Enabled = checkState;
        chart.ChartAreas[0].AxisY.MajorGrid.Enabled = checkState;
        chart.ChartAreas[0].AxisY.LabelStyle.Enabled = checkState;
        //chart.ChartAreas[0].AxisX.MinorGrid.Enabled = checkState;
        //chart.ChartAreas[0].AxisY.MinorGrid.Enabled = checkState;
      };

      cbxShowNbu.CheckedChanged += (s, e) => {
        chart.Series[2].Enabled = cbxShowNbu.Checked;
      };

      cbxStickEdges.CheckedChanged += (s, e) => {
        AppSettings.Instance.StickToEdges = cbxStickEdges.Checked;
      };

      cbxWeather.CheckedChanged += (s, e) => {
        AppSettings.Instance.Weather = cbxWeather.Checked;
        browser.Visible = cbxWeather.Checked;
      };

      lstFinanceHistory.DoubleClick += (s, e) => {
        Process.Start("https://tables.finance.ua/ua/currency/official/-/1");
      };
      lstHistory.DoubleClick += (s, e) => {
        Process.Start("https://minfin.com.ua/currency/auction/usd/sell/kharkov/?compact=true");
      };

      _nbuRatesFile.Add($"{Path.GetDirectoryName(AppSettings.GetAppPath())}\\nbu.rates.{GetCurrencyName(0)}.json");
      _nbuRatesFile.Add($"{Path.GetDirectoryName(AppSettings.GetAppPath())}\\nbu.rates.{GetCurrencyName(1)}.json");
    }

    private void LoadNbuRates() {
      _nbuRates = new List<NbuRateItem[]>();
      for (var i = 0; i < cmbCurrency.Items.Count; i++) {
        _nbuRates.Add(new NbuRateItem[0]);
        if (File.Exists(_nbuRatesFile[i]))
          _nbuRates[i] = File.ReadAllText(_nbuRatesFile[i]).FromJson<NbuRateItem[]>();
      }
    }

    private void SaveNbuRates(int currencyIndex) {
      var data = _nbuRates[currencyIndex].ToJson();
      File.WriteAllText(_nbuRatesFile[currencyIndex], data);
      if (currencyIndex == cmbCurrency.SelectedIndex)
        UpdateNbuRateText(_nbuRates[currencyIndex]);
    }

    private void GetJsonData(string uri, Action<string> onResponse, int timeout = 10, string method = "GET") {

      Task.Factory.StartNew(() => { 
        var request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = method;
        request.Timeout = timeout * 1000;
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        //request.Accept = "text/xml,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        request.ContentType = "application/json; charset=utf-8";
        request.BeginGetResponse(ar => {
          try {
            using (var webResp = (HttpWebResponse)request.EndGetResponse(ar)) {
              using (var stream = webResp.GetResponseStream()) {
                if (stream == null) return;
                var answer = new StreamReader(stream, Encoding.UTF8);
                var result = answer.ReadToEnd();
                if (string.IsNullOrWhiteSpace(result)) return;
                if (this.InvokeRequired)
                  this.Invoke(new Action<string>(onResponse), result);
                else
                  onResponse(result);
              }
            }
          }
          catch (Exception ex) {
            Console.WriteLine(ex);
            //onResponse(null);
          }
        }, null);
      });
    }

    private Color GetDiffColor(IComparable lastValue, IComparable prevValue) {
      if (lastValue == null || prevValue == null)
        return Color.LightGray;
      return lastValue.CompareTo(prevValue) == 1 ? Color.LightGreen
          : lastValue.CompareTo(prevValue) == -1 ? Color.Plum
          : Color.LightGray;
    }

    private Color GetDiffColor(decimal delta) {
      return delta > 0 ? Color.LightGreen
          : delta < 0 ? Color.Plum
          : Color.LightGray;
    }

    private void UpdateData() {
      UpdateBrowser();

      var currencyIndex = cmbCurrency.SelectedIndex;
      var currencyText = GetCurrencyName(currencyIndex);
      UpdateNbuRateText(_nbuRates[currencyIndex]);

      for (var cIndex = 0; cIndex < 2; cIndex++) {
        var index = cIndex;
        GetJsonData($"https://minfin.com.ua/data/currency/nbu/nbu.{GetCurrencyName(index)}.stock.json", data => {
          if (string.IsNullOrEmpty(data)) return;
          var newRates = data.FromJson<NbuRateItem[]>();
          if (newRates == null || newRates.Length == 0)
            return;

          if (_nbuRates[index] == null || _nbuRates[index].Length == 0) {
            _nbuRates[index] = newRates;
            SaveNbuRates(index);
          }
          else {
            if (newRates[newRates.Length - 1].Date.Date > _nbuRates[index][_nbuRates[index].Length - 1].Date.Date) {
              var newItems = new List<NbuRateItem>();
              newItems.AddRange(_nbuRates[index]);
              newItems.AddRange(newRates.Where(i => i.Date.Date > _nbuRates[index][_nbuRates[index].Length - 1].Date.Date).ToList());
              _nbuRates[index] = newItems.ToArray();
              SaveNbuRates(index);
            }
          }
        });
      }

      GetJsonData($"http://resources.finance.ua/chart/data?for=currency-order&currency={currencyText}", OnFinanceUaResponse);

      var nowDate = DateTime.Now;
      if (_lastCurrencyChanged > _lastPrivat24HistoryGet || _lastPrivat24HistoryGet.Date != nowDate.Date) {
        GetJsonData("https://otp24.privatbank.ua/v3/api/1/info/currency/history", OnPrivat24HistoryResponse, 30, "POST");
      }

      if (_lastCurrencyChanged > _lastMinfinStats || _lastMinfinStats.AddMinutes(30) < nowDate) {
        UpdateMinfinChartData();
      }
    }

    private void UpdateNbuRateText(NbuRateItem[] nbuRateItems) {
      if (nbuRateItems == null || nbuRateItems.Length == 0) return;
      var lastItem = nbuRateItems[nbuRateItems.Length - 1];
      Text = $"Sergiy's Informer (NBU rate = {lastItem.Rate:n2} at {lastItem.Date:M})";
    }

    private void UpdateBrowser() {
      //const string theHtml = "<div style=\"margin:-15;\"><a href=\"https://pogoda.yandex.ru/kharkiv\" target=\"_blank\"><img src=\"https://info.weather.yandex.net/kharkiv/3.ru.png\" border=\"0\" alt=\"Яндекс.Погода\"/></a></div>";
      //Common.UpdateBrowser(browser, theHtml);
      Common.UpdateBrowser(browser, Properties.Resources.weather);
    }

    private void OnPrivat24HistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var currencyCode = GetCurrencyName(cmbCurrency.SelectedIndex);
      var historyData = response.FromJson<Privat24HistoryResponse>();
      if (historyData?.Data?.History == null) return;
      if (historyData.Data.History.Length <= 0) return;
      
      lstHistory.Items.Clear();
      var filteredItems = historyData.Data.History.Where(i => i.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
        .OrderByDescending(x => {
          DateTime.TryParseExact(x.Date, "dd-MM-yyyy", null, DateTimeStyles.AllowWhiteSpaces, out var dt);
          x.DateParsed = dt;
          return dt;
        }).ToArray();
      if (filteredItems.Length == 0) return;
        
      for (var i = 0; i < filteredItems.Length && i < 9; i++) {
        var item = filteredItems[i];

        var lvItem = new ListViewItem(item.Date, 0);
        lvItem.SubItems.Add(item.Rate_B.ToString("F"));
        //lvItem.SubItems.Add(item.Rate_B_Delta.ToString("F"));
        lvItem.SubItems.Add(item.Rate_S.ToString("F"));
        //lvItem.SubItems.Add(item.Rate_S_Delta.ToString("F"));
        lvItem.Tag = item;
        lvItem.BackColor = GetDiffColor(item.Rate_B_Delta);
        lstHistory.Items.Add(lvItem);
      }

      if (filteredItems[0].DateParsed.Date == DateTime.Today)
        _lastPrivat24HistoryGet = DateTime.Now;
      // if (_lastPrivat24HistoryGet < filteredItems[0].DateParsed) {
      //   _lastPrivat24HistoryGet = filteredItems[0].DateParsed;
      // }
    }

    private void UpdateMinfinChartData() {
      var currencyCode = GetCurrencyName(cmbCurrency.SelectedIndex).ToLower();
      var city = 22;
      var nowDate = DateTime.Today;
      DateTime startDate;
      switch (cmbChartMode.SelectedIndex) {
        case 1:
          startDate = nowDate.AddMonths(-3);
          break;
        case 2:
          startDate = nowDate.AddMonths(-6);
          break;
        default:
          startDate = nowDate.AddMonths(-1);
          break;
      }
      GetJsonData($"https://va-backend.treeum.net/api/currency_rates/get_rates/{currencyCode}/{city}/{startDate:yyyy-MM-dd}/{nowDate:yyyy-MM-dd}", OnMinfinHistoryResponse);
    }

    private void OnMinfinHistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var historyData = response.FromJson<Dictionary<string, MinfinItem>>();
      if (historyData == null || historyData.Count == 0) return;

      var currencyIndex = cmbCurrency.SelectedIndex;
      var showNbu = _nbuRates[currencyIndex] != null && _nbuRates[currencyIndex].Length > 0;
      if (!showNbu)
        cbxShowNbu.Checked = false;
      cbxShowNbu.Visible = showNbu;

      foreach (var s in chart.Series)
        s.Points.Clear();

      foreach (var item in historyData.Values) {
        chart.Series[0].Points.AddXY(item.Date, item.Rate_Buy);
        chart.Series[1].Points.AddXY(item.Date, item.Rate_Sell);
        if (showNbu) {
          var nbuValue = _nbuRates[currencyIndex].FirstOrDefault(i=>i.Date.Date == item.Date.Date);
          if (nbuValue != null) {
            chart.Series[2].Points.AddXY(item.Date, nbuValue.Rate);
          }
        }
        //todo: process "count_sell": 3, "count_buy": 7,
      }

      //chart.ChartAreas[0].RecalculateAxesScale();
      var chartMin = chart.Series[0].Points.Min(p => p.YValues[0]);
      var chartMax = chart.Series[0].Points.Max(p => p.YValues[0]);
      for (int i = 1; i < chart.Series.Count; i++) {
        if (chart.Series[i].Points.Count == 0) continue;
        var min = chart.Series[i].Points.Min(p => p.YValues[0]);
        if (chartMin > min)
          chartMin = min;
        var max = chart.Series[i].Points.Max(p => p.YValues[0]);
        if (chartMax < max)
          chartMax = max;
      }
      var space = 0;//(chartMax-chartMin) * 0.05;
      chart.ChartAreas[0].AxisY.Minimum = chartMin - space;
      chart.ChartAreas[0].AxisY.Maximum = chartMax + space;

      //if (historyData.Last().Value.Date == DateTime.Now.Date)
      _lastMinfinStats = DateTime.Now;
    }

    private void OnFinanceUaResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var chartData = response.FromJson<string[][]>();
      if (chartData == null || chartData.Length <= 0) return;

      lstFinanceHistory.Items.Clear();
      if (chartData.Length > 1) {
        var currentItem = chartData[chartData.Length - 1];
        for (var i = chartData.Length - 2; i >= 0; i--) {
          string[] prevItem = chartData[i];
          if (currentItem[1] == prevItem[1] && currentItem[2] == prevItem[2])
            continue;
          var timePart = currentItem[0].Split(' ')[1];
          var lvItem = new ListViewItem(timePart, 0) { UseItemStyleForSubItems = false };
          lvItem.SubItems.Add(currentItem[1], lstFinanceHistory.ForeColor, GetDiffColor(currentItem[1], prevItem[1]), lstFinanceHistory.Font);
          lvItem.SubItems.Add(currentItem[2], lstFinanceHistory.ForeColor, GetDiffColor(currentItem[2], prevItem[2]), lstFinanceHistory.Font);
          lstFinanceHistory.Items.Add(lvItem);

          currentItem = prevItem;
        }
      }
    }

    private string GetCurrencyName(int currencyIndex, bool lower =true) {
      var result = currencyIndex == 0 ? "USD" : "EUR";
      if (lower)
        result = result.ToLower();
      return result;
    }

    private void btnRefresh_Click(object sender, EventArgs e) {
      UpdateData();
    }

    private void chart_MouseMove(object sender, MouseEventArgs e) {

      // var chart = sender as Chart;
      if (chart == null) return;
      var ta = chart.Annotations[0] as CalloutAnnotation;
      if (ta == null) return;

      var result = chart.HitTest(e.X, e.Y);
      switch (result.ChartElementType) {
        case ChartElementType.DataPoint:
          ta.AnchorDataPoint = result.Series.Points[result.PointIndex];
          var dateVal = DateTime.FromOADate(result.Series.Points[result.PointIndex].XValue);
          ta.Text = $"{dateVal:yyyy-MMM-dd}\n{result.Series.Points[result.PointIndex].YValues[0]}";
          chart.Invalidate();
          break;
        default:
          // var val = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
          // var date = DateTime.FromOADate(val);
          // var buy = interpolatedY(chart.Series[0], val);
          // var sell = interpolatedY(chart.Series[1], val);
          // ta.Text = $"{date:yyyy-MMM-dd}\nBuy: {buy}\nSell: {sell}";
          var mousePoint = new Point(e.X, e.Y);
          chart.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
          var xVal = chart.ChartAreas[0].CursorX.Position;
          var xDate = DateTime.FromOADate(xVal);
          var xBuy = chart.Series[0].Points.FirstOrDefault(x => x.XValue == xVal);
          var xSell = chart.Series[1].Points.FirstOrDefault(x => x.XValue == xVal);
          ta.AnchorDataPoint = xSell;
          ta.Text = $"{xDate:yyyy-MMM-dd}\nSell: {xSell?.YValues[0]}\nBuy: {xBuy?.YValues[0]}";
          if (cbxShowNbu.Checked) {
            var nbu = chart.Series[2].Points.FirstOrDefault(x => x.XValue == xVal);
            if (nbu != null)
              ta.Text += $"\nNbu: {nbu.YValues[0]}";
          }
          break;
      }
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
      panGridOptions.Visible = tabControl.SelectedIndex == 1;
    }

    private void numTaxesSource_ValueChanged(object sender, EventArgs e) {
      var usd = numTaxesSource.Value;
      var date = dtTaxesSource.Value;
      var currencyIndex = cmbCurrency.SelectedIndex;
      if (_nbuRates != null && _nbuRates[currencyIndex] != null && _nbuRates[currencyIndex].Length != 0) {
        var nbuRate = _nbuRates[currencyIndex].FirstOrDefault(i => i.Date.Date == date.Date);
        if (nbuRate != null) {
          var uah = usd * nbuRate.Rate;
          txtUahResult.Text = uah.ToString("n2");
          txtTaxesResult.Text = (uah * (decimal)0.05).ToString("n2");
          return;
        }
      }
      txtUahResult.Clear();
      txtTaxesResult.Clear();
    }
  }
}
