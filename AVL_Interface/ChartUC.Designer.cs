namespace AVL_Interface
{
    partial class ChartUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartUC));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.trefftzPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.totalCLVsAoAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.primaryXAxisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primXNoneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.primaryYAxisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primYNoneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.secondaryYAxisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secYNoneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.secondaryXAxisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secXNoneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subChart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.subChart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.runMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showRawDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.rawTotalForcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawStripForcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawStabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteRunCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
            this.chartMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subChart2)).BeginInit();
            this.runMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 274F));
            this.tableLayoutPanel1.Controls.Add(this.mainChart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.subChart1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.subChart2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.44589F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.77056F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1016, 462);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainChart
            // 
            chartArea4.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea4.AxisX.Title = "Normalized Spanwise Location";
            chartArea4.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea4.AxisY.Title = "Section Lift Coefficient";
            chartArea4.InnerPlotPosition.Auto = false;
            chartArea4.InnerPlotPosition.Height = 88F;
            chartArea4.InnerPlotPosition.Width = 93F;
            chartArea4.InnerPlotPosition.X = 7F;
            chartArea4.InnerPlotPosition.Y = 1F;
            chartArea4.Name = "ChartArea1";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 92F;
            chartArea4.Position.Width = 94F;
            chartArea4.Position.X = 5F;
            chartArea4.Position.Y = 3F;
            this.mainChart.ChartAreas.Add(chartArea4);
            this.mainChart.ContextMenuStrip = this.chartMenuStrip;
            this.mainChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.BackColor = System.Drawing.Color.Transparent;
            legend4.DockedToChartArea = "ChartArea1";
            legend4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend4.Name = "Legend1";
            this.mainChart.Legends.Add(legend4);
            this.mainChart.Location = new System.Drawing.Point(189, 3);
            this.mainChart.Name = "mainChart";
            this.tableLayoutPanel1.SetRowSpan(this.mainChart, 3);
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            dataPoint2.IsEmpty = true;
            series4.Points.Add(dataPoint2);
            this.mainChart.Series.Add(series4);
            this.mainChart.Size = new System.Drawing.Size(550, 456);
            this.mainChart.TabIndex = 0;
            this.mainChart.Text = "chart1";
            // 
            // chartMenuStrip
            // 
            this.chartMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripSeparator2,
            this.primaryXAxisMenuItem,
            this.primaryYAxisMenuItem,
            this.secondaryYAxisMenuItem,
            this.secondaryXAxisMenuItem,
            this.toolStripSeparator1,
            this.exportDataToolStripMenuItem,
            this.saveAsImageToolStripMenuItem});
            this.chartMenuStrip.Name = "contextMenuStrip1";
            this.chartMenuStrip.Size = new System.Drawing.Size(166, 170);
            this.chartMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trefftzPlotToolStripMenuItem,
            this.totalCLVsAoAToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItem3.Text = "Common Graphs";
            // 
            // trefftzPlotToolStripMenuItem
            // 
            this.trefftzPlotToolStripMenuItem.Name = "trefftzPlotToolStripMenuItem";
            this.trefftzPlotToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.trefftzPlotToolStripMenuItem.Text = "Trefftz Plot";
            this.trefftzPlotToolStripMenuItem.Click += new System.EventHandler(this.trefftzPlotToolStripMenuItem_Click);
            // 
            // totalCLVsAoAToolStripMenuItem
            // 
            this.totalCLVsAoAToolStripMenuItem.Name = "totalCLVsAoAToolStripMenuItem";
            this.totalCLVsAoAToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.totalCLVsAoAToolStripMenuItem.Text = "Total CL vs AoA";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(162, 6);
            // 
            // primaryXAxisMenuItem
            // 
            this.primaryXAxisMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.primXNoneItem,
            this.toolStripSeparator3});
            this.primaryXAxisMenuItem.Name = "primaryXAxisMenuItem";
            this.primaryXAxisMenuItem.Size = new System.Drawing.Size(165, 22);
            this.primaryXAxisMenuItem.Text = "Primary X-Axis";
            // 
            // primXNoneItem
            // 
            this.primXNoneItem.Name = "primXNoneItem";
            this.primXNoneItem.Size = new System.Drawing.Size(103, 22);
            this.primXNoneItem.Text = "None";
            this.primXNoneItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(100, 6);
            // 
            // primaryYAxisMenuItem
            // 
            this.primaryYAxisMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.primYNoneItem,
            this.toolStripSeparator4});
            this.primaryYAxisMenuItem.Name = "primaryYAxisMenuItem";
            this.primaryYAxisMenuItem.Size = new System.Drawing.Size(165, 22);
            this.primaryYAxisMenuItem.Text = "Primary Y-Axis";
            // 
            // primYNoneItem
            // 
            this.primYNoneItem.Name = "primYNoneItem";
            this.primYNoneItem.Size = new System.Drawing.Size(103, 22);
            this.primYNoneItem.Text = "None";
            this.primYNoneItem.Click += new System.EventHandler(this.primYNoneItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(100, 6);
            // 
            // secondaryYAxisMenuItem
            // 
            this.secondaryYAxisMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secYNoneItem,
            this.toolStripSeparator5});
            this.secondaryYAxisMenuItem.Name = "secondaryYAxisMenuItem";
            this.secondaryYAxisMenuItem.Size = new System.Drawing.Size(165, 22);
            this.secondaryYAxisMenuItem.Text = "Secondary Y-Axis";
            // 
            // secYNoneItem
            // 
            this.secYNoneItem.Name = "secYNoneItem";
            this.secYNoneItem.Size = new System.Drawing.Size(103, 22);
            this.secYNoneItem.Text = "None";
            this.secYNoneItem.Click += new System.EventHandler(this.secYNoneItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(100, 6);
            // 
            // secondaryXAxisMenuItem
            // 
            this.secondaryXAxisMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.secXNoneItem,
            this.toolStripSeparator6});
            this.secondaryXAxisMenuItem.Name = "secondaryXAxisMenuItem";
            this.secondaryXAxisMenuItem.Size = new System.Drawing.Size(165, 22);
            this.secondaryXAxisMenuItem.Text = "Secondary X-Axis";
            // 
            // secXNoneItem
            // 
            this.secXNoneItem.Name = "secXNoneItem";
            this.secXNoneItem.Size = new System.Drawing.Size(103, 22);
            this.secXNoneItem.Text = "None";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(100, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportDataToolStripMenuItem.Image")));
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            // 
            // saveAsImageToolStripMenuItem
            // 
            this.saveAsImageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsImageToolStripMenuItem.Image")));
            this.saveAsImageToolStripMenuItem.Name = "saveAsImageToolStripMenuItem";
            this.saveAsImageToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveAsImageToolStripMenuItem.Text = "Save as Image";
            this.saveAsImageToolStripMenuItem.Click += new System.EventHandler(this.saveAsImageToolStripMenuItem_Click);
            // 
            // subChart1
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.MajorTickMark.Interval = 0D;
            chartArea1.AxisX.Title = "Angle of Attack";
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.Title = "Total cL";
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY2.Title = "Elevator Deflection";
            chartArea1.Name = "ChartArea1";
            this.subChart1.ChartAreas.Add(chartArea1);
            this.subChart1.ContextMenuStrip = this.chartMenuStrip;
            this.subChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            this.subChart1.Legends.Add(legend1);
            this.subChart1.Location = new System.Drawing.Point(745, 3);
            this.subChart1.Name = "subChart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.subChart1.Series.Add(series1);
            this.subChart1.Size = new System.Drawing.Size(268, 224);
            this.subChart1.TabIndex = 1;
            this.subChart1.Text = "chart2";
            // 
            // subChart2
            // 
            chartArea2.AxisX.Title = "Angle of Attack";
            chartArea2.Name = "ChartArea1";
            this.subChart2.ChartAreas.Add(chartArea2);
            this.subChart2.ContextMenuStrip = this.chartMenuStrip;
            this.subChart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.BackColor = System.Drawing.Color.Transparent;
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Name = "Legend1";
            this.subChart2.Legends.Add(legend2);
            this.subChart2.Location = new System.Drawing.Point(745, 233);
            this.subChart2.Name = "subChart2";
            this.tableLayoutPanel1.SetRowSpan(this.subChart2, 2);
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.subChart2.Series.Add(series2);
            this.subChart2.Size = new System.Drawing.Size(268, 226);
            this.subChart2.TabIndex = 2;
            this.subChart2.Text = "chart3";
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.tableLayoutPanel1.SetRowSpan(this.listView1, 2);
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(186, 402);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Analysis Results";
            this.columnHeader1.Width = 163;
            // 
            // runMenuStrip
            // 
            this.runMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showRawDataToolStripMenuItem,
            this.toolStripSeparator7,
            this.rawTotalForcesToolStripMenuItem,
            this.rawStripForcesToolStripMenuItem,
            this.rawStabilityToolStripMenuItem,
            this.toolStripSeparator8,
            this.deleteRunCaseToolStripMenuItem});
            this.runMenuStrip.Name = "runMenuStrip";
            this.runMenuStrip.Size = new System.Drawing.Size(173, 126);
            // 
            // showRawDataToolStripMenuItem
            // 
            this.showRawDataToolStripMenuItem.Name = "showRawDataToolStripMenuItem";
            this.showRawDataToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showRawDataToolStripMenuItem.Text = "Show All Raw Data";
            this.showRawDataToolStripMenuItem.Click += new System.EventHandler(this.showRawDataToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(169, 6);
            // 
            // rawTotalForcesToolStripMenuItem
            // 
            this.rawTotalForcesToolStripMenuItem.Name = "rawTotalForcesToolStripMenuItem";
            this.rawTotalForcesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.rawTotalForcesToolStripMenuItem.Text = "Raw Total Forces";
            this.rawTotalForcesToolStripMenuItem.Click += new System.EventHandler(this.rawTotalForcesToolStripMenuItem_Click);
            // 
            // rawStripForcesToolStripMenuItem
            // 
            this.rawStripForcesToolStripMenuItem.Name = "rawStripForcesToolStripMenuItem";
            this.rawStripForcesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.rawStripForcesToolStripMenuItem.Text = "Raw Strip Forces";
            this.rawStripForcesToolStripMenuItem.Click += new System.EventHandler(this.rawStripForcesToolStripMenuItem_Click);
            // 
            // rawStabilityToolStripMenuItem
            // 
            this.rawStabilityToolStripMenuItem.Name = "rawStabilityToolStripMenuItem";
            this.rawStabilityToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.rawStabilityToolStripMenuItem.Text = "Raw Stability";
            this.rawStabilityToolStripMenuItem.Click += new System.EventHandler(this.rawStabilityToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(169, 6);
            // 
            // deleteRunCaseToolStripMenuItem
            // 
            this.deleteRunCaseToolStripMenuItem.Name = "deleteRunCaseToolStripMenuItem";
            this.deleteRunCaseToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.deleteRunCaseToolStripMenuItem.Text = "Delete Run Case";
            this.deleteRunCaseToolStripMenuItem.Click += new System.EventHandler(this.deleteRunCaseToolStripMenuItem_Click);
            // 
            // ChartUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ChartUC";
            this.Size = new System.Drawing.Size(1016, 462);
            this.Load += new System.EventHandler(this.ChartUC_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
            this.chartMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.subChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subChart2)).EndInit();
            this.runMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart subChart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart subChart2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip chartMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem primaryXAxisMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primaryYAxisMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondaryYAxisMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondaryXAxisMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem trefftzPlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem totalCLVsAoAToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem primXNoneItem;
        private System.Windows.Forms.ToolStripMenuItem primYNoneItem;
        private System.Windows.Forms.ToolStripMenuItem secYNoneItem;
        private System.Windows.Forms.ToolStripMenuItem secXNoneItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ContextMenuStrip runMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showRawDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem rawTotalForcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawStripForcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawStabilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem deleteRunCaseToolStripMenuItem;
    }
}
