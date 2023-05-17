using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace currency.watcher {

  internal class Common {

    internal const int WmSysCommand = 0x112;
    internal const int HWND_BROADCAST = 0xffff;
    internal static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

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

    [DllImport("user32")]
    internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
    [DllImport("user32")]
    internal static extern int RegisterWindowMessage(string message);

    internal static bool DoSnap(int pos, int edge) {
      const int snapDist = 50;
      var delta = Math.Abs(pos - edge);
      return delta <= snapDist;
    }
  }
}
