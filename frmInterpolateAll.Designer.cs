﻿
namespace KimeraCS
{
    partial class frmInterpolateAll
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInterpolateAll));
            this.lblExtractedCharLGP = new System.Windows.Forms.Label();
            this.lblExtractedBattleLGP = new System.Windows.Forms.Label();
            this.lblExtractedMagicLGP = new System.Windows.Forms.Label();
            this.lblSourcePathChar = new System.Windows.Forms.Label();
            this.lblDestinationPathChar = new System.Windows.Forms.Label();
            this.lblNumInterpolatedFrames = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnSaveOptions = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.nudInterpFrameField = new System.Windows.Forms.NumericUpDown();
            this.nudInterpFrameBattleMagic = new System.Windows.Forms.NumericUpDown();
            this.txtExtractedCharLGPSrc = new System.Windows.Forms.TextBox();
            this.txtExtractedBattleLGPSrc = new System.Windows.Forms.TextBox();
            this.txtExtractedMagicLGPSrc = new System.Windows.Forms.TextBox();
            this.txtExtractedCharLGPDest = new System.Windows.Forms.TextBox();
            this.txtExtractedBattleLGPDest = new System.Windows.Forms.TextBox();
            this.txtExtractedMagicLGPDest = new System.Windows.Forms.TextBox();
            this.btnExtCharLGPSrc = new System.Windows.Forms.Button();
            this.btnExtCharLGPDst = new System.Windows.Forms.Button();
            this.btnExtBattleLGPSrc = new System.Windows.Forms.Button();
            this.btnExtBattleLGPDst = new System.Windows.Forms.Button();
            this.btnExtMagicLGPSrc = new System.Windows.Forms.Button();
            this.btnExtMagicLGPDst = new System.Windows.Forms.Button();
            this.chkProcessCharLGP = new System.Windows.Forms.CheckBox();
            this.chkProcessBattleLGP = new System.Windows.Forms.CheckBox();
            this.chkProcessMagicLGP = new System.Windows.Forms.CheckBox();
            this.lblSourcePathBattle = new System.Windows.Forms.Label();
            this.lblDestinationPathBattle = new System.Windows.Forms.Label();
            this.lblSourcePathMagic = new System.Windows.Forms.Label();
            this.lblDestinationPathMagic = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progBarIntAllAnim = new System.Windows.Forms.ProgressBar();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.chkShowOnlyNoProcessed = new System.Windows.Forms.CheckBox();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpFrameField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpFrameBattleMagic)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblExtractedCharLGP
            // 
            this.lblExtractedCharLGP.AutoSize = true;
            this.lblExtractedCharLGP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractedCharLGP.ForeColor = System.Drawing.SystemColors.Control;
            this.lblExtractedCharLGP.Location = new System.Drawing.Point(8, 28);
            this.lblExtractedCharLGP.Name = "lblExtractedCharLGP";
            this.lblExtractedCharLGP.Size = new System.Drawing.Size(159, 17);
            this.lblExtractedCharLGP.TabIndex = 0;
            this.lblExtractedCharLGP.Text = "Extracted CHAR.LGP";
            // 
            // lblExtractedBattleLGP
            // 
            this.lblExtractedBattleLGP.AutoSize = true;
            this.lblExtractedBattleLGP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractedBattleLGP.ForeColor = System.Drawing.SystemColors.Control;
            this.lblExtractedBattleLGP.Location = new System.Drawing.Point(8, 126);
            this.lblExtractedBattleLGP.Name = "lblExtractedBattleLGP";
            this.lblExtractedBattleLGP.Size = new System.Drawing.Size(176, 17);
            this.lblExtractedBattleLGP.TabIndex = 1;
            this.lblExtractedBattleLGP.Text = "Extracted BATTLE.LGP";
            // 
            // lblExtractedMagicLGP
            // 
            this.lblExtractedMagicLGP.AutoSize = true;
            this.lblExtractedMagicLGP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractedMagicLGP.ForeColor = System.Drawing.SystemColors.Control;
            this.lblExtractedMagicLGP.Location = new System.Drawing.Point(8, 214);
            this.lblExtractedMagicLGP.Name = "lblExtractedMagicLGP";
            this.lblExtractedMagicLGP.Size = new System.Drawing.Size(165, 17);
            this.lblExtractedMagicLGP.TabIndex = 2;
            this.lblExtractedMagicLGP.Text = "Extracted MAGIC.LGP";
            // 
            // lblSourcePathChar
            // 
            this.lblSourcePathChar.AutoSize = true;
            this.lblSourcePathChar.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSourcePathChar.Location = new System.Drawing.Point(35, 58);
            this.lblSourcePathChar.Name = "lblSourcePathChar";
            this.lblSourcePathChar.Size = new System.Drawing.Size(89, 17);
            this.lblSourcePathChar.TabIndex = 3;
            this.lblSourcePathChar.Text = "Source path:";
            // 
            // lblDestinationPathChar
            // 
            this.lblDestinationPathChar.AutoSize = true;
            this.lblDestinationPathChar.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDestinationPathChar.Location = new System.Drawing.Point(9, 82);
            this.lblDestinationPathChar.Name = "lblDestinationPathChar";
            this.lblDestinationPathChar.Size = new System.Drawing.Size(115, 17);
            this.lblDestinationPathChar.TabIndex = 4;
            this.lblDestinationPathChar.Text = "Destination path:";
            // 
            // lblNumInterpolatedFrames
            // 
            this.lblNumInterpolatedFrames.ForeColor = System.Drawing.SystemColors.Control;
            this.lblNumInterpolatedFrames.Location = new System.Drawing.Point(533, 12);
            this.lblNumInterpolatedFrames.Name = "lblNumInterpolatedFrames";
            this.lblNumInterpolatedFrames.Size = new System.Drawing.Size(123, 39);
            this.lblNumInterpolatedFrames.TabIndex = 5;
            this.lblNumInterpolatedFrames.Text = "Num Interpolated Frames";
            this.lblNumInterpolatedFrames.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.ForeColor = System.Drawing.SystemColors.Control;
            this.lblProcess.Location = new System.Drawing.Point(676, 23);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(59, 17);
            this.lblProcess.TabIndex = 6;
            this.lblProcess.Text = "Process";
            // 
            // btnGo
            // 
            this.btnGo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGo.Location = new System.Drawing.Point(15, 335);
            this.btnGo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(299, 34);
            this.btnGo.TabIndex = 19;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnSaveOptions
            // 
            this.btnSaveOptions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSaveOptions.Location = new System.Drawing.Point(319, 335);
            this.btnSaveOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveOptions.Name = "btnSaveOptions";
            this.btnSaveOptions.Size = new System.Drawing.Size(132, 34);
            this.btnSaveOptions.TabIndex = 1;
            this.btnSaveOptions.Text = "Save options";
            this.btnSaveOptions.UseVisualStyleBackColor = true;
            this.btnSaveOptions.Click += new System.EventHandler(this.btnSaveOptions_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Location = new System.Drawing.Point(567, 335);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(165, 34);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // nudInterpFrameField
            // 
            this.nudInterpFrameField.Location = new System.Drawing.Point(560, 69);
            this.nudInterpFrameField.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudInterpFrameField.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInterpFrameField.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterpFrameField.Name = "nudInterpFrameField";
            this.nudInterpFrameField.Size = new System.Drawing.Size(71, 22);
            this.nudInterpFrameField.TabIndex = 14;
            this.nudInterpFrameField.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudInterpFrameBattleMagic
            // 
            this.nudInterpFrameBattleMagic.Location = new System.Drawing.Point(560, 209);
            this.nudInterpFrameBattleMagic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudInterpFrameBattleMagic.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInterpFrameBattleMagic.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterpFrameBattleMagic.Name = "nudInterpFrameBattleMagic";
            this.nudInterpFrameBattleMagic.Size = new System.Drawing.Size(71, 22);
            this.nudInterpFrameBattleMagic.TabIndex = 15;
            this.nudInterpFrameBattleMagic.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtExtractedCharLGPSrc
            // 
            this.txtExtractedCharLGPSrc.Location = new System.Drawing.Point(125, 54);
            this.txtExtractedCharLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedCharLGPSrc.Name = "txtExtractedCharLGPSrc";
            this.txtExtractedCharLGPSrc.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedCharLGPSrc.TabIndex = 2;
            // 
            // txtExtractedBattleLGPSrc
            // 
            this.txtExtractedBattleLGPSrc.Location = new System.Drawing.Point(125, 151);
            this.txtExtractedBattleLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedBattleLGPSrc.Name = "txtExtractedBattleLGPSrc";
            this.txtExtractedBattleLGPSrc.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedBattleLGPSrc.TabIndex = 6;
            // 
            // txtExtractedMagicLGPSrc
            // 
            this.txtExtractedMagicLGPSrc.Location = new System.Drawing.Point(125, 240);
            this.txtExtractedMagicLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedMagicLGPSrc.Name = "txtExtractedMagicLGPSrc";
            this.txtExtractedMagicLGPSrc.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedMagicLGPSrc.TabIndex = 10;
            // 
            // txtExtractedCharLGPDest
            // 
            this.txtExtractedCharLGPDest.Location = new System.Drawing.Point(125, 80);
            this.txtExtractedCharLGPDest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedCharLGPDest.Name = "txtExtractedCharLGPDest";
            this.txtExtractedCharLGPDest.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedCharLGPDest.TabIndex = 4;
            // 
            // txtExtractedBattleLGPDest
            // 
            this.txtExtractedBattleLGPDest.Location = new System.Drawing.Point(125, 177);
            this.txtExtractedBattleLGPDest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedBattleLGPDest.Name = "txtExtractedBattleLGPDest";
            this.txtExtractedBattleLGPDest.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedBattleLGPDest.TabIndex = 8;
            // 
            // txtExtractedMagicLGPDest
            // 
            this.txtExtractedMagicLGPDest.Location = new System.Drawing.Point(125, 266);
            this.txtExtractedMagicLGPDest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExtractedMagicLGPDest.Name = "txtExtractedMagicLGPDest";
            this.txtExtractedMagicLGPDest.Size = new System.Drawing.Size(343, 22);
            this.txtExtractedMagicLGPDest.TabIndex = 12;
            // 
            // btnExtCharLGPSrc
            // 
            this.btnExtCharLGPSrc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtCharLGPSrc.Location = new System.Drawing.Point(475, 54);
            this.btnExtCharLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtCharLGPSrc.Name = "btnExtCharLGPSrc";
            this.btnExtCharLGPSrc.Size = new System.Drawing.Size(40, 25);
            this.btnExtCharLGPSrc.TabIndex = 3;
            this.btnExtCharLGPSrc.Text = "...";
            this.btnExtCharLGPSrc.UseVisualStyleBackColor = true;
            this.btnExtCharLGPSrc.Click += new System.EventHandler(this.btnExtCharLGPSrc_Click);
            // 
            // btnExtCharLGPDst
            // 
            this.btnExtCharLGPDst.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtCharLGPDst.Location = new System.Drawing.Point(475, 80);
            this.btnExtCharLGPDst.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtCharLGPDst.Name = "btnExtCharLGPDst";
            this.btnExtCharLGPDst.Size = new System.Drawing.Size(40, 25);
            this.btnExtCharLGPDst.TabIndex = 5;
            this.btnExtCharLGPDst.Text = "...";
            this.btnExtCharLGPDst.UseVisualStyleBackColor = true;
            this.btnExtCharLGPDst.Click += new System.EventHandler(this.btnExtCharLGPDst_Click);
            // 
            // btnExtBattleLGPSrc
            // 
            this.btnExtBattleLGPSrc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtBattleLGPSrc.Location = new System.Drawing.Point(475, 151);
            this.btnExtBattleLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtBattleLGPSrc.Name = "btnExtBattleLGPSrc";
            this.btnExtBattleLGPSrc.Size = new System.Drawing.Size(40, 25);
            this.btnExtBattleLGPSrc.TabIndex = 7;
            this.btnExtBattleLGPSrc.Text = "...";
            this.btnExtBattleLGPSrc.UseVisualStyleBackColor = true;
            this.btnExtBattleLGPSrc.Click += new System.EventHandler(this.btnExtBattleLGPSrc_Click);
            // 
            // btnExtBattleLGPDst
            // 
            this.btnExtBattleLGPDst.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtBattleLGPDst.Location = new System.Drawing.Point(475, 177);
            this.btnExtBattleLGPDst.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtBattleLGPDst.Name = "btnExtBattleLGPDst";
            this.btnExtBattleLGPDst.Size = new System.Drawing.Size(40, 25);
            this.btnExtBattleLGPDst.TabIndex = 9;
            this.btnExtBattleLGPDst.Text = "...";
            this.btnExtBattleLGPDst.UseVisualStyleBackColor = true;
            this.btnExtBattleLGPDst.Click += new System.EventHandler(this.btnExtBattleLGPDst_Click);
            // 
            // btnExtMagicLGPSrc
            // 
            this.btnExtMagicLGPSrc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtMagicLGPSrc.Location = new System.Drawing.Point(475, 240);
            this.btnExtMagicLGPSrc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtMagicLGPSrc.Name = "btnExtMagicLGPSrc";
            this.btnExtMagicLGPSrc.Size = new System.Drawing.Size(40, 25);
            this.btnExtMagicLGPSrc.TabIndex = 11;
            this.btnExtMagicLGPSrc.Text = "...";
            this.btnExtMagicLGPSrc.UseVisualStyleBackColor = true;
            this.btnExtMagicLGPSrc.Click += new System.EventHandler(this.btnExtMagicLGPSrc_Click);
            // 
            // btnExtMagicLGPDst
            // 
            this.btnExtMagicLGPDst.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExtMagicLGPDst.Location = new System.Drawing.Point(475, 266);
            this.btnExtMagicLGPDst.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtMagicLGPDst.Name = "btnExtMagicLGPDst";
            this.btnExtMagicLGPDst.Size = new System.Drawing.Size(40, 25);
            this.btnExtMagicLGPDst.TabIndex = 13;
            this.btnExtMagicLGPDst.Text = "...";
            this.btnExtMagicLGPDst.UseVisualStyleBackColor = true;
            this.btnExtMagicLGPDst.Click += new System.EventHandler(this.btnExtMagicLGPDst_Click);
            // 
            // chkProcessCharLGP
            // 
            this.chkProcessCharLGP.AutoSize = true;
            this.chkProcessCharLGP.Location = new System.Drawing.Point(695, 70);
            this.chkProcessCharLGP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkProcessCharLGP.Name = "chkProcessCharLGP";
            this.chkProcessCharLGP.Size = new System.Drawing.Size(18, 17);
            this.chkProcessCharLGP.TabIndex = 16;
            this.chkProcessCharLGP.UseVisualStyleBackColor = true;
            // 
            // chkProcessBattleLGP
            // 
            this.chkProcessBattleLGP.AutoSize = true;
            this.chkProcessBattleLGP.Location = new System.Drawing.Point(695, 171);
            this.chkProcessBattleLGP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkProcessBattleLGP.Name = "chkProcessBattleLGP";
            this.chkProcessBattleLGP.Size = new System.Drawing.Size(18, 17);
            this.chkProcessBattleLGP.TabIndex = 17;
            this.chkProcessBattleLGP.UseVisualStyleBackColor = true;
            // 
            // chkProcessMagicLGP
            // 
            this.chkProcessMagicLGP.AutoSize = true;
            this.chkProcessMagicLGP.Location = new System.Drawing.Point(695, 258);
            this.chkProcessMagicLGP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkProcessMagicLGP.Name = "chkProcessMagicLGP";
            this.chkProcessMagicLGP.Size = new System.Drawing.Size(18, 17);
            this.chkProcessMagicLGP.TabIndex = 18;
            this.chkProcessMagicLGP.UseVisualStyleBackColor = true;
            // 
            // lblSourcePathBattle
            // 
            this.lblSourcePathBattle.AutoSize = true;
            this.lblSourcePathBattle.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSourcePathBattle.Location = new System.Drawing.Point(35, 155);
            this.lblSourcePathBattle.Name = "lblSourcePathBattle";
            this.lblSourcePathBattle.Size = new System.Drawing.Size(89, 17);
            this.lblSourcePathBattle.TabIndex = 21;
            this.lblSourcePathBattle.Text = "Source path:";
            // 
            // lblDestinationPathBattle
            // 
            this.lblDestinationPathBattle.AutoSize = true;
            this.lblDestinationPathBattle.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDestinationPathBattle.Location = new System.Drawing.Point(9, 181);
            this.lblDestinationPathBattle.Name = "lblDestinationPathBattle";
            this.lblDestinationPathBattle.Size = new System.Drawing.Size(115, 17);
            this.lblDestinationPathBattle.TabIndex = 22;
            this.lblDestinationPathBattle.Text = "Destination path:";
            // 
            // lblSourcePathMagic
            // 
            this.lblSourcePathMagic.AutoSize = true;
            this.lblSourcePathMagic.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSourcePathMagic.Location = new System.Drawing.Point(35, 242);
            this.lblSourcePathMagic.Name = "lblSourcePathMagic";
            this.lblSourcePathMagic.Size = new System.Drawing.Size(89, 17);
            this.lblSourcePathMagic.TabIndex = 23;
            this.lblSourcePathMagic.Text = "Source path:";
            // 
            // lblDestinationPathMagic
            // 
            this.lblDestinationPathMagic.AutoSize = true;
            this.lblDestinationPathMagic.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDestinationPathMagic.Location = new System.Drawing.Point(9, 268);
            this.lblDestinationPathMagic.Name = "lblDestinationPathMagic";
            this.lblDestinationPathMagic.Size = new System.Drawing.Size(115, 17);
            this.lblDestinationPathMagic.TabIndex = 24;
            this.lblDestinationPathMagic.Text = "Destination path:";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(652, 42);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progBarIntAllAnim
            // 
            this.progBarIntAllAnim.ForeColor = System.Drawing.SystemColors.ControlText;
            this.progBarIntAllAnim.Location = new System.Drawing.Point(15, 42);
            this.progBarIntAllAnim.Margin = new System.Windows.Forms.Padding(4);
            this.progBarIntAllAnim.MarqueeAnimationSpeed = 10;
            this.progBarIntAllAnim.Name = "progBarIntAllAnim";
            this.progBarIntAllAnim.Size = new System.Drawing.Size(631, 30);
            this.progBarIntAllAnim.Step = 1;
            this.progBarIntAllAnim.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progBarIntAllAnim.TabIndex = 27;
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.btnSaveLog);
            this.gbSettings.Controls.Add(this.chkShowOnlyNoProcessed);
            this.gbSettings.Controls.Add(this.lblExtractedCharLGP);
            this.gbSettings.Controls.Add(this.lblExtractedBattleLGP);
            this.gbSettings.Controls.Add(this.lblExtractedMagicLGP);
            this.gbSettings.Controls.Add(this.lblSourcePathChar);
            this.gbSettings.Controls.Add(this.lblDestinationPathChar);
            this.gbSettings.Controls.Add(this.btnClose);
            this.gbSettings.Controls.Add(this.btnSaveOptions);
            this.gbSettings.Controls.Add(this.lblDestinationPathMagic);
            this.gbSettings.Controls.Add(this.btnGo);
            this.gbSettings.Controls.Add(this.lblNumInterpolatedFrames);
            this.gbSettings.Controls.Add(this.lblSourcePathMagic);
            this.gbSettings.Controls.Add(this.lblProcess);
            this.gbSettings.Controls.Add(this.lblDestinationPathBattle);
            this.gbSettings.Controls.Add(this.nudInterpFrameField);
            this.gbSettings.Controls.Add(this.lblSourcePathBattle);
            this.gbSettings.Controls.Add(this.nudInterpFrameBattleMagic);
            this.gbSettings.Controls.Add(this.chkProcessMagicLGP);
            this.gbSettings.Controls.Add(this.txtExtractedCharLGPSrc);
            this.gbSettings.Controls.Add(this.chkProcessBattleLGP);
            this.gbSettings.Controls.Add(this.txtExtractedBattleLGPSrc);
            this.gbSettings.Controls.Add(this.chkProcessCharLGP);
            this.gbSettings.Controls.Add(this.txtExtractedMagicLGPSrc);
            this.gbSettings.Controls.Add(this.btnExtMagicLGPDst);
            this.gbSettings.Controls.Add(this.txtExtractedCharLGPDest);
            this.gbSettings.Controls.Add(this.btnExtMagicLGPSrc);
            this.gbSettings.Controls.Add(this.txtExtractedBattleLGPDest);
            this.gbSettings.Controls.Add(this.btnExtBattleLGPDst);
            this.gbSettings.Controls.Add(this.txtExtractedMagicLGPDest);
            this.gbSettings.Controls.Add(this.btnExtBattleLGPSrc);
            this.gbSettings.Controls.Add(this.btnExtCharLGPSrc);
            this.gbSettings.Controls.Add(this.btnExtCharLGPDst);
            this.gbSettings.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSettings.Location = new System.Drawing.Point(11, 2);
            this.gbSettings.Margin = new System.Windows.Forms.Padding(4);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Padding = new System.Windows.Forms.Padding(4);
            this.gbSettings.Size = new System.Drawing.Size(747, 382);
            this.gbSettings.TabIndex = 29;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSaveLog.Location = new System.Drawing.Point(456, 335);
            this.btnSaveLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(105, 34);
            this.btnSaveLog.TabIndex = 26;
            this.btnSaveLog.Text = "Save log";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // chkShowOnlyNoProcessed
            // 
            this.chkShowOnlyNoProcessed.Location = new System.Drawing.Point(537, 299);
            this.chkShowOnlyNoProcessed.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowOnlyNoProcessed.Name = "chkShowOnlyNoProcessed";
            this.chkShowOnlyNoProcessed.Size = new System.Drawing.Size(195, 30);
            this.chkShowOnlyNoProcessed.TabIndex = 25;
            this.chkShowOnlyNoProcessed.Text = "Log only not processed";
            this.chkShowOnlyNoProcessed.UseVisualStyleBackColor = true;
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.lblProcessing);
            this.gbProgress.Controls.Add(this.progBarIntAllAnim);
            this.gbProgress.Controls.Add(this.btnCancel);
            this.gbProgress.Enabled = false;
            this.gbProgress.ForeColor = System.Drawing.SystemColors.Control;
            this.gbProgress.Location = new System.Drawing.Point(11, 385);
            this.gbProgress.Margin = new System.Windows.Forms.Padding(4);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Padding = new System.Windows.Forms.Padding(4);
            this.gbProgress.Size = new System.Drawing.Size(747, 82);
            this.gbProgress.TabIndex = 30;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Progress";
            // 
            // lblProcessing
            // 
            this.lblProcessing.Location = new System.Drawing.Point(456, 19);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(190, 23);
            this.lblProcessing.TabIndex = 28;
            this.lblProcessing.Text = "Processing... 0000  / 0000";
            this.lblProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(11, 475);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(745, 94);
            this.rtbLog.TabIndex = 31;
            this.rtbLog.Text = "";
            // 
            // frmInterpolateAll
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(768, 585);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.gbProgress);
            this.Controls.Add(this.gbSettings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "frmInterpolateAll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Interpolate All Animations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInterpolateAll_FormClosing);
            this.Load += new System.EventHandler(this.frmInterpolateAll_Load);
            this.Shown += new System.EventHandler(this.frmInterpolateAll_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpFrameField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpFrameBattleMagic)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblExtractedCharLGP;
        private System.Windows.Forms.Label lblExtractedBattleLGP;
        private System.Windows.Forms.Label lblExtractedMagicLGP;
        private System.Windows.Forms.Label lblSourcePathChar;
        private System.Windows.Forms.Label lblDestinationPathChar;
        private System.Windows.Forms.Label lblNumInterpolatedFrames;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnSaveOptions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.NumericUpDown nudInterpFrameField;
        private System.Windows.Forms.NumericUpDown nudInterpFrameBattleMagic;
        private System.Windows.Forms.TextBox txtExtractedCharLGPSrc;
        private System.Windows.Forms.TextBox txtExtractedBattleLGPSrc;
        private System.Windows.Forms.TextBox txtExtractedMagicLGPSrc;
        private System.Windows.Forms.TextBox txtExtractedCharLGPDest;
        private System.Windows.Forms.TextBox txtExtractedBattleLGPDest;
        private System.Windows.Forms.TextBox txtExtractedMagicLGPDest;
        private System.Windows.Forms.Button btnExtCharLGPSrc;
        private System.Windows.Forms.Button btnExtCharLGPDst;
        private System.Windows.Forms.Button btnExtBattleLGPSrc;
        private System.Windows.Forms.Button btnExtBattleLGPDst;
        private System.Windows.Forms.Button btnExtMagicLGPSrc;
        private System.Windows.Forms.Button btnExtMagicLGPDst;
        private System.Windows.Forms.CheckBox chkProcessCharLGP;
        private System.Windows.Forms.CheckBox chkProcessBattleLGP;
        private System.Windows.Forms.CheckBox chkProcessMagicLGP;
        private System.Windows.Forms.Label lblSourcePathBattle;
        private System.Windows.Forms.Label lblDestinationPathBattle;
        private System.Windows.Forms.Label lblSourcePathMagic;
        private System.Windows.Forms.Label lblDestinationPathMagic;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progBarIntAllAnim;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.CheckBox chkShowOnlyNoProcessed;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.Label lblProcessing;
    }
}