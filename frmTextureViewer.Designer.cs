
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
            this.panelTextureViewer = new System.Windows.Forms.Panel();
            this.pbTextureView = new System.Windows.Forms.PictureBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.CheckBox();
            this.btnRotateUV = new System.Windows.Forms.Button();
            this.btnFlipV = new System.Windows.Forms.Button();
            this.btnFlipH = new System.Windows.Forms.Button();
            this.btnCommit = new System.Windows.Forms.Button();
            this.panelTextureViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureView)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTextureViewer
            // 
            this.panelTextureViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTextureViewer.AutoScroll = true;
            this.panelTextureViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTextureViewer.Controls.Add(this.pbTextureView);
            this.panelTextureViewer.Location = new System.Drawing.Point(5, 4);
            this.panelTextureViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTextureViewer.Name = "panelTextureViewer";
            this.panelTextureViewer.Size = new System.Drawing.Size(692, 639);
            this.panelTextureViewer.TabIndex = 2;
            // 
            // pbTextureView
            // 
            this.pbTextureView.Location = new System.Drawing.Point(3, 2);
            this.pbTextureView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbTextureView.Name = "pbTextureView";
            this.pbTextureView.Size = new System.Drawing.Size(683, 630);
            this.pbTextureView.TabIndex = 1;
            this.pbTextureView.TabStop = false;
            this.pbTextureView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTextureView_MouseDown);
            this.pbTextureView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTextureView_MouseMove);
            this.pbTextureView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTextureView_MouseUp);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnZoomIn);
            this.panelButtons.Controls.Add(this.btnZoomOut);
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnGreen);
            this.panelButtons.Controls.Add(this.btnRotateUV);
            this.panelButtons.Controls.Add(this.btnFlipV);
            this.panelButtons.Controls.Add(this.btnFlipH);
            this.panelButtons.Controls.Add(this.btnCommit);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(3, 649);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(4);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(695, 47);
            this.panelButtons.TabIndex = 9;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackgroundImage = global::KimeraCS.Properties.Resources.UV_zoomin;
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnZoomIn.Location = new System.Drawing.Point(240, 2);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(45, 42);
            this.btnZoomIn.TabIndex = 17;
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackgroundImage = global::KimeraCS.Properties.Resources.UV_zoomout;
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnZoomOut.Enabled = false;
            this.btnZoomOut.Location = new System.Drawing.Point(193, 2);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(45, 42);
            this.btnZoomOut.TabIndex = 16;
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(519, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(173, 42);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGreen
            // 
            this.btnGreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnGreen.BackColor = System.Drawing.Color.White;
            this.btnGreen.Location = new System.Drawing.Point(143, 2);
            this.btnGreen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(45, 42);
            this.btnGreen.TabIndex = 13;
            this.btnGreen.UseVisualStyleBackColor = false;
            this.btnGreen.CheckedChanged += new System.EventHandler(this.btnGreen_CheckedChanged);
            // 
            // btnRotateUV
            // 
            this.btnRotateUV.BackgroundImage = global::KimeraCS.Properties.Resources.rotateUV_right;
            this.btnRotateUV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRotateUV.Location = new System.Drawing.Point(96, 2);
            this.btnRotateUV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRotateUV.Name = "btnRotateUV";
            this.btnRotateUV.Size = new System.Drawing.Size(45, 42);
            this.btnRotateUV.TabIndex = 12;
            this.btnRotateUV.UseVisualStyleBackColor = true;
            this.btnRotateUV.Click += new System.EventHandler(this.btnRotateUV_Click);
            // 
            // btnFlipV
            // 
            this.btnFlipV.BackgroundImage = global::KimeraCS.Properties.Resources.flipUV_vertically;
            this.btnFlipV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipV.Location = new System.Drawing.Point(49, 2);
            this.btnFlipV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFlipV.Name = "btnFlipV";
            this.btnFlipV.Size = new System.Drawing.Size(45, 42);
            this.btnFlipV.TabIndex = 11;
            this.btnFlipV.UseVisualStyleBackColor = true;
            this.btnFlipV.Click += new System.EventHandler(this.btnFlipV_Click);
            // 
            // btnFlipH
            // 
            this.btnFlipH.BackgroundImage = global::KimeraCS.Properties.Resources.flipUV_horizontally;
            this.btnFlipH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipH.Location = new System.Drawing.Point(3, 2);
            this.btnFlipH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFlipH.Name = "btnFlipH";
            this.btnFlipH.Size = new System.Drawing.Size(45, 42);
            this.btnFlipH.TabIndex = 10;
            this.btnFlipH.UseVisualStyleBackColor = true;
            this.btnFlipH.Click += new System.EventHandler(this.btnFlipH_Click);
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCommit.AutoSize = true;
            this.btnCommit.Location = new System.Drawing.Point(340, 2);
            this.btnCommit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(173, 42);
            this.btnCommit.TabIndex = 9;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // frmTextureViewer
            // 
            this.AcceptButton = this.btnCommit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(701, 698);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelTextureViewer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(717, 735);
            this.Name = "frmTextureViewer";
            this.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Texture Coordinates (UVs) Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTextureViewer_FormClosed);
            this.Load += new System.EventHandler(this.frmTextureViewer_Load);
            this.panelTextureViewer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureView)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTextureViewer;
        private System.Windows.Forms.PictureBox pbTextureView;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox btnGreen;
        private System.Windows.Forms.Button btnRotateUV;
        private System.Windows.Forms.Button btnFlipV;
        private System.Windows.Forms.Button btnFlipH;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
    }
}