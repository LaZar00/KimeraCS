
namespace KimeraCS
{
    partial class FrmSkeletonJoints
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSkeletonJoints));
            this.label1 = new System.Windows.Forms.Label();
            this.cbBoneAttachedTo = new System.Windows.Forms.ComboBox();
            this.lbLength = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lbBoneAttaches = new System.Windows.Forms.Label();
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtBoneName = new System.Windows.Forms.TextBox();
            this.btnPathRSDFile = new System.Windows.Forms.Button();
            this.lbRSDFile = new System.Windows.Forms.Label();
            this.txtRSDFile = new System.Windows.Forms.TextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(49, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bone Attached to:";
            // 
            // cbBoneAttachedTo
            // 
            this.cbBoneAttachedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoneAttachedTo.FormattingEnabled = true;
            this.cbBoneAttachedTo.Location = new System.Drawing.Point(148, 21);
            this.cbBoneAttachedTo.Name = "cbBoneAttachedTo";
            this.cbBoneAttachedTo.Size = new System.Drawing.Size(164, 21);
            this.cbBoneAttachedTo.TabIndex = 2;
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.ForeColor = System.Drawing.SystemColors.Control;
            this.lbLength.Location = new System.Drawing.Point(99, 72);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(43, 13);
            this.lbLength.TabIndex = 3;
            this.lbLength.Text = "Length:";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(148, 69);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(164, 20);
            this.txtLength.TabIndex = 4;
            // 
            // lbBoneAttaches
            // 
            this.lbBoneAttaches.AutoSize = true;
            this.lbBoneAttaches.ForeColor = System.Drawing.SystemColors.Control;
            this.lbBoneAttaches.Location = new System.Drawing.Point(76, 48);
            this.lbBoneAttaches.Name = "lbBoneAttaches";
            this.lbBoneAttaches.Size = new System.Drawing.Size(66, 13);
            this.lbBoneAttaches.TabIndex = 5;
            this.lbBoneAttaches.Text = "Bone Name:";
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(14, 149);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(138, 24);
            this.btnCommit.TabIndex = 7;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(208, 149);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 24);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtBoneName
            // 
            this.txtBoneName.Location = new System.Drawing.Point(148, 45);
            this.txtBoneName.Name = "txtBoneName";
            this.txtBoneName.Size = new System.Drawing.Size(164, 20);
            this.txtBoneName.TabIndex = 9;
            // 
            // btnPathRSDFile
            // 
            this.btnPathRSDFile.Location = new System.Drawing.Point(321, 113);
            this.btnPathRSDFile.Name = "btnPathRSDFile";
            this.btnPathRSDFile.Size = new System.Drawing.Size(25, 23);
            this.btnPathRSDFile.TabIndex = 10;
            this.btnPathRSDFile.Text = "...";
            this.btnPathRSDFile.UseVisualStyleBackColor = true;
            this.btnPathRSDFile.Click += new System.EventHandler(this.BtnPathRSDFile_Click);
            // 
            // lbRSDFile
            // 
            this.lbRSDFile.AutoSize = true;
            this.lbRSDFile.ForeColor = System.Drawing.SystemColors.Control;
            this.lbRSDFile.Location = new System.Drawing.Point(12, 99);
            this.lbRSDFile.Name = "lbRSDFile";
            this.lbRSDFile.Size = new System.Drawing.Size(49, 13);
            this.lbRSDFile.TabIndex = 11;
            this.lbRSDFile.Text = "RSD File";
            // 
            // txtRSDFile
            // 
            this.txtRSDFile.Location = new System.Drawing.Point(14, 115);
            this.txtRSDFile.Name = "txtRSDFile";
            this.txtRSDFile.Size = new System.Drawing.Size(301, 20);
            this.txtRSDFile.TabIndex = 12;
            // 
            // frmSkeletonJoints
            // 
            this.AcceptButton = this.btnCommit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(358, 185);
            this.Controls.Add(this.txtRSDFile);
            this.Controls.Add(this.lbRSDFile);
            this.Controls.Add(this.btnPathRSDFile);
            this.Controls.Add(this.txtBoneName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.lbBoneAttaches);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.cbBoneAttachedTo);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSkeletonJoints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Skeleton Joints Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSkeletonJoints_FormClosing);
            this.Load += new System.EventHandler(this.FrmSkeletonJoints_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBoneAttachedTo;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label lbBoneAttaches;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtBoneName;
        private System.Windows.Forms.Button btnPathRSDFile;
        private System.Windows.Forms.Label lbRSDFile;
        private System.Windows.Forms.TextBox txtRSDFile;
        private System.Windows.Forms.OpenFileDialog openFile;
    }
}