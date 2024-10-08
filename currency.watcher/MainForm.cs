﻿using System;
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
    private DateTime lastMinfinStats;
    private readonly List<CombinedTradeItem> combinedTradeItems = new List<CombinedTradeItem>();
    
    private DataProvider dataProvider;

    #region form decoration

    private const int SysMenuAboutId = 0x1;
    private const int SysMenuTopMost = 0x2;
    private const int SysMenuStickEdges = 0x3;

    protected override void OnHandleCreated(EventArgs e) {

      base.OnHandleCreated(e);

      var hSysMenu = Common.GetSystemMenu(Handle, false);

      //Common.DeleteMenu(hSysMenu, Common.SC_SIZE, Common.MfByCommand); // Disable resizing
      //Common.DeleteMenu(hSysMenu, Common.SC_MINIMIZE, Common.MfByCommand); // Disable minimizing
      Common.DeleteMenu(hSysMenu, Common.SC_MAXIMIZE, Common.MfByCommand); // Disable maximizing

      Common.AppendMenu(hSysMenu, Common.MfSeparator, 0, string.Empty);
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuTopMost, "&Always on top");
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuStickEdges, "&Stick edges");
      Common.AppendMenu(hSysMenu, Common.MfString, SysMenuAboutId, "&About…");
    }

    protected override void WndProc(ref Message m) {

      base.WndProc(ref m);
      IntPtr hSysMenu;
      if (m.Msg == Common.WmSysCommand) {
        switch ((int)m.WParam) {
          case SysMenuAboutId:
            var asm = Assembly.GetExecutingAssembly();
            MessageBox.Show($"{((AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute), false)).Title} {asm.GetName().Version.ToString(3)} {(Environment.Is64BitProcess ? "x64" : "x32")}\nWritten by Sergiy Egoshyn (egoshin.sergey@gmail.com)", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            break;
          case SysMenuTopMost:
            TopMost = !TopMost;
            AppSettings.Instance.TopMost = TopMost;
            hSysMenu = Common.GetSystemMenu(Handle, false);
            Common.CheckMenuItem(hSysMenu, SysMenuTopMost, TopMost ? Common.MfChecked : Common.MfUnchecked);
            break;
          case SysMenuStickEdges:
            AppSettings.Instance.StickToEdges = !AppSettings.Instance.StickToEdges;
            hSysMenu = Common.GetSystemMenu(Handle, false);
            Common.CheckMenuItem(hSysMenu, SysMenuStickEdges, AppSettings.Instance.StickToEdges ? Common.MfChecked : Common.MfUnchecked);
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

        dataProvider = new DataProvider(AppSettings.GetAppPath());
        dataProvider.OnDataChanged += () => {
          UpdateRates();
          UpdateMinfinChartData();
        };

        Left = AppSettings.Instance.Left;
        Top = AppSettings.Instance.Top;
        Height = AppSettings.Instance.Height;
        Width = AppSettings.Instance.Width;
        Opacity = AppSettings.Instance.Opacity;


        lstRates.Width = AppSettings.Instance.HistoryWidth;
        for (var i = 0; i < 5; i++) {
          lstFinanceHistory.Columns[i].Width = AppSettings.Instance.FinanceHistorySizes[i];
        }
        for (var i = 0; i < 7; i++) {
          lstRates.Columns[i].Width = AppSettings.Instance.HistorySizes[i];
        }

        timer.Interval = AppSettings.Instance.RefreshInterval * 60 * 1000;
        cmbChartMode.SelectedIndex = AppSettings.Instance.ChartViewMode;
        cbxChartGridMode.Checked = AppSettings.Instance.ChartLines;
        cbxShowNbu.Checked = AppSettings.Instance.ShowNbu;

        var hSysMenu = Common.GetSystemMenu(Handle, false);
        if (AppSettings.Instance.TopMost) {
          TopMost = true;
          Common.CheckMenuItem(hSysMenu, SysMenuTopMost, Common.MfChecked);
        }
        if (AppSettings.Instance.StickToEdges) {
          Common.CheckMenuItem(hSysMenu, SysMenuStickEdges, Common.MfChecked);
        }

        UpdateData();
        numTaxesSource_ValueChanged(this, EventArgs.Empty);
      };

      this.Closing += (sender, args) => {
        AppSettings.Instance.Left = Left;
        AppSettings.Instance.Top = Top;
        AppSettings.Instance.Height = Height;
        AppSettings.Instance.Width = Width;
        AppSettings.Instance.Opacity = Opacity;

        AppSettings.Instance.HistoryWidth = lstRates.Width;
        for (var i = 0; i < 5; i++) {
          AppSettings.Instance.FinanceHistorySizes[i] = lstFinanceHistory.Columns[i].Width;
        }
        for (var i = 0; i < 7; i++) {
          AppSettings.Instance.HistorySizes[i] = lstRates.Columns[i].Width;
        }

        AppSettings.Instance.ChartViewMode = cmbChartMode.SelectedIndex;
        AppSettings.Instance.ChartLines = cbxChartGridMode.Checked;
        AppSettings.Instance.ShowNbu = cbxShowNbu.Checked;

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

      lstFinanceHistory.DoubleClick += (s, e) => {
        Process.Start("https://minfin.com.ua/currency/auction/usd/sell/kharkov/?compact=true");
        //Process.Start("https://tables.finance.ua/ua/currency/official/-/1");
      };
      lstRates.DoubleClick += (s, e) => {
        Process.Start("https://next.privat24.ua/exchange-rates");
      };
    }

    private void GetJsonData(string uri, Action<string> onResponse, int timeout = 10, string method = "GET") {

      Task.Factory.StartNew(async () => {
        var result = await Common.GetJsonData(uri, timeout, method);
        if (result != null) {
          if (InvokeRequired)
            Invoke(new Action<string>(onResponse), result);
          else
            onResponse(result);
        }
      });
    }

    private void UpdateData() {
      
      UpdateRates();
      dataProvider.Refresh();

      if (lastMinfinStats.AddMinutes(30) < DateTime.Now) {
        UpdateMinfinChartData();
      }

      GetJsonData("http://resources.finance.ua/chart/data?for=currency-order&currency=usd",
        data => { OnFinanceUaResponse(true, data);}
      );
      GetJsonData("http://resources.finance.ua/chart/data?for=currency-order&currency=eur",
        data => { OnFinanceUaResponse(false, data);}
      );
    }

    private void UpdateRates() {

      if (dataProvider.IsEmpty()) return;

      if (InvokeRequired) {
        Invoke(new Action(UpdateRates));
        return;
      }

      //Text = $"{appTitle} (NBU: {lastItem.Rate:n3} {lastItem.Date:M})";

      lstRates.Items.Clear();

      var data = dataProvider.Take(100);
      if (data == null || data.Length == 0) return;

      var currentItem = data[data.Length - 1];
      for (var i = data.Length - 2; i >= 0; i--) {
        var prevItem = data[i];
        var timePart = currentItem.Date;
        var lvItem = lstRates.Items.Add(timePart.ToString("dd:MM"));

        lvItem.UseItemStyleForSubItems = !ColorScheme.AppsUseLightTheme;
        if (!ColorScheme.AppsUseLightTheme)
          lvItem.BackColor = ColorScheme.GetDiffColor(currentItem.NbuRateUsd, prevItem.NbuRateUsd);
        
        lvItem.SubItems.Add(currentItem.NbuRateUsd.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.NbuRateUsd, prevItem.NbuRateUsd), lstRates.Font);
        lvItem.SubItems.Add(currentItem.PbRateUsdB == 0 ? "" : currentItem.PbRateUsdB.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.PbRateUsdB, prevItem.PbRateUsdB), lstRates.Font);
        lvItem.SubItems.Add(currentItem.PbRateUsdS == 0 ? "" : currentItem.PbRateUsdS.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.PbRateUsdS, prevItem.PbRateUsdS), lstRates.Font);
        
        lvItem.SubItems.Add(currentItem.NbuRateEur.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.NbuRateEur, prevItem.NbuRateEur), lstRates.Font);
        lvItem.SubItems.Add(currentItem.PbRateEurB == 0 ? "" : currentItem.PbRateEurB.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.PbRateEurB, prevItem.PbRateEurB), lstRates.Font);
        lvItem.SubItems.Add(currentItem.PbRateEurS == 0 ? "" : currentItem.PbRateEurS.ToString("n3"), lstRates.ForeColor, ColorScheme.GetDiffColor(currentItem.PbRateEurS, prevItem.PbRateEurS), lstRates.Font);
        
        currentItem = prevItem;
      }
    }

    private void UpdateMinfinChartData() {
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
      GetJsonData($"https://va-rates.treeumapp.net/api/v1/rates?currency=usd&{filter}&city={city}", OnMinfinHistoryResponse);
    }

    private void OnMinfinHistoryResponse(string response) {
      if (string.IsNullOrEmpty(response)) return;
      var historyData = response.FromJson<MinfinHistoryResponse>();

      var currencyIndex = 0;
      if (currencyIndex == 0 && historyData?.Items?.Usd?.Length == 0) return;
      if (currencyIndex == 1 && historyData?.Items?.Eur?.Length == 0) return;

      foreach (var s in chart.Series)
        s.Points.Clear();

      var values = currencyIndex == 0 ? historyData?.Items?.Usd : historyData?.Items?.Eur;
      foreach (var item in values) {
        chart.Series[0].Points.AddXY(item.Date, item.Buy);
        chart.Series[1].Points.AddXY(item.Date, item.Sell);
        var nbuValue = dataProvider.GetByDate(item.Date);
        if (nbuValue != null) {
          chart.Series[2].Points.AddXY(item.Date, nbuValue.NbuRateUsd);
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

    private void OnFinanceUaResponse(bool usd, string response) {
      if (string.IsNullOrEmpty(response)) return;
      var chartData = response.FromJson<string[][]>();
      if (chartData == null || chartData.Length <= 1) return;

      var dataChanged = false;
      lock (combinedTradeItems) {
        foreach (var current in chartData) {
          var timePart = current[0].Split(' ')[1];
          decimal.TryParse(current[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var buy);
          decimal.TryParse(current[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var sell);
          var old = combinedTradeItems.FirstOrDefault(t => t.Date == timePart);
          if (old == null) {
            if (usd) {
              combinedTradeItems.Add(new CombinedTradeItem {
                Date = timePart,
                UsdB = buy,
                UsdS = sell
              });
            }
            else {
              combinedTradeItems.Add(new CombinedTradeItem {
                Date = timePart,
                EurB = buy,
                EurS = sell
              });
            }
            dataChanged = true;
          }
          else {
            if (usd) {
              if (old.UsdB == buy && old.UsdS == sell) continue;
              old.UsdB = buy;
              old.UsdS = sell;
              dataChanged = true;
            }
            else {
              if (old.EurB == buy && old.EurS == sell) continue;
              old.EurB = buy;
              old.EurS = sell;
              dataChanged = true;
            }
          } 
        }

        if (dataChanged) {
          combinedTradeItems.Sort((t1, t2) => string.CompareOrdinal(t2.Date, t1.Date));
        }
      }

      if (!dataChanged) return;
      
      CombinedTradeItem[] unlockedData;
      lock(combinedTradeItems)
        unlockedData = combinedTradeItems.ToArray();
      
      lstFinanceHistory.Items.Clear();
      for (var i = 0; i < unlockedData.Length - 1; i++) {
        var currentItem = unlockedData[i];
        var prevItem = unlockedData[i + 1];
        if (prevItem.RatesEquals(currentItem)) continue;
        var lvItem = lstFinanceHistory.Items.Add(currentItem.Date);
        lvItem.UseItemStyleForSubItems = !ColorScheme.AppsUseLightTheme;
        if (!ColorScheme.AppsUseLightTheme)
          lvItem.BackColor = ColorScheme.GetDiffColor(currentItem.UsdB, prevItem.UsdB);
        lvItem.SubItems.Add(currentItem.UsdB.ToString("n3"), lstFinanceHistory.ForeColor,
          ColorScheme.GetDiffColor(currentItem.UsdB, prevItem.UsdB), lstFinanceHistory.Font);
        lvItem.SubItems.Add(currentItem.UsdS.ToString("n3"), ColorScheme.InputForeColor,
          ColorScheme.GetDiffColor(currentItem.UsdS, prevItem.UsdS), lstFinanceHistory.Font);
        lvItem.SubItems.Add(currentItem.EurB.ToString("n3"), lstFinanceHistory.ForeColor,
          ColorScheme.GetDiffColor(currentItem.EurB, prevItem.EurB), lstFinanceHistory.Font);
        lvItem.SubItems.Add(currentItem.EurS.ToString("n3"), ColorScheme.InputForeColor,
          ColorScheme.GetDiffColor(currentItem.EurS, prevItem.EurS), lstFinanceHistory.Font);
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
          if (chart.Series[2].Enabled) {
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
      if (!dataProvider.IsEmpty()) {
        var nbuRate = dataProvider.GetByDate(date);
        if (nbuRate != null) {
          var uah = usd * nbuRate.NbuRateUsd;
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
