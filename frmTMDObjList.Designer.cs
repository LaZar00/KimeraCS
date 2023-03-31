
namespace KimeraCS
{
    partial class FrmTMDObjList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTMDObjList));
            this.lbTMDObjectList = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.btnCommitPModel = new System.Windows.Forms.Button();
            this.btnSaveTMD = new System.Windows.Forms.Button();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // lbTMDObjectList
            // 
            this.lbTMDObjectList.FormattingEnabled = true;
            this.lbTMDObjectList.Location = new System.Drawing.Point(4, 3);
            this.lbTMDObjectList.Name = "lbTMDObjectList";
            this.lbTMDObjectList.Size = new System.Drawing.Size(98, 355);
            this.lbTMDObjectList.TabIndex = 0;            
            this.lbTMDObjectList.DoubleClick += new System.EventHandler(this.LbTMDObjectList_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(4, 417);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 20);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(4, 398);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(98, 20);
            this.btnSaveLog.TabIndex = 3;
            this.btnSaveLog.Text = "Save Log";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.BtnSaveInfo_Click);
            // 
            // btnCommitPModel
            // 
            this.btnCommitPModel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCommitPModel.Location = new System.Drawing.Point(4, 360);
            this.btnCommitPModel.Name = "btnCommitPModel";
            this.btnCommitPModel.Size = new System.Drawing.Size(98, 20);
            this.btnCommitPModel.TabIndex = 1;
            this.btnCommitPModel.Text = "Commit P Model";
            this.btnCommitPModel.UseVisualStyleBackColor = true;
            this.btnCommitPModel.Click += new System.EventHandler(this.BtnCommitPModel_Click);
            // 
            // btnSaveTMD
            // 
            this.btnSaveTMD.Location = new System.Drawing.Point(4, 379);
            this.btnSaveTMD.Name = "btnSaveTMD";
            this.btnSaveTMD.Size = new System.Drawing.Size(98, 20);
            this.btnSaveTMD.TabIndex = 2;
            this.btnSaveTMD.Text = "Save TMD";
            this.btnSaveTMD.UseVisualStyleBackColor = true;
            this.btnSaveTMD.Click += new System.EventHandler(this.BtnSaveTMD_Click);
            // 
            // frmTMDObjList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(106, 441);
            this.Controls.Add(this.btnSaveTMD);
            this.Controls.Add(this.btnCommitPModel);
            this.Controls.Add(this.btnSaveLog);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbTMDObjectList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTMDObjList";
            this.ShowInTaskbar = false;
            this.Text = "TMD Obj List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTMDObjList_FormClosing);
            this.Load += new System.EventHandler(this.FrmTMDObjList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbTMDObjectList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.Button btnCommitPModel;
        private System.Windows.Forms.Button btnSaveTMD;
        private System.Windows.Forms.SaveFileDialog saveFile;
    }
}