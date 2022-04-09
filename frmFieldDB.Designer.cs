
namespace KimeraCS
{
    partial class frmFieldDB
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
            this.lblModel.Location = new System.Drawing.Point(16, 15);
            this.lblModel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(36, 13);
            this.lblModel.TabIndex = 0;
            this.lblModel.Text = "Model";
            // 
            // cbModel
            // 
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(19, 31);
            this.cbModel.Margin = new System.Windows.Forms.Padding(2);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(131, 21);
            this.cbModel.TabIndex = 1;
            this.cbModel.SelectedValueChanged += new System.EventHandler(this.cbModel_SelectedValueChanged);
            // 
            // gbModelNames
            // 
            this.gbModelNames.Controls.Add(this.lblModelNames);
            this.gbModelNames.ForeColor = System.Drawing.SystemColors.Control;
            this.gbModelNames.Location = new System.Drawing.Point(164, 15);
            this.gbModelNames.Margin = new System.Windows.Forms.Padding(2);
            this.gbModelNames.Name = "gbModelNames";
            this.gbModelNames.Padding = new System.Windows.Forms.Padding(2);
            this.gbModelNames.Size = new System.Drawing.Size(232, 163);
            this.gbModelNames.TabIndex = 2;
            this.gbModelNames.TabStop = false;
            this.gbModelNames.Text = "Model names";
            // 
            // lblModelNames
            // 
            this.lblModelNames.ForeColor = System.Drawing.SystemColors.Control;
            this.lblModelNames.Location = new System.Drawing.Point(11, 22);
            this.lblModelNames.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblModelNames.Name = "lblModelNames";
            this.lblModelNames.Size = new System.Drawing.Size(209, 128);
            this.lblModelNames.TabIndex = 0;
            this.lblModelNames.Text = "label1";
            // 
            // btnLoadModelAnimation
            // 
            this.btnLoadModelAnimation.Location = new System.Drawing.Point(164, 194);
            this.btnLoadModelAnimation.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadModelAnimation.Name = "btnLoadModelAnimation";
            this.btnLoadModelAnimation.Size = new System.Drawing.Size(232, 39);
            this.btnLoadModelAnimation.TabIndex = 3;
            this.btnLoadModelAnimation.Text = "Load Model and Animation";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.btnLoadModelAnimation_Click);
            // 
            // lblAnimations
            // 
            this.lblAnimations.AutoSize = true;
            this.lblAnimations.ForeColor = System.Drawing.SystemColors.Control;
            this.lblAnimations.Location = new System.Drawing.Point(16, 68);
            this.lblAnimations.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAnimations.Name = "lblAnimations";
            this.lblAnimations.Size = new System.Drawing.Size(58, 13);
            this.lblAnimations.TabIndex = 4;
            this.lblAnimations.Text = "Animations";
            // 
            // lbAnimation
            // 
            this.lbAnimation.FormattingEnabled = true;
            this.lbAnimation.Location = new System.Drawing.Point(19, 86);
            this.lbAnimation.Margin = new System.Windows.Forms.Padding(2);
            this.lbAnimation.Name = "lbAnimation";
            this.lbAnimation.Size = new System.Drawing.Size(131, 147);
            this.lbAnimation.TabIndex = 5;
            // 
            // lblFieldDataDir
            // 
            this.lblFieldDataDir.AutoSize = true;
            this.lblFieldDataDir.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFieldDataDir.Location = new System.Drawing.Point(16, 260);
            this.lblFieldDataDir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFieldDataDir.Name = "lblFieldDataDir";
            this.lblFieldDataDir.Size = new System.Drawing.Size(103, 13);
            this.lblFieldDataDir.TabIndex = 6;
            this.lblFieldDataDir.Text = "Field Data Directory:";
            // 
            // txtFieldDataDir
            // 
            this.txtFieldDataDir.Location = new System.Drawing.Point(19, 276);
            this.txtFieldDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.txtFieldDataDir.Name = "txtFieldDataDir";
            this.txtFieldDataDir.Size = new System.Drawing.Size(343, 20);
            this.txtFieldDataDir.TabIndex = 7;
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(366, 276);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(30, 20);
            this.btnSelectDirBrowser.TabIndex = 8;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.btnSelectDirBrowser_Click);
            // 
            // btnSaveFieldDataDir
            // 
            this.btnSaveFieldDataDir.Location = new System.Drawing.Point(19, 306);
            this.btnSaveFieldDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveFieldDataDir.Name = "btnSaveFieldDataDir";
            this.btnSaveFieldDataDir.Size = new System.Drawing.Size(188, 34);
            this.btnSaveFieldDataDir.TabIndex = 9;
            this.btnSaveFieldDataDir.Text = "Save Field Data Directory";
            this.btnSaveFieldDataDir.UseVisualStyleBackColor = true;
            this.btnSaveFieldDataDir.Click += new System.EventHandler(this.btnSaveFieldDataDir_Click);
            // 
            // lblLine
            // 
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLine.Location = new System.Drawing.Point(19, 246);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(377, 2);
            this.lblLine.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(208, 306);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(188, 34);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmFieldDB
            // 
            this.AcceptButton = this.btnLoadModelAnimation;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(414, 353);
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFieldDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FF7 Field Database";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFieldDB_FormClosed);
            this.Load += new System.EventHandler(this.frmFieldDB_Load);
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