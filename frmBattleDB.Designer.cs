
namespace KimeraCS
{
    partial class FrmBattleDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBattleDB));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveBattleDataDir = new System.Windows.Forms.Button();
            this.btnSelectDirBrowser = new System.Windows.Forms.Button();
            this.txtBattleDataDir = new System.Windows.Forms.TextBox();
            this.lblBattleDataDir = new System.Windows.Forms.Label();
            this.btnLoadModelAnimation = new System.Windows.Forms.Button();
            this.tcBattleDB = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvEnemies = new System.Windows.Forms.DataGridView();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvLocations = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvMainPCs = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcBattleDB.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnemies)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainPCs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(300, 382);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 34);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSaveBattleDataDir
            // 
            this.btnSaveBattleDataDir.Location = new System.Drawing.Point(9, 382);
            this.btnSaveBattleDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveBattleDataDir.Name = "btnSaveBattleDataDir";
            this.btnSaveBattleDataDir.Size = new System.Drawing.Size(151, 34);
            this.btnSaveBattleDataDir.TabIndex = 5;
            this.btnSaveBattleDataDir.Text = "Save Battle Data Directory";
            this.btnSaveBattleDataDir.UseVisualStyleBackColor = true;
            this.btnSaveBattleDataDir.Click += new System.EventHandler(this.BtnSaveBattleDataDir_Click);
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(405, 352);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(30, 20);
            this.btnSelectDirBrowser.TabIndex = 4;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.BtnSelectDirBrowser_Click);
            // 
            // txtBattleDataDir
            // 
            this.txtBattleDataDir.Location = new System.Drawing.Point(9, 352);
            this.txtBattleDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.txtBattleDataDir.Name = "txtBattleDataDir";
            this.txtBattleDataDir.Size = new System.Drawing.Size(392, 20);
            this.txtBattleDataDir.TabIndex = 3;
            // 
            // lblBattleDataDir
            // 
            this.lblBattleDataDir.AutoSize = true;
            this.lblBattleDataDir.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBattleDataDir.Location = new System.Drawing.Point(8, 336);
            this.lblBattleDataDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBattleDataDir.Name = "lblBattleDataDir";
            this.lblBattleDataDir.Size = new System.Drawing.Size(108, 13);
            this.lblBattleDataDir.TabIndex = 12;
            this.lblBattleDataDir.Text = "Battle Data Directory:";
            // 
            // btnLoadModelAnimation
            // 
            this.btnLoadModelAnimation.Location = new System.Drawing.Point(163, 382);
            this.btnLoadModelAnimation.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadModelAnimation.Name = "btnLoadModelAnimation";
            this.btnLoadModelAnimation.Size = new System.Drawing.Size(135, 34);
            this.btnLoadModelAnimation.TabIndex = 6;
            this.btnLoadModelAnimation.Text = "Load Model";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.BtnLoadModelAnimation_Click);
            // 
            // tcBattleDB
            // 
            this.tcBattleDB.Controls.Add(this.tabPage1);
            this.tcBattleDB.Controls.Add(this.tabPage2);
            this.tcBattleDB.Controls.Add(this.tabPage3);
            this.tcBattleDB.ItemSize = new System.Drawing.Size(52, 21);
            this.tcBattleDB.Location = new System.Drawing.Point(10, 10);
            this.tcBattleDB.Margin = new System.Windows.Forms.Padding(2);
            this.tcBattleDB.Name = "tcBattleDB";
            this.tcBattleDB.SelectedIndex = 0;
            this.tcBattleDB.Size = new System.Drawing.Size(425, 312);
            this.tcBattleDB.TabIndex = 1;
            this.tcBattleDB.SelectedIndexChanged += new System.EventHandler(this.TcBattleDB_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.dgvEnemies);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(417, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Enemies";
            // 
            // dgvEnemies
            // 
            this.dgvEnemies.AllowUserToAddRows = false;
            this.dgvEnemies.AllowUserToDeleteRows = false;
            this.dgvEnemies.AllowUserToOrderColumns = true;
            this.dgvEnemies.AllowUserToResizeColumns = false;
            this.dgvEnemies.AllowUserToResizeRows = false;
            this.dgvEnemies.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvEnemies.ColumnHeadersHeight = 20;
            this.dgvEnemies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFileName,
            this.colName});
            this.dgvEnemies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEnemies.Location = new System.Drawing.Point(0, 2);
            this.dgvEnemies.Margin = new System.Windows.Forms.Padding(2);
            this.dgvEnemies.MultiSelect = false;
            this.dgvEnemies.Name = "dgvEnemies";
            this.dgvEnemies.ReadOnly = true;
            this.dgvEnemies.RowHeadersWidth = 60;
            this.dgvEnemies.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEnemies.RowTemplate.Height = 20;
            this.dgvEnemies.RowTemplate.ReadOnly = true;
            this.dgvEnemies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvEnemies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnemies.Size = new System.Drawing.Size(419, 281);
            this.dgvEnemies.TabIndex = 2;
            this.dgvEnemies.Sorted += new System.EventHandler(this.DgvEnemies_Sorted);
            this.dgvEnemies.Click += new System.EventHandler(this.DgvEnemies_Click);
            this.dgvEnemies.DoubleClick += new System.EventHandler(this.DgvEnemies_DoubleClick);
            this.dgvEnemies.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DgvEnemies_KeyPress);
            // 
            // colFileName
            // 
            this.colFileName.Frozen = true;
            this.colFileName.HeaderText = "Filename";
            this.colFileName.MaxInputLength = 8;
            this.colFileName.MinimumWidth = 6;
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFileName.Width = 70;
            // 
            // colName
            // 
            this.colName.Frozen = true;
            this.colName.HeaderText = "Name";
            this.colName.MaxInputLength = 255;
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colName.Width = 270;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.dgvLocations);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(417, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Locations";
            // 
            // dgvLocations
            // 
            this.dgvLocations.AllowUserToAddRows = false;
            this.dgvLocations.AllowUserToDeleteRows = false;
            this.dgvLocations.AllowUserToOrderColumns = true;
            this.dgvLocations.AllowUserToResizeColumns = false;
            this.dgvLocations.AllowUserToResizeRows = false;
            this.dgvLocations.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvLocations.ColumnHeadersHeight = 20;
            this.dgvLocations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvLocations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLocations.Location = new System.Drawing.Point(0, 2);
            this.dgvLocations.Margin = new System.Windows.Forms.Padding(2);
            this.dgvLocations.MultiSelect = false;
            this.dgvLocations.Name = "dgvLocations";
            this.dgvLocations.ReadOnly = true;
            this.dgvLocations.RowHeadersWidth = 60;
            this.dgvLocations.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLocations.RowTemplate.Height = 20;
            this.dgvLocations.RowTemplate.ReadOnly = true;
            this.dgvLocations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocations.Size = new System.Drawing.Size(419, 281);
            this.dgvLocations.TabIndex = 8;
            this.dgvLocations.DoubleClick += new System.EventHandler(this.DgvLocations_DoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Filename";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 8;
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 255;
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.Width = 270;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.dgvMainPCs);
            this.tabPage3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(417, 283);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Main PCs";
            // 
            // dgvMainPCs
            // 
            this.dgvMainPCs.AllowUserToAddRows = false;
            this.dgvMainPCs.AllowUserToDeleteRows = false;
            this.dgvMainPCs.AllowUserToOrderColumns = true;
            this.dgvMainPCs.AllowUserToResizeColumns = false;
            this.dgvMainPCs.AllowUserToResizeRows = false;
            this.dgvMainPCs.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvMainPCs.ColumnHeadersHeight = 20;
            this.dgvMainPCs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvMainPCs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMainPCs.Location = new System.Drawing.Point(0, 2);
            this.dgvMainPCs.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMainPCs.MultiSelect = false;
            this.dgvMainPCs.Name = "dgvMainPCs";
            this.dgvMainPCs.ReadOnly = true;
            this.dgvMainPCs.RowHeadersWidth = 60;
            this.dgvMainPCs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMainPCs.RowTemplate.Height = 20;
            this.dgvMainPCs.RowTemplate.ReadOnly = true;
            this.dgvMainPCs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMainPCs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMainPCs.Size = new System.Drawing.Size(419, 281);
            this.dgvMainPCs.TabIndex = 9;
            this.dgvMainPCs.DoubleClick += new System.EventHandler(this.DgvMainPCs_DoubleClick);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Filename";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 8;
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "Name";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 255;
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.Width = 270;
            // 
            // frmBattleDB
            // 
            this.AcceptButton = this.btnLoadModelAnimation;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(446, 425);
            this.Controls.Add(this.tcBattleDB);
            this.Controls.Add(this.btnLoadModelAnimation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveBattleDataDir);
            this.Controls.Add(this.btnSelectDirBrowser);
            this.Controls.Add(this.txtBattleDataDir);
            this.Controls.Add(this.lblBattleDataDir);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBattleDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FF7 Battle Database";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmBattleDB_FormClosed);
            this.Load += new System.EventHandler(this.FrmBattleDB_Load);
            this.tcBattleDB.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnemies)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMainPCs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveBattleDataDir;
        private System.Windows.Forms.Button btnSelectDirBrowser;
        private System.Windows.Forms.TextBox txtBattleDataDir;
        private System.Windows.Forms.Label lblBattleDataDir;
        private System.Windows.Forms.Button btnLoadModelAnimation;
        private System.Windows.Forms.TabControl tcBattleDB;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvEnemies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridView dgvLocations;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView dgvMainPCs;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}