using System;
using System.Threading;
using System.Windows.Forms;

namespace currency.watcher {

  static class Program {

    static Mutex mutex = new Mutex(true, "{df1ad336-292a-4375-8fc6-109088e260df}");

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {

      if (!mutex.WaitOne(TimeSpan.Zero, true)) {
        MessageBox.Show("Another instance of the application is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        Common.PostMessage((IntPtr)Common.HWND_BROADCAST, Common.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
        Environment.Exit(0);
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
      mutex.ReleaseMutex();
    }
  }
}
