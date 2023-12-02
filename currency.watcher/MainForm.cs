using System;
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
            var asm = Assembly.GetExecutingAssembly();
            MessageBox.Show($"{((AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute), false)).Title} {asm.GetName().Version.ToString(3)} {(Environment.Is64BitProcess ? "x64" : "x32")}\nWritten by Sergiy Egoshyn (egoshin.sergey@gmail.com)", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

      this.ApplyColorScheme();
        
      var asm = Assembly.GetExecutingAssembly();
      appTitle = $"{((AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute), false)).Title} {(Environment.Is64BitProcess ? "x64" : "x32")} ";
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

      for(var i = 0; i < ColorScheme.SeriesColors.Length; i++) {
        chart.Series.Add(new Series {
          ChartArea = chart.ChartAreas[0].Name,
          ChartType = SeriesChartType.Spline,
          Color = ColorScheme.SeriesColors[i],
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
        AppSettings.Instance.CurrencyIndex = cmbCurrency.SelectedIndex;
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
          if (index == AppSettings.Instance.CurrencyIndex) {
            UpdateNbuRates(index);
          }
        };

        Left = AppSettings.Instance.Left;
        Top = AppSettings.Instance.Top;
        Height = AppSettings.Instance.Height;
        Width = AppSettings.Instance.Width;
        Opacity = AppSettings.Instance.Opacity;


        lstFinanceHistory.Width = AppSettings.Instance.MainPanWidth;
        for (var i = 0; i < 3; i++) {
          lstFinanceHistory.Columns[i].Width = AppSettings.Instance.FinanceHistorySizes[i];
          lstHistory.Columns[i].Width = AppSettings.Instance.HistorySizes[i];
        }

        lstNbuRates.Width = AppSettings.Instance.NbuHistoryWidth;
        lstNbuRates.Columns[0].Width = AppSettings.Instance.NbuHistorySizes[0];
        lstNbuRates.Columns[1].Width = AppSettings.Instance.NbuHistorySizes[1];

        cmbCurrency.SelectedIndex = AppSettings.Instance.CurrencyIndex;
        timer.Interval = AppSettings.Instance.RefreshInterval * 60 * 1000;
        cmbChartMode.SelectedIndex = AppSettings.Instance.ChartViewMode;
        cbxChartGridMode.Checked = AppSettings.Instance.ChartLines;
        cbxStickEdges.Checked = AppSettings.Instance.StickToEdges;
      };

      this.Closing += (sender, args) => {
        AppSettings.Instance.Left = Left;
        AppSettings.Instance.Top = Top;
        AppSettings.Instance.Height = Height;
        AppSettings.Instance.Width = Width;
        AppSettings.Instance.Opacity = Opacity;

        AppSettings.Instance.MainPanWidth = lstFinanceHistory.Width;
        for (var i = 0; i < 3; i++) {
          AppSettings.Instance.FinanceHistorySizes[i] = lstFinanceHistory.Columns[i].Width;
          AppSettings.Instance.HistorySizes[i] = lstHistory.Columns[i].Width;
        }

        AppSettings.Instance.NbuHistoryWidth = lstNbuRates.Width;
        AppSettings.Instance.NbuHistorySizes[0] = lstNbuRates.Columns[0].Width;
        AppSettings.Instance.NbuHistorySizes[1] = lstNbuRates.Columns[1].Width;

        // AppSettings.Instance.CurrencyIndex = cmbCurrency.SelectedIndex;

        AppSettings.Instance.ChartViewMode = cmbChartMode.SelectedIndex;
        AppSettings.Instance.ChartLines = cbxChartGridMode.Checked;

        AppSettings.Instance.Save();
      };

      //chart.ChartAreas[0].BackColor = Color.Black;
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
        chart.ChartAreas[0].AxisY.LabelStyle.Format = "{#.##}";
        //chart.ChartAreas[0].AxisX.MinorGrid.Enabled = checkState;
        //chart.ChartAreas[0].AxisY.MinorGrid.Enabled = checkState;
      };

      cbxShowNbu.CheckedChanged += (s, e) => {
        chart.Series[2].Enabled = cbxShowNbu.Checked;
        UpdateChartBounds();
      };

      cbxStickEdges.CheckedChanged += (s, e) => {
        AppSettings.Instance.StickToEdges = cbxStickEdges.Checked;
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
        return ColorScheme.InputBackColor;
      return lastValue.CompareTo(prevValue) == 1 ? ColorScheme.ColorGreater
          : lastValue.CompareTo(prevValue) == -1 ? ColorScheme.ColorLower
          : ColorScheme.InputBackColor;
    }

    private Color GetDiffColor(decimal delta) {
      return delta > 0 ? ColorScheme.ColorGreater
          : delta < 0 ? ColorScheme.ColorLower
          : ColorScheme.InputBackColor;
    }

    private void UpdateData() {
      var currencyIndex = AppSettings.Instance.CurrencyIndex;
      UpdateNbuRates(currencyIndex);

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

    private void UpdateNbuRates(int currencyIndex) {
      if (InvokeRequired) {
        Invoke(new Action<int>(UpdateNbuRates), currencyIndex);
        return;
      }

      var lastItem = nbuProvider.GetLastItem(currencyIndex);
      if (lastItem == null) return;
      //Text = $"{appTitle} (NBU: {lastItem.Rate:n3} {lastItem.Date:M})";

      lstNbuRates.Items.Clear();

      var data = nbuProvider.GetLastItems(currencyIndex, 100);
      if (data == null || data.Length == 0) return;

      var currentItem = data[data.Length - 1];
      for (var i = data.Length - 2; i >= 0; i--) {
        var prevItem = data[i];
        var timePart = currentItem.Date;
        var lvItem = new ListViewItem(timePart.ToString("dd:MM:yy"), 0) {
          UseItemStyleForSubItems = !ColorScheme.AppsUseLightTheme
        };
        lvItem.SubItems.Add(currentItem.Rate.ToString("n3"), lstNbuRates.ForeColor, GetDiffColor(currentItem.Rate, prevItem.Rate), lstNbuRates.Font);
        if (!ColorScheme.AppsUseLightTheme)
          lvItem.BackColor = GetDiffColor(currentItem.Rate, prevItem.Rate);

        lstNbuRates.Items.Add(lvItem);

        currentItem = prevItem;
      }
    }

    private void OnPrivat24HistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var currencyCode = Helper.GetCurrencyName(AppSettings.Instance.CurrencyIndex);
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
      var currencyCode = Helper.GetCurrencyName(AppSettings.Instance.CurrencyIndex).ToLower();
      var city = 22;
      string filter;
      switch (cmbChartMode.SelectedIndex) {
        case 1:
          filter = "period=month&group=day";
          break;
        case 2:
          filter = "period=3month&group=day"; //group=week
          break;
        default:
          filter = "period=week"; //&group=hour
          break;
      }
      GetJsonData($"https://va-rates.treeumapp.net/api/v1/rates?currency={currencyCode}&{filter}&city={city}", OnMinfinHistoryResponse);
    }

    private void OnMinfinHistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var historyData = response.FromJson<MinfinHistoryResponse>();

      var currencyIndex = AppSettings.Instance.CurrencyIndex;
      if (currencyIndex == 0 && historyData?.Items?.Usd?.Length == 0) return;
      if (currencyIndex == 1 && historyData?.Items?.Eur?.Length == 0) return;

      var showNbu = !nbuProvider.IsEmpty(currencyIndex);
      if (!showNbu)
        cbxShowNbu.Checked = false;
      cbxShowNbu.Visible = showNbu;

      foreach (var s in chart.Series)
        s.Points.Clear();

      var values = currencyIndex == 0 ? historyData?.Items?.Usd : historyData?.Items?.Eur;
      foreach (var item in values) {
        chart.Series[0].Points.AddXY(item.Date, item.Buy);
        chart.Series[1].Points.AddXY(item.Date, item.Sell);
        if (showNbu) {
          var nbuValue = nbuProvider.GetByDate(currencyIndex, item.Date);
          if (nbuValue != null) {
            chart.Series[2].Points.AddXY(item.Date, nbuValue.Rate);
          }
        }
        //todo: process "count_sell": 3, "count_buy": 7,
      }
      UpdateChartBounds();

      //if (historyData.Last().Value.Date == DateTime.Now.Date)
      lastMinfinStats = DateTime.Now;
    }

    private void UpdateChartBounds() {
      if (chart.Series[0].Points.Count == 0) return;
      //chart.ChartAreas[0].RecalculateAxesScale();
      var chartMin = chart.Series[0].Points.Min(p => p.YValues[0]);
      var chartMax = chart.Series[0].Points.Max(p => p.YValues[0]);
      var maxSeries = chart.Series[2].Enabled ? chart.Series.Count : chart.Series.Count - 1;
      for (int i = 1; i < maxSeries; i++) {
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
          var lvItem = new ListViewItem(timePart, 0) {
            UseItemStyleForSubItems = !ColorScheme.AppsUseLightTheme
          };
          decimal.TryParse(currentItem[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var buy);
          decimal.TryParse(currentItem[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var sell);
          lvItem.SubItems.Add(buy.ToString("n3"), lstFinanceHistory.ForeColor, GetDiffColor(currentItem[1], prevItem[1]), lstFinanceHistory.Font);
          lvItem.SubItems.Add(sell.ToString("n3"), ColorScheme.InputForeColor, GetDiffColor(currentItem[2], prevItem[2]), lstFinanceHistory.Font);
          if (!ColorScheme.AppsUseLightTheme)
            lvItem.BackColor = GetDiffColor(currentItem[2], prevItem[2]);

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
      var currencyIndex = AppSettings.Instance.CurrencyIndex;
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
