
namespace KimeraCS
{
    partial class FrmGroupProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGroupProperties));
            this.gbBlending = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbUnknown = new System.Windows.Forms.RadioButton();
            this.rbSubstractive = new System.Windows.Forms.RadioButton();
            this.rbAdditive = new System.Windows.Forms.RadioButton();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.gbRenderState = new System.Windows.Forms.GroupBox();
            this.chkLighted = new System.Windows.Forms.CheckBox();
            this.chkV_SHADEMODE = new System.Windows.Forms.CheckBox();
            this.chkTrueV_ALPHA = new System.Windows.Forms.CheckBox();
            this.chkV_ALPHABLEND = new System.Windows.Forms.CheckBox();
            this.chkTrueV_DEPTHMASK = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHMASK = new System.Windows.Forms.CheckBox();
            this.chkTrueV_DEPTHTEST = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHTEST = new System.Windows.Forms.CheckBox();
            this.chkCullBackFacing = new System.Windows.Forms.CheckBox();
            this.chkV_CULLFACE = new System.Windows.Forms.CheckBox();
            this.chkTrueV_NOCULL = new System.Windows.Forms.CheckBox();
            this.chkV_NOCULL = new System.Windows.Forms.CheckBox();
            this.chkTrueV_LINEAR = new System.Windows.Forms.CheckBox();
            this.chkV_LINEARFILTER = new System.Windows.Forms.CheckBox();
            this.chkTrueV_TEX = new System.Windows.Forms.CheckBox();
            this.chkV_TEXTURE = new System.Windows.Forms.CheckBox();
            this.chkTrueV_WIRE = new System.Windows.Forms.CheckBox();
            this.chkV_WIREFRAME = new System.Windows.Forms.CheckBox();
            this.lblRSValue = new System.Windows.Forms.Label();
            this.lblEnableRSChange = new System.Windows.Forms.Label();
            this.lblTextureID = new System.Windows.Forms.Label();
            this.nudTextureID = new System.Windows.Forms.NumericUpDown();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.gbPolygonType = new System.Windows.Forms.GroupBox();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.gbBlending.SuspendLayout();
            this.gbRenderState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextureID)).BeginInit();
            this.gbPolygonType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBlending
            // 
            this.gbBlending.Controls.Add(this.rbNone);
            this.gbBlending.Controls.Add(this.rbUnknown);
            this.gbBlending.Controls.Add(this.rbSubstractive);
            this.gbBlending.Controls.Add(this.rbAdditive);
            this.gbBlending.Controls.Add(this.rbAverage);
            this.gbBlending.ForeColor = System.Drawing.SystemColors.Control;
            this.gbBlending.Location = new System.Drawing.Point(12, 12);
            this.gbBlending.Name = "gbBlending";
            this.gbBlending.Size = new System.Drawing.Size(312, 71);
            this.gbBlending.TabIndex = 0;
            this.gbBlending.TabStop = false;
            this.gbBlending.Text = "Blending Mode";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(213, 45);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 4;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbUnknown
            // 
            this.rbUnknown.AutoSize = true;
            this.rbUnknown.Location = new System.Drawing.Point(17, 45);
            this.rbUnknown.Name = "rbUnknown";
            this.rbUnknown.Size = new System.Drawing.Size(119, 17);
            this.rbUnknown.TabIndex = 3;
            this.rbUnknown.TabStop = true;
            this.rbUnknown.Text = "Unknown (broken?)";
            this.rbUnknown.UseVisualStyleBackColor = true;
            // 
            // rbSubstractive
            // 
            this.rbSubstractive.AutoSize = true;
            this.rbSubstractive.Location = new System.Drawing.Point(213, 23);
            this.rbSubstractive.Name = "rbSubstractive";
            this.rbSubstractive.Size = new System.Drawing.Size(84, 17);
            this.rbSubstractive.TabIndex = 2;
            this.rbSubstractive.TabStop = true;
            this.rbSubstractive.Text = "Substractive";
            this.rbSubstractive.UseVisualStyleBackColor = true;
            // 
            // rbAdditive
            // 
            this.rbAdditive.AutoSize = true;
            this.rbAdditive.Location = new System.Drawing.Point(120, 23);
            this.rbAdditive.Name = "rbAdditive";
            this.rbAdditive.Size = new System.Drawing.Size(63, 17);
            this.rbAdditive.TabIndex = 1;
            this.rbAdditive.TabStop = true;
            this.rbAdditive.Text = "Additive";
            this.rbAdditive.UseVisualStyleBackColor = true;
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Location = new System.Drawing.Point(17, 23);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(65, 17);
            this.rbAverage.TabIndex = 0;
            this.rbAverage.TabStop = true;
            this.rbAverage.Text = "Average";
            this.rbAverage.UseVisualStyleBackColor = true;
            // 
            // gbRenderState
            // 
            this.gbRenderState.Controls.Add(this.chkLighted);
            this.gbRenderState.Controls.Add(this.chkV_SHADEMODE);
            this.gbRenderState.Controls.Add(this.chkTrueV_ALPHA);
            this.gbRenderState.Controls.Add(this.chkV_ALPHABLEND);
            this.gbRenderState.Controls.Add(this.chkTrueV_DEPTHMASK);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHMASK);
            this.gbRenderState.Controls.Add(this.chkTrueV_DEPTHTEST);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHTEST);
            this.gbRenderState.Controls.Add(this.chkCullBackFacing);
            this.gbRenderState.Controls.Add(this.chkV_CULLFACE);
            this.gbRenderState.Controls.Add(this.chkTrueV_NOCULL);
            this.gbRenderState.Controls.Add(this.chkV_NOCULL);
            this.gbRenderState.Controls.Add(this.chkTrueV_LINEAR);
            this.gbRenderState.Controls.Add(this.chkV_LINEARFILTER);
            this.gbRenderState.Controls.Add(this.chkTrueV_TEX);
            this.gbRenderState.Controls.Add(this.chkV_TEXTURE);
            this.gbRenderState.Controls.Add(this.chkTrueV_WIRE);
            this.gbRenderState.Controls.Add(this.chkV_WIREFRAME);
            this.gbRenderState.Controls.Add(this.lblRSValue);
            this.gbRenderState.Controls.Add(this.lblEnableRSChange);
            this.gbRenderState.ForeColor = System.Drawing.SystemColors.Control;
            this.gbRenderState.Location = new System.Drawing.Point(12, 89);
            this.gbRenderState.Name = "gbRenderState";
            this.gbRenderState.Size = new System.Drawing.Size(312, 268);
            this.gbRenderState.TabIndex = 1;
            this.gbRenderState.TabStop = false;
            this.gbRenderState.Text = "Render State";
            // 
            // chkLighted
            // 
            this.chkLighted.AutoSize = true;
            this.chkLighted.Location = new System.Drawing.Point(192, 241);
            this.chkLighted.Name = "chkLighted";
            this.chkLighted.Size = new System.Drawing.Size(61, 17);
            this.chkLighted.TabIndex = 19;
            this.chkLighted.Text = "Lighted";
            this.chkLighted.UseVisualStyleBackColor = true;
            // 
            // chkV_SHADEMODE
            // 
            this.chkV_SHADEMODE.AutoSize = true;
            this.chkV_SHADEMODE.Location = new System.Drawing.Point(26, 241);
            this.chkV_SHADEMODE.Name = "chkV_SHADEMODE";
            this.chkV_SHADEMODE.Size = new System.Drawing.Size(108, 17);
            this.chkV_SHADEMODE.TabIndex = 18;
            this.chkV_SHADEMODE.Text = "V_SHADEMODE";
            this.chkV_SHADEMODE.UseVisualStyleBackColor = true;
            this.chkV_SHADEMODE.CheckedChanged += new System.EventHandler(this.ChkV_SHADEMODE_CheckedChanged);
            // 
            // chkTrueV_ALPHA
            // 
            this.chkTrueV_ALPHA.AutoSize = true;
            this.chkTrueV_ALPHA.Location = new System.Drawing.Point(192, 218);
            this.chkTrueV_ALPHA.Name = "chkTrueV_ALPHA";
            this.chkTrueV_ALPHA.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_ALPHA.TabIndex = 17;
            this.chkTrueV_ALPHA.Text = "True";
            this.chkTrueV_ALPHA.UseVisualStyleBackColor = true;
            // 
            // chkV_ALPHABLEND
            // 
            this.chkV_ALPHABLEND.AutoSize = true;
            this.chkV_ALPHABLEND.Location = new System.Drawing.Point(26, 218);
            this.chkV_ALPHABLEND.Name = "chkV_ALPHABLEND";
            this.chkV_ALPHABLEND.Size = new System.Drawing.Size(110, 17);
            this.chkV_ALPHABLEND.TabIndex = 16;
            this.chkV_ALPHABLEND.Text = "V_ALPHABLEND";
            this.chkV_ALPHABLEND.UseVisualStyleBackColor = true;
            this.chkV_ALPHABLEND.CheckedChanged += new System.EventHandler(this.ChkV_ALPHABLEND_CheckedChanged);
            // 
            // chkTrueV_DEPTHMASK
            // 
            this.chkTrueV_DEPTHMASK.AutoSize = true;
            this.chkTrueV_DEPTHMASK.Location = new System.Drawing.Point(192, 196);
            this.chkTrueV_DEPTHMASK.Name = "chkTrueV_DEPTHMASK";
            this.chkTrueV_DEPTHMASK.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_DEPTHMASK.TabIndex = 15;
            this.chkTrueV_DEPTHMASK.Text = "True";
            this.chkTrueV_DEPTHMASK.UseVisualStyleBackColor = true;
            // 
            // chkV_DEPTHMASK
            // 
            this.chkV_DEPTHMASK.AutoSize = true;
            this.chkV_DEPTHMASK.Location = new System.Drawing.Point(26, 196);
            this.chkV_DEPTHMASK.Name = "chkV_DEPTHMASK";
            this.chkV_DEPTHMASK.Size = new System.Drawing.Size(106, 17);
            this.chkV_DEPTHMASK.TabIndex = 14;
            this.chkV_DEPTHMASK.Text = "V_DEPTHMASK";
            this.chkV_DEPTHMASK.UseVisualStyleBackColor = true;
            this.chkV_DEPTHMASK.CheckedChanged += new System.EventHandler(this.ChkV_DEPTHMASK_CheckedChanged);
            // 
            // chkTrueV_DEPTHTEST
            // 
            this.chkTrueV_DEPTHTEST.AutoSize = true;
            this.chkTrueV_DEPTHTEST.Location = new System.Drawing.Point(192, 173);
            this.chkTrueV_DEPTHTEST.Name = "chkTrueV_DEPTHTEST";
            this.chkTrueV_DEPTHTEST.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_DEPTHTEST.TabIndex = 13;
            this.chkTrueV_DEPTHTEST.Text = "True";
            this.chkTrueV_DEPTHTEST.UseVisualStyleBackColor = true;
            // 
            // chkV_DEPTHTEST
            // 
            this.chkV_DEPTHTEST.AutoSize = true;
            this.chkV_DEPTHTEST.Location = new System.Drawing.Point(26, 173);
            this.chkV_DEPTHTEST.Name = "chkV_DEPTHTEST";
            this.chkV_DEPTHTEST.Size = new System.Drawing.Size(104, 17);
            this.chkV_DEPTHTEST.TabIndex = 12;
            this.chkV_DEPTHTEST.Text = "V_DEPTHTEST";
            this.chkV_DEPTHTEST.UseVisualStyleBackColor = true;
            this.chkV_DEPTHTEST.CheckedChanged += new System.EventHandler(this.ChkV_DEPTHTEST_CheckedChanged);
            // 
            // chkCullBackFacing
            // 
            this.chkCullBackFacing.AutoSize = true;
            this.chkCullBackFacing.Location = new System.Drawing.Point(192, 149);
            this.chkCullBackFacing.Name = "chkCullBackFacing";
            this.chkCullBackFacing.Size = new System.Drawing.Size(102, 17);
            this.chkCullBackFacing.TabIndex = 11;
            this.chkCullBackFacing.Text = "Cull back-facing";
            this.chkCullBackFacing.UseVisualStyleBackColor = true;
            // 
            // chkV_CULLFACE
            // 
            this.chkV_CULLFACE.AutoSize = true;
            this.chkV_CULLFACE.Location = new System.Drawing.Point(26, 149);
            this.chkV_CULLFACE.Name = "chkV_CULLFACE";
            this.chkV_CULLFACE.Size = new System.Drawing.Size(93, 17);
            this.chkV_CULLFACE.TabIndex = 10;
            this.chkV_CULLFACE.Text = "V_CULLFACE";
            this.chkV_CULLFACE.UseVisualStyleBackColor = true;
            this.chkV_CULLFACE.CheckedChanged += new System.EventHandler(this.ChkV_CULLFACE_CheckedChanged);
            // 
            // chkTrueV_NOCULL
            // 
            this.chkTrueV_NOCULL.AutoSize = true;
            this.chkTrueV_NOCULL.Location = new System.Drawing.Point(192, 126);
            this.chkTrueV_NOCULL.Name = "chkTrueV_NOCULL";
            this.chkTrueV_NOCULL.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_NOCULL.TabIndex = 9;
            this.chkTrueV_NOCULL.Text = "True";
            this.chkTrueV_NOCULL.UseVisualStyleBackColor = true;
            // 
            // chkV_NOCULL
            // 
            this.chkV_NOCULL.AutoSize = true;
            this.chkV_NOCULL.Location = new System.Drawing.Point(26, 126);
            this.chkV_NOCULL.Name = "chkV_NOCULL";
            this.chkV_NOCULL.Size = new System.Drawing.Size(82, 17);
            this.chkV_NOCULL.TabIndex = 8;
            this.chkV_NOCULL.Text = "V_NOCULL";
            this.chkV_NOCULL.UseVisualStyleBackColor = true;
            this.chkV_NOCULL.CheckedChanged += new System.EventHandler(this.ChkV_NOCULL_CheckedChanged);
            // 
            // chkTrueV_LINEAR
            // 
            this.chkTrueV_LINEAR.AutoSize = true;
            this.chkTrueV_LINEAR.Location = new System.Drawing.Point(192, 103);
            this.chkTrueV_LINEAR.Name = "chkTrueV_LINEAR";
            this.chkTrueV_LINEAR.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_LINEAR.TabIndex = 7;
            this.chkTrueV_LINEAR.Text = "True";
            this.chkTrueV_LINEAR.UseVisualStyleBackColor = true;
            // 
            // chkV_LINEARFILTER
            // 
            this.chkV_LINEARFILTER.AutoSize = true;
            this.chkV_LINEARFILTER.Location = new System.Drawing.Point(26, 103);
            this.chkV_LINEARFILTER.Name = "chkV_LINEARFILTER";
            this.chkV_LINEARFILTER.Size = new System.Drawing.Size(115, 17);
            this.chkV_LINEARFILTER.TabIndex = 6;
            this.chkV_LINEARFILTER.Text = "V_LINEARFILTER";
            this.chkV_LINEARFILTER.UseVisualStyleBackColor = true;
            this.chkV_LINEARFILTER.CheckedChanged += new System.EventHandler(this.ChkV_LINEARFILTER_CheckedChanged);
            // 
            // chkTrueV_TEX
            // 
            this.chkTrueV_TEX.AutoSize = true;
            this.chkTrueV_TEX.Location = new System.Drawing.Point(192, 80);
            this.chkTrueV_TEX.Name = "chkTrueV_TEX";
            this.chkTrueV_TEX.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_TEX.TabIndex = 5;
            this.chkTrueV_TEX.Text = "True";
            this.chkTrueV_TEX.UseVisualStyleBackColor = true;
            // 
            // chkV_TEXTURE
            // 
            this.chkV_TEXTURE.AutoSize = true;
            this.chkV_TEXTURE.Location = new System.Drawing.Point(26, 80);
            this.chkV_TEXTURE.Name = "chkV_TEXTURE";
            this.chkV_TEXTURE.Size = new System.Drawing.Size(90, 17);
            this.chkV_TEXTURE.TabIndex = 4;
            this.chkV_TEXTURE.Text = "V_TEXTURE";
            this.chkV_TEXTURE.UseVisualStyleBackColor = true;
            this.chkV_TEXTURE.CheckedChanged += new System.EventHandler(this.ChkV_TEXTURE_CheckedChanged);
            // 
            // chkTrueV_WIRE
            // 
            this.chkTrueV_WIRE.AutoSize = true;
            this.chkTrueV_WIRE.Location = new System.Drawing.Point(192, 57);
            this.chkTrueV_WIRE.Name = "chkTrueV_WIRE";
            this.chkTrueV_WIRE.Size = new System.Drawing.Size(48, 17);
            this.chkTrueV_WIRE.TabIndex = 3;
            this.chkTrueV_WIRE.Text = "True";
            this.chkTrueV_WIRE.UseVisualStyleBackColor = true;
            // 
            // chkV_WIREFRAME
            // 
            this.chkV_WIREFRAME.AutoSize = true;
            this.chkV_WIREFRAME.Location = new System.Drawing.Point(26, 57);
            this.chkV_WIREFRAME.Name = "chkV_WIREFRAME";
            this.chkV_WIREFRAME.Size = new System.Drawing.Size(105, 17);
            this.chkV_WIREFRAME.TabIndex = 2;
            this.chkV_WIREFRAME.Text = "V_WIREFRAME";
            this.chkV_WIREFRAME.UseVisualStyleBackColor = true;
            this.chkV_WIREFRAME.CheckedChanged += new System.EventHandler(this.ChkV_WIREFRAME_CheckedChanged);
            // 
            // lblRSValue
            // 
            this.lblRSValue.AutoSize = true;
            this.lblRSValue.Location = new System.Drawing.Point(179, 30);
            this.lblRSValue.Name = "lblRSValue";
            this.lblRSValue.Size = new System.Drawing.Size(55, 13);
            this.lblRSValue.TabIndex = 1;
            this.lblRSValue.Text = "RS Value:";
            // 
            // lblEnableRSChange
            // 
            this.lblEnableRSChange.AutoSize = true;
            this.lblEnableRSChange.Location = new System.Drawing.Point(13, 30);
            this.lblEnableRSChange.Name = "lblEnableRSChange";
            this.lblEnableRSChange.Size = new System.Drawing.Size(101, 13);
            this.lblEnableRSChange.TabIndex = 0;
            this.lblEnableRSChange.Text = "Enable RS Change:";
            // 
            // lblTextureID
            // 
            this.lblTextureID.AutoSize = true;
            this.lblTextureID.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTextureID.Location = new System.Drawing.Point(17, 376);
            this.lblTextureID.Name = "lblTextureID";
            this.lblTextureID.Size = new System.Drawing.Size(60, 13);
            this.lblTextureID.TabIndex = 2;
            this.lblTextureID.Text = "Texture ID:";
            // 
            // nudTextureID
            // 
            this.nudTextureID.Location = new System.Drawing.Point(83, 374);
            this.nudTextureID.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudTextureID.Name = "nudTextureID";
            this.nudTextureID.Size = new System.Drawing.Size(60, 20);
            this.nudTextureID.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(176, 411);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(148, 24);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 411);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(148, 24);
            this.btnAccept.TabIndex = 5;
            this.btnAccept.Text = "Accept changes";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
            // 
            // gbPolygonType
            // 
            this.gbPolygonType.Controls.Add(this.rb3);
            this.gbPolygonType.Controls.Add(this.rb2);
            this.gbPolygonType.Controls.Add(this.rb1);
            this.gbPolygonType.ForeColor = System.Drawing.SystemColors.Control;
            this.gbPolygonType.Location = new System.Drawing.Point(163, 357);
            this.gbPolygonType.Name = "gbPolygonType";
            this.gbPolygonType.Size = new System.Drawing.Size(161, 48);
            this.gbPolygonType.TabIndex = 7;
            this.gbPolygonType.TabStop = false;
            this.gbPolygonType.Text = "Polygon Type";
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(13, 20);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(31, 17);
            this.rb1.TabIndex = 0;
            this.rb1.TabStop = true;
            this.rb1.Text = "1";
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(67, 20);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(31, 17);
            this.rb2.TabIndex = 1;
            this.rb2.TabStop = true;
            this.rb2.Text = "2";
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(121, 20);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(31, 17);
            this.rb3.TabIndex = 2;
            this.rb3.TabStop = true;
            this.rb3.Text = "3";
            this.rb3.UseVisualStyleBackColor = true;
            // 
            // FrmGroupProperties
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(336, 442);
            this.Controls.Add(this.gbPolygonType);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.nudTextureID);
            this.Controls.Add(this.lblTextureID);
            this.Controls.Add(this.gbRenderState);
            this.Controls.Add(this.gbBlending);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmGroupProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Group Properties";
            this.gbBlending.ResumeLayout(false);
            this.gbBlending.PerformLayout();
            this.gbRenderState.ResumeLayout(false);
            this.gbRenderState.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextureID)).EndInit();
            this.gbPolygonType.ResumeLayout(false);
            this.gbPolygonType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBlending;
        private System.Windows.Forms.GroupBox gbRenderState;
        private System.Windows.Forms.Label lblTextureID;
        private System.Windows.Forms.NumericUpDown nudTextureID;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbUnknown;
        private System.Windows.Forms.RadioButton rbSubstractive;
        private System.Windows.Forms.RadioButton rbAdditive;
        private System.Windows.Forms.RadioButton rbAverage;
        private System.Windows.Forms.Label lblRSValue;
        private System.Windows.Forms.Label lblEnableRSChange;
        private System.Windows.Forms.CheckBox chkLighted;
        private System.Windows.Forms.CheckBox chkV_SHADEMODE;
        private System.Windows.Forms.CheckBox chkTrueV_ALPHA;
        private System.Windows.Forms.CheckBox chkV_ALPHABLEND;
        private System.Windows.Forms.CheckBox chkTrueV_DEPTHMASK;
        private System.Windows.Forms.CheckBox chkV_DEPTHMASK;
        private System.Windows.Forms.CheckBox chkTrueV_DEPTHTEST;
        private System.Windows.Forms.CheckBox chkV_DEPTHTEST;
        private System.Windows.Forms.CheckBox chkCullBackFacing;
        private System.Windows.Forms.CheckBox chkV_CULLFACE;
        private System.Windows.Forms.CheckBox chkTrueV_NOCULL;
        private System.Windows.Forms.CheckBox chkV_NOCULL;
        private System.Windows.Forms.CheckBox chkTrueV_LINEAR;
        private System.Windows.Forms.CheckBox chkV_LINEARFILTER;
        private System.Windows.Forms.CheckBox chkTrueV_TEX;
        private System.Windows.Forms.CheckBox chkV_TEXTURE;
        private System.Windows.Forms.CheckBox chkTrueV_WIRE;
        private System.Windows.Forms.CheckBox chkV_WIREFRAME;
        private System.Windows.Forms.GroupBox gbPolygonType;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rb1;
    }
}