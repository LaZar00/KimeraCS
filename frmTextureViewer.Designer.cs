
namespace KimeraCS
{
    partial class frmTextureViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTextureViewer));
            this.btnClose = new System.Windows.Forms.Button();
            this.panelTextureViewer = new System.Windows.Forms.Panel();
            this.pbTextureView = new System.Windows.Forms.PictureBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnRotateUV = new System.Windows.Forms.Button();
            this.btnFlipV = new System.Windows.Forms.Button();
            this.btnFlipH = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.CheckBox();
            this.panelTextureViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(373, 526);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 34);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelTextureViewer
            // 
            this.panelTextureViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTextureViewer.Controls.Add(this.pbTextureView);
            this.panelTextureViewer.Location = new System.Drawing.Point(4, 3);
            this.panelTextureViewer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelTextureViewer.Name = "panelTextureViewer";
            this.panelTextureViewer.Size = new System.Drawing.Size(520, 520);
            this.panelTextureViewer.TabIndex = 2;
            // 
            // pbTextureView
            // 
            this.pbTextureView.Location = new System.Drawing.Point(2, 2);
            this.pbTextureView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbTextureView.Name = "pbTextureView";
            this.pbTextureView.Size = new System.Drawing.Size(512, 512);
            this.pbTextureView.TabIndex = 1;
            this.pbTextureView.TabStop = false;
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(218, 526);
            this.btnCommit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(150, 34);
            this.btnCommit.TabIndex = 3;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // btnRotateUV
            // 
            this.btnRotateUV.BackgroundImage = global::KimeraCS.Properties.Resources.rotateUV_right;
            this.btnRotateUV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRotateUV.Location = new System.Drawing.Point(80, 526);
            this.btnRotateUV.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRotateUV.Name = "btnRotateUV";
            this.btnRotateUV.Size = new System.Drawing.Size(34, 34);
            this.btnRotateUV.TabIndex = 6;
            this.btnRotateUV.UseVisualStyleBackColor = true;
            this.btnRotateUV.Click += new System.EventHandler(this.btnRotateUV_Click);
            // 
            // btnFlipV
            // 
            this.btnFlipV.BackgroundImage = global::KimeraCS.Properties.Resources.flipUV_vertically;
            this.btnFlipV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipV.Location = new System.Drawing.Point(42, 526);
            this.btnFlipV.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFlipV.Name = "btnFlipV";
            this.btnFlipV.Size = new System.Drawing.Size(34, 34);
            this.btnFlipV.TabIndex = 5;
            this.btnFlipV.UseVisualStyleBackColor = true;
            this.btnFlipV.Click += new System.EventHandler(this.btnFlipV_Click);
            // 
            // btnFlipH
            // 
            this.btnFlipH.BackgroundImage = global::KimeraCS.Properties.Resources.flipUV_horizontally;
            this.btnFlipH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipH.Location = new System.Drawing.Point(4, 526);
            this.btnFlipH.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFlipH.Name = "btnFlipH";
            this.btnFlipH.Size = new System.Drawing.Size(34, 34);
            this.btnFlipH.TabIndex = 4;
            this.btnFlipH.UseVisualStyleBackColor = true;
            this.btnFlipH.Click += new System.EventHandler(this.btnFlipH_Click);
            // 
            // btnGreen
            // 
            this.btnGreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnGreen.BackColor = System.Drawing.Color.White;
            this.btnGreen.Location = new System.Drawing.Point(118, 526);
            this.btnGreen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(34, 34);
            this.btnGreen.TabIndex = 7;
            this.btnGreen.UseVisualStyleBackColor = false;
            this.btnGreen.CheckedChanged += new System.EventHandler(this.btnGreen_CheckedChanged);
            // 
            // frmTextureViewer
            // 
            this.AcceptButton = this.btnCommit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(526, 564);
            this.Controls.Add(this.btnGreen);
            this.Controls.Add(this.btnRotateUV);
            this.Controls.Add(this.btnFlipV);
            this.Controls.Add(this.btnFlipH);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.panelTextureViewer);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "frmTextureViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Texture Coordinates (UVs) Viewer";
            this.Load += new System.EventHandler(this.frmTextureViewer_Load);
            this.panelTextureViewer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelTextureViewer;
        private System.Windows.Forms.PictureBox pbTextureView;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnFlipH;
        private System.Windows.Forms.Button btnFlipV;
        private System.Windows.Forms.Button btnRotateUV;
        private System.Windows.Forms.CheckBox btnGreen;
    }
}