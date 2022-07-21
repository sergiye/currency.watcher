using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace currency.watcher {

  internal class Common {

    internal const int WmSysCommand = 0x112;

    internal const int SC_SIZE = 0xF000;
    internal const int SC_MINIMIZE = 0xF020;
    internal const int SC_MAXIMIZE = 0xF030;

    internal const int MfUnchecked = 0x00000000;
    internal const int MfByCommand = 0x00000000;
    internal const int MfString = 0x00000000;
    internal const int MfGrayed = 0x00000001;
    internal const int MfDisabled = 0x00000002;
    internal const int MfChecked = 0x00000008;
    internal const int MfPopup = 0x00000010; //Pass the menu handle of the popup menu into the ID parameter
    internal const int MfBarBreak = 0x00000020;
    internal const int MfBreak = 0x00000040;
    internal const int MfByPosition = 0x00000400;
    internal const int MfSeparator = 0x00000800; //String and ID parameters are ignored

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-checkmenuitem
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool CheckMenuItem(IntPtr hMenu, int uIDCheckItem, int uCheck);

    [DllImport("user32.dll", EntryPoint = "InsertMenuW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    internal static extern int InsertMenu(IntPtr hMenu, int position, int uFlags, int uIdNewItem, string lpNewItem);

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-deletemenu
    [DllImport("user32.dll")]
    internal static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    //https://docs.microsoft.com/en-us/windows/console/getconsolewindow
    [DllImport("kernel32.dll", ExactSpelling = true)]
    internal static extern IntPtr GetConsoleWindow();

    internal static string GetDeveloperText() {
      return Encoding.ASCII.GetString(new byte[]
      {
                0x57, 0x72, 0x69, 0x74, 0x74, 0x65, 0x6E, 0x20, 0x62, 0x79, 0x20, 0x53, 0x65, 0x72, 0x67, 0x65,
                0x79, 0x20, 0x45, 0x67, 0x6F, 0x73, 0x68, 0x69, 0x6E, 0x20, 0x28, 0x65, 0x67, 0x6F, 0x73, 0x68,
                0x69, 0x6E, 0x2E, 0x73, 0x65, 0x72, 0x67, 0x65, 0x79, 0x40, 0x67, 0x6D, 0x61, 0x69, 0x6C, 0x2E,
                0x63, 0x6F, 0x6D, 0x29
      });
    }

    internal static bool DoSnap(int pos, int edge) {
      const int snapDist = 50;
      var delta = Math.Abs(pos - edge);
      return delta <= snapDist;
    }

    internal static void UpdateBrowser(WebBrowser browser, string documentData) {
      browser.DocumentText = "0";
      if (browser.Document != null) {
        browser.Document.OpenNew(true);
        browser.Document.Write(documentData);
      }
      browser.Refresh();
    }
  }
}
