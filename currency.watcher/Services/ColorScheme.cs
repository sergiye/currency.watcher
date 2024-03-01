using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace currency.watcher {
  
  public static class ColorScheme {

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    
    [DllImport("uxtheme.dll", SetLastError=true, ExactSpelling=true, CharSet=CharSet.Unicode)]

    public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWWMA_BORDER_COLOR = 34;
    private const int DWMWA_MICA_EFFECT = 1029;
    
    private static bool IsWindows10OrGreater(int build = -1) {
      return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
    }

    #region Colors
    
    public static Color InputBackColor { get; } = SystemColors.Window;
    public static Color InputForeColor { get; } = SystemColors.WindowText;
    public static Color PanelBackColor { get; } = SystemColors.Control;
    public static Color PanelForeColor { get; } = SystemColors.ControlText;
    public static readonly Color[] SeriesColors = { Color.Green, Color.Blue, Color.Orange };
    public static Color ColorGreater => AppsUseLightTheme ? Color.FromArgb(0xF7CF18) : Color.FromArgb(0xD55500);
    public static Color ColorLower => AppsUseLightTheme ? Color.FromArgb(0x8AB1F2) : Color.FromArgb(0x5070FF);

    #endregion
    
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

      InputBackColor = Color.DimGray;//Color.FromArgb(32, 32, 32);
      InputForeColor = Color.White;
      PanelBackColor = Color.DimGray;
      PanelForeColor = Color.White;
    }

    public static void ApplyColorScheme(this Control component) {
      if (AppsUseLightTheme) return;

      SetWindowTheme(component.Handle, "DarkMode_Explorer", null);
      if (IsWindows10OrGreater(17763)) {
        var attribute = IsWindows10OrGreater(18985)
          ? DWMWA_USE_IMMERSIVE_DARK_MODE
          : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
        var trueValue = 0x01;
        DwmSetWindowAttribute(component.Handle, attribute, ref trueValue, sizeof(int));
        DwmSetWindowAttribute(component.Handle, DWMWA_MICA_EFFECT, ref trueValue, sizeof(int));
      }
      // var border = int.Parse(ToBgr(InputBackColor), System.Globalization.NumberStyles.HexNumber);
      // DwmSetWindowAttribute(component.Handle, DWWMA_BORDER_COLOR, ref border, sizeof(int));
        
      switch (component) {
        case TabPage tabPage:
          tabPage.UseVisualStyleBackColor = true;
          tabPage.BorderStyle = BorderStyle.None;
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
            using (var foreBrush = new SolidBrush(InputForeColor))
              e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, e.Bounds);
          };
          // listView.DrawItem += (sender, e) => {
          //   if (e.Item.UseItemStyleForSubItems && !e.Item.Selected) {
          //
          //     var rowBounds = e.Bounds;
          //     var leftMargin = e.Item.GetBounds(ItemBoundsPortion.Label).Left;
          //     // e.Graphics.FillRectangle(SystemBrushes.Highlight,
          //     //   new Rectangle(leftMargin, rowBounds.Top, rowBounds.Width - leftMargin, rowBounds.Height));
          //     
          //     using (var backBrush = new SolidBrush(e.Item.BackColor))
          //       e.Graphics.FillRectangle(backBrush, new Rectangle(leftMargin, rowBounds.Top, rowBounds.Width - leftMargin, rowBounds.Height));
          //       // e.Graphics.FillRectangle(backBrush, e.Item.Bounds);
          //     // e.DrawText();
          //     // using (var foreBrush = new SolidBrush(e.Item.ForeColor))
          //     //   e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreBrush, e.Item.Bounds);
          //   }
          //   else {
          //     e.DrawDefault = true;              
          //   }
          // };
          listView.DrawSubItem += (sender, e) => {
            // if (e.Item.UseItemStyleForSubItems) {
            //   using (var backBrush = new SolidBrush(e.SubItem.BackColor))
            //     e.Graphics.FillRectangle(backBrush, e.SubItem.Bounds);
            //   // e.DrawText();
            //   using (var foreBrush = new SolidBrush(e.SubItem.ForeColor))
            //     e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, foreBrush, e.SubItem.Bounds);
            // }
            // else {
            //   e.DrawDefault = true;              
            // }
            
            // const int TEXT_OFFSET = 1;    // I don't know why the text is located at 1px to the right. Maybe it's only for me.
            // var lv = (ListView)sender;
            // Check if e.Item is selected and the ListView has a focus.
            // if (e.Item.Selected) {
            //   var rowBounds = e.SubItem.Bounds;
            //   var labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
            //   var leftMargin = labelBounds.Left - TEXT_OFFSET;
            //   var bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : rowBounds.Width - leftMargin - TEXT_OFFSET, rowBounds.Height);
            //   TextFormatFlags align;
            //   switch (lv.Columns[e.ColumnIndex].TextAlign) {
            //     case HorizontalAlignment.Right:
            //       align = TextFormatFlags.Right;
            //       break;
            //     case HorizontalAlignment.Center:
            //       align = TextFormatFlags.HorizontalCenter;
            //       break;
            //     default:
            //       align = TextFormatFlags.Left;
            //       break;
            //   }
            //   TextRenderer.DrawText(e.Graphics, e.SubItem.Text, lv.Font, bounds, SystemColors.HighlightText,
            //     align | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
            // }
            // else
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

    public static Color GetDiffColor(decimal lastValue, decimal prevValue) {
      if (lastValue == 0 || prevValue == 0)
        return InputBackColor;
      return lastValue.CompareTo(prevValue) == 1 ? ColorGreater
          : lastValue.CompareTo(prevValue) == -1 ? ColorLower
          : InputBackColor;
    }

    public static Color GetDiffColor(IComparable lastValue, IComparable prevValue) {
      if (lastValue == null || prevValue == null)
        return InputBackColor;
      return lastValue.CompareTo(prevValue) == 1 ? ColorGreater
          : lastValue.CompareTo(prevValue) == -1 ? ColorLower
          : InputBackColor;
    }

    public static Color GetDiffColor(decimal delta) {
      return delta > 0 ? ColorGreater
          : delta < 0 ? ColorLower
          : InputBackColor;
    }
  }
}