using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Sergiy.Informer {

  public class AppSettings {

    private static readonly Lazy<AppSettings> InstanceField = new Lazy<AppSettings>(Load);
    internal static AppSettings Instance => InstanceField.Value;

    public int Left;
    public int Top;
    public int Height;
    public int Width;
    public double Opacity;
    public bool StickToEdges;
    public bool Weather;
    
    public int ChartViewMode;
    public bool ChartLines;
    
    public int CurrencyIndex;
    public readonly int RefreshInterval;

    public AppSettings() {
      Left = Screen.PrimaryScreen.WorkingArea.Left;// Screen.PrimaryScreen.WorkingArea.Right - Width;
      Top = Screen.PrimaryScreen.WorkingArea.Top;
      Height = 245;
      Width = 175;
      Opacity = 1;
      StickToEdges = true;
      Weather = false;
    
      ChartViewMode = 0;
      ChartLines = false;

      CurrencyIndex = 0;
      RefreshInterval = 30;
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