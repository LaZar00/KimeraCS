using System;
using System.Timers;

namespace KimeraCS
{
    partial class FrmSkeletonEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSkeletonEditor));
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
            this.chkColorKeyFlag = new System.Windows.Forms.CheckBox();
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
            this.loadRSDResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.load3DSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSkeletonAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.Import3DSFixingPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.extiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowAxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.uIOpacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity100 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity90 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity75 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity50 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsUIOpacity25 = new System.Windows.Forms.ToolStripMenuItem();
            this.skeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addJointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editJointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.showNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showVertexNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFaceNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalsColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalsScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneftoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fiveftoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thirtyftoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thousandftoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TEXToPNGBatchConversionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFieldAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBattleMagicLimitsAnimationStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAnimationAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputFramesDataTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFramesDataTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputFramesDataTXTToolSelectiveStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeFramesDataTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFPS60 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.interpolateAllAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCharlgpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBattlelgpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMagiclgpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panel1.Location = new System.Drawing.Point(574, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(160, 625);
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
            this.gbTexturesFrame.Controls.Add(this.chkColorKeyFlag);
            this.gbTexturesFrame.Controls.Add(this.cbTextureSelect);
            this.gbTexturesFrame.Controls.Add(this.pbTextureViewer);
            this.gbTexturesFrame.Enabled = false;
            this.gbTexturesFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbTexturesFrame.Location = new System.Drawing.Point(4, 193);
            this.gbTexturesFrame.Margin = new System.Windows.Forms.Padding(2);
            this.gbTexturesFrame.Name = "gbTexturesFrame";
            this.gbTexturesFrame.Padding = new System.Windows.Forms.Padding(2);
            this.gbTexturesFrame.Size = new System.Drawing.Size(152, 223);
            this.gbTexturesFrame.TabIndex = 15;
            this.gbTexturesFrame.TabStop = false;
            this.gbTexturesFrame.Text = "Textures (Part)";
            this.gbTexturesFrame.Visible = false;
            // 
            // nUDMoveTextureUpDown
            // 
            this.nUDMoveTextureUpDown.Location = new System.Drawing.Point(8, 122);
            this.nUDMoveTextureUpDown.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDMoveTextureUpDown.Size = new System.Drawing.Size(18, 20);
            this.nUDMoveTextureUpDown.TabIndex = 9;
            this.nUDMoveTextureUpDown.ValueChanged += new System.EventHandler(this.NudMoveTextureUpDown_ValueChanged);
            // 
            // btnRemoveTexture
            // 
            this.btnRemoveTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemoveTexture.Location = new System.Drawing.Point(3, 199);
            this.btnRemoveTexture.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveTexture.Name = "btnRemoveTexture";
            this.btnRemoveTexture.Size = new System.Drawing.Size(146, 21);
            this.btnRemoveTexture.TabIndex = 8;
            this.btnRemoveTexture.Text = "Remove Texture";
            this.btnRemoveTexture.UseVisualStyleBackColor = true;
            this.btnRemoveTexture.Click += new System.EventHandler(this.BtnRemoveTexture_Click);
            // 
            // btnAddTexture
            // 
            this.btnAddTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTexture.Location = new System.Drawing.Point(3, 179);
            this.btnAddTexture.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddTexture.Name = "btnAddTexture";
            this.btnAddTexture.Size = new System.Drawing.Size(146, 21);
            this.btnAddTexture.TabIndex = 7;
            this.btnAddTexture.Text = "Add Texture";
            this.btnAddTexture.UseVisualStyleBackColor = true;
            this.btnAddTexture.Click += new System.EventHandler(this.BtnAddTexture_Click);
            // 
            // btnChangeTexture
            // 
            this.btnChangeTexture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnChangeTexture.Location = new System.Drawing.Point(3, 159);
            this.btnChangeTexture.Margin = new System.Windows.Forms.Padding(2);
            this.btnChangeTexture.Name = "btnChangeTexture";
            this.btnChangeTexture.Size = new System.Drawing.Size(146, 21);
            this.btnChangeTexture.TabIndex = 6;
            this.btnChangeTexture.Text = "Change Texture";
            this.btnChangeTexture.UseVisualStyleBackColor = true;
            this.btnChangeTexture.Click += new System.EventHandler(this.BtnChangeTexture_Click);
            // 
            // btnFlipHorizontal
            // 
            this.btnFlipHorizontal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFlipHorizontal.BackgroundImage")));
            this.btnFlipHorizontal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipHorizontal.Location = new System.Drawing.Point(113, 86);
            this.btnFlipHorizontal.Margin = new System.Windows.Forms.Padding(2);
            this.btnFlipHorizontal.Name = "btnFlipHorizontal";
            this.btnFlipHorizontal.Size = new System.Drawing.Size(32, 32);
            this.btnFlipHorizontal.TabIndex = 5;
            this.btnFlipHorizontal.UseVisualStyleBackColor = true;
            this.btnFlipHorizontal.Click += new System.EventHandler(this.BtnFlipHorizontal_Click);
            // 
            // btnFlipVertical
            // 
            this.btnFlipVertical.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFlipVertical.BackgroundImage")));
            this.btnFlipVertical.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlipVertical.Location = new System.Drawing.Point(113, 52);
            this.btnFlipVertical.Margin = new System.Windows.Forms.Padding(2);
            this.btnFlipVertical.Name = "btnFlipVertical";
            this.btnFlipVertical.Size = new System.Drawing.Size(32, 32);
            this.btnFlipVertical.TabIndex = 4;
            this.btnFlipVertical.UseVisualStyleBackColor = true;
            this.btnFlipVertical.Click += new System.EventHandler(this.BtnFlipVertical_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRotate.BackgroundImage")));
            this.btnRotate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRotate.Location = new System.Drawing.Point(113, 18);
            this.btnRotate.Margin = new System.Windows.Forms.Padding(2);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(32, 32);
            this.btnRotate.TabIndex = 3;
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.BtnRotate_Click);
            // 
            // chkColorKeyFlag
            // 
            this.chkColorKeyFlag.AutoSize = true;
            this.chkColorKeyFlag.Enabled = false;
            this.chkColorKeyFlag.Location = new System.Drawing.Point(16, 144);
            this.chkColorKeyFlag.Margin = new System.Windows.Forms.Padding(2);
            this.chkColorKeyFlag.Name = "chkColorKeyFlag";
            this.chkColorKeyFlag.Size = new System.Drawing.Size(127, 17);
            this.chkColorKeyFlag.TabIndex = 2;
            this.chkColorKeyFlag.Text = "Black is transparency";
            this.chkColorKeyFlag.UseVisualStyleBackColor = true;
            this.chkColorKeyFlag.Click += new System.EventHandler(this.ChkZeroAsTransparent_Click);
            // 
            // cbTextureSelect
            // 
            this.cbTextureSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTextureSelect.FormattingEnabled = true;
            this.cbTextureSelect.Location = new System.Drawing.Point(28, 122);
            this.cbTextureSelect.Margin = new System.Windows.Forms.Padding(2);
            this.cbTextureSelect.Name = "cbTextureSelect";
            this.cbTextureSelect.Size = new System.Drawing.Size(115, 21);
            this.cbTextureSelect.TabIndex = 1;
            this.cbTextureSelect.SelectedIndexChanged += new System.EventHandler(this.CbTextureSelect_SelectedIndexChanged);
            // 
            // pbTextureViewer
            // 
            this.pbTextureViewer.BackColor = System.Drawing.Color.Transparent;
            this.pbTextureViewer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbTextureViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbTextureViewer.Location = new System.Drawing.Point(8, 16);
            this.pbTextureViewer.Margin = new System.Windows.Forms.Padding(2);
            this.pbTextureViewer.Name = "pbTextureViewer";
            this.pbTextureViewer.Size = new System.Drawing.Size(104, 104);
            this.pbTextureViewer.TabIndex = 0;
            this.pbTextureViewer.TabStop = false;
            this.pbTextureViewer.DoubleClick += new System.EventHandler(this.PbTextureViewer_DoubleClick);
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
            this.gbAnimationOptionsFrame.Location = new System.Drawing.Point(6, 422);
            this.gbAnimationOptionsFrame.Margin = new System.Windows.Forms.Padding(2);
            this.gbAnimationOptionsFrame.Name = "gbAnimationOptionsFrame";
            this.gbAnimationOptionsFrame.Padding = new System.Windows.Forms.Padding(2);
            this.gbAnimationOptionsFrame.Size = new System.Drawing.Size(152, 201);
            this.gbAnimationOptionsFrame.TabIndex = 14;
            this.gbAnimationOptionsFrame.TabStop = false;
            this.gbAnimationOptionsFrame.Text = "Frame options";
            this.gbAnimationOptionsFrame.Visible = false;
            // 
            // btnInterpolateFrame
            // 
            this.btnInterpolateFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInterpolateFrame.Location = new System.Drawing.Point(3, 177);
            this.btnInterpolateFrame.Margin = new System.Windows.Forms.Padding(2);
            this.btnInterpolateFrame.Name = "btnInterpolateFrame";
            this.btnInterpolateFrame.Size = new System.Drawing.Size(146, 21);
            this.btnInterpolateFrame.TabIndex = 9;
            this.btnInterpolateFrame.Text = "Interpolate Frame";
            this.btnInterpolateFrame.UseVisualStyleBackColor = true;
            this.btnInterpolateFrame.Click += new System.EventHandler(this.BtnInterpolateFrame_Click);
            // 
            // btnDuplicateFrame
            // 
            this.btnDuplicateFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDuplicateFrame.Location = new System.Drawing.Point(3, 157);
            this.btnDuplicateFrame.Margin = new System.Windows.Forms.Padding(2);
            this.btnDuplicateFrame.Name = "btnDuplicateFrame";
            this.btnDuplicateFrame.Size = new System.Drawing.Size(146, 21);
            this.btnDuplicateFrame.TabIndex = 8;
            this.btnDuplicateFrame.Text = "Duplicate Frame";
            this.btnDuplicateFrame.UseVisualStyleBackColor = true;
            this.btnDuplicateFrame.Click += new System.EventHandler(this.BtnDuplicateFrame_Click);
            // 
            // btnRemoveFrame
            // 
            this.btnRemoveFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemoveFrame.Location = new System.Drawing.Point(3, 137);
            this.btnRemoveFrame.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveFrame.Name = "btnRemoveFrame";
            this.btnRemoveFrame.Size = new System.Drawing.Size(146, 21);
            this.btnRemoveFrame.TabIndex = 7;
            this.btnRemoveFrame.Text = "Remove Frame";
            this.btnRemoveFrame.UseVisualStyleBackColor = true;
            this.btnRemoveFrame.Click += new System.EventHandler(this.BtnRemoveFrame_Click);
            // 
            // chkPropagateChangesForward
            // 
            this.chkPropagateChangesForward.AutoSize = true;
            this.chkPropagateChangesForward.Checked = true;
            this.chkPropagateChangesForward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPropagateChangesForward.Location = new System.Drawing.Point(22, 122);
            this.chkPropagateChangesForward.Name = "chkPropagateChangesForward";
            this.chkPropagateChangesForward.Size = new System.Drawing.Size(113, 17);
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
            this.gbFrameDataPartOptions.Location = new System.Drawing.Point(9, 34);
            this.gbFrameDataPartOptions.Margin = new System.Windows.Forms.Padding(2);
            this.gbFrameDataPartOptions.Name = "gbFrameDataPartOptions";
            this.gbFrameDataPartOptions.Padding = new System.Windows.Forms.Padding(2);
            this.gbFrameDataPartOptions.Size = new System.Drawing.Size(133, 87);
            this.gbFrameDataPartOptions.TabIndex = 2;
            this.gbFrameDataPartOptions.TabStop = false;
            this.gbFrameDataPartOptions.Text = "Bone rotation";
            // 
            // nUDZAnimationFramePart
            // 
            this.nUDZAnimationFramePart.DecimalPlaces = 6;
            this.nUDZAnimationFramePart.Location = new System.Drawing.Point(38, 61);
            this.nUDZAnimationFramePart.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDZAnimationFramePart.Size = new System.Drawing.Size(88, 20);
            this.nUDZAnimationFramePart.TabIndex = 10;
            this.nUDZAnimationFramePart.ValueChanged += new System.EventHandler(this.NudZAnimationFramePart_ValueChanged);
            // 
            // nUDYAnimationFramePart
            // 
            this.nUDYAnimationFramePart.DecimalPlaces = 6;
            this.nUDYAnimationFramePart.Location = new System.Drawing.Point(38, 39);
            this.nUDYAnimationFramePart.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDYAnimationFramePart.Size = new System.Drawing.Size(88, 20);
            this.nUDYAnimationFramePart.TabIndex = 9;
            this.nUDYAnimationFramePart.ValueChanged += new System.EventHandler(this.NudYAnimationFramePart_ValueChanged);
            // 
            // nUDXAnimationFramePart
            // 
            this.nUDXAnimationFramePart.DecimalPlaces = 6;
            this.nUDXAnimationFramePart.Location = new System.Drawing.Point(38, 17);
            this.nUDXAnimationFramePart.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDXAnimationFramePart.Size = new System.Drawing.Size(88, 20);
            this.nUDXAnimationFramePart.TabIndex = 8;
            this.nUDXAnimationFramePart.ValueChanged += new System.EventHandler(this.NudXAnimationFramePart_ValueChanged);
            // 
            // lblZAnimationFramePart
            // 
            this.lblZAnimationFramePart.AutoSize = true;
            this.lblZAnimationFramePart.Location = new System.Drawing.Point(16, 63);
            this.lblZAnimationFramePart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZAnimationFramePart.Name = "lblZAnimationFramePart";
            this.lblZAnimationFramePart.Size = new System.Drawing.Size(14, 13);
            this.lblZAnimationFramePart.TabIndex = 7;
            this.lblZAnimationFramePart.Text = "Z";
            // 
            // lblYAnimationFramePart
            // 
            this.lblYAnimationFramePart.AutoSize = true;
            this.lblYAnimationFramePart.Location = new System.Drawing.Point(16, 41);
            this.lblYAnimationFramePart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYAnimationFramePart.Name = "lblYAnimationFramePart";
            this.lblYAnimationFramePart.Size = new System.Drawing.Size(14, 13);
            this.lblYAnimationFramePart.TabIndex = 6;
            this.lblYAnimationFramePart.Text = "Y";
            // 
            // lblXAnimationFramePart
            // 
            this.lblXAnimationFramePart.AutoSize = true;
            this.lblXAnimationFramePart.Location = new System.Drawing.Point(16, 19);
            this.lblXAnimationFramePart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblXAnimationFramePart.Name = "lblXAnimationFramePart";
            this.lblXAnimationFramePart.Size = new System.Drawing.Size(14, 13);
            this.lblXAnimationFramePart.TabIndex = 5;
            this.lblXAnimationFramePart.Text = "X";
            // 
            // nUDFrameDataPart
            // 
            this.nUDFrameDataPart.Location = new System.Drawing.Point(106, 16);
            this.nUDFrameDataPart.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDFrameDataPart.Size = new System.Drawing.Size(18, 20);
            this.nUDFrameDataPart.TabIndex = 1;
            this.nUDFrameDataPart.ValueChanged += new System.EventHandler(this.NudFrameDataPart_ValueChanged);
            // 
            // lblFrameOptionsPart
            // 
            this.lblFrameOptionsPart.AutoSize = true;
            this.lblFrameOptionsPart.Location = new System.Drawing.Point(23, 18);
            this.lblFrameOptionsPart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrameOptionsPart.Name = "lblFrameOptionsPart";
            this.lblFrameOptionsPart.Size = new System.Drawing.Size(81, 13);
            this.lblFrameOptionsPart.TabIndex = 0;
            this.lblFrameOptionsPart.Text = "Frame data part";
            // 
            // gbSelectedBoneFrame
            // 
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
            this.gbSelectedBoneFrame.Location = new System.Drawing.Point(4, 38);
            this.gbSelectedBoneFrame.Margin = new System.Windows.Forms.Padding(2);
            this.gbSelectedBoneFrame.Name = "gbSelectedBoneFrame";
            this.gbSelectedBoneFrame.Padding = new System.Windows.Forms.Padding(2);
            this.gbSelectedBoneFrame.Size = new System.Drawing.Size(152, 150);
            this.gbSelectedBoneFrame.TabIndex = 13;
            this.gbSelectedBoneFrame.TabStop = false;
            this.gbSelectedBoneFrame.Text = "Joint options";
            this.gbSelectedBoneFrame.Visible = false;
            // 
            // btnRemovePiece
            // 
            this.btnRemovePiece.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemovePiece.Location = new System.Drawing.Point(3, 126);
            this.btnRemovePiece.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemovePiece.Name = "btnRemovePiece";
            this.btnRemovePiece.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRemovePiece.Size = new System.Drawing.Size(146, 21);
            this.btnRemovePiece.TabIndex = 9;
            this.btnRemovePiece.Text = "Remove part from the Joint";
            this.btnRemovePiece.UseVisualStyleBackColor = true;
            this.btnRemovePiece.Click += new System.EventHandler(this.BtnRemovePiece_Click);
            // 
            // btnAddPiece
            // 
            this.btnAddPiece.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddPiece.Location = new System.Drawing.Point(3, 106);
            this.btnAddPiece.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddPiece.Name = "btnAddPiece";
            this.btnAddPiece.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAddPiece.Size = new System.Drawing.Size(146, 21);
            this.btnAddPiece.TabIndex = 8;
            this.btnAddPiece.Text = "Add part to the Joint";
            this.btnAddPiece.UseVisualStyleBackColor = true;
            this.btnAddPiece.Click += new System.EventHandler(this.BtnAddPiece_Click);
            // 
            // nUDBoneOptionsLength
            // 
            this.nUDBoneOptionsLength.DecimalPlaces = 6;
            this.nUDBoneOptionsLength.Location = new System.Drawing.Point(47, 84);
            this.nUDBoneOptionsLength.Margin = new System.Windows.Forms.Padding(2);
            this.nUDBoneOptionsLength.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nUDBoneOptionsLength.Minimum = new decimal(new int[] {
            65536,
            0,
            0,
            -2147483648});
            this.nUDBoneOptionsLength.Name = "nUDBoneOptionsLength";
            this.nUDBoneOptionsLength.Size = new System.Drawing.Size(102, 20);
            this.nUDBoneOptionsLength.TabIndex = 7;
            this.nUDBoneOptionsLength.TextChanged += new System.EventHandler(this.NudBoneLength_TextChanged);
            this.nUDBoneOptionsLength.ValueChanged += new System.EventHandler(this.NudBoneLength_ValueChanged);
            // 
            // lblBoneOptionsLength
            // 
            this.lblBoneOptionsLength.AutoSize = true;
            this.lblBoneOptionsLength.Location = new System.Drawing.Point(4, 86);
            this.lblBoneOptionsLength.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoneOptionsLength.Name = "lblBoneOptionsLength";
            this.lblBoneOptionsLength.Size = new System.Drawing.Size(40, 13);
            this.lblBoneOptionsLength.TabIndex = 6;
            this.lblBoneOptionsLength.Text = "Length";
            // 
            // nUDResizeBoneZ
            // 
            this.nUDResizeBoneZ.Location = new System.Drawing.Point(47, 61);
            this.nUDResizeBoneZ.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDResizeBoneZ.Size = new System.Drawing.Size(102, 20);
            this.nUDResizeBoneZ.TabIndex = 5;
            this.nUDResizeBoneZ.ValueChanged += new System.EventHandler(this.NudResizeBoneZ_ValueChanged);
            // 
            // lblZScaleBoneOptions
            // 
            this.lblZScaleBoneOptions.AutoSize = true;
            this.lblZScaleBoneOptions.Location = new System.Drawing.Point(4, 63);
            this.lblZScaleBoneOptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZScaleBoneOptions.Name = "lblZScaleBoneOptions";
            this.lblZScaleBoneOptions.Size = new System.Drawing.Size(44, 13);
            this.lblZScaleBoneOptions.TabIndex = 4;
            this.lblZScaleBoneOptions.Text = "Z Scale";
            // 
            // nUDResizeBoneY
            // 
            this.nUDResizeBoneY.Location = new System.Drawing.Point(47, 38);
            this.nUDResizeBoneY.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDResizeBoneY.Size = new System.Drawing.Size(102, 20);
            this.nUDResizeBoneY.TabIndex = 3;
            this.nUDResizeBoneY.ValueChanged += new System.EventHandler(this.NudResizeBoneY_ValueChanged);
            // 
            // lblYScaleBoneOptions
            // 
            this.lblYScaleBoneOptions.AutoSize = true;
            this.lblYScaleBoneOptions.Location = new System.Drawing.Point(4, 40);
            this.lblYScaleBoneOptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYScaleBoneOptions.Name = "lblYScaleBoneOptions";
            this.lblYScaleBoneOptions.Size = new System.Drawing.Size(44, 13);
            this.lblYScaleBoneOptions.TabIndex = 2;
            this.lblYScaleBoneOptions.Text = "Y Scale";
            // 
            // nUDResizeBoneX
            // 
            this.nUDResizeBoneX.Location = new System.Drawing.Point(47, 15);
            this.nUDResizeBoneX.Margin = new System.Windows.Forms.Padding(2);
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
            this.nUDResizeBoneX.Size = new System.Drawing.Size(102, 20);
            this.nUDResizeBoneX.TabIndex = 1;
            this.nUDResizeBoneX.ValueChanged += new System.EventHandler(this.NudResizeBoneX_ValueChanged);
            // 
            // lblXScaleBoneOptions
            // 
            this.lblXScaleBoneOptions.AutoSize = true;
            this.lblXScaleBoneOptions.Location = new System.Drawing.Point(4, 17);
            this.lblXScaleBoneOptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblXScaleBoneOptions.Name = "lblXScaleBoneOptions";
            this.lblXScaleBoneOptions.Size = new System.Drawing.Size(44, 13);
            this.lblXScaleBoneOptions.TabIndex = 0;
            this.lblXScaleBoneOptions.Text = "X Scale";
            // 
            // lblBoneSelector
            // 
            this.lblBoneSelector.AutoSize = true;
            this.lblBoneSelector.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBoneSelector.Location = new System.Drawing.Point(3, 2);
            this.lblBoneSelector.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoneSelector.Name = "lblBoneSelector";
            this.lblBoneSelector.Size = new System.Drawing.Size(77, 13);
            this.lblBoneSelector.TabIndex = 12;
            this.lblBoneSelector.Text = "Selected Joint:";
            this.lblBoneSelector.Visible = false;
            // 
            // cbBoneSelector
            // 
            this.cbBoneSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoneSelector.FormattingEnabled = true;
            this.cbBoneSelector.Location = new System.Drawing.Point(4, 16);
            this.cbBoneSelector.Margin = new System.Windows.Forms.Padding(2);
            this.cbBoneSelector.Name = "cbBoneSelector";
            this.cbBoneSelector.Size = new System.Drawing.Size(152, 21);
            this.cbBoneSelector.TabIndex = 11;
            this.cbBoneSelector.Visible = false;
            this.cbBoneSelector.SelectedIndexChanged += new System.EventHandler(this.CbBoneSelector_SelectedIndexChanged);
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
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 625);
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
            this.groupBox1.Location = new System.Drawing.Point(5, 489);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 134);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Lighting";
            // 
            // chkInifintyFarLights
            // 
            this.chkInifintyFarLights.AutoSize = true;
            this.chkInifintyFarLights.Location = new System.Drawing.Point(13, 114);
            this.chkInifintyFarLights.Name = "chkInifintyFarLights";
            this.chkInifintyFarLights.Size = new System.Drawing.Size(108, 17);
            this.chkInifintyFarLights.TabIndex = 14;
            this.chkInifintyFarLights.Text = "Inifinitely far lights";
            this.chkInifintyFarLights.UseVisualStyleBackColor = true;
            this.chkInifintyFarLights.CheckedChanged += new System.EventHandler(this.ChkInifintyFarLights_CheckedChanged);
            // 
            // chkRightLight
            // 
            this.chkRightLight.AutoSize = true;
            this.chkRightLight.Location = new System.Drawing.Point(79, 97);
            this.chkRightLight.Name = "chkRightLight";
            this.chkRightLight.Size = new System.Drawing.Size(51, 17);
            this.chkRightLight.TabIndex = 13;
            this.chkRightLight.Text = "Right";
            this.chkRightLight.UseVisualStyleBackColor = true;
            this.chkRightLight.CheckedChanged += new System.EventHandler(this.ChkRight_CheckedChanged);
            // 
            // chkLeftLight
            // 
            this.chkLeftLight.AutoSize = true;
            this.chkLeftLight.Location = new System.Drawing.Point(79, 81);
            this.chkLeftLight.Name = "chkLeftLight";
            this.chkLeftLight.Size = new System.Drawing.Size(44, 17);
            this.chkLeftLight.TabIndex = 12;
            this.chkLeftLight.Text = "Left";
            this.chkLeftLight.UseVisualStyleBackColor = true;
            this.chkLeftLight.CheckedChanged += new System.EventHandler(this.ChkLeftLight_CheckedChanged);
            // 
            // chkRearLight
            // 
            this.chkRearLight.AutoSize = true;
            this.chkRearLight.Location = new System.Drawing.Point(13, 97);
            this.chkRearLight.Name = "chkRearLight";
            this.chkRearLight.Size = new System.Drawing.Size(49, 17);
            this.chkRearLight.TabIndex = 11;
            this.chkRearLight.Text = "Rear";
            this.chkRearLight.UseVisualStyleBackColor = true;
            this.chkRearLight.CheckedChanged += new System.EventHandler(this.ChkRearLight_CheckedChanged);
            // 
            // chkFrontLight
            // 
            this.chkFrontLight.AutoSize = true;
            this.chkFrontLight.Location = new System.Drawing.Point(13, 81);
            this.chkFrontLight.Name = "chkFrontLight";
            this.chkFrontLight.Size = new System.Drawing.Size(50, 17);
            this.chkFrontLight.TabIndex = 10;
            this.chkFrontLight.Text = "Front";
            this.chkFrontLight.UseVisualStyleBackColor = true;
            this.chkFrontLight.CheckedChanged += new System.EventHandler(this.ChkFrontLight_CheckedChanged);
            // 
            // lblLightPosZ
            // 
            this.lblLightPosZ.AutoSize = true;
            this.lblLightPosZ.Location = new System.Drawing.Point(7, 62);
            this.lblLightPosZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLightPosZ.Name = "lblLightPosZ";
            this.lblLightPosZ.Size = new System.Drawing.Size(14, 13);
            this.lblLightPosZ.TabIndex = 8;
            this.lblLightPosZ.Text = "Z";
            // 
            // hsbLightPosZ
            // 
            this.hsbLightPosZ.LargeChange = 1;
            this.hsbLightPosZ.Location = new System.Drawing.Point(28, 59);
            this.hsbLightPosZ.Maximum = 32767;
            this.hsbLightPosZ.Name = "hsbLightPosZ";
            this.hsbLightPosZ.Size = new System.Drawing.Size(110, 19);
            this.hsbLightPosZ.TabIndex = 7;
            this.hsbLightPosZ.ValueChanged += new System.EventHandler(this.HsbLightPosZ_ValueChanged);
            // 
            // lblLightPosY
            // 
            this.lblLightPosY.AutoSize = true;
            this.lblLightPosY.Location = new System.Drawing.Point(7, 42);
            this.lblLightPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLightPosY.Name = "lblLightPosY";
            this.lblLightPosY.Size = new System.Drawing.Size(14, 13);
            this.lblLightPosY.TabIndex = 6;
            this.lblLightPosY.Text = "Y";
            // 
            // hsbLightPosY
            // 
            this.hsbLightPosY.LargeChange = 1;
            this.hsbLightPosY.Location = new System.Drawing.Point(28, 39);
            this.hsbLightPosY.Maximum = 32767;
            this.hsbLightPosY.Name = "hsbLightPosY";
            this.hsbLightPosY.Size = new System.Drawing.Size(110, 19);
            this.hsbLightPosY.TabIndex = 5;
            this.hsbLightPosY.ValueChanged += new System.EventHandler(this.HsbLightPosY_ValueChanged);
            // 
            // lblLightPosX
            // 
            this.lblLightPosX.AutoSize = true;
            this.lblLightPosX.Location = new System.Drawing.Point(7, 22);
            this.lblLightPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLightPosX.Name = "lblLightPosX";
            this.lblLightPosX.Size = new System.Drawing.Size(14, 13);
            this.lblLightPosX.TabIndex = 4;
            this.lblLightPosX.Text = "X";
            // 
            // hsbLightPosX
            // 
            this.hsbLightPosX.LargeChange = 1;
            this.hsbLightPosX.Location = new System.Drawing.Point(28, 19);
            this.hsbLightPosX.Maximum = 32767;
            this.hsbLightPosX.Name = "hsbLightPosX";
            this.hsbLightPosX.Size = new System.Drawing.Size(110, 19);
            this.hsbLightPosX.TabIndex = 3;
            this.hsbLightPosX.ValueChanged += new System.EventHandler(this.HsbLightPosX_ValueChanged);
            // 
            // cbWeapon
            // 
            this.cbWeapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeapon.FormattingEnabled = true;
            this.cbWeapon.Location = new System.Drawing.Point(90, 465);
            this.cbWeapon.Name = "cbWeapon";
            this.cbWeapon.Size = new System.Drawing.Size(64, 21);
            this.cbWeapon.TabIndex = 12;
            this.cbWeapon.Visible = false;
            this.cbWeapon.SelectedIndexChanged += new System.EventHandler(this.CbWeapon_SelectedIndexChanged);
            // 
            // cbBattleAnimation
            // 
            this.cbBattleAnimation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBattleAnimation.FormattingEnabled = true;
            this.cbBattleAnimation.Location = new System.Drawing.Point(90, 444);
            this.cbBattleAnimation.Name = "cbBattleAnimation";
            this.cbBattleAnimation.Size = new System.Drawing.Size(64, 21);
            this.cbBattleAnimation.TabIndex = 11;
            this.cbBattleAnimation.Visible = false;
            this.cbBattleAnimation.SelectedIndexChanged += new System.EventHandler(this.CbBattleAnimation_SelectedIndexChanged);
            // 
            // lblWeapon
            // 
            this.lblWeapon.ForeColor = System.Drawing.SystemColors.Control;
            this.lblWeapon.Location = new System.Drawing.Point(39, 467);
            this.lblWeapon.Name = "lblWeapon";
            this.lblWeapon.Size = new System.Drawing.Size(51, 14);
            this.lblWeapon.TabIndex = 10;
            this.lblWeapon.Text = "Weapon:";
            this.lblWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblWeapon.Visible = false;
            // 
            // lblBattleAnimation
            // 
            this.lblBattleAnimation.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBattleAnimation.Location = new System.Drawing.Point(4, 446);
            this.lblBattleAnimation.Name = "lblBattleAnimation";
            this.lblBattleAnimation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBattleAnimation.Size = new System.Drawing.Size(86, 14);
            this.lblBattleAnimation.TabIndex = 9;
            this.lblBattleAnimation.Text = "Battle Animation:";
            this.lblBattleAnimation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBattleAnimation.Visible = false;
            // 
            // btnComputeWeaponPosition
            // 
            this.btnComputeWeaponPosition.Location = new System.Drawing.Point(5, 407);
            this.btnComputeWeaponPosition.Name = "btnComputeWeaponPosition";
            this.btnComputeWeaponPosition.Size = new System.Drawing.Size(148, 35);
            this.btnComputeWeaponPosition.TabIndex = 8;
            this.btnComputeWeaponPosition.Text = "Compute attached weapon position";
            this.btnComputeWeaponPosition.UseVisualStyleBackColor = true;
            this.btnComputeWeaponPosition.Visible = false;
            this.btnComputeWeaponPosition.Click += new System.EventHandler(this.BtnComputeWeaponPosition_Click);
            // 
            // btnComputeGroundHeight
            // 
            this.btnComputeGroundHeight.Location = new System.Drawing.Point(5, 386);
            this.btnComputeGroundHeight.Name = "btnComputeGroundHeight";
            this.btnComputeGroundHeight.Size = new System.Drawing.Size(148, 22);
            this.btnComputeGroundHeight.TabIndex = 7;
            this.btnComputeGroundHeight.Text = "Compute ground height";
            this.btnComputeGroundHeight.UseVisualStyleBackColor = true;
            this.btnComputeGroundHeight.Visible = false;
            this.btnComputeGroundHeight.Click += new System.EventHandler(this.BtnComputeGroundHeight_Click);
            // 
            // chkShowLastFrameGhost
            // 
            this.chkShowLastFrameGhost.AutoSize = true;
            this.chkShowLastFrameGhost.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowLastFrameGhost.Location = new System.Drawing.Point(10, 338);
            this.chkShowLastFrameGhost.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowLastFrameGhost.Name = "chkShowLastFrameGhost";
            this.chkShowLastFrameGhost.Size = new System.Drawing.Size(140, 17);
            this.chkShowLastFrameGhost.TabIndex = 6;
            this.chkShowLastFrameGhost.Text = "Overlap last frame ghost";
            this.chkShowLastFrameGhost.UseVisualStyleBackColor = true;
            this.chkShowLastFrameGhost.CheckedChanged += new System.EventHandler(this.ChkShowLastFrameGhost_CheckedChanged);
            // 
            // chkShowGround
            // 
            this.chkShowGround.AutoSize = true;
            this.chkShowGround.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowGround.Location = new System.Drawing.Point(10, 353);
            this.chkShowGround.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowGround.Name = "chkShowGround";
            this.chkShowGround.Size = new System.Drawing.Size(89, 17);
            this.chkShowGround.TabIndex = 5;
            this.chkShowGround.Text = "Show ground";
            this.chkShowGround.UseVisualStyleBackColor = true;
            this.chkShowGround.CheckedChanged += new System.EventHandler(this.ChkShowGround_CheckedChanged);
            // 
            // gbSelectedPieceFrame
            // 
            this.gbSelectedPieceFrame.Controls.Add(this.gbRotateFrame);
            this.gbSelectedPieceFrame.Controls.Add(this.gbRepositionFrame);
            this.gbSelectedPieceFrame.Controls.Add(this.gbResizeFrame);
            this.gbSelectedPieceFrame.Enabled = false;
            this.gbSelectedPieceFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSelectedPieceFrame.Location = new System.Drawing.Point(5, 3);
            this.gbSelectedPieceFrame.Margin = new System.Windows.Forms.Padding(2);
            this.gbSelectedPieceFrame.Name = "gbSelectedPieceFrame";
            this.gbSelectedPieceFrame.Padding = new System.Windows.Forms.Padding(2);
            this.gbSelectedPieceFrame.Size = new System.Drawing.Size(148, 333);
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
            this.gbRotateFrame.Location = new System.Drawing.Point(5, 192);
            this.gbRotateFrame.Name = "gbRotateFrame";
            this.gbRotateFrame.Size = new System.Drawing.Size(138, 137);
            this.gbRotateFrame.TabIndex = 2;
            this.gbRotateFrame.TabStop = false;
            this.gbRotateFrame.Text = "Rotation";
            // 
            // txtRotateGamma
            // 
            this.txtRotateGamma.Location = new System.Drawing.Point(101, 111);
            this.txtRotateGamma.Name = "txtRotateGamma";
            this.txtRotateGamma.Size = new System.Drawing.Size(28, 20);
            this.txtRotateGamma.TabIndex = 8;
            this.txtRotateGamma.Text = "0";
            this.txtRotateGamma.TextChanged += new System.EventHandler(this.TxtRotateGamma_TextChanged);
            // 
            // hsbRotateGamma
            // 
            this.hsbRotateGamma.LargeChange = 1;
            this.hsbRotateGamma.Location = new System.Drawing.Point(7, 111);
            this.hsbRotateGamma.Maximum = 360;
            this.hsbRotateGamma.Name = "hsbRotateGamma";
            this.hsbRotateGamma.Size = new System.Drawing.Size(90, 19);
            this.hsbRotateGamma.TabIndex = 7;
            this.hsbRotateGamma.ValueChanged += new System.EventHandler(this.HsbRotateGamma_ValueChanged);
            // 
            // lblRotateGamma
            // 
            this.lblRotateGamma.AutoSize = true;
            this.lblRotateGamma.Location = new System.Drawing.Point(5, 96);
            this.lblRotateGamma.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateGamma.Name = "lblRotateGamma";
            this.lblRotateGamma.Size = new System.Drawing.Size(119, 13);
            this.lblRotateGamma.TabIndex = 6;
            this.lblRotateGamma.Text = "Gamma rotation (Z-Axis)";
            // 
            // txtRotateBeta
            // 
            this.txtRotateBeta.Location = new System.Drawing.Point(102, 72);
            this.txtRotateBeta.Name = "txtRotateBeta";
            this.txtRotateBeta.Size = new System.Drawing.Size(28, 20);
            this.txtRotateBeta.TabIndex = 5;
            this.txtRotateBeta.Text = "0";
            this.txtRotateBeta.TextChanged += new System.EventHandler(this.TxtRotateBeta_TextChanged);
            // 
            // hsbRotateBeta
            // 
            this.hsbRotateBeta.LargeChange = 1;
            this.hsbRotateBeta.Location = new System.Drawing.Point(8, 72);
            this.hsbRotateBeta.Maximum = 360;
            this.hsbRotateBeta.Name = "hsbRotateBeta";
            this.hsbRotateBeta.Size = new System.Drawing.Size(90, 19);
            this.hsbRotateBeta.TabIndex = 4;
            this.hsbRotateBeta.ValueChanged += new System.EventHandler(this.HsbRotateBeta_ValueChanged);
            // 
            // lblRotateBeta
            // 
            this.lblRotateBeta.AutoSize = true;
            this.lblRotateBeta.Location = new System.Drawing.Point(5, 57);
            this.lblRotateBeta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateBeta.Name = "lblRotateBeta";
            this.lblRotateBeta.Size = new System.Drawing.Size(105, 13);
            this.lblRotateBeta.TabIndex = 3;
            this.lblRotateBeta.Text = "Beta rotation (Y-Axis)";
            // 
            // lblRotateAlpha
            // 
            this.lblRotateAlpha.AutoSize = true;
            this.lblRotateAlpha.Location = new System.Drawing.Point(5, 18);
            this.lblRotateAlpha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateAlpha.Name = "lblRotateAlpha";
            this.lblRotateAlpha.Size = new System.Drawing.Size(110, 13);
            this.lblRotateAlpha.TabIndex = 2;
            this.lblRotateAlpha.Text = "Alpha rotation (X-Axis)";
            // 
            // txtRotateAlpha
            // 
            this.txtRotateAlpha.Location = new System.Drawing.Point(102, 33);
            this.txtRotateAlpha.Name = "txtRotateAlpha";
            this.txtRotateAlpha.Size = new System.Drawing.Size(28, 20);
            this.txtRotateAlpha.TabIndex = 1;
            this.txtRotateAlpha.Text = "0";
            this.txtRotateAlpha.TextChanged += new System.EventHandler(this.TxtRotateAlpha_TextChanged);
            // 
            // hsbRotateAlpha
            // 
            this.hsbRotateAlpha.LargeChange = 1;
            this.hsbRotateAlpha.Location = new System.Drawing.Point(8, 33);
            this.hsbRotateAlpha.Maximum = 360;
            this.hsbRotateAlpha.Name = "hsbRotateAlpha";
            this.hsbRotateAlpha.Size = new System.Drawing.Size(90, 19);
            this.hsbRotateAlpha.TabIndex = 0;
            this.hsbRotateAlpha.ValueChanged += new System.EventHandler(this.HsbRotateAlpha_ValueChanged);
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
            this.gbRepositionFrame.Location = new System.Drawing.Point(5, 103);
            this.gbRepositionFrame.Name = "gbRepositionFrame";
            this.gbRepositionFrame.Size = new System.Drawing.Size(138, 86);
            this.gbRepositionFrame.TabIndex = 1;
            this.gbRepositionFrame.TabStop = false;
            this.gbRepositionFrame.Text = "Reposition";
            // 
            // txtRepositionZ
            // 
            this.txtRepositionZ.Location = new System.Drawing.Point(104, 60);
            this.txtRepositionZ.Name = "txtRepositionZ";
            this.txtRepositionZ.Size = new System.Drawing.Size(28, 20);
            this.txtRepositionZ.TabIndex = 8;
            this.txtRepositionZ.Text = "0";
            this.txtRepositionZ.TextChanged += new System.EventHandler(this.TxtRepositionZ_TextChanged);
            // 
            // hsbRepositionZ
            // 
            this.hsbRepositionZ.LargeChange = 1;
            this.hsbRepositionZ.Location = new System.Drawing.Point(22, 60);
            this.hsbRepositionZ.Maximum = 500;
            this.hsbRepositionZ.Minimum = -500;
            this.hsbRepositionZ.Name = "hsbRepositionZ";
            this.hsbRepositionZ.Size = new System.Drawing.Size(78, 19);
            this.hsbRepositionZ.TabIndex = 7;
            this.hsbRepositionZ.ValueChanged += new System.EventHandler(this.HsbRepositionZ_ValueChanged);
            // 
            // lblRepositionZ
            // 
            this.lblRepositionZ.AutoSize = true;
            this.lblRepositionZ.Location = new System.Drawing.Point(6, 61);
            this.lblRepositionZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionZ.Name = "lblRepositionZ";
            this.lblRepositionZ.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionZ.TabIndex = 6;
            this.lblRepositionZ.Text = "Z";
            // 
            // txtRepositionY
            // 
            this.txtRepositionY.Location = new System.Drawing.Point(104, 40);
            this.txtRepositionY.Name = "txtRepositionY";
            this.txtRepositionY.Size = new System.Drawing.Size(28, 20);
            this.txtRepositionY.TabIndex = 5;
            this.txtRepositionY.Text = "0";
            this.txtRepositionY.TextChanged += new System.EventHandler(this.TxtRepositionY_TextChanged);
            // 
            // hsbRepositionY
            // 
            this.hsbRepositionY.LargeChange = 1;
            this.hsbRepositionY.Location = new System.Drawing.Point(22, 40);
            this.hsbRepositionY.Maximum = 500;
            this.hsbRepositionY.Minimum = -500;
            this.hsbRepositionY.Name = "hsbRepositionY";
            this.hsbRepositionY.Size = new System.Drawing.Size(78, 19);
            this.hsbRepositionY.TabIndex = 4;
            this.hsbRepositionY.ValueChanged += new System.EventHandler(this.HsbRepositionY_ValueChanged);
            // 
            // lblRepositionY
            // 
            this.lblRepositionY.AutoSize = true;
            this.lblRepositionY.Location = new System.Drawing.Point(6, 42);
            this.lblRepositionY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionY.Name = "lblRepositionY";
            this.lblRepositionY.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionY.TabIndex = 3;
            this.lblRepositionY.Text = "Y";
            // 
            // lblRepositionX
            // 
            this.lblRepositionX.AutoSize = true;
            this.lblRepositionX.Location = new System.Drawing.Point(6, 23);
            this.lblRepositionX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionX.Name = "lblRepositionX";
            this.lblRepositionX.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionX.TabIndex = 2;
            this.lblRepositionX.Text = "X";
            // 
            // txtRepositionX
            // 
            this.txtRepositionX.Location = new System.Drawing.Point(104, 20);
            this.txtRepositionX.Name = "txtRepositionX";
            this.txtRepositionX.Size = new System.Drawing.Size(28, 20);
            this.txtRepositionX.TabIndex = 1;
            this.txtRepositionX.Text = "0";
            this.txtRepositionX.TextChanged += new System.EventHandler(this.TxtRepositionX_TextChanged);
            // 
            // hsbRepositionX
            // 
            this.hsbRepositionX.LargeChange = 1;
            this.hsbRepositionX.Location = new System.Drawing.Point(22, 20);
            this.hsbRepositionX.Maximum = 500;
            this.hsbRepositionX.Minimum = -500;
            this.hsbRepositionX.Name = "hsbRepositionX";
            this.hsbRepositionX.Size = new System.Drawing.Size(78, 19);
            this.hsbRepositionX.TabIndex = 0;
            this.hsbRepositionX.ValueChanged += new System.EventHandler(this.HsbRepositionX_ValueChanged);
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
            this.gbResizeFrame.Location = new System.Drawing.Point(5, 14);
            this.gbResizeFrame.Name = "gbResizeFrame";
            this.gbResizeFrame.Size = new System.Drawing.Size(138, 86);
            this.gbResizeFrame.TabIndex = 0;
            this.gbResizeFrame.TabStop = false;
            this.gbResizeFrame.Text = "Resize";
            // 
            // txtResizePieceZ
            // 
            this.txtResizePieceZ.Location = new System.Drawing.Point(104, 60);
            this.txtResizePieceZ.Name = "txtResizePieceZ";
            this.txtResizePieceZ.Size = new System.Drawing.Size(28, 20);
            this.txtResizePieceZ.TabIndex = 8;
            this.txtResizePieceZ.Text = "100";
            this.txtResizePieceZ.TextChanged += new System.EventHandler(this.TxtResizePieceZ_TextChanged);
            // 
            // hsbResizePieceZ
            // 
            this.hsbResizePieceZ.LargeChange = 1;
            this.hsbResizePieceZ.Location = new System.Drawing.Point(22, 60);
            this.hsbResizePieceZ.Maximum = 500;
            this.hsbResizePieceZ.Name = "hsbResizePieceZ";
            this.hsbResizePieceZ.Size = new System.Drawing.Size(78, 19);
            this.hsbResizePieceZ.TabIndex = 7;
            this.hsbResizePieceZ.Value = 100;
            this.hsbResizePieceZ.ValueChanged += new System.EventHandler(this.HsbResizePieceZ_ValueChanged);
            // 
            // lblResizePieceZ
            // 
            this.lblResizePieceZ.AutoSize = true;
            this.lblResizePieceZ.Location = new System.Drawing.Point(6, 61);
            this.lblResizePieceZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceZ.Name = "lblResizePieceZ";
            this.lblResizePieceZ.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceZ.TabIndex = 6;
            this.lblResizePieceZ.Text = "Z";
            // 
            // txtResizePieceY
            // 
            this.txtResizePieceY.Location = new System.Drawing.Point(104, 40);
            this.txtResizePieceY.Name = "txtResizePieceY";
            this.txtResizePieceY.Size = new System.Drawing.Size(28, 20);
            this.txtResizePieceY.TabIndex = 5;
            this.txtResizePieceY.Text = "100";
            this.txtResizePieceY.TextChanged += new System.EventHandler(this.TxtResizePieceY_TextChanged);
            // 
            // hsbResizePieceY
            // 
            this.hsbResizePieceY.LargeChange = 1;
            this.hsbResizePieceY.Location = new System.Drawing.Point(22, 40);
            this.hsbResizePieceY.Maximum = 500;
            this.hsbResizePieceY.Name = "hsbResizePieceY";
            this.hsbResizePieceY.Size = new System.Drawing.Size(78, 19);
            this.hsbResizePieceY.TabIndex = 4;
            this.hsbResizePieceY.Value = 100;
            this.hsbResizePieceY.ValueChanged += new System.EventHandler(this.HsbResizePieceY_ValueChanged);
            // 
            // lblResizePieceY
            // 
            this.lblResizePieceY.AutoSize = true;
            this.lblResizePieceY.Location = new System.Drawing.Point(6, 42);
            this.lblResizePieceY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceY.Name = "lblResizePieceY";
            this.lblResizePieceY.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceY.TabIndex = 3;
            this.lblResizePieceY.Text = "Y";
            // 
            // lblResizePieceX
            // 
            this.lblResizePieceX.AutoSize = true;
            this.lblResizePieceX.Location = new System.Drawing.Point(6, 23);
            this.lblResizePieceX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceX.Name = "lblResizePieceX";
            this.lblResizePieceX.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceX.TabIndex = 2;
            this.lblResizePieceX.Text = "X";
            // 
            // txtResizePieceX
            // 
            this.txtResizePieceX.Location = new System.Drawing.Point(104, 20);
            this.txtResizePieceX.Name = "txtResizePieceX";
            this.txtResizePieceX.Size = new System.Drawing.Size(28, 20);
            this.txtResizePieceX.TabIndex = 1;
            this.txtResizePieceX.Text = "100";
            this.txtResizePieceX.TextChanged += new System.EventHandler(this.TxtResizePieceX_TextChanged);
            // 
            // hsbResizePieceX
            // 
            this.hsbResizePieceX.LargeChange = 1;
            this.hsbResizePieceX.Location = new System.Drawing.Point(22, 20);
            this.hsbResizePieceX.Maximum = 500;
            this.hsbResizePieceX.Name = "hsbResizePieceX";
            this.hsbResizePieceX.Size = new System.Drawing.Size(78, 19);
            this.hsbResizePieceX.TabIndex = 0;
            this.hsbResizePieceX.Value = 100;
            this.hsbResizePieceX.ValueChanged += new System.EventHandler(this.HsbResizePieceX_ValueChanged);
            // 
            // chkDListEnable
            // 
            this.chkDListEnable.AutoSize = true;
            this.chkDListEnable.ForeColor = System.Drawing.SystemColors.Control;
            this.chkDListEnable.Location = new System.Drawing.Point(10, 369);
            this.chkDListEnable.Margin = new System.Windows.Forms.Padding(2);
            this.chkDListEnable.Name = "chkDListEnable";
            this.chkDListEnable.Size = new System.Drawing.Size(121, 17);
            this.chkDListEnable.TabIndex = 3;
            this.chkDListEnable.Text = "Render using DLists";
            this.chkDListEnable.UseVisualStyleBackColor = true;
            this.chkDListEnable.CheckedChanged += new System.EventHandler(this.CheckBDListEnable_CheckedChanged);
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
            this.panel3.Location = new System.Drawing.Point(158, 578);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(416, 71);
            this.panel3.TabIndex = 8;
            // 
            // btnFramePrev
            // 
            this.btnFramePrev.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFramePrev.BackgroundImage")));
            this.btnFramePrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFramePrev.Location = new System.Drawing.Point(286, 7);
            this.btnFramePrev.Name = "btnFramePrev";
            this.btnFramePrev.Size = new System.Drawing.Size(32, 32);
            this.btnFramePrev.TabIndex = 25;
            this.btnFramePrev.UseVisualStyleBackColor = true;
            this.btnFramePrev.Visible = false;
            this.btnFramePrev.Click += new System.EventHandler(this.BtnFramePrev_Click);
            // 
            // btnFrameNext
            // 
            this.btnFrameNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFrameNext.BackgroundImage")));
            this.btnFrameNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameNext.Location = new System.Drawing.Point(348, 7);
            this.btnFrameNext.Name = "btnFrameNext";
            this.btnFrameNext.Size = new System.Drawing.Size(32, 32);
            this.btnFrameNext.TabIndex = 24;
            this.btnFrameNext.UseVisualStyleBackColor = true;
            this.btnFrameNext.Visible = false;
            this.btnFrameNext.Click += new System.EventHandler(this.BtnFrameNext_Click);
            // 
            // btnPlayStopAnim
            // 
            this.btnPlayStopAnim.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPlayStopAnim.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlayStopAnim.BackgroundImage")));
            this.btnPlayStopAnim.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayStopAnim.Location = new System.Drawing.Point(317, 7);
            this.btnPlayStopAnim.Name = "btnPlayStopAnim";
            this.btnPlayStopAnim.Size = new System.Drawing.Size(32, 32);
            this.btnPlayStopAnim.TabIndex = 23;
            this.btnPlayStopAnim.UseVisualStyleBackColor = true;
            this.btnPlayStopAnim.Visible = false;
            this.btnPlayStopAnim.CheckedChanged += new System.EventHandler(this.BtnPlayStopAnm_CheckedChanged);
            // 
            // btnFrameEnd
            // 
            this.btnFrameEnd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFrameEnd.BackgroundImage")));
            this.btnFrameEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameEnd.Location = new System.Drawing.Point(379, 7);
            this.btnFrameEnd.Name = "btnFrameEnd";
            this.btnFrameEnd.Size = new System.Drawing.Size(32, 32);
            this.btnFrameEnd.TabIndex = 22;
            this.btnFrameEnd.UseVisualStyleBackColor = true;
            this.btnFrameEnd.Visible = false;
            this.btnFrameEnd.Click += new System.EventHandler(this.BtnFrameEnd_Click);
            // 
            // btnFrameBegin
            // 
            this.btnFrameBegin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFrameBegin.BackgroundImage")));
            this.btnFrameBegin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFrameBegin.Location = new System.Drawing.Point(255, 7);
            this.btnFrameBegin.Name = "btnFrameBegin";
            this.btnFrameBegin.Size = new System.Drawing.Size(32, 32);
            this.btnFrameBegin.TabIndex = 21;
            this.btnFrameBegin.UseVisualStyleBackColor = true;
            this.btnFrameBegin.Visible = false;
            this.btnFrameBegin.Click += new System.EventHandler(this.BtnFrameBegin_Click);
            // 
            // chkShowBones
            // 
            this.chkShowBones.AutoSize = true;
            this.chkShowBones.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowBones.Location = new System.Drawing.Point(192, 51);
            this.chkShowBones.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowBones.Name = "chkShowBones";
            this.chkShowBones.Size = new System.Drawing.Size(86, 17);
            this.chkShowBones.TabIndex = 19;
            this.chkShowBones.Text = "Show Bones";
            this.chkShowBones.UseVisualStyleBackColor = true;
            this.chkShowBones.Visible = false;
            this.chkShowBones.CheckedChanged += new System.EventHandler(this.ChkShowBones_CheckedChanged);
            // 
            // btnInterpolateAnimation
            // 
            this.btnInterpolateAnimation.Location = new System.Drawing.Point(288, 47);
            this.btnInterpolateAnimation.Margin = new System.Windows.Forms.Padding(2);
            this.btnInterpolateAnimation.Name = "btnInterpolateAnimation";
            this.btnInterpolateAnimation.Size = new System.Drawing.Size(121, 21);
            this.btnInterpolateAnimation.TabIndex = 18;
            this.btnInterpolateAnimation.Text = "Interpolate Animation";
            this.btnInterpolateAnimation.UseVisualStyleBackColor = true;
            this.btnInterpolateAnimation.Visible = false;
            this.btnInterpolateAnimation.Click += new System.EventHandler(this.BtnInterpolateAnimation_Click);
            // 
            // txtCopyPasteFrame
            // 
            this.txtCopyPasteFrame.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtCopyPasteFrame.CausesValidation = false;
            this.txtCopyPasteFrame.Location = new System.Drawing.Point(70, 47);
            this.txtCopyPasteFrame.Margin = new System.Windows.Forms.Padding(2);
            this.txtCopyPasteFrame.Name = "txtCopyPasteFrame";
            this.txtCopyPasteFrame.ReadOnly = true;
            this.txtCopyPasteFrame.ShortcutsEnabled = false;
            this.txtCopyPasteFrame.Size = new System.Drawing.Size(105, 20);
            this.txtCopyPasteFrame.TabIndex = 17;
            this.txtCopyPasteFrame.TabStop = false;
            this.txtCopyPasteFrame.Visible = false;
            // 
            // btnPasteFrame
            // 
            this.btnPasteFrame.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPasteFrame.BackgroundImage")));
            this.btnPasteFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPasteFrame.Enabled = false;
            this.btnPasteFrame.Location = new System.Drawing.Point(38, 36);
            this.btnPasteFrame.Margin = new System.Windows.Forms.Padding(2);
            this.btnPasteFrame.Name = "btnPasteFrame";
            this.btnPasteFrame.Size = new System.Drawing.Size(32, 32);
            this.btnPasteFrame.TabIndex = 16;
            this.btnPasteFrame.UseVisualStyleBackColor = true;
            this.btnPasteFrame.Visible = false;
            this.btnPasteFrame.Click += new System.EventHandler(this.BtnPasteFrame_Click);
            // 
            // btnCopyFrame
            // 
            this.btnCopyFrame.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCopyFrame.BackgroundImage")));
            this.btnCopyFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCopyFrame.Location = new System.Drawing.Point(7, 36);
            this.btnCopyFrame.Margin = new System.Windows.Forms.Padding(2);
            this.btnCopyFrame.Name = "btnCopyFrame";
            this.btnCopyFrame.Size = new System.Drawing.Size(32, 32);
            this.btnCopyFrame.TabIndex = 15;
            this.btnCopyFrame.UseVisualStyleBackColor = true;
            this.btnCopyFrame.Visible = false;
            this.btnCopyFrame.Click += new System.EventHandler(this.BtnCopyFrame_Click);
            // 
            // tbCurrentFrameScroll
            // 
            this.tbCurrentFrameScroll.Enabled = false;
            this.tbCurrentFrameScroll.Location = new System.Drawing.Point(91, 2);
            this.tbCurrentFrameScroll.Margin = new System.Windows.Forms.Padding(2);
            this.tbCurrentFrameScroll.Maximum = 5;
            this.tbCurrentFrameScroll.Name = "tbCurrentFrameScroll";
            this.tbCurrentFrameScroll.Size = new System.Drawing.Size(160, 45);
            this.tbCurrentFrameScroll.TabIndex = 14;
            this.tbCurrentFrameScroll.Visible = false;
            this.tbCurrentFrameScroll.Scroll += new System.EventHandler(this.TbCurrentFrameScroll_Scroll);
            this.tbCurrentFrameScroll.ValueChanged += new System.EventHandler(this.TbCurrentFrameScroll_ValueChanged);
            // 
            // lblAnimationFrame
            // 
            this.lblAnimationFrame.AutoSize = true;
            this.lblAnimationFrame.ForeColor = System.Drawing.SystemColors.Control;
            this.lblAnimationFrame.Location = new System.Drawing.Point(7, 12);
            this.lblAnimationFrame.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAnimationFrame.Name = "lblAnimationFrame";
            this.lblAnimationFrame.Size = new System.Drawing.Size(39, 13);
            this.lblAnimationFrame.TabIndex = 13;
            this.lblAnimationFrame.Text = "Frame:";
            this.lblAnimationFrame.Visible = false;
            // 
            // txtAnimationFrame
            // 
            this.txtAnimationFrame.Location = new System.Drawing.Point(46, 8);
            this.txtAnimationFrame.Margin = new System.Windows.Forms.Padding(2);
            this.txtAnimationFrame.Name = "txtAnimationFrame";
            this.txtAnimationFrame.ReadOnly = true;
            this.txtAnimationFrame.Size = new System.Drawing.Size(42, 20);
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
            this.skeletonToolStripMenuItem,
            this.textureToolStripMenuItem,
            this.animationToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(734, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFieldSkeletonToolStripMenuItem,
            this.loadBattleMagicSkeletonToolStripMenuItem,
            this.loadRSDResourceToolStripMenuItem,
            this.loadPModelToolStripMenuItem,
            this.load3DSToolStripMenuItem,
            this.loadTMDToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveSkeletonToolStripMenuItem,
            this.saveSkeletonAsToolStripMenuItem,
            this.toolStripSeparator9,
            this.Import3DSFixingPositionToolStripMenuItem,
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem,
            this.toolStripSeparator2,
            this.extiToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadFieldSkeletonToolStripMenuItem
            // 
            this.loadFieldSkeletonToolStripMenuItem.Name = "loadFieldSkeletonToolStripMenuItem";
            this.loadFieldSkeletonToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.loadFieldSkeletonToolStripMenuItem.Text = "Load Field/World Skeleton";
            this.loadFieldSkeletonToolStripMenuItem.Click += new System.EventHandler(this.LoadFieldSkeletonToolStripMenuItem_Click);
            // 
            // loadBattleMagicSkeletonToolStripMenuItem
            // 
            this.loadBattleMagicSkeletonToolStripMenuItem.Name = "loadBattleMagicSkeletonToolStripMenuItem";
            this.loadBattleMagicSkeletonToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.loadBattleMagicSkeletonToolStripMenuItem.Text = "Load Battle/Magic Skeleton";
            this.loadBattleMagicSkeletonToolStripMenuItem.Click += new System.EventHandler(this.LoadBattleMagicSkeletonToolStripMenuItem_Click);
            // 
            // loadRSDResourceToolStripMenuItem
            // 
            this.loadRSDResourceToolStripMenuItem.Name = "loadRSDResourceToolStripMenuItem";
            this.loadRSDResourceToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.loadRSDResourceToolStripMenuItem.Text = "Load RSD Resource";
            this.loadRSDResourceToolStripMenuItem.Click += new System.EventHandler(this.LoadRSDResourceToolStripMenuItem_Click);
            // 
            // loadPModelToolStripMenuItem
            // 
            this.loadPModelToolStripMenuItem.Name = "loadPModelToolStripMenuItem";
            this.loadPModelToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.loadPModelToolStripMenuItem.Text = "Load P Model";
            this.loadPModelToolStripMenuItem.Click += new System.EventHandler(this.LoadPModelToolStripMenuItem_Click);
            // 
            // load3DSToolStripMenuItem
            // 
            this.load3DSToolStripMenuItem.Name = "load3DSToolStripMenuItem";
            this.load3DSToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.load3DSToolStripMenuItem.Text = "Load 3DS";
            this.load3DSToolStripMenuItem.Click += new System.EventHandler(this.Load3DSToolStripMenuItem_Click);
            // 
            // loadTMDToolStripMenuItem
            // 
            this.loadTMDToolStripMenuItem.Name = "loadTMDToolStripMenuItem";
            this.loadTMDToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.loadTMDToolStripMenuItem.Text = "Load TMD";
            this.loadTMDToolStripMenuItem.Click += new System.EventHandler(this.LoadTMDToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(257, 6);
            // 
            // saveSkeletonToolStripMenuItem
            // 
            this.saveSkeletonToolStripMenuItem.Enabled = false;
            this.saveSkeletonToolStripMenuItem.Name = "saveSkeletonToolStripMenuItem";
            this.saveSkeletonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSkeletonToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.saveSkeletonToolStripMenuItem.Text = "Save Skeleton";
            this.saveSkeletonToolStripMenuItem.Click += new System.EventHandler(this.SaveSkeletonToolStripMenuItem_Click);
            // 
            // saveSkeletonAsToolStripMenuItem
            // 
            this.saveSkeletonAsToolStripMenuItem.Enabled = false;
            this.saveSkeletonAsToolStripMenuItem.Name = "saveSkeletonAsToolStripMenuItem";
            this.saveSkeletonAsToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.saveSkeletonAsToolStripMenuItem.Text = "Save Skeleton/RSD/Model As...";
            this.saveSkeletonAsToolStripMenuItem.Click += new System.EventHandler(this.SaveSkeletonAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(257, 6);
            // 
            // Import3DSFixingPositionToolStripMenuItem
            // 
            this.Import3DSFixingPositionToolStripMenuItem.CheckOnClick = true;
            this.Import3DSFixingPositionToolStripMenuItem.Name = "Import3DSFixingPositionToolStripMenuItem";
            this.Import3DSFixingPositionToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.Import3DSFixingPositionToolStripMenuItem.Text = "Fix position of .3DS on import";
            this.Import3DSFixingPositionToolStripMenuItem.Click += new System.EventHandler(this.Import3DSFixingPositionToolStripMenuItem_Click);
            // 
            // DontCheckDuplicatedPolysVertsToolStripMenuItem
            // 
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem.CheckOnClick = true;
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem.Name = "DontCheckDuplicatedPolysVertsToolStripMenuItem";
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem.Text = "Don\'t Check Duplicated Polys/Verts";
            this.DontCheckDuplicatedPolysVertsToolStripMenuItem.Click += new System.EventHandler(this.DontCheckRepeatedPolysVertsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(257, 6);
            // 
            // extiToolStripMenuItem
            // 
            this.extiToolStripMenuItem.Name = "extiToolStripMenuItem";
            this.extiToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.extiToolStripMenuItem.Text = "Exit";
            this.extiToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem2
            // 
            this.editToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator6,
            this.ShowAxesToolStripMenuItem,
            this.toolStripSeparator10,
            this.resetCameraToolStripMenuItem,
            this.toolStripSeparator7,
            this.uIOpacityToolStripMenuItem});
            this.editToolStripMenuItem2.Name = "editToolStripMenuItem2";
            this.editToolStripMenuItem2.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem2.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(218, 6);
            // 
            // ShowAxesToolStripMenuItem
            // 
            this.ShowAxesToolStripMenuItem.CheckOnClick = true;
            this.ShowAxesToolStripMenuItem.Name = "ShowAxesToolStripMenuItem";
            this.ShowAxesToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.ShowAxesToolStripMenuItem.Text = "Show Axes";
            this.ShowAxesToolStripMenuItem.Click += new System.EventHandler(this.DrawAxesToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(218, 6);
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.ShortcutKeyDisplayString = "CTRL+Home";
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.resetCameraToolStripMenuItem.Text = "Reset Camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.ResetCameraToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(218, 6);
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
            this.uIOpacityToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.uIOpacityToolStripMenuItem.Text = "UI Opacity";
            // 
            // tsUIOpacity100
            // 
            this.tsUIOpacity100.Checked = true;
            this.tsUIOpacity100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsUIOpacity100.Name = "tsUIOpacity100";
            this.tsUIOpacity100.Size = new System.Drawing.Size(102, 22);
            this.tsUIOpacity100.Text = "100%";
            this.tsUIOpacity100.Click += new System.EventHandler(this.TsUIOpacity100_Click);
            // 
            // tsUIOpacity90
            // 
            this.tsUIOpacity90.Name = "tsUIOpacity90";
            this.tsUIOpacity90.Size = new System.Drawing.Size(102, 22);
            this.tsUIOpacity90.Text = "90%";
            this.tsUIOpacity90.Click += new System.EventHandler(this.TsUIOpacity90_Click);
            // 
            // tsUIOpacity75
            // 
            this.tsUIOpacity75.Name = "tsUIOpacity75";
            this.tsUIOpacity75.Size = new System.Drawing.Size(102, 22);
            this.tsUIOpacity75.Text = "75%";
            this.tsUIOpacity75.Click += new System.EventHandler(this.TsUIOpacity75_Click);
            // 
            // tsUIOpacity50
            // 
            this.tsUIOpacity50.Name = "tsUIOpacity50";
            this.tsUIOpacity50.Size = new System.Drawing.Size(102, 22);
            this.tsUIOpacity50.Text = "50%";
            this.tsUIOpacity50.Click += new System.EventHandler(this.TsUIOpacity50_Click);
            // 
            // tsUIOpacity25
            // 
            this.tsUIOpacity25.Name = "tsUIOpacity25";
            this.tsUIOpacity25.Size = new System.Drawing.Size(102, 22);
            this.tsUIOpacity25.Text = "25%";
            this.tsUIOpacity25.Click += new System.EventHandler(this.TsUIOpacity25_Click);
            // 
            // skeletonToolStripMenuItem
            // 
            this.skeletonToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addJointToolStripMenuItem,
            this.editJointToolStripMenuItem,
            this.toolStripSeparator8,
            this.showNormalsToolStripMenuItem,
            this.normalsColorToolStripMenuItem,
            this.normalsScaleToolStripMenuItem,
            this.toolStripSeparator11,
            this.statisticsToolStripMenuItem});
            this.skeletonToolStripMenuItem.Name = "skeletonToolStripMenuItem";
            this.skeletonToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.skeletonToolStripMenuItem.Text = "&Skeleton";
            // 
            // addJointToolStripMenuItem
            // 
            this.addJointToolStripMenuItem.Enabled = false;
            this.addJointToolStripMenuItem.Name = "addJointToolStripMenuItem";
            this.addJointToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.addJointToolStripMenuItem.Text = "Add Joint";
            this.addJointToolStripMenuItem.Click += new System.EventHandler(this.AddJointToolStripMenuItem_Click);
            // 
            // editJointToolStripMenuItem
            // 
            this.editJointToolStripMenuItem.Enabled = false;
            this.editJointToolStripMenuItem.Name = "editJointToolStripMenuItem";
            this.editJointToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.editJointToolStripMenuItem.Text = "Edit Joint";
            this.editJointToolStripMenuItem.Click += new System.EventHandler(this.EditJointToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(148, 6);
            // 
            // showNormalsToolStripMenuItem
            // 
            this.showNormalsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showVertexNormalsToolStripMenuItem,
            this.showFaceNormalsToolStripMenuItem});
            this.showNormalsToolStripMenuItem.Enabled = false;
            this.showNormalsToolStripMenuItem.Name = "showNormalsToolStripMenuItem";
            this.showNormalsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.showNormalsToolStripMenuItem.Text = "Show Normals";
            // 
            // showVertexNormalsToolStripMenuItem
            // 
            this.showVertexNormalsToolStripMenuItem.CheckOnClick = true;
            this.showVertexNormalsToolStripMenuItem.Name = "showVertexNormalsToolStripMenuItem";
            this.showVertexNormalsToolStripMenuItem.ShortcutKeyDisplayString = "V";
            this.showVertexNormalsToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.showVertexNormalsToolStripMenuItem.Text = "Vertex";
            this.showVertexNormalsToolStripMenuItem.Click += new System.EventHandler(this.ShowVertexNormalsToolStripMenuItem_Click);
            // 
            // showFaceNormalsToolStripMenuItem
            // 
            this.showFaceNormalsToolStripMenuItem.CheckOnClick = true;
            this.showFaceNormalsToolStripMenuItem.Name = "showFaceNormalsToolStripMenuItem";
            this.showFaceNormalsToolStripMenuItem.ShortcutKeyDisplayString = "F";
            this.showFaceNormalsToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.showFaceNormalsToolStripMenuItem.Text = "Face";
            this.showFaceNormalsToolStripMenuItem.Click += new System.EventHandler(this.ShowFaceNormalsToolStripMenuItem_Click);
            // 
            // normalsColorToolStripMenuItem
            // 
            this.normalsColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem});
            this.normalsColorToolStripMenuItem.Name = "normalsColorToolStripMenuItem";
            this.normalsColorToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.normalsColorToolStripMenuItem.Text = "Normals Color";
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.CheckOnClick = true;
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.redToolStripMenuItem.Text = "Red";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.RedToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Checked = true;
            this.greenToolStripMenuItem.CheckOnClick = true;
            this.greenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.greenToolStripMenuItem.Text = "Green";
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.GreenToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.CheckOnClick = true;
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.blueToolStripMenuItem.Text = "Blue";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.BlueToolStripMenuItem_Click);
            // 
            // normalsScaleToolStripMenuItem
            // 
            this.normalsScaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oneftoolStripMenuItem,
            this.fiveftoolStripMenuItem,
            this.thirtyftoolStripMenuItem,
            this.thousandftoolStripMenuItem});
            this.normalsScaleToolStripMenuItem.Name = "normalsScaleToolStripMenuItem";
            this.normalsScaleToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.normalsScaleToolStripMenuItem.Text = "Normals Scale";
            // 
            // oneftoolStripMenuItem
            // 
            this.oneftoolStripMenuItem.Checked = true;
            this.oneftoolStripMenuItem.CheckOnClick = true;
            this.oneftoolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.oneftoolStripMenuItem.Name = "oneftoolStripMenuItem";
            this.oneftoolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.oneftoolStripMenuItem.Text = "1.0 (Field Models)";
            this.oneftoolStripMenuItem.Click += new System.EventHandler(this.OneftoolStripMenuItem_Click);
            // 
            // fiveftoolStripMenuItem
            // 
            this.fiveftoolStripMenuItem.CheckOnClick = true;
            this.fiveftoolStripMenuItem.Name = "fiveftoolStripMenuItem";
            this.fiveftoolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.fiveftoolStripMenuItem.Text = "5.0";
            this.fiveftoolStripMenuItem.Click += new System.EventHandler(this.FiveftoolStripMenuItem_Click);
            // 
            // thirtyftoolStripMenuItem
            // 
            this.thirtyftoolStripMenuItem.CheckOnClick = true;
            this.thirtyftoolStripMenuItem.Name = "thirtyftoolStripMenuItem";
            this.thirtyftoolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.thirtyftoolStripMenuItem.Text = "30.0 (Battle/Magic Models)";
            this.thirtyftoolStripMenuItem.Click += new System.EventHandler(this.ThirtyftoolStripMenuItem_Click);
            // 
            // thousandftoolStripMenuItem
            // 
            this.thousandftoolStripMenuItem.CheckOnClick = true;
            this.thousandftoolStripMenuItem.Name = "thousandftoolStripMenuItem";
            this.thousandftoolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.thousandftoolStripMenuItem.Text = "1000.0 (Locations)";
            this.thousandftoolStripMenuItem.Click += new System.EventHandler(this.ThousandftoolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(148, 6);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Enabled = false;
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.StatisticsToolStripMenuItem_Click);
            // 
            // textureToolStripMenuItem
            // 
            this.textureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TEXToPNGBatchConversionToolStripMenuItem});
            this.textureToolStripMenuItem.Name = "textureToolStripMenuItem";
            this.textureToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.textureToolStripMenuItem.Text = "&Texture";
            // 
            // TEXToPNGBatchConversionToolStripMenuItem
            // 
            this.TEXToPNGBatchConversionToolStripMenuItem.Name = "TEXToPNGBatchConversionToolStripMenuItem";
            this.TEXToPNGBatchConversionToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.TEXToPNGBatchConversionToolStripMenuItem.Text = ".TEX to .PNG Batch Conversion";
            this.TEXToPNGBatchConversionToolStripMenuItem.Click += new System.EventHandler(this.TEXToPNGBatchConversionToolStripMenuItem_Click);
            // 
            // animationToolStripMenuItem
            // 
            this.animationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFieldAnimationToolStripMenuItem,
            this.loadBattleMagicLimitsAnimationStripMenuItem,
            this.toolStripSeparator3,
            this.saveAnimationToolStripMenuItem,
            this.saveAnimationAsToolStripMenuItem,
            this.outputFramesDataTXTToolStripMenuItem,
            this.inputFramesDataTXTToolStripMenuItem,
            this.inputFramesDataTXTToolSelectiveStripMenuItem,
            this.mergeFramesDataTXTToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripMenuItem1,
            this.toolStripSeparator5,
            this.interpolateAllAnimationsToolStripMenuItem});
            this.animationToolStripMenuItem.Name = "animationToolStripMenuItem";
            this.animationToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.animationToolStripMenuItem.Text = "&Animation";
            // 
            // loadFieldAnimationToolStripMenuItem
            // 
            this.loadFieldAnimationToolStripMenuItem.Enabled = false;
            this.loadFieldAnimationToolStripMenuItem.Name = "loadFieldAnimationToolStripMenuItem";
            this.loadFieldAnimationToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.loadFieldAnimationToolStripMenuItem.Text = "Load Field Animation";
            this.loadFieldAnimationToolStripMenuItem.Click += new System.EventHandler(this.LoadFieldAnimationToolStripMenuItem_Click);
            // 
            // loadBattleMagicLimitsAnimationStripMenuItem
            // 
            this.loadBattleMagicLimitsAnimationStripMenuItem.Enabled = false;
            this.loadBattleMagicLimitsAnimationStripMenuItem.Name = "loadBattleMagicLimitsAnimationStripMenuItem";
            this.loadBattleMagicLimitsAnimationStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.loadBattleMagicLimitsAnimationStripMenuItem.Text = "Load Battle/Magic/Limits Animations";
            this.loadBattleMagicLimitsAnimationStripMenuItem.Click += new System.EventHandler(this.LoadBattleMagicLimitAnimationsStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(379, 6);
            // 
            // saveAnimationToolStripMenuItem
            // 
            this.saveAnimationToolStripMenuItem.Enabled = false;
            this.saveAnimationToolStripMenuItem.Name = "saveAnimationToolStripMenuItem";
            this.saveAnimationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.saveAnimationToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.saveAnimationToolStripMenuItem.Text = "Save Animation";
            this.saveAnimationToolStripMenuItem.Click += new System.EventHandler(this.SaveAnimationToolStripMenuItem_Click);
            // 
            // saveAnimationAsToolStripMenuItem
            // 
            this.saveAnimationAsToolStripMenuItem.Enabled = false;
            this.saveAnimationAsToolStripMenuItem.Name = "saveAnimationAsToolStripMenuItem";
            this.saveAnimationAsToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.saveAnimationAsToolStripMenuItem.Text = "Save Animation As...";
            this.saveAnimationAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAnimationAsToolStripMenuItem_Click);
            // 
            // outputFramesDataTXTToolStripMenuItem
            // 
            this.outputFramesDataTXTToolStripMenuItem.Enabled = false;
            this.outputFramesDataTXTToolStripMenuItem.Name = "outputFramesDataTXTToolStripMenuItem";
            this.outputFramesDataTXTToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.outputFramesDataTXTToolStripMenuItem.Text = "Output Frames Data to .TXT (Only Field Models)";
            this.outputFramesDataTXTToolStripMenuItem.Click += new System.EventHandler(this.OutputFramesDataAsToolStripMenuItem_Click);
            // 
            // inputFramesDataTXTToolStripMenuItem
            // 
            this.inputFramesDataTXTToolStripMenuItem.Enabled = false;
            this.inputFramesDataTXTToolStripMenuItem.Name = "inputFramesDataTXTToolStripMenuItem";
            this.inputFramesDataTXTToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.inputFramesDataTXTToolStripMenuItem.Text = "Input Frames Data from .TXT (Only Field Models)";
            this.inputFramesDataTXTToolStripMenuItem.Click += new System.EventHandler(this.InputFramesDataTXTToolStripMenuItem_Click);
            // 
            // inputFramesDataTXTToolSelectiveStripMenuItem
            // 
            this.inputFramesDataTXTToolSelectiveStripMenuItem.Enabled = false;
            this.inputFramesDataTXTToolSelectiveStripMenuItem.Name = "inputFramesDataTXTToolSelectiveStripMenuItem";
            this.inputFramesDataTXTToolSelectiveStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.inputFramesDataTXTToolSelectiveStripMenuItem.Text = "Input Frames Data from .TXT (Only Field Models, Selective)";
            this.inputFramesDataTXTToolSelectiveStripMenuItem.Click += new System.EventHandler(this.InputFramesDataFromTXTOnlyFieldModelsSelectiveToolStripMenuItem_Click);
            // 
            // mergeFramesDataTXTToolStripMenuItem
            // 
            this.mergeFramesDataTXTToolStripMenuItem.Enabled = false;
            this.mergeFramesDataTXTToolStripMenuItem.Name = "mergeFramesDataTXTToolStripMenuItem";
            this.mergeFramesDataTXTToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.mergeFramesDataTXTToolStripMenuItem.Text = "Merge Frames Data from .TXT (Only Field Models)";
            this.mergeFramesDataTXTToolStripMenuItem.Click += new System.EventHandler(this.MergeFramesDataTXTOnlyFieldModelsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(379, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFPS15,
            this.toolStripFPS30,
            this.toolStripFPS60});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(382, 22);
            this.toolStripMenuItem1.Text = "FPS for Play Animation";
            // 
            // toolStripFPS15
            // 
            this.toolStripFPS15.Checked = true;
            this.toolStripFPS15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripFPS15.Name = "toolStripFPS15";
            this.toolStripFPS15.Size = new System.Drawing.Size(86, 22);
            this.toolStripFPS15.Text = "15";
            this.toolStripFPS15.Click += new System.EventHandler(this.ToolStripFPS15_Click);
            // 
            // toolStripFPS30
            // 
            this.toolStripFPS30.Name = "toolStripFPS30";
            this.toolStripFPS30.Size = new System.Drawing.Size(86, 22);
            this.toolStripFPS30.Text = "30";
            this.toolStripFPS30.Click += new System.EventHandler(this.ToolStripFPS30_Click);
            // 
            // toolStripFPS60
            // 
            this.toolStripFPS60.Name = "toolStripFPS60";
            this.toolStripFPS60.Size = new System.Drawing.Size(86, 22);
            this.toolStripFPS60.Text = "60";
            this.toolStripFPS60.Click += new System.EventHandler(this.ToolStripFPS60_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(379, 6);
            // 
            // interpolateAllAnimationsToolStripMenuItem
            // 
            this.interpolateAllAnimationsToolStripMenuItem.Name = "interpolateAllAnimationsToolStripMenuItem";
            this.interpolateAllAnimationsToolStripMenuItem.Size = new System.Drawing.Size(382, 22);
            this.interpolateAllAnimationsToolStripMenuItem.Text = "Interpolate All Animations";
            this.interpolateAllAnimationsToolStripMenuItem.Click += new System.EventHandler(this.InterpolateAllAnimationsToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCharlgpToolStripMenuItem,
            this.showBattlelgpToolStripMenuItem,
            this.showMagiclgpToolStripMenuItem});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.databaseToolStripMenuItem.Text = "&Database";
            // 
            // showCharlgpToolStripMenuItem
            // 
            this.showCharlgpToolStripMenuItem.Name = "showCharlgpToolStripMenuItem";
            this.showCharlgpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.showCharlgpToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showCharlgpToolStripMenuItem.Text = "Show CHAR.LGP";
            this.showCharlgpToolStripMenuItem.Click += new System.EventHandler(this.ShowCharlgpToolStripMenuItem_Click);
            // 
            // showBattlelgpToolStripMenuItem
            // 
            this.showBattlelgpToolStripMenuItem.Name = "showBattlelgpToolStripMenuItem";
            this.showBattlelgpToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showBattlelgpToolStripMenuItem.Text = "Show BATTLE.LGP";
            this.showBattlelgpToolStripMenuItem.Click += new System.EventHandler(this.ShowBattlelgpToolStripMenuItem_Click);
            // 
            // showMagiclgpToolStripMenuItem
            // 
            this.showMagiclgpToolStripMenuItem.Name = "showMagiclgpToolStripMenuItem";
            this.showMagiclgpToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showMagiclgpToolStripMenuItem.Text = "Show MAGIC.LGP";
            this.showMagiclgpToolStripMenuItem.Click += new System.EventHandler(this.ShowMagiclgpToolStripMenuItem_Click);
            // 
            // panelModel
            // 
            this.panelModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelModel.Location = new System.Drawing.Point(159, 25);
            this.panelModel.Name = "panelModel";
            this.panelModel.Size = new System.Drawing.Size(415, 553);
            this.panelModel.TabIndex = 9;
            this.panelModel.TabStop = false;
            this.panelModel.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelModel_Paint);
            this.panelModel.DoubleClick += new System.EventHandler(this.PanelModel_DoubleClick);
            this.panelModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelModel_MouseDown);
            this.panelModel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelModel_MouseMove);
            this.panelModel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelModel_MouseUp);
            // 
            // FrmSkeletonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(734, 649);
            this.Controls.Add(this.panelModel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(750, 673);
            this.Name = "FrmSkeletonEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KimeraCS";
            this.Activated += new System.EventHandler(this.FrmSkeletonEditor_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSkeletonEditor_FormClosed);
            this.Load += new System.EventHandler(this.FrmSkeletonEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSkeletonEditor_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmSkeletonEditor_KeyUp);
            this.Move += new System.EventHandler(this.FrmSkeletonEditor_Move);
            this.Resize += new System.EventHandler(this.FrmSkeletonEditor_Resize);
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
        private System.Windows.Forms.CheckBox chkColorKeyFlag;
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
        private System.Windows.Forms.ToolStripMenuItem outputFramesDataTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputFramesDataTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBattlelgpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMagiclgpToolStripMenuItem;
        public System.Windows.Forms.GroupBox gbTexturesFrame;
        private System.Windows.Forms.ToolStripMenuItem textureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TEXToPNGBatchConversionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadRSDResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputFramesDataTXTToolSelectiveStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skeletonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addJointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editJointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTMDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeFramesDataTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem showNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalsColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalsScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneftoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fiveftoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thousandftoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thirtyftoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showVertexNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFaceNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem Import3DSFixingPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem ShowAxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem DontCheckDuplicatedPolysVertsToolStripMenuItem;
    }
}

