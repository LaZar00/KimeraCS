using System;
using System.Timers;

namespace KimeraCS
{
    partial class frmSkeletonEditor
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSkeletonEditor));
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbTexturesFrame = new System.Windows.Forms.GroupBox();
            this.nUDMoveTextureUpDown = new System.Windows.Forms.NumericUpDown();
            this.btnRemoveTexture = new System.Windows.Forms.Button();
            this.btnAddTexture = new System.Windows.Forms.Button();
            this.btnChangeTexture = new System.Windows.Forms.Button();
            this.btnFlipHorizontal = new System.Windows.Forms.Button();
            this.btnFlipVertical = new System.Windows.Forms.Button();
            this.btnRotate = new System.Windows.Forms.Button();
            this.chkZeroAsTransparent = new System.Windows.Forms.CheckBox();
            this.cbTextureSelect = new System.Windows.Forms.ComboBox();
            this.pbTextureViewer = new System.Windows.Forms.PictureBox();
            this.gbAnimationOptionsFrame = new System.Windows.Forms.GroupBox();
            this.btnInterpolateFrame = new System.Windows.Forms.Button();
            this.btnDuplicateFrame = new System.Windows.Forms.Button();
            this.btnRemoveFrame = new System.Windows.Forms.Button();
            this.chkPropagateChangesForward = new System.Windows.Forms.CheckBox();
            this.gbFrameDataPartOptions = new System.Windows.Forms.GroupBox();
            this.nUDZAnimationFramePart = new System.Windows.Forms.NumericUpDown();
            this.nUDYAnimationFramePart = new System.Windows.Forms.NumericUpDown();
            this.nUDXAnimationFramePart = new System.Windows.Forms.NumericUpDown();
            this.lblZAnimationFramePart = new System.Windows.Forms.Label();
            this.lblYAnimationFramePart = new System.Windows.Forms.Label();
            this.lblXAnimationFramePart = new System.Windows.Forms.Label();
            this.nUDFrameDataPart = new System.Windows.Forms.NumericUpDown();
            this.lblFrameOptionsPart = new System.Windows.Forms.Label();
            this.gbSelectedBoneFrame = new System.Windows.Forms.GroupBox();
            this.txtBoneOptionsLength = new System.Windows.Forms.TextBox();
            this.btnRemovePiece = new System.Windows.Forms.Button();
            this.btnAddPiece = new System.Windows.Forms.Button();
            this.nUDBoneOptionsLength = new System.Windows.Forms.NumericUpDown();
            this.lblBoneOptionsLength = new System.Windows.Forms.Label();
            this.nUDResizeBoneZ = new System.Windows.Forms.NumericUpDown();
            this.lblZScaleBoneOptions = new System.Windows.Forms.Label();
            this.nUDResizeBoneY = new System.Windows.Forms.NumericUpDown();
            this.lblYScaleBoneOptions = new System.Windows.Forms.Label();
            this.nUDResizeBoneX = new System.Windows.Forms.NumericUpDown();
            this.lblXScaleBoneOptions = new System.Windows.Forms.Label();
            this.lblBoneSelector = new System.Windows.Forms.Label();
            this.cbBoneSelector = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkInifintyFarLights = new System.Windows.Forms.CheckBox();
            this.chkRightLight = new System.Windows.Forms.CheckBox();
            this.chkLeftLight = new System.Windows.Forms.CheckBox();
            this.chkRearLight = new System.Windows.Forms.CheckBox();
            this.chkFrontLight = new System.Windows.Forms.CheckBox();
            this.lblLightPosZ = new System.Windows.Forms.Label();
            this.hsbLightPosZ = new System.Windows.Forms.HScrollBar();
            this.lblLightPosY = new System.Windows.Forms.Label();
            this.hsbLightPosY = new System.Windows.Forms.HScrollBar();
            this.lblLightPosX = new System.Windows.Forms.Label();
            this.hsbLightPosX = new System.Windows.Forms.HScrollBar();
            this.cbWeapon = new System.Windows.Forms.ComboBox();
            this.cbBattleAnimation = new System.Windows.Forms.ComboBox();
            this.lblWeapon = new System.Windows.Forms.Label();
            this.lblBattleAnimation = new System.Windows.Forms.Label();
            this.btnComputeWeaponPosition = new System.Windows.Forms.Button();
            this.btnComputeGroundHeight = new System.Windows.Forms.Button();
            this.chkShowLastFrameGhost = new System.Windows.Forms.CheckBox();
            this.chkShowGround = new System.Windows.Forms.CheckBox();
            this.gbSelectedPieceFrame = new System.Windows.Forms.GroupBox();
            this.gbRotateFrame = new System.Windows.Forms.GroupBox();
            this.txtRotateGamma = new System.Windows.Forms.TextBox();
            this.hsbRotateGamma = new System.Windows.Forms.HScrollBar();
            this.lblRotateGamma = new System.Windows.Forms.Label();
            this.txtRotateBeta = new System.Windows.Forms.TextBox();
            this.hsbRotateBeta = new System.Windows.Forms.HScrollBar();
            this.lblRotateBeta = new System.Windows.Forms.Label();
            this.lblRotateAlpha = new System.Windows.Forms.Label();
            this.txtRotateAlpha = new System.Windows.Forms.TextBox();
            this.hsbRotateAlpha = new System.Windows.Forms.HScrollBar();
            this.gbRepositionFrame = new System.Windows.Forms.GroupBox();
            this.txtRepositionZ = new System.Windows.Forms.TextBox();
            this.hsbRepositionZ = new System.Windows.Forms.HScrollBar();
            this.lblRepositionZ = new System.Windows.Forms.Label();
            this.txtRepositionY = new System.Windows.Forms.TextBox();
            this.hsbRepositionY = new System.Windows.Forms.HScrollBar();
            this.lblRepositionY = new System.Windows.Forms.Label();
            this.lblRepositionX = new System.Windows.Forms.Label();
            this.txtRepositionX = new System.Windows.Forms.TextBox();
            this.hsbRepositionX = new System.Windows.Forms.HScrollBar();
            this.gbResizeFrame = new System.Windows.Forms.GroupBox();
            this.txtResizePieceZ = new System.Windows.Forms.TextBox();
            this.hsbResizePieceZ = new System.Windows.Forms.HScrollBar();
            this.lblResizePieceZ = new System.Windows.Forms.Label();
            this.txtResizePieceY = new System.Windows.Forms.TextBox();
            this.hsbResizePieceY = new System.Windows.Forms.HScrollBar();
            this.lblResizePieceY = new System.Windows.Forms.Label();
            this.lblResizePieceX = new System.Windows.Forms.Label();
            this.txtResizePieceX = new System.Windows.Forms.TextBox();
            this.hsbResizePieceX = new System.Windows.Forms.HScrollBar();
            this.chkDListEnable = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnFramePrev = new System.Windows.Forms.Button();
            this.btnFrameNext = new System.Windows.Forms.Button();
            this.btnPlayStopAnim = new System.Windows.Forms.CheckBox();
            this.btnFrameEnd = new System.Windows.Forms.Button();
            this.btnFrameBegin = new System.Windows.Forms.Button();
            this.chkShowBones = new System.Windows.Forms.CheckBox();
            this.btnInterpolateAnimation = new System.Windows.Forms.Button();
            this.txtCopyPasteFrame = new System.Windows.Forms.TextBox();
            this.btnPasteFrame = new System.Windows.Forms.Button();
            this.btnCopyFrame = new System.Windows.Forms.Button();
            this.tbCurrentFrameScroll = new System.Windows.Forms.TrackBar();
            this.lblAnimationFrame = new System.Windows.Forms.Label();
            this.txtAnimationFrame = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFieldSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBattleMagicSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.load3DSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSkeletonAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.extiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.uIOpacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity100 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity90 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity75 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity50 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity25 = new System.Windows.Forms.ToolStripMenuItem();
            this.animationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFieldAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBattleMagicLimitsAnimationStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAnimationAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS60 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.interpolateAllAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCharlgpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.panelModel = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.gbTexturesFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDMoveTextureUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureViewer)).BeginInit();
            this.gbAnimationOptionsFrame.SuspendLayout();
            this.gbFrameDataPartOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDZAnimationFramePart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDYAnimationFramePart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXAnimationFramePart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDFrameDataPart)).BeginInit();
            this.gbSelectedBoneFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDBoneOptionsLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneX)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbSelectedPieceFrame.SuspendLayout();
            this.gbRotateFrame.SuspendLayout();
            this.gbRepositionFrame.SuspendLayout();
            this.gbResizeFrame.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCurrentFrameScroll)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelModel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.gbTexturesFrame);
            this.panel1.Controls.Add(this.gbAnimationOptionsFrame);
            this.panel1.Controls.Add(this.gbSelectedBoneFrame);
            this.panel1.Controls.Add(this.lblBoneSelector);
            this.panel1.Controls.Add(this.cbBoneSelector);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(766, 28);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(213, 771);
            this.panel1.TabIndex = 6;
            // 
            // gbTexturesFrame
            // 
            this.gbTexturesFrame.Controls.Add(this.nUDMoveTextureUpDown);
            this.gbTexturesFrame.Controls.Add(this.btnRemoveTexture);
            this.gbTexturesFrame.Controls.Add(this.btnAddTexture);
            this.gbTexturesFrame.Controls.Add(this.btnChangeTexture);
            this.gbTexturesFrame.Controls.Add(this.btnFlipHorizontal);
            this.gbTexturesFrame.Controls.Add(this.btnFlipVertical);
            this.gbTexturesFrame.Controls.Add(this.btnRotate);
            this.gbTexturesFrame.Controls.Add(this.chkZeroAsTransparent);
            this.gbTexturesFrame.Controls.Add(this.cbTextureSelect);
            this.gbTexturesFrame.Controls.Add(this.pbTextureViewer);
            this.gbTexturesFrame.Enabled = false;
            this.gbTexturesFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbTexturesFrame.Location = new System.Drawing.Point(5, 238);
            this.gbTexturesFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbTexturesFrame.Name = "gbTexturesFrame";
            this.gbTexturesFrame.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbTexturesFrame.Size = new System.Drawing.Size(203, 274);
            this.gbTexturesFrame.TabIndex = 15;
            this.gbTexturesFrame.TabStop = false;
            this.gbTexturesFrame.Text = "Textures (Part)";
            this.gbTexturesFrame.Visible = false;
            // 
            // nUDMoveTextureUpDown
            // 
            this.nUDMoveTextureUpDown.Location = new System.Drawing.Point(11, 150);
            this.nUDMoveTextureUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDMoveTextureUpDown.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nUDMoveTextureUpDown.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
            this.nUDMoveTextureUpDown.Name = "nUDMoveTextureUpDown";
            this.nUDMoveTextureUpDown.Size = new System.Drawing.Size(24, 22);
            this.nUDMoveTextureUpDown.TabIndex = 9;
            this.nUDMoveTextureUpDown.ValueChanged += new System.EventHandler(this.nUDMoveTextureUpDown_ValueChanged);
            // 
            // btnRemoveTexture
            // 
            this.btnRemoveTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemoveTexture.Location = new System.Drawing.Point(4, 245);
            this.btnRemoveTexture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoveTexture.Name = "btnRemoveTexture";
            this.btnRemoveTexture.Size = new System.Drawing.Size(195, 26);
            this.btnRemoveTexture.TabIndex = 8;
            this.btnRemoveTexture.Text = "Remove Texture";
            this.btnRemoveTexture.UseVisualStyleBackColor = true;
            this.btnRemoveTexture.Click += new System.EventHandler(this.btnRemoveTexture_Click);
            // 
            // btnAddTexture
            // 
            this.btnAddTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTexture.Location = new System.Drawing.Point(4, 220);
            this.btnAddTexture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddTexture.Name = "btnAddTexture";
            this.btnAddTexture.Size = new System.Drawing.Size(195, 26);
            this.btnAddTexture.TabIndex = 7;
            this.btnAddTexture.Text = "Add Texture";
            this.btnAddTexture.UseVisualStyleBackColor = true;
            this.btnAddTexture.Click += new System.EventHandler(this.btnAddTexture_Click);
            // 
            // btnChangeTexture
            // 
            this.btnChangeTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnChangeTexture.Location = new System.Drawing.Point(4, 196);
            this.btnChangeTexture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChangeTexture.Name = "btnChangeTexture";
            this.btnChangeTexture.Size = new System.Drawing.Size(195, 26);
            this.btnChangeTexture.TabIndex = 6;
            this.btnChangeTexture.Text = "Change Texture";
            this.btnChangeTexture.UseVisualStyleBackColor = true;
            this.btnChangeTexture.Click += new System.EventHandler(this.btnChangeTexture_Click);
            // 
            // btnFlipHorizontal
            // 
            this.btnFlipHorizontal.BackgroundImage = global::KimeraCS.Properties.Resources.mirror_vertically;
            this.btnFlipHorizontal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipHorizontal.Location = new System.Drawing.Point(149, 106);
            this.btnFlipHorizontal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFlipHorizontal.Name = "btnFlipHorizontal";
            this.btnFlipHorizontal.Size = new System.Drawing.Size(43, 39);
            this.btnFlipHorizontal.TabIndex = 5;
            this.btnFlipHorizontal.UseVisualStyleBackColor = true;
            this.btnFlipHorizontal.Click += new System.EventHandler(this.btnFlipHorizontal_Click);
            // 
            // btnFlipVertical
            // 
            this.btnFlipVertical.BackgroundImage = global::KimeraCS.Properties.Resources.mirror_horizontally;
            this.btnFlipVertical.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipVertical.Location = new System.Drawing.Point(151, 64);
            this.btnFlipVertical.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFlipVertical.Name = "btnFlipVertical";
            this.btnFlipVertical.Size = new System.Drawing.Size(43, 39);
            this.btnFlipVertical.TabIndex = 4;
            this.btnFlipVertical.UseVisualStyleBackColor = true;
            this.btnFlipVertical.Click += new System.EventHandler(this.btnFlipVertical_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.BackgroundImage = global::KimeraCS.Properties.Resources.arrow_circle2;
            this.btnRotate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRotate.Location = new System.Drawing.Point(151, 22);
            this.btnRotate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(43, 39);
            this.btnRotate.TabIndex = 3;
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // chkZeroAsTransparent
            // 
            this.chkZeroAsTransparent.AutoSize = true;
            this.chkZeroAsTransparent.Location = new System.Drawing.Point(35, 177);
            this.chkZeroAsTransparent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkZeroAsTransparent.Name = "chkZeroAsTransparent";
            this.chkZeroAsTransparent.Size = new System.Drawing.Size(134, 21);
            this.chkZeroAsTransparent.TabIndex = 2;
            this.chkZeroAsTransparent.Text = "0 as transparent";
            this.chkZeroAsTransparent.UseVisualStyleBackColor = true;
            this.chkZeroAsTransparent.Click += new System.EventHandler(this.chkZeroAsTransparent_Click);
            // 
            // cbTextureSelect
            // 
            this.cbTextureSelect.FormattingEnabled = true;
            this.cbTextureSelect.Location = new System.Drawing.Point(37, 150);
            this.cbTextureSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbTextureSelect.Name = "cbTextureSelect";
            this.cbTextureSelect.Size = new System.Drawing.Size(152, 24);
            this.cbTextureSelect.TabIndex = 1;
            this.cbTextureSelect.SelectedIndexChanged += new System.EventHandler(this.cbTextureSelect_SelectedIndexChanged);
            this.cbTextureSelect.Click += new System.EventHandler(this.cbTextureSelect_Click);
            // 
            // pbTextureViewer
            // 
            this.pbTextureViewer.BackColor = System.Drawing.Color.White;
            this.pbTextureViewer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbTextureViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbTextureViewer.Location = new System.Drawing.Point(11, 20);
            this.pbTextureViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbTextureViewer.Name = "pbTextureViewer";
            this.pbTextureViewer.Size = new System.Drawing.Size(137, 127);
            this.pbTextureViewer.TabIndex = 0;
            this.pbTextureViewer.TabStop = false;
            // 
            // gbAnimationOptionsFrame
            // 
            this.gbAnimationOptionsFrame.Controls.Add(this.btnInterpolateFrame);
            this.gbAnimationOptionsFrame.Controls.Add(this.btnDuplicateFrame);
            this.gbAnimationOptionsFrame.Controls.Add(this.btnRemoveFrame);
            this.gbAnimationOptionsFrame.Controls.Add(this.chkPropagateChangesForward);
            this.gbAnimationOptionsFrame.Controls.Add(this.gbFrameDataPartOptions);
            this.gbAnimationOptionsFrame.Controls.Add(this.nUDFrameDataPart);
            this.gbAnimationOptionsFrame.Controls.Add(this.lblFrameOptionsPart);
            this.gbAnimationOptionsFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbAnimationOptionsFrame.Location = new System.Drawing.Point(8, 519);
            this.gbAnimationOptionsFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbAnimationOptionsFrame.Name = "gbAnimationOptionsFrame";
            this.gbAnimationOptionsFrame.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbAnimationOptionsFrame.Size = new System.Drawing.Size(203, 247);
            this.gbAnimationOptionsFrame.TabIndex = 14;
            this.gbAnimationOptionsFrame.TabStop = false;
            this.gbAnimationOptionsFrame.Text = "Frame options";
            this.gbAnimationOptionsFrame.Visible = false;
            // 
            // btnInterpolateFrame
            // 
            this.btnInterpolateFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInterpolateFrame.Location = new System.Drawing.Point(4, 218);
            this.btnInterpolateFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInterpolateFrame.Name = "btnInterpolateFrame";
            this.btnInterpolateFrame.Size = new System.Drawing.Size(195, 26);
            this.btnInterpolateFrame.TabIndex = 9;
            this.btnInterpolateFrame.Text = "Interpolate Frame";
            this.btnInterpolateFrame.UseVisualStyleBackColor = true;
            this.btnInterpolateFrame.Click += new System.EventHandler(this.btnInterpolateFrame_Click);
            // 
            // btnDuplicateFrame
            // 
            this.btnDuplicateFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDuplicateFrame.Location = new System.Drawing.Point(4, 193);
            this.btnDuplicateFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDuplicateFrame.Name = "btnDuplicateFrame";
            this.btnDuplicateFrame.Size = new System.Drawing.Size(195, 26);
            this.btnDuplicateFrame.TabIndex = 8;
            this.btnDuplicateFrame.Text = "Duplicate Frame";
            this.btnDuplicateFrame.UseVisualStyleBackColor = true;
            this.btnDuplicateFrame.Click += new System.EventHandler(this.btnDuplicateFrame_Click);
            // 
            // btnRemoveFrame
            // 
            this.btnRemoveFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemoveFrame.Location = new System.Drawing.Point(4, 169);
            this.btnRemoveFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemoveFrame.Name = "btnRemoveFrame";
            this.btnRemoveFrame.Size = new System.Drawing.Size(195, 26);
            this.btnRemoveFrame.TabIndex = 7;
            this.btnRemoveFrame.Text = "Remove Frame";
            this.btnRemoveFrame.UseVisualStyleBackColor = true;
            this.btnRemoveFrame.Click += new System.EventHandler(this.btnRemoveFrame_Click);
            // 
            // chkPropagateChangesForward
            // 
            this.chkPropagateChangesForward.AutoSize = true;
            this.chkPropagateChangesForward.Checked = true;
            this.chkPropagateChangesForward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPropagateChangesForward.Location = new System.Drawing.Point(29, 150);
            this.chkPropagateChangesForward.Margin = new System.Windows.Forms.Padding(4);
            this.chkPropagateChangesForward.Name = "chkPropagateChangesForward";
            this.chkPropagateChangesForward.Size = new System.Drawing.Size(147, 21);
            this.chkPropagateChangesForward.TabIndex = 3;
            this.chkPropagateChangesForward.Text = "Propagate forward";
            this.chkPropagateChangesForward.UseVisualStyleBackColor = true;
            // 
            // gbFrameDataPartOptions
            // 
            this.gbFrameDataPartOptions.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gbFrameDataPartOptions.Controls.Add(this.nUDZAnimationFramePart);
            this.gbFrameDataPartOptions.Controls.Add(this.nUDYAnimationFramePart);
            this.gbFrameDataPartOptions.Controls.Add(this.nUDXAnimationFramePart);
            this.gbFrameDataPartOptions.Controls.Add(this.lblZAnimationFramePart);
            this.gbFrameDataPartOptions.Controls.Add(this.lblYAnimationFramePart);
            this.gbFrameDataPartOptions.Controls.Add(this.lblXAnimationFramePart);
            this.gbFrameDataPartOptions.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.gbFrameDataPartOptions.Location = new System.Drawing.Point(12, 42);
            this.gbFrameDataPartOptions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbFrameDataPartOptions.Name = "gbFrameDataPartOptions";
            this.gbFrameDataPartOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbFrameDataPartOptions.Size = new System.Drawing.Size(177, 107);
            this.gbFrameDataPartOptions.TabIndex = 2;
            this.gbFrameDataPartOptions.TabStop = false;
            this.gbFrameDataPartOptions.Text = "Bone rotation";
            // 
            // nUDZAnimationFramePart
            // 
            this.nUDZAnimationFramePart.DecimalPlaces = 6;
            this.nUDZAnimationFramePart.Location = new System.Drawing.Point(51, 75);
            this.nUDZAnimationFramePart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDZAnimationFramePart.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDZAnimationFramePart.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDZAnimationFramePart.Name = "nUDZAnimationFramePart";
            this.nUDZAnimationFramePart.Size = new System.Drawing.Size(117, 22);
            this.nUDZAnimationFramePart.TabIndex = 10;
            this.nUDZAnimationFramePart.ValueChanged += new System.EventHandler(this.nUDZAnimationFramePart_ValueChanged);
            // 
            // nUDYAnimationFramePart
            // 
            this.nUDYAnimationFramePart.DecimalPlaces = 6;
            this.nUDYAnimationFramePart.Location = new System.Drawing.Point(51, 48);
            this.nUDYAnimationFramePart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDYAnimationFramePart.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDYAnimationFramePart.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDYAnimationFramePart.Name = "nUDYAnimationFramePart";
            this.nUDYAnimationFramePart.Size = new System.Drawing.Size(117, 22);
            this.nUDYAnimationFramePart.TabIndex = 9;
            this.nUDYAnimationFramePart.ValueChanged += new System.EventHandler(this.nUDYAnimationFramePart_ValueChanged);
            // 
            // nUDXAnimationFramePart
            // 
            this.nUDXAnimationFramePart.DecimalPlaces = 6;
            this.nUDXAnimationFramePart.Location = new System.Drawing.Point(51, 21);
            this.nUDXAnimationFramePart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDXAnimationFramePart.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDXAnimationFramePart.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDXAnimationFramePart.Name = "nUDXAnimationFramePart";
            this.nUDXAnimationFramePart.Size = new System.Drawing.Size(117, 22);
            this.nUDXAnimationFramePart.TabIndex = 8;
            this.nUDXAnimationFramePart.ValueChanged += new System.EventHandler(this.nUDXAnimationFramePart_ValueChanged);
            // 
            // lblZAnimationFramePart
            // 
            this.lblZAnimationFramePart.AutoSize = true;
            this.lblZAnimationFramePart.Location = new System.Drawing.Point(21, 78);
            this.lblZAnimationFramePart.Name = "lblZAnimationFramePart";
            this.lblZAnimationFramePart.Size = new System.Drawing.Size(17, 17);
            this.lblZAnimationFramePart.TabIndex = 7;
            this.lblZAnimationFramePart.Text = "Z";
            // 
            // lblYAnimationFramePart
            // 
            this.lblYAnimationFramePart.AutoSize = true;
            this.lblYAnimationFramePart.Location = new System.Drawing.Point(21, 50);
            this.lblYAnimationFramePart.Name = "lblYAnimationFramePart";
            this.lblYAnimationFramePart.Size = new System.Drawing.Size(17, 17);
            this.lblYAnimationFramePart.TabIndex = 6;
            this.lblYAnimationFramePart.Text = "Y";
            // 
            // lblXAnimationFramePart
            // 
            this.lblXAnimationFramePart.AutoSize = true;
            this.lblXAnimationFramePart.Location = new System.Drawing.Point(21, 23);
            this.lblXAnimationFramePart.Name = "lblXAnimationFramePart";
            this.lblXAnimationFramePart.Size = new System.Drawing.Size(17, 17);
            this.lblXAnimationFramePart.TabIndex = 5;
            this.lblXAnimationFramePart.Text = "X";
            // 
            // nUDFrameDataPart
            // 
            this.nUDFrameDataPart.Location = new System.Drawing.Point(141, 20);
            this.nUDFrameDataPart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDFrameDataPart.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nUDFrameDataPart.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
            this.nUDFrameDataPart.Name = "nUDFrameDataPart";
            this.nUDFrameDataPart.Size = new System.Drawing.Size(24, 22);
            this.nUDFrameDataPart.TabIndex = 1;
            this.nUDFrameDataPart.ValueChanged += new System.EventHandler(this.nUDFrameDataPart_ValueChanged);
            // 
            // lblFrameOptionsPart
            // 
            this.lblFrameOptionsPart.AutoSize = true;
            this.lblFrameOptionsPart.Location = new System.Drawing.Point(31, 22);
            this.lblFrameOptionsPart.Name = "lblFrameOptionsPart";
            this.lblFrameOptionsPart.Size = new System.Drawing.Size(109, 17);
            this.lblFrameOptionsPart.TabIndex = 0;
            this.lblFrameOptionsPart.Text = "Frame data part";
            // 
            // gbSelectedBoneFrame
            // 
            this.gbSelectedBoneFrame.Controls.Add(this.txtBoneOptionsLength);
            this.gbSelectedBoneFrame.Controls.Add(this.btnRemovePiece);
            this.gbSelectedBoneFrame.Controls.Add(this.btnAddPiece);
            this.gbSelectedBoneFrame.Controls.Add(this.nUDBoneOptionsLength);
            this.gbSelectedBoneFrame.Controls.Add(this.lblBoneOptionsLength);
            this.gbSelectedBoneFrame.Controls.Add(this.nUDResizeBoneZ);
            this.gbSelectedBoneFrame.Controls.Add(this.lblZScaleBoneOptions);
            this.gbSelectedBoneFrame.Controls.Add(this.nUDResizeBoneY);
            this.gbSelectedBoneFrame.Controls.Add(this.lblYScaleBoneOptions);
            this.gbSelectedBoneFrame.Controls.Add(this.nUDResizeBoneX);
            this.gbSelectedBoneFrame.Controls.Add(this.lblXScaleBoneOptions);
            this.gbSelectedBoneFrame.Enabled = false;
            this.gbSelectedBoneFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSelectedBoneFrame.Location = new System.Drawing.Point(5, 47);
            this.gbSelectedBoneFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectedBoneFrame.Name = "gbSelectedBoneFrame";
            this.gbSelectedBoneFrame.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectedBoneFrame.Size = new System.Drawing.Size(203, 185);
            this.gbSelectedBoneFrame.TabIndex = 13;
            this.gbSelectedBoneFrame.TabStop = false;
            this.gbSelectedBoneFrame.Text = "Bone options";
            this.gbSelectedBoneFrame.Visible = false;
            // 
            // txtBoneOptionsLength
            // 
            this.txtBoneOptionsLength.Location = new System.Drawing.Point(61, 103);
            this.txtBoneOptionsLength.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoneOptionsLength.Name = "txtBoneOptionsLength";
            this.txtBoneOptionsLength.Size = new System.Drawing.Size(111, 22);
            this.txtBoneOptionsLength.TabIndex = 10;
            this.txtBoneOptionsLength.Text = "0";
            this.txtBoneOptionsLength.TextChanged += new System.EventHandler(this.txtBoneLength_TextChanged);
            // 
            // btnRemovePiece
            // 
            this.btnRemovePiece.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemovePiece.Location = new System.Drawing.Point(4, 155);
            this.btnRemovePiece.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRemovePiece.Name = "btnRemovePiece";
            this.btnRemovePiece.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRemovePiece.Size = new System.Drawing.Size(195, 26);
            this.btnRemovePiece.TabIndex = 9;
            this.btnRemovePiece.Text = "Remove part from the bone";
            this.btnRemovePiece.UseVisualStyleBackColor = true;
            this.btnRemovePiece.Click += new System.EventHandler(this.btnRemovePiece_Click);
            // 
            // btnAddPiece
            // 
            this.btnAddPiece.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddPiece.Location = new System.Drawing.Point(4, 130);
            this.btnAddPiece.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddPiece.Name = "btnAddPiece";
            this.btnAddPiece.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAddPiece.Size = new System.Drawing.Size(195, 26);
            this.btnAddPiece.TabIndex = 8;
            this.btnAddPiece.Text = "Add part to the bone";
            this.btnAddPiece.UseVisualStyleBackColor = true;
            this.btnAddPiece.Click += new System.EventHandler(this.btnAddPiece_Click);
            // 
            // nUDBoneOptionsLength
            // 
            this.nUDBoneOptionsLength.DecimalPlaces = 6;
            this.nUDBoneOptionsLength.Location = new System.Drawing.Point(175, 103);
            this.nUDBoneOptionsLength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDBoneOptionsLength.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDBoneOptionsLength.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDBoneOptionsLength.Name = "nUDBoneOptionsLength";
            this.nUDBoneOptionsLength.Size = new System.Drawing.Size(24, 22);
            this.nUDBoneOptionsLength.TabIndex = 7;
            this.nUDBoneOptionsLength.ValueChanged += new System.EventHandler(this.nUDBoneLength_ValueChanged);
            // 
            // lblBoneOptionsLength
            // 
            this.lblBoneOptionsLength.AutoSize = true;
            this.lblBoneOptionsLength.Location = new System.Drawing.Point(5, 106);
            this.lblBoneOptionsLength.Name = "lblBoneOptionsLength";
            this.lblBoneOptionsLength.Size = new System.Drawing.Size(52, 17);
            this.lblBoneOptionsLength.TabIndex = 6;
            this.lblBoneOptionsLength.Text = "Length";
            // 
            // nUDResizeBoneZ
            // 
            this.nUDResizeBoneZ.Location = new System.Drawing.Point(63, 75);
            this.nUDResizeBoneZ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDResizeBoneZ.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDResizeBoneZ.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDResizeBoneZ.Name = "nUDResizeBoneZ";
            this.nUDResizeBoneZ.Size = new System.Drawing.Size(136, 22);
            this.nUDResizeBoneZ.TabIndex = 5;
            this.nUDResizeBoneZ.ValueChanged += new System.EventHandler(this.nUDResizeBoneZ_ValueChanged);
            // 
            // lblZScaleBoneOptions
            // 
            this.lblZScaleBoneOptions.AutoSize = true;
            this.lblZScaleBoneOptions.Location = new System.Drawing.Point(5, 78);
            this.lblZScaleBoneOptions.Name = "lblZScaleBoneOptions";
            this.lblZScaleBoneOptions.Size = new System.Drawing.Size(56, 17);
            this.lblZScaleBoneOptions.TabIndex = 4;
            this.lblZScaleBoneOptions.Text = "Z Scale";
            // 
            // nUDResizeBoneY
            // 
            this.nUDResizeBoneY.Location = new System.Drawing.Point(63, 47);
            this.nUDResizeBoneY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDResizeBoneY.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDResizeBoneY.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDResizeBoneY.Name = "nUDResizeBoneY";
            this.nUDResizeBoneY.Size = new System.Drawing.Size(136, 22);
            this.nUDResizeBoneY.TabIndex = 3;
            this.nUDResizeBoneY.ValueChanged += new System.EventHandler(this.nUDResizeBoneY_ValueChanged);
            // 
            // lblYScaleBoneOptions
            // 
            this.lblYScaleBoneOptions.AutoSize = true;
            this.lblYScaleBoneOptions.Location = new System.Drawing.Point(5, 49);
            this.lblYScaleBoneOptions.Name = "lblYScaleBoneOptions";
            this.lblYScaleBoneOptions.Size = new System.Drawing.Size(56, 17);
            this.lblYScaleBoneOptions.TabIndex = 2;
            this.lblYScaleBoneOptions.Text = "Y Scale";
            // 
            // nUDResizeBoneX
            // 
            this.nUDResizeBoneX.Location = new System.Drawing.Point(63, 18);
            this.nUDResizeBoneX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nUDResizeBoneX.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nUDResizeBoneX.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.nUDResizeBoneX.Name = "nUDResizeBoneX";
            this.nUDResizeBoneX.Size = new System.Drawing.Size(136, 22);
            this.nUDResizeBoneX.TabIndex = 1;
            this.nUDResizeBoneX.ValueChanged += new System.EventHandler(this.nUDResizeBoneX_ValueChanged);
            // 
            // lblXScaleBoneOptions
            // 
            this.lblXScaleBoneOptions.AutoSize = true;
            this.lblXScaleBoneOptions.Location = new System.Drawing.Point(5, 21);
            this.lblXScaleBoneOptions.Name = "lblXScaleBoneOptions";
            this.lblXScaleBoneOptions.Size = new System.Drawing.Size(56, 17);
            this.lblXScaleBoneOptions.TabIndex = 0;
            this.lblXScaleBoneOptions.Text = "X Scale";
            // 
            // lblBoneSelector
            // 
            this.lblBoneSelector.AutoSize = true;
            this.lblBoneSelector.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBoneSelector.Location = new System.Drawing.Point(4, 2);
            this.lblBoneSelector.Name = "lblBoneSelector";
            this.lblBoneSelector.Size = new System.Drawing.Size(103, 17);
            this.lblBoneSelector.TabIndex = 12;
            this.lblBoneSelector.Text = "Selected bone:";
            this.lblBoneSelector.Visible = false;
            // 
            // cbBoneSelector
            // 
            this.cbBoneSelector.FormattingEnabled = true;
            this.cbBoneSelector.Location = new System.Drawing.Point(5, 20);
            this.cbBoneSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbBoneSelector.Name = "cbBoneSelector";
            this.cbBoneSelector.Size = new System.Drawing.Size(201, 24);
            this.cbBoneSelector.TabIndex = 11;
            this.cbBoneSelector.Visible = false;
            this.cbBoneSelector.SelectedIndexChanged += new System.EventHandler(this.cbBoneSelector_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.cbWeapon);
            this.panel2.Controls.Add(this.cbBattleAnimation);
            this.panel2.Controls.Add(this.lblWeapon);
            this.panel2.Controls.Add(this.lblBattleAnimation);
            this.panel2.Controls.Add(this.btnComputeWeaponPosition);
            this.panel2.Controls.Add(this.btnComputeGroundHeight);
            this.panel2.Controls.Add(this.chkShowLastFrameGhost);
            this.panel2.Controls.Add(this.chkShowGround);
            this.panel2.Controls.Add(this.gbSelectedPieceFrame);
            this.panel2.Controls.Add(this.chkDListEnable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(211, 771);
            this.panel2.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkInifintyFarLights);
            this.groupBox1.Controls.Add(this.chkRightLight);
            this.groupBox1.Controls.Add(this.chkLeftLight);
            this.groupBox1.Controls.Add(this.chkRearLight);
            this.groupBox1.Controls.Add(this.chkFrontLight);
            this.groupBox1.Controls.Add(this.lblLightPosZ);
            this.groupBox1.Controls.Add(this.hsbLightPosZ);
            this.groupBox1.Controls.Add(this.lblLightPosY);
            this.groupBox1.Controls.Add(this.hsbLightPosY);
            this.groupBox1.Controls.Add(this.lblLightPosX);
            this.groupBox1.Controls.Add(this.hsbLightPosX);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(7, 602);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(197, 165);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Lighting";
            // 
            // chkInifintyFarLights
            // 
            this.chkInifintyFarLights.AutoSize = true;
            this.chkInifintyFarLights.Location = new System.Drawing.Point(17, 140);
            this.chkInifintyFarLights.Margin = new System.Windows.Forms.Padding(4);
            this.chkInifintyFarLights.Name = "chkInifintyFarLights";
            this.chkInifintyFarLights.Size = new System.Drawing.Size(142, 21);
            this.chkInifintyFarLights.TabIndex = 14;
            this.chkInifintyFarLights.Text = "Inifinitely far lights";
            this.chkInifintyFarLights.UseVisualStyleBackColor = true;
            this.chkInifintyFarLights.CheckedChanged += new System.EventHandler(this.chkInifintyFarLights_CheckedChanged);
            // 
            // chkRightLight
            // 
            this.chkRightLight.AutoSize = true;
            this.chkRightLight.Location = new System.Drawing.Point(105, 118);
            this.chkRightLight.Margin = new System.Windows.Forms.Padding(4);
            this.chkRightLight.Name = "chkRightLight";
            this.chkRightLight.Size = new System.Drawing.Size(63, 21);
            this.chkRightLight.TabIndex = 13;
            this.chkRightLight.Text = "Right";
            this.chkRightLight.UseVisualStyleBackColor = true;
            this.chkRightLight.CheckedChanged += new System.EventHandler(this.chkRight_CheckedChanged);
            // 
            // chkLeftLight
            // 
            this.chkLeftLight.AutoSize = true;
            this.chkLeftLight.Location = new System.Drawing.Point(17, 118);
            this.chkLeftLight.Margin = new System.Windows.Forms.Padding(4);
            this.chkLeftLight.Name = "chkLeftLight";
            this.chkLeftLight.Size = new System.Drawing.Size(54, 21);
            this.chkLeftLight.TabIndex = 12;
            this.chkLeftLight.Text = "Left";
            this.chkLeftLight.UseVisualStyleBackColor = true;
            this.chkLeftLight.CheckedChanged += new System.EventHandler(this.chkLeftLight_CheckedChanged);
            // 
            // chkRearLight
            // 
            this.chkRearLight.AutoSize = true;
            this.chkRearLight.Location = new System.Drawing.Point(105, 100);
            this.chkRearLight.Margin = new System.Windows.Forms.Padding(4);
            this.chkRearLight.Name = "chkRearLight";
            this.chkRearLight.Size = new System.Drawing.Size(61, 21);
            this.chkRearLight.TabIndex = 11;
            this.chkRearLight.Text = "Rear";
            this.chkRearLight.UseVisualStyleBackColor = true;
            this.chkRearLight.CheckedChanged += new System.EventHandler(this.chkRearLight_CheckedChanged);
            // 
            // chkFrontLight
            // 
            this.chkFrontLight.AutoSize = true;
            this.chkFrontLight.Location = new System.Drawing.Point(17, 100);
            this.chkFrontLight.Margin = new System.Windows.Forms.Padding(4);
            this.chkFrontLight.Name = "chkFrontLight";
            this.chkFrontLight.Size = new System.Drawing.Size(63, 21);
            this.chkFrontLight.TabIndex = 10;
            this.chkFrontLight.Text = "Front";
            this.chkFrontLight.UseVisualStyleBackColor = true;
            this.chkFrontLight.CheckedChanged += new System.EventHandler(this.chkFrontLight_CheckedChanged);
            // 
            // lblLightPosZ
            // 
            this.lblLightPosZ.AutoSize = true;
            this.lblLightPosZ.Location = new System.Drawing.Point(9, 76);
            this.lblLightPosZ.Name = "lblLightPosZ";
            this.lblLightPosZ.Size = new System.Drawing.Size(17, 17);
            this.lblLightPosZ.TabIndex = 8;
            this.lblLightPosZ.Text = "Z";
            // 
            // hsbLightPosZ
            // 
            this.hsbLightPosZ.LargeChange = 1;
            this.hsbLightPosZ.Location = new System.Drawing.Point(37, 73);
            this.hsbLightPosZ.Maximum = 32767;
            this.hsbLightPosZ.Name = "hsbLightPosZ";
            this.hsbLightPosZ.Size = new System.Drawing.Size(147, 19);
            this.hsbLightPosZ.TabIndex = 7;
            this.hsbLightPosZ.ValueChanged += new System.EventHandler(this.hsbLightPosZ_ValueChanged);
            // 
            // lblLightPosY
            // 
            this.lblLightPosY.AutoSize = true;
            this.lblLightPosY.Location = new System.Drawing.Point(9, 52);
            this.lblLightPosY.Name = "lblLightPosY";
            this.lblLightPosY.Size = new System.Drawing.Size(17, 17);
            this.lblLightPosY.TabIndex = 6;
            this.lblLightPosY.Text = "Y";
            // 
            // hsbLightPosY
            // 
            this.hsbLightPosY.LargeChange = 1;
            this.hsbLightPosY.Location = new System.Drawing.Point(37, 48);
            this.hsbLightPosY.Maximum = 32767;
            this.hsbLightPosY.Name = "hsbLightPosY";
            this.hsbLightPosY.Size = new System.Drawing.Size(147, 19);
            this.hsbLightPosY.TabIndex = 5;
            this.hsbLightPosY.ValueChanged += new System.EventHandler(this.hsbLightPosY_ValueChanged);
            // 
            // lblLightPosX
            // 
            this.lblLightPosX.AutoSize = true;
            this.lblLightPosX.Location = new System.Drawing.Point(9, 27);
            this.lblLightPosX.Name = "lblLightPosX";
            this.lblLightPosX.Size = new System.Drawing.Size(17, 17);
            this.lblLightPosX.TabIndex = 4;
            this.lblLightPosX.Text = "X";
            // 
            // hsbLightPosX
            // 
            this.hsbLightPosX.LargeChange = 1;
            this.hsbLightPosX.Location = new System.Drawing.Point(37, 23);
            this.hsbLightPosX.Maximum = 32767;
            this.hsbLightPosX.Name = "hsbLightPosX";
            this.hsbLightPosX.Size = new System.Drawing.Size(147, 19);
            this.hsbLightPosX.TabIndex = 3;
            this.hsbLightPosX.ValueChanged += new System.EventHandler(this.hsbLightPosX_ValueChanged);
            // 
            // cbWeapon
            // 
            this.cbWeapon.FormattingEnabled = true;
            this.cbWeapon.Location = new System.Drawing.Point(132, 572);
            this.cbWeapon.Margin = new System.Windows.Forms.Padding(4);
            this.cbWeapon.Name = "cbWeapon";
            this.cbWeapon.Size = new System.Drawing.Size(69, 24);
            this.cbWeapon.TabIndex = 12;
            this.cbWeapon.Visible = false;
            this.cbWeapon.SelectedIndexChanged += new System.EventHandler(this.cbWeapon_SelectedIndexChanged);
            // 
            // cbBattleAnimation
            // 
            this.cbBattleAnimation.FormattingEnabled = true;
            this.cbBattleAnimation.Location = new System.Drawing.Point(132, 546);
            this.cbBattleAnimation.Margin = new System.Windows.Forms.Padding(4);
            this.cbBattleAnimation.Name = "cbBattleAnimation";
            this.cbBattleAnimation.Size = new System.Drawing.Size(69, 24);
            this.cbBattleAnimation.TabIndex = 11;
            this.cbBattleAnimation.Visible = false;
            this.cbBattleAnimation.SelectedIndexChanged += new System.EventHandler(this.cbBattleAnimation_SelectedIndexChanged);
            // 
            // lblWeapon
            // 
            this.lblWeapon.ForeColor = System.Drawing.SystemColors.Control;
            this.lblWeapon.Location = new System.Drawing.Point(56, 575);
            this.lblWeapon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWeapon.Name = "lblWeapon";
            this.lblWeapon.Size = new System.Drawing.Size(68, 17);
            this.lblWeapon.TabIndex = 10;
            this.lblWeapon.Text = "Weapon:";
            this.lblWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblWeapon.Visible = false;
            // 
            // lblBattleAnimation
            // 
            this.lblBattleAnimation.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBattleAnimation.Location = new System.Drawing.Point(9, 549);
            this.lblBattleAnimation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBattleAnimation.Name = "lblBattleAnimation";
            this.lblBattleAnimation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBattleAnimation.Size = new System.Drawing.Size(115, 17);
            this.lblBattleAnimation.TabIndex = 9;
            this.lblBattleAnimation.Text = "Battle Animation:";
            this.lblBattleAnimation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBattleAnimation.Visible = false;
            // 
            // btnComputeWeaponPosition
            // 
            this.btnComputeWeaponPosition.Location = new System.Drawing.Point(7, 501);
            this.btnComputeWeaponPosition.Margin = new System.Windows.Forms.Padding(4);
            this.btnComputeWeaponPosition.Name = "btnComputeWeaponPosition";
            this.btnComputeWeaponPosition.Size = new System.Drawing.Size(197, 43);
            this.btnComputeWeaponPosition.TabIndex = 8;
            this.btnComputeWeaponPosition.Text = "Compute attached weapon position";
            this.btnComputeWeaponPosition.UseVisualStyleBackColor = true;
            this.btnComputeWeaponPosition.Visible = false;
            this.btnComputeWeaponPosition.Click += new System.EventHandler(this.btnComputeWeaponPosition_Click);
            // 
            // btnComputeGroundHeight
            // 
            this.btnComputeGroundHeight.Location = new System.Drawing.Point(7, 475);
            this.btnComputeGroundHeight.Margin = new System.Windows.Forms.Padding(4);
            this.btnComputeGroundHeight.Name = "btnComputeGroundHeight";
            this.btnComputeGroundHeight.Size = new System.Drawing.Size(197, 27);
            this.btnComputeGroundHeight.TabIndex = 7;
            this.btnComputeGroundHeight.Text = "Compute ground height";
            this.btnComputeGroundHeight.UseVisualStyleBackColor = true;
            this.btnComputeGroundHeight.Visible = false;
            this.btnComputeGroundHeight.Click += new System.EventHandler(this.btnComputeGroundHeight_Click);
            // 
            // chkShowLastFrameGhost
            // 
            this.chkShowLastFrameGhost.AutoSize = true;
            this.chkShowLastFrameGhost.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowLastFrameGhost.Location = new System.Drawing.Point(13, 416);
            this.chkShowLastFrameGhost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowLastFrameGhost.Name = "chkShowLastFrameGhost";
            this.chkShowLastFrameGhost.Size = new System.Drawing.Size(185, 21);
            this.chkShowLastFrameGhost.TabIndex = 6;
            this.chkShowLastFrameGhost.Text = "Overlap last frame ghost";
            this.chkShowLastFrameGhost.UseVisualStyleBackColor = true;
            this.chkShowLastFrameGhost.CheckedChanged += new System.EventHandler(this.chkShowLastFrameGhost_CheckedChanged);
            // 
            // chkShowGround
            // 
            this.chkShowGround.AutoSize = true;
            this.chkShowGround.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowGround.Location = new System.Drawing.Point(13, 434);
            this.chkShowGround.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowGround.Name = "chkShowGround";
            this.chkShowGround.Size = new System.Drawing.Size(113, 21);
            this.chkShowGround.TabIndex = 5;
            this.chkShowGround.Text = "Show ground";
            this.chkShowGround.UseVisualStyleBackColor = true;
            this.chkShowGround.CheckedChanged += new System.EventHandler(this.chkShowGround_CheckedChanged);
            // 
            // gbSelectedPieceFrame
            // 
            this.gbSelectedPieceFrame.Controls.Add(this.gbRotateFrame);
            this.gbSelectedPieceFrame.Controls.Add(this.gbRepositionFrame);
            this.gbSelectedPieceFrame.Controls.Add(this.gbResizeFrame);
            this.gbSelectedPieceFrame.Enabled = false;
            this.gbSelectedPieceFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSelectedPieceFrame.Location = new System.Drawing.Point(7, 4);
            this.gbSelectedPieceFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectedPieceFrame.Name = "gbSelectedPieceFrame";
            this.gbSelectedPieceFrame.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbSelectedPieceFrame.Size = new System.Drawing.Size(197, 410);
            this.gbSelectedPieceFrame.TabIndex = 4;
            this.gbSelectedPieceFrame.TabStop = false;
            this.gbSelectedPieceFrame.Text = "Selected Piece";
            // 
            // gbRotateFrame
            // 
            this.gbRotateFrame.Controls.Add(this.txtRotateGamma);
            this.gbRotateFrame.Controls.Add(this.hsbRotateGamma);
            this.gbRotateFrame.Controls.Add(this.lblRotateGamma);
            this.gbRotateFrame.Controls.Add(this.txtRotateBeta);
            this.gbRotateFrame.Controls.Add(this.hsbRotateBeta);
            this.gbRotateFrame.Controls.Add(this.lblRotateBeta);
            this.gbRotateFrame.Controls.Add(this.lblRotateAlpha);
            this.gbRotateFrame.Controls.Add(this.txtRotateAlpha);
            this.gbRotateFrame.Controls.Add(this.hsbRotateAlpha);
            this.gbRotateFrame.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.gbRotateFrame.Location = new System.Drawing.Point(7, 236);
            this.gbRotateFrame.Margin = new System.Windows.Forms.Padding(4);
            this.gbRotateFrame.Name = "gbRotateFrame";
            this.gbRotateFrame.Padding = new System.Windows.Forms.Padding(4);
            this.gbRotateFrame.Size = new System.Drawing.Size(184, 169);
            this.gbRotateFrame.TabIndex = 2;
            this.gbRotateFrame.TabStop = false;
            this.gbRotateFrame.Text = "Rotation";
            // 
            // txtRotateGamma
            // 
            this.txtRotateGamma.Location = new System.Drawing.Point(135, 137);
            this.txtRotateGamma.Margin = new System.Windows.Forms.Padding(4);
            this.txtRotateGamma.Name = "txtRotateGamma";
            this.txtRotateGamma.Size = new System.Drawing.Size(36, 22);
            this.txtRotateGamma.TabIndex = 8;
            this.txtRotateGamma.Text = "0";
            this.txtRotateGamma.TextChanged += new System.EventHandler(this.txtRotateGamma_TextChanged);
            // 
            // hsbRotateGamma
            // 
            this.hsbRotateGamma.LargeChange = 1;
            this.hsbRotateGamma.Location = new System.Drawing.Point(9, 137);
            this.hsbRotateGamma.Maximum = 360;
            this.hsbRotateGamma.Name = "hsbRotateGamma";
            this.hsbRotateGamma.Size = new System.Drawing.Size(120, 19);
            this.hsbRotateGamma.TabIndex = 7;
            this.hsbRotateGamma.ValueChanged += new System.EventHandler(this.hsbRotateGamma_ValueChanged);
            // 
            // lblRotateGamma
            // 
            this.lblRotateGamma.AutoSize = true;
            this.lblRotateGamma.Location = new System.Drawing.Point(7, 118);
            this.lblRotateGamma.Name = "lblRotateGamma";
            this.lblRotateGamma.Size = new System.Drawing.Size(162, 17);
            this.lblRotateGamma.TabIndex = 6;
            this.lblRotateGamma.Text = "Gamma rotation (Z-Axis)";
            // 
            // txtRotateBeta
            // 
            this.txtRotateBeta.Location = new System.Drawing.Point(136, 89);
            this.txtRotateBeta.Margin = new System.Windows.Forms.Padding(4);
            this.txtRotateBeta.Name = "txtRotateBeta";
            this.txtRotateBeta.Size = new System.Drawing.Size(36, 22);
            this.txtRotateBeta.TabIndex = 5;
            this.txtRotateBeta.Text = "0";
            this.txtRotateBeta.TextChanged += new System.EventHandler(this.txtRotateBeta_TextChanged);
            // 
            // hsbRotateBeta
            // 
            this.hsbRotateBeta.LargeChange = 1;
            this.hsbRotateBeta.Location = new System.Drawing.Point(11, 89);
            this.hsbRotateBeta.Maximum = 360;
            this.hsbRotateBeta.Name = "hsbRotateBeta";
            this.hsbRotateBeta.Size = new System.Drawing.Size(120, 19);
            this.hsbRotateBeta.TabIndex = 4;
            this.hsbRotateBeta.ValueChanged += new System.EventHandler(this.hsbRotateBeta_ValueChanged);
            // 
            // lblRotateBeta
            // 
            this.lblRotateBeta.AutoSize = true;
            this.lblRotateBeta.Location = new System.Drawing.Point(7, 70);
            this.lblRotateBeta.Name = "lblRotateBeta";
            this.lblRotateBeta.Size = new System.Drawing.Size(142, 17);
            this.lblRotateBeta.TabIndex = 3;
            this.lblRotateBeta.Text = "Beta rotation (Y-Axis)";
            // 
            // lblRotateAlpha
            // 
            this.lblRotateAlpha.AutoSize = true;
            this.lblRotateAlpha.Location = new System.Drawing.Point(7, 22);
            this.lblRotateAlpha.Name = "lblRotateAlpha";
            this.lblRotateAlpha.Size = new System.Drawing.Size(149, 17);
            this.lblRotateAlpha.TabIndex = 2;
            this.lblRotateAlpha.Text = "Alpha rotation (X-Axis)";
            // 
            // txtRotateAlpha
            // 
            this.txtRotateAlpha.Location = new System.Drawing.Point(136, 41);
            this.txtRotateAlpha.Margin = new System.Windows.Forms.Padding(4);
            this.txtRotateAlpha.Name = "txtRotateAlpha";
            this.txtRotateAlpha.Size = new System.Drawing.Size(36, 22);
            this.txtRotateAlpha.TabIndex = 1;
            this.txtRotateAlpha.Text = "0";
            this.txtRotateAlpha.TextChanged += new System.EventHandler(this.txtRotateAlpha_TextChanged);
            // 
            // hsbRotateAlpha
            // 
            this.hsbRotateAlpha.LargeChange = 1;
            this.hsbRotateAlpha.Location = new System.Drawing.Point(11, 41);
            this.hsbRotateAlpha.Maximum = 360;
            this.hsbRotateAlpha.Name = "hsbRotateAlpha";
            this.hsbRotateAlpha.Size = new System.Drawing.Size(120, 19);
            this.hsbRotateAlpha.TabIndex = 0;
            this.hsbRotateAlpha.ValueChanged += new System.EventHandler(this.hsbRotateAlpha_ValueChanged);
            // 
            // gbRepositionFrame
            // 
            this.gbRepositionFrame.Controls.Add(this.txtRepositionZ);
            this.gbRepositionFrame.Controls.Add(this.hsbRepositionZ);
            this.gbRepositionFrame.Controls.Add(this.lblRepositionZ);
            this.gbRepositionFrame.Controls.Add(this.txtRepositionY);
            this.gbRepositionFrame.Controls.Add(this.hsbRepositionY);
            this.gbRepositionFrame.Controls.Add(this.lblRepositionY);
            this.gbRepositionFrame.Controls.Add(this.lblRepositionX);
            this.gbRepositionFrame.Controls.Add(this.txtRepositionX);
            this.gbRepositionFrame.Controls.Add(this.hsbRepositionX);
            this.gbRepositionFrame.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.gbRepositionFrame.Location = new System.Drawing.Point(7, 127);
            this.gbRepositionFrame.Margin = new System.Windows.Forms.Padding(4);
            this.gbRepositionFrame.Name = "gbRepositionFrame";
            this.gbRepositionFrame.Padding = new System.Windows.Forms.Padding(4);
            this.gbRepositionFrame.Size = new System.Drawing.Size(184, 106);
            this.gbRepositionFrame.TabIndex = 1;
            this.gbRepositionFrame.TabStop = false;
            this.gbRepositionFrame.Text = "Reposition";
            // 
            // txtRepositionZ
            // 
            this.txtRepositionZ.Location = new System.Drawing.Point(139, 74);
            this.txtRepositionZ.Margin = new System.Windows.Forms.Padding(4);
            this.txtRepositionZ.Name = "txtRepositionZ";
            this.txtRepositionZ.Size = new System.Drawing.Size(36, 22);
            this.txtRepositionZ.TabIndex = 8;
            this.txtRepositionZ.Text = "0";
            this.txtRepositionZ.TextChanged += new System.EventHandler(this.txtRepositionZ_TextChanged);
            // 
            // hsbRepositionZ
            // 
            this.hsbRepositionZ.LargeChange = 1;
            this.hsbRepositionZ.Location = new System.Drawing.Point(29, 74);
            this.hsbRepositionZ.Maximum = 500;
            this.hsbRepositionZ.Minimum = -500;
            this.hsbRepositionZ.Name = "hsbRepositionZ";
            this.hsbRepositionZ.Size = new System.Drawing.Size(104, 19);
            this.hsbRepositionZ.TabIndex = 7;
            this.hsbRepositionZ.ValueChanged += new System.EventHandler(this.hsbRepositionZ_ValueChanged);
            // 
            // lblRepositionZ
            // 
            this.lblRepositionZ.AutoSize = true;
            this.lblRepositionZ.Location = new System.Drawing.Point(8, 75);
            this.lblRepositionZ.Name = "lblRepositionZ";
            this.lblRepositionZ.Size = new System.Drawing.Size(17, 17);
            this.lblRepositionZ.TabIndex = 6;
            this.lblRepositionZ.Text = "Z";
            // 
            // txtRepositionY
            // 
            this.txtRepositionY.Location = new System.Drawing.Point(139, 49);
            this.txtRepositionY.Margin = new System.Windows.Forms.Padding(4);
            this.txtRepositionY.Name = "txtRepositionY";
            this.txtRepositionY.Size = new System.Drawing.Size(36, 22);
            this.txtRepositionY.TabIndex = 5;
            this.txtRepositionY.Text = "0";
            this.txtRepositionY.TextChanged += new System.EventHandler(this.txtRepositionY_TextChanged);
            // 
            // hsbRepositionY
            // 
            this.hsbRepositionY.LargeChange = 1;
            this.hsbRepositionY.Location = new System.Drawing.Point(29, 49);
            this.hsbRepositionY.Maximum = 500;
            this.hsbRepositionY.Minimum = -500;
            this.hsbRepositionY.Name = "hsbRepositionY";
            this.hsbRepositionY.Size = new System.Drawing.Size(104, 19);
            this.hsbRepositionY.TabIndex = 4;
            this.hsbRepositionY.ValueChanged += new System.EventHandler(this.hsbRepositionY_ValueChanged);
            // 
            // lblRepositionY
            // 
            this.lblRepositionY.AutoSize = true;
            this.lblRepositionY.Location = new System.Drawing.Point(8, 52);
            this.lblRepositionY.Name = "lblRepositionY";
            this.lblRepositionY.Size = new System.Drawing.Size(17, 17);
            this.lblRepositionY.TabIndex = 3;
            this.lblRepositionY.Text = "Y";
            // 
            // lblRepositionX
            // 
            this.lblRepositionX.AutoSize = true;
            this.lblRepositionX.Location = new System.Drawing.Point(8, 28);
            this.lblRepositionX.Name = "lblRepositionX";
            this.lblRepositionX.Size = new System.Drawing.Size(17, 17);
            this.lblRepositionX.TabIndex = 2;
            this.lblRepositionX.Text = "X";
            // 
            // txtRepositionX
            // 
            this.txtRepositionX.Location = new System.Drawing.Point(139, 25);
            this.txtRepositionX.Margin = new System.Windows.Forms.Padding(4);
            this.txtRepositionX.Name = "txtRepositionX";
            this.txtRepositionX.Size = new System.Drawing.Size(36, 22);
            this.txtRepositionX.TabIndex = 1;
            this.txtRepositionX.Text = "0";
            this.txtRepositionX.TextChanged += new System.EventHandler(this.txtRepositionX_TextChanged);
            // 
            // hsbRepositionX
            // 
            this.hsbRepositionX.LargeChange = 1;
            this.hsbRepositionX.Location = new System.Drawing.Point(29, 25);
            this.hsbRepositionX.Maximum = 500;
            this.hsbRepositionX.Minimum = -500;
            this.hsbRepositionX.Name = "hsbRepositionX";
            this.hsbRepositionX.Size = new System.Drawing.Size(104, 19);
            this.hsbRepositionX.TabIndex = 0;
            this.hsbRepositionX.ValueChanged += new System.EventHandler(this.hsbRepositionX_ValueChanged);
            // 
            // gbResizeFrame
            // 
            this.gbResizeFrame.Controls.Add(this.txtResizePieceZ);
            this.gbResizeFrame.Controls.Add(this.hsbResizePieceZ);
            this.gbResizeFrame.Controls.Add(this.lblResizePieceZ);
            this.gbResizeFrame.Controls.Add(this.txtResizePieceY);
            this.gbResizeFrame.Controls.Add(this.hsbResizePieceY);
            this.gbResizeFrame.Controls.Add(this.lblResizePieceY);
            this.gbResizeFrame.Controls.Add(this.lblResizePieceX);
            this.gbResizeFrame.Controls.Add(this.txtResizePieceX);
            this.gbResizeFrame.Controls.Add(this.hsbResizePieceX);
            this.gbResizeFrame.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.gbResizeFrame.Location = new System.Drawing.Point(7, 17);
            this.gbResizeFrame.Margin = new System.Windows.Forms.Padding(4);
            this.gbResizeFrame.Name = "gbResizeFrame";
            this.gbResizeFrame.Padding = new System.Windows.Forms.Padding(4);
            this.gbResizeFrame.Size = new System.Drawing.Size(184, 106);
            this.gbResizeFrame.TabIndex = 0;
            this.gbResizeFrame.TabStop = false;
            this.gbResizeFrame.Text = "Resize";
            // 
            // txtResizePieceZ
            // 
            this.txtResizePieceZ.Location = new System.Drawing.Point(139, 74);
            this.txtResizePieceZ.Margin = new System.Windows.Forms.Padding(4);
            this.txtResizePieceZ.Name = "txtResizePieceZ";
            this.txtResizePieceZ.Size = new System.Drawing.Size(36, 22);
            this.txtResizePieceZ.TabIndex = 8;
            this.txtResizePieceZ.Text = "100";
            this.txtResizePieceZ.TextChanged += new System.EventHandler(this.txtResizePieceZ_TextChanged);
            // 
            // hsbResizePieceZ
            // 
            this.hsbResizePieceZ.LargeChange = 1;
            this.hsbResizePieceZ.Location = new System.Drawing.Point(29, 74);
            this.hsbResizePieceZ.Maximum = 400;
            this.hsbResizePieceZ.Name = "hsbResizePieceZ";
            this.hsbResizePieceZ.Size = new System.Drawing.Size(104, 19);
            this.hsbResizePieceZ.TabIndex = 7;
            this.hsbResizePieceZ.Value = 100;
            this.hsbResizePieceZ.ValueChanged += new System.EventHandler(this.hsbResizePieceZ_ValueChanged);
            // 
            // lblResizePieceZ
            // 
            this.lblResizePieceZ.AutoSize = true;
            this.lblResizePieceZ.Location = new System.Drawing.Point(8, 75);
            this.lblResizePieceZ.Name = "lblResizePieceZ";
            this.lblResizePieceZ.Size = new System.Drawing.Size(17, 17);
            this.lblResizePieceZ.TabIndex = 6;
            this.lblResizePieceZ.Text = "Z";
            // 
            // txtResizePieceY
            // 
            this.txtResizePieceY.Location = new System.Drawing.Point(139, 49);
            this.txtResizePieceY.Margin = new System.Windows.Forms.Padding(4);
            this.txtResizePieceY.Name = "txtResizePieceY";
            this.txtResizePieceY.Size = new System.Drawing.Size(36, 22);
            this.txtResizePieceY.TabIndex = 5;
            this.txtResizePieceY.Text = "100";
            this.txtResizePieceY.TextChanged += new System.EventHandler(this.txtResizePieceY_TextChanged);
            // 
            // hsbResizePieceY
            // 
            this.hsbResizePieceY.LargeChange = 1;
            this.hsbResizePieceY.Location = new System.Drawing.Point(29, 49);
            this.hsbResizePieceY.Maximum = 400;
            this.hsbResizePieceY.Name = "hsbResizePieceY";
            this.hsbResizePieceY.Size = new System.Drawing.Size(104, 19);
            this.hsbResizePieceY.TabIndex = 4;
            this.hsbResizePieceY.Value = 100;
            this.hsbResizePieceY.ValueChanged += new System.EventHandler(this.hsbResizePieceY_ValueChanged);
            // 
            // lblResizePieceY
            // 
            this.lblResizePieceY.AutoSize = true;
            this.lblResizePieceY.Location = new System.Drawing.Point(8, 52);
            this.lblResizePieceY.Name = "lblResizePieceY";
            this.lblResizePieceY.Size = new System.Drawing.Size(17, 17);
            this.lblResizePieceY.TabIndex = 3;
            this.lblResizePieceY.Text = "Y";
            // 
            // lblResizePieceX
            // 
            this.lblResizePieceX.AutoSize = true;
            this.lblResizePieceX.Location = new System.Drawing.Point(8, 28);
            this.lblResizePieceX.Name = "lblResizePieceX";
            this.lblResizePieceX.Size = new System.Drawing.Size(17, 17);
            this.lblResizePieceX.TabIndex = 2;
            this.lblResizePieceX.Text = "X";
            // 
            // txtResizePieceX
            // 
            this.txtResizePieceX.Location = new System.Drawing.Point(139, 25);
            this.txtResizePieceX.Margin = new System.Windows.Forms.Padding(4);
            this.txtResizePieceX.Name = "txtResizePieceX";
            this.txtResizePieceX.Size = new System.Drawing.Size(36, 22);
            this.txtResizePieceX.TabIndex = 1;
            this.txtResizePieceX.Text = "100";
            this.txtResizePieceX.TextChanged += new System.EventHandler(this.txtResizePieceX_TextChanged);
            // 
            // hsbResizePieceX
            // 
            this.hsbResizePieceX.LargeChange = 1;
            this.hsbResizePieceX.Location = new System.Drawing.Point(29, 25);
            this.hsbResizePieceX.Maximum = 400;
            this.hsbResizePieceX.Name = "hsbResizePieceX";
            this.hsbResizePieceX.Size = new System.Drawing.Size(104, 19);
            this.hsbResizePieceX.TabIndex = 0;
            this.hsbResizePieceX.Value = 100;
            this.hsbResizePieceX.ValueChanged += new System.EventHandler(this.hsbResizePieceX_ValueChanged);
            // 
            // chkDListEnable
            // 
            this.chkDListEnable.AutoSize = true;
            this.chkDListEnable.ForeColor = System.Drawing.SystemColors.Control;
            this.chkDListEnable.Location = new System.Drawing.Point(13, 454);
            this.chkDListEnable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDListEnable.Name = "chkDListEnable";
            this.chkDListEnable.Size = new System.Drawing.Size(158, 21);
            this.chkDListEnable.TabIndex = 3;
            this.chkDListEnable.Text = "Render using DLists";
            this.chkDListEnable.UseVisualStyleBackColor = true;
            this.chkDListEnable.CheckedChanged += new System.EventHandler(this.checkBDListEnable_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panel3.Controls.Add(this.btnFramePrev);
            this.panel3.Controls.Add(this.btnFrameNext);
            this.panel3.Controls.Add(this.btnPlayStopAnim);
            this.panel3.Controls.Add(this.btnFrameEnd);
            this.panel3.Controls.Add(this.btnFrameBegin);
            this.panel3.Controls.Add(this.chkShowBones);
            this.panel3.Controls.Add(this.btnInterpolateAnimation);
            this.panel3.Controls.Add(this.txtCopyPasteFrame);
            this.panel3.Controls.Add(this.btnPasteFrame);
            this.panel3.Controls.Add(this.btnCopyFrame);
            this.panel3.Controls.Add(this.tbCurrentFrameScroll);
            this.panel3.Controls.Add(this.lblAnimationFrame);
            this.panel3.Controls.Add(this.txtAnimationFrame);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(211, 712);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(555, 87);
            this.panel3.TabIndex = 8;
            // 
            // btnFramePrev
            // 
            this.btnFramePrev.BackgroundImage = global::KimeraCS.Properties.Resources.media_rewind;
            this.btnFramePrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFramePrev.Location = new System.Drawing.Point(381, 9);
            this.btnFramePrev.Margin = new System.Windows.Forms.Padding(4);
            this.btnFramePrev.Name = "btnFramePrev";
            this.btnFramePrev.Size = new System.Drawing.Size(43, 39);
            this.btnFramePrev.TabIndex = 25;
            this.btnFramePrev.UseVisualStyleBackColor = true;
            this.btnFramePrev.Visible = false;
            this.btnFramePrev.Click += new System.EventHandler(this.btnFramePrev_Click);
            // 
            // btnFrameNext
            // 
            this.btnFrameNext.BackgroundImage = global::KimeraCS.Properties.Resources.media_fast_forward;
            this.btnFrameNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameNext.Location = new System.Drawing.Point(464, 9);
            this.btnFrameNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnFrameNext.Name = "btnFrameNext";
            this.btnFrameNext.Size = new System.Drawing.Size(43, 39);
            this.btnFrameNext.TabIndex = 24;
            this.btnFrameNext.UseVisualStyleBackColor = true;
            this.btnFrameNext.Visible = false;
            this.btnFrameNext.Click += new System.EventHandler(this.btnFrameNext_Click);
            // 
            // btnPlayStopAnim
            // 
            this.btnPlayStopAnim.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPlayStopAnim.BackgroundImage = global::KimeraCS.Properties.Resources.media_play;
            this.btnPlayStopAnim.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayStopAnim.Location = new System.Drawing.Point(423, 9);
            this.btnPlayStopAnim.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlayStopAnim.Name = "btnPlayStopAnim";
            this.btnPlayStopAnim.Size = new System.Drawing.Size(43, 39);
            this.btnPlayStopAnim.TabIndex = 23;
            this.btnPlayStopAnim.UseVisualStyleBackColor = true;
            this.btnPlayStopAnim.Visible = false;
            this.btnPlayStopAnim.CheckedChanged += new System.EventHandler(this.btnPlayStopAnm_CheckedChanged);
            // 
            // btnFrameEnd
            // 
            this.btnFrameEnd.BackgroundImage = global::KimeraCS.Properties.Resources.media_end;
            this.btnFrameEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameEnd.Location = new System.Drawing.Point(505, 9);
            this.btnFrameEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnFrameEnd.Name = "btnFrameEnd";
            this.btnFrameEnd.Size = new System.Drawing.Size(43, 39);
            this.btnFrameEnd.TabIndex = 22;
            this.btnFrameEnd.UseVisualStyleBackColor = true;
            this.btnFrameEnd.Visible = false;
            this.btnFrameEnd.Click += new System.EventHandler(this.btnFrameEnd_Click);
            // 
            // btnFrameBegin
            // 
            this.btnFrameBegin.BackgroundImage = global::KimeraCS.Properties.Resources.media_beginning;
            this.btnFrameBegin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameBegin.Location = new System.Drawing.Point(340, 9);
            this.btnFrameBegin.Margin = new System.Windows.Forms.Padding(4);
            this.btnFrameBegin.Name = "btnFrameBegin";
            this.btnFrameBegin.Size = new System.Drawing.Size(43, 39);
            this.btnFrameBegin.TabIndex = 21;
            this.btnFrameBegin.UseVisualStyleBackColor = true;
            this.btnFrameBegin.Visible = false;
            this.btnFrameBegin.Click += new System.EventHandler(this.btnFrameBegin_Click);
            // 
            // chkShowBones
            // 
            this.chkShowBones.AutoSize = true;
            this.chkShowBones.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowBones.Location = new System.Drawing.Point(256, 63);
            this.chkShowBones.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowBones.Name = "chkShowBones";
            this.chkShowBones.Size = new System.Drawing.Size(108, 21);
            this.chkShowBones.TabIndex = 19;
            this.chkShowBones.Text = "Show Bones";
            this.chkShowBones.UseVisualStyleBackColor = true;
            this.chkShowBones.Visible = false;
            this.chkShowBones.CheckedChanged += new System.EventHandler(this.chkShowBones_CheckedChanged);
            // 
            // btnInterpolateAnimation
            // 
            this.btnInterpolateAnimation.Location = new System.Drawing.Point(384, 58);
            this.btnInterpolateAnimation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnInterpolateAnimation.Name = "btnInterpolateAnimation";
            this.btnInterpolateAnimation.Size = new System.Drawing.Size(161, 26);
            this.btnInterpolateAnimation.TabIndex = 18;
            this.btnInterpolateAnimation.Text = "Interpolate Animation";
            this.btnInterpolateAnimation.UseVisualStyleBackColor = true;
            this.btnInterpolateAnimation.Visible = false;
            this.btnInterpolateAnimation.Click += new System.EventHandler(this.btnInterpolateAnimation_Click);
            // 
            // txtCopyPasteFrame
            // 
            this.txtCopyPasteFrame.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtCopyPasteFrame.CausesValidation = false;
            this.txtCopyPasteFrame.Location = new System.Drawing.Point(93, 58);
            this.txtCopyPasteFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCopyPasteFrame.Name = "txtCopyPasteFrame";
            this.txtCopyPasteFrame.ReadOnly = true;
            this.txtCopyPasteFrame.ShortcutsEnabled = false;
            this.txtCopyPasteFrame.Size = new System.Drawing.Size(139, 22);
            this.txtCopyPasteFrame.TabIndex = 17;
            this.txtCopyPasteFrame.TabStop = false;
            this.txtCopyPasteFrame.Visible = false;
            // 
            // btnPasteFrame
            // 
            this.btnPasteFrame.BackgroundImage = global::KimeraCS.Properties.Resources.clipboard_paste;
            this.btnPasteFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPasteFrame.Enabled = false;
            this.btnPasteFrame.Location = new System.Drawing.Point(51, 44);
            this.btnPasteFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPasteFrame.Name = "btnPasteFrame";
            this.btnPasteFrame.Size = new System.Drawing.Size(43, 39);
            this.btnPasteFrame.TabIndex = 16;
            this.btnPasteFrame.UseVisualStyleBackColor = true;
            this.btnPasteFrame.Visible = false;
            this.btnPasteFrame.Click += new System.EventHandler(this.btnPasteFrame_Click);
            // 
            // btnCopyFrame
            // 
            this.btnCopyFrame.BackgroundImage = global::KimeraCS.Properties.Resources.copy;
            this.btnCopyFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCopyFrame.Location = new System.Drawing.Point(9, 44);
            this.btnCopyFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCopyFrame.Name = "btnCopyFrame";
            this.btnCopyFrame.Size = new System.Drawing.Size(43, 39);
            this.btnCopyFrame.TabIndex = 15;
            this.btnCopyFrame.UseVisualStyleBackColor = true;
            this.btnCopyFrame.Visible = false;
            this.btnCopyFrame.Click += new System.EventHandler(this.btnCopyFrame_Click);
            // 
            // tbCurrentFrameScroll
            // 
            this.tbCurrentFrameScroll.Enabled = false;
            this.tbCurrentFrameScroll.Location = new System.Drawing.Point(121, 2);
            this.tbCurrentFrameScroll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbCurrentFrameScroll.Maximum = 5;
            this.tbCurrentFrameScroll.Name = "tbCurrentFrameScroll";
            this.tbCurrentFrameScroll.Size = new System.Drawing.Size(213, 56);
            this.tbCurrentFrameScroll.TabIndex = 14;
            this.tbCurrentFrameScroll.Visible = false;
            this.tbCurrentFrameScroll.Scroll += new System.EventHandler(this.tbCurrentFrameScroll_Scroll);
            this.tbCurrentFrameScroll.ValueChanged += new System.EventHandler(this.tbCurrentFrameScroll_ValueChanged);
            // 
            // lblAnimationFrame
            // 
            this.lblAnimationFrame.AutoSize = true;
            this.lblAnimationFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.lblAnimationFrame.Location = new System.Drawing.Point(9, 15);
            this.lblAnimationFrame.Name = "lblAnimationFrame";
            this.lblAnimationFrame.Size = new System.Drawing.Size(52, 17);
            this.lblAnimationFrame.TabIndex = 13;
            this.lblAnimationFrame.Text = "Frame:";
            this.lblAnimationFrame.Visible = false;
            // 
            // txtAnimationFrame
            // 
            this.txtAnimationFrame.Location = new System.Drawing.Point(61, 10);
            this.txtAnimationFrame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAnimationFrame.Name = "txtAnimationFrame";
            this.txtAnimationFrame.ReadOnly = true;
            this.txtAnimationFrame.Size = new System.Drawing.Size(55, 22);
            this.txtAnimationFrame.TabIndex = 1;
            this.txtAnimationFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAnimationFrame.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem2,
            this.animationToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(979, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFieldSkeletonToolStripMenuItem,
            this.loadBattleMagicSkeletonToolStripMenuItem,
            this.loadPModelToolStripMenuItem,
            this.load3DSToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveSkeletonToolStripMenuItem,
            this.saveSkeletonAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.extiToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadFieldSkeletonToolStripMenuItem
            // 
            this.loadFieldSkeletonToolStripMenuItem.Name = "loadFieldSkeletonToolStripMenuItem";
            this.loadFieldSkeletonToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.loadFieldSkeletonToolStripMenuItem.Text = "Load Field Skeleton";
            this.loadFieldSkeletonToolStripMenuItem.Click += new System.EventHandler(this.loadFieldSkeletonToolStripMenuItem_Click);
            // 
            // loadBattleMagicSkeletonToolStripMenuItem
            // 
            this.loadBattleMagicSkeletonToolStripMenuItem.Name = "loadBattleMagicSkeletonToolStripMenuItem";
            this.loadBattleMagicSkeletonToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.loadBattleMagicSkeletonToolStripMenuItem.Text = "Load Battle/Magic Skeleton";
            this.loadBattleMagicSkeletonToolStripMenuItem.Click += new System.EventHandler(this.loadBattleMagicSkeletonToolStripMenuItem_Click);
            // 
            // loadPModelToolStripMenuItem
            // 
            this.loadPModelToolStripMenuItem.Name = "loadPModelToolStripMenuItem";
            this.loadPModelToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.loadPModelToolStripMenuItem.Text = "Load Model";
            this.loadPModelToolStripMenuItem.Click += new System.EventHandler(this.loadPModelToolStripMenuItem_Click);
            // 
            // load3DSToolStripMenuItem
            // 
            this.load3DSToolStripMenuItem.Name = "load3DSToolStripMenuItem";
            this.load3DSToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.load3DSToolStripMenuItem.Text = "Load 3DS";
            this.load3DSToolStripMenuItem.Click += new System.EventHandler(this.load3DSToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(273, 6);
            // 
            // saveSkeletonToolStripMenuItem
            // 
            this.saveSkeletonToolStripMenuItem.Enabled = false;
            this.saveSkeletonToolStripMenuItem.Name = "saveSkeletonToolStripMenuItem";
            this.saveSkeletonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSkeletonToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.saveSkeletonToolStripMenuItem.Text = "Save Skeleton";
            this.saveSkeletonToolStripMenuItem.Click += new System.EventHandler(this.saveSkeletonToolStripMenuItem_Click);
            // 
            // saveSkeletonAsToolStripMenuItem
            // 
            this.saveSkeletonAsToolStripMenuItem.Enabled = false;
            this.saveSkeletonAsToolStripMenuItem.Name = "saveSkeletonAsToolStripMenuItem";
            this.saveSkeletonAsToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.saveSkeletonAsToolStripMenuItem.Text = "Save Skeleton/Model As...";
            this.saveSkeletonAsToolStripMenuItem.Click += new System.EventHandler(this.saveSkeletonAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(273, 6);
            // 
            // extiToolStripMenuItem
            // 
            this.extiToolStripMenuItem.Name = "extiToolStripMenuItem";
            this.extiToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.extiToolStripMenuItem.Text = "Exit";
            this.extiToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem2
            // 
            this.editToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator6,
            this.resetCameraToolStripMenuItem,
            this.toolStripSeparator7,
            this.uIOpacityToolStripMenuItem});
            this.editToolStripMenuItem2.Name = "editToolStripMenuItem2";
            this.editToolStripMenuItem2.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem2.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(178, 6);
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.resetCameraToolStripMenuItem.Text = "Reset camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(178, 6);
            // 
            // uIOpacityToolStripMenuItem
            // 
            this.uIOpacityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUIOpacity100,
            this.tsUIOpacity90,
            this.tsUIOpacity75,
            this.tsUIOpacity50,
            this.tsUIOpacity25});
            this.uIOpacityToolStripMenuItem.Name = "uIOpacityToolStripMenuItem";
            this.uIOpacityToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.uIOpacityToolStripMenuItem.Text = "UI Opacity";
            // 
            // tsUIOpacity100
            // 
            this.tsUIOpacity100.Checked = true;
            this.tsUIOpacity100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsUIOpacity100.Name = "tsUIOpacity100";
            this.tsUIOpacity100.Size = new System.Drawing.Size(128, 26);
            this.tsUIOpacity100.Text = "100%";
            this.tsUIOpacity100.Click += new System.EventHandler(this.tsUIOpacity100_Click);
            // 
            // tsUIOpacity90
            // 
            this.tsUIOpacity90.Name = "tsUIOpacity90";
            this.tsUIOpacity90.Size = new System.Drawing.Size(128, 26);
            this.tsUIOpacity90.Text = "90%";
            this.tsUIOpacity90.Click += new System.EventHandler(this.tsUIOpacity90_Click);
            // 
            // tsUIOpacity75
            // 
            this.tsUIOpacity75.Name = "tsUIOpacity75";
            this.tsUIOpacity75.Size = new System.Drawing.Size(128, 26);
            this.tsUIOpacity75.Text = "75%";
            this.tsUIOpacity75.Click += new System.EventHandler(this.tsUIOpacity75_Click);
            // 
            // tsUIOpacity50
            // 
            this.tsUIOpacity50.Name = "tsUIOpacity50";
            this.tsUIOpacity50.Size = new System.Drawing.Size(128, 26);
            this.tsUIOpacity50.Text = "50%";
            this.tsUIOpacity50.Click += new System.EventHandler(this.tsUIOpacity50_Click);
            // 
            // tsUIOpacity25
            // 
            this.tsUIOpacity25.Name = "tsUIOpacity25";
            this.tsUIOpacity25.Size = new System.Drawing.Size(128, 26);
            this.tsUIOpacity25.Text = "25%";
            this.tsUIOpacity25.Click += new System.EventHandler(this.tsUIOpacity25_Click);
            // 
            // animationToolStripMenuItem
            // 
            this.animationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFieldAnimationToolStripMenuItem,
            this.loadBattleMagicLimitsAnimationStripMenuItem,
            this.toolStripSeparator3,
            this.saveAnimationToolStripMenuItem,
            this.saveAnimationAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripMenuItem1,
            this.toolStripSeparator5,
            this.interpolateAllAnimationsToolStripMenuItem});
            this.animationToolStripMenuItem.Name = "animationToolStripMenuItem";
            this.animationToolStripMenuItem.Size = new System.Drawing.Size(92, 24);
            this.animationToolStripMenuItem.Text = "&Animation";
            // 
            // loadFieldAnimationToolStripMenuItem
            // 
            this.loadFieldAnimationToolStripMenuItem.Enabled = false;
            this.loadFieldAnimationToolStripMenuItem.Name = "loadFieldAnimationToolStripMenuItem";
            this.loadFieldAnimationToolStripMenuItem.Size = new System.Drawing.Size(339, 26);
            this.loadFieldAnimationToolStripMenuItem.Text = "Load Field Animation";
            this.loadFieldAnimationToolStripMenuItem.Click += new System.EventHandler(this.loadFieldAnimationToolStripMenuItem_Click);
            // 
            // loadBattleMagicLimitsAnimationStripMenuItem
            // 
            this.loadBattleMagicLimitsAnimationStripMenuItem.Enabled = false;
            this.loadBattleMagicLimitsAnimationStripMenuItem.Name = "loadBattleMagicLimitsAnimationStripMenuItem";
            this.loadBattleMagicLimitsAnimationStripMenuItem.Size = new System.Drawing.Size(339, 26);
            this.loadBattleMagicLimitsAnimationStripMenuItem.Text = "Load Battle/Magic/Limits Animations";
            this.loadBattleMagicLimitsAnimationStripMenuItem.Click += new System.EventHandler(this.loadBattleMagicLimitAnimationsStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(336, 6);
            // 
            // saveAnimationToolStripMenuItem
            // 
            this.saveAnimationToolStripMenuItem.Enabled = false;
            this.saveAnimationToolStripMenuItem.Name = "saveAnimationToolStripMenuItem";
            this.saveAnimationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.saveAnimationToolStripMenuItem.Size = new System.Drawing.Size(339, 26);
            this.saveAnimationToolStripMenuItem.Text = "Save Animation";
            this.saveAnimationToolStripMenuItem.Click += new System.EventHandler(this.saveAnimationToolStripMenuItem_Click);
            // 
            // saveAnimationAsToolStripMenuItem
            // 
            this.saveAnimationAsToolStripMenuItem.Enabled = false;
            this.saveAnimationAsToolStripMenuItem.Name = "saveAnimationAsToolStripMenuItem";
            this.saveAnimationAsToolStripMenuItem.Size = new System.Drawing.Size(339, 26);
            this.saveAnimationAsToolStripMenuItem.Text = "Save Animation As...";
            this.saveAnimationAsToolStripMenuItem.Click += new System.EventHandler(this.saveAnimationAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(336, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFPS15,
            this.toolStripFPS30,
            this.toolStripFPS60});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(339, 26);
            this.toolStripMenuItem1.Text = "FPS for Play Animation";
            // 
            // toolStripFPS15
            // 
            this.toolStripFPS15.Checked = true;
            this.toolStripFPS15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripFPS15.Name = "toolStripFPS15";
            this.toolStripFPS15.Size = new System.Drawing.Size(108, 26);
            this.toolStripFPS15.Text = "15";
            this.toolStripFPS15.Click += new System.EventHandler(this.toolStripFPS15_Click);
            // 
            // toolStripFPS30
            // 
            this.toolStripFPS30.Name = "toolStripFPS30";
            this.toolStripFPS30.Size = new System.Drawing.Size(108, 26);
            this.toolStripFPS30.Text = "30";
            this.toolStripFPS30.Click += new System.EventHandler(this.toolStripFPS30_Click);
            // 
            // toolStripFPS60
            // 
            this.toolStripFPS60.Name = "toolStripFPS60";
            this.toolStripFPS60.Size = new System.Drawing.Size(108, 26);
            this.toolStripFPS60.Text = "60";
            this.toolStripFPS60.Click += new System.EventHandler(this.toolStripFPS60_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(336, 6);
            // 
            // interpolateAllAnimationsToolStripMenuItem
            // 
            this.interpolateAllAnimationsToolStripMenuItem.Name = "interpolateAllAnimationsToolStripMenuItem";
            this.interpolateAllAnimationsToolStripMenuItem.Size = new System.Drawing.Size(339, 26);
            this.interpolateAllAnimationsToolStripMenuItem.Text = "Interpolate All Animations";
            this.interpolateAllAnimationsToolStripMenuItem.Click += new System.EventHandler(this.interpolateAllAnimationsToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCharlgpToolStripMenuItem});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.databaseToolStripMenuItem.Text = "&Database";
            // 
            // showCharlgpToolStripMenuItem
            // 
            this.showCharlgpToolStripMenuItem.Name = "showCharlgpToolStripMenuItem";
            this.showCharlgpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.showCharlgpToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
            this.showCharlgpToolStripMenuItem.Text = "Show CHAR.LGP";
            this.showCharlgpToolStripMenuItem.Click += new System.EventHandler(this.showCharlgpToolStripMenuItem_Click);
            // 
            // panelModel
            // 
            this.panelModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelModel.Location = new System.Drawing.Point(212, 31);
            this.panelModel.Margin = new System.Windows.Forms.Padding(4);
            this.panelModel.Name = "panelModel";
            this.panelModel.Size = new System.Drawing.Size(552, 680);
            this.panelModel.TabIndex = 9;
            this.panelModel.TabStop = false;
            this.panelModel.Paint += new System.Windows.Forms.PaintEventHandler(this.panelModel_Paint);
            this.panelModel.DoubleClick += new System.EventHandler(this.panelModel_DoubleClick);
            this.panelModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelModel_MouseDown);
            this.panelModel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelModel_MouseMove);
            this.panelModel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelModel_MouseUp);
            // 
            // frmSkeletonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(979, 799);
            this.Controls.Add(this.panelModel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(994, 820);
            this.Name = "frmSkeletonEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KimeraCS";
            this.Activated += new System.EventHandler(this.frmSkeletonEditor_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSkeletonEditor_FormClosed);
            this.Load += new System.EventHandler(this.frmSkeletonEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSkeletonEditor_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSkeletonEditor_KeyUp);
            this.Move += new System.EventHandler(this.frmSkeletonEditor_Move);
            this.Resize += new System.EventHandler(this.frmSkeletonEditor_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbTexturesFrame.ResumeLayout(false);
            this.gbTexturesFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDMoveTextureUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTextureViewer)).EndInit();
            this.gbAnimationOptionsFrame.ResumeLayout(false);
            this.gbAnimationOptionsFrame.PerformLayout();
            this.gbFrameDataPartOptions.ResumeLayout(false);
            this.gbFrameDataPartOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDZAnimationFramePart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDYAnimationFramePart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXAnimationFramePart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDFrameDataPart)).EndInit();
            this.gbSelectedBoneFrame.ResumeLayout(false);
            this.gbSelectedBoneFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDBoneOptionsLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDResizeBoneX)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbSelectedPieceFrame.ResumeLayout(false);
            this.gbRotateFrame.ResumeLayout(false);
            this.gbRotateFrame.PerformLayout();
            this.gbRepositionFrame.ResumeLayout(false);
            this.gbRepositionFrame.PerformLayout();
            this.gbResizeFrame.ResumeLayout(false);
            this.gbResizeFrame.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCurrentFrameScroll)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelModel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.PictureBox panelModel;
        private System.Windows.Forms.Label lblBoneSelector;
        private System.Windows.Forms.ComboBox cbBoneSelector;
        private System.Windows.Forms.Label lblAnimationFrame;
        private System.Windows.Forms.TextBox txtAnimationFrame;
        public System.Windows.Forms.CheckBox chkDListEnable;
        private System.Windows.Forms.GroupBox gbSelectedBoneFrame;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFieldSkeletonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem load3DSToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveSkeletonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSkeletonAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem extiToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbSelectedPieceFrame;
        private System.Windows.Forms.ToolStripMenuItem animationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFieldAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAnimationAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadBattleMagicLimitsAnimationStripMenuItem;
        private System.Windows.Forms.NumericUpDown nUDResizeBoneZ;
        private System.Windows.Forms.Label lblZScaleBoneOptions;
        private System.Windows.Forms.NumericUpDown nUDResizeBoneY;
        private System.Windows.Forms.Label lblYScaleBoneOptions;
        private System.Windows.Forms.Label lblXScaleBoneOptions;
        private System.Windows.Forms.NumericUpDown nUDBoneOptionsLength;
        private System.Windows.Forms.Label lblBoneOptionsLength;
        private System.Windows.Forms.GroupBox gbAnimationOptionsFrame;
        private System.Windows.Forms.NumericUpDown nUDFrameDataPart;
        private System.Windows.Forms.Label lblFrameOptionsPart;
        private System.Windows.Forms.GroupBox gbFrameDataPartOptions;
        private System.Windows.Forms.Label lblZAnimationFramePart;
        private System.Windows.Forms.Label lblYAnimationFramePart;
        private System.Windows.Forms.Label lblXAnimationFramePart;
        private System.Windows.Forms.NumericUpDown nUDZAnimationFramePart;
        private System.Windows.Forms.NumericUpDown nUDYAnimationFramePart;
        private System.Windows.Forms.NumericUpDown nUDXAnimationFramePart;
        private System.Windows.Forms.Button btnRemovePiece;
        private System.Windows.Forms.Button btnAddPiece;
        private System.Windows.Forms.GroupBox gbTexturesFrame;
        private System.Windows.Forms.CheckBox chkZeroAsTransparent;
        private System.Windows.Forms.Button btnChangeTexture;
        private System.Windows.Forms.Button btnFlipHorizontal;
        private System.Windows.Forms.Button btnFlipVertical;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnRemoveTexture;
        private System.Windows.Forms.Button btnAddTexture;
        private System.Windows.Forms.CheckBox chkPropagateChangesForward;
        private System.Windows.Forms.Button btnDuplicateFrame;
        private System.Windows.Forms.Button btnRemoveFrame;
        private System.Windows.Forms.Button btnInterpolateFrame;
        private System.Windows.Forms.GroupBox gbResizeFrame;
        private System.Windows.Forms.Label lblResizePieceX;
        private System.Windows.Forms.TextBox txtResizePieceX;
        private System.Windows.Forms.HScrollBar hsbResizePieceX;
        private System.Windows.Forms.TextBox txtResizePieceZ;
        private System.Windows.Forms.HScrollBar hsbResizePieceZ;
        private System.Windows.Forms.Label lblResizePieceZ;
        private System.Windows.Forms.TextBox txtResizePieceY;
        private System.Windows.Forms.HScrollBar hsbResizePieceY;
        private System.Windows.Forms.Label lblResizePieceY;
        private System.Windows.Forms.GroupBox gbRepositionFrame;
        private System.Windows.Forms.TextBox txtRepositionZ;
        private System.Windows.Forms.HScrollBar hsbRepositionZ;
        private System.Windows.Forms.Label lblRepositionZ;
        private System.Windows.Forms.TextBox txtRepositionY;
        private System.Windows.Forms.HScrollBar hsbRepositionY;
        private System.Windows.Forms.Label lblRepositionY;
        private System.Windows.Forms.Label lblRepositionX;
        private System.Windows.Forms.TextBox txtRepositionX;
        private System.Windows.Forms.HScrollBar hsbRepositionX;
        private System.Windows.Forms.GroupBox gbRotateFrame;
        private System.Windows.Forms.TextBox txtRotateGamma;
        private System.Windows.Forms.HScrollBar hsbRotateGamma;
        private System.Windows.Forms.Label lblRotateGamma;
        private System.Windows.Forms.TextBox txtRotateBeta;
        private System.Windows.Forms.HScrollBar hsbRotateBeta;
        private System.Windows.Forms.Label lblRotateBeta;
        private System.Windows.Forms.Label lblRotateAlpha;
        private System.Windows.Forms.TextBox txtRotateAlpha;
        private System.Windows.Forms.HScrollBar hsbRotateAlpha;
        public System.Windows.Forms.CheckBox chkShowLastFrameGhost;
        public System.Windows.Forms.CheckBox chkShowGround;
        private System.Windows.Forms.Button btnComputeWeaponPosition;
        private System.Windows.Forms.Button btnComputeGroundHeight;
        private System.Windows.Forms.Label lblWeapon;
        private System.Windows.Forms.Label lblBattleAnimation;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblLightPosX;
        private System.Windows.Forms.HScrollBar hsbLightPosX;
        private System.Windows.Forms.Label lblLightPosZ;
        private System.Windows.Forms.HScrollBar hsbLightPosZ;
        private System.Windows.Forms.Label lblLightPosY;
        private System.Windows.Forms.HScrollBar hsbLightPosY;
        private System.Windows.Forms.CheckBox chkInifintyFarLights;
        private System.Windows.Forms.CheckBox chkRightLight;
        private System.Windows.Forms.CheckBox chkLeftLight;
        private System.Windows.Forms.CheckBox chkRearLight;
        private System.Windows.Forms.NumericUpDown nUDMoveTextureUpDown;
        private System.Windows.Forms.TextBox txtCopyPasteFrame;
        private System.Windows.Forms.Button btnPasteFrame;
        private System.Windows.Forms.Button btnCopyFrame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem interpolateAllAnimationsToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkShowBones;
        private System.Windows.Forms.Button btnInterpolateAnimation;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCharlgpToolStripMenuItem;
        public System.Windows.Forms.CheckBox chkFrontLight;
        public System.Windows.Forms.PictureBox pbTextureViewer;
        private System.Windows.Forms.Button btnFrameEnd;
        private System.Windows.Forms.Button btnFrameBegin;
        private System.Windows.Forms.CheckBox btnPlayStopAnim;
        private System.Windows.Forms.TextBox txtBoneOptionsLength;
        private System.Windows.Forms.Button btnFramePrev;
        private System.Windows.Forms.Button btnFrameNext;
        public System.Windows.Forms.TrackBar tbCurrentFrameScroll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripFPS15;
        private System.Windows.Forms.ToolStripMenuItem toolStripFPS30;
        private System.Windows.Forms.ToolStripMenuItem toolStripFPS60;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem loadBattleMagicSkeletonToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.NumericUpDown nUDResizeBoneX;
        public System.Windows.Forms.ComboBox cbTextureSelect;
        public System.Windows.Forms.ComboBox cbWeapon;
        public System.Windows.Forms.ComboBox cbBattleAnimation;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem resetCameraToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem uIOpacityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsUIOpacity100;
        private System.Windows.Forms.ToolStripMenuItem tsUIOpacity90;
        private System.Windows.Forms.ToolStripMenuItem tsUIOpacity75;
        private System.Windows.Forms.ToolStripMenuItem tsUIOpacity50;
        private System.Windows.Forms.ToolStripMenuItem tsUIOpacity25;
    }
}

