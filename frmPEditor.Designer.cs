
namespace KimeraCS
{
    partial class FrmPEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelAsNewGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveModelAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.unselectGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ResetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mergeGroupsIntoOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mirrorModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeModelSimmetricToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eraseLowerEmisphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.fattenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slimToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.invertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllPolysSelectedColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllPolysnotSelectedColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killPrecalculatedLightningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkVertexColor = new System.Windows.Forms.CheckBox();
            this.gbColorEditor = new System.Windows.Forms.GroupBox();
            this.pbPaletteColor = new System.Windows.Forms.PictureBox();
            this.btnResetBrightness = new System.Windows.Forms.Button();
            this.btnLessBrightness = new System.Windows.Forms.Button();
            this.btnMoreBrightness = new System.Windows.Forms.Button();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.txtSelectedColorB = new System.Windows.Forms.TextBox();
            this.hsbSelectedColorB = new System.Windows.Forms.HScrollBar();
            this.txtSelectedColorG = new System.Windows.Forms.TextBox();
            this.hsbSelectedColorG = new System.Windows.Forms.HScrollBar();
            this.txtSelectedColorR = new System.Windows.Forms.TextBox();
            this.hsbSelectedColorR = new System.Windows.Forms.HScrollBar();
            this.txtThresholdSlider = new System.Windows.Forms.TextBox();
            this.hsbThresholdSlider = new System.Windows.Forms.HScrollBar();
            this.lblDetectionThreshold = new System.Windows.Forms.Label();
            this.chkPaletteMode = new System.Windows.Forms.CheckBox();
            this.lblBlueLevel = new System.Windows.Forms.Label();
            this.lblGreenLevel = new System.Windows.Forms.Label();
            this.lblRedLevel = new System.Windows.Forms.Label();
            this.lblPalette = new System.Windows.Forms.Label();
            this.pbPalette = new System.Windows.Forms.PictureBox();
            this.btnCommitChanges = new System.Windows.Forms.Button();
            this.chkShowAxes = new System.Windows.Forms.CheckBox();
            this.gbDrawMode = new System.Windows.Forms.GroupBox();
            this.rbVertexColors = new System.Windows.Forms.RadioButton();
            this.rbPolygonColors = new System.Windows.Forms.RadioButton();
            this.rbMesh = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbPlaneOperations = new System.Windows.Forms.GroupBox();
            this.chkShowPlane = new System.Windows.Forms.CheckBox();
            this.nudZPlane = new System.Windows.Forms.NumericUpDown();
            this.nudAlphaPlane = new System.Windows.Forms.NumericUpDown();
            this.nudYPlane = new System.Windows.Forms.NumericUpDown();
            this.nudXPlane = new System.Windows.Forms.NumericUpDown();
            this.nudBetaPlane = new System.Windows.Forms.NumericUpDown();
            this.lblAlphaPO = new System.Windows.Forms.Label();
            this.lblZPO = new System.Windows.Forms.Label();
            this.lblBetaPO = new System.Windows.Forms.Label();
            this.lblYPO = new System.Windows.Forms.Label();
            this.lblXPO = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbRotation = new System.Windows.Forms.GroupBox();
            this.txtRotateBeta = new System.Windows.Forms.TextBox();
            this.hsbRotateAlpha = new System.Windows.Forms.HScrollBar();
            this.txtRotateGamma = new System.Windows.Forms.TextBox();
            this.hsbRotateGamma = new System.Windows.Forms.HScrollBar();
            this.lblRotateGamma = new System.Windows.Forms.Label();
            this.hsbRotateBeta = new System.Windows.Forms.HScrollBar();
            this.lblRotateBeta = new System.Windows.Forms.Label();
            this.lblRotateAlpha = new System.Windows.Forms.Label();
            this.txtRotateAlpha = new System.Windows.Forms.TextBox();
            this.gbReposition = new System.Windows.Forms.GroupBox();
            this.txtRepositionZ = new System.Windows.Forms.TextBox();
            this.hsbRepositionZ = new System.Windows.Forms.HScrollBar();
            this.lblRepositionZ = new System.Windows.Forms.Label();
            this.txtRepositionY = new System.Windows.Forms.TextBox();
            this.hsbRepositionY = new System.Windows.Forms.HScrollBar();
            this.lblRepositionY = new System.Windows.Forms.Label();
            this.lblRepositionX = new System.Windows.Forms.Label();
            this.txtRepositionX = new System.Windows.Forms.TextBox();
            this.hsbRepositionX = new System.Windows.Forms.HScrollBar();
            this.gbResize = new System.Windows.Forms.GroupBox();
            this.txtResizeZ = new System.Windows.Forms.TextBox();
            this.hsbResizeZ = new System.Windows.Forms.HScrollBar();
            this.lblResizePieceZ = new System.Windows.Forms.Label();
            this.txtResizeY = new System.Windows.Forms.TextBox();
            this.hsbResizeY = new System.Windows.Forms.HScrollBar();
            this.lblResizePieceY = new System.Windows.Forms.Label();
            this.lblResizePieceX = new System.Windows.Forms.Label();
            this.txtResizeX = new System.Windows.Forms.TextBox();
            this.hsbResizeX = new System.Windows.Forms.HScrollBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gbDrawingOptions = new System.Windows.Forms.GroupBox();
            this.rbNewPolygon = new System.Windows.Forms.RadioButton();
            this.rbPanning = new System.Windows.Forms.RadioButton();
            this.rbZoomInOut = new System.Windows.Forms.RadioButton();
            this.rbFreeRotate = new System.Windows.Forms.RadioButton();
            this.rbMoveVertex = new System.Windows.Forms.RadioButton();
            this.rbErasePolygon = new System.Windows.Forms.RadioButton();
            this.rbCutEdge = new System.Windows.Forms.RadioButton();
            this.rbPaintPolygon = new System.Windows.Forms.RadioButton();
            this.gbGroups = new System.Windows.Forms.GroupBox();
            this.btnDuplicateGroup = new System.Windows.Forms.Button();
            this.btnRemoveGroup = new System.Windows.Forms.Button();
            this.btnGroupProperties = new System.Windows.Forms.Button();
            this.btnHideShowGroup = new System.Windows.Forms.Button();
            this.btnDownGroup = new System.Windows.Forms.Button();
            this.btnUpGroup = new System.Windows.Forms.Button();
            this.lbGroups = new System.Windows.Forms.ListBox();
            this.gbLight = new System.Windows.Forms.GroupBox();
            this.lblZ = new System.Windows.Forms.Label();
            this.hsbLightZ = new System.Windows.Forms.HScrollBar();
            this.lblY = new System.Windows.Forms.Label();
            this.hsbLightY = new System.Windows.Forms.HScrollBar();
            this.lblX = new System.Windows.Forms.Label();
            this.hsbLightX = new System.Windows.Forms.HScrollBar();
            this.chkEnableLighting = new System.Windows.Forms.CheckBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.panelEditorPModel = new System.Windows.Forms.PictureBox();
            this.tmrRenderPModel = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbColorEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaletteColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).BeginInit();
            this.gbDrawMode.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbPlaneOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZPlane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlphaPlane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPlane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXPlane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBetaPlane)).BeginInit();
            this.gbRotation.SuspendLayout();
            this.gbReposition.SuspendLayout();
            this.gbResize.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbDrawingOptions.SuspendLayout();
            this.gbGroups.SuspendLayout();
            this.gbLight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEditorPModel)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.planeToolStripMenuItem,
            this.paletteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(720, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadModelToolStripMenuItem,
            this.loadModelAsNewGroupToolStripMenuItem,
            this.saveModelAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadModelToolStripMenuItem
            // 
            this.loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            this.loadModelToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.loadModelToolStripMenuItem.Text = "Load Model";
            this.loadModelToolStripMenuItem.Click += new System.EventHandler(this.LoadModelToolStripMenuItem_Click);
            // 
            // loadModelAsNewGroupToolStripMenuItem
            // 
            this.loadModelAsNewGroupToolStripMenuItem.Name = "loadModelAsNewGroupToolStripMenuItem";
            this.loadModelAsNewGroupToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.loadModelAsNewGroupToolStripMenuItem.Text = "Load Model as new Group";
            this.loadModelAsNewGroupToolStripMenuItem.Click += new System.EventHandler(this.LoadModelAsNewGroupToolStripMenuItem_Click);
            // 
            // saveModelAsToolStripMenuItem
            // 
            this.saveModelAsToolStripMenuItem.Name = "saveModelAsToolStripMenuItem";
            this.saveModelAsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.saveModelAsToolStripMenuItem.Text = "Save Model As...";
            this.saveModelAsToolStripMenuItem.Click += new System.EventHandler(this.SaveModelAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator4,
            this.unselectGroupToolStripMenuItem,
            this.toolStripSeparator6,
            this.ResetCameraToolStripMenuItem,
            this.toolStripSeparator5,
            this.mergeGroupsIntoOneToolStripMenuItem,
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(274, 6);
            // 
            // unselectGroupToolStripMenuItem
            // 
            this.unselectGroupToolStripMenuItem.Name = "unselectGroupToolStripMenuItem";
            this.unselectGroupToolStripMenuItem.ShortcutKeyDisplayString = "Q";
            this.unselectGroupToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.unselectGroupToolStripMenuItem.Text = "Unselect group";
            this.unselectGroupToolStripMenuItem.Click += new System.EventHandler(this.UnselectGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(274, 6);
            // 
            // ResetCameraToolStripMenuItem
            // 
            this.ResetCameraToolStripMenuItem.Name = "ResetCameraToolStripMenuItem";
            this.ResetCameraToolStripMenuItem.ShortcutKeyDisplayString = "CTRL+Home";
            this.ResetCameraToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.ResetCameraToolStripMenuItem.Text = "Reset Camera";
            this.ResetCameraToolStripMenuItem.Click += new System.EventHandler(this.ResetCameraToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(274, 6);
            // 
            // mergeGroupsIntoOneToolStripMenuItem
            // 
            this.mergeGroupsIntoOneToolStripMenuItem.Name = "mergeGroupsIntoOneToolStripMenuItem";
            this.mergeGroupsIntoOneToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.mergeGroupsIntoOneToolStripMenuItem.Text = "Merge Groups into One (All)";
            this.mergeGroupsIntoOneToolStripMenuItem.Click += new System.EventHandler(this.MergeGroupsIntoOneToolStripMenuItem_Click);
            // 
            // mergeGroupsIntoOnenotTexturedToolStripMenuItem
            // 
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem.Name = "mergeGroupsIntoOnenotTexturedToolStripMenuItem";
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem.Text = "Merge Groups into One (Not Textured)";
            this.mergeGroupsIntoOnenotTexturedToolStripMenuItem.Click += new System.EventHandler(this.MergeGroupsIntoOnenotTexturedToolStripMenuItem_Click);
            // 
            // planeToolStripMenuItem
            // 
            this.planeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mirrorModelToolStripMenuItem,
            this.makeModelSimmetricToolStripMenuItem,
            this.eraseLowerEmisphereToolStripMenuItem,
            this.cutModelToolStripMenuItem,
            this.toolStripSeparator2,
            this.fattenToolStripMenuItem,
            this.slimToolStripMenuItem,
            this.toolStripSeparator3,
            this.invertToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.planeToolStripMenuItem.Name = "planeToolStripMenuItem";
            this.planeToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.planeToolStripMenuItem.Text = "&Plane features";
            // 
            // mirrorModelToolStripMenuItem
            // 
            this.mirrorModelToolStripMenuItem.Name = "mirrorModelToolStripMenuItem";
            this.mirrorModelToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.mirrorModelToolStripMenuItem.Text = "Mirror model";
            this.mirrorModelToolStripMenuItem.Click += new System.EventHandler(this.MirrorModelToolStripMenuItem_Click);
            // 
            // makeModelSimmetricToolStripMenuItem
            // 
            this.makeModelSimmetricToolStripMenuItem.Name = "makeModelSimmetricToolStripMenuItem";
            this.makeModelSimmetricToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.makeModelSimmetricToolStripMenuItem.Text = "Make model symmetric";
            this.makeModelSimmetricToolStripMenuItem.Click += new System.EventHandler(this.MakeModelSymmetricToolStripMenuItem_Click);
            // 
            // eraseLowerEmisphereToolStripMenuItem
            // 
            this.eraseLowerEmisphereToolStripMenuItem.Name = "eraseLowerEmisphereToolStripMenuItem";
            this.eraseLowerEmisphereToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.eraseLowerEmisphereToolStripMenuItem.Text = "Erase lower hemisphere";
            this.eraseLowerEmisphereToolStripMenuItem.Click += new System.EventHandler(this.EraseLowerEmisphereToolStripMenuItem_Click);
            // 
            // cutModelToolStripMenuItem
            // 
            this.cutModelToolStripMenuItem.Name = "cutModelToolStripMenuItem";
            this.cutModelToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.cutModelToolStripMenuItem.Text = "Cut model";
            this.cutModelToolStripMenuItem.Click += new System.EventHandler(this.CutModelToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(196, 6);
            // 
            // fattenToolStripMenuItem
            // 
            this.fattenToolStripMenuItem.Name = "fattenToolStripMenuItem";
            this.fattenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fattenToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.fattenToolStripMenuItem.Text = "Fatten";
            this.fattenToolStripMenuItem.Click += new System.EventHandler(this.FattenToolStripMenuItem_Click);
            // 
            // slimToolStripMenuItem
            // 
            this.slimToolStripMenuItem.Name = "slimToolStripMenuItem";
            this.slimToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.slimToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.slimToolStripMenuItem.Text = "Slim";
            this.slimToolStripMenuItem.Click += new System.EventHandler(this.SlimToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(196, 6);
            // 
            // invertToolStripMenuItem
            // 
            this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
            this.invertToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.invertToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.invertToolStripMenuItem.Text = "Invert";
            this.invertToolStripMenuItem.Click += new System.EventHandler(this.InvertToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.resetToolStripMenuItem.Text = "Reset Plane";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetToolStripMenuItem_Click);
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAllPolysSelectedColorToolStripMenuItem,
            this.deleteAllPolysnotSelectedColorToolStripMenuItem,
            this.killPrecalculatedLightningToolStripMenuItem});
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.paletteToolStripMenuItem.Text = "Pale&tte features";
            // 
            // deleteAllPolysSelectedColorToolStripMenuItem
            // 
            this.deleteAllPolysSelectedColorToolStripMenuItem.Enabled = false;
            this.deleteAllPolysSelectedColorToolStripMenuItem.Name = "deleteAllPolysSelectedColorToolStripMenuItem";
            this.deleteAllPolysSelectedColorToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.deleteAllPolysSelectedColorToolStripMenuItem.Text = "Delete all polys (selected color)";
            this.deleteAllPolysSelectedColorToolStripMenuItem.Click += new System.EventHandler(this.DeleteAllPolysSelectedColorToolStripMenuItem_Click);
            // 
            // deleteAllPolysnotSelectedColorToolStripMenuItem
            // 
            this.deleteAllPolysnotSelectedColorToolStripMenuItem.Enabled = false;
            this.deleteAllPolysnotSelectedColorToolStripMenuItem.Name = "deleteAllPolysnotSelectedColorToolStripMenuItem";
            this.deleteAllPolysnotSelectedColorToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.deleteAllPolysnotSelectedColorToolStripMenuItem.Text = "Delete all polys (not selected color)";
            this.deleteAllPolysnotSelectedColorToolStripMenuItem.Click += new System.EventHandler(this.DeleteAllPolysnotSelectedColorToolStripMenuItem_Click);
            // 
            // killPrecalculatedLightningToolStripMenuItem
            // 
            this.killPrecalculatedLightningToolStripMenuItem.Enabled = false;
            this.killPrecalculatedLightningToolStripMenuItem.Name = "killPrecalculatedLightningToolStripMenuItem";
            this.killPrecalculatedLightningToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.killPrecalculatedLightningToolStripMenuItem.Text = "Kill precalculated lighting";
            this.killPrecalculatedLightningToolStripMenuItem.Visible = false;
            this.killPrecalculatedLightningToolStripMenuItem.Click += new System.EventHandler(this.KillPrecalculatedLightningToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.chkVertexColor);
            this.panel1.Controls.Add(this.gbColorEditor);
            this.panel1.Controls.Add(this.btnCommitChanges);
            this.panel1.Controls.Add(this.chkShowAxes);
            this.panel1.Controls.Add(this.gbDrawMode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 523);
            this.panel1.TabIndex = 1;
            // 
            // chkVertexColor
            // 
            this.chkVertexColor.ForeColor = System.Drawing.SystemColors.Control;
            this.chkVertexColor.Location = new System.Drawing.Point(88, 119);
            this.chkVertexColor.Margin = new System.Windows.Forms.Padding(2);
            this.chkVertexColor.Name = "chkVertexColor";
            this.chkVertexColor.Size = new System.Drawing.Size(60, 32);
            this.chkVertexColor.TabIndex = 29;
            this.chkVertexColor.Text = "Vertex color";
            this.chkVertexColor.UseVisualStyleBackColor = true;
            // 
            // gbColorEditor
            // 
            this.gbColorEditor.Controls.Add(this.pbPaletteColor);
            this.gbColorEditor.Controls.Add(this.btnResetBrightness);
            this.gbColorEditor.Controls.Add(this.btnLessBrightness);
            this.gbColorEditor.Controls.Add(this.btnMoreBrightness);
            this.gbColorEditor.Controls.Add(this.lblBrightness);
            this.gbColorEditor.Controls.Add(this.txtSelectedColorB);
            this.gbColorEditor.Controls.Add(this.hsbSelectedColorB);
            this.gbColorEditor.Controls.Add(this.txtSelectedColorG);
            this.gbColorEditor.Controls.Add(this.hsbSelectedColorG);
            this.gbColorEditor.Controls.Add(this.txtSelectedColorR);
            this.gbColorEditor.Controls.Add(this.hsbSelectedColorR);
            this.gbColorEditor.Controls.Add(this.txtThresholdSlider);
            this.gbColorEditor.Controls.Add(this.hsbThresholdSlider);
            this.gbColorEditor.Controls.Add(this.lblDetectionThreshold);
            this.gbColorEditor.Controls.Add(this.chkPaletteMode);
            this.gbColorEditor.Controls.Add(this.lblBlueLevel);
            this.gbColorEditor.Controls.Add(this.lblGreenLevel);
            this.gbColorEditor.Controls.Add(this.lblRedLevel);
            this.gbColorEditor.Controls.Add(this.lblPalette);
            this.gbColorEditor.Controls.Add(this.pbPalette);
            this.gbColorEditor.ForeColor = System.Drawing.SystemColors.Control;
            this.gbColorEditor.Location = new System.Drawing.Point(7, 146);
            this.gbColorEditor.Name = "gbColorEditor";
            this.gbColorEditor.Size = new System.Drawing.Size(146, 371);
            this.gbColorEditor.TabIndex = 28;
            this.gbColorEditor.TabStop = false;
            this.gbColorEditor.Text = "Color Editor";
            // 
            // pbPaletteColor
            // 
            this.pbPaletteColor.BackColor = System.Drawing.Color.Black;
            this.pbPaletteColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPaletteColor.Location = new System.Drawing.Point(96, 183);
            this.pbPaletteColor.Margin = new System.Windows.Forms.Padding(2);
            this.pbPaletteColor.Name = "pbPaletteColor";
            this.pbPaletteColor.Size = new System.Drawing.Size(42, 20);
            this.pbPaletteColor.TabIndex = 30;
            this.pbPaletteColor.TabStop = false;
            this.pbPaletteColor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbPaletteColor_MouseDown);
            // 
            // btnResetBrightness
            // 
            this.btnResetBrightness.BackgroundImage = global::KimeraCS.Properties.Resources.refresh;
            this.btnResetBrightness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnResetBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnResetBrightness.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnResetBrightness.Location = new System.Drawing.Point(116, 342);
            this.btnResetBrightness.Name = "btnResetBrightness";
            this.btnResetBrightness.Size = new System.Drawing.Size(24, 24);
            this.btnResetBrightness.TabIndex = 28;
            this.btnResetBrightness.UseVisualStyleBackColor = true;
            this.btnResetBrightness.Click += new System.EventHandler(this.BtnResetBrightness_Click);
            // 
            // btnLessBrightness
            // 
            this.btnLessBrightness.BackgroundImage = global::KimeraCS.Properties.Resources.minus;
            this.btnLessBrightness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLessBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnLessBrightness.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLessBrightness.Location = new System.Drawing.Point(92, 342);
            this.btnLessBrightness.Name = "btnLessBrightness";
            this.btnLessBrightness.Size = new System.Drawing.Size(24, 24);
            this.btnLessBrightness.TabIndex = 27;
            this.btnLessBrightness.UseVisualStyleBackColor = true;
            this.btnLessBrightness.Click += new System.EventHandler(this.BtnLessBrightness_Click);
            // 
            // btnMoreBrightness
            // 
            this.btnMoreBrightness.BackgroundImage = global::KimeraCS.Properties.Resources.plus;
            this.btnMoreBrightness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMoreBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnMoreBrightness.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMoreBrightness.Location = new System.Drawing.Point(68, 342);
            this.btnMoreBrightness.Name = "btnMoreBrightness";
            this.btnMoreBrightness.Size = new System.Drawing.Size(24, 24);
            this.btnMoreBrightness.TabIndex = 26;
            this.btnMoreBrightness.UseVisualStyleBackColor = true;
            this.btnMoreBrightness.Click += new System.EventHandler(this.BtnMoreBrightness_Click);
            // 
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Location = new System.Drawing.Point(7, 347);
            this.lblBrightness.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(59, 13);
            this.lblBrightness.TabIndex = 25;
            this.lblBrightness.Text = "Brightness:";
            // 
            // txtSelectedColorB
            // 
            this.txtSelectedColorB.Location = new System.Drawing.Point(106, 283);
            this.txtSelectedColorB.Name = "txtSelectedColorB";
            this.txtSelectedColorB.Size = new System.Drawing.Size(33, 20);
            this.txtSelectedColorB.TabIndex = 24;
            this.txtSelectedColorB.Text = "0";
            this.txtSelectedColorB.TextChanged += new System.EventHandler(this.TxtSelectedColorB_TextChanged);
            // 
            // hsbSelectedColorB
            // 
            this.hsbSelectedColorB.LargeChange = 1;
            this.hsbSelectedColorB.Location = new System.Drawing.Point(9, 283);
            this.hsbSelectedColorB.Maximum = 255;
            this.hsbSelectedColorB.Name = "hsbSelectedColorB";
            this.hsbSelectedColorB.Size = new System.Drawing.Size(94, 19);
            this.hsbSelectedColorB.TabIndex = 23;
            this.hsbSelectedColorB.ValueChanged += new System.EventHandler(this.HsbSelectedColorB_ValueChanged);
            // 
            // txtSelectedColorG
            // 
            this.txtSelectedColorG.Location = new System.Drawing.Point(106, 250);
            this.txtSelectedColorG.Name = "txtSelectedColorG";
            this.txtSelectedColorG.Size = new System.Drawing.Size(33, 20);
            this.txtSelectedColorG.TabIndex = 22;
            this.txtSelectedColorG.Text = "0";
            this.txtSelectedColorG.TextChanged += new System.EventHandler(this.TxtSelectedColorG_TextChanged);
            // 
            // hsbSelectedColorG
            // 
            this.hsbSelectedColorG.LargeChange = 1;
            this.hsbSelectedColorG.Location = new System.Drawing.Point(9, 250);
            this.hsbSelectedColorG.Maximum = 255;
            this.hsbSelectedColorG.Name = "hsbSelectedColorG";
            this.hsbSelectedColorG.Size = new System.Drawing.Size(94, 19);
            this.hsbSelectedColorG.TabIndex = 21;
            this.hsbSelectedColorG.ValueChanged += new System.EventHandler(this.HsbSelectedColorG_ValueChanged);
            // 
            // txtSelectedColorR
            // 
            this.txtSelectedColorR.Location = new System.Drawing.Point(106, 218);
            this.txtSelectedColorR.Name = "txtSelectedColorR";
            this.txtSelectedColorR.Size = new System.Drawing.Size(33, 20);
            this.txtSelectedColorR.TabIndex = 20;
            this.txtSelectedColorR.Text = "0";
            this.txtSelectedColorR.TextChanged += new System.EventHandler(this.TxtSelectedColorR_TextChanged);
            // 
            // hsbSelectedColorR
            // 
            this.hsbSelectedColorR.LargeChange = 1;
            this.hsbSelectedColorR.Location = new System.Drawing.Point(9, 218);
            this.hsbSelectedColorR.Maximum = 255;
            this.hsbSelectedColorR.Name = "hsbSelectedColorR";
            this.hsbSelectedColorR.Size = new System.Drawing.Size(94, 19);
            this.hsbSelectedColorR.TabIndex = 19;
            this.hsbSelectedColorR.ValueChanged += new System.EventHandler(this.HsbSelectedColorR_ValueChanged);
            // 
            // txtThresholdSlider
            // 
            this.txtThresholdSlider.Enabled = false;
            this.txtThresholdSlider.Location = new System.Drawing.Point(107, 320);
            this.txtThresholdSlider.Name = "txtThresholdSlider";
            this.txtThresholdSlider.Size = new System.Drawing.Size(33, 20);
            this.txtThresholdSlider.TabIndex = 18;
            this.txtThresholdSlider.Text = "0";
            this.txtThresholdSlider.TextChanged += new System.EventHandler(this.TxtThresholdSlider_TextChanged);
            // 
            // hsbThresholdSlider
            // 
            this.hsbThresholdSlider.Enabled = false;
            this.hsbThresholdSlider.LargeChange = 1;
            this.hsbThresholdSlider.Location = new System.Drawing.Point(9, 320);
            this.hsbThresholdSlider.Maximum = 255;
            this.hsbThresholdSlider.Name = "hsbThresholdSlider";
            this.hsbThresholdSlider.Size = new System.Drawing.Size(94, 19);
            this.hsbThresholdSlider.TabIndex = 17;
            this.hsbThresholdSlider.ValueChanged += new System.EventHandler(this.HsbThresholdSlider_ValueChanged);
            // 
            // lblDetectionThreshold
            // 
            this.lblDetectionThreshold.AutoSize = true;
            this.lblDetectionThreshold.Location = new System.Drawing.Point(7, 306);
            this.lblDetectionThreshold.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDetectionThreshold.Name = "lblDetectionThreshold";
            this.lblDetectionThreshold.Size = new System.Drawing.Size(102, 13);
            this.lblDetectionThreshold.TabIndex = 9;
            this.lblDetectionThreshold.Text = "Detection threshold:";
            // 
            // chkPaletteMode
            // 
            this.chkPaletteMode.AutoSize = true;
            this.chkPaletteMode.Location = new System.Drawing.Point(7, 186);
            this.chkPaletteMode.Name = "chkPaletteMode";
            this.chkPaletteMode.Size = new System.Drawing.Size(87, 17);
            this.chkPaletteMode.TabIndex = 8;
            this.chkPaletteMode.Text = "Pallete mode";
            this.chkPaletteMode.UseVisualStyleBackColor = true;
            this.chkPaletteMode.CheckedChanged += new System.EventHandler(this.ChkPalettized_CheckedChanged);
            // 
            // lblBlueLevel
            // 
            this.lblBlueLevel.AutoSize = true;
            this.lblBlueLevel.Location = new System.Drawing.Point(7, 270);
            this.lblBlueLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBlueLevel.Name = "lblBlueLevel";
            this.lblBlueLevel.Size = new System.Drawing.Size(56, 13);
            this.lblBlueLevel.TabIndex = 5;
            this.lblBlueLevel.Text = "Blue level:";
            // 
            // lblGreenLevel
            // 
            this.lblGreenLevel.AutoSize = true;
            this.lblGreenLevel.Location = new System.Drawing.Point(7, 237);
            this.lblGreenLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGreenLevel.Name = "lblGreenLevel";
            this.lblGreenLevel.Size = new System.Drawing.Size(64, 13);
            this.lblGreenLevel.TabIndex = 4;
            this.lblGreenLevel.Text = "Green level:";
            // 
            // lblRedLevel
            // 
            this.lblRedLevel.AutoSize = true;
            this.lblRedLevel.Location = new System.Drawing.Point(7, 204);
            this.lblRedLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRedLevel.Name = "lblRedLevel";
            this.lblRedLevel.Size = new System.Drawing.Size(55, 13);
            this.lblRedLevel.TabIndex = 2;
            this.lblRedLevel.Text = "Red level:";
            // 
            // lblPalette
            // 
            this.lblPalette.AutoSize = true;
            this.lblPalette.Location = new System.Drawing.Point(4, 16);
            this.lblPalette.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPalette.Name = "lblPalette";
            this.lblPalette.Size = new System.Drawing.Size(43, 13);
            this.lblPalette.TabIndex = 1;
            this.lblPalette.Text = "Palette:";
            // 
            // pbPalette
            // 
            this.pbPalette.BackColor = System.Drawing.Color.White;
            this.pbPalette.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbPalette.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPalette.Location = new System.Drawing.Point(6, 31);
            this.pbPalette.Margin = new System.Windows.Forms.Padding(2);
            this.pbPalette.Name = "pbPalette";
            this.pbPalette.Size = new System.Drawing.Size(132, 148);
            this.pbPalette.TabIndex = 0;
            this.pbPalette.TabStop = false;
            this.pbPalette.DragDrop += new System.Windows.Forms.DragEventHandler(this.PbPalette_DragDrop);
            this.pbPalette.DragEnter += new System.Windows.Forms.DragEventHandler(this.PbPalette_DragEnter);
            this.pbPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbPalette_MouseDown);
            this.pbPalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbPalette_MouseMove);
            // 
            // btnCommitChanges
            // 
            this.btnCommitChanges.Location = new System.Drawing.Point(7, 6);
            this.btnCommitChanges.Name = "btnCommitChanges";
            this.btnCommitChanges.Size = new System.Drawing.Size(146, 30);
            this.btnCommitChanges.TabIndex = 1;
            this.btnCommitChanges.Text = "Commit changes";
            this.btnCommitChanges.UseVisualStyleBackColor = true;
            this.btnCommitChanges.Click += new System.EventHandler(this.BtnCommitChanges_Click);
            // 
            // chkShowAxes
            // 
            this.chkShowAxes.Checked = true;
            this.chkShowAxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAxes.ForeColor = System.Drawing.SystemColors.Control;
            this.chkShowAxes.Location = new System.Drawing.Point(8, 123);
            this.chkShowAxes.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowAxes.Name = "chkShowAxes";
            this.chkShowAxes.Size = new System.Drawing.Size(93, 24);
            this.chkShowAxes.TabIndex = 27;
            this.chkShowAxes.Text = "Show axes";
            this.chkShowAxes.UseVisualStyleBackColor = true;
            this.chkShowAxes.Click += new System.EventHandler(this.ChkShowAxes_Click);
            // 
            // gbDrawMode
            // 
            this.gbDrawMode.Controls.Add(this.rbVertexColors);
            this.gbDrawMode.Controls.Add(this.rbPolygonColors);
            this.gbDrawMode.Controls.Add(this.rbMesh);
            this.gbDrawMode.ForeColor = System.Drawing.SystemColors.Control;
            this.gbDrawMode.Location = new System.Drawing.Point(7, 37);
            this.gbDrawMode.Name = "gbDrawMode";
            this.gbDrawMode.Size = new System.Drawing.Size(146, 82);
            this.gbDrawMode.TabIndex = 0;
            this.gbDrawMode.TabStop = false;
            this.gbDrawMode.Text = "Draw Mode";
            // 
            // rbVertexColors
            // 
            this.rbVertexColors.AutoCheck = false;
            this.rbVertexColors.AutoSize = true;
            this.rbVertexColors.Location = new System.Drawing.Point(9, 57);
            this.rbVertexColors.Name = "rbVertexColors";
            this.rbVertexColors.Size = new System.Drawing.Size(86, 17);
            this.rbVertexColors.TabIndex = 2;
            this.rbVertexColors.Text = "Vertex colors";
            this.rbVertexColors.UseVisualStyleBackColor = true;
            this.rbVertexColors.Click += new System.EventHandler(this.RbVertexColors_Click);
            // 
            // rbPolygonColors
            // 
            this.rbPolygonColors.AutoCheck = false;
            this.rbPolygonColors.AutoSize = true;
            this.rbPolygonColors.Location = new System.Drawing.Point(9, 38);
            this.rbPolygonColors.Name = "rbPolygonColors";
            this.rbPolygonColors.Size = new System.Drawing.Size(94, 17);
            this.rbPolygonColors.TabIndex = 1;
            this.rbPolygonColors.Text = "Polygon colors";
            this.rbPolygonColors.UseVisualStyleBackColor = true;
            this.rbPolygonColors.Click += new System.EventHandler(this.RbPolygonColors_Click);
            // 
            // rbMesh
            // 
            this.rbMesh.AutoCheck = false;
            this.rbMesh.AutoSize = true;
            this.rbMesh.Location = new System.Drawing.Point(9, 19);
            this.rbMesh.Name = "rbMesh";
            this.rbMesh.Size = new System.Drawing.Size(51, 17);
            this.rbMesh.TabIndex = 0;
            this.rbMesh.Text = "Mesh";
            this.rbMesh.UseVisualStyleBackColor = true;
            this.rbMesh.Click += new System.EventHandler(this.RbMesh_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel2.Controls.Add(this.gbPlaneOperations);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.gbRotation);
            this.panel2.Controls.Add(this.gbReposition);
            this.panel2.Controls.Add(this.gbResize);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(576, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(144, 523);
            this.panel2.TabIndex = 2;
            // 
            // gbPlaneOperations
            // 
            this.gbPlaneOperations.Controls.Add(this.chkShowPlane);
            this.gbPlaneOperations.Controls.Add(this.nudZPlane);
            this.gbPlaneOperations.Controls.Add(this.nudAlphaPlane);
            this.gbPlaneOperations.Controls.Add(this.nudYPlane);
            this.gbPlaneOperations.Controls.Add(this.nudXPlane);
            this.gbPlaneOperations.Controls.Add(this.nudBetaPlane);
            this.gbPlaneOperations.Controls.Add(this.lblAlphaPO);
            this.gbPlaneOperations.Controls.Add(this.lblZPO);
            this.gbPlaneOperations.Controls.Add(this.lblBetaPO);
            this.gbPlaneOperations.Controls.Add(this.lblYPO);
            this.gbPlaneOperations.Controls.Add(this.lblXPO);
            this.gbPlaneOperations.ForeColor = System.Drawing.SystemColors.Control;
            this.gbPlaneOperations.Location = new System.Drawing.Point(6, 363);
            this.gbPlaneOperations.Margin = new System.Windows.Forms.Padding(2);
            this.gbPlaneOperations.Name = "gbPlaneOperations";
            this.gbPlaneOperations.Padding = new System.Windows.Forms.Padding(2);
            this.gbPlaneOperations.Size = new System.Drawing.Size(132, 154);
            this.gbPlaneOperations.TabIndex = 2;
            this.gbPlaneOperations.TabStop = false;
            this.gbPlaneOperations.Text = "Plane operations";
            // 
            // chkShowPlane
            // 
            this.chkShowPlane.Checked = true;
            this.chkShowPlane.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPlane.Location = new System.Drawing.Point(28, 125);
            this.chkShowPlane.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowPlane.Name = "chkShowPlane";
            this.chkShowPlane.Size = new System.Drawing.Size(88, 24);
            this.chkShowPlane.TabIndex = 32;
            this.chkShowPlane.Text = "Show plane";
            this.chkShowPlane.UseVisualStyleBackColor = true;
            this.chkShowPlane.Click += new System.EventHandler(this.ChkShowPlane_Click);
            // 
            // nudZPlane
            // 
            this.nudZPlane.Location = new System.Drawing.Point(45, 62);
            this.nudZPlane.Margin = new System.Windows.Forms.Padding(2);
            this.nudZPlane.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudZPlane.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudZPlane.Name = "nudZPlane";
            this.nudZPlane.Size = new System.Drawing.Size(77, 20);
            this.nudZPlane.TabIndex = 29;
            this.nudZPlane.TextChanged += new System.EventHandler(this.NudZPlane_TextChanged);
            this.nudZPlane.ValueChanged += new System.EventHandler(this.NudZPlane_ValueChanged);
            // 
            // nudAlphaPlane
            // 
            this.nudAlphaPlane.Location = new System.Drawing.Point(45, 83);
            this.nudAlphaPlane.Margin = new System.Windows.Forms.Padding(2);
            this.nudAlphaPlane.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAlphaPlane.Name = "nudAlphaPlane";
            this.nudAlphaPlane.Size = new System.Drawing.Size(77, 20);
            this.nudAlphaPlane.TabIndex = 30;
            this.nudAlphaPlane.TextChanged += new System.EventHandler(this.NudAlphaPlane_TextChanged);
            this.nudAlphaPlane.ValueChanged += new System.EventHandler(this.NudAlphaPlane_ValueChanged);
            // 
            // nudYPlane
            // 
            this.nudYPlane.Location = new System.Drawing.Point(45, 41);
            this.nudYPlane.Margin = new System.Windows.Forms.Padding(2);
            this.nudYPlane.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudYPlane.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudYPlane.Name = "nudYPlane";
            this.nudYPlane.Size = new System.Drawing.Size(77, 20);
            this.nudYPlane.TabIndex = 28;
            this.nudYPlane.TextChanged += new System.EventHandler(this.NudYPlane_TextChanged);
            this.nudYPlane.ValueChanged += new System.EventHandler(this.NudYPlane_ValueChanged);
            // 
            // nudXPlane
            // 
            this.nudXPlane.Location = new System.Drawing.Point(45, 20);
            this.nudXPlane.Margin = new System.Windows.Forms.Padding(2);
            this.nudXPlane.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudXPlane.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudXPlane.Name = "nudXPlane";
            this.nudXPlane.Size = new System.Drawing.Size(77, 20);
            this.nudXPlane.TabIndex = 27;
            this.nudXPlane.TextChanged += new System.EventHandler(this.NudXPlane_TextChanged);
            this.nudXPlane.ValueChanged += new System.EventHandler(this.NudXPlane_ValueChanged);
            // 
            // nudBetaPlane
            // 
            this.nudBetaPlane.Location = new System.Drawing.Point(45, 104);
            this.nudBetaPlane.Margin = new System.Windows.Forms.Padding(2);
            this.nudBetaPlane.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudBetaPlane.Name = "nudBetaPlane";
            this.nudBetaPlane.Size = new System.Drawing.Size(77, 20);
            this.nudBetaPlane.TabIndex = 31;
            this.nudBetaPlane.TextChanged += new System.EventHandler(this.NudBetaPlane_TextChanged);
            this.nudBetaPlane.ValueChanged += new System.EventHandler(this.NudBetaPlane_ValueChanged);
            // 
            // lblAlphaPO
            // 
            this.lblAlphaPO.AutoSize = true;
            this.lblAlphaPO.Location = new System.Drawing.Point(7, 85);
            this.lblAlphaPO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAlphaPO.Name = "lblAlphaPO";
            this.lblAlphaPO.Size = new System.Drawing.Size(34, 13);
            this.lblAlphaPO.TabIndex = 22;
            this.lblAlphaPO.Text = "Alpha";
            this.lblAlphaPO.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblZPO
            // 
            this.lblZPO.AutoSize = true;
            this.lblZPO.Location = new System.Drawing.Point(26, 64);
            this.lblZPO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZPO.Name = "lblZPO";
            this.lblZPO.Size = new System.Drawing.Size(14, 13);
            this.lblZPO.TabIndex = 18;
            this.lblZPO.Text = "Z";
            // 
            // lblBetaPO
            // 
            this.lblBetaPO.AutoSize = true;
            this.lblBetaPO.Location = new System.Drawing.Point(12, 106);
            this.lblBetaPO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBetaPO.Name = "lblBetaPO";
            this.lblBetaPO.Size = new System.Drawing.Size(29, 13);
            this.lblBetaPO.TabIndex = 23;
            this.lblBetaPO.Text = "Beta";
            this.lblBetaPO.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblYPO
            // 
            this.lblYPO.AutoSize = true;
            this.lblYPO.Location = new System.Drawing.Point(26, 43);
            this.lblYPO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYPO.Name = "lblYPO";
            this.lblYPO.Size = new System.Drawing.Size(14, 13);
            this.lblYPO.TabIndex = 17;
            this.lblYPO.Text = "Y";
            // 
            // lblXPO
            // 
            this.lblXPO.AutoSize = true;
            this.lblXPO.Location = new System.Drawing.Point(26, 22);
            this.lblXPO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblXPO.Name = "lblXPO";
            this.lblXPO.Size = new System.Drawing.Size(14, 13);
            this.lblXPO.TabIndex = 16;
            this.lblXPO.Text = "X";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(6, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(132, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gbRotation
            // 
            this.gbRotation.Controls.Add(this.txtRotateBeta);
            this.gbRotation.Controls.Add(this.hsbRotateAlpha);
            this.gbRotation.Controls.Add(this.txtRotateGamma);
            this.gbRotation.Controls.Add(this.hsbRotateGamma);
            this.gbRotation.Controls.Add(this.lblRotateGamma);
            this.gbRotation.Controls.Add(this.hsbRotateBeta);
            this.gbRotation.Controls.Add(this.lblRotateBeta);
            this.gbRotation.Controls.Add(this.lblRotateAlpha);
            this.gbRotation.Controls.Add(this.txtRotateAlpha);
            this.gbRotation.ForeColor = System.Drawing.SystemColors.Control;
            this.gbRotation.Location = new System.Drawing.Point(6, 221);
            this.gbRotation.Name = "gbRotation";
            this.gbRotation.Size = new System.Drawing.Size(132, 138);
            this.gbRotation.TabIndex = 2;
            this.gbRotation.TabStop = false;
            this.gbRotation.Text = "Rotation";
            // 
            // txtRotateBeta
            // 
            this.txtRotateBeta.Location = new System.Drawing.Point(95, 71);
            this.txtRotateBeta.MaxLength = 8;
            this.txtRotateBeta.Name = "txtRotateBeta";
            this.txtRotateBeta.Size = new System.Drawing.Size(32, 20);
            this.txtRotateBeta.TabIndex = 24;
            this.txtRotateBeta.Text = "0";
            this.txtRotateBeta.TextChanged += new System.EventHandler(this.TxtRotateBeta_TextChanged);
            // 
            // hsbRotateAlpha
            // 
            this.hsbRotateAlpha.LargeChange = 1;
            this.hsbRotateAlpha.Location = new System.Drawing.Point(6, 34);
            this.hsbRotateAlpha.Maximum = 360;
            this.hsbRotateAlpha.Name = "hsbRotateAlpha";
            this.hsbRotateAlpha.Size = new System.Drawing.Size(86, 19);
            this.hsbRotateAlpha.TabIndex = 21;
            this.hsbRotateAlpha.ValueChanged += new System.EventHandler(this.HsbRotateAlpha_ValueChanged);
            // 
            // txtRotateGamma
            // 
            this.txtRotateGamma.Location = new System.Drawing.Point(95, 110);
            this.txtRotateGamma.MaxLength = 8;
            this.txtRotateGamma.Name = "txtRotateGamma";
            this.txtRotateGamma.Size = new System.Drawing.Size(32, 20);
            this.txtRotateGamma.TabIndex = 26;
            this.txtRotateGamma.Text = "0";
            this.txtRotateGamma.TextChanged += new System.EventHandler(this.TxtRotateGamma_TextChanged);
            // 
            // hsbRotateGamma
            // 
            this.hsbRotateGamma.LargeChange = 1;
            this.hsbRotateGamma.Location = new System.Drawing.Point(6, 111);
            this.hsbRotateGamma.Maximum = 360;
            this.hsbRotateGamma.Name = "hsbRotateGamma";
            this.hsbRotateGamma.Size = new System.Drawing.Size(86, 19);
            this.hsbRotateGamma.TabIndex = 25;
            this.hsbRotateGamma.ValueChanged += new System.EventHandler(this.HsbRotateGamma_ValueChanged);
            // 
            // lblRotateGamma
            // 
            this.lblRotateGamma.AutoSize = true;
            this.lblRotateGamma.Location = new System.Drawing.Point(3, 96);
            this.lblRotateGamma.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateGamma.Name = "lblRotateGamma";
            this.lblRotateGamma.Size = new System.Drawing.Size(119, 13);
            this.lblRotateGamma.TabIndex = 13;
            this.lblRotateGamma.Text = "Gamma rotation (Z-Axis)";
            // 
            // hsbRotateBeta
            // 
            this.hsbRotateBeta.LargeChange = 1;
            this.hsbRotateBeta.Location = new System.Drawing.Point(6, 72);
            this.hsbRotateBeta.Maximum = 360;
            this.hsbRotateBeta.Name = "hsbRotateBeta";
            this.hsbRotateBeta.Size = new System.Drawing.Size(86, 19);
            this.hsbRotateBeta.TabIndex = 23;
            this.hsbRotateBeta.ValueChanged += new System.EventHandler(this.HsbRotateBeta_ValueChanged);
            // 
            // lblRotateBeta
            // 
            this.lblRotateBeta.AutoSize = true;
            this.lblRotateBeta.Location = new System.Drawing.Point(3, 57);
            this.lblRotateBeta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateBeta.Name = "lblRotateBeta";
            this.lblRotateBeta.Size = new System.Drawing.Size(105, 13);
            this.lblRotateBeta.TabIndex = 11;
            this.lblRotateBeta.Text = "Beta rotation (Y-Axis)";
            // 
            // lblRotateAlpha
            // 
            this.lblRotateAlpha.AutoSize = true;
            this.lblRotateAlpha.Location = new System.Drawing.Point(3, 18);
            this.lblRotateAlpha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRotateAlpha.Name = "lblRotateAlpha";
            this.lblRotateAlpha.Size = new System.Drawing.Size(110, 13);
            this.lblRotateAlpha.TabIndex = 10;
            this.lblRotateAlpha.Text = "Alpha rotation (X-Axis)";
            // 
            // txtRotateAlpha
            // 
            this.txtRotateAlpha.Location = new System.Drawing.Point(95, 33);
            this.txtRotateAlpha.MaxLength = 8;
            this.txtRotateAlpha.Name = "txtRotateAlpha";
            this.txtRotateAlpha.Size = new System.Drawing.Size(32, 20);
            this.txtRotateAlpha.TabIndex = 22;
            this.txtRotateAlpha.Text = "0";
            this.txtRotateAlpha.TextChanged += new System.EventHandler(this.TxtRotateAlpha_TextChanged);
            // 
            // gbReposition
            // 
            this.gbReposition.Controls.Add(this.txtRepositionZ);
            this.gbReposition.Controls.Add(this.hsbRepositionZ);
            this.gbReposition.Controls.Add(this.lblRepositionZ);
            this.gbReposition.Controls.Add(this.txtRepositionY);
            this.gbReposition.Controls.Add(this.hsbRepositionY);
            this.gbReposition.Controls.Add(this.lblRepositionY);
            this.gbReposition.Controls.Add(this.lblRepositionX);
            this.gbReposition.Controls.Add(this.txtRepositionX);
            this.gbReposition.Controls.Add(this.hsbRepositionX);
            this.gbReposition.ForeColor = System.Drawing.SystemColors.Control;
            this.gbReposition.Location = new System.Drawing.Point(6, 129);
            this.gbReposition.Name = "gbReposition";
            this.gbReposition.Size = new System.Drawing.Size(132, 86);
            this.gbReposition.TabIndex = 1;
            this.gbReposition.TabStop = false;
            this.gbReposition.Text = "Reposition";
            // 
            // txtRepositionZ
            // 
            this.txtRepositionZ.Location = new System.Drawing.Point(94, 59);
            this.txtRepositionZ.MaxLength = 8;
            this.txtRepositionZ.Name = "txtRepositionZ";
            this.txtRepositionZ.Size = new System.Drawing.Size(34, 20);
            this.txtRepositionZ.TabIndex = 20;
            this.txtRepositionZ.Text = "0";
            this.txtRepositionZ.TextChanged += new System.EventHandler(this.TxtRepositionZ_TextChanged);
            // 
            // hsbRepositionZ
            // 
            this.hsbRepositionZ.LargeChange = 1;
            this.hsbRepositionZ.Location = new System.Drawing.Point(19, 59);
            this.hsbRepositionZ.Maximum = 500;
            this.hsbRepositionZ.Minimum = -500;
            this.hsbRepositionZ.Name = "hsbRepositionZ";
            this.hsbRepositionZ.Size = new System.Drawing.Size(73, 19);
            this.hsbRepositionZ.TabIndex = 19;
            this.hsbRepositionZ.ValueChanged += new System.EventHandler(this.HsbRepositionZ_ValueChanged);
            // 
            // lblRepositionZ
            // 
            this.lblRepositionZ.AutoSize = true;
            this.lblRepositionZ.Location = new System.Drawing.Point(3, 61);
            this.lblRepositionZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionZ.Name = "lblRepositionZ";
            this.lblRepositionZ.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionZ.TabIndex = 15;
            this.lblRepositionZ.Text = "Z";
            // 
            // txtRepositionY
            // 
            this.txtRepositionY.Location = new System.Drawing.Point(94, 39);
            this.txtRepositionY.MaxLength = 8;
            this.txtRepositionY.Name = "txtRepositionY";
            this.txtRepositionY.Size = new System.Drawing.Size(34, 20);
            this.txtRepositionY.TabIndex = 18;
            this.txtRepositionY.Text = "0";
            this.txtRepositionY.TextChanged += new System.EventHandler(this.TxtRepositionY_TextChanged);
            // 
            // hsbRepositionY
            // 
            this.hsbRepositionY.LargeChange = 1;
            this.hsbRepositionY.Location = new System.Drawing.Point(19, 39);
            this.hsbRepositionY.Maximum = 500;
            this.hsbRepositionY.Minimum = -500;
            this.hsbRepositionY.Name = "hsbRepositionY";
            this.hsbRepositionY.Size = new System.Drawing.Size(73, 19);
            this.hsbRepositionY.TabIndex = 17;
            this.hsbRepositionY.ValueChanged += new System.EventHandler(this.HsbRepositionY_ValueChanged);
            // 
            // lblRepositionY
            // 
            this.lblRepositionY.AutoSize = true;
            this.lblRepositionY.Location = new System.Drawing.Point(3, 42);
            this.lblRepositionY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionY.Name = "lblRepositionY";
            this.lblRepositionY.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionY.TabIndex = 12;
            this.lblRepositionY.Text = "Y";
            // 
            // lblRepositionX
            // 
            this.lblRepositionX.AutoSize = true;
            this.lblRepositionX.Location = new System.Drawing.Point(3, 22);
            this.lblRepositionX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRepositionX.Name = "lblRepositionX";
            this.lblRepositionX.Size = new System.Drawing.Size(14, 13);
            this.lblRepositionX.TabIndex = 11;
            this.lblRepositionX.Text = "X";
            // 
            // txtRepositionX
            // 
            this.txtRepositionX.Location = new System.Drawing.Point(94, 19);
            this.txtRepositionX.MaxLength = 8;
            this.txtRepositionX.Name = "txtRepositionX";
            this.txtRepositionX.Size = new System.Drawing.Size(34, 20);
            this.txtRepositionX.TabIndex = 16;
            this.txtRepositionX.Text = "0";
            this.txtRepositionX.TextChanged += new System.EventHandler(this.TxtRepositionX_TextChanged);
            // 
            // hsbRepositionX
            // 
            this.hsbRepositionX.LargeChange = 1;
            this.hsbRepositionX.Location = new System.Drawing.Point(19, 19);
            this.hsbRepositionX.Maximum = 500;
            this.hsbRepositionX.Minimum = -500;
            this.hsbRepositionX.Name = "hsbRepositionX";
            this.hsbRepositionX.Size = new System.Drawing.Size(73, 19);
            this.hsbRepositionX.TabIndex = 15;
            this.hsbRepositionX.ValueChanged += new System.EventHandler(this.HsbRepositionX_ValueChanged);
            // 
            // gbResize
            // 
            this.gbResize.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gbResize.Controls.Add(this.txtResizeZ);
            this.gbResize.Controls.Add(this.hsbResizeZ);
            this.gbResize.Controls.Add(this.lblResizePieceZ);
            this.gbResize.Controls.Add(this.txtResizeY);
            this.gbResize.Controls.Add(this.hsbResizeY);
            this.gbResize.Controls.Add(this.lblResizePieceY);
            this.gbResize.Controls.Add(this.lblResizePieceX);
            this.gbResize.Controls.Add(this.txtResizeX);
            this.gbResize.Controls.Add(this.hsbResizeX);
            this.gbResize.ForeColor = System.Drawing.SystemColors.Control;
            this.gbResize.Location = new System.Drawing.Point(6, 37);
            this.gbResize.Name = "gbResize";
            this.gbResize.Size = new System.Drawing.Size(132, 86);
            this.gbResize.TabIndex = 0;
            this.gbResize.TabStop = false;
            this.gbResize.Text = "Resize";
            // 
            // txtResizeZ
            // 
            this.txtResizeZ.Location = new System.Drawing.Point(99, 59);
            this.txtResizeZ.MaxLength = 8;
            this.txtResizeZ.Name = "txtResizeZ";
            this.txtResizeZ.Size = new System.Drawing.Size(28, 20);
            this.txtResizeZ.TabIndex = 14;
            this.txtResizeZ.Text = "100";
            this.txtResizeZ.TextChanged += new System.EventHandler(this.TxtResizeZ_TextChanged);
            // 
            // hsbResizeZ
            // 
            this.hsbResizeZ.LargeChange = 1;
            this.hsbResizeZ.Location = new System.Drawing.Point(19, 59);
            this.hsbResizeZ.Maximum = 500;
            this.hsbResizeZ.Name = "hsbResizeZ";
            this.hsbResizeZ.Size = new System.Drawing.Size(78, 19);
            this.hsbResizeZ.TabIndex = 13;
            this.hsbResizeZ.Value = 100;
            this.hsbResizeZ.ValueChanged += new System.EventHandler(this.HsbResizeZ_ValueChanged);
            // 
            // lblResizePieceZ
            // 
            this.lblResizePieceZ.AutoSize = true;
            this.lblResizePieceZ.Location = new System.Drawing.Point(3, 62);
            this.lblResizePieceZ.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceZ.Name = "lblResizePieceZ";
            this.lblResizePieceZ.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceZ.TabIndex = 15;
            this.lblResizePieceZ.Text = "Z";
            // 
            // txtResizeY
            // 
            this.txtResizeY.Location = new System.Drawing.Point(99, 39);
            this.txtResizeY.MaxLength = 8;
            this.txtResizeY.Name = "txtResizeY";
            this.txtResizeY.Size = new System.Drawing.Size(28, 20);
            this.txtResizeY.TabIndex = 12;
            this.txtResizeY.Text = "100";
            this.txtResizeY.TextChanged += new System.EventHandler(this.TxtResizeY_TextChanged);
            // 
            // hsbResizeY
            // 
            this.hsbResizeY.LargeChange = 1;
            this.hsbResizeY.Location = new System.Drawing.Point(19, 39);
            this.hsbResizeY.Maximum = 500;
            this.hsbResizeY.Name = "hsbResizeY";
            this.hsbResizeY.Size = new System.Drawing.Size(78, 19);
            this.hsbResizeY.TabIndex = 11;
            this.hsbResizeY.Value = 100;
            this.hsbResizeY.ValueChanged += new System.EventHandler(this.HsbResizeY_ValueChanged);
            // 
            // lblResizePieceY
            // 
            this.lblResizePieceY.AutoSize = true;
            this.lblResizePieceY.Location = new System.Drawing.Point(3, 42);
            this.lblResizePieceY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceY.Name = "lblResizePieceY";
            this.lblResizePieceY.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceY.TabIndex = 12;
            this.lblResizePieceY.Text = "Y";
            // 
            // lblResizePieceX
            // 
            this.lblResizePieceX.AutoSize = true;
            this.lblResizePieceX.Location = new System.Drawing.Point(3, 22);
            this.lblResizePieceX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResizePieceX.Name = "lblResizePieceX";
            this.lblResizePieceX.Size = new System.Drawing.Size(14, 13);
            this.lblResizePieceX.TabIndex = 11;
            this.lblResizePieceX.Text = "X";
            // 
            // txtResizeX
            // 
            this.txtResizeX.Location = new System.Drawing.Point(99, 19);
            this.txtResizeX.MaxLength = 8;
            this.txtResizeX.Name = "txtResizeX";
            this.txtResizeX.Size = new System.Drawing.Size(28, 20);
            this.txtResizeX.TabIndex = 10;
            this.txtResizeX.Text = "100";
            this.txtResizeX.TextChanged += new System.EventHandler(this.TxtResizeX_TextChanged);
            // 
            // hsbResizeX
            // 
            this.hsbResizeX.LargeChange = 1;
            this.hsbResizeX.Location = new System.Drawing.Point(19, 19);
            this.hsbResizeX.Maximum = 500;
            this.hsbResizeX.Name = "hsbResizeX";
            this.hsbResizeX.Size = new System.Drawing.Size(78, 19);
            this.hsbResizeX.TabIndex = 9;
            this.hsbResizeX.Value = 100;
            this.hsbResizeX.ValueChanged += new System.EventHandler(this.HsbResizeX_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3.Controls.Add(this.gbDrawingOptions);
            this.panel3.Controls.Add(this.gbGroups);
            this.panel3.Controls.Add(this.gbLight);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(158, 347);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(418, 200);
            this.panel3.TabIndex = 3;
            // 
            // gbDrawingOptions
            // 
            this.gbDrawingOptions.Controls.Add(this.rbNewPolygon);
            this.gbDrawingOptions.Controls.Add(this.rbPanning);
            this.gbDrawingOptions.Controls.Add(this.rbZoomInOut);
            this.gbDrawingOptions.Controls.Add(this.rbFreeRotate);
            this.gbDrawingOptions.Controls.Add(this.rbMoveVertex);
            this.gbDrawingOptions.Controls.Add(this.rbErasePolygon);
            this.gbDrawingOptions.Controls.Add(this.rbCutEdge);
            this.gbDrawingOptions.Controls.Add(this.rbPaintPolygon);
            this.gbDrawingOptions.ForeColor = System.Drawing.SystemColors.Control;
            this.gbDrawingOptions.Location = new System.Drawing.Point(6, 114);
            this.gbDrawingOptions.Name = "gbDrawingOptions";
            this.gbDrawingOptions.Size = new System.Drawing.Size(406, 80);
            this.gbDrawingOptions.TabIndex = 28;
            this.gbDrawingOptions.TabStop = false;
            this.gbDrawingOptions.Text = "Drawing options";
            // 
            // rbNewPolygon
            // 
            this.rbNewPolygon.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbNewPolygon.BackColor = System.Drawing.Color.Transparent;
            this.rbNewPolygon.BackgroundImage = global::KimeraCS.Properties.Resources.draw_new_polygon;
            this.rbNewPolygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbNewPolygon.Font = new System.Drawing.Font("OpenSymbol", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNewPolygon.ForeColor = System.Drawing.Color.Black;
            this.rbNewPolygon.Location = new System.Drawing.Point(347, 20);
            this.rbNewPolygon.Name = "rbNewPolygon";
            this.rbNewPolygon.Size = new System.Drawing.Size(48, 48);
            this.rbNewPolygon.TabIndex = 40;
            this.rbNewPolygon.TabStop = true;
            this.rbNewPolygon.Text = "0/3";
            this.rbNewPolygon.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.rbNewPolygon.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.rbNewPolygon.UseVisualStyleBackColor = false;
            this.rbNewPolygon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbNewPolygon_MouseDown);
            // 
            // rbPanning
            // 
            this.rbPanning.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPanning.BackColor = System.Drawing.Color.MediumAquamarine;
            this.rbPanning.BackgroundImage = global::KimeraCS.Properties.Resources.draw_panning;
            this.rbPanning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbPanning.Location = new System.Drawing.Point(299, 20);
            this.rbPanning.Name = "rbPanning";
            this.rbPanning.Size = new System.Drawing.Size(48, 48);
            this.rbPanning.TabIndex = 39;
            this.rbPanning.TabStop = true;
            this.rbPanning.UseVisualStyleBackColor = false;
            this.rbPanning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbPanning_MouseDown);
            // 
            // rbZoomInOut
            // 
            this.rbZoomInOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbZoomInOut.BackColor = System.Drawing.Color.PowderBlue;
            this.rbZoomInOut.BackgroundImage = global::KimeraCS.Properties.Resources.draw_zoom;
            this.rbZoomInOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbZoomInOut.Location = new System.Drawing.Point(251, 20);
            this.rbZoomInOut.Name = "rbZoomInOut";
            this.rbZoomInOut.Size = new System.Drawing.Size(48, 48);
            this.rbZoomInOut.TabIndex = 38;
            this.rbZoomInOut.TabStop = true;
            this.rbZoomInOut.UseVisualStyleBackColor = false;
            this.rbZoomInOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbZoomInOut_MouseDown);
            // 
            // rbFreeRotate
            // 
            this.rbFreeRotate.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbFreeRotate.BackColor = System.Drawing.Color.LightCoral;
            this.rbFreeRotate.BackgroundImage = global::KimeraCS.Properties.Resources.draw_free_rotate;
            this.rbFreeRotate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbFreeRotate.Location = new System.Drawing.Point(203, 20);
            this.rbFreeRotate.Name = "rbFreeRotate";
            this.rbFreeRotate.Size = new System.Drawing.Size(48, 48);
            this.rbFreeRotate.TabIndex = 37;
            this.rbFreeRotate.TabStop = true;
            this.rbFreeRotate.UseVisualStyleBackColor = false;
            this.rbFreeRotate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbFreeRotate_MouseDown);
            // 
            // rbMoveVertex
            // 
            this.rbMoveVertex.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbMoveVertex.BackgroundImage = global::KimeraCS.Properties.Resources.draw_move_vertex;
            this.rbMoveVertex.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbMoveVertex.Location = new System.Drawing.Point(155, 20);
            this.rbMoveVertex.Name = "rbMoveVertex";
            this.rbMoveVertex.Size = new System.Drawing.Size(48, 48);
            this.rbMoveVertex.TabIndex = 36;
            this.rbMoveVertex.TabStop = true;
            this.rbMoveVertex.UseVisualStyleBackColor = true;
            this.rbMoveVertex.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbMoveVertex_MouseDown);
            // 
            // rbErasePolygon
            // 
            this.rbErasePolygon.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbErasePolygon.BackColor = System.Drawing.Color.Transparent;
            this.rbErasePolygon.BackgroundImage = global::KimeraCS.Properties.Resources.draw_erase_polygon;
            this.rbErasePolygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbErasePolygon.Location = new System.Drawing.Point(107, 20);
            this.rbErasePolygon.Name = "rbErasePolygon";
            this.rbErasePolygon.Size = new System.Drawing.Size(48, 48);
            this.rbErasePolygon.TabIndex = 35;
            this.rbErasePolygon.TabStop = true;
            this.rbErasePolygon.UseVisualStyleBackColor = false;
            this.rbErasePolygon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbErasePolygon_MouseDown);
            // 
            // rbCutEdge
            // 
            this.rbCutEdge.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbCutEdge.BackColor = System.Drawing.Color.Transparent;
            this.rbCutEdge.BackgroundImage = global::KimeraCS.Properties.Resources.draw_cut_edge;
            this.rbCutEdge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbCutEdge.Location = new System.Drawing.Point(59, 20);
            this.rbCutEdge.Name = "rbCutEdge";
            this.rbCutEdge.Size = new System.Drawing.Size(48, 48);
            this.rbCutEdge.TabIndex = 34;
            this.rbCutEdge.TabStop = true;
            this.rbCutEdge.UseVisualStyleBackColor = false;
            this.rbCutEdge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbCutEdge_MouseDown);
            // 
            // rbPaintPolygon
            // 
            this.rbPaintPolygon.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPaintPolygon.BackColor = System.Drawing.Color.Transparent;
            this.rbPaintPolygon.BackgroundImage = global::KimeraCS.Properties.Resources.draw_paint_polygon;
            this.rbPaintPolygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rbPaintPolygon.Location = new System.Drawing.Point(11, 20);
            this.rbPaintPolygon.Name = "rbPaintPolygon";
            this.rbPaintPolygon.Size = new System.Drawing.Size(48, 48);
            this.rbPaintPolygon.TabIndex = 33;
            this.rbPaintPolygon.TabStop = true;
            this.rbPaintPolygon.UseVisualStyleBackColor = false;
            this.rbPaintPolygon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RbPaintPolygon_MouseDown);
            // 
            // gbGroups
            // 
            this.gbGroups.Controls.Add(this.btnDuplicateGroup);
            this.gbGroups.Controls.Add(this.btnRemoveGroup);
            this.gbGroups.Controls.Add(this.btnGroupProperties);
            this.gbGroups.Controls.Add(this.btnHideShowGroup);
            this.gbGroups.Controls.Add(this.btnDownGroup);
            this.gbGroups.Controls.Add(this.btnUpGroup);
            this.gbGroups.Controls.Add(this.lbGroups);
            this.gbGroups.ForeColor = System.Drawing.SystemColors.Control;
            this.gbGroups.Location = new System.Drawing.Point(131, 2);
            this.gbGroups.Name = "gbGroups";
            this.gbGroups.Size = new System.Drawing.Size(281, 107);
            this.gbGroups.TabIndex = 1;
            this.gbGroups.TabStop = false;
            this.gbGroups.Text = "Groups";
            // 
            // btnDuplicateGroup
            // 
            this.btnDuplicateGroup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDuplicateGroup.Location = new System.Drawing.Point(178, 34);
            this.btnDuplicateGroup.Name = "btnDuplicateGroup";
            this.btnDuplicateGroup.Size = new System.Drawing.Size(99, 23);
            this.btnDuplicateGroup.TabIndex = 4;
            this.btnDuplicateGroup.Text = "Duplicate group";
            this.btnDuplicateGroup.UseVisualStyleBackColor = true;
            this.btnDuplicateGroup.Click += new System.EventHandler(this.BtnDuplicateGroup_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemoveGroup.Location = new System.Drawing.Point(178, 12);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new System.Drawing.Size(99, 23);
            this.btnRemoveGroup.TabIndex = 3;
            this.btnRemoveGroup.Text = "Remove group";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            this.btnRemoveGroup.Click += new System.EventHandler(this.BtnRemoveGroup_Click);
            // 
            // btnGroupProperties
            // 
            this.btnGroupProperties.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGroupProperties.Location = new System.Drawing.Point(178, 56);
            this.btnGroupProperties.Name = "btnGroupProperties";
            this.btnGroupProperties.Size = new System.Drawing.Size(99, 23);
            this.btnGroupProperties.TabIndex = 5;
            this.btnGroupProperties.Text = "Group properties";
            this.btnGroupProperties.UseVisualStyleBackColor = true;
            this.btnGroupProperties.Click += new System.EventHandler(this.BtnGroupProperties_Click);
            // 
            // btnHideShowGroup
            // 
            this.btnHideShowGroup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHideShowGroup.Location = new System.Drawing.Point(178, 78);
            this.btnHideShowGroup.Name = "btnHideShowGroup";
            this.btnHideShowGroup.Size = new System.Drawing.Size(99, 23);
            this.btnHideShowGroup.TabIndex = 6;
            this.btnHideShowGroup.Text = "Hide/Show group";
            this.btnHideShowGroup.UseVisualStyleBackColor = true;
            this.btnHideShowGroup.Click += new System.EventHandler(this.BtnHideShowGroup_Click);
            // 
            // btnDownGroup
            // 
            this.btnDownGroup.BackgroundImage = global::KimeraCS.Properties.Resources.sort_down;
            this.btnDownGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDownGroup.Location = new System.Drawing.Point(4, 58);
            this.btnDownGroup.Name = "btnDownGroup";
            this.btnDownGroup.Size = new System.Drawing.Size(19, 42);
            this.btnDownGroup.TabIndex = 2;
            this.btnDownGroup.UseVisualStyleBackColor = true;
            this.btnDownGroup.Click += new System.EventHandler(this.BtnDownGroup_Click);
            // 
            // btnUpGroup
            // 
            this.btnUpGroup.BackgroundImage = global::KimeraCS.Properties.Resources.sort_up;
            this.btnUpGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUpGroup.Location = new System.Drawing.Point(4, 16);
            this.btnUpGroup.Name = "btnUpGroup";
            this.btnUpGroup.Size = new System.Drawing.Size(19, 42);
            this.btnUpGroup.TabIndex = 1;
            this.btnUpGroup.UseVisualStyleBackColor = true;
            this.btnUpGroup.Click += new System.EventHandler(this.BtnUpGroup_Click);
            // 
            // lbGroups
            // 
            this.lbGroups.FormattingEnabled = true;
            this.lbGroups.IntegralHeight = false;
            this.lbGroups.Location = new System.Drawing.Point(23, 16);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.Size = new System.Drawing.Size(155, 84);
            this.lbGroups.TabIndex = 0;
            this.lbGroups.Click += new System.EventHandler(this.LbGroups_Click);
            this.lbGroups.DoubleClick += new System.EventHandler(this.LbGroups_DoubleClick);
            // 
            // gbLight
            // 
            this.gbLight.Controls.Add(this.lblZ);
            this.gbLight.Controls.Add(this.hsbLightZ);
            this.gbLight.Controls.Add(this.lblY);
            this.gbLight.Controls.Add(this.hsbLightY);
            this.gbLight.Controls.Add(this.lblX);
            this.gbLight.Controls.Add(this.hsbLightX);
            this.gbLight.Controls.Add(this.chkEnableLighting);
            this.gbLight.ForeColor = System.Drawing.SystemColors.Control;
            this.gbLight.Location = new System.Drawing.Point(6, 2);
            this.gbLight.Name = "gbLight";
            this.gbLight.Size = new System.Drawing.Size(119, 107);
            this.gbLight.TabIndex = 0;
            this.gbLight.TabStop = false;
            this.gbLight.Text = "Light";
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(6, 54);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(17, 13);
            this.lblZ.TabIndex = 6;
            this.lblZ.Text = "Z:";
            // 
            // hsbLightZ
            // 
            this.hsbLightZ.Enabled = false;
            this.hsbLightZ.LargeChange = 1;
            this.hsbLightZ.Location = new System.Drawing.Point(29, 52);
            this.hsbLightZ.Maximum = 10;
            this.hsbLightZ.Minimum = -10;
            this.hsbLightZ.Name = "hsbLightZ";
            this.hsbLightZ.Size = new System.Drawing.Size(84, 17);
            this.hsbLightZ.TabIndex = 5;
            this.hsbLightZ.ValueChanged += new System.EventHandler(this.HsbLightZ_ValueChanged);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(6, 36);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 4;
            this.lblY.Text = "Y:";
            // 
            // hsbLightY
            // 
            this.hsbLightY.Enabled = false;
            this.hsbLightY.LargeChange = 1;
            this.hsbLightY.Location = new System.Drawing.Point(29, 34);
            this.hsbLightY.Maximum = 10;
            this.hsbLightY.Minimum = -10;
            this.hsbLightY.Name = "hsbLightY";
            this.hsbLightY.Size = new System.Drawing.Size(84, 17);
            this.hsbLightY.TabIndex = 3;
            this.hsbLightY.ValueChanged += new System.EventHandler(this.HsbLightY_ValueChanged);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(6, 18);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 2;
            this.lblX.Text = "X:";
            // 
            // hsbLightX
            // 
            this.hsbLightX.Enabled = false;
            this.hsbLightX.LargeChange = 1;
            this.hsbLightX.Location = new System.Drawing.Point(29, 16);
            this.hsbLightX.Maximum = 10;
            this.hsbLightX.Minimum = -10;
            this.hsbLightX.Name = "hsbLightX";
            this.hsbLightX.Size = new System.Drawing.Size(84, 17);
            this.hsbLightX.TabIndex = 1;
            this.hsbLightX.ValueChanged += new System.EventHandler(this.HsbLightX_ValueChanged);
            // 
            // chkEnableLighting
            // 
            this.chkEnableLighting.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnableLighting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkEnableLighting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkEnableLighting.Image = global::KimeraCS.Properties.Resources.lightbulb_off16;
            this.chkEnableLighting.Location = new System.Drawing.Point(6, 75);
            this.chkEnableLighting.Name = "chkEnableLighting";
            this.chkEnableLighting.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkEnableLighting.Size = new System.Drawing.Size(107, 24);
            this.chkEnableLighting.TabIndex = 7;
            this.chkEnableLighting.Text = "Enable lighting";
            this.chkEnableLighting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableLighting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.chkEnableLighting.UseVisualStyleBackColor = true;
            this.chkEnableLighting.CheckedChanged += new System.EventHandler(this.ChkEnableLighting_CheckedChanged);
            // 
            // panelEditorPModel
            // 
            this.panelEditorPModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditorPModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelEditorPModel.Location = new System.Drawing.Point(159, 25);
            this.panelEditorPModel.Name = "panelEditorPModel";
            this.panelEditorPModel.Size = new System.Drawing.Size(417, 322);
            this.panelEditorPModel.TabIndex = 4;
            this.panelEditorPModel.TabStop = false;
            this.panelEditorPModel.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelEditorPModel_Paint);
            this.panelEditorPModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelEditorPModel_MouseDown);
            this.panelEditorPModel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelEditorPModel_MouseMove);
            this.panelEditorPModel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelEditorPModel_MouseUp);
            // 
            // tmrRenderPModel
            // 
            this.tmrRenderPModel.Interval = 500;
            this.tmrRenderPModel.Tick += new System.EventHandler(this.TmrRenderPModel_Tick);
            // 
            // FrmPEditor
            // 
            this.AcceptButton = this.btnCommitChanges;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(720, 547);
            this.Controls.Add(this.panelEditorPModel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(734, 575);
            this.Name = "FrmPEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "P Editor";
            this.Activated += new System.EventHandler(this.FrmPEditor_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPEditor_FormClosed);
            this.Load += new System.EventHandler(this.FrmPEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPEditor_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmPEditor_KeyUp);
            this.Move += new System.EventHandler(this.FrmPEditor_Move);
            this.Resize += new System.EventHandler(this.FrmPEditor_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbColorEditor.ResumeLayout(false);
            this.gbColorEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaletteColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).EndInit();
            this.gbDrawMode.ResumeLayout(false);
            this.gbDrawMode.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.gbPlaneOperations.ResumeLayout(false);
            this.gbPlaneOperations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudZPlane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlphaPlane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPlane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXPlane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBetaPlane)).EndInit();
            this.gbRotation.ResumeLayout(false);
            this.gbRotation.PerformLayout();
            this.gbReposition.ResumeLayout(false);
            this.gbReposition.PerformLayout();
            this.gbResize.ResumeLayout(false);
            this.gbResize.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.gbDrawingOptions.ResumeLayout(false);
            this.gbGroups.ResumeLayout(false);
            this.gbLight.ResumeLayout(false);
            this.gbLight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEditorPModel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveModelAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.PictureBox panelEditorPModel;
        private System.Windows.Forms.GroupBox gbRotation;
        private System.Windows.Forms.GroupBox gbReposition;
        private System.Windows.Forms.GroupBox gbResize;
        private System.Windows.Forms.Label lblRotateGamma;
        private System.Windows.Forms.Label lblRotateBeta;
        private System.Windows.Forms.Label lblRotateAlpha;
        private System.Windows.Forms.GroupBox gbLight;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.GroupBox gbGroups;
        private System.Windows.Forms.GroupBox gbDrawMode;
        private System.Windows.Forms.RadioButton rbVertexColors;
        private System.Windows.Forms.RadioButton rbPolygonColors;
        private System.Windows.Forms.RadioButton rbMesh;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.Button btnDownGroup;
        private System.Windows.Forms.Button btnUpGroup;
        private System.Windows.Forms.Button btnRemoveGroup;
        private System.Windows.Forms.Button btnGroupProperties;
        private System.Windows.Forms.Button btnHideShowGroup;
        private System.Windows.Forms.Label lblResizePieceZ;
        private System.Windows.Forms.Label lblResizePieceY;
        private System.Windows.Forms.Label lblResizePieceX;
        private System.Windows.Forms.Label lblRepositionZ;
        private System.Windows.Forms.Label lblRepositionY;
        private System.Windows.Forms.Label lblRepositionX;
        private System.Windows.Forms.GroupBox gbPlaneOperations;
        private System.Windows.Forms.NumericUpDown nudXPlane;
        private System.Windows.Forms.Label lblZPO;
        private System.Windows.Forms.Label lblYPO;
        private System.Windows.Forms.Label lblXPO;
        private System.Windows.Forms.NumericUpDown nudZPlane;
        private System.Windows.Forms.NumericUpDown nudYPlane;
        private System.Windows.Forms.NumericUpDown nudBetaPlane;
        private System.Windows.Forms.Label lblBetaPO;
        private System.Windows.Forms.Label lblAlphaPO;
        private System.Windows.Forms.NumericUpDown nudAlphaPlane;
        private System.Windows.Forms.ToolStripMenuItem planeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mirrorModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeModelSimmetricToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eraseLowerEmisphereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem fattenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slimToolStripMenuItem;
        private System.Windows.Forms.Button btnCommitChanges;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbDrawingOptions;
        private System.Windows.Forms.RadioButton rbNewPolygon;
        private System.Windows.Forms.RadioButton rbPanning;
        private System.Windows.Forms.RadioButton rbZoomInOut;
        private System.Windows.Forms.RadioButton rbFreeRotate;
        private System.Windows.Forms.RadioButton rbMoveVertex;
        private System.Windows.Forms.RadioButton rbErasePolygon;
        private System.Windows.Forms.RadioButton rbCutEdge;
        private System.Windows.Forms.RadioButton rbPaintPolygon;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem invertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        public System.Windows.Forms.HScrollBar hsbRotateAlpha;
        public System.Windows.Forms.HScrollBar hsbRotateGamma;
        public System.Windows.Forms.HScrollBar hsbRotateBeta;
        public System.Windows.Forms.CheckBox chkEnableLighting;
        public System.Windows.Forms.CheckBox chkShowPlane;
        public System.Windows.Forms.CheckBox chkShowAxes;
        public System.Windows.Forms.HScrollBar hsbLightZ;
        public System.Windows.Forms.HScrollBar hsbLightY;
        public System.Windows.Forms.HScrollBar hsbLightX;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.GroupBox gbColorEditor;
        private System.Windows.Forms.Label lblPalette;
        private System.Windows.Forms.Label lblBlueLevel;
        private System.Windows.Forms.Label lblGreenLevel;
        private System.Windows.Forms.Label lblRedLevel;
        private System.Windows.Forms.Label lblDetectionThreshold;
        private System.Windows.Forms.TextBox txtThresholdSlider;
        private System.Windows.Forms.HScrollBar hsbThresholdSlider;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.TextBox txtSelectedColorB;
        private System.Windows.Forms.HScrollBar hsbSelectedColorB;
        private System.Windows.Forms.TextBox txtSelectedColorG;
        private System.Windows.Forms.HScrollBar hsbSelectedColorG;
        private System.Windows.Forms.TextBox txtSelectedColorR;
        private System.Windows.Forms.HScrollBar hsbSelectedColorR;
        private System.Windows.Forms.Button btnLessBrightness;
        private System.Windows.Forms.Button btnMoreBrightness;
        private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllPolysSelectedColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllPolysnotSelectedColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killPrecalculatedLightningToolStripMenuItem;
        public System.Windows.Forms.PictureBox pbPalette;
        private System.Windows.Forms.Button btnResetBrightness;
        public System.Windows.Forms.HScrollBar hsbResizeX;
        public System.Windows.Forms.TextBox txtResizeY;
        public System.Windows.Forms.HScrollBar hsbResizeY;
        public System.Windows.Forms.TextBox txtResizeX;
        public System.Windows.Forms.TextBox txtRotateGamma;
        public System.Windows.Forms.TextBox txtRotateAlpha;
        public System.Windows.Forms.TextBox txtRotateBeta;
        public System.Windows.Forms.TextBox txtResizeZ;
        public System.Windows.Forms.HScrollBar hsbResizeZ;
        public System.Windows.Forms.TextBox txtRepositionZ;
        public System.Windows.Forms.HScrollBar hsbRepositionZ;
        public System.Windows.Forms.TextBox txtRepositionY;
        public System.Windows.Forms.HScrollBar hsbRepositionY;
        public System.Windows.Forms.TextBox txtRepositionX;
        public System.Windows.Forms.HScrollBar hsbRepositionX;
        public System.Windows.Forms.CheckBox chkPaletteMode;
        public System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbPaletteColor;
        public System.Windows.Forms.Timer tmrRenderPModel;
        private System.Windows.Forms.Button btnDuplicateGroup;
        private System.Windows.Forms.ToolStripMenuItem loadModelAsNewGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem unselectGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mergeGroupsIntoOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeGroupsIntoOnenotTexturedToolStripMenuItem;
        public System.Windows.Forms.CheckBox chkVertexColor;
        private System.Windows.Forms.ToolStripMenuItem ResetCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}