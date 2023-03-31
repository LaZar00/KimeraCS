
namespace KimeraCS
{
    partial class FrmTEXToPNGBatchConversion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTEXToPNGBatchConversion));
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.lblProcessingT2P = new System.Windows.Forms.Label();
            this.progBarTEXBatch = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chkFlipVertical = new System.Windows.Forms.CheckBox();
            this.chkRecursiveSearch = new System.Windows.Forms.CheckBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.chkShowOnlyNoProcessed = new System.Windows.Forms.CheckBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveOptions = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtTEX2PNGBatchPath = new System.Windows.Forms.TextBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.chkFFNXAALINaming = new System.Windows.Forms.CheckBox();
            this.gbProgress.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(8, 198);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(560, 77);
            this.rtbLog.TabIndex = 34;
            this.rtbLog.Text = "";
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.lblProcessingT2P);
            this.gbProgress.Controls.Add(this.progBarTEXBatch);
            this.gbProgress.Controls.Add(this.btnCancel);
            this.gbProgress.Enabled = false;
            this.gbProgress.ForeColor = System.Drawing.SystemColors.Control;
            this.gbProgress.Location = new System.Drawing.Point(8, 125);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Size = new System.Drawing.Size(560, 67);
            this.gbProgress.TabIndex = 33;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Progress";
            // 
            // lblProcessingT2P
            // 
            this.lblProcessingT2P.Location = new System.Drawing.Point(342, 15);
            this.lblProcessingT2P.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProcessingT2P.Name = "lblProcessingT2P";
            this.lblProcessingT2P.Size = new System.Drawing.Size(142, 19);
            this.lblProcessingT2P.TabIndex = 28;
            this.lblProcessingT2P.Text = "Processing... 0000  / 0000";
            this.lblProcessingT2P.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progBarTEXBatch
            // 
            this.progBarTEXBatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.progBarTEXBatch.Location = new System.Drawing.Point(11, 34);
            this.progBarTEXBatch.MarqueeAnimationSpeed = 10;
            this.progBarTEXBatch.Name = "progBarTEXBatch";
            this.progBarTEXBatch.Size = new System.Drawing.Size(473, 24);
            this.progBarTEXBatch.Step = 1;
            this.progBarTEXBatch.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progBarTEXBatch.TabIndex = 27;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(489, 34);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.chkFFNXAALINaming);
            this.gbSettings.Controls.Add(this.chkFlipVertical);
            this.gbSettings.Controls.Add(this.chkRecursiveSearch);
            this.gbSettings.Controls.Add(this.btnSaveLog);
            this.gbSettings.Controls.Add(this.chkShowOnlyNoProcessed);
            this.gbSettings.Controls.Add(this.lblPath);
            this.gbSettings.Controls.Add(this.btnClose);
            this.gbSettings.Controls.Add(this.btnSaveOptions);
            this.gbSettings.Controls.Add(this.btnGo);
            this.gbSettings.Controls.Add(this.txtTEX2PNGBatchPath);
            this.gbSettings.Controls.Add(this.btnPath);
            this.gbSettings.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSettings.Location = new System.Drawing.Point(8, 1);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(560, 118);
            this.gbSettings.TabIndex = 32;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // chkFlipVertical
            // 
            this.chkFlipVertical.Location = new System.Drawing.Point(308, 53);
            this.chkFlipVertical.Name = "chkFlipVertical";
            this.chkFlipVertical.Size = new System.Drawing.Size(85, 24);
            this.chkFlipVertical.TabIndex = 8;
            this.chkFlipVertical.Text = "Flip Vertical";
            this.chkFlipVertical.UseVisualStyleBackColor = true;
            // 
            // chkRecursiveSearch
            // 
            this.chkRecursiveSearch.Checked = true;
            this.chkRecursiveSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecursiveSearch.Location = new System.Drawing.Point(397, 53);
            this.chkRecursiveSearch.Name = "chkRecursiveSearch";
            this.chkRecursiveSearch.Size = new System.Drawing.Size(160, 24);
            this.chkRecursiveSearch.TabIndex = 7;
            this.chkRecursiveSearch.Text = "Recursive search in folders";
            this.chkRecursiveSearch.UseVisualStyleBackColor = true;
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSaveLog.Location = new System.Drawing.Point(342, 82);
            this.btnSaveLog.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(79, 28);
            this.btnSaveLog.TabIndex = 4;
            this.btnSaveLog.Text = "Save log";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.BtnSaveLog_Click);
            // 
            // chkShowOnlyNoProcessed
            // 
            this.chkShowOnlyNoProcessed.Location = new System.Drawing.Point(11, 53);
            this.chkShowOnlyNoProcessed.Name = "chkShowOnlyNoProcessed";
            this.chkShowOnlyNoProcessed.Size = new System.Drawing.Size(146, 24);
            this.chkShowOnlyNoProcessed.TabIndex = 6;
            this.chkShowOnlyNoProcessed.Text = "Log only not processed";
            this.chkShowOnlyNoProcessed.UseVisualStyleBackColor = true;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.ForeColor = System.Drawing.SystemColors.Control;
            this.lblPath.Location = new System.Drawing.Point(8, 29);
            this.lblPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "Path:";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Location = new System.Drawing.Point(425, 82);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(124, 28);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSaveOptions
            // 
            this.btnSaveOptions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSaveOptions.Location = new System.Drawing.Point(239, 82);
            this.btnSaveOptions.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveOptions.Name = "btnSaveOptions";
            this.btnSaveOptions.Size = new System.Drawing.Size(99, 28);
            this.btnSaveOptions.TabIndex = 3;
            this.btnSaveOptions.Text = "Save options";
            this.btnSaveOptions.UseVisualStyleBackColor = true;
            this.btnSaveOptions.Click += new System.EventHandler(this.BtnSaveOptions_Click);
            // 
            // btnGo
            // 
            this.btnGo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGo.Location = new System.Drawing.Point(11, 82);
            this.btnGo.Margin = new System.Windows.Forms.Padding(2);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(224, 28);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // txtTEX2PNGBatchPath
            // 
            this.txtTEX2PNGBatchPath.Location = new System.Drawing.Point(44, 26);
            this.txtTEX2PNGBatchPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtTEX2PNGBatchPath.Name = "txtTEX2PNGBatchPath";
            this.txtTEX2PNGBatchPath.Size = new System.Drawing.Size(471, 20);
            this.txtTEX2PNGBatchPath.TabIndex = 0;
            // 
            // btnPath
            // 
            this.btnPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPath.Location = new System.Drawing.Point(519, 26);
            this.btnPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(30, 20);
            this.btnPath.TabIndex = 1;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.BtnPath_Click);
            // 
            // chkFFNXAALINaming
            // 
            this.chkFFNXAALINaming.Location = new System.Drawing.Point(188, 53);
            this.chkFFNXAALINaming.Name = "chkFFNXAALINaming";
            this.chkFFNXAALINaming.Size = new System.Drawing.Size(116, 24);
            this.chkFFNXAALINaming.TabIndex = 9;
            this.chkFFNXAALINaming.Text = "FFNx/Aali naming";
            this.chkFFNXAALINaming.UseVisualStyleBackColor = true;
            // 
            // frmTEXToPNGBatchConversion
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(576, 285);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.gbProgress);
            this.Controls.Add(this.gbSettings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmTEXToPNGBatchConversion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TEX2PNG Batch Conversion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTEXToPNGBatchConversion_FormClosing);
            this.Load += new System.EventHandler(this.FrmTEXToPNGBatchConversion_Load);
            this.gbProgress.ResumeLayout(false);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.Label lblProcessingT2P;
        private System.Windows.Forms.ProgressBar progBarTEXBatch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.CheckBox chkShowOnlyNoProcessed;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveOptions;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtTEX2PNGBatchPath;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.CheckBox chkRecursiveSearch;
        private System.Windows.Forms.CheckBox chkFlipVertical;
        private System.Windows.Forms.CheckBox chkFFNXAALINaming;
    }
}