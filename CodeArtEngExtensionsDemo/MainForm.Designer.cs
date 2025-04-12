namespace CodeArtEngExtensionsDemo
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DgvRevert = new System.Windows.Forms.Button();
            this.DgvCommit = new System.Windows.Forms.Button();
            this.DgvEnable = new System.Windows.Forms.Button();
            this.BtCount = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Dgv);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DataGridView";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(3, 43);
            this.Dgv.Name = "Dgv";
            this.Dgv.Size = new System.Drawing.Size(786, 378);
            this.Dgv.TabIndex = 1;
            this.Dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellEndEdit);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Column7";
            this.Column7.Name = "Column7";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtCount);
            this.panel1.Controls.Add(this.DgvRevert);
            this.panel1.Controls.Add(this.DgvCommit);
            this.panel1.Controls.Add(this.DgvEnable);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 40);
            this.panel1.TabIndex = 0;
            // 
            // DgvRevert
            // 
            this.DgvRevert.Location = new System.Drawing.Point(173, 9);
            this.DgvRevert.Name = "DgvRevert";
            this.DgvRevert.Size = new System.Drawing.Size(75, 23);
            this.DgvRevert.TabIndex = 2;
            this.DgvRevert.Text = "Revert";
            this.DgvRevert.UseVisualStyleBackColor = true;
            this.DgvRevert.Click += new System.EventHandler(this.DgvRevert_Click);
            // 
            // DgvCommit
            // 
            this.DgvCommit.Location = new System.Drawing.Point(92, 9);
            this.DgvCommit.Name = "DgvCommit";
            this.DgvCommit.Size = new System.Drawing.Size(75, 23);
            this.DgvCommit.TabIndex = 1;
            this.DgvCommit.Text = "Commit";
            this.DgvCommit.UseVisualStyleBackColor = true;
            this.DgvCommit.Click += new System.EventHandler(this.DgvCommit_Click);
            // 
            // DgvEnable
            // 
            this.DgvEnable.Location = new System.Drawing.Point(11, 9);
            this.DgvEnable.Name = "DgvEnable";
            this.DgvEnable.Size = new System.Drawing.Size(75, 23);
            this.DgvEnable.TabIndex = 0;
            this.DgvEnable.Text = "Enable";
            this.DgvEnable.UseVisualStyleBackColor = true;
            this.DgvEnable.Click += new System.EventHandler(this.DgvEnable_Click);
            // 
            // BtCount
            // 
            this.BtCount.Location = new System.Drawing.Point(706, 9);
            this.BtCount.Name = "BtCount";
            this.BtCount.Size = new System.Drawing.Size(75, 23);
            this.BtCount.TabIndex = 3;
            this.BtCount.Text = "Count";
            this.BtCount.UseVisualStyleBackColor = true;
            this.BtCount.Click += new System.EventHandler(this.BtCount_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "MainForm - Tests";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button DgvRevert;
        private System.Windows.Forms.Button DgvCommit;
        private System.Windows.Forms.Button DgvEnable;
        private System.Windows.Forms.Button BtCount;
    }
}

