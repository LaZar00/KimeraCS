
namespace KimeraCS
{
    partial class frmBattleDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBattleDB));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveBattleDataDir = new System.Windows.Forms.Button();
            this.btnSelectDirBrowser = new System.Windows.Forms.Button();
            this.txtBattleDataDir = new System.Windows.Forms.TextBox();
            this.lblBattleDataDir = new System.Windows.Forms.Label();
            this.btnLoadModelAnimation = new System.Windows.Forms.Button();
            this.tcBattleDB = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbEnemies = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbLocations = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lbMainPCs = new System.Windows.Forms.ListBox();
            this.tcBattleDB.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(300, 382);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 34);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSaveBattleDataDir
            // 
            this.btnSaveBattleDataDir.Location = new System.Drawing.Point(9, 382);
            this.btnSaveBattleDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveBattleDataDir.Name = "btnSaveBattleDataDir";
            this.btnSaveBattleDataDir.Size = new System.Drawing.Size(151, 34);
            this.btnSaveBattleDataDir.TabIndex = 15;
            this.btnSaveBattleDataDir.Text = "Save Battle Data Directory";
            this.btnSaveBattleDataDir.UseVisualStyleBackColor = true;
            this.btnSaveBattleDataDir.Click += new System.EventHandler(this.btnSaveBattleDataDir_Click);
            // 
            // btnSelectDirBrowser
            // 
            this.btnSelectDirBrowser.Location = new System.Drawing.Point(405, 352);
            this.btnSelectDirBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectDirBrowser.Name = "btnSelectDirBrowser";
            this.btnSelectDirBrowser.Size = new System.Drawing.Size(30, 20);
            this.btnSelectDirBrowser.TabIndex = 14;
            this.btnSelectDirBrowser.Text = "...";
            this.btnSelectDirBrowser.UseVisualStyleBackColor = true;
            this.btnSelectDirBrowser.Click += new System.EventHandler(this.btnSelectDirBrowser_Click);
            // 
            // txtBattleDataDir
            // 
            this.txtBattleDataDir.Location = new System.Drawing.Point(9, 352);
            this.txtBattleDataDir.Margin = new System.Windows.Forms.Padding(2);
            this.txtBattleDataDir.Name = "txtBattleDataDir";
            this.txtBattleDataDir.Size = new System.Drawing.Size(392, 20);
            this.txtBattleDataDir.TabIndex = 13;
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
            this.btnLoadModelAnimation.TabIndex = 18;
            this.btnLoadModelAnimation.Text = "Load Model";
            this.btnLoadModelAnimation.UseVisualStyleBackColor = true;
            this.btnLoadModelAnimation.Click += new System.EventHandler(this.btnLoadModelAnimation_Click);
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
            this.tcBattleDB.Size = new System.Drawing.Size(425, 310);
            this.tcBattleDB.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.lbEnemies);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(417, 281);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Enemies";
            // 
            // lbEnemies
            // 
            this.lbEnemies.FormattingEnabled = true;
            this.lbEnemies.Location = new System.Drawing.Point(3, 4);
            this.lbEnemies.Margin = new System.Windows.Forms.Padding(2);
            this.lbEnemies.Name = "lbEnemies";
            this.lbEnemies.Size = new System.Drawing.Size(414, 277);
            this.lbEnemies.TabIndex = 6;
            this.lbEnemies.DoubleClick += new System.EventHandler(this.lbEnemies_DoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.lbLocations);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(417, 281);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Locations";
            // 
            // lbLocations
            // 
            this.lbLocations.FormattingEnabled = true;
            this.lbLocations.Location = new System.Drawing.Point(3, 4);
            this.lbLocations.Margin = new System.Windows.Forms.Padding(2);
            this.lbLocations.Name = "lbLocations";
            this.lbLocations.Size = new System.Drawing.Size(414, 277);
            this.lbLocations.TabIndex = 7;
            this.lbLocations.DoubleClick += new System.EventHandler(this.lbLocations_DoubleClick);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.lbMainPCs);
            this.tabPage3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(417, 281);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Main PCs";
            // 
            // lbMainPCs
            // 
            this.lbMainPCs.FormattingEnabled = true;
            this.lbMainPCs.Location = new System.Drawing.Point(3, 4);
            this.lbMainPCs.Margin = new System.Windows.Forms.Padding(2);
            this.lbMainPCs.Name = "lbMainPCs";
            this.lbMainPCs.Size = new System.Drawing.Size(414, 277);
            this.lbMainPCs.TabIndex = 7;
            this.lbMainPCs.DoubleClick += new System.EventHandler(this.lbMainPCs_DoubleClick);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBattleDB_FormClosed);
            this.Load += new System.EventHandler(this.frmBattleDB_Load);
            this.tcBattleDB.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lbEnemies;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lbLocations;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox lbMainPCs;
    }
}