
namespace KimeraCS
{
    partial class FrmStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStatistics));
            this.rtbStats = new System.Windows.Forms.RichTextBox();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSaveStats = new System.Windows.Forms.Button();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // rtbStats
            // 
            this.rtbStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbStats.DetectUrls = false;
            this.rtbStats.Font = new System.Drawing.Font("Consolas", 14F);
            this.rtbStats.Location = new System.Drawing.Point(12, 12);
            this.rtbStats.Name = "rtbStats";
            this.rtbStats.ReadOnly = true;
            this.rtbStats.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbStats.Size = new System.Drawing.Size(760, 538);
            this.rtbStats.TabIndex = 0;
            this.rtbStats.Text = "";
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnClose.Location = new System.Drawing.Point(520, 556);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(252, 33);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSaveStats
            // 
            this.BtnSaveStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnSaveStats.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSaveStats.Location = new System.Drawing.Point(12, 556);
            this.BtnSaveStats.Name = "BtnSaveStats";
            this.BtnSaveStats.Size = new System.Drawing.Size(252, 33);
            this.BtnSaveStats.TabIndex = 2;
            this.BtnSaveStats.Text = "Save Statistics";
            this.BtnSaveStats.UseVisualStyleBackColor = true;
            this.BtnSaveStats.Click += new System.EventHandler(this.BtnSaveStats_Click);
            // 
            // FrmStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(784, 601);
            this.Controls.Add(this.BtnSaveStats);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.rtbStats);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(550, 39);
            this.Name = "FrmStatistics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Skeleton Statistics";
            this.Load += new System.EventHandler(this.FrmStatistics_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.RichTextBox rtbStats;
        private System.Windows.Forms.Button BtnSaveStats;
        private System.Windows.Forms.SaveFileDialog saveFile;
    }
}