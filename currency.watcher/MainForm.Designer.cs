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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.btnRefresh = new System.Windows.Forms.Button();
      this.lstFinanceHistory = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ratesSplitter = new System.Windows.Forms.Splitter();
      this.lstRates = new System.Windows.Forms.ListView();
      this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderUsd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderEur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.lblUsd = new System.Windows.Forms.Label();
      this.numTaxesSource = new System.Windows.Forms.NumericUpDown();
      this.lblTaxesResult = new System.Windows.Forms.Label();
      this.lblUah = new System.Windows.Forms.Label();
      this.lblDate = new System.Windows.Forms.Label();
      this.txtUahResult = new System.Windows.Forms.TextBox();
      this.txtTaxesResult = new System.Windows.Forms.TextBox();
      this.dtTaxesSource = new System.Windows.Forms.DateTimePicker();
      this.panStatus = new System.Windows.Forms.Panel();
      this.cbxTaxes = new System.Windows.Forms.CheckBox();
      this.cbxShowNbu = new System.Windows.Forms.CheckBox();
      this.cmbChartMode = new System.Windows.Forms.ComboBox();
      this.cbxChartGridMode = new System.Windows.Forms.CheckBox();
      this.gridSplitter = new System.Windows.Forms.Splitter();
      this.panTaxes = new System.Windows.Forms.Panel();
      this.panRates = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numTaxesSource)).BeginInit();
      this.panStatus.SuspendLayout();
      this.panTaxes.SuspendLayout();
      this.panRates.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnRefresh
      // 
      this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
      this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnRefresh.Location = new System.Drawing.Point(0, 0);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(25, 30);
      this.btnRefresh.TabIndex = 1;
      this.btnRefresh.UseVisualStyleBackColor = true;
      // 
      // lstFinanceHistory
      // 
      this.lstFinanceHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.lstFinanceHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
      this.lstFinanceHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lstFinanceHistory.FullRowSelect = true;
      this.lstFinanceHistory.GridLines = true;
      this.lstFinanceHistory.HideSelection = false;
      this.lstFinanceHistory.Location = new System.Drawing.Point(0, 241);
      this.lstFinanceHistory.Name = "lstFinanceHistory";
      this.lstFinanceHistory.Size = new System.Drawing.Size(333, 201);
      this.lstFinanceHistory.TabIndex = 1;
      this.lstFinanceHistory.UseCompatibleStateImageBehavior = false;
      this.lstFinanceHistory.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Time";
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Usd/B";
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Usd/S";
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Eur/B";
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Eur/S";
      // 
      // ratesSplitter
      // 
      this.ratesSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.ratesSplitter.Location = new System.Drawing.Point(0, 238);
      this.ratesSplitter.MinSize = 0;
      this.ratesSplitter.Name = "ratesSplitter";
      this.ratesSplitter.Size = new System.Drawing.Size(333, 3);
      this.ratesSplitter.TabIndex = 1;
      this.ratesSplitter.TabStop = false;
      // 
      // lstRates
      // 
      this.lstRates.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.lstRates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDate,
            this.columnHeaderUsd,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeaderEur,
            this.columnHeader8,
            this.columnHeader9});
      this.lstRates.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstRates.FullRowSelect = true;
      this.lstRates.GridLines = true;
      this.lstRates.HideSelection = false;
      this.lstRates.Location = new System.Drawing.Point(0, 0);
      this.lstRates.Name = "lstRates";
      this.lstRates.Size = new System.Drawing.Size(333, 238);
      this.lstRates.TabIndex = 2;
      this.lstRates.UseCompatibleStateImageBehavior = false;
      this.lstRates.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderDate
      // 
      this.columnHeaderDate.Text = "Day";
      // 
      // columnHeaderUsd
      // 
      this.columnHeaderUsd.Text = "Usd";
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Pb/B";
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Pb/S";
      // 
      // columnHeaderEur
      // 
      this.columnHeaderEur.Text = "Eur";
      // 
      // columnHeader8
      // 
      this.columnHeader8.Text = "Pb/B";
      // 
      // columnHeader9
      // 
      this.columnHeader9.Text = "Pb/S";
      // 
      // chart
      // 
      this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.chart.Location = new System.Drawing.Point(336, 0);
      this.chart.Name = "chart";
      this.chart.Size = new System.Drawing.Size(1050, 607);
      this.chart.TabIndex = 20;
      this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
      // 
      // lblUsd
      // 
      this.lblUsd.AutoSize = true;
      this.lblUsd.Location = new System.Drawing.Point(7, 9);
      this.lblUsd.Name = "lblUsd";
      this.lblUsd.Size = new System.Drawing.Size(76, 28);
      this.lblUsd.TabIndex = 0;
      this.lblUsd.Text = "Income";
      // 
      // numTaxesSource
      // 
      this.numTaxesSource.DecimalPlaces = 2;
      this.numTaxesSource.Location = new System.Drawing.Point(120, 3);
      this.numTaxesSource.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
      this.numTaxesSource.Name = "numTaxesSource";
      this.numTaxesSource.Size = new System.Drawing.Size(154, 34);
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
      this.lblTaxesResult.Location = new System.Drawing.Point(7, 126);
      this.lblTaxesResult.Name = "lblTaxesResult";
      this.lblTaxesResult.Size = new System.Drawing.Size(57, 28);
      this.lblTaxesResult.TabIndex = 6;
      this.lblTaxesResult.Text = "Taxes";
      // 
      // lblUah
      // 
      this.lblUah.AutoSize = true;
      this.lblUah.Location = new System.Drawing.Point(7, 86);
      this.lblUah.Name = "lblUah";
      this.lblUah.Size = new System.Drawing.Size(53, 28);
      this.lblUah.TabIndex = 4;
      this.lblUah.Text = "UAH";
      // 
      // lblDate
      // 
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new System.Drawing.Point(7, 48);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new System.Drawing.Size(53, 28);
      this.lblDate.TabIndex = 2;
      this.lblDate.Text = "Date";
      // 
      // txtUahResult
      // 
      this.txtUahResult.Location = new System.Drawing.Point(120, 83);
      this.txtUahResult.Name = "txtUahResult";
      this.txtUahResult.ReadOnly = true;
      this.txtUahResult.Size = new System.Drawing.Size(154, 34);
      this.txtUahResult.TabIndex = 5;
      this.txtUahResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtTaxesResult
      // 
      this.txtTaxesResult.Location = new System.Drawing.Point(120, 123);
      this.txtTaxesResult.Name = "txtTaxesResult";
      this.txtTaxesResult.ReadOnly = true;
      this.txtTaxesResult.Size = new System.Drawing.Size(154, 34);
      this.txtTaxesResult.TabIndex = 7;
      this.txtTaxesResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // dtTaxesSource
      // 
      this.dtTaxesSource.CustomFormat = "dd-MM-yyyy";
      this.dtTaxesSource.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtTaxesSource.Location = new System.Drawing.Point(120, 43);
      this.dtTaxesSource.Name = "dtTaxesSource";
      this.dtTaxesSource.Size = new System.Drawing.Size(154, 34);
      this.dtTaxesSource.TabIndex = 3;
      this.dtTaxesSource.ValueChanged += new System.EventHandler(this.numTaxesSource_ValueChanged);
      // 
      // panStatus
      // 
      this.panStatus.Controls.Add(this.cbxTaxes);
      this.panStatus.Controls.Add(this.cbxShowNbu);
      this.panStatus.Controls.Add(this.cmbChartMode);
      this.panStatus.Controls.Add(this.cbxChartGridMode);
      this.panStatus.Controls.Add(this.btnRefresh);
      this.panStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panStatus.Location = new System.Drawing.Point(0, 607);
      this.panStatus.Name = "panStatus";
      this.panStatus.Size = new System.Drawing.Size(1386, 30);
      this.panStatus.TabIndex = 20;
      // 
      // cbxTaxes
      // 
      this.cbxTaxes.AutoSize = true;
      this.cbxTaxes.Dock = System.Windows.Forms.DockStyle.Left;
      this.cbxTaxes.Location = new System.Drawing.Point(25, 0);
      this.cbxTaxes.Name = "cbxTaxes";
      this.cbxTaxes.Size = new System.Drawing.Size(83, 30);
      this.cbxTaxes.TabIndex = 3;
      this.cbxTaxes.Text = "Taxes";
      this.cbxTaxes.UseVisualStyleBackColor = true;
      this.cbxTaxes.CheckedChanged += new System.EventHandler(this.cbxTaxes_CheckedChanged);
      // 
      // cbxShowNbu
      // 
      this.cbxShowNbu.AutoSize = true;
      this.cbxShowNbu.Checked = true;
      this.cbxShowNbu.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowNbu.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxShowNbu.Location = new System.Drawing.Point(1067, 0);
      this.cbxShowNbu.Name = "cbxShowNbu";
      this.cbxShowNbu.Size = new System.Drawing.Size(78, 30);
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
            "1 week",
            "1 month",
            "3 months",
            "6 months"});
      this.cmbChartMode.Location = new System.Drawing.Point(1145, 0);
      this.cmbChartMode.Name = "cmbChartMode";
      this.cmbChartMode.Size = new System.Drawing.Size(121, 36);
      this.cmbChartMode.TabIndex = 1;
      // 
      // cbxChartGridMode
      // 
      this.cbxChartGridMode.AutoSize = true;
      this.cbxChartGridMode.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxChartGridMode.Location = new System.Drawing.Point(1266, 0);
      this.cbxChartGridMode.Name = "cbxChartGridMode";
      this.cbxChartGridMode.Size = new System.Drawing.Size(120, 30);
      this.cbxChartGridMode.TabIndex = 2;
      this.cbxChartGridMode.Text = "Axis";
      this.cbxChartGridMode.UseVisualStyleBackColor = true;
      // 
      // gridSplitter
      // 
      this.gridSplitter.Location = new System.Drawing.Point(333, 0);
      this.gridSplitter.Name = "gridSplitter";
      this.gridSplitter.Size = new System.Drawing.Size(3, 607);
      this.gridSplitter.TabIndex = 21;
      this.gridSplitter.TabStop = false;
      // 
      // panTaxes
      // 
      this.panTaxes.Controls.Add(this.lblUsd);
      this.panTaxes.Controls.Add(this.numTaxesSource);
      this.panTaxes.Controls.Add(this.lblTaxesResult);
      this.panTaxes.Controls.Add(this.lblUah);
      this.panTaxes.Controls.Add(this.lblDate);
      this.panTaxes.Controls.Add(this.txtUahResult);
      this.panTaxes.Controls.Add(this.txtTaxesResult);
      this.panTaxes.Controls.Add(this.dtTaxesSource);
      this.panTaxes.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panTaxes.Location = new System.Drawing.Point(0, 442);
      this.panTaxes.Name = "panTaxes";
      this.panTaxes.Size = new System.Drawing.Size(333, 165);
      this.panTaxes.TabIndex = 22;
      this.panTaxes.Visible = false;
      // 
      // panRates
      // 
      this.panRates.Controls.Add(this.lstRates);
      this.panRates.Controls.Add(this.ratesSplitter);
      this.panRates.Controls.Add(this.lstFinanceHistory);
      this.panRates.Controls.Add(this.panTaxes);
      this.panRates.Dock = System.Windows.Forms.DockStyle.Left;
      this.panRates.Location = new System.Drawing.Point(0, 0);
      this.panRates.Name = "panRates";
      this.panRates.Size = new System.Drawing.Size(333, 607);
      this.panRates.TabIndex = 23;
      // 
      // MainForm
      // 
      this.ClientSize = new System.Drawing.Size(1386, 637);
      this.Controls.Add(this.chart);
      this.Controls.Add(this.gridSplitter);
      this.Controls.Add(this.panRates);
      this.Controls.Add(this.panStatus);
      this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
      ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numTaxesSource)).EndInit();
      this.panStatus.ResumeLayout(false);
      this.panStatus.PerformLayout();
      this.panTaxes.ResumeLayout(false);
      this.panTaxes.PerformLayout();
      this.panRates.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private Button btnRefresh;
    private ListView lstFinanceHistory;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    private Panel panStatus;
    private ComboBox cmbChartMode;
    private CheckBox cbxChartGridMode;
    private CheckBox cbxShowNbu;
    private DateTimePicker dtTaxesSource;
    private NumericUpDown numTaxesSource;
    private TextBox txtTaxesResult;
    private Label lblUah;
    private Label lblDate;
    private Label lblUsd;
    private Label lblTaxesResult;
    private TextBox txtUahResult;
    private Splitter ratesSplitter;
    private ListView lstRates;
    private ColumnHeader columnHeaderDate;
    private ColumnHeader columnHeaderUsd;
    private ColumnHeader columnHeaderEur;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ColumnHeader columnHeader9;
    private Splitter gridSplitter;
    private Panel panTaxes;
    private Panel panRates;
    private CheckBox cbxTaxes;
  }
}