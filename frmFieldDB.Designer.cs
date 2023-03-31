
namespace KimeraCS
{
    partial class FrmFieldDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFieldDB));
            this.lblModel = new System.Windows.Forms.Label();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.gbModelNames = new System.Windows.Forms.GroupBox();
            this.lblModelNames = new System.Windows.Forms.Label();
            this.btnLoadModelAnimation = new System.Windows.Forms.Button();
            this.lblAnimations = new System.Windows.Forms.Label();
            this.lbAnimation = new System.Windows.Forms.ListBox();
            this.lblFieldDataDir = new System.Windows.Forms.Label();
            this.txtFieldDataDir = new System.Windows.Forms.TextBox();
            this.btnSelectDirBrowser = new System.Windows.Forms.Button();
            this.btnSaveFieldDataDir = new System.Windows.Forms.Button();
            this.lblLine = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbModelNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.ForeColor = System.Drawing.SystemColors.Control;
            this.lblModel.Location = new System.Drawing.Point(21, 18);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(46, 17);
            this.lblModel.TabIndex = 0;
            this.lblModel.Text = "Model";
            // 
            // cbModel
            // 
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(25, 38);
            this.cbModel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(173, 24);
            this.cbModel.TabIndex = 1;
            this.cbModel.SelectedValueChanged += new System.EventHandler(this.CbModel_SelectedValueChanged);
            this.cbModel.TextChanged += new System.EventHandler(this.CbModel_TextChanged);
            // 
            // gbModelNames
            // 
            this.gbModelNames.Controls.Add(this.lblModelNames);
            this.gbModelNames.ForeColor = System.Drawing.SystemColors.Control;
            this.gbModelNames.Location = new System.Drawing.Point(219, 18);
            this.gbModelNames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbModelNames.Name = "gbModelNames";
            this.gbModelNames.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbModelNames.Size = new System.Drawing.Size(309, 201);
            this.gbModelNames.TabIndex = 2;
            this.gbModelNames.TabStop = false;
            this.gbModelNames.Text = "Model names";
            // 
            // lblModelNames
            // 
            this.lblModelNames.ForeColor = System.Drawing.SystemColors.Control;
            this.lblModelNames.Location = new System.Drawing.Point(15, 27);
            this.lblModelNames.Name = "lblModelNames";
            this.lblModelNames.Size = new System.Drawing.Size(279, 158);
            this.lblModelNames.TabIndex = 0;
            this.lblModelNames.Text = "label1";
            // 
            // btnLoadModelAnimation
            // 
            this.btnLoadModelAnimation.Location = new System.Drawing.Point(219, 239);
            this.btnLoadModelAnimation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadModelAnimation.Name = "btnLoadModelAnimation";
            this.btnLoadModelAnimation.Size = new System.Drawing.Size(309, 48);
            this.btnLoadModelAnimation.TabIndex = 3;
            this.btnLoadModelAnimation.Text = "Load Model and Animation";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.BtnLoadModelAnimation_Click);
            // 
            // lblAnimations
            // 
            this.lblAnimations.AutoSize = true;
            this.lblAnimations.ForeColor = System.Drawing.SystemColors.Control;
            this.lblAnimations.Location = new System.Drawing.Point(21, 84);
            this.lblAnimations.Name = "lblAnimations";
            this.lblAnimations.Size = new System.Drawing.Size(77, 17);
            this.lblAnimations.TabIndex = 4;
            this.lblAnimations.Text = "Animations";
            // 
            // lbAnimation
            // 
            this.lbAnimation.FormattingEnabled = true;
            this.lbAnimation.ItemHeight = 16;
            this.lbAnimation.Location = new System.Drawing.Point(25, 106);
            this.lbAnimation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbAnimation.Name = "lbAnimation";
            this.lbAnimation.Size = new System.Drawing.Size(173, 180);
            this.lbAnimation.TabIndex = 5;
            // 
            // lblFieldDataDir
            // 
            this.lblFieldDataDir.AutoSize = true;
            this.lblFieldDataDir.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFieldDataDir.Location = new System.Drawing.Point(23, 320);
            this.lblFieldDataDir.Name = "lblFieldDataDir";
            this.lblFieldDataDir.Size = new System.Drawing.Size(137, 17);
            this.lblFieldDataDir.TabIndex = 6;
            this.lblFieldDataDir.Text = "Field Data Directory:";
            // 
            // txtFieldDataDir
            // 
            this.txtFieldDataDir.Location = new System.Drawing.Point(25, 340);
            this.txtFieldDataDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFieldDataDir.Name = "txtFieldDataDir";
            this.txtFieldDataDir.Size = new System.Drawing.Size(456, 22);
            this.txtFieldDataDir.TabIndex = 7;
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(488, 340);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(40, 25);
            this.btnSelectDirBrowser.TabIndex = 8;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.BtnSelectDirBrowser_Click);
            // 
            // btnSaveFieldDataDir
            // 
            this.btnSaveFieldDataDir.Location = new System.Drawing.Point(25, 377);
            this.btnSaveFieldDataDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveFieldDataDir.Name = "btnSaveFieldDataDir";
            this.btnSaveFieldDataDir.Size = new System.Drawing.Size(251, 42);
            this.btnSaveFieldDataDir.TabIndex = 9;
            this.btnSaveFieldDataDir.Text = "Save Field Data Directory";
            this.btnSaveFieldDataDir.UseVisualStyleBackColor = true;
            this.btnSaveFieldDataDir.Click += new System.EventHandler(this.BtnSaveFieldDataDir_Click);
            // 
            // lblLine
            // 
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLine.Location = new System.Drawing.Point(25, 303);
            this.lblLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(503, 2);
            this.lblLine.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(277, 377);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(251, 42);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // frmFieldDB
            // 
            this.AcceptButton = this.btnLoadModelAnimation;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(552, 434);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.btnSaveFieldDataDir);
            this.Controls.Add(this.btnSelectDirBrowser);
            this.Controls.Add(this.txtFieldDataDir);
            this.Controls.Add(this.lblFieldDataDir);
            this.Controls.Add(this.lbAnimation);
            this.Controls.Add(this.lblAnimations);
            this.Controls.Add(this.btnLoadModelAnimation);
            this.Controls.Add(this.gbModelNames);
            this.Controls.Add(this.cbModel);
            this.Controls.Add(this.lblModel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFieldDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FF7 Field Database";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFieldDB_FormClosed);
            this.Load += new System.EventHandler(this.FrmFieldDB_Load);
            this.gbModelNames.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.GroupBox gbModelNames;
        private System.Windows.Forms.Button btnLoadModelAnimation;
        private System.Windows.Forms.Label lblAnimations;
        private System.Windows.Forms.ListBox lbAnimation;
        private System.Windows.Forms.Label lblFieldDataDir;
        private System.Windows.Forms.TextBox txtFieldDataDir;
        private System.Windows.Forms.Button btnSelectDirBrowser;
        private System.Windows.Forms.Button btnSaveFieldDataDir;
        private System.Windows.Forms.Label lblModelNames;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Button btnClose;
    }
}