using System;
using System.Drawing;
using sergiye.Common;

namespace currency.watcher {
  
  public static class ColorScheme {

    #region Colors
    
    public static readonly Color[] SeriesColors = { Color.Green, Color.Blue, Color.Orange };

    private static Color ColorGreater => Theme.Current.WindowTitlebarFallbackToImmersiveDarkMode ? Color.FromArgb(0xBF652E) : Color.FromArgb(0xF7CF18);
    private static Color ColorLower => Theme.Current.WindowTitlebarFallbackToImmersiveDarkMode ? Color.FromArgb(0x2170CF) : Color.FromArgb(0x8AB1F2);

    #endregion
    
    public static Color GetDiffColor(decimal lastValue, decimal prevValue) {
      if (lastValue == 0 || prevValue == 0)
        return Theme.Current.BackgroundColor;
      return lastValue.CompareTo(prevValue) == 1 ? ColorGreater
          : lastValue.CompareTo(prevValue) == -1 ? ColorLower
          : Theme.Current.BackgroundColor;
    }

    public static Color GetDiffColor(IComparable lastValue, IComparable prevValue) {
      if (lastValue == null || prevValue == null)
        return Theme.Current.BackgroundColor;
      return lastValue.CompareTo(prevValue) == 1 ? ColorGreater
          : lastValue.CompareTo(prevValue) == -1 ? ColorLower
          : Theme.Current.BackgroundColor;
    }

    public static Color GetDiffColor(decimal delta) {
      return delta > 0 ? ColorGreater
          : delta < 0 ? ColorLower
          : Theme.Current.BackgroundColor;
    }
  }
}