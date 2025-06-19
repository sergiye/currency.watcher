using System;
using System.Runtime.InteropServices;

namespace currency.watcher {

  internal class WinApiHelper {

    internal const int WM_SYS_COMMAND = 0x112;
    internal const int HWND_BROADCAST = 0xffff;
    internal static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

    internal const int SC_SIZE = 0xF000;
    internal const int SC_MINIMIZE = 0xF020;
    internal const int SC_MAXIMIZE = 0xF030;

    internal const int MF_UNCHECKED = 0x00000000;
    internal const int MF_BY_COMMAND = 0x00000000;
    internal const int MF_STRING = 0x00000000;
    internal const int MF_GRAYED = 0x00000001;
    internal const int MF_DISABLED = 0x00000002;
    internal const int MF_CHECKED = 0x00000008;
    internal const int MF_POPUP = 0x00000010; //Pass the menu handle of the popup menu into the ID parameter
    internal const int MF_BAR_BREAK = 0x00000020;
    internal const int MF_BREAK = 0x00000040;
    internal const int MF_BY_POSITION = 0x00000400;
    internal const int MF_SEPARATOR = 0x00000800; //String and ID parameters are ignored

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-checkmenuitem
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool CheckMenuItem(IntPtr hMenu, int uIDCheckItem, int uCheck);

    [DllImport("user32.dll", EntryPoint = "InsertMenuW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    internal static extern int InsertMenu(IntPtr hMenu, uint position, int uFlags, int uIdNewItem, string lpNewItem);

    [DllImport("user32.dll")]
    internal static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-deletemenu
    [DllImport("user32.dll")]
    internal static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32")]
    internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

    [DllImport("user32")]
    internal static extern int RegisterWindowMessage(string message);
  }
}
