﻿using sergiye.Common;
using System;
using System.Windows.Forms;

namespace currency.watcher {

  static class Program {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {

      if (!OperatingSystemHelper.IsCompatible(true, out var errorMessage, out var fixAction)) {
        if (fixAction != null) {
          if (MessageBox.Show(errorMessage, Updater.ApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
            fixAction?.Invoke();
          }
        }
        else {
          MessageBox.Show(errorMessage, Updater.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        Environment.Exit(0);
      }

      if (WinApiHelper.CheckRunningInstances(true, true)) {
        MessageBox.Show($"{Updater.ApplicationTitle} is already running.\nIt is recommended to close this instance.", Updater.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Environment.Exit(0);
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}
