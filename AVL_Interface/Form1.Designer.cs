namespace AVL_Interface
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ribbon = new System.Windows.Forms.Ribbon();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.newFileButton = new System.Windows.Forms.RibbonButton();
            this.openFileButton = new System.Windows.Forms.RibbonButton();
            this.saveAllButton = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.avlLocButton = new System.Windows.Forms.RibbonButton();
            this.ribbonCheckBox1 = new System.Windows.Forms.RibbonCheckBox();
            this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
            this.constraintWindowButton = new System.Windows.Forms.RibbonButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.aircraft_Info1 = new AVL_Interface.Aircraft_Info();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chartUC1 = new AVL_Interface.ChartUC();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ribbon, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 682);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ribbon
            // 
            this.ribbon.CaptionBarVisible = false;
            this.tableLayoutPanel1.SetColumnSpan(this.ribbon, 2);
            this.ribbon.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon.Location = new System.Drawing.Point(3, 3);
            this.ribbon.Minimized = false;
            this.ribbon.Name = "ribbon";
            // 
            // 
            // 
            this.ribbon.OrbDropDown.BorderRoundness = 8;
            this.ribbon.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon.OrbDropDown.Name = "";
            this.ribbon.OrbDropDown.Size = new System.Drawing.Size(527, 447);
            this.ribbon.OrbDropDown.TabIndex = 0;
            this.ribbon.OrbImage = null;
            this.ribbon.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.ribbon.OrbVisible = false;
            this.ribbon.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon.Size = new System.Drawing.Size(1258, 120);
            this.ribbon.TabIndex = 2;
            this.ribbon.Tabs.Add(this.ribbonTab1);
            this.ribbon.TabsMargin = new System.Windows.Forms.Padding(12, 2, 20, 0);
            this.ribbon.Text = "ribbon2";
            this.ribbon.ThemeColor = System.Windows.Forms.RibbonTheme.Black;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Panels.Add(this.ribbonPanel2);
            this.ribbonTab1.Panels.Add(this.ribbonPanel3);
            this.ribbonTab1.Text = "PROGRAM SETTINGS";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.ButtonMoreVisible = false;
            this.ribbonPanel1.Items.Add(this.newFileButton);
            this.ribbonPanel1.Items.Add(this.openFileButton);
            this.ribbonPanel1.Items.Add(this.saveAllButton);
            this.ribbonPanel1.Text = "File Options";
            // 
            // newFileButton
            // 
            this.newFileButton.Image = ((System.Drawing.Image)(resources.GetObject("newFileButton.Image")));
            this.newFileButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("newFileButton.SmallImage")));
            this.newFileButton.Text = "New File";
            this.newFileButton.Click += new System.EventHandler(this.newFileButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Image = ((System.Drawing.Image)(resources.GetObject("openFileButton.Image")));
            this.openFileButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("openFileButton.SmallImage")));
            this.openFileButton.Text = "Open";
            this.openFileButton.ToolTip = "Open an existing *.AVL file";
            this.openFileButton.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAllButton
            // 
            this.saveAllButton.Image = ((System.Drawing.Image)(resources.GetObject("saveAllButton.Image")));
            this.saveAllButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("saveAllButton.SmallImage")));
            this.saveAllButton.Text = "Save All";
            this.saveAllButton.Click += new System.EventHandler(this.saveAllButton_Click);
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.ButtonMoreVisible = false;
            this.ribbonPanel2.Items.Add(this.avlLocButton);
            this.ribbonPanel2.Items.Add(this.ribbonCheckBox1);
            this.ribbonPanel2.Text = "Run Settings";
            // 
            // avlLocButton
            // 
            this.avlLocButton.Image = ((System.Drawing.Image)(resources.GetObject("avlLocButton.Image")));
            this.avlLocButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("avlLocButton.SmallImage")));
            this.avlLocButton.Text = "AVL.exe";
            this.avlLocButton.ToolTip = "Select location of the AVL executable.";
            this.avlLocButton.Click += new System.EventHandler(this.aVLLocationToolStripMenuItem_Click);
            // 
            // ribbonCheckBox1
            // 
            this.ribbonCheckBox1.Checked = global::AVL_Interface.Properties.Settings.Default.AVL_Inst_Count;
            this.ribbonCheckBox1.Text = "One Instance Per Aircraft";
            this.ribbonCheckBox1.ToolTip = "If checked, use only one instance of AVL per aircraft when running cases. Otherwi" +
    "se use one per CPU core.";
            this.ribbonCheckBox1.CheckBoxCheckChanged += new System.EventHandler(this.ribbonCheckBox1_CheckBoxCheckChanged);
            // 
            // ribbonPanel3
            // 
            this.ribbonPanel3.Items.Add(this.constraintWindowButton);
            this.ribbonPanel3.Text = "Run Cases";
            // 
            // constraintWindowButton
            // 
            this.constraintWindowButton.Image = ((System.Drawing.Image)(resources.GetObject("constraintWindowButton.Image")));
            this.constraintWindowButton.SmallImage = ((System.Drawing.Image)(resources.GetObject("constraintWindowButton.SmallImage")));
            this.constraintWindowButton.Text = "Design Analysis";
            this.constraintWindowButton.ToolTip = "Design the parameters to run AVL";
            this.constraintWindowButton.Click += new System.EventHandler(this.constraintWindowButton_Click);
            // 
            // splitContainer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 2);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 126);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(1264, 556);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(4, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(146, 556);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(138, 525);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "AIRCRAFT";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(138, 525);
            this.panel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(0, 100);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(138, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.flowLayoutPanel1_ControlAdded);
            this.flowLayoutPanel1.Resize += new System.EventHandler(this.flowLayoutPanel1_Resize);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1110, 556);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.aircraft_Info1);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1102, 525);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "AIRCRAFT DESIGN";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // aircraft_Info1
            // 
            this.aircraft_Info1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aircraft_Info1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aircraft_Info1.Location = new System.Drawing.Point(0, 0);
            this.aircraft_Info1.Margin = new System.Windows.Forms.Padding(0);
            this.aircraft_Info1.Name = "aircraft_Info1";
            this.aircraft_Info1.Size = new System.Drawing.Size(1102, 525);
            this.aircraft_Info1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.flowLayoutPanel2);
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1102, 525);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "CASES TO RUN";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AllowDrop = true;
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1102, 525);
            this.flowLayoutPanel2.TabIndex = 1;
            this.flowLayoutPanel2.WrapContents = false;
            this.flowLayoutPanel2.Resize += new System.EventHandler(this.flowLayoutPanel2_Resize);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chartUC1);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1102, 525);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "GRAPH RESULTS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chartUC1
            // 
            this.chartUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartUC1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.chartUC1.Location = new System.Drawing.Point(0, 0);
            this.chartUC1.Margin = new System.Windows.Forms.Padding(0, 6, 6, 6);
            this.chartUC1.Name = "chartUC1";
            this.chartUC1.Size = new System.Drawing.Size(1102, 525);
            this.chartUC1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.Form1_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Ribbon ribbon;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton openFileButton;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton avlLocButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Aircraft_Info aircraft_Info1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RibbonButton newFileButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonButton constraintWindowButton;
        private System.Windows.Forms.RibbonCheckBox ribbonCheckBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private ChartUC chartUC1;
        private System.Windows.Forms.RibbonButton saveAllButton;
    }
}

