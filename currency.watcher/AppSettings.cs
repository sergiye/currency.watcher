using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace currency.watcher {

  public class AppSettings {

    private static readonly Lazy<AppSettings> InstanceField = new Lazy<AppSettings>(Load);
    internal static AppSettings Instance => InstanceField.Value;

    public int Left = Screen.PrimaryScreen.WorkingArea.Left; // Screen.PrimaryScreen.WorkingArea.Right - Width;
    public int Top = Screen.PrimaryScreen.WorkingArea.Top;
    public int Height = 245;
    public int Width = 175;
    public double Opacity = 1;
    public bool StickToEdges = true;
    
    public int ChartViewMode;
    public bool ChartLines;
    
    public int CurrencyIndex;
    public readonly int RefreshInterval = 30;

    public int MainPanWidth = 159;
    public int[] FinanceHistorySizes = { 40, 50, 50 };
    public int[] HistorySizes = { 85, 55, 55 };
    public int NbuHistoryWidth = 200;
    public int[] NbuHistorySizes = { 100, 90 };

    #region Load/Save

    public static string GetAppPath() {
      return Assembly.GetExecutingAssembly().Location;
    }

    private static string GetSettingsFilePath() {
      return Path.ChangeExtension(GetAppPath(), ".json");
    }

    internal void Save() {
      var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\sergiye\\currency.watcher");
      if (key == null) {
        SaveToFile();
        return;
      }
      key.SetValue("Height", Height);
      key.SetValue("Width", Width);
      key.SetValue("Top", Top);
      key.SetValue("Left", Left);
      key.SetValue("Opacity", Opacity);
      key.SetValue("StickToEdges", StickToEdges);
        
      // key.SetValue("RefreshInterval")?.ToString(), out result.RefreshInterval);

      key.SetValue("CurrencyIndex", CurrencyIndex);
      key.SetValue("ChartViewMode", ChartViewMode);
      key.SetValue("ChartLines", ChartLines);
      key.SetValue("MainPanWidth", MainPanWidth);
      key.SetValue("FinanceHistorySizes1", FinanceHistorySizes[0]);
      key.SetValue("FinanceHistorySizes2", FinanceHistorySizes[1]);
      key.SetValue("FinanceHistorySizes3", FinanceHistorySizes[2]);
      key.SetValue("HistorySizes1", HistorySizes[0]);
      key.SetValue("HistorySizes2", HistorySizes[1]);
      key.SetValue("HistorySizes3", HistorySizes[2]);
      key.SetValue("NbuHistorySizes1", NbuHistorySizes[0]);
      key.SetValue("NbuHistorySizes2", NbuHistorySizes[1]);

      key.SetValue("NbuHistoryWidth", NbuHistoryWidth);
    }

    private static AppSettings Load() {
      var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\sergiye\\currency.watcher");
      var result = new AppSettings();
      if (key == null) {
        LoadFromFile();
        return result;
      }
      int.TryParse(key.GetValue("Height")?.ToString(), out result.Height);
      int.TryParse(key.GetValue("Width")?.ToString(), out result.Width);
      int.TryParse(key.GetValue("Top")?.ToString(), out result.Top);
      int.TryParse(key.GetValue("Left")?.ToString(), out result.Left);
      double.TryParse(key.GetValue("Opacity")?.ToString(), out result.Opacity);
      bool.TryParse(key.GetValue("StickToEdges")?.ToString(), out result.StickToEdges);
        
      // int.TryParse(key.GetValue("RefreshInterval")?.ToString(), out result.RefreshInterval);

      int.TryParse(key.GetValue("CurrencyIndex")?.ToString(), out result.CurrencyIndex);
      int.TryParse(key.GetValue("ChartViewMode")?.ToString(), out result.ChartViewMode);
      bool.TryParse(key.GetValue("ChartLines")?.ToString(), out result.ChartLines);
      int.TryParse(key.GetValue("MainPanWidth")?.ToString(), out result.MainPanWidth);
      int.TryParse(key.GetValue("FinanceHistorySizes1")?.ToString(), out result.FinanceHistorySizes[0]);
      int.TryParse(key.GetValue("FinanceHistorySizes2")?.ToString(), out result.FinanceHistorySizes[1]);
      int.TryParse(key.GetValue("FinanceHistorySizes3")?.ToString(), out result.FinanceHistorySizes[2]);
      int.TryParse(key.GetValue("HistorySizes1")?.ToString(), out result.HistorySizes[0]);
      int.TryParse(key.GetValue("HistorySizes2")?.ToString(), out result.HistorySizes[1]);
      int.TryParse(key.GetValue("HistorySizes3")?.ToString(), out result.HistorySizes[2]);
      int.TryParse(key.GetValue("NbuHistorySizes1")?.ToString(), out result.NbuHistorySizes[0]);
      int.TryParse(key.GetValue("NbuHistorySizes2")?.ToString(), out result.NbuHistorySizes[1]);
        
      int.TryParse(key.GetValue("NbuHistoryWidth")?.ToString(), out result.NbuHistoryWidth);

      return result;
    }

    internal void SaveToFile() {
      var fileName = GetSettingsFilePath();
      GC.Collect(GC.MaxGeneration);
      File.WriteAllText(fileName, this.ToJson());
    }

    private static AppSettings LoadFromFile() {
      var fileName = GetSettingsFilePath();
      try {
        if (File.Exists(fileName)) {
          var fileData = File.ReadAllText(fileName);
          var result = fileData.FromJson<AppSettings>();
          if (result != null)
            return result;
        }
      }
      catch {
        // ignored
      }

      return new AppSettings();
    }

    #endregion Load/Save
  }
}