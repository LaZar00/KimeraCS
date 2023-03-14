using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


using HDC = System.IntPtr;


namespace KimeraCS
{

    using Defines;

    using static frmPEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;
    using static FF7FieldRSDResource;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    using static FF7TEXTexture;
    using static FF7PModel;
    using static FF7TMDModel;

    using static ModelDrawing;
    using static InputBoxCS;

    using static Lighting;
    using static Model_3DS;

    using static UndoRedo;
    using static Utils;
    using static FileTools;
    using static OpenGL32;
    using static User32;
    using static GDI32;

    public partial class frmSkeletonEditor : Form
    {

        public const string STR_APPNAME = "KimeraCS 1.7d";

        public static int modelWidth;
        public static int modelHeight;

        public static int SelectedBone;
        public static int SelectedBonePiece;

        public static double diameter;

        public static double alpha, beta, gamma;
        public static double DIST;
        public static float panX, panY, panZ;

        public static bool bLoaded, bChangesDone;
        public static bool selectBoneForWeaponAttachmentQ;
        public static int nWeapons;

        public static int x_last, y_last;

        public static bool loadingBoneModifiersQ;
        public static bool loadingBonePieceModifiersQ;
        public static bool loadingAnimationQ;
        public static bool controlPressedQ;

        //public static bool OpenGLValid = false;

        public static HDC panelModelDC;
        public static HDC textureViewerDC;
        public static HDC OGLContext;

        // This is for the Copy/Paste Frame feature
        FieldFrame CopyfFieldFrame;
        BattleFrame CopybBattleFrame;
        BattleFrame CopybBattleWFrame;

        // Public vars of controls
        // Drawing
        public static bool bDListsEnable;
        public static bool bShowBones;
        public static bool bShowGround;
        public static bool bShowLastFrameGhost;
        public static bool bDontRefreshPicture;

        // Animation
        public static int iCurrentFrameScroll;
        public static int ianimIndex;
        public static int ianimWeaponIndex;
        public static int iFPS;
        public static float fFPS;

        // Lighting
        public static bool bchkFrontLight;
        public static bool bchkRearLight;
        public static bool bchkRightLight;
        public static bool bchkLeftLight;
        public static bool infinityFarQ;
        public static float fLightPosXScroll, fLightPosYScroll, fLightPosZScroll;

        // Mouse PanelModel
        private bool pbMouseIsDown;
        public bool pbIsMinimized;

        // Undo/Redo feature
        public static bool DoNotAddStateQ;

        // Helper Vars
        int nUDTexUpDown;

        // Other forms instances of main frmSkeletonEditor
        frmFieldDB frmFieldDatabase;
        frmBattleDB frmBattleDatabase;
        frmMagicDB frmMagicDatabase;
        frmInterpolateAll frmInterpAll;
        frmTEXToPNGBatchConversion frmTEX2PNGBC;
        frmSkeletonJoints frmSJ;

        // StopWatch
        Stopwatch swPlayAnimation;


        // PEditor vars
        public frmPEditor frmPEdit;

        // Texture Viewer vars
        public frmTextureViewer frmTexViewer;
        public bool bPaintGreen;
        public int iTexCoordViewerScale;

        // TMD Object List
        public frmTMDObjList frmTMDOL;

        // DPI vars
        public decimal dDPIScaleFactor;


        public frmSkeletonEditor()
        {
            InitializeComponent();

            strGlobalPath = Application.StartupPath;
        }


        /////////////////////////////////////////////////////////////
        // OpenGL methods:
        public void SetOGLSettings()
        {
            glClearDepth(1.0f);
            glDepthFunc(glFunc.GL_LEQUAL);
            glEnable(glCapability.GL_DEPTH_TEST);
            glEnable(glCapability.GL_BLEND);
            glEnable(glCapability.GL_ALPHA_TEST);
            glBlendFunc(glBlendFuncFactor.GL_SRC_ALPHA, glBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);
            glAlphaFunc(glFunc.GL_GREATER, 0);
            glCullFace(glFace.GL_FRONT);
            glEnable(glCapability.GL_CULL_FACE);
        }

        public void InitOpenGLContext()
        {

            //DisableOpenGL(OGLContext);
            panelModelDC = GetDC(panelModel.Handle);
            OGLContext = CreateOGLContext(panelModelDC);

            glEnable(glCapability.GL_DEPTH_TEST);
            glClearColor(0.4f, 0.4f, 0.65f, 0);
            //glClearColor(0.20f, 0.20f, 0.28f, 0);

            SetOGLContext(panelModelDC, OGLContext);
        }
        /////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////
        // ToolTip Helpers:
        // Create the ToolTip and associate with the Form container.
        ToolTip toolTip1 = new ToolTip();

        public void DefineToolTips()
        {
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(btnCopyFrame, "Copy frame");
            toolTip1.SetToolTip(btnPasteFrame, "Paste frame");
            toolTip1.SetToolTip(btnRotate, "Rotate texture");
            toolTip1.SetToolTip(btnFlipVertical, "Flip texture vertical");
            toolTip1.SetToolTip(btnFlipHorizontal, "Flip texture horizontal");
            toolTip1.SetToolTip(btnFrameBegin, "Rewind to first frame");
            toolTip1.SetToolTip(btnFrameEnd, "Forward to last frame");
            toolTip1.SetToolTip(btnFrameNext, "Next frame");
            toolTip1.SetToolTip(btnFramePrev, "Previous frame");
            toolTip1.SetToolTip(btnPlayStopAnim, "Play/Stop Animation");
        }


        /////////////////////////////////////////////////////////////
        // MAIN LOAD FORM
        public void InitializeDBs()
        {
            // Instantiate Field Database form
            switch (ReadCharFilterFile())
            {
                case 0:
                    MessageBox.Show("Caution: " + CHAR_LGP_FILTER_FILE_NAME + " file does not exists. Field Database NOT available.", "Warning", MessageBoxButtons.OK);

                    bDBLoaded = false;
                    showCharlgpToolStripMenuItem.Enabled = false;
                    break;

                case -1:
                    MessageBox.Show("Error reading " + CHAR_LGP_FILTER_FILE_NAME + " file. Field Database NOT available.", "Error", MessageBoxButtons.OK);

                    bDBLoaded = false;
                    showCharlgpToolStripMenuItem.Enabled = false;
                    break;

                default:
                    // Instantiate Database form
                    bDBLoaded = true;
                    frmFieldDatabase = new frmFieldDB();
                    break;
            }

            // Instantiate Battle Databases (Enemies/Locations/MainPCs) form
            switch (ReadBattleFilterFile(BATTLEENEMIES_LGP_FILTER_FILE_NAME, ref lstBattleEnemiesLGPRegisters))
            {
                case 0:
                    MessageBox.Show("Caution: " + BATTLEENEMIES_LGP_FILTER_FILE_NAME + " file does not exists. Battle Enemies Database NOT available.", "Warning", MessageBoxButtons.OK);

                    bDBEnemiesLoaded = false;                    
                    break;

                case -1:
                    MessageBox.Show("Error reading " + BATTLEENEMIES_LGP_FILTER_FILE_NAME + " file. Battle Enemies Database NOT available.", "Error", MessageBoxButtons.OK);

                    bDBEnemiesLoaded = false;
                    break;

                default:
                    bDBEnemiesLoaded = true;
                    break;
            }

            switch (ReadBattleFilterFile(BATTLELOCATIONS_LGP_FILTER_FILE_NAME, ref lstBattleLocationsLGPRegisters))
            {
                case 0:
                    MessageBox.Show("Caution: " + BATTLELOCATIONS_LGP_FILTER_FILE_NAME + " file does not exists. Battle Locations Database NOT available.", "Warning", MessageBoxButtons.OK);

                    bDBLocationsLoaded = false;
                    break;

                case -1:
                    MessageBox.Show("Error reading " + BATTLELOCATIONS_LGP_FILTER_FILE_NAME + " file. Battle Locations Database NOT available.", "Error", MessageBoxButtons.OK);

                    bDBLocationsLoaded = false;
                    break;

                default:
                    bDBLocationsLoaded = true;
                    break;
            }

            switch (ReadBattleFilterFile(BATTLEMAINPCS_LGP_FILTER_FILE_NAME, ref lstBattleMainPCsLGPRegisters))
            {
                case 0:
                    MessageBox.Show("Caution: " + BATTLEMAINPCS_LGP_FILTER_FILE_NAME + " file does not exists. Battle Main PCs Database NOT available.", "Warning", MessageBoxButtons.OK);

                    bDBMainPCsLoaded = false;
                    break;

                case -1:
                    MessageBox.Show("Error reading " + BATTLEMAINPCS_LGP_FILTER_FILE_NAME + " file. Battle Main PCs Database NOT available.", "Error", MessageBoxButtons.OK);

                    bDBMainPCsLoaded = false;
                    break;

                default:
                    bDBMainPCsLoaded = true;
                    break;
            }

            if (bDBEnemiesLoaded || bDBLocationsLoaded || bDBMainPCsLoaded)
            {
                showBattlelgpToolStripMenuItem.Enabled = true;
                frmBattleDatabase = new frmBattleDB();
            }                
            else
            {
                showBattlelgpToolStripMenuItem.Enabled = false;
            }

            // Instantiate Magic Database form
            switch (ReadBattleFilterFile(MAGIC_LGP_FILTER_FILE_NAME, ref lstMagicLGPRegisters))
            {
                case 0:
                    MessageBox.Show("Caution: " + MAGIC_LGP_FILTER_FILE_NAME + " file does not exists. Magic Database NOT available.", "Warning", MessageBoxButtons.OK);

                    showMagiclgpToolStripMenuItem.Enabled = false;
                    bDBMagicLoaded = false;
                    break;

                case -1:
                    MessageBox.Show("Error reading " + MAGIC_LGP_FILTER_FILE_NAME + " file. Magic Database NOT available.", "Error", MessageBoxButtons.OK);

                    showMagiclgpToolStripMenuItem.Enabled = false;
                    bDBMagicLoaded = false;
                    break;

                default:
                    showMagiclgpToolStripMenuItem.Enabled = true;
                    bDBMagicLoaded = true;
                    
                    frmMagicDatabase = new frmMagicDB();
                    break;
            }

        }

        private void frmSkeletonEditor_Load(object sender, EventArgs e)
        {
            ReadCFGFile();
            Text = STR_APPNAME;

            //   Some initial settings to reduce flickering of the screen
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Let's detect the original DPI Scale Factor of the computer
            Screen[] screenList = Screen.AllScreens;

            foreach (Screen screen in screenList)
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen.DeviceName, -1, ref dm);

                dDPIScaleFactor = Math.Round(Decimal.Divide(dm.dmPelsWidth, screen.Bounds.Width), 2);
            }


            // Initialize Databases files
            InitializeDBs();


            // Instantiate other forms
            frmInterpAll = new frmInterpolateAll();
            frmTEX2PNGBC = new frmTEXToPNGBatchConversion();

            // Set Minimum Size (using the property sometimes changes auto in designer)
            // Init also Width/Height as per CFG values
            this.MinimumSize = new Size(750, 688);
            this.Size = new Size(isizeWindowWidth, isizeWindowHeight);

            if (iwindowPosX == 0 && iwindowPosY == 0) this.CenterToScreen();
            else this.Location = new Point(iwindowPosX, iwindowPosY);

            // Undo/Redo feature
            DoNotAddStateQ = false;
            InitializeUndoRedo();

            // Generic local vars
            hsbLightPosX.Maximum = Lighting.LIGHT_STEPS;
            hsbLightPosX.Minimum = -Lighting.LIGHT_STEPS;
            hsbLightPosY.Maximum = Lighting.LIGHT_STEPS;
            hsbLightPosY.Minimum = -Lighting.LIGHT_STEPS;
            hsbLightPosZ.Maximum = Lighting.LIGHT_STEPS;
            hsbLightPosZ.Minimum = -Lighting.LIGHT_STEPS;
            
            textureViewerDC = GetDC(pbTextureViewer.Handle);

            bLoaded = false;

            cbBoneSelector.Items.Add("None");

            nUDTexUpDown = (int)nUDMoveTextureUpDown.Value;

            // Timer Play Animations
            swPlayAnimation = new Stopwatch();
            iFPS = 15;
            fFPS = 1000 / iFPS;

            // Init Copy/Paste Frames vars
            CopyfFieldFrame = new FieldFrame();
            CopybBattleFrame = new BattleFrame();
            CopybBattleWFrame = new BattleFrame();

            // Some few Hints/ToolTips
            DefineToolTips();

            // Define CTRL+Home shortcut for reset camera feature ("Home" key is not present in my visual studio notebook? is a VS bug?
            resetCameraToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Home;
            resetCameraToolStripMenuItem.ShortcutKeyDisplayString = "CTRL+Home";

            // Activate MouseWheel events for panelModel PictureBox;
            panelModel.MouseWheel += panelModel_MouseWheel;

            // Texture Coordinates(UVs) Viewer Globals
            bPaintGreen = false;
            iTexCoordViewerScale = 1;

            // Initialize OpenGL Context;
            InitOpenGLContext();
        }

        private void frmSkeletonEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DestroySkeleton();

            DisableOpenGL(OGLContext);
            btnPlayStopAnim.Checked = false;
        }

        private void frmSkeletonEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) controlPressedQ = true;

            if (e.KeyCode == Keys.Escape) selectBoneForWeaponAttachmentQ = false;

            if (e.KeyCode == Keys.Delete && SelectedBone > -1)
                btnRemovePiece.PerformClick();

            if (e.KeyCode == Keys.Space)
                if (cbBoneSelector.SelectedIndex >= 0)
                {
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            if (fSkeleton.bones[SelectedBone].nResources > 0)
                                SelectedBonePiece = 0;
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            if (bSkeleton.bones[SelectedBone].Models.Count > 0)
                                SelectedBonePiece = 0;
                            break;
                    }

                    panelModel_DoubleClick(sender, e);
                }                   

            if (e.KeyCode == Keys.F2)
                if (IsTMDModel) frmTMDOL.Show();

            if (controlPressedQ && e.KeyCode == Keys.Up) alpha++;
            if (controlPressedQ && e.KeyCode == Keys.Down) alpha--;
            if (controlPressedQ && e.KeyCode == Keys.Left) beta--;
            if (controlPressedQ && e.KeyCode == Keys.Right) beta++;

            panelModel.Update();
            panelModel_Paint(null, null);
            textureViewer_Paint(null, null);
        }

        private void frmSkeletonEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey) controlPressedQ = false;

            panelModel.Update();
            panelModel_Paint(null, null);
            textureViewer_Paint(null, null);
        }

        private void frmSkeletonEditor_Resize(object sender, EventArgs e)
        {
            // Check first if minimized.
            if (Application.OpenForms.Count > 0)
            {
                if (Application.OpenForms[0].WindowState == FormWindowState.Minimized) return;

                // We can redraw the model in panel
                panelModel.Update();
                panelModel_Paint(null, null);

                // We can redraw the texture in picturebox
                pbTextureViewer.Update();
                textureViewer_Paint(null, null);
            }

            // Assign this size if visible
            if (this.Visible)
            {
                isizeWindowWidth = this.Size.Width;
                isizeWindowHeight = this.Size.Height;
                WriteCFGFile();
            }
        }

        public void InitializeWinFormsDataControls()
        {
            // Init vars
            ianimIndex = 0;
            ianimWeaponIndex = 0;

            SelectedBone = -1;
            SelectedBonePiece = -1;
            Text = STR_APPNAME;
            

            // UndoRedo
            UndoCursor = 0;
            RedoCursor = 0;


            // Anims Buttons
            if (btnPlayStopAnim.Checked) btnPlayStopAnim.Checked = false;

            lblAnimationFrame.Visible = false;
            txtAnimationFrame.Visible = false;
            btnPlayStopAnim.Visible = false;
            btnFrameBegin.Visible = false;
            btnFrameEnd.Visible = false;
            btnFrameNext.Visible = false;
            btnFramePrev.Visible = false;

            iCurrentFrameScroll = 0;
            txtAnimationFrame.Text = iCurrentFrameScroll.ToString();
            tbCurrentFrameScroll.Maximum = 0;
            tbCurrentFrameScroll.Visible = false;
            
            chkShowBones.Visible = false;
            btnCopyFrame.Visible = false;
            btnPasteFrame.Visible = false;
            btnPasteFrame.Enabled = false;
            txtCopyPasteFrame.Visible = false;
            txtCopyPasteFrame.Text = "";
            btnInterpolateAnimation.Visible = false;

            loadingBonePieceModifiersQ = true;

            txtResizePieceX.Text = "100";
            txtResizePieceY.Text = "100";
            txtResizePieceZ.Text = "100";
            hsbResizePieceX.Value = 100;
            hsbResizePieceY.Value = 100;
            hsbResizePieceZ.Value = 100;

            txtRepositionX.Text = "0";
            txtRepositionY.Text = "0";
            txtRepositionZ.Text = "0";
            hsbRepositionX.Value = 0;
            hsbRepositionY.Value = 0;
            hsbRepositionZ.Value = 0;

            txtRotateAlpha.Text = "0";
            txtRotateBeta.Text = "0";
            txtRotateGamma.Text = "0";
            hsbRotateAlpha.Value = 0;
            hsbRotateBeta.Value = 0;
            hsbRotateGamma.Value = 0;

            loadingBonePieceModifiersQ = false;

            lblBoneSelector.Visible = false;
            cbBoneSelector.Visible = false;
            cbBoneSelector.SelectedIndex = -1;
            cbBoneSelector.Items.Clear();
            editJointToolStripMenuItem.Enabled = false;

            gbSelectedBoneFrame.Visible = false;
            gbTexturesFrame.Visible = false;
            //gbTexturesFrame.Location = new Point(gbTexturesFrame.Location.X, 193);
            gbAnimationOptionsFrame.Visible = false;

            btnComputeGroundHeight.Visible = false;

            cbBattleAnimation.Items.Clear();
            cbBattleAnimation.Visible = false;
            cbWeapon.Visible = false;
            cbWeapon.Items.Clear();
            lblBattleAnimation.Visible = false;
            lblWeapon.Visible = false;
            btnComputeWeaponPosition.Visible = false;

            // Menu Strip
            skeletonToolStripMenuItem.Enabled = false;
            loadFieldAnimationToolStripMenuItem.Enabled = false;
            loadBattleMagicLimitsAnimationStripMenuItem.Enabled = false;

            saveAnimationToolStripMenuItem.Enabled = false;
            saveAnimationAsToolStripMenuItem.Enabled = false;
            outputFramesDataTXTToolStripMenuItem.Enabled = false;
            inputFramesDataTXTToolStripMenuItem.Enabled = false;
            inputFramesDataTXTToolSelectiveStripMenuItem.Enabled = false;
            mergeFramesDataTXTToolStripMenuItem.Enabled = false;

            saveSkeletonToolStripMenuItem.Enabled = false;
            saveSkeletonAsToolStripMenuItem.Enabled = false;

            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;


            // TMD vars
            IsTMDModel = false;
            if (frmTMDOL != null) frmTMDOL.Dispose();

            // RSD vars
            IsRSDResource = false;

            // Model intrinsic vars
            bSkeleton.IsBattleLocation = false;

            //SetOGLContext(panelModelDC, OGLContext);
            //SetOGLSettings();
        }

        public void EnableWinFormsDataControls()
        {
            int wi, ai;

            // Visual controls
            switch (modelType)
            {
                case K_P_FIELD_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    gbSelectedPieceFrame.Enabled = true;

                    // Menu Strip
                    saveSkeletonAsToolStripMenuItem.Enabled = true;
                    break;

                case K_HRC_SKELETON:
                    lblBoneSelector.Visible = true;
                    cbBoneSelector.Visible = true;

                    gbSelectedBoneFrame.Visible = true;

                    gbTexturesFrame.Visible = true;
                    gbTexturesFrame.Enabled = false;
                    gbTexturesFrame.Text = "Textures (Part)";

                    tbCurrentFrameScroll.Value = 0;
                    tbCurrentFrameScroll.Minimum = 0;
                    
                    tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                    tbCurrentFrameScroll.Enabled = true;
                    txtAnimationFrame.Text = tbCurrentFrameScroll.Value.ToString();
                    lblAnimationFrame.Visible = true;
                    txtAnimationFrame.Visible = true;
                    tbCurrentFrameScroll.Visible = true;

                    btnPlayStopAnim.Visible = true;
                    btnFrameBegin.Visible = true;
                    btnFrameEnd.Visible = true;
                    btnFrameNext.Visible = true;
                    btnFramePrev.Visible = true;

                    btnCopyFrame.Visible = true;
                    btnPasteFrame.Visible = true;
                    txtCopyPasteFrame.Visible = true;
                    chkShowBones.Enabled = true;
                    chkShowBones.Visible = true;
                    btnInterpolateAnimation.Visible = true;

                    gbAnimationOptionsFrame.Visible = true;

                    btnComputeGroundHeight.Visible = true;

                    // Menu Strip
                    skeletonToolStripMenuItem.Enabled = true;
                    loadFieldAnimationToolStripMenuItem.Enabled = true;

                    saveAnimationToolStripMenuItem.Enabled = true;
                    saveAnimationAsToolStripMenuItem.Enabled = true;
                    outputFramesDataTXTToolStripMenuItem.Enabled = true;
                    inputFramesDataTXTToolStripMenuItem.Enabled = true;
                    inputFramesDataTXTToolSelectiveStripMenuItem.Enabled = true;
                    mergeFramesDataTXTToolStripMenuItem.Enabled = true;

                    saveSkeletonToolStripMenuItem.Enabled = true;
                    saveSkeletonAsToolStripMenuItem.Enabled = true;
                    break;

                case K_AA_SKELETON:
                    lblBoneSelector.Visible = true;
                    cbBoneSelector.Visible = true;

                    gbSelectedBoneFrame.Visible = true;

                    gbTexturesFrame.Visible = true;
                    gbTexturesFrame.Enabled = true;
                    gbTexturesFrame.Text = "Textures (Model)";

                    // Battle Weapons
                    for (wi = 0; wi < bSkeleton.nWeapons; wi++)
                    {
                        if (bSkeleton.wpModels[wi].Polys != null)
                            cbWeapon.Items.Add(wi.ToString());
                        else
                            cbWeapon.Items.Add("EMPTY"); 
                    }

                    if (!bSkeleton.IsBattleLocation)
                    {
                        cbBattleAnimation.Visible = true;
                        lblBattleAnimation.Visible = true;
                        lblBattleAnimation.Text = "Battle Animation";

                        btnPlayStopAnim.Visible = true;
                        btnFrameBegin.Visible = true;
                        btnFrameEnd.Visible = true;
                        btnFrameNext.Visible = true;
                        btnFramePrev.Visible = true;

                        btnCopyFrame.Visible = true;
                        btnPasteFrame.Visible = true;
                        txtCopyPasteFrame.Visible = true;
                        chkShowBones.Enabled = true;
                        chkShowBones.Visible = true;

                        // Battle Animations
                        for (ai = 0; ai < bAnimationsPack.nbSkeletonAnims; ai++)
                        {
                            if (bAnimationsPack.SkeletonAnimations[ai].numFramesShort > 0)
                            {
                                cbBattleAnimation.Items.Add(ai.ToString());
                            }
                        }

                        cbBattleAnimation.SelectedIndex = 0;
                        ianimIndex = 0;

                        iCurrentFrameScroll = 0;
                        tbCurrentFrameScroll.Value = 0;
                        tbCurrentFrameScroll.Minimum = 0;
                        tbCurrentFrameScroll.Maximum = bAnimationsPack.SkeletonAnimations[0].numFramesShort - 1;
                        tbCurrentFrameScroll.Enabled = true;
                        txtAnimationFrame.Text = tbCurrentFrameScroll.Value.ToString();
                        lblAnimationFrame.Visible = true;
                        txtAnimationFrame.Visible = true;
                        tbCurrentFrameScroll.Visible = true;

                        btnInterpolateAnimation.Visible = true;

                        gbAnimationOptionsFrame.Visible = true;

                        btnComputeGroundHeight.Visible = true;

                        // Menu Strip
                        loadBattleMagicLimitsAnimationStripMenuItem.Enabled = true;

                        saveAnimationToolStripMenuItem.Enabled = true;
                        saveAnimationAsToolStripMenuItem.Enabled = true;
                    }

                    if (bSkeleton.wpModels.Count > 0)
                    {
                        cbWeapon.Visible = true;
                        lblWeapon.Visible = true;
                        btnComputeWeaponPosition.Visible = bSkeleton.wpModels.Count > 0;
                        cbWeapon.SelectedIndex = 0;
                        ianimWeaponIndex = 0;
                    }

                    // Menu Strip
                    saveSkeletonToolStripMenuItem.Enabled = true;
                    saveSkeletonAsToolStripMenuItem.Enabled = true;
                    break;

                case K_MAGIC_SKELETON:
                    lblBoneSelector.Visible = true;
                    cbBoneSelector.Visible = true;

                    gbSelectedBoneFrame.Visible = true;

                    gbTexturesFrame.Visible = true;
                    gbTexturesFrame.Enabled = true;
                    gbTexturesFrame.Text = "Textures (Model)";

                    cbBattleAnimation.Visible = true;
                    lblBattleAnimation.Visible = true;
                    lblBattleAnimation.Text = "Magic Animation";

                    btnPlayStopAnim.Visible = true;
                    btnFrameBegin.Visible = true;
                    btnFrameEnd.Visible = true;
                    btnFrameNext.Visible = true;
                    btnFramePrev.Visible = true;

                    btnCopyFrame.Visible = true;
                    btnPasteFrame.Visible = true;
                    txtCopyPasteFrame.Visible = true;
                    chkShowBones.Enabled = true;
                    chkShowBones.Visible = true;

                    // Battle Animations
                    for (ai = 0; ai < bAnimationsPack.nbSkeletonAnims; ai++)
                    {
                        if (bAnimationsPack.SkeletonAnimations[ai].numFramesShort > 0)
                        {
                            cbBattleAnimation.Items.Add(ai.ToString());
                        }
                    }

                    cbBattleAnimation.SelectedIndex = 0;
                    ianimIndex = 0;

                    iCurrentFrameScroll = 0;
                    tbCurrentFrameScroll.Value = 0;
                    tbCurrentFrameScroll.Maximum = bAnimationsPack.SkeletonAnimations[0].numFramesShort - 1;
                    tbCurrentFrameScroll.Enabled = true;
                    txtAnimationFrame.Text = tbCurrentFrameScroll.Value.ToString();
                    lblAnimationFrame.Visible = true;
                    txtAnimationFrame.Visible = true;
                    tbCurrentFrameScroll.Visible = true;

                    btnInterpolateAnimation.Visible = true;

                    gbAnimationOptionsFrame.Visible = true;

                    btnComputeGroundHeight.Visible = true;

                    // Menu Strip
                    loadBattleMagicLimitsAnimationStripMenuItem.Enabled = true;

                    saveAnimationToolStripMenuItem.Enabled = true;
                    saveAnimationAsToolStripMenuItem.Enabled = true;

                    saveSkeletonToolStripMenuItem.Enabled = true;
                    saveSkeletonAsToolStripMenuItem.Enabled = true;
                    break;

                default:
                    saveSkeletonAsToolStripMenuItem.Enabled = true;

                    // Menu Strip
                    saveSkeletonToolStripMenuItem.Enabled = true;
                    saveSkeletonAsToolStripMenuItem.Enabled = true;
                    break;
            }

            bChangesDone = false;
            UpdateMainSkeletonWindowTitle();

            // Close previous P Editor if any.
            if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

            WriteCFGFile();
        }

        public void textureViewer_Paint(object sender, PaintEventArgs e)
        {
            if (cbTextureSelect.SelectedIndex > -1)
            {

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].width == 0 ||
                            fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].height == 0)
                            return;

                        if (SelectedBone > -1 && SelectedBonePiece > -1)
                        {
                            if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].texID != 0xFFFFFFFF)
                            {
                                //chkColorKeyFlag.Enabled = true;

                                if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].ColorKeyFlag == 1)
                                    chkColorKeyFlag.Checked = true;
                                else
                                    chkColorKeyFlag.Checked = false;

                                // Let's get maximum size for texture (I do this for simplify the printing)
                                // Some textures can have different width/height sizes.                                
                                pbTextureViewer.Image =
                                    FitBitmapToPictureBox(pbTextureViewer,
                                         fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].width,
                                         fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].height,
                                         fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].HBMP);

                            }
                        }
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        if (bSkeleton.textures[cbTextureSelect.SelectedIndex].texID != 0xFFFFFFFF)
                        {
                            //chkColorKeyFlag.Enabled = true;
                            if (bSkeleton.textures[cbTextureSelect.SelectedIndex].ColorKeyFlag == 1) chkColorKeyFlag.Checked = true;
                            else chkColorKeyFlag.Checked = false;

                            // Let's get maximum size for texture (I do this for simplify the printing)
                            // Some textures can have different width/height sizes.
                            pbTextureViewer.Image =
                                FitBitmapToPictureBox(pbTextureViewer,
                                                      bSkeleton.textures[cbTextureSelect.SelectedIndex].width,
                                                      bSkeleton.textures[cbTextureSelect.SelectedIndex].height,
                                                      bSkeleton.textures[cbTextureSelect.SelectedIndex].HBMP);
                        }

                        break;

                    default:
                        pbTextureViewer.Image = null;
                        pbTextureViewer.Update();

                        break;
                }
            }
            else
            {
                pbTextureViewer.Image = null;
                pbTextureViewer.Update();
            }
        }

        public void panelModel_Paint(object sender, PaintEventArgs e)
        {
            if (bLoaded)
            {
                //if (Application.OpenForms.Count > 1 && bPEditorOpened() && !pbIsMinimized)
                //{
                //    // Render also PEditor. It seems is opened
                //    frmPEdit.panelEditorPModel_Paint(null, null);
                //}

                if (GetOGLContext() != OGLContext)
                    SetOGLContext(panelModelDC, OGLContext);

                SetOGLSettings();

                glViewport(0, 0, panelModel.ClientRectangle.Width, 
                                 panelModel.ClientRectangle.Height);
                ClearPanel();
                SetDefaultOGLRenderState();

                DrawSkeletonModel(panelModel, cbBattleAnimation, cbWeapon, bDListsEnable);

                glFlush();
                SwapBuffers(panelModelDC);

            }
        }

        // BACKUP
        //public void panelModel_Paint(object sender, PaintEventArgs e)
        //{
        //    if (bLoaded)
        //    {

        //        glViewport(0, 0, panelModel.ClientRectangle.Width, panelModel.ClientRectangle.Height);
        //        ClearPanel();
        //        SetDefaultOGLRenderState();

        //        DrawCurrentModel(panelModel, cbBattleAnimation, cbWeapon);

        //        glFlush();
        //        SwapBuffers(panelModelDC);

        //        //if (selectBoneForWeaponAttachmentQ)
        //        //{
        //        //    using (Graphics g = Graphics.FromImage(panelModel.Image))
        //        //    {
        //        //        g.DrawString("Please choose a bone to attach the weapon to...", this.Font, Brushes.Black, new Point(0, 0));
        //        //    }
        //        //}
        //        // Original in VB6
        //        //If SelectBoneForWeaponAttachmentQ Then
        //        //    Picture1.CurrentX = 0
        //        //    Picture1.CurrentY = 0
        //        //    Picture1.Print "Please choose a bone to attach the weapon to"
        //        //End If
        //    }
        //}

        public void PostLoadModelPreparations(ref Point3D p_min, ref Point3D p_max)
        {
            // Fill combobox with list of bones
            FillBoneSelector(cbBoneSelector);

            bLoaded = true;

            alpha = 200;
            beta = 45;
            gamma = 0;
            panX = 0;
            panY = 0;
            panZ = 0;
            DIST = -2 * ComputeSceneRadius(p_min, p_max);
            selectBoneForWeaponAttachmentQ = false;

            glClearDepth(1.0f);
            glDepthFunc(glFunc.GL_LEQUAL);
            glEnable(glCapability.GL_DEPTH_TEST);
            glEnable(glCapability.GL_BLEND);
            glEnable(glCapability.GL_ALPHA_TEST);
            glBlendFunc(glBlendFuncFactor.GL_SRC_ALPHA, glBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);
            glAlphaFunc(glFunc.GL_GREATER, 0);
            glCullFace(glFace.GL_FRONT);
            glEnable(glCapability.GL_CULL_FACE);
        }

        private void checkBDListEnable_CheckedChanged(object sender, EventArgs e)
        {
            bDListsEnable = chkDListEnable.Checked;

            panelModel_Paint(null, null);
        }

        private void cbBoneSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bLoaded)
            {
                SelectedBone = cbBoneSelector.SelectedIndex;
                SelectedBonePiece = -1;

                if (SelectedBone > -1)
                {
                    SetBoneModifiers();
                    if (!FindWindowOpened("frmPEditor")) gbSelectedBoneFrame.Enabled = true;
                    //if (modelType == K_AA_SKELETON) SetTextureEditorFields();

                    if (!FindWindowOpened("frmPEditor")) editJointToolStripMenuItem.Enabled = true;
                }
                else
                {
                    gbSelectedBoneFrame.Enabled = false;
                    editJointToolStripMenuItem.Enabled = false;
                }


                SetTextureEditorFields();

                if (!pbMouseIsDown)
                    panelModel_Paint(null, null);
            }
        }

        public static void FillBoneSelector(ComboBox cbIn)
        {
            int bi;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    //for (bi = 0; bi < fSkeleton.nBones; bi++)
                    for (bi = 0; bi < fSkeleton.bones.Count; bi++)
                    {
                        cbIn.Items.Add(fSkeleton.bones[bi].joint_i + "-" + fSkeleton.bones[bi].joint_f);
                    }

                    cbIn.Enabled = true;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    for (bi = 0; bi < bSkeleton.nBones; bi++)
                    {
                        cbIn.Items.Add("Joint" + bSkeleton.bones[bi].parentBone.ToString() + "- Joint" + bi.ToString());
                    }

                    if (bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0) cbIn.Items.Add("Weapon");

                    cbIn.Enabled = true;
                    break;

                default:
                    cbIn.Enabled = false;
                    break;
            }
        }

        public void SetBoneModifiers()
        {
            loadingBoneModifiersQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    nUDResizeBoneX.Value = (decimal)fSkeleton.bones[SelectedBone].resizeX * 100;
                    nUDResizeBoneY.Value = (decimal)fSkeleton.bones[SelectedBone].resizeY * 100;
                    nUDResizeBoneZ.Value = (decimal)fSkeleton.bones[SelectedBone].resizeZ * 100;

                    txtBoneOptionsLength.Text = fSkeleton.bones[SelectedBone].len.ToString("F7");
                    nUDBoneOptionsLength.Value = (decimal)fSkeleton.bones[SelectedBone].len * 10000;
                    nUDBoneOptionsLength.Increment = Math.Abs(nUDBoneOptionsLength.Value / 100);

                    SetFrameEditorFields();
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    if (SelectedBone == bSkeleton.nBones)
                    {
                        nUDResizeBoneX.Value = (decimal)bSkeleton.wpModels[cbWeapon.SelectedIndex].resizeX * 100;
                        nUDResizeBoneY.Value = (decimal)bSkeleton.wpModels[cbWeapon.SelectedIndex].resizeY * 100;
                        nUDResizeBoneZ.Value = (decimal)bSkeleton.wpModels[cbWeapon.SelectedIndex].resizeZ * 100;

                        lblBoneOptionsLength.Visible = false;
                        txtBoneOptionsLength.Visible = false;
                        nUDBoneOptionsLength.Visible = false;
                        btnAddPiece.Visible = false;
                        btnRemovePiece.Visible = false;
                    }
                    else
                    {
                        nUDResizeBoneX.Value = (decimal)bSkeleton.bones[SelectedBone].resizeX * 100;
                        nUDResizeBoneY.Value = (decimal)bSkeleton.bones[SelectedBone].resizeY * 100;
                        nUDResizeBoneZ.Value = (decimal)bSkeleton.bones[SelectedBone].resizeZ * 100;

                        txtBoneOptionsLength.Text = bSkeleton.bones[SelectedBone].len.ToString("F7");
                        nUDBoneOptionsLength.Value = (decimal)bSkeleton.bones[SelectedBone].len * 10000;
                        nUDBoneOptionsLength.Increment = Math.Abs(nUDBoneOptionsLength.Value / 100);

                        lblBoneOptionsLength.Visible = true;
                        txtBoneOptionsLength.Visible = true;
                        nUDBoneOptionsLength.Visible = true;
                        btnAddPiece.Visible = true;
                        btnRemovePiece.Visible = true;
                    }

                    SetFrameEditorFields();
                    break;

            }

            loadingBoneModifiersQ = false;
        }

        public void SetFrameEditorFields()
        {
            if (modelType < 3 || modelType > 5) return;

            if (btnPlayStopAnim.Checked)
            {
                gbFrameDataPartOptions.Enabled = false;
            }
            else
                if (Math.Abs(nUDFrameDataPart.Value % 3) != K_FRAME_BONE_ROTATION) gbFrameDataPartOptions.Enabled = true;
            else if (SelectedBone <= -1) gbFrameDataPartOptions.Enabled = false;
            else gbFrameDataPartOptions.Enabled = true;

            loadingAnimationQ = true;

            switch (Math.Abs(nUDFrameDataPart.Value % 3))
            {
                case K_FRAME_BONE_ROTATION:
                    if (SelectedBone > -1)
                    {
                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                nUDXAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rotations[SelectedBone].alpha;
                                nUDYAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rotations[SelectedBone].beta;
                                nUDZAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rotations[SelectedBone].gamma;

                                //With AAnim.Frames(CurrentFrameScroll.value).Rotations(SelectedBone)
                                //    XAnimationFramePartText.Text = .alpha
                                //    YAnimationFramePartText.Text = .Beta
                                //    ZAnimationFramePartText.Text = .Gamma
                                //    XAnimationFramePartUpDown.value = .alpha * 10000
                                //    YAnimationFramePartUpDown.value = .Beta * 10000
                                //    ZAnimationFramePartUpDown.value = .Gamma * 10000
                                //End With
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                if (SelectedBone == bSkeleton.nBones)
                                {
                                    nUDXAnimationFramePart.Value = (decimal)bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].alpha;
                                    nUDYAnimationFramePart.Value = (decimal)bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].beta;
                                    nUDZAnimationFramePart.Value = (decimal)bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].gamma;

                                    //                       With DAAnims.WeaponAnimations(anim_index).Frames(CurrentFrameScroll.value).Bones(0)
                                    //                            XAnimationFramePartText.Text = .alpha
                                    //                            YAnimationFramePartText.Text = .Beta
                                    //                            ZAnimationFramePartText.Text = .Gamma
                                    //                            XAnimationFramePartUpDown.value = .alpha * 10000
                                    //                            YAnimationFramePartUpDown.value = .Beta * 10000
                                    //                            ZAnimationFramePartUpDown.value = .Gamma * 10000
                                    //                        End With
                                }
                                else
                                {
                                    int nbone;
                                    if (bSkeleton.nBones > 1) nbone = SelectedBone + 1;
                                    else nbone = SelectedBone + 0;
                                    nUDXAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[nbone].alpha;
                                    nUDYAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[nbone].beta;
                                    nUDZAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[nbone].gamma;

                                    //                        With DAAnims.BodyAnimations(anim_index).Frames(CurrentFrameScroll.value).Bones(SelectedBone + IIf(aa_sk.NumBones > 1, 1, 0))
                                    //                            XAnimationFramePartText.Text = .alpha
                                    //                            YAnimationFramePartText.Text = .Beta
                                    //                            ZAnimationFramePartText.Text = .Gamma
                                    //                            XAnimationFramePartUpDown.value = .alpha * 10000
                                    //                            YAnimationFramePartUpDown.value = .Beta * 10000
                                    //                            ZAnimationFramePartUpDown.value = .Gamma * 10000
                                    //                        End With
                                }

                                break;
                        }
                    }
                    else
                    {
                        nUDXAnimationFramePart.Value = 0;
                        nUDYAnimationFramePart.Value = 0;
                        nUDZAnimationFramePart.Value = 0;

                        gbFrameDataPartOptions.Enabled = false;

                        //  XAnimationFramePartText.Text = " "
                        //  YAnimationFramePartText.Text = " "
                        //  ZAnimationFramePartText.Text = " "
                        //  FrameDataPartOptions.Enabled = False
                    }

                    break;

                case K_FRAME_ROOT_ROTATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            nUDXAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootRotationAlpha;
                            nUDYAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootRotationBeta;
                            nUDZAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootRotationGamma;

                            //With AAnim.Frames(CurrentFrameScroll.value)
                            //    XAnimationFramePartText.Text = .RootRotationAlpha
                            //    YAnimationFramePartText.Text = .RootRotationBeta
                            //    ZAnimationFramePartText.Text = .RootRotationGamma
                            //    XAnimationFramePartUpDown.value = .RootRotationAlpha * 10000
                            //    YAnimationFramePartUpDown.value = .RootRotationBeta * 10000
                            //    ZAnimationFramePartUpDown.value = .RootRotationGamma * 10000
                            //End With
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            nUDXAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].alpha;
                            nUDYAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].beta;
                            nUDZAnimationFramePart.Value = (decimal)bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].bones[0].gamma;

                            //With DAAnims.BodyAnimations(anim_index).Frames(CurrentFrameScroll.value).Bones(0)
                            //    XAnimationFramePartText.Text = .alpha
                            //    YAnimationFramePartText.Text = .Beta
                            //    ZAnimationFramePartText.Text = .Gamma
                            //    XAnimationFramePartUpDown.value = .alpha * 10000
                            //    YAnimationFramePartUpDown.value = .Beta * 10000
                            //    ZAnimationFramePartUpDown.value = .Gamma * 10000
                            //End With
                            break;
                    }

                    break;

                case K_FRAME_ROOT_TRANSLATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            nUDXAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootTranslationX;
                            nUDYAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootTranslationY;
                            nUDZAnimationFramePart.Value = (decimal)fAnimation.frames[tbCurrentFrameScroll.Value].rootTranslationZ;

                            //With AAnim.Frames(CurrentFrameScroll.value)
                            //    XAnimationFramePartText.Text = .RootTranslationX
                            //    YAnimationFramePartText.Text = .RootTranslationY
                            //    ZAnimationFramePartText.Text = .RootTranslationZ
                            //    XAnimationFramePartUpDown.value = .RootTranslationX * 10000
                            //    YAnimationFramePartUpDown.value = .RootTranslationX * 10000
                            //    ZAnimationFramePartUpDown.value = .RootTranslationX * 10000
                            //End With
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            if (SelectedBone == bSkeleton.nBones)
                            {
                                nUDXAnimationFramePart.Value = bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startX;
                                nUDYAnimationFramePart.Value = bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startY;
                                nUDZAnimationFramePart.Value = bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startZ;

                                //   With DAAnims.WeaponAnimations(anim_index).Frames(CurrentFrameScroll.value)
                                //        XAnimationFramePartText.Text = .X_start
                                //        YAnimationFramePartText.Text = .Y_start
                                //        ZAnimationFramePartText.Text = .Z_start
                                //        XAnimationFramePartUpDown.value = .X_start * 10000
                                //        YAnimationFramePartUpDown.value = .Y_start * 10000
                                //        ZAnimationFramePartUpDown.value = .Z_start * 10000
                                //    End With
                            }
                            else
                            {
                                nUDXAnimationFramePart.Value = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startX;
                                nUDYAnimationFramePart.Value = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startY;
                                nUDZAnimationFramePart.Value = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value].startZ;

                                //    With DAAnims.BodyAnimations(anim_index).Frames(CurrentFrameScroll.value)
                                //        XAnimationFramePartText.Text = .X_start
                                //        YAnimationFramePartText.Text = .Y_start
                                //        ZAnimationFramePartText.Text = .Z_start
                                //        XAnimationFramePartUpDown.value = .X_start * 10000
                                //        YAnimationFramePartUpDown.value = .Y_start * 10000
                                //        ZAnimationFramePartUpDown.value = .Z_start * 10000
                                //    End With
                            }
                            break;
                    }

                    break;
            }

            loadingAnimationQ = false;
        }

        private void panelModel_MouseUp(object sender, MouseEventArgs e)
        {
            pbMouseIsDown = false;
        }

        private void panelModel_MouseDown(object sender, MouseEventArgs e)
        {
            int bi, pi;

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            BattleFrame wpFrame;

            pbMouseIsDown = true;

            if (bLoaded)
            {               

                //glClearColor(0.4f, 0.4f, 0.65f, 0);
                //glViewport(0, 0, panelModel.ClientRectangle.Width, panelModel.ClientRectangle.Height);
                //glClear(glBufferMask.GL_COLOR_BUFFER_BIT | glBufferMask.GL_DEPTH_BUFFER_BIT);

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[tbCurrentFrameScroll.Value],
                                                ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max, panX, panY, (float)(panZ + DIST),
                                             (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                        bi = GetClosestFieldBone(fSkeleton, fAnimation.frames[tbCurrentFrameScroll.Value],
                                                 e.X, e.Y);

                        SelectedBone = bi;
                        cbBoneSelector.SelectedIndex = bi;

                        if (bi > -1)
                        {
                            pi = GetClosestFieldBonePiece(fSkeleton, fAnimation.frames[tbCurrentFrameScroll.Value],
                                                          bi, e.X, e.Y);

                            SelectedBonePiece = pi;
                            if (pi > -1)
                            {
                                SetBonePieceModifiers();
                                if (!FindWindowOpened("frmPEditor")) gbSelectedPieceFrame.Enabled = true;
                            }
                            else
                            {
                                gbSelectedBoneFrame.Enabled = false;
                            }

                            SetBoneModifiers();
                            if (!FindWindowOpened("frmPEditor")) gbSelectedBoneFrame.Enabled = true;
                        }
                        else
                        {
                            SelectedBonePiece = -1;
                            gbSelectedBoneFrame.Enabled = false;
                            gbSelectedPieceFrame.Enabled = false;
                        }

                        SetTextureEditorFields();
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        wpFrame = new BattleFrame();

                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value],
                                                 ref p_min, ref p_max);
                        SetCameraAroundModel(ref p_min, ref p_max, panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                        if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.wpModels.Count > 0)
                            wpFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value];

                        bi = GetClosestBattleBone(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value],
                                                  wpFrame, ianimWeaponIndex, e.X, e.Y);

                        SelectedBone = bi;

                        if (bi <= bSkeleton.nBones) cbBoneSelector.SelectedIndex = bi;

                        if (bi > -1 && bi < bSkeleton.nBones)
                        {
                            if (selectBoneForWeaponAttachmentQ) SetWeaponAnimationAttachedToBone(e.Button == MouseButtons.Right, this);

                            if (bSkeleton.IsBattleLocation)
                            {
                                pi = 0;
                            }
                            else
                            {
                                pi = GetClosestBattleBoneModel(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value],
                                                               bi, e.X, e.Y);                                
                            }

                            SelectedBonePiece = pi;
                            SetBoneModifiers();

                            if (pi > -1)
                            {
                                SetBonePieceModifiers();
                                if (!FindWindowOpened("frmPEditor")) gbSelectedPieceFrame.Enabled = true;
                            }
                            else gbSelectedPieceFrame.Enabled = false;
                            if (!FindWindowOpened("frmPEditor")) gbSelectedBoneFrame.Enabled = true;
                        }
                        else
                        {
                            if (bi == bSkeleton.nBones)
                            {
                                SetBoneModifiers();
                                SelectedBonePiece = -2;

                                if (!FindWindowOpened("frmPEditor")) gbSelectedBoneFrame.Enabled = true;
                                SetBonePieceModifiers();

                                if (!FindWindowOpened("frmPEditor")) gbSelectedPieceFrame.Enabled = true;

                                // Select the weapon texture
                                if (bSkeleton.wpModels[cbWeapon.SelectedIndex].Groups[0].texFlag == 1)
                                {
                                    if (bSkeleton.textures.Count > bSkeleton.wpModels[cbWeapon.SelectedIndex].Groups[0].texID)
                                        cbTextureSelect.SelectedIndex = bSkeleton.wpModels[cbWeapon.SelectedIndex].Groups[0].texID;
                                    else
                                        if (bSkeleton.textures.Count > 0)
                                            cbTextureSelect.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                SelectedBonePiece = -1;
                                gbSelectedBoneFrame.Enabled = false;
                                gbSelectedPieceFrame.Enabled = false;
                            }
                        }

                        SetTextureEditorFields();
                        break;
                }

                //  gbAnimationOptionsFrame.Enabled = gbSelectedBoneFrame.Enabled;

                SetFrameEditorFields();
                panelModel_Paint(null, null);

                x_last = e.X;
                y_last = e.Y;
            }
        }

        private void panelModel_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbMouseIsDown)
            {
                Point3D p_min = new Point3D();
                Point3D p_max = new Point3D();

                Point3D p_temp;
                Point3D p_temp2;

                float aux_alpha, aux_y, aux_dist;
                bool wasValidQ;

                if (bLoaded && e.Button != MouseButtons.None)
                {
                    if (chkShowGround.Checked)
                    {
                        SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);
                        wasValidQ = !IsCameraUnderGround();
                    }
                    else
                    {
                        wasValidQ = false;
                    }

                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            beta = (beta + e.X - x_last) % 360;
                            aux_alpha = (float)alpha;
                            alpha = (alpha + e.Y - y_last) % 360;
                            SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                            if (wasValidQ && IsCameraUnderGround()) alpha = aux_alpha;

                            break;

                        case MouseButtons.Right:
                            aux_dist = (float)DIST;

                            DIST = DIST + (e.Y - y_last) * diameter / 100;

                            SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                            if (wasValidQ && IsCameraUnderGround()) DIST = aux_dist;

                            break;

                        //case MouseButtons.Left | MouseButtons.Right:
                        case MouseButtons.Middle:

                            switch (modelType)
                            {
                                case K_P_FIELD_MODEL:
                                case K_P_BATTLE_MODEL:
                                case K_P_MAGIC_MODEL:
                                case K_3DS_MODEL:
                                    SetCameraPModel(fPModel, 0, 0, (float)DIST, 0, 0, 0, 1, 1, 1);
                                    break;

                                case K_HRC_SKELETON:
                                    ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[tbCurrentFrameScroll.Value],
                                                            ref p_min, ref p_max);

                                    SetCameraAroundModel(ref p_min, ref p_max, 0, 0, (float)DIST, 0, 0, 0, 1, 1, 1);
                                    break;

                                case K_AA_SKELETON:
                                case K_MAGIC_SKELETON:
                                    ComputeBattleBoundingBox(bSkeleton,
                                                             bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value],
                                                             ref p_min, ref p_max);

                                    SetCameraAroundModel(ref p_min, ref p_max, 0, 0, (float)DIST, 0, 0, 0, 1, 1, 1);
                                    break;
                            }

                            aux_y = panY;

                            p_temp2 = new Point3D();
                            p_temp = new Point3D(e.X, e.Y, GetDepthZ(p_temp2));

                            p_temp = GetUnProjectedCoords(p_temp);

                            panX = panX + p_temp.x;
                            panY = panY + p_temp.y;
                            panZ = panZ + p_temp.z;

                            p_temp.x = x_last;
                            p_temp.y = y_last;
                            p_temp.z = GetDepthZ(p_temp2);
                            p_temp = GetUnProjectedCoords(p_temp);

                            panX = panX - p_temp.x;
                            panY = panY - p_temp.y;
                            panZ = panZ - p_temp.z;

                            SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                            if (wasValidQ && IsCameraUnderGround()) panY = aux_y;

                            break;
                    }

                    x_last = e.X;
                    y_last = e.Y;

                    panelModel_Paint(null, null);

                }
            }
        }

        public void panelModel_MouseWheel(object sender, MouseEventArgs e)
        {
            Point3D p_temp;
            Point3D p_temp2;

            float aux_y;
            float tmpDIST;
            bool wasValidQ;

            if (ActiveForm != this) return;

            if (bLoaded)
            {
                if (chkShowGround.Checked)
                {
                    SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);
                    wasValidQ = !IsCameraUnderGround();
                }
                else wasValidQ = false;

                tmpDIST = (float)DIST;

                if (controlPressedQ)
                    DIST = DIST + (e.Delta * diameter) / 10000;
                else
                    DIST = DIST + (e.Delta * diameter) / 1000;

                SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                if (wasValidQ && IsCameraUnderGround()) DIST = tmpDIST;

                aux_y = panY;

                p_temp2 = new Point3D();
                p_temp = new Point3D(x_last, y_last, GetDepthZ(p_temp2));

                p_temp = GetUnProjectedCoords(p_temp);

                panX = panX + p_temp.x;
                panY = panY + p_temp.y;
                panZ = panZ + p_temp.z;

                p_temp.x = x_last;
                p_temp.y = y_last;
                p_temp.z = GetDepthZ(p_temp2);
                p_temp = GetUnProjectedCoords(p_temp);

                panX = panX - p_temp.x;
                panY = panY - p_temp.y;
                panZ = panZ - p_temp.z;

                SetCameraModelView(panX, panY, (float)(panZ + DIST), (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                if (wasValidQ && IsCameraUnderGround()) panY = aux_y;

                panelModel_Paint(null, null);
            }
        }

        private void panelModel_DoubleClick(object sender, EventArgs e)
        {
            PModel tmpPModel = new PModel();

            if (bLoaded)
            {
                EditedBone = SelectedBone;
                EditedBonePiece = SelectedBonePiece;

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1 && SelectedBonePiece > -1)
                            tmpPModel = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].Model;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        if (SelectedBone > -1 && SelectedBonePiece > -1)
                            tmpPModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        else
                        {
                            if (SelectedBone == bSkeleton.nBones)
                                tmpPModel = bSkeleton.wpModels[ianimWeaponIndex];
                        }                         
                        break;

                    case K_P_BATTLE_MODEL:
                    case K_P_FIELD_MODEL:
                    case K_P_MAGIC_MODEL:
                    case K_3DS_MODEL:
                        tmpPModel = fPModel;
                        break;
                }

                if (tmpPModel.Verts != null && tmpPModel.Verts.Length > 0)
                {
                    // Close previous P Editor if any.
                    if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                    // We will stop Play Animation if it is running
                    if (btnPlayStopAnim.Checked) btnPlayStopAnim.Checked = false;

                    // Change sliders status
                    ChangeGroupBoxesStatusPEditor(false);

                    frmPEdit = new frmPEditor(this, tmpPModel);
                    frmPEdit.Show();
                    frmPEdit.tmrRenderPModel.Interval = 100;
                    frmPEdit.tmrRenderPModel.Start();
                }

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo(frmPEdit, this);

            panelModel_Paint(null, null);
            textureViewer_Paint(null, null);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo(frmPEdit, this);

            panelModel_Paint(null, null);
            textureViewer_Paint(null, null);
        }

        private void resetCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCamera(ref alpha, ref beta, ref gamma, ref panX, ref panY, ref panZ, ref DIST);

            panelModel_Paint(null, null);
            textureViewer_Paint(null, null);
        }

        private void showCharlgpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFieldDatabase.ShowDialog();

            if (frmFieldDB.bSelectedFileFromDB)
            {
                loadSkeletonFromDB();
            }
        }

        private void showBattlelgpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBattleDatabase.ShowDialog();

            if (frmBattleDB.bSelectedBattleFileFromDB)
            {
                loadSkeletonFromBattleDB(frmBattleDB.strBattleFile, frmBattleDB.strBattleAnimFile);
            }
        }

        private void showMagiclgpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMagicDatabase.ShowDialog();

            if (frmMagicDB.bSelectedMagicFileFromDB)
            {
                loadSkeletonFromBattleDB(frmMagicDB.strMagicFile, frmMagicDB.strMagicAnimFile);
            }
        }

        private void loadFieldSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iLoadResult;
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Load Field Skeleton";
            openFile.Filter = "Field Skeleton|*.HRC|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathFieldSkeletonFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathFieldSkeletonFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // Load Field Skeleton
                        iLoadResult = LoadSkeleton(openFile.FileName, true);

                        if (iLoadResult == -2)
                        {
                            MessageBox.Show("Error Destroying Field Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                            "Error");
                            return;
                        }

                        if (iLoadResult == -1)
                        {
                            MessageBox.Show("Error opening Field Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                            "Error");
                            return;
                        }

                        if (iLoadResult == 0)
                        {
                            MessageBox.Show("The file " + Path.GetFileName(openFile.FileName).ToUpper() + " has not any known Field Skeleton format.",
                                            "Warning");
                            return;
                        }

                        // Set Global Paths
                        strGlobalFieldSkeletonName = Path.GetFileName(openFile.FileName).ToUpper();
                        strGlobalPathFieldSkeletonFolder = Path.GetDirectoryName(openFile.FileName);
                        strGlobalPathFieldAnimationFolder = Path.GetDirectoryName(openFile.FileName);

                        // Enable/Make Visible Win Forms Data controls
                        EnableWinFormsDataControls();

                        // ComputeBoundingBoxes
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[0], ref p_min, ref p_max);

                        diameter = ComputeFieldDiameter(fSkeleton);

                        // Set frame values in frame editor groupbox...
                        SetFrameEditorFields();

                        // Set texture values in texture editor groupbox...
                        SetTextureEditorFields();

                        // PostLoadModelPreparations
                        PostLoadModelPreparations(ref p_min, ref p_max);

                        // We can draw the model in panel
                        panelModel_Paint(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Global error opening Field Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
            }
        }

        private void loadBattleMagicSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iLoadResult; //, animIndex;
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Load Battle/Magic Skeleton";
            openFile.Filter = "Battle/Magic Skeleton|*AA;*.D|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            openFile.InitialDirectory = strGlobalPath;

            if (strGlobalPathBattleSkeletonFolder != "")
                openFile.InitialDirectory = strGlobalPathBattleSkeletonFolder;
                
            if (modelType == K_MAGIC_SKELETON)
                if (strGlobalPathMagicSkeletonFolder != "")
                    openFile.InitialDirectory = strGlobalPathMagicSkeletonFolder;

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // Load Field Skeleton
                        iLoadResult = LoadSkeleton(openFile.FileName, true);

                        if (iLoadResult == -2)
                        {
                            MessageBox.Show("Error Destroying Battle Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                            "Error");
                            return;
                        }

                        if (iLoadResult == -1)
                        {
                            MessageBox.Show("Error opening Battle Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                            "Error");
                            return;
                        }

                        if (iLoadResult == 0)
                        {
                            MessageBox.Show("The file " + Path.GetFileName(openFile.FileName).ToUpper() +
                                            " has not any known Battle Skeleton format.",
                                            "Warning");
                            return;
                        }

                        // Set Global Paths
                        if (modelType == K_AA_SKELETON)
                        {
                            strGlobalPathBattleSkeletonFolder = Path.GetDirectoryName(openFile.FileName);
                            strGlobalPathBattleAnimationFolder = strGlobalPathBattleSkeletonFolder;

                            strGlobalBattleSkeletonName = Path.GetFileName(openFile.FileName).ToUpper();
                        }
                        else
                        {
                            strGlobalPathMagicSkeletonFolder = Path.GetDirectoryName(openFile.FileName);
                            strGlobalPathMagicAnimationFolder = strGlobalPathMagicSkeletonFolder;

                            strGlobalMagicSkeletonName = Path.GetFileName(openFile.FileName).ToUpper();
                        }

                        // Update Paths
                        WriteCFGFile();

                        // Enable/Make Visible Win Forms Data controls
                        EnableWinFormsDataControls();

                        // ComputeBoundingBoxes
                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[0], ref p_min, ref p_max);

                        diameter = ComputeBattleDiameter(bSkeleton);

                        // Set frame values in frame editor groupbox...
                        SetFrameEditorFields();

                        // Set texture values in texture editor groupbox...
                        SetTextureEditorFields();

                        // PostLoadModelPreparations
                        PostLoadModelPreparations(ref p_min, ref p_max);

                        // We can draw the model in panel
                        panelModel_Paint(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Global error opening Battle Skeleton file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
            }
        }

        private void loadRSDResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Open RSD Resource";
            openFile.Filter = "FF7 RSD Resource|*.RSD|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathRSDResourceFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathRSDResourceFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName) && Path.GetExtension(openFile.FileName).ToUpper() == ".RSD")
                    {
                        // We load the Model into memory.
                        // But first we destroy the previous loaded Skeleton.
                        if (bLoaded)
                        {
                            if (DestroySkeleton() != 1)
                            {
                                MessageBox.Show("Error Destroying Skeleton file " + openFile.FileName.ToUpper() + ".",
                                                "Error");
                                return;
                            }
                            else
                            {
                                bLoaded = false;
                            }
                        }

                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // Set Global Paths
                        strGlobalPathRSDResourceFolder = Path.GetDirectoryName(openFile.FileName);
                        strGlobalRSDResourceName = Path.GetFileName(openFile.FileName).ToUpper();

                        // Load the RSD Resource
                        // We need to prepare some type of "FAKE" or RSD only fSkeleton
                        LoadRSDResourceModel(strGlobalPathRSDResourceFolder, 
                                             Path.GetFileNameWithoutExtension(strGlobalRSDResourceName));

                        // Enable/Make Visible Win Forms Data controls
                        EnableWinFormsDataControls();

                        // ComputeBoundingBoxes
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[0], ref p_min, ref p_max);
                        diameter = ComputeFieldDiameter(fSkeleton);

                        // Set frame values in frame editor groupbox...
                        SetFrameEditorFields();

                        // Set texture values in texture editor groupbox...
                        SetTextureEditorFields();

                        // PostLoadModelPreparations
                        PostLoadModelPreparations(ref p_min, ref p_max);

                        // We can draw the model in panel
                        panelModel_Paint(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Model file " + openFile.FileName.ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void loadPModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Open Model";
            openFile.Filter = "FF7 Field Model|*.P|FF7 Battle Model (*.*)|*.*|FF7 Magic Model|*.P??|All files|*.*";
            openFile.FilterIndex = 4;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathPModelFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathPModelFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // We load the Model into memory.
                        // But first we destroy the previous loaded Skeleton.
                        if (bLoaded)
                        {
                            if (DestroySkeleton() != 1)
                            {
                                MessageBox.Show("Error Destroying Skeleton file " + openFile.FileName.ToUpper() + ".",
                                                "Error");
                                return;
                            }
                            else
                            {
                                bLoaded = false;
                            }
                        }

                        // Load the Model
                        if (Path.GetExtension(openFile.FileName).ToUpper() != ".TMD")
                        {
                                    // Set Global Paths
                            strGlobalPathPModelFolder = Path.GetDirectoryName(openFile.FileName);
                            strGlobalPModelName = Path.GetFileName(openFile.FileName).ToUpper();

                            fPModel = new PModel();
                            LoadPModel(ref fPModel, strGlobalPathPModelFolder,
                                       Path.GetFileName(strGlobalPModelName));
                        }

                        // Decide the modelType
                        if (fPModel.Header.numVerts > 0)
                        {
                            modelType = GetPModelType(strGlobalPModelName);
                        }

                        // Enable/Make Visible Win Forms Data controls
                        EnableWinFormsDataControls();

                        ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                        diameter = ComputeDiameter(fPModel.BoundingBox);

                        // Set frame values in frame editor groupbox...
                        SetFrameEditorFields();

                        // Set texture values in texture editor groupbox...
                        SetTextureEditorFields();

                        // PostLoadModelPreparations
                        PostLoadModelPreparations(ref p_min, ref p_max);

                        // We can draw the model in panel
                        panelModel_Paint(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening P Model file " + openFile.FileName.ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void load3DSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Load 3DS Model";
            openFile.Filter = "FF7 3DS Model|*.3DS|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPath3DSModelFolder != null)
            {
                openFile.InitialDirectory = strGlobalPath3DSModelFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // We load the Model into memory.
                        // But first we destroy the previous loaded Skeleton.
                        if (bLoaded)
                        {
                            if (DestroySkeleton() != 1)
                            {
                                MessageBox.Show("Error Destroying Skeleton file " + openFile.FileName.ToUpper() + ".",
                                                "Error");
                                return;
                            }
                            else
                            {
                                bLoaded = false;
                            }
                        }
                        // Set Global Paths
                        strGlobalPath3DSModelFolder = Path.GetDirectoryName(openFile.FileName);
                        strGlobal3DSModelName = Path.GetFileName(openFile.FileName).ToUpper();

                        // We load the 3DS model into memory.
                        Model3DS[] model3DS;
                        fPModel = new PModel();

                        Load3DS(openFile.FileName, out model3DS);
                        ConvertModels3DSToPModel(model3DS, ref fPModel);

                        if (fPModel.Header.numVerts > 0)
                        {
                            modelType = K_3DS_MODEL;

                            // Enable/Make Visible Win Forms Data controls
                            EnableWinFormsDataControls();

                            ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                            diameter = ComputeDiameter(fPModel.BoundingBox);

                            // Set frame values in frame editor groupbox...
                            SetFrameEditorFields();

                            // Set texture values in texture editor groupbox...
                            SetTextureEditorFields();

                            // PostLoadModelPreparations
                            PostLoadModelPreparations(ref p_min, ref p_max);

                            // We can draw the model in panel
                            panelModel_Paint(null, null);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error opening .3DS file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void loadTMDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            // Set filter options and filter index.
            openFile.Title = "Open Model";
            openFile.Filter = "FF7 TMD Model|*.TMD|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathTMDModelFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathTMDModelFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Close frmPEditor if opened
                        if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                        // Disable/Make Invisible in Forms Data controls
                        InitializeWinFormsDataControls();

                        // We load the Model into memory.
                        // But first we destroy the previous loaded Skeleton.
                        if (bLoaded)
                        {
                            if (DestroySkeleton() != 1)
                            {
                                MessageBox.Show("Error Destroying Skeleton file " + openFile.FileName.ToUpper() + ".",
                                                "Error");
                                return;
                            }
                            else
                            {
                                bLoaded = false;
                            }
                        }

                        // Load the Model
                        if (Path.GetExtension(openFile.FileName).ToUpper() == ".TMD")
                        {
                            // Set Global Paths
                            strGlobalPathTMDModelFolder = Path.GetDirectoryName(openFile.FileName);
                            strGlobalTMDModelName = Path.GetFileName(openFile.FileName).ToUpper();

                            mTMDModel = new TMDModel();
                            LoadTMDModel(ref mTMDModel, strGlobalPathTMDModelFolder,
                                         Path.GetFileName(strGlobalTMDModelName));

                            if (mTMDModel.TMDObjectList != null && mTMDModel.TMDObjectList.Length > 0)
                            {
                                // We have at least loaded the TMD_MODEL.
                                IsTMDModel = true;

                                // Now let's try to convert the TMD model into P model.
                                fPModel = new PModel();

                                fPModel.fileName = Path.GetFileNameWithoutExtension(strGlobalTMDModelName) + "_" + "001";

                                ConvertTMD2PModel(ref fPModel, mTMDModel, 0);

                                //for (int i = 0; i < 15; i++)
                                //{
                                //    ConvertTMD2PModel(ref fPModel, mTMDModel, i);
                                //}

                                frmTMDOL = new frmTMDObjList(this);
                                frmTMDOL.PopulateTMDObjList();
                                frmTMDOL.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("The selected file is not a .TMD file.", "Information");
                            return;
                        }

                        // Decide the modelType
                        if (fPModel.Header.numVerts > 0)
                        {
                            modelType = GetPModelType(strGlobalTMDModelName);
                        }

                        // Enable/Make Visible Win Forms Data controls
                        EnableWinFormsDataControls();

                        ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                        diameter = ComputeDiameter(fPModel.BoundingBox);

                        // Set frame values in frame editor groupbox...
                        SetFrameEditorFields();

                        // Set texture values in texture editor groupbox...
                        SetTextureEditorFields();

                        // PostLoadModelPreparations
                        PostLoadModelPreparations(ref p_min, ref p_max);

                        // We can draw the model in panel
                        panelModel_Paint(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening TMD file " + openFile.FileName.ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void saveSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string modelTypeStr = "";
            string saveFileName = "";
            int iSaveResult = 0;

            try
            {
                if (bLoaded)
                {
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            if (IsRSDResource)
                            {
                                modelTypeStr = "RSD Resource";

                                saveFileName = strGlobalPathRSDResourceFolder + "\\" + strGlobalRSDResourceName.ToUpper();
                                strGlobalPathSaveSkeletonFolder = strGlobalPathRSDResourceFolder;
                            }
                            else
                            {
                                modelTypeStr = "Field Skeleton";

                                saveFileName = strGlobalPathFieldSkeletonFolder + "\\" + strGlobalFieldSkeletonFileName.ToUpper();
                                strGlobalPathSaveSkeletonFolder = strGlobalPathFieldSkeletonFolder;
                            }

                            break;

                        case K_AA_SKELETON:
                            modelTypeStr = "Battle Skeleton";

                            saveFileName = strGlobalPathBattleSkeletonFolder + "\\" + strGlobalBattleSkeletonFileName.ToUpper();
                            strGlobalPathSaveSkeletonFolder = strGlobalPathBattleSkeletonFolder;
                            break;

                        case K_MAGIC_SKELETON:
                            modelTypeStr = "Magic Skeleton";

                            saveFileName = strGlobalPathMagicSkeletonFolder + "\\" + strGlobalMagicSkeletonFileName.ToUpper();
                            strGlobalPathSaveSkeletonFolder = strGlobalPathMagicSkeletonFolder;
                            break;
                    }

                    // Check if it is RSD Resource.
                    if (IsRSDResource)
                    {
                        // We save the RSD Resource.
                        iSaveResult = WriteFullRSDResource(fSkeleton.bones[0], saveFileName);
                    }
                    else
                    {
                        // We save the Skeleton.
                        iSaveResult = WriteSkeleton(saveFileName);
                    }

                    if (iSaveResult == 1)
                    {
                        MessageBox.Show(modelTypeStr + " " + Path.GetFileName(saveFileName).ToUpper() + " saved.",
                                        "Information");

                        WriteCFGFile();

                        bChangesDone = false;
                        UpdateMainSkeletonWindowTitle();

                        panelModel_Paint(null, null);
                    }

                }
            }
            catch
            {
                MessageBox.Show("Error exception saving " + modelTypeStr + " ... '" + Path.GetFileName(saveFileName) + ".",
                                "Error");
                return;
            }
        }

        private void saveSkeletonAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string modelTypeStr = "";
            //string saveAnimationFileName;
            int iSaveResult;

            // Set filter options and filter index depending on modelType
            // Check Initial Directory
            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (IsRSDResource)
                    {
                        saveFile.Title = "Save RSD Resource As...";
                        saveFile.Filter = "RSD Resource|*.RSD|All files|*.*";

                        if (strGlobalPathRSDResourceFolder == "")
                            strGlobalPathSaveSkeletonFolder = strGlobalPathRSDResourceFolder;
                        saveFile.FileName = strGlobalRSDResourceName.ToUpper();

                        modelTypeStr = "RSD Resource";
                    }
                    else
                    {
                        saveFile.Title = "Save Field Skeleton As...";
                        saveFile.Filter = "Field Skeleton|*.HRC|All files|*.*";

                        if (strGlobalPathSaveSkeletonFolder == "")
                            strGlobalPathSaveSkeletonFolder = strGlobalPathFieldSkeletonFolder;
                        saveFile.FileName = strGlobalFieldSkeletonName.ToUpper();

                        modelTypeStr = "Field Skeleton";
                    }

                    //if (strGlobalFieldAnimationName == "") saveAnimationFileName = "dummy_animation.a";
                    //else saveAnimationFileName = strGlobalFieldAnimationName;
                    break;

                case K_AA_SKELETON:
                    saveFile.Title = "Save Battle Skeleton As...";
                    saveFile.Filter = "Battle Skeleton|*AA|All files|*.*";

                    if (strGlobalPathSaveSkeletonFolder == "")
                        strGlobalPathSaveSkeletonFolder = strGlobalPathBattleSkeletonFolder;
                    saveFile.FileName = strGlobalBattleSkeletonName.ToUpper();

                    modelTypeStr = "Battle Skeleton";
                    //if (strGlobalBattleAnimationName == "")
                    //    saveAnimationFileName = strGlobalBattleSkeletonName[0] +
                    //                            strGlobalBattleSkeletonName[1] +
                    //                            "da";
                    //else saveAnimationFileName = strGlobalBattleAnimationName;
                    break;

                case K_MAGIC_SKELETON:
                    saveFile.Title = "Save Magic Skeleton As...";
                    saveFile.Filter = "Magic Skeleton|*.D|All files|*.*";

                    if (strGlobalPathSaveSkeletonFolder == "")
                        strGlobalPathSaveSkeletonFolder = strGlobalPathMagicSkeletonFolder;
                    saveFile.FileName = strGlobalMagicSkeletonName.ToUpper();

                    modelTypeStr = "Magic Skeleton";
                    //if (strGlobalMagicAnimationName == "")
                    //    saveAnimationFileName = Path.GetFileNameWithoutExtension(strGlobalMagicSkeletonName) + ".a00";
                    //else saveAnimationFileName = strGlobalMagicAnimationName;
                    break;

                case K_P_FIELD_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    saveFile.Title = "Save Model As...";
                    saveFile.Filter = "Field Model|*.P|Battle Model|*.*|Magic Model|*.P??|All files|*.*";

                    if (strGlobalPathSaveModelFolder == "")
                        strGlobalPathSaveModelFolder = strGlobalPathPModelFolder;

                    if (modelType == K_3DS_MODEL) saveFile.FileName =
                                Path.GetFileNameWithoutExtension(strGlobal3DSModelName).ToUpper();
                    else saveFile.FileName = strGlobalPModelName.ToUpper();

                    modelTypeStr = "Model";
                    //if (strGlobalMagicAnimationName == "")
                    //    saveAnimationFileName = Path.GetFileNameWithoutExtension(strGlobalMagicSkeletonName) + ".a00";
                    //else saveAnimationFileName = strGlobalMagicAnimationName;
                    break;
            }

            saveFile.FilterIndex = 1;

            if (modelTypeStr == "Model") saveFile.InitialDirectory = strGlobalPathSaveModelFolder;
            else saveFile.InitialDirectory = strGlobalPathSaveSkeletonFolder;

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (bLoaded)
                    {
                        // I don't think it is needed when saving
                        //AddStateToBuffer(this);

                        strGlobalPathSaveSkeletonFolder = Path.GetDirectoryName(saveFile.FileName);
                        saveFile.FileName = strGlobalPathSaveSkeletonFolder + "\\" + Path.GetFileName(saveFile.FileName).ToUpper();

                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                // We have a different process for RSD Resource
                                if (IsRSDResource)
                                {
                                    // We save the RSD Resource.
                                    iSaveResult = WriteFullRSDResource(fSkeleton.bones[0], saveFile.FileName);
                                }
                                else
                                {
                                    // We save the Skeleton.
                                    iSaveResult = WriteSkeleton(saveFile.FileName);
                                }

                                if (iSaveResult == 1)
                                {
                                    MessageBox.Show(modelTypeStr + " " + Path.GetFileName(saveFile.FileName).ToUpper() + " saved.",
                                                    "Information");
                                }
                                else
                                {
                                    MessageBox.Show("Error saving " + modelTypeStr + " " + Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                                    "Error");
                                    return;
                                }
                                break;

                            case K_P_FIELD_MODEL:
                            case K_P_BATTLE_MODEL:
                            case K_P_MAGIC_MODEL:
                            case K_3DS_MODEL:
                                // We save the Model.
                                iSaveResult = WritePModel(saveFile.FileName);

                                if (iSaveResult == 1)
                                {
                                    MessageBox.Show("Model " + Path.GetFileName(saveFile.FileName).ToUpper() + " saved.",
                                                    "Information");
                                }
                                break;
                        }
                    }

                    WriteCFGFile();

                    bChangesDone = false;
                    UpdateMainSkeletonWindowTitle();

                    panelModel_Paint(null, null);
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving " + modelTypeStr + " as... " + Path.GetFileName(saveFile.FileName) + ".",
                                "Error");
                return;
            }
        }

        private void loadFieldAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldAnimation tmpfAnimation = new FieldAnimation();

            // Set filter options and filter index.
            openFile.Title = "Open Field Animation";
            openFile.Filter = "Field Animation|*.A|All files|*.*";
            openFile.FilterIndex = 1;
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathFieldAnimationFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathFieldAnimationFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Set Global Paths
                        strGlobalFieldAnimationName = Path.GetFileName(openFile.FileName).ToUpper();
                        strGlobalPathFieldAnimationFolder = Path.GetDirectoryName(openFile.FileName);

                        if (!SameFieldAnimNumBones(openFile.FileName, fSkeleton) &&
                            strGlobalFieldAnimationName != "BZBC.A")
                        {
                            MessageBox.Show("The Animation file " + Path.GetFileName(openFile.FileName).ToUpper() +
                                            " and the loaded skeleton have different number of bones. Animation not loaded.",
                                            "Error");

                            return;
                        }

                        // Load the Field Animation
                        tmpfAnimation = fAnimation;
                        fAnimation = new FieldAnimation(fSkeleton, openFile.FileName.ToUpper(), true);

                        // Let's stop the Animation
                        btnPlayStopAnim.Checked = false;

                        iCurrentFrameScroll = 0;
                        tbCurrentFrameScroll.Value = 0;
                        txtAnimationFrame.Text = iCurrentFrameScroll.ToString();

                        tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                        SetFrameEditorFields();

                        UpdateMainSkeletonWindowTitle();

                        WriteCFGFile();
                    }

                    panelModel_Paint(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Field Animation file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");

                if (tmpfAnimation.nBones > 0) fAnimation = tmpfAnimation;
                return;
            }
        }

        private void loadBattleMagicLimitAnimationsStripMenuItem_Click(object sender, EventArgs e)
        {
            int bi;
            BattleAnimationsPack tmpbAnimationsPack = new BattleAnimationsPack();

            // Set filter options and filter index.
            openFile.Title = "Open Battle/Magic/Limit Animation";

            switch (modelType)
            {
                case K_AA_SKELETON:
                    // We will check if the model loaded can have Limit Breaks.
                    openFile.Filter = "Battle/Limit Animation|*DA;";

                    if (bSkeleton.CanHaveLimitBreak)
                    {
                        //openFile.Filter = openFile.Filter + "|Limit Animation|";

                        STLimitsRegister lstLimits = lstBattleLimitsAnimations.Find(x => x.lstModelNames.Contains(strGlobalBattleSkeletonName));

                        foreach (string itmLimitBrk in lstLimits.lstLimitsAnimations)
                            openFile.Filter = openFile.Filter + itmLimitBrk.ToString() + ";";
                    }

                    openFile.Filter = openFile.Filter + "|All files|*.*";
                    break;

                case K_MAGIC_SKELETON:
                    openFile.Filter = "Magic Animation|*.A00|All files|*.*";
                    break;
            }

            openFile.FilterIndex = 1;

            // Check Initial Directory
            if (strGlobalPathBattleAnimationFolder != null)
            {
                if (modelType == K_AA_SKELETON)
                {
                    openFile.InitialDirectory = strGlobalPathBattleAnimationFolder;
                    openFile.FileName = strGlobalBattleAnimationName;
                }
                else
                {
                    openFile.InitialDirectory = strGlobalPathMagicAnimationFolder;
                    openFile.FileName = strGlobalMagicAnimationName;
                }
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        if (!SameBattleAnimNumBones(openFile.FileName, bSkeleton))
                        {
                            MessageBox.Show("The Animations Pack file " + Path.GetFileName(openFile.FileName).ToUpper() +
                                            " and the loaded skeleton have different number of bones. Animation not loaded.",
                                            "Error");
                            return;
                        }

                        // Load the Battle Animations Pack
                        tmpbAnimationsPack = bAnimationsPack;  // let's save the anim temporary
                        bAnimationsPack = new BattleAnimationsPack(bSkeleton, openFile.FileName.ToUpper());

                        // Set Global Paths
                        if (modelType == K_AA_SKELETON)
                        {
                            strGlobalBattleAnimationName = Path.GetFileName(openFile.FileName).ToUpper();
                            strGlobalPathBattleAnimationFolder = Path.GetDirectoryName(openFile.FileName);
                        }
                        else
                        {
                            strGlobalMagicAnimationName = Path.GetFileName(openFile.FileName).ToUpper();
                            strGlobalPathMagicAnimationFolder = Path.GetDirectoryName(openFile.FileName);
                        }

                        UpdateMainSkeletonWindowTitle();

                        // Let's stop the Animation
                        btnPlayStopAnim.Checked = false;                      

                        // Let's initialize some things like Battle Animations ComboBox
                        if (modelType == K_MAGIC_SKELETON) lblBattleAnimation.Text = "Magic Animation:";
                        else if (bAnimationsPack.IsLimit) lblBattleAnimation.Text = "Limit Animation:";
                        else lblBattleAnimation.Text = "Battle Animation:";

                        cbBattleAnimation.Items.Clear();
                        for (bi = 0; bi < bAnimationsPack.nbSkeletonAnims; bi++)
                        {
                            if (bAnimationsPack.SkeletonAnimations[bi].numFramesShort > 0)
                            {
                                cbBattleAnimation.Items.Add(bi.ToString());
                            }
                        }

                        cbBattleAnimation.SelectedIndex = 0;
                        ianimIndex = 0;

                        iCurrentFrameScroll = 0;
                        tbCurrentFrameScroll.Value = 0;
                        txtAnimationFrame.Text = iCurrentFrameScroll.ToString();
                        tbCurrentFrameScroll.Maximum = bAnimationsPack.SkeletonAnimations[0].numFramesShort - 1;

                        SetFrameEditorFields();

                        WriteCFGFile();
                    }

                    panelModel_Paint(null, null);
                }
            }
            catch
            {
                MessageBox.Show("Error opening Animation file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");

                if (tmpbAnimationsPack.nAnimations > 0) bAnimationsPack = tmpbAnimationsPack;
                return;
            }
        }

        private void saveAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string modelTypeStr = "";
            string saveFileName = "";
            int iSaveResult = 0;

            try
            {
                if (bLoaded)
                {
                    // Prepare direct filename Folder+Name
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            modelTypeStr = "Field Animation";
                            if (strGlobalFieldAnimationName == "") strGlobalFieldAnimationName = "DUMMY.A";

                            saveFileName = strGlobalPathFieldAnimationFolder + "\\" + strGlobalFieldAnimationName.ToUpper();
                            break;

                        case K_AA_SKELETON:
                            modelTypeStr = "Battle Animation";

                            saveFileName = strGlobalPathBattleAnimationFolder + "\\" + strGlobalBattleAnimationName.ToUpper();
                            break;

                        case K_MAGIC_SKELETON:
                            modelTypeStr = "Magic Animation";

                            saveFileName = strGlobalPathMagicAnimationFolder + "\\" + strGlobalMagicAnimationName.ToUpper();
                            break;
                    }

                    // We save the Animation
                    iSaveResult = WriteAnimation(saveFileName);

                    if (iSaveResult == 1)
                    {
                        MessageBox.Show(modelTypeStr + " " + Path.GetFileName(saveFileName).ToUpper() + " saved.",
                                        "Information");

                        WriteCFGFile();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving " + modelTypeStr + "... " + Path.GetFileName(saveFileName) + ".",
                                "Error");
                return;
            }
        }

        private void saveAnimationAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string modelTypeStr = "";
            int iSaveResult;

            // Set filter options and filter index depending on modelType
            // Check Initial Directory
            switch (modelType)
            {
                case K_HRC_SKELETON:
                    modelTypeStr = "Field Animation";
                    saveFile.Title = "Save Field Animation As...";
                    saveFile.Filter = "Field Animation|*.A|All files|*.*";

                    if (strGlobalPathSaveAnimationFolder == "") strGlobalPathSaveAnimationFolder = strGlobalPathFieldAnimationFolder;
                    saveFile.FileName = strGlobalFieldAnimationName.ToUpper();
                    break;

                case K_AA_SKELETON:
                    modelTypeStr = "Battle Animation";

                    if (bAnimationsPack.IsLimit)
                    {
                        saveFile.Title = "Save Limit Pack Animation As...";
                        saveFile.Filter = "Limit Pack Animation|*.A00|All files|*.*";
                    }
                    else
                    {
                        saveFile.Title = "Save Battle Pack Animation As...";
                        saveFile.Filter = "Battle Pack Animation|*DA|All files|*.*";
                    }

                    if (strGlobalPathSaveAnimationFolder == "") strGlobalPathSaveAnimationFolder = strGlobalPathBattleAnimationFolder;
                    saveFile.FileName = strGlobalBattleAnimationName.ToUpper();
                    break;

                case K_MAGIC_SKELETON:
                    modelTypeStr = "Magic Animation";
                    saveFile.Title = "Save Magic Animation As...";
                    saveFile.Filter = "Magic Pack Animation|*.A00|All files|*.*";

                    if (strGlobalPathSaveAnimationFolder == "") strGlobalPathSaveAnimationFolder = strGlobalPathMagicAnimationFolder;
                    saveFile.FileName = strGlobalMagicAnimationName.ToUpper();
                    break;
            }

            saveFile.FilterIndex = 1;
            saveFile.InitialDirectory = strGlobalPathSaveAnimationFolder;

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // We save the Animation
                    iSaveResult = WriteAnimation(saveFile.FileName);

                    if (iSaveResult == 1)
                    {
                        MessageBox.Show(modelTypeStr + " " + Path.GetFileName(saveFile.FileName).ToUpper() + " saved.",
                                        "Information");

                        strGlobalPathSaveAnimationFolder = Path.GetDirectoryName(saveFile.FileName);

                        WriteCFGFile();
                    }
                    else if (iSaveResult == -1)
                    {
                        MessageBox.Show("It has been some problem when saving " + modelTypeStr + " file " + 
                                        Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                        "Error");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving " + modelTypeStr + " file " + 
                                Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void outputFramesDataAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iSaveResult;

            // Set filter options and filter index depending on modelType
            saveFile.Title = "Output Frame Data to TXT";
            saveFile.Filter = "Frame Data|*.txt|All files|*.*";

            // Check Initial Directory
            saveFile.FileName = Path.GetFileNameWithoutExtension(strGlobalFieldAnimationName) + ".TXT";
            saveFile.FilterIndex = 1;
            saveFile.InitialDirectory = strGlobalPathSaveAnimationFolder;

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // We save the Frames Data
                    iSaveResult = WriteFrameData(saveFile.FileName);

                    if (iSaveResult == 1)
                    {
                        MessageBox.Show("Frame Data of animation " + strGlobalFieldAnimationName.ToUpper() + " has been saved.",
                                        "Information");
                    }
                    else if (iSaveResult == -1)
                    {
                        MessageBox.Show("It has been some problem while saving the Frame Data to file " +
                                        Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                        "Error");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving the Frame Data to file " +
                                Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        public void SetTextureEditorFields()
        {
            int ti;

            cbTextureSelect.Items.Clear();
            cbTextureSelect.Text = string.Empty;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (SelectedBone > -1 && SelectedBonePiece > -1)
                    {
                        gbTexturesFrame.Enabled = true;

                        for (ti = 0; ti < fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].numTextures; ti++)
                        {
                            cbTextureSelect.Items.Add(fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[ti].TEXfileName);
                        }

                        if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].numTextures > 0)
                            cbTextureSelect.SelectedIndex = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].Model.Groups[0].texID;
                        else cbTextureSelect.SelectedIndex = -1;
                    }
                    else gbTexturesFrame.Enabled = false;

                    textureViewer_Paint(null, null);
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    if (SelectedBone > -1 && SelectedBonePiece > -1)
                    {
                        gbTexturesFrame.Enabled = true;

                        for (ti = 0; ti < bSkeleton.nTextures; ti++)
                        {
                            cbTextureSelect.Items.Add(ti);
                        }

                        if (bSkeleton.nTextures > 0) 
                        {
                            // Find the first group with texture flag enabled.
                            int iGroupIdx = 0;
                            bool bFound = false;

                            while (iGroupIdx < bSkeleton.bones[SelectedBone].Models[SelectedBonePiece].Groups.Length &&
                                   !bFound)
                            {
                                if (bSkeleton.bones[SelectedBone].Models[SelectedBonePiece].Groups[iGroupIdx].texFlag == 0)
                                    iGroupIdx++;
                                else bFound = true;
                            }

                            if (bSkeleton.bones[SelectedBone].Models[SelectedBonePiece].Groups.Length == iGroupIdx)
                            {
                                cbTextureSelect.SelectedIndex = -1;
                            }
                            else
                            {
                                cbTextureSelect.SelectedIndex =
                                    bSkeleton.bones[SelectedBone].Models[SelectedBonePiece].Groups[iGroupIdx].texID;
                            }
                        
                        }
                    }
                    else gbTexturesFrame.Enabled = false;

                    textureViewer_Paint(null, null);

                    break;
            }
        }

        private void cbTextureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            textureViewer_Paint(null, null);
        }

        private void chkShowGround_CheckedChanged(object sender, EventArgs e)
        {
            bShowGround = chkShowGround.Checked;

            panelModel_Paint(null, null);
        }

        private void chkShowLastFrameGhost_CheckedChanged(object sender, EventArgs e)
        {
            bShowLastFrameGhost = chkShowLastFrameGhost.Checked;

            panelModel_Paint(null, null);
        }

        private void btnAddTexture_Click(object sender, EventArgs e)
        {
            FieldBone tmpfBone;
            FieldRSDResource tmpfResource;

            TEX tex = new TEX();

            try
            {
                // Set filter options and filter index.
                openFile.Title = "Add Texture";

                openFile.Filter = "Any Image file|*.bmp;*.jpg;*.gif;*.png;*.ico;*.rle;*.wmf;*.emf|TEX texture|*.TEX;*AC;*AD;*AE;*AF;*AG;*AH;*AI;*AJ;AK*;AL*;*.T??|All files|*.*";

                openFile.FilterIndex = 1;
                openFile.FileName = null;

                // Check Initial Directory
                if (strGlobalPathTextureFolder != null)
                {
                    openFile.InitialDirectory = strGlobalPathTextureFolder;
                }
                else
                {
                    openFile.InitialDirectory = strGlobalPath;
                }

                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Set Global Paths
                        strGlobalTextureName = Path.GetFileName(openFile.FileName).ToUpper();
                        strGlobalPathTextureFolder = Path.GetDirectoryName(openFile.FileName);

                        LoadImageAsTEXTexture(openFile.FileName, ref tex);

                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                if (SelectedBone > -1 && SelectedBonePiece > -1)
                                {
                                    AddStateToBuffer(this);

                                    tmpfBone = fSkeleton.bones[SelectedBone];
                                    tmpfResource = tmpfBone.fRSDResources[SelectedBonePiece];

                                    if (tmpfResource.numTextures == 0) tmpfResource.textures = new List<TEX>();

                                    tmpfResource.textures.Add(tex);
                                    tmpfResource.numTextures++;

                                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpfResource;
                                    fSkeleton.bones[SelectedBone] = tmpfBone;

                                    SetTextureEditorFields();
                                    cbTextureSelect.SelectedIndex = tmpfResource.numTextures - 1;
                                }
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                if (bSkeleton.nTextures <= 10)
                                {
                                    AddStateToBuffer(this);

                                    bSkeleton.nTextures++;

                                    tex.TEXfileName = GetBattleModelTextureFilename(bSkeleton, bSkeleton.nTextures - 1);

                                    bSkeleton.textures.Add(tex);
                                    Array.Resize(ref bSkeleton.TexIDS, bSkeleton.nTextures);
                                    bSkeleton.TexIDS[bSkeleton.nTextures - 1] = tex.texID;
                                    SetTextureEditorFields();
                                    cbTextureSelect.SelectedIndex = bSkeleton.nTextures - 1;
                                }
                                else
                                {
                                    MessageBox.Show("The maximum number of textures for battle models is 10.", "Error", MessageBoxButtons.OK);
                                }
                                break;
                        }

                        // Update main title window
                        bChangesDone = true;
                        UpdateMainSkeletonWindowTitle();
                    }

                    panelModel_Paint(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding texture file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void btnRemoveTexture_Click(object sender, EventArgs e)
        {
            int ti, texIndex;
            FieldRSDResource tmpfResource;

            if ((texIndex = cbTextureSelect.SelectedIndex) == -1)
            {
                MessageBox.Show("There are no textures to remove.", "Information");
                return;
            }

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                        {
                            AddStateToBuffer(this);

                            tmpfResource = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece];
                            tmpfResource.numTextures--;
                            fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece] = tmpfResource;

                            fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures.RemoveAt(texIndex);

                            SetTextureEditorFields();
                        }

                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        AddStateToBuffer(this);

                        bSkeleton.textures.RemoveAt(texIndex);

                        //  This is dirty, but will prevent problems with the undo/redo
                        //  UnloadTexture .textures(tex_index)
                        for (ti = texIndex; ti < bSkeleton.nTextures - 2; ti++)
                        {
                            bSkeleton.TexIDS[ti] = bSkeleton.TexIDS[ti + 1];
                        }

                        bSkeleton.nTextures--;

                        if (bSkeleton.nTextures == 0) bSkeleton.TexIDS[0] = 0;
                        SetTextureEditorFields();

                        break;
                }

                // Update main title window
                bChangesDone = true;
                UpdateMainSkeletonWindowTitle();

                panelModel_Paint(null, null);
            }
            catch
            {
                MessageBox.Show("Error removing texture file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void btnChangeTexture_Click(object sender, EventArgs e)
        {
            FieldBone tmpfBone;
            FieldRSDResource tmpfRSDResource;

            TEX tex = new TEX();
            int texIndex;

            if ((texIndex = cbTextureSelect.SelectedIndex) == -1)
            {
                MessageBox.Show("There are no textures to change.", "Information");
                return;
            }

            try
            {
                // Set filter options and filter index.
                openFile.Title = "Change Texture";

                openFile.Filter = "Any Image file|*.bmp;*.jpg;*.gif;*.png;*.ico;*.rle;*.wmf;*.emf|TEX texture|*.TEX;*AC;*AD;*AE;*AF;*AG;*AH;*AI;*AJ;AK*;AL*;*.T??|All files|*.*";

                openFile.FilterIndex = 1;
                openFile.FileName = null;

                // Check Initial Directory
                if (strGlobalPathTextureFolder != null)
                {
                    openFile.InitialDirectory = strGlobalPathTextureFolder;
                }
                else
                {
                    openFile.InitialDirectory = strGlobalPath;
                }

                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Set Global Paths
                        strGlobalTextureName = Path.GetFileName(openFile.FileName).ToUpper();
                        strGlobalPathTextureFolder = Path.GetDirectoryName(openFile.FileName);

                        LoadImageAsTEXTexture(openFile.FileName, ref tex);

                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                AddStateToBuffer(this);

                                tmpfBone = fSkeleton.bones[SelectedBone];
                                tmpfRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                                tmpfRSDResource.textures[texIndex] = tex;

                                tmpfBone.fRSDResources[SelectedBonePiece] = tmpfRSDResource;
                                fSkeleton.bones[SelectedBone] = tmpfBone;

                                SetTextureEditorFields();
                                cbTextureSelect.SelectedIndex = texIndex;
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                AddStateToBuffer(this);

                                tex.TEXfileName = GetBattleModelTextureFilename(bSkeleton, texIndex);

                                bSkeleton.textures[texIndex] = tex;
                                bSkeleton.TexIDS[texIndex] = tex.texID;

                                SetTextureEditorFields();
                                cbTextureSelect.SelectedIndex = texIndex;
                                break;
                        }

                        WriteCFGFile();

                        // Update main title window
                        bChangesDone = true;
                        UpdateMainSkeletonWindowTitle();
                    }
                }

                panelModel_Paint(null, null);
            }
            catch
            {
                MessageBox.Show("Error changing texture file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void chkFrontLight_CheckedChanged(object sender, EventArgs e)
        {
            bchkFrontLight = chkFrontLight.Checked;
            panelModel_Paint(null, null);
        }

        private void chkRearLight_CheckedChanged(object sender, EventArgs e)
        {
            bchkRearLight = chkRearLight.Checked;
            panelModel_Paint(null, null);
        }

        private void chkRight_CheckedChanged(object sender, EventArgs e)
        {
            bchkRightLight = chkRightLight.Checked;
            panelModel_Paint(null, null);
        }

        private void hsbLightPosX_ValueChanged(object sender, EventArgs e)
        {
            fLightPosXScroll = hsbLightPosX.Value;
            panelModel_Paint(null, null);
        }

        private void hsbLightPosY_ValueChanged(object sender, EventArgs e)
        {
            fLightPosYScroll = hsbLightPosY.Value;
            panelModel_Paint(null, null);
        }

        private void hsbLightPosZ_ValueChanged(object sender, EventArgs e)
        {
            fLightPosZScroll = hsbLightPosZ.Value;
            panelModel_Paint(null, null);
        }

        private void btnComputeGroundHeight_Click(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            FieldFrame tmpfFrame;
            BattleFrame tmpbFrame;

            //int animIndex, fi;
            int fi;
            float maxDiff;

            AddStateToBuffer(this);

            maxDiff = (float)INFINITY_SINGLE;

            switch (modelType)
            {
                case K_HRC_SKELETON:

                    for (fi = 0; fi < fAnimation.nFrames; fi++)
                    {
                        tmpfFrame = fAnimation.frames[fi];
                        ComputeFieldBoundingBox(fSkeleton, tmpfFrame, ref p_min, ref p_max);
                        fAnimation.frames[fi] = tmpfFrame;

                        if (maxDiff > p_max.y) maxDiff = p_max.y;
                    }

                    for (fi = 0; fi < fAnimation.nFrames; fi++)
                    {
                        tmpfFrame = fAnimation.frames[fi];
                        tmpfFrame.rootTranslationY = tmpfFrame.rootTranslationY + maxDiff;
                        fAnimation.frames[fi] = tmpfFrame;
                    }
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    for (fi = 0; fi < bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort; fi++)
                    {
                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi], ref p_min, ref p_max);

                        if (maxDiff > p_max.y) maxDiff = p_max.y;
                    }

                    if (maxDiff != 0)
                    {
                        for (fi = 0; fi < bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort; fi++)
                        {
                            tmpbFrame = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi];
                            tmpbFrame.startY = tmpbFrame.startY - (int)maxDiff;
                            bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi] = tmpbFrame;
                        }

                        //  Also don't forget the weapon frames if available
                        if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.wpModels.Count > 0)
                        {
                            for (fi = 0; fi < bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort; fi++)
                            {
                                tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                tmpbFrame.startY = tmpbFrame.startY - (int)maxDiff;
                                bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                            }
                        }
                    }
                    break;
            }

            panelModel_Paint(null, null);
            SetFrameEditorFields();
        }

        private void hsbResizePieceX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.resizeX = hsbResizePieceX.Value / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;
                    //int wpIndex;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        //wpIndex = getBattleWeaponIndex();
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.resizeX = hsbResizePieceX.Value / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.resizeX = hsbResizePieceX.Value / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.resizeX = hsbResizePieceX.Value / 100f;
                    break;
            }

            txtResizePieceX.Text = hsbResizePieceX.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbResizePieceY_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.resizeY = hsbResizePieceY.Value / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.resizeY = hsbResizePieceY.Value / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.resizeY = hsbResizePieceY.Value / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.resizeY = hsbResizePieceY.Value / 100f;
                    break;
            }

            txtResizePieceY.Text = hsbResizePieceY.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbResizePieceZ_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.resizeZ = hsbResizePieceZ.Value / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.resizeZ = hsbResizePieceZ.Value / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.resizeZ = hsbResizePieceZ.Value / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.resizeZ = hsbResizePieceZ.Value / 100f;
                    break;
            }

            txtResizePieceZ.Text = hsbResizePieceZ.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbRepositionX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.repositionX = hsbRepositionX.Value * ComputeDiameter(tmpRSDResource.Model.BoundingBox) / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.repositionX = hsbRepositionX.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.repositionX = hsbRepositionX.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.repositionX = hsbRepositionX.Value * ComputeDiameter(fPModel.BoundingBox) / 100f;
                    break;
            }

            txtRepositionX.Text = hsbRepositionX.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbRepositionY_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.repositionY = hsbRepositionY.Value * ComputeDiameter(tmpRSDResource.Model.BoundingBox) / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.repositionY = hsbRepositionY.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.repositionY = hsbRepositionY.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.repositionY = hsbRepositionY.Value * ComputeDiameter(fPModel.BoundingBox) / 100f;
                    break;
            }

            txtRepositionY.Text = hsbRepositionY.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbRepositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;
                    FieldRSDResource tmpRSDResource;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    tmpRSDResource.Model.repositionZ = hsbRepositionZ.Value * ComputeDiameter(tmpRSDResource.Model.BoundingBox) / 100f;

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[ianimWeaponIndex];
                        wpModel.repositionZ = hsbRepositionZ.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.wpModels[ianimWeaponIndex] = wpModel;
                    }
                    else
                    {
                        wpModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        wpModel.repositionZ = hsbRepositionZ.Value * ComputeDiameter(wpModel.BoundingBox) / 100f;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = wpModel;
                    }
                    break;

                default:
                    fPModel.repositionZ = hsbRepositionZ.Value * ComputeDiameter(fPModel.BoundingBox) / 100f;
                    break;
            }

            txtRepositionZ.Text = hsbRepositionZ.Value.ToString();
            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void hsbRotateAlpha_ValueChanged(object sender, EventArgs e)
        {
            PieceRotationModifiersChanged();
        }

        private void hsbRotateBeta_ValueChanged(object sender, EventArgs e)
        {
            PieceRotationModifiersChanged();
        }

        private void hsbRotateGamma_ValueChanged(object sender, EventArgs e)
        {
            PieceRotationModifiersChanged();
        }

        private void chkLeftLight_CheckedChanged(object sender, EventArgs e)
        {
            bchkLeftLight = chkLeftLight.Checked;
            panelModel_Paint(null, null);
        }

        private void chkZeroAsTransparent_Click(object sender, EventArgs e)
        {
            TEX tmpTEX;

            int newZeroTransparentValue;

            newZeroTransparentValue = chkColorKeyFlag.Checked ? 1 : 0;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (SelectedBone > -1 && SelectedBonePiece > -1)
                    {
                        if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].
                                            textures[cbTextureSelect.SelectedIndex].texID != 0xFFFFFFFF)
                        {
                            if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].
                                                textures[cbTextureSelect.SelectedIndex].ColorKeyFlag != newZeroTransparentValue)
                            {
                                tmpTEX = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex];

                                UnloadTexture(ref tmpTEX);
                                LoadTEXTexture(ref tmpTEX);
                                LoadBitmapFromTEXTexture(ref tmpTEX);

                                fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex] = tmpTEX;
                            }
                        }
                    }

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    if (bSkeleton.textures[cbTextureSelect.SelectedIndex].texID != 0xFFFFFFFF)
                    {
                        if (bSkeleton.textures[cbTextureSelect.SelectedIndex].ColorKeyFlag != newZeroTransparentValue)
                        {
                            tmpTEX = bSkeleton.textures[cbTextureSelect.SelectedIndex];

                            UnloadTexture(ref tmpTEX);
                            LoadTEXTexture(ref tmpTEX);
                            LoadBitmapFromTEXTexture(ref tmpTEX);

                            bSkeleton.textures[cbTextureSelect.SelectedIndex] = tmpTEX;
                        }
                    }
                    break;
            }

            panelModel_Paint(null, null);
        }

        private void txtResizePieceX_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iResizePieceX;

            if (Int32.TryParse(txtResizePieceX.Text, out iResizePieceX))
            {
                if (iResizePieceX >= 0 && iResizePieceX <= 400)
                    hsbResizePieceX.Value = iResizePieceX;
                else
                    txtResizePieceX.Text = "100";
            }
            else
                txtResizePieceX.Text = "100";
        }

        private void txtResizePieceY_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iResizePieceY;

            if (Int32.TryParse(txtResizePieceY.Text, out iResizePieceY))
            {
                if (iResizePieceY >= 0 && iResizePieceY <= 400)
                    hsbResizePieceY.Value = iResizePieceY;
                else
                    txtResizePieceY.Text = "100";
            }
            else
                txtResizePieceY.Text = "100";
        }

        private void txtResizePieceZ_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iResizePieceZ;

            if (Int32.TryParse(txtResizePieceZ.Text, out iResizePieceZ))
            {
                if (iResizePieceZ >= 0 && iResizePieceZ <= 400)
                    hsbResizePieceZ.Value = iResizePieceZ;
                else
                    txtResizePieceZ.Text = "100";
            }
            else
                txtResizePieceZ.Text = "100";
        }

        private void txtRepositionX_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRepositionX;

            if (Int32.TryParse(txtRepositionX.Text, out iRepositionX))
            {
                if (iRepositionX >= -500 && iRepositionX <= 500)
                    hsbRepositionX.Value = iRepositionX;
                else
                    txtRepositionX.Text = "0";
            }
            else txtRepositionX.Text = "0";
        }

        private void txtRepositionY_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRepositionY;

            if (Int32.TryParse(txtRepositionY.Text, out iRepositionY))
            {
                if (iRepositionY >= -500 && iRepositionY <= 500)
                    hsbRepositionY.Value = iRepositionY;
                else
                    txtRepositionY.Text = "0";
            }
            else txtRepositionY.Text = "0";
        }

        private void txtRepositionZ_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRepositionZ;

            if (Int32.TryParse(txtRepositionZ.Text, out iRepositionZ))
            {
                if (iRepositionZ >= -500 && iRepositionZ <= 500)
                    hsbRepositionZ.Value = iRepositionZ;
                else
                    txtRepositionZ.Text = "0";
            }
            else txtRepositionZ.Text = "0";
        }

        private void txtRotateAlpha_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRotateAlpha;

            if (Int32.TryParse(txtRotateAlpha.Text, out iRotateAlpha))
            {
                if (iRotateAlpha >= 0 && iRotateAlpha <= 360)
                    hsbRotateAlpha.Value = iRotateAlpha;
                else
                    txtRotateAlpha.Text = "360";
            }
            else txtRotateAlpha.Text = "0";
        }

        private void txtRotateBeta_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRotateBeta;

            if (Int32.TryParse(txtRotateBeta.Text, out iRotateBeta))
            {
                if (iRotateBeta >= 0 && iRotateBeta <= 360)
                    hsbRotateBeta.Value = iRotateBeta;
                else
                    txtRotateBeta.Text = "360";
            }
            else txtRotateBeta.Text = "0";
        }

        private void txtRotateGamma_TextChanged(object sender, EventArgs e)
        {
            if (loadingBonePieceModifiersQ || SelectedBonePiece == -1) return;

            int iRotateGamma;

            if (Int32.TryParse(txtRotateGamma.Text, out iRotateGamma))
            {
                if (iRotateGamma >= 0 && iRotateGamma <= 360)
                    hsbRotateGamma.Value = iRotateGamma;
                else
                    txtRotateGamma.Text = "360";
            }
            else txtRotateGamma.Text = "0";
        }

        private void nUDResizeBoneX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBoneModifiersQ || SelectedBone == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpfBone.resizeX = (float)nUDResizeBoneX.Value / 100;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;
                    BattleBone bBone;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[cbWeapon.SelectedIndex];
                        wpModel.resizeX = (float)nUDResizeBoneX.Value / 100f;
                        bSkeleton.wpModels[cbWeapon.SelectedIndex] = wpModel;
                    }
                    else
                    {
                        bBone = bSkeleton.bones[SelectedBone];
                        bBone.resizeX = (float)nUDResizeBoneX.Value / 100f;
                        bSkeleton.bones[SelectedBone] = bBone;
                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void nUDResizeBoneY_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBoneModifiersQ || SelectedBone == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpfBone.resizeY = (float)nUDResizeBoneY.Value / 100;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;
                    BattleBone bBone;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[cbWeapon.SelectedIndex];
                        wpModel.resizeY = (float)nUDResizeBoneY.Value / 100f;
                        bSkeleton.wpModels[cbWeapon.SelectedIndex] = wpModel;
                    }
                    else
                    {
                        bBone = bSkeleton.bones[SelectedBone];
                        bBone.resizeY = (float)nUDResizeBoneY.Value / 100f;
                        bSkeleton.bones[SelectedBone] = bBone;
                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void nUDResizeBoneZ_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBoneModifiersQ || SelectedBone == -1) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpfBone.resizeZ = (float)nUDResizeBoneZ.Value / 100;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel wpModel;
                    BattleBone bBone;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        wpModel = bSkeleton.wpModels[cbWeapon.SelectedIndex];
                        wpModel.resizeZ = (float)nUDResizeBoneZ.Value / 100f;
                        bSkeleton.wpModels[cbWeapon.SelectedIndex] = wpModel;
                    }
                    else
                    {
                        bBone = bSkeleton.bones[SelectedBone];
                        bBone.resizeZ = (float)nUDResizeBoneZ.Value / 100f;
                        bSkeleton.bones[SelectedBone] = bBone;
                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void nUDBoneLength_ValueChanged(object sender, EventArgs e)
        {
            if (loadingBoneModifiersQ) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone;

                    tmpfBone = fSkeleton.bones[SelectedBone];
                    tmpfBone.len = (float)nUDBoneOptionsLength.Value / 10000;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    txtBoneOptionsLength.Text = tmpfBone.len.ToString("F7");
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    BattleBone bBone;

                    bBone = bSkeleton.bones[SelectedBone];
                    bBone.len = (float)nUDBoneOptionsLength.Value / 10000;
                    bSkeleton.bones[SelectedBone] = bBone;
                    txtBoneOptionsLength.Text = bBone.len.ToString("F7");
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void txtBoneLength_TextChanged(object sender, EventArgs e)
        {
            if (loadingBoneModifiersQ) return;

            float fBoneLength;

            if (float.TryParse(txtBoneOptionsLength.Text, out fBoneLength))
            {
                nUDBoneOptionsLength.Value = (decimal)fBoneLength * 10000;
            }
        }

        private void btnAddPiece_Click(object sender, EventArgs e)
        {
            Model3DS[] models3DS_auxV;
            PModel AdditionalP;
            int iResult;

            if (modelType != K_HRC_SKELETON && modelType != K_AA_SKELETON && modelType != K_MAGIC_SKELETON)
            {
                // MessageBox.Show("This should not happen.", "Error");
                return;
            }

            // Set filter options and filter index.
            openFile.Title = "Add Piece";
            openFile.Filter = "FF7 Field Part file|*.P|FF7 Battle Part file|*.*|FF7 Magic Part file|*.P*|3D Studio model|*.3DS";

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    openFile.FilterIndex = 1;
                    break;

                case K_AA_SKELETON:
                    openFile.FilterIndex = 2;
                    break;

                case K_MAGIC_SKELETON:
                    openFile.FilterIndex = 3;
                    break;
            }
            
            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathPartModelFolder != null)
            {
                openFile.InitialDirectory = strGlobalPathPartModelFolder;
            }
            else
            {
                openFile.InitialDirectory = strGlobalPath;
            }

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFile.FileName))
                    {
                        // Set Global Paths
                        strGlobalPartModelName = Path.GetFileName(openFile.FileName).ToUpper();
                        strGlobalPathPartModelFolder = Path.GetDirectoryName(openFile.FileName);

                        AdditionalP = new PModel();

                        if (Path.GetExtension(openFile.FileName).ToUpper() == ".3DS")
                        {
                            iResult = Load3DS(openFile.FileName, out models3DS_auxV);

                            if (iResult == 1)
                                ConvertModels3DSToPModel(models3DS_auxV, ref AdditionalP);
                        }
                        else
                        {
                            LoadPModel(ref AdditionalP, strGlobalPathPartModelFolder, strGlobalPartModelName);
                        }

                        if (AdditionalP.Header.numVerts > 0)
                        {
                            AddStateToBuffer(this);

                            if (modelType == K_HRC_SKELETON)
                            {
                                FieldBone tmpfBone = fSkeleton.bones[SelectedBone];
                                AddFieldBone(ref tmpfBone, ref AdditionalP);
                                fSkeleton.bones[SelectedBone] = tmpfBone;
                            }
                            else
                            {
                                BattleBone tmpbBone = bSkeleton.bones[SelectedBone];
                                AddBattleBoneModel(ref tmpbBone, ref AdditionalP);
                                bSkeleton.bones[SelectedBone] = tmpbBone;
                            }

                            SelectedBonePiece++;
                        }

                        SetTextureEditorFields();
                        panelModel_Paint(null, null);
                        WriteCFGFile();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Global error Adding Piece file " + Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
            }
        }

        private void btnRemovePiece_Click(object sender, EventArgs e)
        {
            try
            {
                if (modelType == K_HRC_SKELETON)
                {
                    if (fSkeleton.bones[SelectedBone].nResources > 0)
                    {
                        AddStateToBuffer(this);

                        FieldBone tmpfBone = fSkeleton.bones[SelectedBone];
                        RemoveFieldBone(ref tmpfBone, ref SelectedBonePiece);
                        fSkeleton.bones[SelectedBone] = tmpfBone;
                    }
                }
                else
                {
                    if (bSkeleton.bones[SelectedBone].nModels > 0)
                    {
                        AddStateToBuffer(this);

                        BattleBone tmpbBone = bSkeleton.bones[SelectedBone];
                        RemoveBattleBoneModel(ref tmpbBone, ref SelectedBonePiece);
                        bSkeleton.bones[SelectedBone] = tmpbBone;
                    }
                }

                SelectedBonePiece = -1;
                gbSelectedPieceFrame.Enabled = false;
                
                SetBoneModifiers();
                SetTextureEditorFields();
                panelModel_Paint(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Global error Removing Piece.", "Error");
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            TEX tex = new TEX();
            int texIndex;

            texIndex = cbTextureSelect.SelectedIndex;

            if (texIndex > -1)
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                            tex = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex];

                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        if (SelectedBone > -1)
                            tex = bSkeleton.textures[texIndex];
                        break;
                }

                int row, col, BPPStride, i;
                int newWidth, newHeight, originalWidth, originalHeight, newWidthMinusOne; //, newHeight, originalWidth, originalHeight;
                int originalWidthStride, originalHeightStride;
                int destinationX, destinationY, destinationPosition, sourcePosition;
                byte[] result;

                BPPStride = tex.bytesPerPixel;

                result = new byte[tex.width * tex.height * BPPStride];

                newWidth = tex.height;
                newHeight = tex.width;

                originalWidth = tex.width;
                originalHeight = tex.height;
                originalWidthStride = originalWidth * BPPStride;
                originalHeightStride = originalHeight * BPPStride;

                // We're going to use the new width and height minus one a lot so lets 
                // pre-calculate that once to save some more time
                newWidthMinusOne = newWidth - 1;

                for (row = 0; row < originalHeightStride; row += BPPStride)
                {
                    destinationX = (newWidthMinusOne * BPPStride) - row;

                    for (col = 0; col < originalWidthStride; col += BPPStride)
                    {
                        sourcePosition = (col + row * originalWidth);
                        destinationY = col;
                        destinationPosition = (destinationX + destinationY * newWidth);

                        for (i = 0; i < BPPStride; i++)
                        {
                            result[destinationPosition + i] = tex.pixelData[sourcePosition + i];
                        }
                    }
                }

                tex.pixelData = result;
                tex.width = newWidth;
                tex.height = newHeight;


                //  Let's update all the textures used in other Bones
                UnloadTexture(ref tex);
                LoadTEXTexture(ref tex);
                LoadBitmapFromTEXTexture(ref tex);

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                        {
                            int r, t;

                            //for (i = 0; i < fSkeleton.nBones; i++)
                            for (i = 0; i < fSkeleton.bones.Count; i++)
                            {
                                for (r = 0; r < fSkeleton.bones[i].nResources; r++)
                                {
                                    for (t = 0; t < fSkeleton.bones[i].fRSDResources[r].numTextures; t++)
                                    {
                                        if (fSkeleton.bones[i].fRSDResources[r].textures[t].TEXfileName == tex.TEXfileName)
                                        {
                                            fSkeleton.bones[i].fRSDResources[r].textures[t] = tex;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:

                        for (i = 0; i < bSkeleton.nTextures; i++)
                        {
                            if (bSkeleton.textures[i].TEXfileName == tex.TEXfileName)
                            {
                                bSkeleton.textures[i] = tex;
                            }
                        }
                        break;
                }

                SetTextureEditorFields();
                cbTextureSelect.SelectedIndex = texIndex;

                panelModel_Paint(null, null);
            }
        }

        private void btnFlipVertical_Click(object sender, EventArgs e)
        {
            TEX tex = new TEX();
            int texIndex;

            texIndex = cbTextureSelect.SelectedIndex;

            if (texIndex > -1)
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                            tex = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex];
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:

                        if (SelectedBone > -1)
                            tex = bSkeleton.textures[texIndex];
                        break;
                }

                int row, col, BPPStride, i;
                int current, flipped;
                byte[] result;

                //current = 0;
                //flipped = 0;
                BPPStride = tex.bytesPerPixel;

                result = new byte[tex.width * tex.height * BPPStride];

                int widthStride, heightStride;
                widthStride = tex.width * BPPStride;
                heightStride = tex.height * BPPStride;

                for (row = 0; row < heightStride; row += BPPStride)
                {
                    for (col = 0; col < widthStride; col += BPPStride)
                    {
                        current = (row * tex.width) + col;
                        flipped = (row * tex.width) + (widthStride - col - BPPStride);

                        for (i = 0; i < BPPStride; i++)
                        {
                            result[flipped + i] = tex.pixelData[current + i];
                        }
                    }
                }

                tex.pixelData = result;

                // Let's refresh this TEXTexture in the rest of P Models
                UnloadTexture(ref tex);
                LoadTEXTexture(ref tex);
                LoadBitmapFromTEXTexture(ref tex);

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                        {
                            int r, t;

                            //for (i = 0; i < fSkeleton.nBones; i++)
                            for (i = 0; i < fSkeleton.bones.Count; i++)
                            {
                                for (r = 0; r < fSkeleton.bones[i].nResources; r++)
                                {
                                    for (t = 0; t < fSkeleton.bones[i].fRSDResources[r].numTextures; t++)
                                    {
                                        if (fSkeleton.bones[i].fRSDResources[r].textures[t].TEXfileName == tex.TEXfileName)
                                        {
                                            fSkeleton.bones[i].fRSDResources[r].textures[t] = tex;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:

                        for (i = 0; i < bSkeleton.nTextures; i++)
                        {
                            if (bSkeleton.textures[i].TEXfileName == tex.TEXfileName)
                            {
                                bSkeleton.textures[i] = tex;
                            }
                        }
                        break;
                }

                SetTextureEditorFields();
                cbTextureSelect.SelectedIndex = texIndex;

                panelModel_Paint(null, null);
            }
        }

        private void btnFlipHorizontal_Click(object sender, EventArgs e)
        {
            TEX tex = new TEX();
            int texIndex;

            texIndex = cbTextureSelect.SelectedIndex;

            if (texIndex > -1)
            {

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                            tex = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex];
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:

                        if (SelectedBone > -1)
                            tex = bSkeleton.textures[texIndex];
                        break;
                }

                int row, col, BPPStride, i;
                int current, flipped;
                byte[] result;
                int widthStride, heightStride, widheiTotal;

                BPPStride = tex.bytesPerPixel;

                widheiTotal = tex.width * tex.height * BPPStride;
                result = new byte[widheiTotal];

                widthStride = tex.width * BPPStride;
                heightStride = tex.height * BPPStride;

                for (row = 0; row < tex.height; row++)
                {
                    for (col = 0; col < widthStride; col += BPPStride)
                    {
                        current = (row * widthStride) + col;
                        flipped = widheiTotal - (widthStride * (row + 1)) + col;

                        for (i = 0; i < BPPStride; i++)
                        {
                            result[flipped + i] = tex.pixelData[current + i];
                        }
                    }
                }

                tex.pixelData = result;

                // Let's refresh this TEXTexture in the rest of P Models
                UnloadTexture(ref tex);
                LoadTEXTexture(ref tex);
                LoadBitmapFromTEXTexture(ref tex);

                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (SelectedBone > -1)
                        {
                            int r, t;

                            //for (i = 0; i < fSkeleton.nBones; i++)
                            for (i = 0; i < fSkeleton.bones.Count; i++)
                            {
                                for (r = 0; r < fSkeleton.bones[i].nResources; r++)
                                {
                                    for (t = 0; t < fSkeleton.bones[i].fRSDResources[r].numTextures; t++)
                                    {
                                        if (fSkeleton.bones[i].fRSDResources[r].textures[t].TEXfileName == tex.TEXfileName)
                                        {
                                            fSkeleton.bones[i].fRSDResources[r].textures[t] = tex;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:

                        for (i = 0; i < bSkeleton.nTextures; i++)
                        {
                            if (bSkeleton.textures[i].TEXfileName == tex.TEXfileName)
                            {
                                bSkeleton.textures[i] = tex;
                            }
                        }
                        break;
                }

                //result = null;

                SetTextureEditorFields();
                cbTextureSelect.SelectedIndex = texIndex;

                panelModel_Paint(null, null);
            }
        }

        private void nUDMoveTextureUpDown_ValueChanged(object sender, EventArgs e)
        {
            // Ok. We will use a NumericalUpDown control as UpDown VB6 control.
            if (nUDTexUpDown != nUDMoveTextureUpDown.Value)
            {
                int texIndex = 0;
                uint tmpTexID;  // = 0;
                TEX tmpTEX; // = new TEX();

                if (nUDTexUpDown > nUDMoveTextureUpDown.Value)
                {
                    // Down
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            if (SelectedBone > -1)
                            {
                                if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].numTextures > 0)
                                    texIndex = cbTextureSelect.SelectedIndex;

                                if (texIndex < fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].numTextures - 1)
                                {
                                    tmpTEX = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex];

                                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex] =
                                            fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex + 1];

                                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex + 1] = tmpTEX;

                                    SetTextureEditorFields();
                                    cbTextureSelect.SelectedIndex = texIndex + 1;
                                }
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:

                            texIndex = cbTextureSelect.SelectedIndex;

                            if (texIndex < bSkeleton.nTextures - 1)
                            {
                                tmpTEX = bSkeleton.textures[texIndex];
                                bSkeleton.textures[texIndex] = bSkeleton.textures[texIndex + 1];
                                bSkeleton.textures[texIndex + 1] = tmpTEX;

                                tmpTexID = bSkeleton.TexIDS[texIndex];
                                bSkeleton.TexIDS[texIndex] = bSkeleton.TexIDS[texIndex + 1];
                                bSkeleton.TexIDS[texIndex + 1] = tmpTexID;

                                SetTextureEditorFields();
                                cbTextureSelect.SelectedIndex = texIndex + 1;
                            }
                            break;
                    }
                }
                else
                {
                    // Up
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            if (SelectedBone > -1)
                            {
                                if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].numTextures > 0)
                                    texIndex = cbTextureSelect.SelectedIndex;

                                if (texIndex > 0)
                                {
                                    tmpTEX = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex];

                                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex] =
                                            fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex - 1];

                                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[texIndex - 1] = tmpTEX;

                                    SetTextureEditorFields();
                                    cbTextureSelect.SelectedIndex = texIndex - 1;
                                }
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:

                            texIndex = cbTextureSelect.SelectedIndex;

                            if (texIndex > 0)
                            {
                                tmpTEX = bSkeleton.textures[texIndex];
                                bSkeleton.textures[texIndex] = bSkeleton.textures[texIndex - 1];
                                bSkeleton.textures[texIndex - 1] = tmpTEX;

                                tmpTexID = bSkeleton.TexIDS[texIndex];
                                bSkeleton.TexIDS[texIndex] = bSkeleton.TexIDS[texIndex - 1];
                                bSkeleton.TexIDS[texIndex - 1] = tmpTexID;

                                SetTextureEditorFields();
                                cbTextureSelect.SelectedIndex = texIndex - 1;
                            }
                            break;
                    }
                }

                panelModel_Paint(null, null);
                nUDTexUpDown = (int)nUDMoveTextureUpDown.Value;
            }
        }

        private void nUDFrameDataPart_ValueChanged(object sender, EventArgs e)
        {
            switch (Math.Abs(nUDFrameDataPart.Value % 3))
            {
                case K_FRAME_BONE_ROTATION:
                    gbFrameDataPartOptions.Text = "Bone rotation";
                    break;

                case K_FRAME_ROOT_ROTATION:
                    gbFrameDataPartOptions.Text = "Root rotation";
                    break;

                case K_FRAME_ROOT_TRANSLATION:
                    gbFrameDataPartOptions.Text = "Root translation";
                    break;
            }

            SetFrameEditorFields();
        }

        private void nUDXAnimationFramePart_ValueChanged(object sender, EventArgs e)
        {

            if (loadingAnimationQ) return;

            FieldRotation tmpfRotation;
            FieldFrame tmpfFrame;
            BattleFrameBone tmpbFrameBone;
            BattleFrame tmpbFrame;

            int frameIndex, nFrames, fi;
            float val, diff;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            val = (float)nUDXAnimationFramePart.Value;

            frameIndex = iCurrentFrameScroll;
            nFrames = frameIndex;

            //  Must propagate the changes to the following frames?
            if (chkPropagateChangesForward.Checked) nFrames = tbCurrentFrameScroll.Maximum;

            switch (Math.Abs(nUDFrameDataPart.Value % 3))
            {
                case K_FRAME_BONE_ROTATION:
                    if (SelectedBone > -1)
                    {
                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                diff = val - fAnimation.frames[frameIndex].rotations[SelectedBone].alpha;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpfRotation = fAnimation.frames[fi].rotations[SelectedBone];
                                    tmpfRotation.alpha += diff;
                                    fAnimation.frames[fi].rotations[SelectedBone] = tmpfRotation;
                                }
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                if (SelectedBone == bSkeleton.nBones)
                                {
                                    diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].bones[0].alpha;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0];
                                        tmpbFrameBone.alpha += diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                                    }
                                }
                                else
                                {
                                    diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[SelectedBone + 1].alpha;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1];
                                        tmpbFrameBone.alpha += diff;
                                        bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1] = tmpbFrameBone;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case K_FRAME_ROOT_ROTATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootRotationAlpha;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootRotationAlpha += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[0].alpha;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0];
                                tmpbFrameBone.alpha += diff;
                                bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                            }
                            break;
                    }
                    break;

                case K_FRAME_ROOT_TRANSLATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootTranslationX;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootTranslationX += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            if (SelectedBone == bSkeleton.nBones)
                            {
                                diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].startX;

                                for (fi = frameIndex; fi <= nFrames; fi++) {
                                    tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startX += (int)diff;
                                    bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }
                            }
                            else
                            {
                                diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].startX;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpbFrame = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startX += (int)diff;
                                    bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }

                                if (bSkeleton.wpModels.Count > 0)
                                {
                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                        tmpbFrame.startX += (int)diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                    }
                                }
                            }
                            break;

                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void nUDYAnimationFramePart_ValueChanged(object sender, EventArgs e)
        {
            if (loadingAnimationQ) return;

            FieldRotation tmpfRotation;
            FieldFrame tmpfFrame;
            BattleFrameBone tmpbFrameBone;
            BattleFrame tmpbFrame;

            int frameIndex, nFrames, fi;
            float val, diff;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            val = (float)nUDYAnimationFramePart.Value;

            frameIndex = iCurrentFrameScroll;
            nFrames = frameIndex;

            //  Must propagate the changes to the following frames?
            if (chkPropagateChangesForward.Checked) nFrames = tbCurrentFrameScroll.Maximum;

            switch (Math.Abs(nUDFrameDataPart.Value % 3))
            {
                case K_FRAME_BONE_ROTATION:
                    if (SelectedBone > -1)
                    {
                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                diff = val - fAnimation.frames[frameIndex].rotations[SelectedBone].beta;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpfRotation = fAnimation.frames[fi].rotations[SelectedBone];
                                    tmpfRotation.beta += diff;
                                    fAnimation.frames[fi].rotations[SelectedBone] = tmpfRotation;
                                }
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:
                                if (SelectedBone == bSkeleton.nBones)
                                {
                                    diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].bones[0].beta;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0];
                                        tmpbFrameBone.beta += diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                                    }
                                }
                                else
                                {
                                    diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[SelectedBone + 1].beta;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1];
                                        tmpbFrameBone.beta += diff;
                                        bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1] = tmpbFrameBone;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case K_FRAME_ROOT_ROTATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootRotationBeta;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootRotationBeta += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[0].beta;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0];
                                tmpbFrameBone.beta += diff;
                                bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                            }
                            break;
                    }
                    break;

                case K_FRAME_ROOT_TRANSLATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootTranslationY;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootTranslationY += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:
                            if (SelectedBone == bSkeleton.nBones)
                            {
                                diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].startY;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startY += (int)diff;
                                    bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }
                            }
                            else
                            {
                                diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].startY;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpbFrame = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startY += (int)diff;
                                    bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }

                                if (bSkeleton.wpModels.Count > 0)
                                {
                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                        tmpbFrame.startY += (int)diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                    }
                                }
                            }
                            break;

                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void nUDZAnimationFramePart_ValueChanged(object sender, EventArgs e)
        {
            if (loadingAnimationQ) return;

            FieldRotation tmpfRotation;
            FieldFrame tmpfFrame;
            BattleFrameBone tmpbFrameBone;
            BattleFrame tmpbFrame;

            int frameIndex, nFrames, fi;
            float val, diff;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            val = (float)nUDZAnimationFramePart.Value;

            frameIndex = iCurrentFrameScroll;
            nFrames = frameIndex;

            //  Must propagate the changes to the following frames?
            if (chkPropagateChangesForward.Checked) nFrames = tbCurrentFrameScroll.Maximum;

            switch (Math.Abs(nUDFrameDataPart.Value % 3))
            {
                case K_FRAME_BONE_ROTATION:
                    if (SelectedBone > -1)
                    {
                        switch (modelType)
                        {
                            case K_HRC_SKELETON:
                                diff = val - fAnimation.frames[frameIndex].rotations[SelectedBone].gamma;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpfRotation = fAnimation.frames[fi].rotations[SelectedBone];
                                    tmpfRotation.gamma += diff;
                                    fAnimation.frames[fi].rotations[SelectedBone] = tmpfRotation;
                                }
                                break;

                            case K_AA_SKELETON:
                            case K_MAGIC_SKELETON:

                                if (SelectedBone == bSkeleton.nBones)
                                {
                                    diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].bones[0].gamma;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0];
                                        tmpbFrameBone.gamma += diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                                    }
                                }
                                else
                                {
                                    diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[SelectedBone + 1].gamma;

                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1];
                                        tmpbFrameBone.gamma += diff;
                                        bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[SelectedBone + 1] = tmpbFrameBone;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case K_FRAME_ROOT_ROTATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootRotationGamma;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootRotationGamma += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:

                            diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].bones[0].gamma;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0];
                                tmpbFrameBone.gamma += diff;
                                bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi].bones[0] = tmpbFrameBone;
                            }
                            break;
                    }
                    break;

                case K_FRAME_ROOT_TRANSLATION:
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            diff = val - fAnimation.frames[frameIndex].rootTranslationZ;

                            for (fi = frameIndex; fi <= nFrames; fi++)
                            {
                                tmpfFrame = fAnimation.frames[fi];
                                tmpfFrame.rootTranslationZ += diff;
                                fAnimation.frames[fi] = tmpfFrame;
                            }
                            break;

                        case K_AA_SKELETON:
                        case K_MAGIC_SKELETON:

                            if (SelectedBone == bSkeleton.nBones)
                            {
                                diff = val - bAnimationsPack.WeaponAnimations[ianimIndex].frames[frameIndex].startZ;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startZ += (int)diff;
                                    bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }
                            }
                            else
                            {
                                diff = val - bAnimationsPack.SkeletonAnimations[ianimIndex].frames[frameIndex].startZ;

                                for (fi = frameIndex; fi <= nFrames; fi++)
                                {
                                    tmpbFrame = bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi];
                                    tmpbFrame.startZ += (int)diff;
                                    bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                }

                                if (bSkeleton.wpModels.Count > 0)
                                {
                                    for (fi = frameIndex; fi <= nFrames; fi++)
                                    {
                                        tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                                        tmpbFrame.startZ += (int)diff;
                                        bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;
                                    }
                                }
                            }
                            break;

                    }
                    break;
            }

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        private void btnRemoveFrame_Click(object sender, EventArgs e)
        {
            AddStateToBuffer(this);

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (fAnimation.nFrames > 1)
                    {
                        fAnimation.frames.RemoveAt(tbCurrentFrameScroll.Value);

                        if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Maximum)
                            tbCurrentFrameScroll.Value = tbCurrentFrameScroll.Value - 1;

                        fAnimation.nFrames--;
                        tbCurrentFrameScroll.Maximum = tbCurrentFrameScroll.Maximum - 1;
                    }
                    else
                    {
                        MessageBox.Show("Frame of Field Animation not removed (the animation needs at least 1 frame).", "Information");
                    }
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    BattleAnimation tmpbAnimation;
                    float primarySecondaryCountersCoef;

                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ianimIndex];

                    if (tmpbAnimation.numFramesShort > 1)
                    {
                        //for (fi = tbCurrentFrameScroll.Value; fi < tmpbAnimation.numFramesShort - 1; fi++)
                        //    tmpbAnimation.frames[fi] = tmpbAnimation.frames[fi + 1];
                        tmpbAnimation.frames.RemoveAt(tbCurrentFrameScroll.Value);

                        primarySecondaryCountersCoef = tmpbAnimation.numFrames / tmpbAnimation.numFramesShort;
                        tmpbAnimation.numFramesShort--;
                        tmpbAnimation.numFrames = (int)(tmpbAnimation.numFramesShort * primarySecondaryCountersCoef);
                        bAnimationsPack.SkeletonAnimations[ianimIndex] = tmpbAnimation;

                        //  Also don't forget the weapon frames if available
                        if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.wpModels.Count > 0)
                        {
                            tmpbAnimation = bAnimationsPack.WeaponAnimations[ianimIndex];
                            //for (fi = tbCurrentFrameScroll.Value; fi < tmpbAnimation.numFramesShort - 1; fi++)
                            //    tmpbAnimation.frames[fi] = tmpbAnimation.frames[fi + 1];
                            tmpbAnimation.frames.RemoveAt(tbCurrentFrameScroll.Value);

                            tmpbAnimation.numFramesShort = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort;
                            tmpbAnimation.numFrames = bAnimationsPack.SkeletonAnimations[ianimIndex].numFrames;
                            bAnimationsPack.WeaponAnimations[ianimIndex] = tmpbAnimation;
                        }

                        if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Maximum) tbCurrentFrameScroll.Value--;
                        tbCurrentFrameScroll.Maximum--;
                    }
                    else
                    {
                        MessageBox.Show("Frame of Battle Animation not removed (the animation needs at least 1 frame).", "Information");
                    }

                    break;
            }

            panelModel_Paint(null, null);
        }

        private void btnDuplicateFrame_Click(object sender, EventArgs e)
        {
            AddStateToBuffer(this);

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    fAnimation.nFrames += 1;

                    fAnimation.frames.Insert(tbCurrentFrameScroll.Value,
                                             CopyfFrame(fAnimation.frames[tbCurrentFrameScroll.Value]));

                    tbCurrentFrameScroll.Maximum += 1;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    BattleAnimation tmpbAnimation;
                    float primarySecondaryCountersCoef;

                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ianimIndex];

                    //  Numframes1 and NumFrames2 are usually different.
                    //  Don't know if this is relevant at all, but keep the balance between them just in case
                    primarySecondaryCountersCoef = tmpbAnimation.numFrames / tmpbAnimation.numFramesShort;
                    tmpbAnimation.numFramesShort++;
                    tmpbAnimation.numFrames = (int)(tmpbAnimation.numFramesShort * primarySecondaryCountersCoef);

                    tmpbAnimation.frames.Insert(tbCurrentFrameScroll.Value,
                                                CopybFrame(bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value]));

                    bAnimationsPack.SkeletonAnimations[ianimIndex] = tmpbAnimation;

                    //  Also don't forget about the weapon frames(where available)
                    if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.wpModels.Count > 0)
                    {
                        tmpbAnimation = bAnimationsPack.WeaponAnimations[ianimIndex];
                        tmpbAnimation.numFramesShort = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort;
                        tmpbAnimation.numFrames = bAnimationsPack.SkeletonAnimations[ianimIndex].numFrames;

                        tmpbAnimation.frames.Insert(tbCurrentFrameScroll.Value,
                                                    CopybFrame(bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value]));

                        bAnimationsPack.WeaponAnimations[ianimIndex] = tmpbAnimation;
                    }

                    tbCurrentFrameScroll.Maximum++;
                    break;
            }

            panelModel_Paint(null, null);
        }

        private void btnInterpolateFrame_Click(object sender, EventArgs e)
        {
            //int animIndex, fi, currentFrame, nextFrame, nFrames, numInterpolatedFrames, i;
            int fi, currentFrame, nextFrame, nFrames, numInterpolatedFrames, i;
            string numInterpolatedFramesStr;
            float alpha, primarySecondaryCountersCoef;


            // Set interpolated frames value readed from Kimera.cfg or updated previously
            if (modelType == K_HRC_SKELETON)
                numInterpolatedFramesStr = idefaultFieldInterpFrames.ToString();
            else
                numInterpolatedFramesStr = idefaultBattleInterpFrames.ToString();


            // Ask for interpolated frames value
            if (InputBox("Animation interpolation",
                         "Number of frames to interpolate between each frame:",
                         ref numInterpolatedFramesStr) == DialogResult.OK)
            {
                if (!Int32.TryParse(numInterpolatedFramesStr, out numInterpolatedFrames))
                {
                    MessageBox.Show("The value entered is not valid.", "Error");
                    return;
                }
            }
            else return;
            

            // Update interpolated frames value in Kimera.cfg file
            if (modelType == K_HRC_SKELETON)
            {
                if (idefaultFieldInterpFrames != numInterpolatedFrames)
                {
                    idefaultFieldInterpFrames = numInterpolatedFrames;
                    WriteCFGFile();
                }
            }
            else
            {
                if (idefaultBattleInterpFrames != numInterpolatedFrames)
                {
                    idefaultBattleInterpFrames = numInterpolatedFrames;
                    WriteCFGFile();
                }
            }
                

            // Interpolate frames processing
            currentFrame = tbCurrentFrameScroll.Value;
            nextFrame = currentFrame + numInterpolatedFrames + 1;

            if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Maximum) nextFrame = 0;

            AddStateToBuffer(this);

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    //  Create new frames
                    FieldFrame tmpfFrame = new FieldFrame();

                    fAnimation.nFrames = fAnimation.nFrames + numInterpolatedFrames;
                    for (i = 0; i < numInterpolatedFrames; i++) fAnimation.frames.Add(tmpfFrame);

                    // Move the original frames into their new positions
                    for (fi = fAnimation.nFrames - 1; fi >= currentFrame + numInterpolatedFrames; fi--)
                        fAnimation.frames[fi] = fAnimation.frames[fi - numInterpolatedFrames];

                    // Interpolate the new frames
                    for (fi = 1; fi <= numInterpolatedFrames; fi++)
                    {
                        alpha = (float)fi / (numInterpolatedFrames + 1);

                        GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[currentFrame], fAnimation.frames[nextFrame],
                                                       alpha, ref tmpfFrame);
                        fAnimation.frames[currentFrame + fi] = tmpfFrame;
                    }

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    BattleAnimation tmpbAnimation;
                    BattleFrame tmpbFrame = new BattleFrame();

                    //bAnimationsPack.SkeletonAnimations[ianimIndex]
                    primarySecondaryCountersCoef = bAnimationsPack.SkeletonAnimations[ianimIndex].numFrames /
                                                   bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort;

                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ianimIndex];

                    // Create new frames
                    tmpbAnimation.numFramesShort += (ushort)numInterpolatedFrames;
                    tmpbAnimation.numFrames = (int)(tmpbAnimation.numFramesShort * primarySecondaryCountersCoef);
                    for (i = 0; i < numInterpolatedFrames; i++) tmpbAnimation.frames.Add(tmpbFrame);

                    // Move the original frames into their new positions
                    for (fi = tmpbAnimation.numFramesShort - 1; fi >= currentFrame + numInterpolatedFrames + 1; fi--)
                        tmpbAnimation.frames[fi] = tmpbAnimation.frames[fi - numInterpolatedFrames];

                    // Interpolate the new frames
                    for (fi = 1; fi <= numInterpolatedFrames; fi++)
                    {
                        alpha = (float)fi / (numInterpolatedFrames + 1);

                        GetTwoBattleFramesInterpolation(bSkeleton, tmpbAnimation.frames[currentFrame], tmpbAnimation.frames[nextFrame],
                                                       alpha, ref tmpbFrame);
                        tmpbAnimation.frames[currentFrame + fi] = tmpbFrame;
                    }

                    // Ok, commit the struct
                    bAnimationsPack.SkeletonAnimations[ianimIndex] = tmpbAnimation;

                    //  Also don't forget about the weapon frames(where available)
                    if (bSkeleton.wpModels.Count > 0)
                    {
                        nFrames = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort;

                        tmpbAnimation = bAnimationsPack.WeaponAnimations[ianimIndex];

                        //  Create new frames
                        tmpbAnimation.numFramesShort += (ushort)numInterpolatedFrames;
                        tmpbAnimation.numFrames = (int)(tmpbAnimation.numFramesShort * primarySecondaryCountersCoef);
                        for (i = 0; i < numInterpolatedFrames; i++) tmpbAnimation.frames.Add(tmpbFrame);

                        // Move the original frames into their new positions
                        for (fi = tmpbAnimation.numFramesShort - 1; fi >= currentFrame + numInterpolatedFrames + 1; fi--)
                            tmpbAnimation.frames[fi] = tmpbAnimation.frames[fi - numInterpolatedFrames];

                        // Interpolate the new frames
                        for (fi = 1; fi <= numInterpolatedFrames; fi++)
                        {
                            alpha = (float)fi / (numInterpolatedFrames + 1);

                            GetTwoBattleFramesWeaponInterpolation(tmpbAnimation.frames[currentFrame], tmpbAnimation.frames[nextFrame],
                                                                  alpha, ref tmpbFrame);
                            tmpbAnimation.frames[currentFrame + fi] = tmpbFrame;
                        }

                        // Ok, commit the struct
                        bAnimationsPack.WeaponAnimations[ianimIndex] = tmpbAnimation;
                    }

                    break;
            }

            tbCurrentFrameScroll.Maximum = tbCurrentFrameScroll.Maximum + numInterpolatedFrames;
            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void chkShowBones_CheckedChanged(object sender, EventArgs e)
        {
            bShowBones = chkShowBones.Checked;
            panelModel_Paint(null, null);
        }

        private void btnInterpolateAnimation_Click(object sender, EventArgs e)
        {
            int fi, ifi, frameOffset, nFrames, numInterpolatedFrames, nextElemDiff, i;
            int baseFinalFrame;
            string numInterpolatedFramesStr;
            float alpha;

            bool bisLoopQ;

            //  Check if number of frames > 1
            if (NumAnimFramesIsOne())
            {
                MessageBox.Show("Can't interpolate animations with a single frame.", "Interpolation warning", MessageBoxButtons.OK);
                return;
            }

            // Ask for interpolated frames
            if (modelType == K_HRC_SKELETON)
                numInterpolatedFramesStr = idefaultFieldInterpFrames.ToString();
            else
                numInterpolatedFramesStr = idefaultBattleInterpFrames.ToString();

            if (InputBox("Animation interpolation",
                         "Number of frames to interpolate between each frame:",
                         ref numInterpolatedFramesStr) == DialogResult.OK)
            {
                if (!Int32.TryParse(numInterpolatedFramesStr, out numInterpolatedFrames))
                {
                    MessageBox.Show("The value entered is not valid.", "Error");
                    return;
                }
            }
            else return;

            // Update default value of interpolated frames
            if (modelType == K_HRC_SKELETON)
                idefaultFieldInterpFrames = numInterpolatedFrames;
            else
                idefaultBattleInterpFrames = numInterpolatedFrames;
            WriteCFGFile();

            nextElemDiff = numInterpolatedFrames + 1;

            bisLoopQ = MessageBox.Show("Is this animation a loop?", "Animation type", MessageBoxButtons.YesNo) == DialogResult.Yes;
            frameOffset = 0;

            AddStateToBuffer(this);

            if (!bisLoopQ) frameOffset = numInterpolatedFrames;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldFrame fFrame = new FieldFrame();

                    //  Create new frames
                    fAnimation.nFrames = fAnimation.nFrames * (numInterpolatedFrames + 1) - frameOffset;
                    nFrames = fAnimation.nFrames - fAnimation.frames.Count;
                    for (i = 0; i < nFrames; i++) fAnimation.frames.Add(fFrame);

                    //  Move the original frames into their new positions
                    for (fi = fAnimation.nFrames - (1 + numInterpolatedFrames - frameOffset); fi >= 0; fi -= nextElemDiff)
                        fAnimation.frames[fi] = fAnimation.frames[fi / (numInterpolatedFrames + 1)];

                    //  Interpolate the new frames
                    for (fi = 0; fi <= fAnimation.nFrames - (1 + nextElemDiff + numInterpolatedFrames - frameOffset); fi += nextElemDiff)
                    {
                        for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                        {
                            alpha = (float)ifi / (numInterpolatedFrames + 1);

                            fFrame = fAnimation.frames[fi + ifi];
                            GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[fi],
                                                           fAnimation.frames[fi + numInterpolatedFrames + 1], alpha, ref fFrame);
                            fAnimation.frames[fi + ifi] = fFrame;
                        }
                    }

                    //  Looped animation
                    if (bisLoopQ)
                    {
                        baseFinalFrame = fAnimation.nFrames - numInterpolatedFrames - 1;

                        for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                        {
                            alpha = (float)ifi / (numInterpolatedFrames + 1);

                            fFrame = fAnimation.frames[baseFinalFrame + ifi];
                            GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[baseFinalFrame],
                                                           fAnimation.frames[0], alpha, ref fFrame);
                            fAnimation.frames[baseFinalFrame + ifi] = fFrame;
                        }
                    }

                    tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    BattleAnimation tmpbAnimation;
                    //BattleFrame tmpbFrame = new BattleFrame();
                    //float primarySecondaryCountersCoef;

                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ianimIndex];
                    InterpolateBattleSkeletonAnimation(ref bSkeleton, ref tmpbAnimation, numInterpolatedFrames, bisLoopQ);
                    bAnimationsPack.SkeletonAnimations[ianimIndex] = tmpbAnimation;

                    if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.nWeapons > 0)
                    {
                        tmpbAnimation = bAnimationsPack.WeaponAnimations[ianimIndex];
                        InterpolateBattleWeaponAnimation(ref tmpbAnimation, numInterpolatedFrames, bisLoopQ,
                                                         bAnimationsPack.SkeletonAnimations[ianimIndex].numFrames,
                                                         bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort);
                        bAnimationsPack.WeaponAnimations[ianimIndex] = tmpbAnimation;
                    }

                    tbCurrentFrameScroll.Maximum = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort - 1;
                    break;
            }

            tbCurrentFrameScroll.Value = tbCurrentFrameScroll.Value * (numInterpolatedFrames + 1);

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void btnFrameNext_Click(object sender, EventArgs e)
        {
            btnPlayStopAnim.Checked = false;

            if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Maximum) tbCurrentFrameScroll.Value = 0;
            else tbCurrentFrameScroll.Value += 1;

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void btnFramePrev_Click(object sender, EventArgs e)
        {
            btnPlayStopAnim.Checked = false;

            if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Minimum) tbCurrentFrameScroll.Value = tbCurrentFrameScroll.Maximum;
            else tbCurrentFrameScroll.Value -= 1;

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void btnFrameEnd_Click(object sender, EventArgs e)
        {
            btnPlayStopAnim.Checked = false;

            tbCurrentFrameScroll.Value = tbCurrentFrameScroll.Maximum;

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void btnFrameBegin_Click(object sender, EventArgs e)
        {
            btnPlayStopAnim.Checked = false;

            tbCurrentFrameScroll.Value = tbCurrentFrameScroll.Minimum;

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void btnPlayStopAnm_CheckedChanged(object sender, EventArgs e)
        {
            // If PEditor opened we will not Play Animation
            // (too much issues when doing things in P Model);
            if (Application.OpenForms.Count > 1)
            {
                btnPlayStopAnim.Checked = false;
                return;
            }

            // Frames less or equal 1 not Play Animation (Field skeleton)
            if (modelType == K_HRC_SKELETON)
                if (fAnimation.nFrames <= 1)
                {
                    btnPlayStopAnim.Checked = false;
                    return;
                }

            // Frames less or equal 1 not Play Animation (Battle Magic skeleton)
            if (modelType == K_AA_SKELETON || modelType == K_MAGIC_SKELETON)
                if (bAnimationsPack.SkeletonAnimations[0].frames.Count <= 1)
                {
                    btnPlayStopAnim.Checked = false;
                    return;
                }

            if (btnPlayStopAnim.Checked)
            {
                btnPlayStopAnim.BackgroundImage = new Bitmap(Properties.Resources.media_stop);
                playAnimation();
            }
            else
            {
                btnPlayStopAnim.BackgroundImage = new Bitmap(Properties.Resources.media_play);
            }
        }

        private void playAnimation()
        {
            TimeSpan current_ts = new TimeSpan();
            TimeSpan prev_ts = new TimeSpan();
            bool bthisFocus = false;

            swPlayAnimation.Start();

            prev_ts = swPlayAnimation.Elapsed;

            // Loop for playing animation.
            // We can stop the animation from any place using "btnPlayStopAnim.Checked = false;"
            while (btnPlayStopAnim.Checked)
            {

                current_ts = swPlayAnimation.Elapsed;

                if (current_ts.TotalMilliseconds - prev_ts.TotalMilliseconds >= fFPS)
                {
                    if (tbCurrentFrameScroll.Value == tbCurrentFrameScroll.Maximum) tbCurrentFrameScroll.Value = 0;
                    else tbCurrentFrameScroll.Value += 1;

                    SetFrameEditorFields();
                    panelModel_Paint(null, null);

                    prev_ts = current_ts;
                }
                
                Application.DoEvents();
                if (!this.Focused && !bthisFocus)
                {
                    this.Focus();
                    bthisFocus = true;
                }

            }

            swPlayAnimation.Stop();
        }

        private void toolStripFPS15_Click(object sender, EventArgs e)
        {
            if (!toolStripFPS15.Checked)
            {
                toolStripFPS15.Checked = true;
                toolStripFPS30.Checked = false;
                toolStripFPS60.Checked = false;

                iFPS = 15;
                fFPS = 1000 / iFPS;
            }
        }

        private void toolStripFPS30_Click(object sender, EventArgs e)
        {
            if (!toolStripFPS30.Checked)
            {
                toolStripFPS15.Checked = false;
                toolStripFPS30.Checked = true;
                toolStripFPS60.Checked = false;
                
                iFPS = 30;
                fFPS = 1000 / iFPS;
            }
            else
            {
                toolStripFPS15.Checked = true;
                toolStripFPS30.Checked = false;
                toolStripFPS60.Checked = false;

                iFPS = 15;
                fFPS = 1000 / iFPS;
            }
            fFPS = 1000 / iFPS;
        }

        private void toolStripFPS60_Click(object sender, EventArgs e)
        {
            if (!toolStripFPS60.Checked)
            {
                toolStripFPS15.Checked = false;
                toolStripFPS30.Checked = false;
                toolStripFPS60.Checked = true;

                iFPS = 60;
            }
            else
            {
                toolStripFPS15.Checked = true;
                toolStripFPS30.Checked = false;
                toolStripFPS60.Checked = false;

                iFPS = 15;
            }
            fFPS = 1000 / iFPS;
        }

        private void btnCopyFrame_Click(object sender, EventArgs e)
        {

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    if (txtAnimationFrame.Text != "")
                    {
                        CopyfFieldFrame = CopyfFrame(fAnimation.frames[tbCurrentFrameScroll.Value]);

                        btnPasteFrame.Enabled = true;
                        txtCopyPasteFrame.Text = "Frm:" + txtAnimationFrame.Text;
                    }
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    if (txtAnimationFrame.Text != "" && ianimIndex >= 0)
                    {
                        CopybBattleFrame = CopybFrame(bAnimationsPack.SkeletonAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value]);

                        // Copy weapon animation if it has any
                        if (ianimIndex < bAnimationsPack.nbWeaponAnims && bAnimationsPack.WeaponAnimations.Count > 0)
                        {
                            CopybBattleWFrame = CopybFrame(bAnimationsPack.WeaponAnimations[ianimIndex].frames[tbCurrentFrameScroll.Value]);
                        }

                        btnPasteFrame.Enabled = true;
                        txtCopyPasteFrame.Text = "Anm: " + cbBattleAnimation.Text + " / Frm:" + txtAnimationFrame.Text;
                    }
                    break;
            }
        }

        private void btnPasteFrame_Click(object sender, EventArgs e)
        {

            AddStateToBuffer(this);

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    if (txtAnimationFrame.Text != "")
                    {
                        fAnimation.nFrames += 1;
                        fAnimation.frames.Insert(tbCurrentFrameScroll.Value + 1, CopyfFieldFrame);

                        tbCurrentFrameScroll.Maximum += 1;
                    }
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    float primarySecondaryCountersCoef;
                    BattleAnimation tmpbAnimation;

                    if (txtAnimationFrame.Text != "" && ianimIndex >= 0)
                    {
                        tmpbAnimation = bAnimationsPack.SkeletonAnimations[ianimIndex];

                        primarySecondaryCountersCoef = tmpbAnimation.numFrames / tmpbAnimation.numFramesShort;

                        tmpbAnimation.numFramesShort++;
                        tmpbAnimation.numFrames = (int)(tmpbAnimation.numFramesShort * primarySecondaryCountersCoef);

                        tmpbAnimation.frames.Insert(tbCurrentFrameScroll.Value + 1, CopybFrame(CopybBattleFrame));

                        bAnimationsPack.SkeletonAnimations[ianimIndex] = tmpbAnimation;


                        //  If we want to copy battle animation with weapon animation, we will do that also.
                        if (ianimIndex < bAnimationsPack.nbWeaponAnims && bSkeleton.wpModels.Count > 0)
                        {
                            tmpbAnimation = bAnimationsPack.WeaponAnimations[ianimIndex];

                            tmpbAnimation.numFramesShort = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort;
                            tmpbAnimation.numFrames = bAnimationsPack.SkeletonAnimations[ianimIndex].numFrames;

                            tmpbAnimation.frames.Insert(tbCurrentFrameScroll.Value + 1, CopybFrame(CopybBattleWFrame));

                            bAnimationsPack.WeaponAnimations[ianimIndex] = tmpbAnimation;
                        }

                        tbCurrentFrameScroll.Maximum++;
                    }
                    break;
            }
        }

        private void chkInifintyFarLights_CheckedChanged(object sender, EventArgs e)
        {
            infinityFarQ = chkInifintyFarLights.Checked;
            panelModel_Paint(null, null);
        }

        public void SetBonePieceModifiers()
        {
            PModel Model;
            float diam;

            loadingBonePieceModifiersQ = true;

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    Model = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].Model;
                    diam = ComputeDiameter(Model.BoundingBox) / 100;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        //weaponIndex = getBattleWeaponIndex();
                        Model = bSkeleton.wpModels[ianimWeaponIndex];
                        diam = ComputeDiameter(Model.BoundingBox) / 100;
                    }
                    else
                    {
                        Model = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        diam = ComputeDiameter(Model.BoundingBox) / 100;
                    }
                    break;

                default:
                    Model = fPModel;
                    diam = ComputeDiameter(Model.BoundingBox) / 100;
                    break;
            }

            hsbResizePieceX.Value = (int)(Model.resizeX * 100);
            hsbResizePieceY.Value = (int)(Model.resizeY * 100);
            hsbResizePieceZ.Value = (int)(Model.resizeZ * 100);

            hsbRepositionX.Value = (int)(Model.repositionX / diam);
            hsbRepositionY.Value = (int)(Model.repositionY / diam);
            hsbRepositionZ.Value = (int)(Model.repositionZ / diam);

            hsbRotateAlpha.Value = (int)(Model.rotateAlpha);
            hsbRotateBeta.Value = (int)(Model.rotateBeta);
            hsbRotateGamma.Value = (int)(Model.rotateGamma);

            txtResizePieceX.Text = (Model.resizeX * 100).ToString();
            txtResizePieceY.Text = (Model.resizeY * 100).ToString();
            txtResizePieceZ.Text = (Model.resizeZ * 100).ToString();

            txtRepositionX.Text = (Model.repositionX / diam).ToString();
            txtRepositionY.Text = (Model.repositionY / diam).ToString();
            txtRepositionZ.Text = (Model.repositionZ / diam).ToString();

            txtRotateAlpha.Text = (Model.rotateAlpha).ToString();
            txtRotateBeta.Text = (Model.rotateBeta).ToString();
            txtRotateGamma.Text = (Model.rotateGamma).ToString();

            hsbResizePieceX.Refresh();
            hsbResizePieceY.Refresh();
            hsbResizePieceZ.Refresh();

            hsbRepositionX.Refresh();
            hsbRepositionY.Refresh();
            hsbRepositionZ.Refresh();

            hsbRotateAlpha.Refresh();
            hsbRotateBeta.Refresh();
            hsbRotateGamma.Refresh();

            loadingBonePieceModifiersQ = false;
        }

        private void frmSkeletonEditor_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            if (this.Visible)
            {
                iwindowPosX = this.Location.X;
                iwindowPosY = this.Location.Y;
                WriteCFGFile();
            }
        }

        private void btnComputeWeaponPosition_Click(object sender, EventArgs e)
        {
            selectBoneForWeaponAttachmentQ = true;
            panelModel_Paint(null, null);
            MessageBox.Show("Please, click (right-click = end, left-click = middle) on the bone " +
                            "you want the weapon to be attached to. Press ESC to cancel.",
                            "Select bone", MessageBoxButtons.OK);
        }

        private void interpolateAllAnimationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInterpAll.ShowDialog();
        }

        private void TEXToPNGBatchConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTEX2PNGBC.ShowDialog();
        }

        private void frmSkeletonEditor_Activated(object sender, EventArgs e)
        {
            //if (GetOGLContext() != OGLContext)
            //    SetOGLContext(panelModelDC, OGLContext);

            //if (ActiveForm != this) return;

            //MessageBox.Show("frmSkeletonEditor", "Test", MessageBoxButtons.OK);

            //panelModel_Paint(null, null);
        }

        public void tsUIOpacity100_Click(object sender, EventArgs e)
        {
            this.Opacity = 1.00F;

            tsUIOpacity100.Checked = true;
            tsUIOpacity90.Checked = false;
            tsUIOpacity75.Checked = false;
            tsUIOpacity50.Checked = false;
            tsUIOpacity25.Checked = false;
        }

        public void tsUIOpacity90_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.90F;

            tsUIOpacity100.Checked = false;
            tsUIOpacity90.Checked = true;
            tsUIOpacity75.Checked = false;
            tsUIOpacity50.Checked = false;
            tsUIOpacity25.Checked = false;
        }

        public void tsUIOpacity75_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.75F;

            tsUIOpacity100.Checked = false;
            tsUIOpacity90.Checked = false;
            tsUIOpacity75.Checked = true;
            tsUIOpacity50.Checked = false;
            tsUIOpacity25.Checked = false;
        }

        public void tsUIOpacity50_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.50F;

            tsUIOpacity100.Checked = false;
            tsUIOpacity90.Checked = false;
            tsUIOpacity75.Checked = false;
            tsUIOpacity50.Checked = true;
            tsUIOpacity25.Checked = false;
        }

        public void tsUIOpacity25_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.25F;

            tsUIOpacity100.Checked = false;
            tsUIOpacity90.Checked = false;
            tsUIOpacity75.Checked = false;
            tsUIOpacity50.Checked = false;
            tsUIOpacity25.Checked = true;
        }

        private void tbCurrentFrameScroll_Scroll(object sender, EventArgs e)
        {
            iCurrentFrameScroll = tbCurrentFrameScroll.Value;
            txtAnimationFrame.Text = iCurrentFrameScroll.ToString();

            SetFrameEditorFields();
            panelModel_Paint(null, null);
        }

        private void cbWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            ianimWeaponIndex = -1;
            if (cbWeapon.Text != "EMPTY") ianimWeaponIndex = Int32.Parse(cbWeapon.Text);
            
            panelModel_Paint(null, null);
        }


        private void cbBattleAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ianimIndex = Int32.Parse(cbBattleAnimation.Text);

            bDontRefreshPicture = true;

            iCurrentFrameScroll = 0;
            txtAnimationFrame.Text = iCurrentFrameScroll.ToString();
            tbCurrentFrameScroll.Value = iCurrentFrameScroll;
            tbCurrentFrameScroll.Maximum = bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort - 1;

            bDontRefreshPicture = false;

            SetFrameEditorFields();

            panelModel_Paint(null, null);
        }

        private void tbCurrentFrameScroll_ValueChanged(object sender, EventArgs e)
        {
            iCurrentFrameScroll = tbCurrentFrameScroll.Value;
            txtAnimationFrame.Text = tbCurrentFrameScroll.Value.ToString();
        }

        public void PieceRotationModifiersChanged()
        {
            if (loadingBonePieceModifiersQ) return;

            if (!DoNotAddStateQ) AddStateToBuffer(this);
            DoNotAddStateQ = true;

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    FieldBone tmpfBone = fSkeleton.bones[SelectedBone];
                    FieldRSDResource tmpRSDResource = tmpfBone.fRSDResources[SelectedBonePiece];

                    RotatePModelModifiers(ref tmpRSDResource.Model,
                                                    hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value);

                    tmpfBone.fRSDResources[SelectedBonePiece] = tmpRSDResource;
                    fSkeleton.bones[SelectedBone] = tmpfBone;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:

                    PModel tmpbModel;

                    if (SelectedBone == bSkeleton.nBones)
                    {
                        tmpbModel = bSkeleton.wpModels[ianimWeaponIndex];
                        RotatePModelModifiers(ref tmpbModel, hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value);
                        bSkeleton.wpModels[ianimWeaponIndex] = tmpbModel;
                    }
                    else
                    {
                        tmpbModel = bSkeleton.bones[SelectedBone].Models[SelectedBonePiece];
                        RotatePModelModifiers(ref tmpbModel, hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value);
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = tmpbModel;
                    }

                    break;

                default:
                    RotatePModelModifiers(ref fPModel,
                                                    hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value);

                    break;
            }

            txtRotateAlpha.Text = hsbRotateAlpha.Value.ToString();
            txtRotateBeta.Text = hsbRotateBeta.Value.ToString();
            txtRotateGamma.Text = hsbRotateGamma.Value.ToString();

            panelModel_Paint(null, null);
            DoNotAddStateQ = false;
        }

        public void loadSkeletonFromDB()
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            int iLoadResult = 0;

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            try
            {
                // Set Global Paths
                strGlobalFieldSkeletonName = 
                        Path.GetDirectoryName(frmFieldDB.strFieldFile) + "\\" + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper();
                strGlobalFieldAnimationName = 
                        Path.GetDirectoryName(frmFieldDB.strAnimFile) + "\\" + Path.GetFileName(frmFieldDB.strAnimFile).ToUpper();

                // Initialize OpenGL Context;
                //InitOpenGLContext();

                // Close frmPEditor if opened
                if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                // Disable/Make Invisible in Forms Data controls
                InitializeWinFormsDataControls();

                // Load Field Skeleton
                iLoadResult = LoadSkeletonFromDB(frmFieldDB.strFieldFile, frmFieldDB.strAnimFile, true);

                if (iLoadResult == -2)
                {
                    MessageBox.Show("Error Destroying Skeleton file " + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper() + ".",
                                    "Error");
                    return;
                }

                if (iLoadResult == -1)
                {
                    MessageBox.Show("Error opening Skeleton file " + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper() + ".",
                                    "Error");
                    return;
                }

                if (iLoadResult == 0)
                {
                    MessageBox.Show("The file " + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper() + " has not any known Skeleton format.",
                                    "Warning");
                    return;
                }

                // Enable/Make Visible Win Forms Data controls
                EnableWinFormsDataControls();

                // ComputeBoundingBoxes
                ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[0], ref p_min, ref p_max);

                diameter = ComputeFieldDiameter(fSkeleton);

                // Set frame values in frame editor groupbox...
                SetFrameEditorFields();

                // Set texture values in texture editor groupbox...
                SetTextureEditorFields();

                // PostLoadModelPreparations
                PostLoadModelPreparations(ref p_min, ref p_max);

                // We can draw the model in panel
                panelModel_Paint(null, null);
            }
            catch
            {
                MessageBox.Show("Global error opening Field Skeleton file " + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper() + ".",
                                "Error");
            }
        }

        public void loadSkeletonFromBattleDB(string strfileNameModel, string strfileNameAnim)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            int iLoadResult = 0;

            // Check if changes has been commited in PEditor
            if (CheckChangesCommittedPEditor()) return;

            try
            {
                // Set Global Paths
                strGlobalBattleSkeletonName =
                        Path.GetDirectoryName(strfileNameModel) + "\\" + Path.GetFileName(strfileNameModel).ToUpper();
                strGlobalBattleAnimationName =
                        Path.GetDirectoryName(strfileNameAnim) + "\\" + Path.GetFileName(strfileNameAnim).ToUpper();

                // Initialize OpenGL Context;
                //InitOpenGLContext();

                // Close frmPEditor if opened
                if (FindWindowOpened("frmPEditor")) frmPEdit.Close();

                // Disable/Make Invisible in Forms Data controls
                InitializeWinFormsDataControls();

                // Load Battle Skeleton
                iLoadResult = LoadSkeletonFromDB(strfileNameModel, strfileNameAnim, true);

                if (iLoadResult == -2)
                {
                    MessageBox.Show("Error Destroying Skeleton file " + Path.GetFileName(strfileNameModel).ToUpper() + ".",
                                    "Error");
                    return;
                }

                if (iLoadResult == -1)
                {
                    MessageBox.Show("Error opening Skeleton file " + Path.GetFileName(strfileNameModel).ToUpper() + ".",
                                    "Error");
                    return;
                }

                if (iLoadResult == 0)
                {
                    MessageBox.Show("The file " + Path.GetFileName(strfileNameModel).ToUpper() + " has not any known Skeleton format.",
                                    "Warning");
                    return;
                }

                // Update Paths
                WriteCFGFile();

                // Enable/Make Visible Win Forms Data controls
                EnableWinFormsDataControls();

                // ComputeBoundingBoxes
                ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[0], ref p_min, ref p_max);

                diameter = ComputeBattleDiameter(bSkeleton);

                // Set frame values in frame editor groupbox...
                SetFrameEditorFields();

                // Set texture values in texture editor groupbox...
                SetTextureEditorFields();

                // PostLoadModelPreparations
                PostLoadModelPreparations(ref p_min, ref p_max);

                // We can draw the model in panel
                panelModel_Paint(null, null);

            }
            catch
            {
                MessageBox.Show("Global error opening Battle Skeleton file " + Path.GetFileName(frmFieldDB.strFieldFile).ToUpper() + ".",
                                "Error");
            }
        }

        public void SetWeaponAnimationAttachedToBone(bool middleQ, frmSkeletonEditor frmSkEditor)
        {
            int fi, jsp;
            double[] MV_matrix = new double[16];
            BattleFrame tmpbFrame;

            AddStateToBuffer(frmSkEditor);

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glLoadIdentity();

            for (fi = 0; fi < bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort; fi++)
            {
                if (middleQ) jsp = MoveToBattleBoneMiddle(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi], SelectedBone);
                else jsp = MoveToBattleBoneEnd(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[fi], SelectedBone);

                glGetDoublev((uint)glCapability.GL_MODELVIEW_MATRIX, MV_matrix);

                tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi];
                tmpbFrame.startX = (int)MV_matrix[12];
                tmpbFrame.startY = (int)MV_matrix[13];
                tmpbFrame.startZ = (int)MV_matrix[14];
                bAnimationsPack.WeaponAnimations[ianimIndex].frames[fi] = tmpbFrame;

                while (jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
            }

            glPopMatrix();

            selectBoneForWeaponAttachmentQ = false;
            SetFrameEditorFields();
        }

        private void pbTextureViewer_DoubleClick(object sender, EventArgs e)
        {
            if (cbTextureSelect.Items.Count > 0 && cbTextureSelect.SelectedIndex > -1)
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        if (fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[cbTextureSelect.SelectedIndex].texID == 0xFFFFFFFF)
                            return;

                        frmTexViewer = new frmTextureViewer(this, fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].Model);

                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        if (bSkeleton.textures[cbTextureSelect.SelectedIndex].texID == 0xFFFFFFFF) return;

                        if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                        {
                            if (ianimWeaponIndex == -1) return;
                            frmTexViewer = new frmTextureViewer(this, bSkeleton.wpModels[cbWeapon.SelectedIndex]);                           
                        }
                        else
                        {
                            frmTexViewer = new frmTextureViewer(this, bSkeleton.bones[SelectedBone].Models[SelectedBonePiece]);
                        }
                        break;
                }

                frmTexViewer.ShowDialog();
            }
        }


        private void inputFramesDataTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iOpenResult;

            // Set filter options and filter index depending on modelType
            openFile.Title = "Input Frame Data from TXT";
            openFile.Filter = "Frame Data|*.txt|All files|*.*";

            // Check Initial Directory
            openFile.FileName = Path.GetFileNameWithoutExtension(strGlobalFieldAnimationName) + ".TXT";
            openFile.FilterIndex = 1;
            openFile.InitialDirectory = strGlobalPathFieldAnimationFolder;

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Let's save state to buffer
                    AddStateToBuffer(this);

                    // We load the Frames Data
                    iOpenResult = ReadFrameData(openFile.FileName, false);

                    if (iOpenResult == -1)
                    {
                        MessageBox.Show("It has been some problem [input] while loading the Frame Data from file " +
                                        Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                        "Error");
                    }

                    strGlobalFieldAnimationName = fAnimation.strFieldAnimationFile;

                    // Let's stop the Animation
                    btnPlayStopAnim.Checked = false;

                    iCurrentFrameScroll = 0;
                    tbCurrentFrameScroll.Value = 0;
                    txtAnimationFrame.Text = iCurrentFrameScroll.ToString();

                    tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                    SetFrameEditorFields();

                    UpdateMainSkeletonWindowTitle();

                    panelModel_Paint(null, null);
                }
            }
            catch
            {
                MessageBox.Show("Error exception [input] loading the Frame Data from file " +
                                Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void inputFramesDataFromTXTOnlyFieldModelsSelectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iOpenResult;

            // Set filter options and filter index depending on modelType
            openFile.Title = "Input Frame Data from TXT (Selective)";
            openFile.Filter = "Frame Data|*.txt|All files|*.*";

            // Check Initial Directory
            openFile.FileName = Path.GetFileNameWithoutExtension(strGlobalFieldAnimationName) + ".TXT";
            openFile.FilterIndex = 1;
            openFile.InitialDirectory = strGlobalPathFieldAnimationFolder;

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Let's save state to buffer
                    AddStateToBuffer(this);

                    // We load the Frames Data
                    iOpenResult = ReadFrameDataSelective(openFile.FileName);

                    if (iOpenResult == -1)
                    {
                        MessageBox.Show("It has been some problem while loading the Frame Data from file (Selective) " +
                                        Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                        "Error");
                    }

                    strGlobalFieldAnimationName = fAnimation.strFieldAnimationFile;

                    // Let's stop the Animation
                    btnPlayStopAnim.Checked = false;

                    iCurrentFrameScroll = 0;
                    tbCurrentFrameScroll.Value = 0;
                    txtAnimationFrame.Text = iCurrentFrameScroll.ToString();

                    tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                    SetFrameEditorFields();

                    UpdateMainSkeletonWindowTitle();

                    panelModel_Paint(null, null);
                }
            }
            catch
            {
                MessageBox.Show("Error exception loading the Frame Data from file (Selective) " +
                                Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }

        private void mergeFramesDataTXTOnlyFieldModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iOpenResult;

            // Set filter options and filter index depending on modelType
            openFile.Title = "Merge Frame Data from TXT";
            openFile.Filter = "Frame Data|*.txt|All files|*.*";

            // Check Initial Directory
            openFile.FileName = Path.GetFileNameWithoutExtension(strGlobalFieldAnimationName) + ".TXT";
            openFile.FilterIndex = 1;
            openFile.InitialDirectory = strGlobalPathFieldAnimationFolder;

            try
            {
                // Process input if the user clicked OK.
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Let's save state to buffer
                    AddStateToBuffer(this);

                    // We load the Frames Data
                    iOpenResult = ReadFrameData(openFile.FileName, true);

                    if (iOpenResult == -1)
                    {
                        MessageBox.Show("It has been some problem [merge] while loading the Frame Data from file " +
                                        Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                        "Error");
                    }

                    strGlobalFieldAnimationName = fAnimation.strFieldAnimationFile;

                    // Let's stop the Animation
                    btnPlayStopAnim.Checked = false;

                    iCurrentFrameScroll = 0;
                    tbCurrentFrameScroll.Value = 0;
                    txtAnimationFrame.Text = iCurrentFrameScroll.ToString();

                    tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                    SetFrameEditorFields();

                    UpdateMainSkeletonWindowTitle();

                    panelModel_Paint(null, null);
                }
            }
            catch
            {
                MessageBox.Show("Error exception [merge] loading the Frame Data from file " +
                                Path.GetFileName(openFile.FileName).ToUpper() + ".",
                                "Error");
                return;
            }
        }


        public void UpdateBones(string strBoneSelected)
        {
            cbBoneSelector.Items.Clear();
            FillBoneSelector(cbBoneSelector);

            cbBoneSelector.SelectedItem = strBoneSelected;

            // We can redraw the model in panel
            panelModel.Update();
            panelModel_Paint(null, null);
        }

        private void addJointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tmpText = "";
            if (fAnimation.nFrames > 1)
            {
                MessageBox.Show("You can Add/Edit Joints when the loaded animation has only 1 frame.",
                                "Information");
                return;
            }

            if (cbBoneSelector.SelectedIndex != -1) tmpText = cbBoneSelector.Text;

            frmSJ = new frmSkeletonJoints(this, 0, tmpText);
            frmSJ.ShowDialog();
            frmSJ.Dispose();
        }

        private void editJointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fAnimation.nFrames > 1)
            {
                MessageBox.Show("You can Add/Edit Joints when the loaded animation has only 1 frame.",
                                "Information");
                return;
            }

            if (cbBoneSelector.SelectedIndex >= 0)
            {
                frmSJ = new frmSkeletonJoints(this, 1, cbBoneSelector.Text);
                frmSJ.ShowDialog();
                frmSJ.Dispose();
            }
            else
                MessageBox.Show("If you want to Edit Joints you need to select a Bone in Selected Bone combobox.",
                                "Information");
        }

        public void UpdateMainSkeletonWindowTitle()
        {
            switch(modelType)
            {
                case K_HRC_SKELETON:

                    if (IsRSDResource) Text = STR_APPNAME + " - Model: " + strGlobalRSDResourceName.ToUpper();
                    else
                        Text = STR_APPNAME + " - Model: " + strGlobalFieldSkeletonFileName.ToUpper() + 
                                             " / Anim: " + strGlobalFieldAnimationName.ToUpper();
                    break;

                case K_AA_SKELETON:
                    Text = STR_APPNAME + " - Model: " + strGlobalBattleSkeletonFileName.ToUpper() +
                                         " / Anim: " + strGlobalBattleAnimationName.ToUpper();
                    break;

                case K_MAGIC_SKELETON:
                    Text = STR_APPNAME + " - Model: " + strGlobalMagicSkeletonFileName.ToUpper() +
                                         " / Anim: " + strGlobalMagicAnimationName.ToUpper();
                    break;

                case K_3DS_MODEL:
                    Text = Text + " - Model: " + Path.GetFileNameWithoutExtension(strGlobal3DSModelName).ToUpper() + ".P";
                    break;

                default:

                    if (IsTMDModel) Text = Text + " - Model: " + strGlobalTMDModelName.ToUpper();
                    else Text = Text + " - Model: " + strGlobalPModelName.ToUpper();
                    break;
            }

            if (bChangesDone)
                    Text += "  *";
        }

        public void UpdateScrollBars(int rszX, int rszY, int rszZ,
                                     int repX, int repY, int repZ,
                                     int alpha, int beta, int gamma)
        {
            loadingBonePieceModifiersQ = true;

            hsbResizePieceX.Value = rszX;
            hsbResizePieceY.Value = rszY;
            hsbResizePieceZ.Value = rszZ;

            hsbRepositionX.Value = repX;
            hsbRepositionY.Value = repY;
            hsbRepositionZ.Value = repZ;

            hsbRotateAlpha.Value = alpha;
            hsbRotateBeta.Value = beta;
            hsbRotateGamma.Value = gamma;

            loadingBonePieceModifiersQ = false;
        }


        // This function asks to the user (as remember) what wants to do with
        // the changes committed in the PEditor in case he pushes Load options
        // without saving.
        public bool CheckChangesCommittedPEditor()
        {
            DialogResult mbbYesNo;
            bool bCheckChangesCommittedPEditor = false;

            if (bChangesDone)
            {
                mbbYesNo = MessageBox.Show("You have performed some changes to the model in the PEditor. " +
                                           "Do you want to cancel this changes?", "Warning",
                                           MessageBoxButtons.YesNo);

                if (mbbYesNo == DialogResult.No) bCheckChangesCommittedPEditor = true;
            }

            return bCheckChangesCommittedPEditor;
        }

        public void ChangeGroupBoxesStatusPEditor(bool bStatus)
        {
            // We will change some sliders status
            gbSelectedPieceFrame.Enabled = bStatus;
            gbSelectedBoneFrame.Enabled = bStatus;
        }

        public void SetBonePieceModifiersPEditor(int SelectedBone, int SelectedBonePiece)
        {

            loadingBonePieceModifiersQ = true;

            hsbResizePieceX.Value = 100;
            hsbResizePieceY.Value = 100;
            hsbResizePieceZ.Value = 100;

            hsbRepositionX.Value = 1;
            hsbRepositionY.Value = 1;
            hsbRepositionZ.Value = 1;

            hsbRotateAlpha.Value = 0;
            hsbRotateBeta.Value = 0;
            hsbRotateGamma.Value = 0;

            txtResizePieceX.Text = "100";
            txtResizePieceY.Text = "100";
            txtResizePieceZ.Text = "100";

            txtRepositionX.Text = "1";
            txtRepositionY.Text = "1";
            txtRepositionZ.Text = "1";

            txtRotateAlpha.Text = "0";
            txtRotateBeta.Text = "0"; 
            txtRotateGamma.Text = "0";

            hsbResizePieceX.Refresh();
            hsbResizePieceY.Refresh();
            hsbResizePieceZ.Refresh();

            txtResizePieceX.Refresh();
            txtResizePieceY.Refresh();
            txtResizePieceZ.Refresh();

            hsbRepositionX.Refresh();
            hsbRepositionY.Refresh();
            hsbRepositionZ.Refresh();

            txtRepositionX.Refresh();
            txtRepositionY.Refresh();
            txtRepositionZ.Refresh();

            hsbRotateAlpha.Refresh();
            hsbRotateBeta.Refresh();
            hsbRotateGamma.Refresh();

            txtRotateAlpha.Refresh();
            txtRotateBeta.Refresh();
            txtRotateGamma.Refresh();

            loadingBonePieceModifiersQ = false;
        }

    }
}

