
namespace KimeraCS
{
    partial class frmMagicDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMagicDB));
            this.lbMagic = new System.Windows.Forms.ListBox();
            this.btnLoadModelAnimation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveMagicDataDir = new System.Windows.Forms.Button();
            this.btnSelectDirBrowser = new System.Windows.Forms.Button();
            this.txtMagicDataDir = new System.Windows.Forms.TextBox();
            this.lblMagicDataDir = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbMagic
            // 
            this.lbMagic.FormattingEnabled = true;
            this.lbMagic.ItemHeight = 16;
            this.lbMagic.Location = new System.Drawing.Point(12, 11);
            this.lbMagic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbMagic.Name = "lbMagic";
            this.lbMagic.Size = new System.Drawing.Size(574, 404);
            this.lbMagic.TabIndex = 7;
            this.lbMagic.DoubleClick += new System.EventHandler(this.lbMagic_DoubleClick);
            // 
            // btnLoadModelAnimation
            // 
            this.btnLoadModelAnimation.Location = new System.Drawing.Point(217, 491);
            this.btnLoadModelAnimation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadModelAnimation.Name = "btnLoadModelAnimation";
            this.btnLoadModelAnimation.Size = new System.Drawing.Size(180, 42);
            this.btnLoadModelAnimation.TabIndex = 24;
            this.btnLoadModelAnimation.Text = "Load Model";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.btnLoadModelAnimation_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(400, 491);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 42);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveMagicDataDir
            // 
            this.btnSaveMagicDataDir.Location = new System.Drawing.Point(12, 491);
            this.btnSaveMagicDataDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveMagicDataDir.Name = "btnSaveMagicDataDir";
            this.btnSaveMagicDataDir.Size = new System.Drawing.Size(201, 42);
            this.btnSaveMagicDataDir.TabIndex = 22;
            this.btnSaveMagicDataDir.Text = "Save Magic Data Directory";
            this.btnSaveMagicDataDir.UseVisualStyleBackColor = true;
            this.btnSaveMagicDataDir.Click += new System.EventHandler(this.btnSaveMagicDataDir_Click);
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(540, 454);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(40, 25);
            this.btnSelectDirBrowser.TabIndex = 21;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.btnSelectDirBrowser_Click);
            // 
            // txtMagicDataDir
            // 
            this.txtMagicDataDir.Location = new System.Drawing.Point(12, 454);
            this.txtMagicDataDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMagicDataDir.Name = "txtMagicDataDir";
            this.txtMagicDataDir.Size = new System.Drawing.Size(521, 22);
            this.txtMagicDataDir.TabIndex = 20;
            // 
            // lblMagicDataDir
            // 
            this.lblMagicDataDir.AutoSize = true;
            this.lblMagicDataDir.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMagicDataDir.Location = new System.Drawing.Point(11, 435);
            this.lblMagicDataDir.Name = "lblMagicDataDir";
            this.lblMagicDataDir.Size = new System.Drawing.Size(144, 17);
            this.lblMagicDataDir.TabIndex = 19;
            this.lblMagicDataDir.Text = "Magic Data Directory:";
            // 
            // frmMagicDB
            // 
            this.AcceptButton = this.btnLoadModelAnimation;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(599, 544);
            this.Controls.Add(this.btnLoadModelAnimation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveMagicDataDir);
            this.Controls.Add(this.btnSelectDirBrowser);
            this.Controls.Add(this.txtMagicDataDir);
            this.Controls.Add(this.lblMagicDataDir);
            this.Controls.Add(this.lbMagic);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMagicDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FF7 Magic Database";
            this.Load += new System.EventHandler(this.frmMagicDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbMagic;
        private System.Windows.Forms.Button btnLoadModelAnimation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveMagicDataDir;
        private System.Windows.Forms.Button btnSelectDirBrowser;
        private System.Windows.Forms.TextBox txtMagicDataDir;
        private System.Windows.Forms.Label lblMagicDataDir;
    }
}