namespace AVL_Interface
{
    partial class ConstraintsUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstraintsUC));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.runCaseButton = new System.Windows.Forms.Button();
            this.clearCasesButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.runCaseButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.clearCasesButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(419, 271);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(100, 60);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(100, 60);
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 3);
            this.pictureBox1.Size = new System.Drawing.Size(100, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.statusLabel, 2);
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(106, 2);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 15);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status";
            // 
            // runCaseButton
            // 
            this.runCaseButton.FlatAppearance.BorderSize = 0;
            this.runCaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runCaseButton.Image = ((System.Drawing.Image)(resources.GetObject("runCaseButton.Image")));
            this.runCaseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.runCaseButton.Location = new System.Drawing.Point(106, 23);
            this.runCaseButton.Name = "runCaseButton";
            this.runCaseButton.Size = new System.Drawing.Size(97, 24);
            this.runCaseButton.TabIndex = 2;
            this.runCaseButton.Text = "Run Cases ";
            this.runCaseButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.runCaseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.runCaseButton.UseVisualStyleBackColor = true;
            this.runCaseButton.Click += new System.EventHandler(this.runCaseButton_Click);
            // 
            // clearCasesButton
            // 
            this.clearCasesButton.FlatAppearance.BorderSize = 0;
            this.clearCasesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearCasesButton.Image = ((System.Drawing.Image)(resources.GetObject("clearCasesButton.Image")));
            this.clearCasesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearCasesButton.Location = new System.Drawing.Point(209, 23);
            this.clearCasesButton.Name = "clearCasesButton";
            this.clearCasesButton.Size = new System.Drawing.Size(97, 24);
            this.clearCasesButton.TabIndex = 3;
            this.clearCasesButton.Text = "Clear Cases";
            this.clearCasesButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearCasesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.clearCasesButton.UseVisualStyleBackColor = true;
            this.clearCasesButton.Click += new System.EventHandler(this.clearCasesButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.tableLayoutPanel1.SetColumnSpan(this.listView1, 3);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(3, 69);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(413, 199);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Variable";
            this.columnHeader1.Width = 159;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Constant";
            this.columnHeader2.Width = 131;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 78;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "009_HighPriority_16x16_72.png");
            this.imageList1.Images.SetKeyName(1, "112_Tick_Green_32x32_72.png");
            this.imageList1.Images.SetKeyName(2, "112_Minus_Grey_16x16_72.png");
            this.imageList1.Images.SetKeyName(3, "FavoriteStar_16x16.png");
            this.imageList1.Images.SetKeyName(4, "1522_stop_watch_48x48.png");
            // 
            // ConstraintsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConstraintsUC";
            this.Size = new System.Drawing.Size(419, 271);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button runCaseButton;
        private System.Windows.Forms.Button clearCasesButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
