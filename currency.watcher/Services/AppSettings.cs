using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace currency.watcher {

  public class AppSettings {

    private static readonly Lazy<AppSettings> instanceField = new Lazy<AppSettings>(Load);
    internal static AppSettings Instance => instanceField.Value;
    private static readonly string settingsPath;

    public int Left = Screen.PrimaryScreen.WorkingArea.Left; // Screen.PrimaryScreen.WorkingArea.Right - Width;
    public int Top = Screen.PrimaryScreen.WorkingArea.Top;
    public int Height = 245;
    public int Width = 175;
    public double Opacity = 1;
    public bool TopMost;
    public bool StickToEdges;
    
    public int ChartViewMode;
    public bool ChartLines;
    public bool ShowNbu;
    
    public readonly int RefreshInterval = 30;

    public int[] FinanceHistorySizes = { 40, 50, 50, 50, 50 };
    public int[] HistorySizes = { 100, 90, 90, 55, 55, 55, 55 };
    public int HistoryWidth = 300;
    public bool PortableMode = true;
    public static readonly string AppPath;

    #region Load/Save
    
    static AppSettings() {
      AppPath = typeof(AppSettings).Assembly.Location;
      settingsPath = Path.ChangeExtension(AppPath, ".json");
    }
    
    internal void Save() {

      if (PortableMode) {
        File.WriteAllText(settingsPath, this.ToJson());
        return;
      }
      var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\sergiye\\currency.watcher");
      if (key == null)
        return;
      
      key.SetValue("Height", Height);
      key.SetValue("Width", Width);
      key.SetValue("Top", Top);
      key.SetValue("Left", Left);
      key.SetValue("Opacity", Opacity);
      key.SetValue("TopMost", TopMost);
      key.SetValue("StickToEdges", StickToEdges);
        
      // key.SetValue("RefreshInterval")?.ToString(), out result.RefreshInterval);

      key.SetValue("ChartViewMode", ChartViewMode);
      key.SetValue("ChartLines", ChartLines);
      key.SetValue("ShowNbu", ShowNbu);
      key.SetValue("FinanceHistorySizes1", FinanceHistorySizes[0]);
      key.SetValue("FinanceHistorySizes2", FinanceHistorySizes[1]);
      key.SetValue("FinanceHistorySizes3", FinanceHistorySizes[2]);
      key.SetValue("FinanceHistorySizes4", FinanceHistorySizes[3]);
      key.SetValue("FinanceHistorySizes5", FinanceHistorySizes[4]);
      key.SetValue("HistorySizes1", HistorySizes[0]);
      key.SetValue("HistorySizes2", HistorySizes[1]);
      key.SetValue("HistorySizes3", HistorySizes[2]);
      key.SetValue("HistorySizes4", HistorySizes[3]);
      key.SetValue("HistorySizes5", HistorySizes[4]);
      key.SetValue("HistorySizes6", HistorySizes[5]);
      key.SetValue("HistorySizes7", HistorySizes[6]);

      key.SetValue("HistoryWidth", HistoryWidth);
    }

    private static AppSettings Load() {
      
      AppSettings result;
      try {
        if (File.Exists(settingsPath)) {
          var fileData = File.ReadAllText(settingsPath);
          result = fileData.FromJson<AppSettings>();
          if (result != null) {
            return result;
          }
        }
      }
      catch {
        // ignored
      }

      var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\sergiye\\currency.watcher");
      result = new AppSettings();
      if (key == null) {
        return result;
      }
      result.Height = TryGetInt(key.GetValue("Height")?.ToString(), result.Height);
      result.Width = TryGetInt(key.GetValue("Width")?.ToString(), result.Width);
      result.Top = TryGetInt(key.GetValue("Top")?.ToString(), result.Top);
      result.Left = TryGetInt(key.GetValue("Left")?.ToString(), result.Left);
      double.TryParse(key.GetValue("Opacity")?.ToString(), out result.Opacity);
      bool.TryParse(key.GetValue("TopMost")?.ToString(), out result.TopMost);
      bool.TryParse(key.GetValue("StickToEdges")?.ToString(), out result.StickToEdges);
      
      // int.TryParse(key.GetValue("RefreshInterval")?.ToString(), out result.RefreshInterval);

      result.ChartViewMode = TryGetInt(key.GetValue("ChartViewMode")?.ToString(), result.ChartViewMode);
      bool.TryParse(key.GetValue("ChartLines")?.ToString(), out result.ChartLines);
      bool.TryParse(key.GetValue("ShowNbu")?.ToString(), out result.ShowNbu);
      result.FinanceHistorySizes[0] = TryGetInt(key.GetValue("FinanceHistorySizes1")?.ToString(), result.FinanceHistorySizes[0]);
      result.FinanceHistorySizes[1] = TryGetInt(key.GetValue("FinanceHistorySizes2")?.ToString(), result.FinanceHistorySizes[1]);
      result.FinanceHistorySizes[2] = TryGetInt(key.GetValue("FinanceHistorySizes3")?.ToString(), result.FinanceHistorySizes[2]);
      result.FinanceHistorySizes[3] = TryGetInt(key.GetValue("FinanceHistorySizes4")?.ToString(), result.FinanceHistorySizes[3]);
      result.FinanceHistorySizes[4] = TryGetInt(key.GetValue("FinanceHistorySizes5")?.ToString(), result.FinanceHistorySizes[4]);
      result.HistorySizes[0] = TryGetInt(key.GetValue("HistorySizes1")?.ToString(), result.HistorySizes[0]);
      result.HistorySizes[1] = TryGetInt(key.GetValue("HistorySizes2")?.ToString(), result.HistorySizes[1]);
      result.HistorySizes[2] = TryGetInt(key.GetValue("HistorySizes3")?.ToString(), result.HistorySizes[2]);
      result.HistorySizes[3] = TryGetInt(key.GetValue("HistorySizes4")?.ToString(), result.HistorySizes[3]);
      result.HistorySizes[4] = TryGetInt(key.GetValue("HistorySizes5")?.ToString(), result.HistorySizes[4]);
      result.HistorySizes[5] = TryGetInt(key.GetValue("HistorySizes6")?.ToString(), result.HistorySizes[5]);
      result.HistorySizes[6] = TryGetInt(key.GetValue("HistorySizes7")?.ToString(), result.HistorySizes[6]);

      result.HistoryWidth = TryGetInt(key.GetValue("HistoryWidth")?.ToString(), result.HistoryWidth);

      return result;
    }

    private static int TryGetInt(string value, int defaultResult) {
      if (int.TryParse(value, out var intRes))
        return intRes;
      return defaultResult;
    }

    #endregion Load/Save
  }
}