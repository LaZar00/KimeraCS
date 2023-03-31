
namespace KimeraCS
{
    partial class FrmMagicDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMagicDB));
            this.btnLoadModelAnimation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveMagicDataDir = new System.Windows.Forms.Button();
            this.btnSelectDirBrowser = new System.Windows.Forms.Button();
            this.txtMagicDataDir = new System.Windows.Forms.TextBox();
            this.lblMagicDataDir = new System.Windows.Forms.Label();
            this.dgvMagic = new System.Windows.Forms.DataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMagic)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadModelAnimation
            // 
            this.btnLoadModelAnimation.Location = new System.Drawing.Point(164, 394);
            this.btnLoadModelAnimation.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadModelAnimation.Name = "btnLoadModelAnimation";
            this.btnLoadModelAnimation.Size = new System.Drawing.Size(135, 34);
            this.btnLoadModelAnimation.TabIndex = 24;
            this.btnLoadModelAnimation.Text = "Load Model";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.BtnLoadModelAnimation_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(304, 394);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 34);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSaveMagicDataDir
            // 
            this.btnSaveMagicDataDir.Location = new System.Drawing.Point(9, 394);
            this.btnSaveMagicDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveMagicDataDir.Name = "btnSaveMagicDataDir";
            this.btnSaveMagicDataDir.Size = new System.Drawing.Size(151, 34);
            this.btnSaveMagicDataDir.TabIndex = 22;
            this.btnSaveMagicDataDir.Text = "Save Magic Data Directory";
            this.btnSaveMagicDataDir.UseVisualStyleBackColor = true;
            this.btnSaveMagicDataDir.Click += new System.EventHandler(this.BtnSaveMagicDataDir_Click);
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(409, 364);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(30, 20);
            this.btnSelectDirBrowser.TabIndex = 21;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.BtnSelectDirBrowser_Click);
            // 
            // txtMagicDataDir
            // 
            this.txtMagicDataDir.Location = new System.Drawing.Point(9, 364);
            this.txtMagicDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.txtMagicDataDir.Name = "txtMagicDataDir";
            this.txtMagicDataDir.Size = new System.Drawing.Size(396, 20);
            this.txtMagicDataDir.TabIndex = 20;
            // 
            // lblMagicDataDir
            // 
            this.lblMagicDataDir.AutoSize = true;
            this.lblMagicDataDir.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMagicDataDir.Location = new System.Drawing.Point(8, 348);
            this.lblMagicDataDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMagicDataDir.Name = "lblMagicDataDir";
            this.lblMagicDataDir.Size = new System.Drawing.Size(110, 13);
            this.lblMagicDataDir.TabIndex = 19;
            this.lblMagicDataDir.Text = "Magic Data Directory:";
            // 
            // dgvMagic
            // 
            this.dgvMagic.AllowUserToAddRows = false;
            this.dgvMagic.AllowUserToDeleteRows = false;
            this.dgvMagic.AllowUserToOrderColumns = true;
            this.dgvMagic.AllowUserToResizeColumns = false;
            this.dgvMagic.AllowUserToResizeRows = false;
            this.dgvMagic.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvMagic.ColumnHeadersHeight = 20;
            this.dgvMagic.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colFileName,
            this.colName});
            this.dgvMagic.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMagic.Location = new System.Drawing.Point(11, 11);
            this.dgvMagic.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMagic.MultiSelect = false;
            this.dgvMagic.Name = "dgvMagic";
            this.dgvMagic.ReadOnly = true;
            this.dgvMagic.RowHeadersVisible = false;
            this.dgvMagic.RowHeadersWidth = 60;
            this.dgvMagic.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMagic.RowTemplate.Height = 20;
            this.dgvMagic.RowTemplate.ReadOnly = true;
            this.dgvMagic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMagic.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMagic.Size = new System.Drawing.Size(428, 322);
            this.dgvMagic.TabIndex = 25;
            this.dgvMagic.DoubleClick += new System.EventHandler(this.DgvMagic_DoubleClick);
            // 
            // colNumber
            // 
            this.colNumber.Frozen = true;
            this.colNumber.HeaderText = "";
            this.colNumber.MaxInputLength = 5;
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNumber.Width = 60;
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
            // frmMagicDB
            // 
            this.AcceptButton = this.btnLoadModelAnimation;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(449, 437);
            this.Controls.Add(this.dgvMagic);
            this.Controls.Add(this.btnLoadModelAnimation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveMagicDataDir);
            this.Controls.Add(this.btnSelectDirBrowser);
            this.Controls.Add(this.txtMagicDataDir);
            this.Controls.Add(this.lblMagicDataDir);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMagicDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FF7 Magic Database";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMagicDB_FormClosed);
            this.Load += new System.EventHandler(this.FrmMagicDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMagic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLoadModelAnimation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveMagicDataDir;
        private System.Windows.Forms.Button btnSelectDirBrowser;
        private System.Windows.Forms.TextBox txtMagicDataDir;
        private System.Windows.Forms.Label lblMagicDataDir;
        private System.Windows.Forms.DataGridView dgvMagic;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    }
}