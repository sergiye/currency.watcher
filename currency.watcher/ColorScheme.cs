using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace currency.watcher {
  
  public static class ColorScheme {

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    private static bool IsWindows10OrGreater(int build = -1) {
      return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
    }

    private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled) {
      if (!IsWindows10OrGreater(17763)) return false;
      var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
      if (IsWindows10OrGreater(18985))
        attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
      var useImmersiveDarkMode = enabled ? 1 : 0;
      return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
    }

    public static Color InputBackColor { get; } = SystemColors.Window;
    public static Color InputForeColor { get; } = SystemColors.WindowText;
    public static Color PanelBackColor { get; } = SystemColors.Control;
    public static Color PanelForeColor { get; } = SystemColors.ControlText;

    private static int? appsUseLightTheme;
    public static bool AppsUseLightTheme {
      get {
        if (!appsUseLightTheme.HasValue) {
          try {
            // 0 : dark theme, 1 : light theme, -1 : undefined
            appsUseLightTheme = (int) Registry.GetValue(
              "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme",
              -1);
          }
          catch {
            appsUseLightTheme = 1;
          }
        }
        return appsUseLightTheme == 1;
      }
    }
    
    static ColorScheme() {

      if (AppsUseLightTheme) return;

      InputBackColor = Color.DimGray;//FromArgb(255, 0x17, 0x16, 0x15);
      InputForeColor = Color.White;
      PanelBackColor = Color.DimGray;
      PanelForeColor = Color.White;
    }
    
    public static void ApplyColorScheme(this Control component) {
      if (AppsUseLightTheme) return;

      UseImmersiveDarkMode(component.Handle, true);
        
      switch (component) {
        case TabPage tabPage:
          tabPage.UseVisualStyleBackColor = true;
          component.BackColor = PanelBackColor;
          component.ForeColor = PanelForeColor;
          break;
        case Label _:
        case GroupBox _:
        case Panel _:
        case CheckBox _:
        case Button _:
          component.BackColor = PanelBackColor;
          component.ForeColor = PanelForeColor;
          break;
        case TextBox _:
        case ComboBox _:
        case NumericUpDown _:
        case DateTimePicker _:
          component.BackColor = InputBackColor;
          component.ForeColor = InputForeColor;
          break;
        case TabControl tabControl:
          component.BackColor = PanelBackColor;
          component.ForeColor = PanelForeColor;
          tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
          tabControl.DrawItem += (sender, e) => {
            var page = tabControl.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);
            var paddedBounds = e.Bounds;
            var yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor);
          };
          break;
        case ListView listView:
          component.BackColor = InputBackColor;
          component.ForeColor = InputForeColor;
          listView.OwnerDraw = true;
          listView.DrawColumnHeader += (sender, e) => {
            using (var backBrush = new SolidBrush(InputBackColor))
              e.Graphics.FillRectangle(backBrush, e.Bounds);
            // e.DrawText();
            using (var foreBrush = new SolidBrush(InputForeColor))
              e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, e.Bounds);
          };
          listView.DrawItem += (sender, e) => {
            e.DrawDefault = true;
          };
          break;
        default:
          //todo: implement other controls
          component.BackColor = PanelBackColor;
          component.ForeColor = PanelForeColor;
          break;
      }
      
      foreach (Control c in component.Controls) ApplyColorScheme(c);
    }
  }
}