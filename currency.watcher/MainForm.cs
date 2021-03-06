using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace currency.watcher {

  internal partial class MainForm : Form {

    private readonly string appTitle;
    private readonly Timer timer;
    private DateTime lastCurrencyChanged;
    private DateTime lastMinfinStats;
    private DateTime lastPrivat24HistoryGet;
    
    private NbuProvider nbuProvider;

    #region form decoration

    private const int SysMenuAboutId = 0x1;
    private const int SysMenuTopMost = 0x2;

    protected override void OnHandleCreated(EventArgs e) {

      base.OnHandleCreated(e);

      var hSysMenu = Common.GetSystemMenu(Handle, false);

      //Common.DeleteMenu(hSysMenu, Common.SC_SIZE, Common.MfByCommand); // Disable resizing
      //Common.DeleteMenu(hSysMenu, Common.SC_MINIMIZE, Common.MfByCommand); // Disable minimizing
      Common.DeleteMenu(hSysMenu, Common.SC_MAXIMIZE, Common.MfByCommand); // Disable maximizing

      Common.AppendMenu(hSysMenu, Common.MfSeparator, 0, string.Empty);
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuTopMost, "&Always on top");
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuAboutId, "&About…");
    }

    protected override void WndProc(ref Message m) {

      base.WndProc(ref m);
      if (m.Msg == Common.WmSysCommand) {
        switch ((int)m.WParam) {
          case SysMenuAboutId:
            MessageBox.Show(Common.GetDeveloperText(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            break;
          case SysMenuTopMost:
            TopMost = !TopMost;
            var hSysMenu = Common.GetSystemMenu(Handle, false);
            Common.CheckMenuItem(hSysMenu, SysMenuTopMost, TopMost ? Common.MfChecked : Common.MfUnchecked);
            break;
        }
      } else if (m.Msg == Common.WM_SHOWME) {
        if (WindowState == FormWindowState.Minimized)
          WindowState = FormWindowState.Normal;
        Activate();
        //bool top = TopMost;
        //TopMost = true;
        //TopMost = top;
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

    #endregion form decoration

    public MainForm() {

      InitializeComponent();

      Icon = Icon.ExtractAssociatedIcon(AppSettings.GetAppPath()); 

      var asm = Assembly.GetExecutingAssembly();
      appTitle = $"{((AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute), false)).Title} {asm.GetName().Version.ToString(3)}";
      Text = appTitle;

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

      timer = new Timer();
      timer.Tick += (sender, args) => UpdateData();
      timer.Interval = 10 * 60 * 1000;
      timer.Enabled = true;

      btnRefresh.Click += btnRefresh_Click;

      cmbCurrency.SelectedIndexChanged += (sender, args) => {
        lastCurrencyChanged = DateTime.Now;
        UpdateData();
        numTaxesSource_ValueChanged(sender, args);
      };

      panStatus.MouseWheel += (sender, e) => {
        if (e.Button != MouseButtons.None) return;
        var delta = e.Delta > 0 ? 0.05 : -0.05;
        // Font = new Font(Font.FontFamily, Font.Size + (float)delta);
        // return;
        Opacity += delta;
        if (Opacity < 0.2)
          Opacity = 0.2;
      };

      cmbChartMode.SelectedIndexChanged += (sender, args) => {
        UpdateMinfinChartData();
      };

      this.Load += (sender, args) => {

        nbuProvider = new NbuProvider(null);//AppSettings.GetAppPath());
        nbuProvider.OnDataChanged += (o, index) => {
          if (index == cmbCurrency.SelectedIndex)
            UpdateNbuRateText(index);
        };

        Left = AppSettings.Instance.Left;
        Top = AppSettings.Instance.Top;
        Height = AppSettings.Instance.Height;
        Width = AppSettings.Instance.Width;
        Opacity = AppSettings.Instance.Opacity;


        panMain.Width = AppSettings.Instance.MainPanWidth;
        for (var i = 0; i < 3; i++) {
          lstFinanceHistory.Columns[i].Width = AppSettings.Instance.FinanceHistorySizes[i];
          lstHistory.Columns[i].Width = AppSettings.Instance.HistorySizes[i];
        }

        cmbCurrency.SelectedIndex = AppSettings.Instance.CurrencyIndex;
        timer.Interval = AppSettings.Instance.RefreshInterval * 60 * 1000;
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

        AppSettings.Instance.MainPanWidth = panMain.Width;
        for (var i = 0; i < 3; i++) {
          AppSettings.Instance.FinanceHistorySizes[i] = lstFinanceHistory.Columns[i].Width;
          AppSettings.Instance.HistorySizes[i] = lstHistory.Columns[i].Width;
        }

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
    }

    private void GetJsonData(string uri, Action<string> onResponse, int timeout = 10, string method = "GET") {

      Task.Factory.StartNew(async () => {
        var result = await Helper.GetJsonData(uri, timeout, method);
        if (result != null) {
          if (InvokeRequired)
            Invoke(new Action<string>(onResponse), result);
          else
            onResponse(result);
        }
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
      UpdateNbuRateText(currencyIndex);

      for (var cIndex = 0; cIndex < 2; cIndex++) {
        var task = nbuProvider.Refresh(cIndex);
      }
      GetJsonData($"http://resources.finance.ua/chart/data?for=currency-order&currency={Helper.GetCurrencyName(currencyIndex)}", OnFinanceUaResponse);

      var nowDate = DateTime.Now;
      if (lastCurrencyChanged > lastPrivat24HistoryGet || lastPrivat24HistoryGet.Date != nowDate.Date) {
        GetJsonData("https://otp24.privatbank.ua/v3/api/1/info/currency/history", OnPrivat24HistoryResponse, 30, "POST");
      }

      if (lastCurrencyChanged > lastMinfinStats || lastMinfinStats.AddMinutes(30) < nowDate) {
        UpdateMinfinChartData();
      }
    }

    private void UpdateNbuRateText(int currencyIndex) {
      var lastItem = nbuProvider.GetLastItem(currencyIndex);
      if (lastItem == null) return;
      Text = $"{appTitle} (NBU: {lastItem.Rate:n2} / {lastItem.Date:M})";
    }

    private void UpdateBrowser() {
      Common.UpdateBrowser(browser, Properties.Resources.weather);
    }

    private void OnPrivat24HistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var currencyCode = Helper.GetCurrencyName(cmbCurrency.SelectedIndex);
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

        var rateDelta = (i+1 < filteredItems.Length) ? item.Rate_B - filteredItems[i+1].Rate_B: item.Rate_B_Delta;
        var lvItem = new ListViewItem(item.Date, 0);
        lvItem.SubItems.Add(item.Rate_B.ToString("n3"));
        //lvItem.SubItems.Add(item.Rate_B_Delta.ToString("F"));
        lvItem.SubItems.Add(item.Rate_S.ToString("n3"));
        //lvItem.SubItems.Add(item.Rate_S_Delta.ToString("F"));
        lvItem.Tag = item;
        lvItem.BackColor = GetDiffColor(rateDelta);
        lstHistory.Items.Add(lvItem);
      }

      if (filteredItems[0].DateParsed.Date == DateTime.Today)
        lastPrivat24HistoryGet = DateTime.Now;
      // if (_lastPrivat24HistoryGet < filteredItems[0].DateParsed) {
      //   _lastPrivat24HistoryGet = filteredItems[0].DateParsed;
      // }
    }

    private void UpdateMinfinChartData() {
      var currencyCode = Helper.GetCurrencyName(cmbCurrency.SelectedIndex).ToLower();
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
      var showNbu = !nbuProvider.IsEmpty(currencyIndex);
      if (!showNbu)
        cbxShowNbu.Checked = false;
      cbxShowNbu.Visible = showNbu;

      foreach (var s in chart.Series)
        s.Points.Clear();

      foreach (var item in historyData.Values) {
        chart.Series[0].Points.AddXY(item.Date, item.Rate_Buy);
        chart.Series[1].Points.AddXY(item.Date, item.Rate_Sell);
        if (showNbu) {
          var nbuValue = nbuProvider.GetByDate(currencyIndex, item.Date);
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
      lastMinfinStats = DateTime.Now;
    }

    private void OnFinanceUaResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var chartData = response.FromJson<string[][]>();
      if (chartData == null || chartData.Length <= 0) return;

      lstFinanceHistory.Items.Clear();
      if (chartData.Length > 1) {
        var currentItem = chartData[chartData.Length - 1];
        for (var i = chartData.Length - 2; i >= 0; i--) {
          var prevItem = chartData[i];
          if (currentItem[1] == prevItem[1] && currentItem[2] == prevItem[2])
            continue;
          var timePart = currentItem[0].Split(' ')[1];
          var lvItem = new ListViewItem(timePart, 0) { UseItemStyleForSubItems = false };
          decimal.TryParse(currentItem[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var buy);
          decimal.TryParse(currentItem[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var sell);
          lvItem.SubItems.Add(buy.ToString("n3"), lstFinanceHistory.ForeColor, GetDiffColor(currentItem[1], prevItem[1]), lstFinanceHistory.Font);
          lvItem.SubItems.Add(sell.ToString("n3"), lstFinanceHistory.ForeColor, GetDiffColor(currentItem[2], prevItem[2]), lstFinanceHistory.Font);
          lstFinanceHistory.Items.Add(lvItem);

          currentItem = prevItem;
        }
      }
    }

    private void btnRefresh_Click(object sender, EventArgs e) {
      UpdateData();
    }

    private void chart_MouseMove(object sender, MouseEventArgs e) {

      // var chart = sender as Chart;
      if (!(chart?.Annotations[0] is CalloutAnnotation ta)) return;

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
      if (!nbuProvider.IsEmpty(currencyIndex)) {
        var nbuRate = nbuProvider.GetByDate(currencyIndex, date);
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
