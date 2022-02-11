using System.ComponentModel;
using System.Windows.Forms;

namespace currency.watcher {
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.btnRefresh = new System.Windows.Forms.Button();
      this.btnTop = new System.Windows.Forms.Button();
      this.cmbCurrency = new System.Windows.Forms.ComboBox();
      this.browser = new System.Windows.Forms.WebBrowser();
      this.panMain = new System.Windows.Forms.Panel();
      this.lstFinanceHistory = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lstHistory = new System.Windows.Forms.ListView();
      this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colBuy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colSale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.panHistory = new System.Windows.Forms.Panel();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageMain = new System.Windows.Forms.TabPage();
      this.panPrivat24Business = new System.Windows.Forms.GroupBox();
      this.tabPageGraphic = new System.Windows.Forms.TabPage();
      this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.tabTaxes = new System.Windows.Forms.TabPage();
      this.lblUsd = new System.Windows.Forms.Label();
      this.numTaxesSource = new System.Windows.Forms.NumericUpDown();
      this.lblTaxesResult = new System.Windows.Forms.Label();
      this.lblUah = new System.Windows.Forms.Label();
      this.lblDate = new System.Windows.Forms.Label();
      this.txtUahResult = new System.Windows.Forms.TextBox();
      this.txtTaxesResult = new System.Windows.Forms.TextBox();
      this.dtTaxesSource = new System.Windows.Forms.DateTimePicker();
      this.panStatus = new System.Windows.Forms.Panel();
      this.cbxWeather = new System.Windows.Forms.CheckBox();
      this.panGridOptions = new System.Windows.Forms.Panel();
      this.cbxShowNbu = new System.Windows.Forms.CheckBox();
      this.cmbChartMode = new System.Windows.Forms.ComboBox();
      this.cbxChartGridMode = new System.Windows.Forms.CheckBox();
      this.cbxStickEdges = new System.Windows.Forms.CheckBox();
      this.panMain.SuspendLayout();
      this.panHistory.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tabPageMain.SuspendLayout();
      this.panPrivat24Business.SuspendLayout();
      this.tabPageGraphic.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
      this.tabTaxes.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numTaxesSource)).BeginInit();
      this.panStatus.SuspendLayout();
      this.panGridOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnRefresh
      // 
      this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.btnRefresh.Location = new System.Drawing.Point(73, 0);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(25, 26);
      this.btnRefresh.TabIndex = 1;
      this.btnRefresh.Text = "R";
      this.btnRefresh.UseVisualStyleBackColor = false;
      // 
      // btnTop
      // 
      this.btnTop.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnTop.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.btnTop.Location = new System.Drawing.Point(98, 0);
      this.btnTop.Name = "btnTop";
      this.btnTop.Size = new System.Drawing.Size(25, 26);
      this.btnTop.TabIndex = 2;
      this.btnTop.Text = "T";
      this.btnTop.UseVisualStyleBackColor = false;
      this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
      // 
      // cmbCurrency
      // 
      this.cmbCurrency.Dock = System.Windows.Forms.DockStyle.Left;
      this.cmbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCurrency.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.cmbCurrency.FormattingEnabled = true;
      this.cmbCurrency.Items.AddRange(new object[] {
            "USD",
            "EUR"});
      this.cmbCurrency.Location = new System.Drawing.Point(0, 0);
      this.cmbCurrency.Name = "cmbCurrency";
      this.cmbCurrency.Size = new System.Drawing.Size(73, 29);
      this.cmbCurrency.TabIndex = 0;
      // 
      // browser
      // 
      this.browser.Dock = System.Windows.Forms.DockStyle.Top;
      this.browser.IsWebBrowserContextMenuEnabled = false;
      this.browser.Location = new System.Drawing.Point(0, 0);
      this.browser.MinimumSize = new System.Drawing.Size(20, 20);
      this.browser.Name = "browser";
      this.browser.ScrollBarsEnabled = false;
      this.browser.Size = new System.Drawing.Size(159, 110);
      this.browser.TabIndex = 0;
      this.browser.Visible = false;
      // 
      // panMain
      // 
      this.panMain.Controls.Add(this.lstFinanceHistory);
      this.panMain.Controls.Add(this.browser);
      this.panMain.Dock = System.Windows.Forms.DockStyle.Left;
      this.panMain.Location = new System.Drawing.Point(3, 3);
      this.panMain.Name = "panMain";
      this.panMain.Size = new System.Drawing.Size(159, 197);
      this.panMain.TabIndex = 17;
      // 
      // lstFinanceHistory
      // 
      this.lstFinanceHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.lstFinanceHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.lstFinanceHistory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstFinanceHistory.FullRowSelect = true;
      this.lstFinanceHistory.GridLines = true;
      this.lstFinanceHistory.HideSelection = false;
      this.lstFinanceHistory.Location = new System.Drawing.Point(0, 110);
      this.lstFinanceHistory.Name = "lstFinanceHistory";
      this.lstFinanceHistory.Size = new System.Drawing.Size(159, 87);
      this.lstFinanceHistory.TabIndex = 1;
      this.lstFinanceHistory.UseCompatibleStateImageBehavior = false;
      this.lstFinanceHistory.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Time";
      this.columnHeader1.Width = 50;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Buy";
      this.columnHeader2.Width = 55;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Sale";
      this.columnHeader3.Width = 55;
      // 
      // lstHistory
      // 
      this.lstHistory.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.lstHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.lstHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colBuy,
            this.colSale});
      this.lstHistory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstHistory.FullRowSelect = true;
      this.lstHistory.GridLines = true;
      this.lstHistory.HideSelection = false;
      this.lstHistory.Location = new System.Drawing.Point(3, 22);
      this.lstHistory.Name = "lstHistory";
      this.lstHistory.Size = new System.Drawing.Size(385, 172);
      this.lstHistory.TabIndex = 0;
      this.lstHistory.UseCompatibleStateImageBehavior = false;
      this.lstHistory.View = System.Windows.Forms.View.Details;
      // 
      // colDate
      // 
      this.colDate.Text = "Date";
      this.colDate.Width = 85;
      // 
      // colBuy
      // 
      this.colBuy.Text = "Buy";
      this.colBuy.Width = 55;
      // 
      // colSale
      // 
      this.colSale.Text = "Sale";
      this.colSale.Width = 55;
      // 
      // panHistory
      // 
      this.panHistory.Controls.Add(this.tabControl);
      this.panHistory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panHistory.Location = new System.Drawing.Point(0, 0);
      this.panHistory.Name = "panHistory";
      this.panHistory.Size = new System.Drawing.Size(564, 235);
      this.panHistory.TabIndex = 19;
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.tabPageMain);
      this.tabControl.Controls.Add(this.tabPageGraphic);
      this.tabControl.Controls.Add(this.tabTaxes);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(564, 235);
      this.tabControl.TabIndex = 0;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      // 
      // tabPageMain
      // 
      this.tabPageMain.Controls.Add(this.panPrivat24Business);
      this.tabPageMain.Controls.Add(this.panMain);
      this.tabPageMain.Location = new System.Drawing.Point(4, 28);
      this.tabPageMain.Name = "tabPageMain";
      this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMain.Size = new System.Drawing.Size(556, 203);
      this.tabPageMain.TabIndex = 1;
      this.tabPageMain.Text = "Rates";
      this.tabPageMain.UseVisualStyleBackColor = true;
      // 
      // panPrivat24Business
      // 
      this.panPrivat24Business.Controls.Add(this.lstHistory);
      this.panPrivat24Business.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panPrivat24Business.Location = new System.Drawing.Point(162, 3);
      this.panPrivat24Business.Name = "panPrivat24Business";
      this.panPrivat24Business.Size = new System.Drawing.Size(391, 197);
      this.panPrivat24Business.TabIndex = 18;
      this.panPrivat24Business.TabStop = false;
      this.panPrivat24Business.Text = "Privat 24 business";
      // 
      // tabPageGraphic
      // 
      this.tabPageGraphic.Controls.Add(this.chart);
      this.tabPageGraphic.Location = new System.Drawing.Point(4, 25);
      this.tabPageGraphic.Name = "tabPageGraphic";
      this.tabPageGraphic.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageGraphic.Size = new System.Drawing.Size(556, 206);
      this.tabPageGraphic.TabIndex = 0;
      this.tabPageGraphic.Text = "Graphic";
      this.tabPageGraphic.UseVisualStyleBackColor = true;
      // 
      // chart
      // 
      this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.chart.Location = new System.Drawing.Point(3, 3);
      this.chart.Name = "chart";
      this.chart.Size = new System.Drawing.Size(550, 200);
      this.chart.TabIndex = 20;
      this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
      // 
      // tabTaxes
      // 
      this.tabTaxes.Controls.Add(this.lblUsd);
      this.tabTaxes.Controls.Add(this.numTaxesSource);
      this.tabTaxes.Controls.Add(this.lblTaxesResult);
      this.tabTaxes.Controls.Add(this.lblUah);
      this.tabTaxes.Controls.Add(this.lblDate);
      this.tabTaxes.Controls.Add(this.txtUahResult);
      this.tabTaxes.Controls.Add(this.txtTaxesResult);
      this.tabTaxes.Controls.Add(this.dtTaxesSource);
      this.tabTaxes.Location = new System.Drawing.Point(4, 25);
      this.tabTaxes.Name = "tabTaxes";
      this.tabTaxes.Padding = new System.Windows.Forms.Padding(3);
      this.tabTaxes.Size = new System.Drawing.Size(556, 206);
      this.tabTaxes.TabIndex = 2;
      this.tabTaxes.Text = "Taxes calculator";
      this.tabTaxes.UseVisualStyleBackColor = true;
      // 
      // lblUsd
      // 
      this.lblUsd.AutoSize = true;
      this.lblUsd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblUsd.Location = new System.Drawing.Point(8, 8);
      this.lblUsd.Name = "lblUsd";
      this.lblUsd.Size = new System.Drawing.Size(67, 23);
      this.lblUsd.TabIndex = 0;
      this.lblUsd.Text = "Income";
      // 
      // numTaxesSource
      // 
      this.numTaxesSource.DecimalPlaces = 2;
      this.numTaxesSource.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.numTaxesSource.Location = new System.Drawing.Point(81, 3);
      this.numTaxesSource.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
      this.numTaxesSource.Name = "numTaxesSource";
      this.numTaxesSource.Size = new System.Drawing.Size(154, 32);
      this.numTaxesSource.TabIndex = 1;
      this.numTaxesSource.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.numTaxesSource.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numTaxesSource.ValueChanged += new System.EventHandler(this.numTaxesSource_ValueChanged);
      // 
      // lblTaxesResult
      // 
      this.lblTaxesResult.AutoSize = true;
      this.lblTaxesResult.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblTaxesResult.Location = new System.Drawing.Point(8, 123);
      this.lblTaxesResult.Name = "lblTaxesResult";
      this.lblTaxesResult.Size = new System.Drawing.Size(50, 23);
      this.lblTaxesResult.TabIndex = 6;
      this.lblTaxesResult.Text = "Taxes";
      // 
      // lblUah
      // 
      this.lblUah.AutoSize = true;
      this.lblUah.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblUah.Location = new System.Drawing.Point(8, 85);
      this.lblUah.Name = "lblUah";
      this.lblUah.Size = new System.Drawing.Size(45, 23);
      this.lblUah.TabIndex = 4;
      this.lblUah.Text = "UAH";
      // 
      // lblDate
      // 
      this.lblDate.AutoSize = true;
      this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblDate.Location = new System.Drawing.Point(8, 47);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new System.Drawing.Size(46, 23);
      this.lblDate.TabIndex = 2;
      this.lblDate.Text = "Date";
      // 
      // txtUahResult
      // 
      this.txtUahResult.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.txtUahResult.Location = new System.Drawing.Point(81, 79);
      this.txtUahResult.Name = "txtUahResult";
      this.txtUahResult.ReadOnly = true;
      this.txtUahResult.Size = new System.Drawing.Size(154, 32);
      this.txtUahResult.TabIndex = 5;
      this.txtUahResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtTaxesResult
      // 
      this.txtTaxesResult.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.txtTaxesResult.Location = new System.Drawing.Point(81, 117);
      this.txtTaxesResult.Name = "txtTaxesResult";
      this.txtTaxesResult.ReadOnly = true;
      this.txtTaxesResult.Size = new System.Drawing.Size(154, 32);
      this.txtTaxesResult.TabIndex = 7;
      this.txtTaxesResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // dtTaxesSource
      // 
      this.dtTaxesSource.CustomFormat = "dd-MM-yyyy";
      this.dtTaxesSource.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.dtTaxesSource.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtTaxesSource.Location = new System.Drawing.Point(81, 41);
      this.dtTaxesSource.Name = "dtTaxesSource";
      this.dtTaxesSource.Size = new System.Drawing.Size(154, 32);
      this.dtTaxesSource.TabIndex = 3;
      this.dtTaxesSource.ValueChanged += new System.EventHandler(this.numTaxesSource_ValueChanged);
      // 
      // panStatus
      // 
      this.panStatus.Controls.Add(this.cbxWeather);
      this.panStatus.Controls.Add(this.panGridOptions);
      this.panStatus.Controls.Add(this.cbxStickEdges);
      this.panStatus.Controls.Add(this.btnTop);
      this.panStatus.Controls.Add(this.btnRefresh);
      this.panStatus.Controls.Add(this.cmbCurrency);
      this.panStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panStatus.Location = new System.Drawing.Point(0, 235);
      this.panStatus.Name = "panStatus";
      this.panStatus.Size = new System.Drawing.Size(564, 26);
      this.panStatus.TabIndex = 20;
      // 
      // cbxWeather
      // 
      this.cbxWeather.AutoSize = true;
      this.cbxWeather.Dock = System.Windows.Forms.DockStyle.Left;
      this.cbxWeather.Location = new System.Drawing.Point(222, 0);
      this.cbxWeather.Name = "cbxWeather";
      this.cbxWeather.Size = new System.Drawing.Size(82, 26);
      this.cbxWeather.TabIndex = 4;
      this.cbxWeather.Text = "Weather";
      this.cbxWeather.UseVisualStyleBackColor = true;
      // 
      // panGridOptions
      // 
      this.panGridOptions.Controls.Add(this.cbxShowNbu);
      this.panGridOptions.Controls.Add(this.cmbChartMode);
      this.panGridOptions.Controls.Add(this.cbxChartGridMode);
      this.panGridOptions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panGridOptions.Location = new System.Drawing.Point(222, 0);
      this.panGridOptions.Name = "panGridOptions";
      this.panGridOptions.Size = new System.Drawing.Size(342, 26);
      this.panGridOptions.TabIndex = 5;
      this.panGridOptions.Visible = false;
      // 
      // cbxShowNbu
      // 
      this.cbxShowNbu.AutoSize = true;
      this.cbxShowNbu.Checked = true;
      this.cbxShowNbu.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowNbu.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxShowNbu.Location = new System.Drawing.Point(74, 0);
      this.cbxShowNbu.Name = "cbxShowNbu";
      this.cbxShowNbu.Size = new System.Drawing.Size(59, 26);
      this.cbxShowNbu.TabIndex = 0;
      this.cbxShowNbu.Text = "NBU";
      this.cbxShowNbu.UseVisualStyleBackColor = true;
      // 
      // cmbChartMode
      // 
      this.cmbChartMode.Dock = System.Windows.Forms.DockStyle.Right;
      this.cmbChartMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbChartMode.FormattingEnabled = true;
      this.cmbChartMode.Items.AddRange(new object[] {
            "1 month",
            "3 months",
            "6 months"});
      this.cmbChartMode.Location = new System.Drawing.Point(133, 0);
      this.cmbChartMode.Name = "cmbChartMode";
      this.cmbChartMode.Size = new System.Drawing.Size(121, 27);
      this.cmbChartMode.TabIndex = 1;
      // 
      // cbxChartGridMode
      // 
      this.cbxChartGridMode.AutoSize = true;
      this.cbxChartGridMode.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxChartGridMode.Location = new System.Drawing.Point(254, 0);
      this.cbxChartGridMode.Name = "cbxChartGridMode";
      this.cbxChartGridMode.Size = new System.Drawing.Size(88, 26);
      this.cbxChartGridMode.TabIndex = 2;
      this.cbxChartGridMode.Text = "Grid lines";
      this.cbxChartGridMode.UseVisualStyleBackColor = true;
      // 
      // cbxStickEdges
      // 
      this.cbxStickEdges.AutoSize = true;
      this.cbxStickEdges.Dock = System.Windows.Forms.DockStyle.Left;
      this.cbxStickEdges.Location = new System.Drawing.Point(123, 0);
      this.cbxStickEdges.Name = "cbxStickEdges";
      this.cbxStickEdges.Size = new System.Drawing.Size(99, 26);
      this.cbxStickEdges.TabIndex = 3;
      this.cbxStickEdges.Text = "Stick edges";
      this.cbxStickEdges.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.ClientSize = new System.Drawing.Size(564, 261);
      this.Controls.Add(this.panHistory);
      this.Controls.Add(this.panStatus);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(582, 300);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
      this.panMain.ResumeLayout(false);
      this.panHistory.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.tabPageMain.ResumeLayout(false);
      this.panPrivat24Business.ResumeLayout(false);
      this.tabPageGraphic.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
      this.tabTaxes.ResumeLayout(false);
      this.tabTaxes.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numTaxesSource)).EndInit();
      this.panStatus.ResumeLayout(false);
      this.panStatus.PerformLayout();
      this.panGridOptions.ResumeLayout(false);
      this.panGridOptions.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private Button btnRefresh;
    private Button btnTop;
    private ComboBox cmbCurrency;
    private WebBrowser browser;
    private Panel panMain;
    private ListView lstHistory;
    private ColumnHeader colBuy;
    private ColumnHeader colSale;
    private ColumnHeader colDate;
    private Panel panHistory;
    private ListView lstFinanceHistory;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    private Panel panStatus;
    private ComboBox cmbChartMode;
    private CheckBox cbxChartGridMode;
    private CheckBox cbxStickEdges;
    private CheckBox cbxShowNbu;
    private TabControl tabControl;
    private TabPage tabPageGraphic;
    private TabPage tabPageMain;
    private Panel panGridOptions;
    private GroupBox panPrivat24Business;
    private DateTimePicker dtTaxesSource;
    private NumericUpDown numTaxesSource;
    private TextBox txtTaxesResult;
    private Label lblUah;
    private Label lblDate;
    private Label lblUsd;
    private Label lblTaxesResult;
    private TextBox txtUahResult;
    private CheckBox cbxWeather;
    private TabPage tabTaxes;
  }
}