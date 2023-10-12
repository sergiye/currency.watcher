using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace currency.watcher {

  public class AppSettings {

    private static readonly Lazy<AppSettings> InstanceField = new Lazy<AppSettings>(Load);
    internal static AppSettings Instance => InstanceField.Value;

    public int Left;
    public int Top;
    public int Height;
    public int Width;
    public double Opacity;
    public bool StickToEdges;
    
    public int ChartViewMode;
    public bool ChartLines;
    
    public int CurrencyIndex;
    public readonly int RefreshInterval;

    public int MainPanWidth;
    public int[] FinanceHistorySizes;
    public int[] HistorySizes;
    public int NbuHistoryWidth;
    public int[] NbuHistorySizes;

    public AppSettings() {
      Left = Screen.PrimaryScreen.WorkingArea.Left;// Screen.PrimaryScreen.WorkingArea.Right - Width;
      Top = Screen.PrimaryScreen.WorkingArea.Top;
      Height = 245;
      Width = 175;
      Opacity = 1;
      StickToEdges = true;
    
      ChartViewMode = 0;
      ChartLines = false;

      CurrencyIndex = 0;
      RefreshInterval = 30;

      MainPanWidth = 159;
      FinanceHistorySizes = new int[3] { 40, 50, 50 };
      HistorySizes = new int[3] { 85, 55, 55 };
      NbuHistoryWidth = 200;
      NbuHistorySizes = new int[2] { 100, 90 };
    }

    #region Load/Save

    public static string GetAppPath() {
      return Assembly.GetExecutingAssembly().Location;
    }

    private static string GetSettingsFilePath() {
      return Path.ChangeExtension(GetAppPath(), ".json");
    }

    internal void Save() {
      var fileName = GetSettingsFilePath();
      GC.Collect(GC.MaxGeneration);
      File.WriteAllText(fileName, this.ToJson());
    }

    private static AppSettings Load() {
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