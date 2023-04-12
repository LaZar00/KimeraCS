
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
            this.rb25P = new System.Windows.Forms.RadioButton();
            this.rbSubstractive = new System.Windows.Forms.RadioButton();
            this.rbAdditive = new System.Windows.Forms.RadioButton();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.gbRenderState = new System.Windows.Forms.GroupBox();
            this.chkV_SHADEMODE_Lighted = new System.Windows.Forms.CheckBox();
            this.chkV_SHADEMODE = new System.Windows.Forms.CheckBox();
            this.chkV_ALPHABLEND_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_ALPHABLEND = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHMASK_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHMASK = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHTEST_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_DEPTHTEST = new System.Windows.Forms.CheckBox();
            this.chkV_CULLFACE_CullFrontFace = new System.Windows.Forms.CheckBox();
            this.chkV_CULLFACE = new System.Windows.Forms.CheckBox();
            this.chkV_NOCULL_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_NOCULL = new System.Windows.Forms.CheckBox();
            this.chkV_LINEARFILTER_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_LINEARFILTER = new System.Windows.Forms.CheckBox();
            this.chkV_TEXTURE_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_TEXTURE = new System.Windows.Forms.CheckBox();
            this.chkV_WIREFRAME_OPTION = new System.Windows.Forms.CheckBox();
            this.chkV_WIREFRAME = new System.Windows.Forms.CheckBox();
            this.lblRSValue = new System.Windows.Forms.Label();
            this.lblEnableRSChange = new System.Windows.Forms.Label();
            this.lblTextureID = new System.Windows.Forms.Label();
            this.nudTextureID = new System.Windows.Forms.NumericUpDown();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.gbPolygonType = new System.Windows.Forms.GroupBox();
            this.rb3 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.gbShadeMode = new System.Windows.Forms.GroupBox();
            this.rb2SM = new System.Windows.Forms.RadioButton();
            this.rb1SM = new System.Windows.Forms.RadioButton();
            this.lbSrcBlend = new System.Windows.Forms.Label();
            this.lbDestBlend = new System.Windows.Forms.Label();
            this.cbSrcBlend = new System.Windows.Forms.ComboBox();
            this.cbDestBlend = new System.Windows.Forms.ComboBox();
            this.gbBlending.SuspendLayout();
            this.gbRenderState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextureID)).BeginInit();
            this.gbPolygonType.SuspendLayout();
            this.gbShadeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBlending
            // 
            this.gbBlending.Controls.Add(this.rbNone);
            this.gbBlending.Controls.Add(this.rb25P);
            this.gbBlending.Controls.Add(this.rbSubstractive);
            this.gbBlending.Controls.Add(this.rbAdditive);
            this.gbBlending.Controls.Add(this.rbAverage);
            this.gbBlending.ForeColor = System.Drawing.SystemColors.Control;
            this.gbBlending.Location = new System.Drawing.Point(330, 12);
            this.gbBlending.Name = "gbBlending";
            this.gbBlending.Size = new System.Drawing.Size(140, 158);
            this.gbBlending.TabIndex = 0;
            this.gbBlending.TabStop = false;
            this.gbBlending.Text = "Blending Mode";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(17, 127);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 4;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rb25P
            // 
            this.rb25P.AutoSize = true;
            this.rb25P.Location = new System.Drawing.Point(17, 102);
            this.rb25P.Name = "rb25P";
            this.rb25P.Size = new System.Drawing.Size(93, 17);
            this.rb25P.TabIndex = 3;
            this.rb25P.TabStop = true;
            this.rb25P.Text = "25% (broken?)";
            this.rb25P.UseVisualStyleBackColor = true;
            // 
            // rbSubstractive
            // 
            this.rbSubstractive.AutoSize = true;
            this.rbSubstractive.Location = new System.Drawing.Point(17, 77);
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
            this.rbAdditive.Location = new System.Drawing.Point(17, 52);
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
            this.rbAverage.Location = new System.Drawing.Point(17, 27);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(65, 17);
            this.rbAverage.TabIndex = 0;
            this.rbAverage.TabStop = true;
            this.rbAverage.Text = "Average";
            this.rbAverage.UseVisualStyleBackColor = true;
            // 
            // gbRenderState
            // 
            this.gbRenderState.Controls.Add(this.chkV_SHADEMODE_Lighted);
            this.gbRenderState.Controls.Add(this.chkV_SHADEMODE);
            this.gbRenderState.Controls.Add(this.chkV_ALPHABLEND_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_ALPHABLEND);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHMASK_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHMASK);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHTEST_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_DEPTHTEST);
            this.gbRenderState.Controls.Add(this.chkV_CULLFACE_CullFrontFace);
            this.gbRenderState.Controls.Add(this.chkV_CULLFACE);
            this.gbRenderState.Controls.Add(this.chkV_NOCULL_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_NOCULL);
            this.gbRenderState.Controls.Add(this.chkV_LINEARFILTER_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_LINEARFILTER);
            this.gbRenderState.Controls.Add(this.chkV_TEXTURE_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_TEXTURE);
            this.gbRenderState.Controls.Add(this.chkV_WIREFRAME_OPTION);
            this.gbRenderState.Controls.Add(this.chkV_WIREFRAME);
            this.gbRenderState.Controls.Add(this.lblRSValue);
            this.gbRenderState.Controls.Add(this.lblEnableRSChange);
            this.gbRenderState.ForeColor = System.Drawing.SystemColors.Control;
            this.gbRenderState.Location = new System.Drawing.Point(12, 12);
            this.gbRenderState.Name = "gbRenderState";
            this.gbRenderState.Size = new System.Drawing.Size(312, 268);
            this.gbRenderState.TabIndex = 1;
            this.gbRenderState.TabStop = false;
            this.gbRenderState.Text = "Render State";
            // 
            // chkV_SHADEMODE_Lighted
            // 
            this.chkV_SHADEMODE_Lighted.AutoSize = true;
            this.chkV_SHADEMODE_Lighted.Location = new System.Drawing.Point(192, 241);
            this.chkV_SHADEMODE_Lighted.Name = "chkV_SHADEMODE_Lighted";
            this.chkV_SHADEMODE_Lighted.Size = new System.Drawing.Size(61, 17);
            this.chkV_SHADEMODE_Lighted.TabIndex = 19;
            this.chkV_SHADEMODE_Lighted.Text = "Lighted";
            this.chkV_SHADEMODE_Lighted.UseVisualStyleBackColor = true;
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
            // chkV_ALPHABLEND_OPTION
            // 
            this.chkV_ALPHABLEND_OPTION.AutoSize = true;
            this.chkV_ALPHABLEND_OPTION.Location = new System.Drawing.Point(192, 218);
            this.chkV_ALPHABLEND_OPTION.Name = "chkV_ALPHABLEND_OPTION";
            this.chkV_ALPHABLEND_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_ALPHABLEND_OPTION.TabIndex = 17;
            this.chkV_ALPHABLEND_OPTION.Text = "True";
            this.chkV_ALPHABLEND_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_DEPTHMASK_OPTION
            // 
            this.chkV_DEPTHMASK_OPTION.AutoSize = true;
            this.chkV_DEPTHMASK_OPTION.Location = new System.Drawing.Point(192, 196);
            this.chkV_DEPTHMASK_OPTION.Name = "chkV_DEPTHMASK_OPTION";
            this.chkV_DEPTHMASK_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_DEPTHMASK_OPTION.TabIndex = 15;
            this.chkV_DEPTHMASK_OPTION.Text = "True";
            this.chkV_DEPTHMASK_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_DEPTHTEST_OPTION
            // 
            this.chkV_DEPTHTEST_OPTION.AutoSize = true;
            this.chkV_DEPTHTEST_OPTION.Location = new System.Drawing.Point(192, 173);
            this.chkV_DEPTHTEST_OPTION.Name = "chkV_DEPTHTEST_OPTION";
            this.chkV_DEPTHTEST_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_DEPTHTEST_OPTION.TabIndex = 13;
            this.chkV_DEPTHTEST_OPTION.Text = "True";
            this.chkV_DEPTHTEST_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_CULLFACE_CullFrontFace
            // 
            this.chkV_CULLFACE_CullFrontFace.AutoSize = true;
            this.chkV_CULLFACE_CullFrontFace.Location = new System.Drawing.Point(192, 149);
            this.chkV_CULLFACE_CullFrontFace.Name = "chkV_CULLFACE_CullFrontFace";
            this.chkV_CULLFACE_CullFrontFace.Size = new System.Drawing.Size(97, 17);
            this.chkV_CULLFACE_CullFrontFace.TabIndex = 11;
            this.chkV_CULLFACE_CullFrontFace.Text = "Cull Front Face";
            this.chkV_CULLFACE_CullFrontFace.UseVisualStyleBackColor = true;
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
            // chkV_NOCULL_OPTION
            // 
            this.chkV_NOCULL_OPTION.AutoSize = true;
            this.chkV_NOCULL_OPTION.Location = new System.Drawing.Point(192, 126);
            this.chkV_NOCULL_OPTION.Name = "chkV_NOCULL_OPTION";
            this.chkV_NOCULL_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_NOCULL_OPTION.TabIndex = 9;
            this.chkV_NOCULL_OPTION.Text = "True";
            this.chkV_NOCULL_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_LINEARFILTER_OPTION
            // 
            this.chkV_LINEARFILTER_OPTION.AutoSize = true;
            this.chkV_LINEARFILTER_OPTION.Location = new System.Drawing.Point(192, 103);
            this.chkV_LINEARFILTER_OPTION.Name = "chkV_LINEARFILTER_OPTION";
            this.chkV_LINEARFILTER_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_LINEARFILTER_OPTION.TabIndex = 7;
            this.chkV_LINEARFILTER_OPTION.Text = "True";
            this.chkV_LINEARFILTER_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_TEXTURE_OPTION
            // 
            this.chkV_TEXTURE_OPTION.AutoSize = true;
            this.chkV_TEXTURE_OPTION.Location = new System.Drawing.Point(192, 80);
            this.chkV_TEXTURE_OPTION.Name = "chkV_TEXTURE_OPTION";
            this.chkV_TEXTURE_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_TEXTURE_OPTION.TabIndex = 5;
            this.chkV_TEXTURE_OPTION.Text = "True";
            this.chkV_TEXTURE_OPTION.UseVisualStyleBackColor = true;
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
            // chkV_WIREFRAME_OPTION
            // 
            this.chkV_WIREFRAME_OPTION.AutoSize = true;
            this.chkV_WIREFRAME_OPTION.Location = new System.Drawing.Point(192, 57);
            this.chkV_WIREFRAME_OPTION.Name = "chkV_WIREFRAME_OPTION";
            this.chkV_WIREFRAME_OPTION.Size = new System.Drawing.Size(48, 17);
            this.chkV_WIREFRAME_OPTION.TabIndex = 3;
            this.chkV_WIREFRAME_OPTION.Text = "True";
            this.chkV_WIREFRAME_OPTION.UseVisualStyleBackColor = true;
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
            this.lblRSValue.Size = new System.Drawing.Size(41, 13);
            this.lblRSValue.TabIndex = 1;
            this.lblRSValue.Text = "Option:";
            // 
            // lblEnableRSChange
            // 
            this.lblEnableRSChange.AutoSize = true;
            this.lblEnableRSChange.Location = new System.Drawing.Point(13, 30);
            this.lblEnableRSChange.Name = "lblEnableRSChange";
            this.lblEnableRSChange.Size = new System.Drawing.Size(46, 13);
            this.lblEnableRSChange.TabIndex = 0;
            this.lblEnableRSChange.Text = "Feature:";
            // 
            // lblTextureID
            // 
            this.lblTextureID.AutoSize = true;
            this.lblTextureID.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTextureID.Location = new System.Drawing.Point(19, 296);
            this.lblTextureID.Name = "lblTextureID";
            this.lblTextureID.Size = new System.Drawing.Size(60, 13);
            this.lblTextureID.TabIndex = 2;
            this.lblTextureID.Text = "Texture ID:";
            // 
            // nudTextureID
            // 
            this.nudTextureID.Location = new System.Drawing.Point(86, 294);
            this.nudTextureID.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudTextureID.Name = "nudTextureID";
            this.nudTextureID.Size = new System.Drawing.Size(57, 20);
            this.nudTextureID.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(250, 329);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(220, 24);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 329);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(220, 24);
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
            this.gbPolygonType.Location = new System.Drawing.Point(330, 285);
            this.gbPolygonType.Name = "gbPolygonType";
            this.gbPolygonType.Size = new System.Drawing.Size(140, 38);
            this.gbPolygonType.TabIndex = 7;
            this.gbPolygonType.TabStop = false;
            this.gbPolygonType.Text = "Polygon Type";
            // 
            // rb3
            // 
            this.rb3.AutoSize = true;
            this.rb3.Location = new System.Drawing.Point(101, 15);
            this.rb3.Name = "rb3";
            this.rb3.Size = new System.Drawing.Size(31, 17);
            this.rb3.TabIndex = 2;
            this.rb3.TabStop = true;
            this.rb3.Text = "3";
            this.rb3.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(56, 15);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(31, 17);
            this.rb2.TabIndex = 1;
            this.rb2.TabStop = true;
            this.rb2.Text = "2";
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(11, 15);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(31, 17);
            this.rb1.TabIndex = 0;
            this.rb1.TabStop = true;
            this.rb1.Text = "1";
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // gbShadeMode
            // 
            this.gbShadeMode.Controls.Add(this.rb2SM);
            this.gbShadeMode.Controls.Add(this.rb1SM);
            this.gbShadeMode.ForeColor = System.Drawing.SystemColors.Control;
            this.gbShadeMode.Location = new System.Drawing.Point(184, 285);
            this.gbShadeMode.Name = "gbShadeMode";
            this.gbShadeMode.Size = new System.Drawing.Size(140, 38);
            this.gbShadeMode.TabIndex = 8;
            this.gbShadeMode.TabStop = false;
            this.gbShadeMode.Text = "Shade Mode";
            // 
            // rb2SM
            // 
            this.rb2SM.AutoSize = true;
            this.rb2SM.Location = new System.Drawing.Point(88, 15);
            this.rb2SM.Name = "rb2SM";
            this.rb2SM.Size = new System.Drawing.Size(31, 17);
            this.rb2SM.TabIndex = 1;
            this.rb2SM.TabStop = true;
            this.rb2SM.Text = "2";
            this.rb2SM.UseVisualStyleBackColor = true;
            // 
            // rb1SM
            // 
            this.rb1SM.AutoSize = true;
            this.rb1SM.Location = new System.Drawing.Point(26, 15);
            this.rb1SM.Name = "rb1SM";
            this.rb1SM.Size = new System.Drawing.Size(31, 17);
            this.rb1SM.TabIndex = 0;
            this.rb1SM.TabStop = true;
            this.rb1SM.Text = "1";
            this.rb1SM.UseVisualStyleBackColor = true;
            // 
            // lbSrcBlend
            // 
            this.lbSrcBlend.AutoSize = true;
            this.lbSrcBlend.ForeColor = System.Drawing.SystemColors.Control;
            this.lbSrcBlend.Location = new System.Drawing.Point(330, 179);
            this.lbSrcBlend.Name = "lbSrcBlend";
            this.lbSrcBlend.Size = new System.Drawing.Size(74, 13);
            this.lbSrcBlend.TabIndex = 9;
            this.lbSrcBlend.Text = "Source Blend:";
            // 
            // lbDestBlend
            // 
            this.lbDestBlend.AutoSize = true;
            this.lbDestBlend.ForeColor = System.Drawing.SystemColors.Control;
            this.lbDestBlend.Location = new System.Drawing.Point(330, 225);
            this.lbDestBlend.Name = "lbDestBlend";
            this.lbDestBlend.Size = new System.Drawing.Size(93, 13);
            this.lbDestBlend.TabIndex = 10;
            this.lbDestBlend.Text = "Destination Blend:";
            // 
            // cbSrcBlend
            // 
            this.cbSrcBlend.DropDownWidth = 180;
            this.cbSrcBlend.Enabled = false;
            this.cbSrcBlend.FormattingEnabled = true;
            this.cbSrcBlend.Items.AddRange(new object[] {
            "GL_ZERO",
            "GL_ONE",
            "GL_SRC_COLOR",
            "GL_ONE_MINUS_SRC_COLOR",
            "GL_SRC_ALPHA",
            "GL_ONE_MINUS_SRC_ALPHA",
            "GL_DST_ALPHA",
            "GL_ONE_MINUS_DST_ALPHA",
            "GL_DST_COLOR",
            "GL_ONE_MINUS_DST_COLOR",
            "GL_SRC_ALPHA_SATURATE",
            "GL_CONSTANT_ALPHA",
            "GL_ONE_MINUS_CONSTANT_ALPHA"});
            this.cbSrcBlend.Location = new System.Drawing.Point(330, 194);
            this.cbSrcBlend.MaxDropDownItems = 9;
            this.cbSrcBlend.Name = "cbSrcBlend";
            this.cbSrcBlend.Size = new System.Drawing.Size(140, 21);
            this.cbSrcBlend.TabIndex = 11;
            // 
            // cbDestBlend
            // 
            this.cbDestBlend.DropDownWidth = 180;
            this.cbDestBlend.Enabled = false;
            this.cbDestBlend.FormattingEnabled = true;
            this.cbDestBlend.Items.AddRange(new object[] {
            "GL_ZERO",
            "GL_ONE",
            "GL_SRC_COLOR",
            "GL_ONE_MINUS_SRC_COLOR",
            "GL_SRC_ALPHA",
            "GL_ONE_MINUS_SRC_ALPHA",
            "GL_DST_ALPHA",
            "GL_ONE_MINUS_DST_ALPHA",
            "GL_DST_COLOR",
            "GL_ONE_MINUS_DST_COLOR",
            "GL_SRC_ALPHA_SATURATE",
            "GL_CONSTANT_ALPHA",
            "GL_ONE_MINUS_CONSTANT_ALPHA"});
            this.cbDestBlend.Location = new System.Drawing.Point(330, 240);
            this.cbDestBlend.MaxDropDownItems = 9;
            this.cbDestBlend.Name = "cbDestBlend";
            this.cbDestBlend.Size = new System.Drawing.Size(140, 21);
            this.cbDestBlend.TabIndex = 12;
            // 
            // FrmGroupProperties
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(482, 364);
            this.Controls.Add(this.cbDestBlend);
            this.Controls.Add(this.cbSrcBlend);
            this.Controls.Add(this.lbDestBlend);
            this.Controls.Add(this.lbSrcBlend);
            this.Controls.Add(this.gbShadeMode);
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
            this.Name = "FrmGroupProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Group Properties";
            this.gbBlending.ResumeLayout(false);
            this.gbBlending.PerformLayout();
            this.gbRenderState.ResumeLayout(false);
            this.gbRenderState.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTextureID)).EndInit();
            this.gbPolygonType.ResumeLayout(false);
            this.gbPolygonType.PerformLayout();
            this.gbShadeMode.ResumeLayout(false);
            this.gbShadeMode.PerformLayout();
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
        private System.Windows.Forms.RadioButton rb25P;
        private System.Windows.Forms.RadioButton rbSubstractive;
        private System.Windows.Forms.RadioButton rbAdditive;
        private System.Windows.Forms.RadioButton rbAverage;
        private System.Windows.Forms.Label lblRSValue;
        private System.Windows.Forms.Label lblEnableRSChange;
        private System.Windows.Forms.CheckBox chkV_SHADEMODE_Lighted;
        private System.Windows.Forms.CheckBox chkV_SHADEMODE;
        private System.Windows.Forms.CheckBox chkV_ALPHABLEND_OPTION;
        private System.Windows.Forms.CheckBox chkV_ALPHABLEND;
        private System.Windows.Forms.CheckBox chkV_DEPTHMASK_OPTION;
        private System.Windows.Forms.CheckBox chkV_DEPTHMASK;
        private System.Windows.Forms.CheckBox chkV_DEPTHTEST_OPTION;
        private System.Windows.Forms.CheckBox chkV_DEPTHTEST;
        private System.Windows.Forms.CheckBox chkV_CULLFACE_CullFrontFace;
        private System.Windows.Forms.CheckBox chkV_CULLFACE;
        private System.Windows.Forms.CheckBox chkV_NOCULL_OPTION;
        private System.Windows.Forms.CheckBox chkV_NOCULL;
        private System.Windows.Forms.CheckBox chkV_LINEARFILTER_OPTION;
        private System.Windows.Forms.CheckBox chkV_LINEARFILTER;
        private System.Windows.Forms.CheckBox chkV_TEXTURE_OPTION;
        private System.Windows.Forms.CheckBox chkV_TEXTURE;
        private System.Windows.Forms.CheckBox chkV_WIREFRAME_OPTION;
        private System.Windows.Forms.CheckBox chkV_WIREFRAME;
        private System.Windows.Forms.GroupBox gbPolygonType;
        private System.Windows.Forms.RadioButton rb3;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.GroupBox gbShadeMode;
        private System.Windows.Forms.RadioButton rb2SM;
        private System.Windows.Forms.RadioButton rb1SM;
        private System.Windows.Forms.Label lbSrcBlend;
        private System.Windows.Forms.Label lbDestBlend;
        private System.Windows.Forms.ComboBox cbSrcBlend;
        private System.Windows.Forms.ComboBox cbDestBlend;
    }
}