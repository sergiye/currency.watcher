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
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageMain = new System.Windows.Forms.TabPage();
      this.splitter1 = new System.Windows.Forms.Splitter();
      this.lstRates = new System.Windows.Forms.ListView();
      this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderUsd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderEur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
      this.panGridOptions = new System.Windows.Forms.Panel();
      this.cbxShowNbu = new System.Windows.Forms.CheckBox();
      this.cmbChartMode = new System.Windows.Forms.ComboBox();
      this.cbxChartGridMode = new System.Windows.Forms.CheckBox();
      this.cbxStickEdges = new System.Windows.Forms.CheckBox();
      this.tabControl.SuspendLayout();
      this.tabPageMain.SuspendLayout();
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
      this.lstFinanceHistory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstFinanceHistory.FullRowSelect = true;
      this.lstFinanceHistory.GridLines = true;
      this.lstFinanceHistory.HideSelection = false;
      this.lstFinanceHistory.Location = new System.Drawing.Point(502, 0);
      this.lstFinanceHistory.Name = "lstFinanceHistory";
      this.lstFinanceHistory.Size = new System.Drawing.Size(267, 201);
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
      // tabControl
      // 
      this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this.tabControl.Controls.Add(this.tabPageMain);
      this.tabControl.Controls.Add(this.tabPageGraphic);
      this.tabControl.Controls.Add(this.tabTaxes);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(777, 231);
      this.tabControl.TabIndex = 0;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      // 
      // tabPageMain
      // 
      this.tabPageMain.Controls.Add(this.lstFinanceHistory);
      this.tabPageMain.Controls.Add(this.splitter1);
      this.tabPageMain.Controls.Add(this.lstRates);
      this.tabPageMain.Location = new System.Drawing.Point(4, 4);
      this.tabPageMain.Name = "tabPageMain";
      this.tabPageMain.Size = new System.Drawing.Size(769, 201);
      this.tabPageMain.TabIndex = 1;
      this.tabPageMain.Text = "Rates";
      this.tabPageMain.UseVisualStyleBackColor = true;
      // 
      // splitter1
      // 
      this.splitter1.Location = new System.Drawing.Point(499, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new System.Drawing.Size(3, 201);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
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
      this.lstRates.Dock = System.Windows.Forms.DockStyle.Left;
      this.lstRates.FullRowSelect = true;
      this.lstRates.GridLines = true;
      this.lstRates.HideSelection = false;
      this.lstRates.Location = new System.Drawing.Point(0, 0);
      this.lstRates.Name = "lstRates";
      this.lstRates.Size = new System.Drawing.Size(499, 201);
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
      // tabPageGraphic
      // 
      this.tabPageGraphic.Controls.Add(this.chart);
      this.tabPageGraphic.Location = new System.Drawing.Point(4, 4);
      this.tabPageGraphic.Name = "tabPageGraphic";
      this.tabPageGraphic.Size = new System.Drawing.Size(769, 205);
      this.tabPageGraphic.TabIndex = 0;
      this.tabPageGraphic.Text = "Graphic";
      this.tabPageGraphic.UseVisualStyleBackColor = true;
      // 
      // chart
      // 
      this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.chart.Location = new System.Drawing.Point(0, 0);
      this.chart.Name = "chart";
      this.chart.Size = new System.Drawing.Size(769, 205);
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
      this.tabTaxes.Location = new System.Drawing.Point(4, 4);
      this.tabTaxes.Name = "tabTaxes";
      this.tabTaxes.Size = new System.Drawing.Size(769, 205);
      this.tabTaxes.TabIndex = 2;
      this.tabTaxes.Text = "Taxes calculator";
      this.tabTaxes.UseVisualStyleBackColor = true;
      // 
      // lblUsd
      // 
      this.lblUsd.AutoSize = true;
      this.lblUsd.Location = new System.Drawing.Point(5, 15);
      this.lblUsd.Name = "lblUsd";
      this.lblUsd.Size = new System.Drawing.Size(54, 19);
      this.lblUsd.TabIndex = 0;
      this.lblUsd.Text = "Income";
      // 
      // numTaxesSource
      // 
      this.numTaxesSource.DecimalPlaces = 2;
      this.numTaxesSource.Location = new System.Drawing.Point(81, 12);
      this.numTaxesSource.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
      this.numTaxesSource.Name = "numTaxesSource";
      this.numTaxesSource.Size = new System.Drawing.Size(154, 25);
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
      this.lblTaxesResult.Location = new System.Drawing.Point(5, 108);
      this.lblTaxesResult.Name = "lblTaxesResult";
      this.lblTaxesResult.Size = new System.Drawing.Size(40, 19);
      this.lblTaxesResult.TabIndex = 6;
      this.lblTaxesResult.Text = "Taxes";
      // 
      // lblUah
      // 
      this.lblUah.AutoSize = true;
      this.lblUah.Location = new System.Drawing.Point(5, 77);
      this.lblUah.Name = "lblUah";
      this.lblUah.Size = new System.Drawing.Size(38, 19);
      this.lblUah.TabIndex = 4;
      this.lblUah.Text = "UAH";
      // 
      // lblDate
      // 
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new System.Drawing.Point(5, 46);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new System.Drawing.Size(38, 19);
      this.lblDate.TabIndex = 2;
      this.lblDate.Text = "Date";
      // 
      // txtUahResult
      // 
      this.txtUahResult.Location = new System.Drawing.Point(81, 74);
      this.txtUahResult.Name = "txtUahResult";
      this.txtUahResult.ReadOnly = true;
      this.txtUahResult.Size = new System.Drawing.Size(154, 25);
      this.txtUahResult.TabIndex = 5;
      this.txtUahResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtTaxesResult
      // 
      this.txtTaxesResult.Location = new System.Drawing.Point(81, 105);
      this.txtTaxesResult.Name = "txtTaxesResult";
      this.txtTaxesResult.ReadOnly = true;
      this.txtTaxesResult.Size = new System.Drawing.Size(154, 25);
      this.txtTaxesResult.TabIndex = 7;
      this.txtTaxesResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // dtTaxesSource
      // 
      this.dtTaxesSource.CustomFormat = "dd-MM-yyyy";
      this.dtTaxesSource.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtTaxesSource.Location = new System.Drawing.Point(81, 43);
      this.dtTaxesSource.Name = "dtTaxesSource";
      this.dtTaxesSource.Size = new System.Drawing.Size(154, 25);
      this.dtTaxesSource.TabIndex = 3;
      this.dtTaxesSource.ValueChanged += new System.EventHandler(this.numTaxesSource_ValueChanged);
      // 
      // panStatus
      // 
      this.panStatus.Controls.Add(this.panGridOptions);
      this.panStatus.Controls.Add(this.cbxStickEdges);
      this.panStatus.Controls.Add(this.btnRefresh);
      this.panStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panStatus.Location = new System.Drawing.Point(0, 231);
      this.panStatus.Name = "panStatus";
      this.panStatus.Size = new System.Drawing.Size(777, 30);
      this.panStatus.TabIndex = 20;
      // 
      // panGridOptions
      // 
      this.panGridOptions.Controls.Add(this.cbxShowNbu);
      this.panGridOptions.Controls.Add(this.cmbChartMode);
      this.panGridOptions.Controls.Add(this.cbxChartGridMode);
      this.panGridOptions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panGridOptions.Location = new System.Drawing.Point(121, 0);
      this.panGridOptions.Name = "panGridOptions";
      this.panGridOptions.Size = new System.Drawing.Size(656, 30);
      this.panGridOptions.TabIndex = 5;
      this.panGridOptions.Visible = false;
      // 
      // cbxShowNbu
      // 
      this.cbxShowNbu.AutoSize = true;
      this.cbxShowNbu.Checked = true;
      this.cbxShowNbu.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowNbu.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxShowNbu.Location = new System.Drawing.Point(394, 0);
      this.cbxShowNbu.Name = "cbxShowNbu";
      this.cbxShowNbu.Size = new System.Drawing.Size(56, 30);
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
            "3 months"});
      this.cmbChartMode.Location = new System.Drawing.Point(450, 0);
      this.cmbChartMode.Name = "cmbChartMode";
      this.cmbChartMode.Size = new System.Drawing.Size(121, 25);
      this.cmbChartMode.TabIndex = 1;
      // 
      // cbxChartGridMode
      // 
      this.cbxChartGridMode.AutoSize = true;
      this.cbxChartGridMode.Dock = System.Windows.Forms.DockStyle.Right;
      this.cbxChartGridMode.Location = new System.Drawing.Point(571, 0);
      this.cbxChartGridMode.Name = "cbxChartGridMode";
      this.cbxChartGridMode.Size = new System.Drawing.Size(85, 30);
      this.cbxChartGridMode.TabIndex = 2;
      this.cbxChartGridMode.Text = "Grid lines";
      this.cbxChartGridMode.UseVisualStyleBackColor = true;
      // 
      // cbxStickEdges
      // 
      this.cbxStickEdges.AutoSize = true;
      this.cbxStickEdges.Dock = System.Windows.Forms.DockStyle.Left;
      this.cbxStickEdges.Location = new System.Drawing.Point(25, 0);
      this.cbxStickEdges.Name = "cbxStickEdges";
      this.cbxStickEdges.Size = new System.Drawing.Size(96, 30);
      this.cbxStickEdges.TabIndex = 3;
      this.cbxStickEdges.Text = "Stick edges";
      this.cbxStickEdges.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.ClientSize = new System.Drawing.Size(777, 261);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.panStatus);
      this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(400, 300);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
      this.tabControl.ResumeLayout(false);
      this.tabPageMain.ResumeLayout(false);
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
    private DateTimePicker dtTaxesSource;
    private NumericUpDown numTaxesSource;
    private TextBox txtTaxesResult;
    private Label lblUah;
    private Label lblDate;
    private Label lblUsd;
    private Label lblTaxesResult;
    private TextBox txtUahResult;
    private TabPage tabTaxes;
    private Splitter splitter1;
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
  }
}