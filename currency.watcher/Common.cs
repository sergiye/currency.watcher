using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace currency.watcher {

  internal class Common {

    internal const int WmSysCommand = 0x112;
    internal const int MfString = 0x0;
    internal const int MfSeparator = 0x800;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

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
