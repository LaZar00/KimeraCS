using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HDC = System.IntPtr;

namespace KimeraCS
{
    using Defines;

    using static FrmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldRSDResource;
    using static FF7BattleSkeleton;

    using static FF7PModel;
    using static Model_3DS;

    using static ModelDrawing;

    using static Lighting;

    using static UndoRedoPE;
    using static Utils;
    using static FileTools;
    using static OpenGL32;
    using static User32;
    using static GDI32;

    public partial class FrmPEditor : Form
    {

        readonly private FrmSkeletonEditor frmSkelEdit;

        // Const
        private const int K_PAINT = 0;
        private const int K_CUT_EDGE = 1;
        private const int K_ERASE_POLY = 2;
        private const int K_PICK_VERTEX = 3;
        private const int K_MOVE_VERTEX = 8;
        private const int K_ROTATE = 4;
        private const int K_ZOOM = 5;
        private const int K_PAN = 6;
        private const int K_NEW_POLY = 7;

        private const int K_LOAD = -1;
        private const int K_MOVE = 0;
        private const int K_CLICK = 1;
        private const int K_CLICK_SHIFT = 2;


        public const int K_MESH = 0;
        public const int K_PCOLORS = 1;
        public const int K_VCOLORS = 2;

        public const int LETTER_SIZE = 5;
        public const int LIGHT_STEPS = 10;


        private const int F_BATTLELOCATION_SCALE = 10;


        // Vars
        public static PModel EditedPModel;
        public uint[] tex_ids = new uint[1] { 0 };

        public static int EditedBone, EditedBonePiece;

        public static int drawMode;

        public int primaryFunc, secondaryFunc, ternaryFunc;

        public Color[] vcolorsOriginal;
        public Color[] pcolorsOriginal;

        public static float alphaPE, betaPE, gammaPE;
        public static float DISTPE;
        public static float panXPE, panYPE, panZPE;

        public float planeA, planeB, planeC, planeD;
        public float planeOriginalA, planeOriginalB, planeOriginalC, planeOriginalD;
        public float oldAlphaPlane, oldBetaPlane, oldGammaPlane;

        public Point3D planeOriginalPoint = new Point3D();
        public Point3D planePoint = new Point3D();
        public Quaternion planeRotationQuat = new Quaternion();
        public static double[] planeTransformation = new double[16];

        public static Point3D planeOriginalPoint1 = new Point3D();
        public static Point3D planeOriginalPoint2 = new Point3D();
        public static Point3D planeOriginalPoint3 = new Point3D();
        public static Point3D planeOriginalPoint4 = new Point3D();

        public static float rszXPE, rszYPE, rszZPE;
        public static float repXPE, repYPE, repZPE;
        public float x_lastPE, y_lastPE;
        public static int iLightX, iLightY, iLightZ;

        // PEditor drawing main DoFunction vars
        public static int VCountNewPoly;
        public static ushort[] tmpVNewPoly = new ushort[3];
        public static double dblPickedVertexZ;
        List<int> lstPickedVertices = new List<int>();
        int[] lstAdjacentPolys;
        STIntVector[] lstAdjacentVerts;
        STIntVector[] lstAdjacentAdjacentPolys;

        public static bool loadedPModel;
        public static bool bLoading;
        private bool pbMouseIsDownPE;

        public bool loadingModifiersQ;
        public bool loadingColorModifiersQ;
        public static bool updateSkeletonEditorQ;
        public bool controlPressedQ;
        public int shiftPressedQ;
        public bool DoNotAddPEStateQ;

        public static HDC panelEditorPModelDC;
        public static HDC OGLContextPEditor;

        // Palette
        public static List<Color> colorTable = new List<Color>();
        public static PairIB[] translationTablePolys;
        public static PairIB[] translationTableVertex;
        public static int iSelectedColor, iBrightnessFactor;
        public static byte iThreshold;
        public DirectBitmap bmpFullGradientPalette;
        public bool bColorsChanged;  // -- (KimeraVB6 var "ModelDirty")

        // Var for Group aspects
        public static int SelectedGroup;
        public static float rszGroupXPE, rszGroupYPE, rszGroupZPE;
        public static float repGroupXPE, repGroupYPE, repGroupZPE;
        public static float alphaGroupPE, betaGroupPE, gammaGroupPE;
        public static float fBattleLocationGroupScale;
        public static bool bGlobalChangeGroup;

        // GroupPropierties vars
        public FrmGroupProperties frmGroupProp;


        public FrmPEditor(FrmSkeletonEditor frmSkelEdit, PModel ModelIn)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;

            EditedPModel = CopyPModel(ModelIn);
        }


        /////////////////////////////////////////////////////////////
        // OpenGL methods:
        public void SetOGLEditorSettings()
        {
            glClearDepth(1.0f);

            glEnable(GLCapability.GL_DEPTH_TEST);
            glDepthFunc(GLFunc.GL_LEQUAL);

            SetBlendMode(BLEND_MODE.BLEND_NONE);

            glCullFace(GLFace.GL_FRONT);
            glEnable(GLCapability.GL_CULL_FACE);

            glEnable(GLCapability.GL_ALPHA_TEST);
            glAlphaFunc(GLFunc.GL_GREATER, 0);

            //glEnable(GLCapability.GL_BLEND);
            //glBlendFunc(GLBlendFuncFactor.GL_SRC_ALPHA, GLBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);

        }
        /////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////
        // ToolTip Helpers:
        // Create the ToolTip and associate with the Form container.
        readonly ToolTip toolTip1 = new ToolTip();

        public void DefineToolTips()
        {
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(rbPaintPolygon, "Paint Polygon");
            toolTip1.SetToolTip(rbCutEdge, "Cut Edge");
            toolTip1.SetToolTip(rbErasePolygon, "Erase Polygon");
            toolTip1.SetToolTip(rbMoveVertex, "Move Vertex");
            toolTip1.SetToolTip(rbFreeRotate, "Free Rotate");
            toolTip1.SetToolTip(rbZoomInOut, "Zoom In/Out");
            toolTip1.SetToolTip(rbPanning, "Panning");
            toolTip1.SetToolTip(rbNewPolygon, "New Polygon");

            toolTip1.SetToolTip(btnLessBrightness, "Less Brightness");
            toolTip1.SetToolTip(btnLessBrightness, "More Brightness");
            toolTip1.SetToolTip(btnResetBrightness, "Reset Brightness");
        }

        private void FrmPEditor_Resize(object sender, EventArgs e)
        {            
            if (loadedPModel && this.Visible && !bLoading)
            {
                // Check first if minimized.
                if (Application.OpenForms.Count > 1)
                {
                    if (Application.OpenForms[1].WindowState == FormWindowState.Minimized) return;

                    // We can redraw the model in panel
                    //panelEditorPModel.Update();
                    PanelEditorPModel_Paint(null, null);
                }

                // Assign this if visible
                isizeWindowWidthPE = this.Size.Width;
                isizeWindowHeightPE = this.Size.Height;

                WriteCFGFile();
            }
        }

        private void FrmPEditor_KeyDown(object sender, KeyEventArgs e)
        {
            DoNotAddPEStateQ = false;

            if (e.KeyCode == Keys.ShiftKey) shiftPressedQ = 1;

            if (e.KeyCode == Keys.ControlKey) controlPressedQ = true;

            // With "Q" key we will "Deselect" any selected group in the list of groups.
            // This method (Group Selected/No Group Selected) will be used to Reposition/Resize/Rotate
            // the Model or the selected Group.
            if (e.KeyCode == Keys.Q)
            {
                unselectGroupToolStripMenuItem.PerformClick();
            }

            // Reset New Poly feature counted vertices
            if (e.KeyCode == Keys.N)
            {
                if (rbNewPolygon.Checked)
                {
                    VCountNewPoly = 0;
                    rbNewPolygon.Text = "0/3";
                }
            }

            // Hide/Show Group
            if (e.KeyCode == Keys.H)
            {
                if (lbGroups.SelectedIndex > -1)
                {
                    btnHideShowGroup.PerformClick();
                }
            }

            // Brightness buttons
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus) btnMoreBrightness.PerformClick();
            if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus) btnLessBrightness.PerformClick();
            if (e.KeyCode == Keys.R) btnResetBrightness.PerformClick();


            panelEditorPModel.Update();
            PanelEditorPModel_Paint(null, null);
        }

        private void FrmPEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey) shiftPressedQ = 0;

            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey) controlPressedQ = false;

            panelEditorPModel.Update();
            PanelEditorPModel_Paint(null, null);
        }

        private void ChkEnableLighting_CheckedChanged(object sender, EventArgs e)
        {
            // Change icon and enable scrolls
            if (chkEnableLighting.Checked)
            {
                chkEnableLighting.Image = new Bitmap(Properties.Resources.lightbulb_on16);

                hsbLightX.Enabled = true;
                hsbLightY.Enabled = true;
                hsbLightZ.Enabled = true;
            }               
            else
            {
                chkEnableLighting.Image = new Bitmap(Properties.Resources.lightbulb_off16);

                hsbLightX.Enabled = false;
                hsbLightY.Enabled = false;
                hsbLightZ.Enabled = false;
            }
                
            // Do processing
            //if (chkEnableLighting.Checked && loadedPModel)
            //{
            //    glEnable(GLCapability.GL_NORMALIZE);
            //    ComputeNormals(ref EditedPModel);
            //}
            //else glDisable(GLCapability.GL_NORMALIZE);

            PanelEditorPModel_Paint(null, null);
        }

        private void ChkShowAxes_Click(object sender, EventArgs e)
        {
            PanelEditorPModel_Paint(null, null);
        }

        private void ChkShowPlane_Click(object sender, EventArgs e)
        {
            PanelEditorPModel_Paint(null, null);
        }

        private void RbMesh_Click(object sender, EventArgs e)
        {
            drawMode = 0;

            rbMesh.Checked = true;
            rbPolygonColors.Checked = false;
            rbVertexColors.Checked = false;

            PanelEditorPModel_Paint(null, null);
        }

        private void RbPolygonColors_Click(object sender, EventArgs e)
        {
            drawMode = 1;

            rbMesh.Checked = false;
            rbPolygonColors.Checked = true;
            rbVertexColors.Checked = false;

            PanelEditorPModel_Paint(null, null);
        }

        private void RbVertexColors_Click(object sender, EventArgs e)
        {
            drawMode = 2;

            rbMesh.Checked = false;
            rbPolygonColors.Checked = false;
            rbVertexColors.Checked = true;
            PanelEditorPModel_Paint(null, null);
        }

        private void PanelEditorPModel_MouseDown(object sender, MouseEventArgs e)
        {
            if (loadedPModel)
            {
                pbMouseIsDownPE = true;

                if (chkEnableLighting.Checked) glEnable(GLCapability.GL_LIGHTING);

                //glClearColor(0.4f, 0.4f, 0.65f, 0);
                glViewport(0, 0, panelEditorPModel.ClientRectangle.Width, panelEditorPModel.ClientRectangle.Height);
                //glClear(GLBufferMask.GL_COLOR_BUFFER_BIT | GLBufferMask.GL_DEPTH_BUFFER_BIT);

                SetCameraPModel(EditedPModel, panXPE, panYPE, panZPE + DISTPE,
                                alphaPE, betaPE, gammaPE, 1, 1, 1);

                ConcatenateCameraModelViewQuat(repXPE, repYPE, repZPE,
                                               EditedPModel.rotationQuaternion,
                                               rszXPE, rszYPE, rszZPE);

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        DoFunction(primaryFunc, K_CLICK + shiftPressedQ, e.X, e.Y);
                        break;

                    case MouseButtons.Right:
                        DoFunction(secondaryFunc, K_CLICK + shiftPressedQ, e.X, e.Y);
                        break;

                    case MouseButtons.Middle:
                        DoFunction(ternaryFunc, K_CLICK + shiftPressedQ, e.X, e.Y);
                        break;

                }

                PanelEditorPModel_Paint(null, null);

                x_lastPE = e.X;
                y_lastPE = e.Y;
            }
        }

        private void PanelEditorPModel_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbMouseIsDownPE)
            {
                if (loadedPModel && e.Button != MouseButtons.None)
                {

                    glClearColor(0.4f, 0.4f, 0.65f, 0);
                    glViewport(0, 0, panelEditorPModel.ClientRectangle.Width, panelEditorPModel.ClientRectangle.Height);
                    glClear(GLBufferMask.GL_COLOR_BUFFER_BIT | GLBufferMask.GL_DEPTH_BUFFER_BIT);

                    SetCameraPModel(EditedPModel, panXPE, panYPE, panZPE + DISTPE,
                                    alphaPE, betaPE, gammaPE, 1, 1, 1);

                    ConcatenateCameraModelView(repXPE, repYPE, repZPE,
                                               (float)(hsbRotateAlpha.Value / fBattleLocationGroupScale),
                                               (float)(hsbRotateBeta.Value / fBattleLocationGroupScale),
                                               (float)(hsbRotateGamma.Value / fBattleLocationGroupScale),
                                               rszXPE, rszYPE, rszZPE);

                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            DoFunction(primaryFunc, K_MOVE, e.X, e.Y);
                            break;

                        case MouseButtons.Right:
                            DoFunction(secondaryFunc, K_MOVE, e.X, e.Y);
                            break;

                        case MouseButtons.Middle:
                            DoFunction(ternaryFunc, K_MOVE, e.X, e.Y);
                            break;

                    }

                    x_lastPE = e.X;
                    y_lastPE = e.Y;

                    PanelEditorPModel_Paint(null, null);
                }
            }
        }

        private void PanelEditorPModel_MouseUp(object sender, MouseEventArgs e)
        {
            pbMouseIsDownPE = false;

            if (e.Button == MouseButtons.Left)
            {
                if (primaryFunc == K_MOVE_VERTEX) 
                        primaryFunc = K_PICK_VERTEX;
                else
                    if (secondaryFunc == K_MOVE_VERTEX) 
                            secondaryFunc = K_PICK_VERTEX;
            }
        }

        public void PanelEditorPModel_MouseWheel(object sender, MouseEventArgs e)
        {

             if (controlPressedQ)
                DISTPE += e.Delta * ComputeDiameter(EditedPModel.BoundingBox) / 10000;
            else
                DISTPE += e.Delta * ComputeDiameter(EditedPModel.BoundingBox) / 1000;

            SetCameraModelView(panXPE, panYPE, panZPE + DISTPE, alphaPE, betaPE, gammaPE, 1, 1, 1);

            PanelEditorPModel_Paint(null, null);

        }

        public void PanelEditorPModel_Paint(object sender, PaintEventArgs e)
        {

            if (loadedPModel)
            {
                if (GetOGLContext() != OGLContextPEditor)
                    SetOGLContext(panelEditorPModelDC, OGLContextPEditor);

                SetOGLEditorSettings();

                glViewport(0, 0, panelEditorPModel.ClientRectangle.Width,
                                 panelEditorPModel.ClientRectangle.Height);
                ClearPanel();
                //SetDefaultOGLRenderState();

                DrawPModelEditor(chkEnableLighting.Checked, panelEditorPModel);

                //SetDefaultOGLRenderState();

                if (chkShowPlane.Checked) DrawPlane(ref planeTransformation, ref planeOriginalPoint1,
                                                                             ref planeOriginalPoint2,
                                                                             ref planeOriginalPoint3,
                                                                             ref planeOriginalPoint4);

                if (chkShowAxes.Checked) DrawAxesPE(panelEditorPModel);

                glFlush();
                SwapBuffers(panelEditorPModelDC);

            }
        }

        private void NudXPlane_ValueChanged(object sender, EventArgs e)
        {
            planeTransformation[12] = (float)nudXPlane.Value * EditedPModel.diameter / 100;
            ComputeCurrentEquations();
            PanelEditorPModel_Paint(null, null);
        }

        private void NudXPlane_TextChanged(object sender, EventArgs e)
        {
            NudXPlane_ValueChanged(sender, e);
        }

        private void NudYPlane_ValueChanged(object sender, EventArgs e)
        {
            planeTransformation[13] = (float)nudYPlane.Value * EditedPModel.diameter / 100;
            ComputeCurrentEquations();
            PanelEditorPModel_Paint(null, null);
        }

        private void NudYPlane_TextChanged(object sender, EventArgs e)
        {
            NudYPlane_ValueChanged(sender, e);
        }

        private void NudZPlane_ValueChanged(object sender, EventArgs e)
        {
            planeTransformation[14] = (float)nudZPlane.Value * EditedPModel.diameter / 100;
            ComputeCurrentEquations();
            PanelEditorPModel_Paint(null, null);
        }

        private void NudZPlane_TextChanged(object sender, EventArgs e)
        {
            NudZPlane_ValueChanged(sender, e);
        }

        private void NudAlphaPlane_ValueChanged(object sender, EventArgs e)
        {
            float fDiff;
            Quaternion tmpQuat = new Quaternion();
            Quaternion resQuat = new Quaternion();

            fDiff = (float)nudAlphaPlane.Value - oldAlphaPlane;
            oldAlphaPlane = (float)nudAlphaPlane.Value;

            BuildQuaternionFromEuler(fDiff, 0, 0, ref tmpQuat);
            MultiplyQuaternions(planeRotationQuat, tmpQuat, ref resQuat);
            planeRotationQuat = resQuat;
            BuildMatrixFromQuaternion(planeRotationQuat, ref planeTransformation);
            planeTransformation[12] = (float)nudXPlane.Value * EditedPModel.diameter / 100;
            planeTransformation[13] = (float)nudYPlane.Value * EditedPModel.diameter / 100;
            planeTransformation[14] = (float)nudZPlane.Value * EditedPModel.diameter / 100;
            NormalizeQuaternion(ref planeRotationQuat);

            ComputeCurrentEquations();
            PanelEditorPModel_Paint(null, null);
        }

        private void NudAlphaPlane_TextChanged(object sender, EventArgs e)
        {
            NudAlphaPlane_ValueChanged(sender, e);
        }

        private void NudBetaPlane_ValueChanged(object sender, EventArgs e)
        {
            float fDiff;
            Quaternion tmpQuat = new Quaternion();
            Quaternion resQuat = new Quaternion();

            fDiff = (float)nudBetaPlane.Value - oldBetaPlane;
            oldBetaPlane = (float)nudBetaPlane.Value;

            BuildQuaternionFromEuler(0, fDiff, 0, ref tmpQuat);
            MultiplyQuaternions(planeRotationQuat, tmpQuat, ref resQuat);
            planeRotationQuat = resQuat;
            BuildMatrixFromQuaternion(planeRotationQuat, ref planeTransformation);
            planeTransformation[12] = (float)nudXPlane.Value * EditedPModel.diameter / 100;
            planeTransformation[13] = (float)nudYPlane.Value * EditedPModel.diameter / 100;
            planeTransformation[14] = (float)nudZPlane.Value * EditedPModel.diameter / 100;
            NormalizeQuaternion(ref planeRotationQuat);

            ComputeCurrentEquations();
            PanelEditorPModel_Paint(null, null);
        }

        private void NudBetaPlane_TextChanged(object sender, EventArgs e)
        {
            NudBetaPlane_ValueChanged(sender, e);
        }

        private void HsbLightX_ValueChanged(object sender, EventArgs e)
        {
            iLightX = hsbLightX.Value;
            PanelEditorPModel_Paint(null, null);
        }

        private void HsbLightY_ValueChanged(object sender, EventArgs e)
        {
            iLightY = hsbLightY.Value;
            PanelEditorPModel_Paint(null, null);
        }

        private void HsbLightZ_ValueChanged(object sender, EventArgs e)
        {
            iLightZ = hsbLightZ.Value;
            PanelEditorPModel_Paint(null, null);
        }

        private void HsbResizeX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                rszGroupXPE = (float)hsbResizeX.Value / 100;
                txtResizeX.Text = hsbResizeX.Value.ToString();
                EditedPModel.Groups[SelectedGroup].rszGroupX = rszGroupXPE;
            }
            else
            {
                rszXPE = (float)hsbResizeX.Value / 100;
                txtResizeX.Text = hsbResizeX.Value.ToString();
                EditedPModel.resizeX = rszXPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void HsbResizeY_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                rszGroupYPE = (float)hsbResizeY.Value / 100;
                txtResizeY.Text = hsbResizeY.Value.ToString();
                EditedPModel.Groups[SelectedGroup].rszGroupY = rszGroupYPE;
            }
            else
            {
                rszYPE = (float)hsbResizeY.Value / 100;
                txtResizeY.Text = hsbResizeY.Value.ToString();
                EditedPModel.resizeY = rszYPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void HsbResizeZ_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                rszGroupZPE = (float)hsbResizeZ.Value / 100;
                txtResizeZ.Text = hsbResizeZ.Value.ToString();
                EditedPModel.Groups[SelectedGroup].rszGroupZ = rszGroupZPE;
            }
            else
            {
                rszZPE = (float)hsbResizeZ.Value / 100;
                txtResizeZ.Text = hsbResizeZ.Value.ToString();
                EditedPModel.resizeZ = rszZPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void TxtResizeX_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtResizeX.Text, out int iResizeX))
            {
                if (iResizeX < 0 || iResizeX > 500)
                {
                    iResizeX = 100;
                }
            }
            else
            {
                txtResizeX.Text = "100";
                iResizeX = 100;
            }

            hsbResizeX.Value = iResizeX;
        }

        private void TxtResizeY_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtResizeY.Text, out int iResizeY))
            {
                if (iResizeY < 0 || iResizeY > 500)
                {
                    iResizeY = 100;
                }
            }
            else
            {
                txtResizeY.Text = "100";
                iResizeY = 100;
            }

            hsbResizeY.Value = iResizeY;
        }

        private void TxtResizeZ_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtResizeZ.Text, out int iResizeZ))
            {
                if (iResizeZ < 0 || iResizeZ > 500)
                {
                    iResizeZ = 100;
                }
            }
            else
            {
                txtResizeZ.Text = "100";
                iResizeZ = 100;
            }

            hsbResizeZ.Value = iResizeZ;
        }

        private void HsbRepositionX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                repGroupXPE = hsbRepositionX.Value / (100 / fBattleLocationGroupScale);
                txtRepositionX.Text = hsbRepositionX.Value.ToString();
                EditedPModel.Groups[SelectedGroup].repGroupX = repGroupXPE;
            }
            else
            {
                repXPE = hsbRepositionX.Value / 100f;
                txtRepositionX.Text = hsbRepositionX.Value.ToString();
                EditedPModel.repositionX = repXPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void HsbRepositionY_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                repGroupYPE = hsbRepositionY.Value / (100 / fBattleLocationGroupScale);
                txtRepositionY.Text = hsbRepositionY.Value.ToString();
                EditedPModel.Groups[SelectedGroup].repGroupY = repGroupYPE;
            }
            else
            {
                repYPE = hsbRepositionY.Value / 100f;
                txtRepositionY.Text = hsbRepositionY.Value.ToString();
                EditedPModel.repositionY = repYPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void HsbRepositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                repGroupZPE = hsbRepositionZ.Value / (100 / fBattleLocationGroupScale);
                txtRepositionZ.Text = hsbRepositionZ.Value.ToString();
                EditedPModel.Groups[SelectedGroup].repGroupZ = repGroupZPE;
            }
            else
            {
                repZPE = hsbRepositionZ.Value / 100f;
                txtRepositionZ.Text = hsbRepositionZ.Value.ToString();
                EditedPModel.repositionZ = repZPE;
            }

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void TxtRepositionX_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRepositionX.Text, out int iRepositionX))
            {
                if (iRepositionX < -500 * fBattleLocationGroupScale || 
                    iRepositionX > 500 * fBattleLocationGroupScale)
                {
                    iRepositionX = 0;
                }
            }
            else
            {
                txtRepositionX.Text = "0";
                iRepositionX = 0;
            }

            hsbRepositionX.Value = iRepositionX;
        }

        private void TxtRepositionY_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRepositionY.Text, out int iRepositionY))
            {
                if (iRepositionY < -500 * fBattleLocationGroupScale || 
                    iRepositionY > 500 * fBattleLocationGroupScale)
                {
                    iRepositionY = 0;
                }
            }
            else
            {
                txtRepositionY.Text = "0";
                iRepositionY = 0;
            }

            hsbRepositionY.Value = iRepositionY;
        }

        private void TxtRepositionZ_TextChanged(object sender, EventArgs e)
        {
            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRepositionZ.Text, out int iRepositionZ))
            {
                if (iRepositionZ < -500 * fBattleLocationGroupScale || 
                    iRepositionZ > 500 * fBattleLocationGroupScale)
                {
                    iRepositionZ = 0;
                }
            }
            else
            {
                txtRepositionZ.Text = "0";
                iRepositionZ = 0;
            }

            hsbRepositionZ.Value = iRepositionZ;
        }

        public void RotationModifiersChanged()
        {
            if (loadingModifiersQ) return;

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            if (SelectedGroup != -1)
            {
                RotatePModelGroupModifiers(ref EditedPModel.Groups[SelectedGroup],
                                           (float)(hsbRotateAlpha.Value / fBattleLocationGroupScale), 
                                           (float)(hsbRotateBeta.Value / fBattleLocationGroupScale),
                                           (float)(hsbRotateGamma.Value / fBattleLocationGroupScale));
            }
            else
            {
                RotatePModelModifiers(ref EditedPModel,
                                      hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value);
            }

            txtRotateAlpha.Text = hsbRotateAlpha.Value.ToString();
            txtRotateBeta.Text = hsbRotateBeta.Value.ToString();
            txtRotateGamma.Text = hsbRotateGamma.Value.ToString();

            PanelEditorPModel_Paint(null, null);
            DoNotAddPEStateQ = false;
        }

        private void HsbRotateAlpha_ValueChanged(object sender, EventArgs e)
        {
            if (!bGlobalChangeGroup) RotationModifiersChanged();
        }

        private void HsbRotateBeta_ValueChanged(object sender, EventArgs e)
        {
            if (!bGlobalChangeGroup) RotationModifiersChanged();
        }

        private void HsbRotateGamma_ValueChanged(object sender, EventArgs e)
        {
            if (!bGlobalChangeGroup) RotationModifiersChanged();
        }

        private void TxtRotateAlpha_TextChanged(object sender, EventArgs e)
        {

            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRotateAlpha.Text, out int iRotateAlpha))
            {
                if (iRotateAlpha < 0 || iRotateAlpha > 360 * fBattleLocationGroupScale)
                {
                    iRotateAlpha = 0;
                }
            }
            else
            {
                txtRotateAlpha.Text = "0";
                iRotateAlpha = 0;
            }

            hsbRotateAlpha.Value = iRotateAlpha;
        }

        private void TxtRotateBeta_TextChanged(object sender, EventArgs e)
        {

            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRotateBeta.Text, out int iRotateBeta))
            {
                if (iRotateBeta < 0 || iRotateBeta > 360 * fBattleLocationGroupScale)
                {
                    iRotateBeta = 0;
                }
            }
            else
            {
                txtRotateBeta.Text = "0";
                iRotateBeta = 0;
            }

            hsbRotateBeta.Value = iRotateBeta;
        }

        private void TxtRotateGamma_TextChanged(object sender, EventArgs e)
        {

            if (loadingModifiersQ) return;

            if (Int32.TryParse(txtRotateGamma.Text, out int iRotateGamma))
            {
                if (iRotateGamma < 0 || iRotateGamma > 360 * fBattleLocationGroupScale)
                {
                    iRotateGamma = 0;
                }
            }
            else
            {
                txtRotateGamma.Text = "0";
                iRotateGamma = 0;
            }

            hsbRotateGamma.Value = iRotateGamma;
        }

        private void BtnUpGroup_Click(object sender, EventArgs e)
        {
            int iGroupIdx, i;
            PGroup[] tmpGroups;
            PHundret[] tmpHundret;

            //  Get the group selected in p model
            iGroupIdx = lbGroups.SelectedIndex;

            if (iGroupIdx > 0)
            {
                // Prepare the new .P Model
                tmpGroups = new PGroup[EditedPModel.Header.numGroups];
                tmpHundret = new PHundret[EditedPModel.Header.numGroups];

                //  Here we will reorder the groups/hundrets of the .P Model
                for (i = 0; i < EditedPModel.Header.numGroups; i++)
                {
                    if (i == iGroupIdx - 1)
                    {
                        tmpGroups[i] = EditedPModel.Groups[iGroupIdx];
                        tmpHundret[i] = EditedPModel.Hundrets[iGroupIdx];
                    }
                    else
                    {
                        if (i == iGroupIdx)
                        {
                            tmpGroups[i] = EditedPModel.Groups[i - 1];
                            tmpHundret[i] = EditedPModel.Hundrets[i - 1];
                        }
                        else
                        {
                            tmpGroups[i] = EditedPModel.Groups[i];
                            tmpHundret[i] = EditedPModel.Hundrets[i];
                        }
                    }
                }

                //  Finally we need to reassign the ordered groups/hundrets to the edited P Model
                EditedPModel.Groups = tmpGroups;
                EditedPModel.Hundrets = tmpHundret;

                //  Refresh Groups List
                FillGroupsList();

                PanelEditorPModel_Paint(null, null);

                //  Let's select in listbox the record.
                lbGroups.SelectedIndex = iGroupIdx - 1;
            }
        }

        private void BtnDownGroup_Click(object sender, EventArgs e)
        {
            int iGroupIdx, i;
            PGroup[] tmpGroups;
            PHundret[] tmpHundret;

            //  Get the group selected in p model
            iGroupIdx = lbGroups.SelectedIndex;

            if (iGroupIdx > -1 && iGroupIdx < EditedPModel.Header.numGroups - 1)
            {
                // Prepare the new .P Model
                tmpGroups = new PGroup[EditedPModel.Header.numGroups];
                tmpHundret = new PHundret[EditedPModel.Header.numGroups];

                //  Here we will reorder the groups of the .P Model
                for (i = 0; i < EditedPModel.Header.numGroups; i++)
                {
                    if (i == iGroupIdx + 1)
                    {
                        tmpGroups[i] = EditedPModel.Groups[iGroupIdx];
                        tmpHundret[i] = EditedPModel.Hundrets[iGroupIdx];
                    }
                    else
                    {
                        if (i == iGroupIdx)
                        {
                            tmpGroups[i] = EditedPModel.Groups[i + 1];
                            tmpHundret[i] = EditedPModel.Hundrets[i + 1];
                        }
                        else
                        {
                            tmpGroups[i] = EditedPModel.Groups[i];
                            tmpHundret[i] = EditedPModel.Hundrets[i];
                        }
                    }
                }

                //  Now we need to reassign the ordered groups to the edited P Model
                EditedPModel.Groups = tmpGroups;
                EditedPModel.Hundrets = tmpHundret;

                //  Refresh Groups List
                FillGroupsList();

                PanelEditorPModel_Paint(null, null);

                //  Let's select in listbox the record.
                lbGroups.SelectedIndex = iGroupIdx + 1;
            }
        }

        private void BtnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (lbGroups.SelectedIndex == -1)
            {
                MessageBox.Show("There is not selected any .P Model group.", "Information", MessageBoxButtons.OK);
                return;
            }

            if (lbGroups.Items.Count < 2)
            {
                MessageBox.Show("A .P Model must have at least two groups for remove a group.", "Information", MessageBoxButtons.OK);
                return;
            }

            AddStateToBufferPE(this);

            VCountNewPoly = 0;
            RemoveGroup(ref EditedPModel, lbGroups.SelectedIndex);

            KillUnusedVertices(ref EditedPModel);

            //ApplyCurrentVCoordsPE(ref EditedPModel);

            ComputePColors(ref EditedPModel);
            ComputeEdges(ref EditedPModel);
            ComputeNormals(ref EditedPModel);

            FillGroupsList();
            
            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
            
            PanelEditorPModel_Paint(null, null);          
            ChkPalettized_CheckedChanged(null, null);

            ChangeGroupEnable(false);
            ChangeGroupStatus(false);
        }

        private void BtnDuplicateGroup_Click(object sender, EventArgs e)
        {
            if (lbGroups.SelectedIndex == -1)
            {
                MessageBox.Show("There is not selected any .P Model group.", "Information", MessageBoxButtons.OK);
                return;
            }

            AddStateToBufferPE(this);

            VCountNewPoly = 0;
            DuplicateGroup(ref EditedPModel, lbGroups.SelectedIndex);
            CheckModelConsistency(ref EditedPModel);

            KillUnusedVertices(ref EditedPModel);

            //ApplyCurrentVCoordsPE(ref EditedPModel);

            ComputePColors(ref EditedPModel);
            ComputeEdges(ref EditedPModel);
            ComputeNormals(ref EditedPModel);

            FillGroupsList();

            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);

            PanelEditorPModel_Paint(null, null);
            ChkPalettized_CheckedChanged(null, null);

            ChangeGroupEnable(false);
            ChangeGroupStatus(false);
        }

        private void BtnGroupProperties_Click(object sender, EventArgs e)
        {
            if (lbGroups.SelectedIndex > -1)
            {
                // Instantiate other forms
                frmGroupProp = new FrmGroupProperties(this, lbGroups.SelectedIndex);
                frmGroupProp.ShowDialog();
            }
        }

        public void ChangeGroupEnable(bool bEnabled)
        {
            if (!bEnabled)
            {
                lbGroups.SelectedIndex = -1;
                SelectedGroup = -1;
            }

            btnUpGroup.Enabled = bEnabled;
            btnDownGroup.Enabled = bEnabled;

            btnRemoveGroup.Enabled = bEnabled;
            btnDuplicateGroup.Enabled = bEnabled;
            btnGroupProperties.Enabled = bEnabled;
            btnHideShowGroup.Enabled = bEnabled;
        }

        private void LbGroups_Click(object sender, EventArgs e)
        {
            if (lbGroups.Items.Count > 0)
            {
                if (lbGroups.SelectedIndex != -1 && lbGroups.SelectedIndex != SelectedGroup)
                {
                    SelectedGroup = lbGroups.SelectedIndex;
                    ChangeGroupEnable(true);
                    ChangeGroupStatus(true);
                }
            }
        }

        private void LbGroups_DoubleClick(object sender, EventArgs e)
        {
            btnGroupProperties.PerformClick();
        }

        private void BtnHideShowGroup_Click(object sender, EventArgs e)
        {
            int iSelIdx;

            if (lbGroups.Items.Count > 0 && lbGroups.SelectedIndex > -1)
            {
                iSelIdx = lbGroups.SelectedIndex;

                AddStateToBufferPE(this);

                EditedPModel.Groups[lbGroups.SelectedIndex].HiddenQ =
                    !EditedPModel.Groups[lbGroups.SelectedIndex].HiddenQ;

                FillGroupsList();

                lbGroups.SelectedIndex = iSelIdx;
                PanelEditorPModel_Paint(null, null);
                ChkPalettized_CheckedChanged(null, null);
            }
        }

        private void BtnCommitChanges_Click(object sender, EventArgs e)
        {
            // Apply changes to the actual EditedPModel local PEditor variable.
            ComputeBoundingBox(ref EditedPModel);

            CommitContextualizedPChanges(false);

            // Apply changes to the Skeleton in frmSkeletonEditor (fSkeleton, bSkeleton, fPModel)
            switch (modelType)
            {
                case K_HRC_SKELETON:
                    FieldRSDResource tmpRSDResourceModel;

                    tmpRSDResourceModel = fSkeleton.bones[EditedBone].fRSDResources[EditedBonePiece];
                    tmpRSDResourceModel.Model = CopyPModel(EditedPModel);
                    fSkeleton.bones[EditedBone].fRSDResources[EditedBonePiece] = tmpRSDResourceModel;

                    CreateDListsFromFieldSkeleton(ref fSkeleton);

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    PModel tmpPModel;

                    if (EditedBone == bSkeleton.nBones)
                    {
                        bSkeleton.wpModels[ianimWeaponIndex] = CopyPModel(EditedPModel);

                        tmpPModel = bSkeleton.wpModels[ianimWeaponIndex];
                        CreateDListsFromPModel(ref tmpPModel);
                        bSkeleton.wpModels[ianimWeaponIndex] = tmpPModel;
                    }
                    else
                    {
                        bSkeleton.bones[EditedBone].Models[EditedBonePiece] = CopyPModel(EditedPModel);
                        CreateDListsFromBattleSkeleton(ref bSkeleton);
                    }
                    break;

                case K_P_BATTLE_MODEL:
                case K_P_FIELD_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    fPModel = CopyPModel(EditedPModel);
                    CreateDListsFromPModel(ref fPModel);

                    break;
            }

            // Update main title window
            bChangesDone = true;
            frmSkelEdit.UpdateMainSkeletonWindowTitle();
            frmSkelEdit.SetBonePieceModifiersPEditor();

            frmSkelEdit.PanelModel_Paint(null, null);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RbPaintPolygon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_PAINT;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_PAINT;
                }
                else
                {
                    ternaryFunc = K_PAINT;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbCutEdge_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_CUT_EDGE;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_CUT_EDGE;
                }
                else
                {
                    ternaryFunc = K_CUT_EDGE;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbErasePolygon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_ERASE_POLY;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_ERASE_POLY;
                }
                else
                {
                    ternaryFunc = K_ERASE_POLY;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbMoveVertex_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_PICK_VERTEX;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_PICK_VERTEX;
                }
                else
                {
                    ternaryFunc = K_PICK_VERTEX;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbFreeRotate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_ROTATE;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_ROTATE;
                }
                else
                {
                    ternaryFunc = K_ROTATE;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbZoomInOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_ZOOM;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_ZOOM;
                }
                else
                {
                    ternaryFunc = K_ZOOM;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbPanning_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_PAN;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_PAN;
                }
                else
                {
                    ternaryFunc = K_PAN;
                }
            }

            SetFunctionButtonColors();
        }

        private void RbNewPolygon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryFunc = K_NEW_POLY;
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    secondaryFunc = K_NEW_POLY;
                }
                else
                {
                    ternaryFunc = K_NEW_POLY;
                }
            }

            SetFunctionButtonColors();
        }


        private void LoadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tmpFileName;

            // Set filter options and filter index.
            openFile.Title = "Open Model (PEditor)";
            openFile.Filter = "FF7 Field Model|*.P|FF7 Battle Model (*.*)|*.*|FF7 Magic Model|*.P??|FF7 3DS Model|*.3DS|All files|*.*";

            if (iPEFilterIdx == -1)
            {
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
            }
            else openFile.FilterIndex = iPEFilterIdx;

            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathPModelFolderPE != null)
            {
                openFile.InitialDirectory = strGlobalPathPModelFolderPE;
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
                        // Set Global Paths and save them
                        strGlobalPathPModelFolderPE = Path.GetDirectoryName(openFile.FileName);
                        strGlobalPModelNamePE = Path.GetFileName(openFile.FileName).ToUpper();

                        iPEFilterIdx = openFile.FilterIndex;

                        WriteCFGFile();

                        // Load the Model (We need to check if we have a .3DS or a .P file
                        tmpFileName = EditedPModel.fileName;

                        DestroyPModelResources(ref EditedPModel);
                        EditedPModel = new PModel();

                        if (Path.GetExtension(openFile.FileName).ToUpper() == ".3DS")
                        {
                            Load3DS(openFile.FileName, out Model3DS[] tmpModel3DS);
                            ConvertModels3DSToPModel(tmpModel3DS, ref EditedPModel, bAdjust3DSImport);
                        }
                        else
                        {
                            LoadPModel(ref EditedPModel, strGlobalPathPModelFolderPE,
                                       Path.GetFileName(strGlobalPModelNamePE),
                                       true);
                        }

                        // Assign old filename to the PModel
                        EditedPModel.fileName = tmpFileName.ToUpper();

                        if (EditedPModel.Header.numVerts > 0)
                        {
                            // Initialize environment and model
                            InitializeLoadPEditor();

                            ComputeBoundingBox(ref EditedPModel);

                            FillGroupsList();

                            CommitContextualizedPChanges(false);

                            ResetCameraPE();
                            ResetPlane();

                            PanelEditorPModel_Paint(null, null);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error opening Model file " + openFile.FileName.ToUpper() + " in the P Editor.",
                                "Error");
                return;
            }
        }

        private void LoadModelAsNewGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PModel GroupModel;

            // Set filter options and filter index.
            openFile.Title = "Open Model as new Group (PEditor)";
            openFile.Filter = "FF7 Field Model|*.P|FF7 Battle Model (*.*)|*.*|FF7 Magic Model|*.P??|FF7 3DS Model|*.3DS|All files|*.*";

            if (iPEFilterIdx == -1)
            {
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
            }
            else openFile.FilterIndex = iPEFilterIdx;

            openFile.FileName = null;

            // Check Initial Directory
            if (strGlobalPathPModelFolderPE != null)
            {
                openFile.InitialDirectory = strGlobalPathPModelFolderPE;
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
                        // Set Global Paths and save them
                        strGlobalPathPModelFolderPE = Path.GetDirectoryName(openFile.FileName);
                        strGlobalPModelNamePE = Path.GetFileName(openFile.FileName).ToUpper();

                        iPEFilterIdx = openFile.FilterIndex;

                        WriteCFGFile();

                        // Load the Model (We need to check if we have a .3DS or a .P file
                        GroupModel = new PModel();

                        if (Path.GetExtension(openFile.FileName).ToUpper() == ".3DS")
                        {
                            Load3DS(openFile.FileName, out Model3DS[] tmpModel3DS);
                            ConvertModels3DSToPModel(tmpModel3DS, ref GroupModel, bAdjust3DSImport);
                        }
                        else
                        {
                            LoadPModel(ref GroupModel, strGlobalPathPModelFolderPE,
                                       Path.GetFileName(strGlobalPModelNamePE),
                                       false);

                        }                     

                        if (GroupModel.Header.numVerts > 0)
                        {
                            AddStateToBufferPE(this);

                            VCountNewPoly = 0;

                            int iGroupIdx = 0, iEditedPModelGroupIdx = EditedPModel.Groups.Length;
                            foreach (PGroup itmGroup in GroupModel.Groups)
                            {
                                // We will add the group having in mind if it has texFlag or not
                                if (itmGroup.texFlag == 0)
                                {
                                    AddGroup(ref EditedPModel,
                                             GroupModel.Verts.Skip(itmGroup.offsetVert).Take(itmGroup.numVert).ToArray(),
                                             GroupModel.Polys.Skip(itmGroup.offsetPoly).Take(itmGroup.numPoly).ToArray(),
                                             null,
                                             GroupModel.Vcolors.Skip(itmGroup.offsetVert).Take(itmGroup.numVert).ToArray(),
                                             GroupModel.Pcolors.Skip(itmGroup.offsetPoly).Take(itmGroup.numPoly).ToArray(),
                                             0);

                                }
                                else
                                {
                                    AddGroup(ref EditedPModel,
                                             GroupModel.Verts.Skip(itmGroup.offsetVert).Take(itmGroup.numVert).ToArray(),
                                             GroupModel.Polys.Skip(itmGroup.offsetPoly).Take(itmGroup.numPoly).ToArray(),
                                             GroupModel.TexCoords.Skip(itmGroup.offsetTex).Take(itmGroup.numVert).ToArray(),
                                             GroupModel.Vcolors.Skip(itmGroup.offsetVert).Take(itmGroup.numVert).ToArray(),
                                             GroupModel.Pcolors.Skip(itmGroup.offsetPoly).Take(itmGroup.numPoly).ToArray(),
                                             itmGroup.texID);

                                    EditedPModel.Groups[iEditedPModelGroupIdx].texID = GroupModel.Groups[iGroupIdx].texID;
                                    EditedPModel.Hundrets[iEditedPModelGroupIdx] = CopyPHundret(GroupModel.Hundrets[iGroupIdx]);
                                }

                                // We need to adjust the 3DS here also, because we are adding a new
                                // Group to the main .P EditedPModel, and the rotGroupGamma would be 0.
                                if (Path.GetExtension(openFile.FileName).ToUpper() == ".3DS" &&
                                    bAdjust3DSImport)
                                {
                                    EditedPModel.Groups[iEditedPModelGroupIdx].rotGroupGamma = 180;
                                }

                                iGroupIdx++;
                                iEditedPModelGroupIdx++;

                            }

                            DestroyPModelResources(ref GroupModel);

                            FillGroupsList();

                            CommitContextualizedPChanges(false);


                            // Let's clear selected groups of lbGroups listbox
                            SelectedGroup = -1;
                            lbGroups.SelectedIndex = -1;
                            ChangeGroupEnable(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("Error opening Model file " + openFile.FileName.ToUpper() + " as new Group in the P Editor.",
                                "Error");
                return;
            }
        }

        private void SaveModelAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set filter options and filter index.
            saveFile.Title = "Save Model As... (PEditor)";
            saveFile.Filter = "FF7 Field Model|*.P|FF7 Battle Model (*.*)|*.*|FF7 Magic Model (*.P??)|*.P??|All files|*.*";

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

            if (strGlobalPathSaveModelFolderPE == "")
            {
                if (strGlobalPathPModelFolderPE == "") strGlobalPathPModelFolderPE = strGlobalPath;

                strGlobalPathSaveModelFolderPE = strGlobalPathPModelFolderPE;               
            }

            saveFile.FileName = EditedPModel.fileName.ToUpper();

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (loadedPModel)
                    {
                        // I don't think it is needed when saving
                        //AddStateToBuffer(this);

                        strGlobalPathSaveModelFolderPE = Path.GetDirectoryName(saveFile.FileName);
                        saveFile.FileName = strGlobalPathSaveModelFolderPE + "\\" + Path.GetFileName(saveFile.FileName).ToUpper();

                        // We save the Model.
                        WriteGlobalPModel(ref EditedPModel, saveFile.FileName);

                        MessageBox.Show("Model part " + Path.GetFileName(saveFile.FileName).ToUpper() + " of P Editor saved.",
                                        "Information");

                        WriteCFGFile();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving .P Model as... " + Path.GetFileName(saveFile.FileName) + " in the P Editor.",
                                "Error");
                return;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnClose.PerformClick();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPlane();

            PanelEditorPModel_Paint(null, null);
        }

        private void InvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nudAlphaPlane.Value = (nudAlphaPlane.Value + 180) % 360;
            //  nudBetaPlane.Value = (nudAlphaPlane.Value + 180) % 360;  -- Commented in KimeraVB6
        }

        private void MirrorModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                MirrorHemisphere(ref EditedPModel, planeA, planeB, planeC, planeD);

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                ComputeNormals(ref EditedPModel);

                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                PanelEditorPModel_Paint(null, null);
            } 
        }

        private void MakeModelSymmetricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point3D> knownPlaneVPoints = new List<Point3D>();

            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                CutPModelThroughPlane(ref EditedPModel, planeA, planeB, planeC, planeD, ref knownPlaneVPoints);
                EraseHemisphereVertices(ref EditedPModel, planeA, planeB, planeC, planeD, false, ref knownPlaneVPoints);
                DuplicateMirrorHemisphere(ref EditedPModel, planeA, planeB, planeC, planeD);
                CheckModelConsistency(ref EditedPModel);

                FillGroupsList();

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                KillUnusedVertices(ref EditedPModel);

                ApplyCurrentVCoordsPE(ref EditedPModel);

                ComputePColors(ref EditedPModel);
                ComputeEdges(ref EditedPModel);
                ComputeNormals(ref EditedPModel);

                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                PanelEditorPModel_Paint(null, null);
            }
        }

        private void DeleteAllPolysSelectedColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iTable, iPolyIdx;

            if (iSelectedColor > -1)
            {
                AddStateToBufferPE(this);
                iPolyIdx = 0;

                for (iTable = 0; iTable < translationTablePolys.Length; iTable++)
                {
                    if (translationTablePolys[iTable].I == iSelectedColor)
                    {
                        if (EditedPModel.Header.numPolys > 1)
                        {
                            RemovePolygon(ref EditedPModel, iPolyIdx);
                        }
                    }
                    else
                        iPolyIdx++;
                }

                KillUnusedVertices(ref EditedPModel);

                RepairGroups(ref EditedPModel);
                FillGroupsList();

                ComputeNormals(ref EditedPModel);
                ComputeEdges(ref EditedPModel);

                FillColorTable(EditedPModel, ref colorTable,
                               ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);

                DrawPalette(K_CLICK);

                PanelEditorPModel_Paint(null, null);
            }
        }

        private void DeleteAllPolysnotSelectedColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iTable, iPolyIdx;

            if (iSelectedColor > -1)
            {
                AddStateToBufferPE(this);
                iPolyIdx = 0;

                for (iTable = 0; iTable < translationTablePolys.Length; iTable++)
                {
                    if (translationTablePolys[iTable].I != iSelectedColor)
                    {
                        if (EditedPModel.Header.numPolys > 1)
                        {
                            RemovePolygon(ref EditedPModel, iPolyIdx);
                        }
                    }                       
                    else
                        iPolyIdx++;
                }

                KillUnusedVertices(ref EditedPModel);
                
                RepairGroups(ref EditedPModel);
                FillGroupsList();

                ComputeNormals(ref EditedPModel);
                ComputeEdges(ref EditedPModel);

                FillColorTable(EditedPModel, ref colorTable,
                               ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);

                DrawPalette(K_CLICK);

                PanelEditorPModel_Paint(null, null);
            }
        }

        ////////////////////////////////////////////////////////////////////
        // IMPORTANT
        //
        // Right now, as we use direct palette colors in the model (polygonal/vertex) we will not use this
        // old feature of KimeraVB6. I made NOT VISIBLE the menu item option in "Palette features", but
        // I will leave enabled the procedure.
        ////////////////////////////////////////////////////////////////////
        private void KillPrecalculatedLightningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                KillPrecalculatedLighting(EditedPModel, ref translationTableVertex);
                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);

                PanelEditorPModel_Paint(null, null);
                PbPalette_Paint(null, null);
            }
        }

        private void ChkPalettized_CheckedChanged(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                if (!chkPaletteMode.Checked)
                {
                    hsbThresholdSlider.Enabled = false;
                    txtThresholdSlider.Enabled = false;

                    if (!bColorsChanged)
                        CopyVPColors2Model(ref EditedPModel, vcolorsOriginal, pcolorsOriginal);

                    bColorsChanged = false;
                    iSelectedColor = -1;
                    deleteAllPolysSelectedColorToolStripMenuItem.Enabled = false;
                    deleteAllPolysnotSelectedColorToolStripMenuItem.Enabled = false;
                    killPrecalculatedLightningToolStripMenuItem.Enabled = false;
                }
                else
                {
                    hsbThresholdSlider.Enabled = true;
                    txtThresholdSlider.Enabled = true;

                    CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);

                    FillColorTable(EditedPModel, ref colorTable,
                                   ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
                    ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);

                    deleteAllPolysSelectedColorToolStripMenuItem.Enabled = true;
                    deleteAllPolysnotSelectedColorToolStripMenuItem.Enabled = true;
                    killPrecalculatedLightningToolStripMenuItem.Enabled = true;
                }

                DrawPalette(K_CLICK);
                bColorsChanged = false;
            }
        }

        private void FrmPEditor_Load(object sender, EventArgs e)
        {
            bLoading = true;

            //   Some initial settings to reduce flickering of the screen
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //// Set Minimum Size (using the property sometimes changes auto in designer)
            //// Init also Width/Height as per CFG values
            this.MinimumSize = new Size(736, 586);
            this.Size = new Size(isizeWindowWidthPE, isizeWindowHeightPE);

            if (iwindowPosXPE <= 0 && iwindowPosYPE <= 0) this.CenterToScreen();
            else this.Location = new Point(iwindowPosXPE, iwindowPosYPE);

            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;

            // Define CTRL+Home shortcut for reset camera feature ("Home" key is not present in my visual studio notebook? is a VS bug?
            ResetCameraToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Home;

            // Scale for different magnitudes when model is from battle location
            if (bSkeleton.IsBattleLocation) fBattleLocationGroupScale = F_BATTLELOCATION_SCALE;
            else fBattleLocationGroupScale = 1f;

            hsbRotateAlpha.Maximum = (int)(360 * fBattleLocationGroupScale);
            hsbRotateBeta.Maximum = (int)(360 * fBattleLocationGroupScale);
            hsbRotateGamma.Maximum = (int)(360 * fBattleLocationGroupScale);

            hsbRepositionX.Maximum = (int)(500 * fBattleLocationGroupScale);
            hsbRepositionX.Minimum = (int)(-500 * fBattleLocationGroupScale);
            hsbRepositionY.Maximum = (int)(500 * fBattleLocationGroupScale);
            hsbRepositionY.Minimum = (int)(-500 * fBattleLocationGroupScale);
            hsbRepositionZ.Maximum = (int)(500 * fBattleLocationGroupScale);
            hsbRepositionZ.Minimum = (int)(-500 * fBattleLocationGroupScale);

            InitializeLoadPEditor();

            bLoading = false;
        }

        private void FrmPEditor_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            if (loadedPModel && this.Visible)
            {
                iwindowPosXPE = this.Location.X;
                iwindowPosYPE = this.Location.Y;
                WriteCFGFile();
            }
        }

        private void PbPalette_Paint(object sender, PaintEventArgs e)
        {
            DrawPalette(K_CLICK);
        }

        private void PbPalette_MouseDown(object sender, MouseEventArgs e)
        {
            int xc, yc, xPos, yPos;
            Color cColor;

            xPos = e.X;
            yPos = e.Y;

            if  (loadedPModel)
            {
                if (chkPaletteMode.Checked)
                {
                    loadingColorModifiersQ = true;

                    int numColorCols, numColorRows, szColorRowsHeight, szColorColsWidth;

                    numColorCols = Convert.ToInt32(Math.Sqrt(colorTable.Count));
                    numColorRows = Convert.ToInt32(Math.Ceiling(colorTable.Count / (double)numColorCols));

                    szColorRowsHeight = Convert.ToInt32(pbPalette.ClientRectangle.Height / numColorRows);
                    szColorColsWidth = Convert.ToInt32(pbPalette.ClientRectangle.Width / numColorCols);

                    yc = (int)Math.Floor((float)yPos / szColorRowsHeight);
                    xc = (int)Math.Floor((float)xPos / szColorColsWidth);

                    iSelectedColor = yc * numColorCols + xc;

                    if (iSelectedColor < colorTable.Count) 
                    {
                        hsbSelectedColorR.Value = colorTable[iSelectedColor].R;
                        hsbSelectedColorG.Value = colorTable[iSelectedColor].G;
                        hsbSelectedColorB.Value = colorTable[iSelectedColor].B;
                    }
                    else iSelectedColor = -1;
                }
                else
                {
                    if ((xPos <= 0 || xPos >= pbPalette.ClientRectangle.Width - 1) ||
                        (yPos <= 0 || yPos >= pbPalette.ClientRectangle.Height - 1))
                    {
                        hsbSelectedColorR.Value = 255;
                        hsbSelectedColorG.Value = 255;
                        hsbSelectedColorB.Value = 255;
                    }
                    else
                    {
                        cColor = bmpFullGradientPalette.GetPixel(xPos, yPos);

                        hsbSelectedColorR.Value = cColor.R;
                        hsbSelectedColorG.Value = cColor.G;
                        hsbSelectedColorB.Value = cColor.B;
                    }
                }

                txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
                txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
                txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();

                DrawPalette(K_CLICK);

                loadingColorModifiersQ = false;
            }
        }

        private void PbPalette_MouseMove(object sender, MouseEventArgs e)
        {
            int xc, yc, xPos, yPos;
            Color cColor;

            xPos = e.X;
            yPos = e.Y;

            if (loadedPModel && e.Button != MouseButtons.None)
            {
                loadingColorModifiersQ = true;

                if (chkPaletteMode.Checked)
                {

                    int numColorCols, numColorRows, szColorRowsHeight, szColorColsWidth;
 
                    numColorCols = Convert.ToInt32(Math.Sqrt(colorTable.Count));
                    numColorRows = Convert.ToInt32(Math.Ceiling(colorTable.Count / (double)numColorCols));

                    szColorRowsHeight = Convert.ToInt32(pbPalette.ClientRectangle.Height / numColorRows);
                    szColorColsWidth = Convert.ToInt32(pbPalette.ClientRectangle.Width / numColorCols);

                    yc = (int)Math.Floor((float)yPos / szColorRowsHeight);
                    xc = (int)Math.Floor((float)xPos / szColorColsWidth);

                    iSelectedColor = yc * numColorCols + xc;

                    if (iSelectedColor < colorTable.Count && iSelectedColor > -1)
                    {
                        hsbSelectedColorR.Value = colorTable[iSelectedColor].R;
                        hsbSelectedColorG.Value = colorTable[iSelectedColor].G;
                        hsbSelectedColorB.Value = colorTable[iSelectedColor].B;
                    }
                    else iSelectedColor = -1;
                }
                else
                {
                    if (e.Y > pbPalette.ClientRectangle.Height - 16) return;

                    loadingColorModifiersQ = true;
                    if ((xPos <= 0 || xPos >= pbPalette.ClientRectangle.Width - 1) ||
                        (yPos <= 0 || yPos >= pbPalette.ClientRectangle.Height - 1))
                    {
                        hsbSelectedColorR.Value = 255;
                        hsbSelectedColorG.Value = 255;
                        hsbSelectedColorB.Value = 255;
                    }
                    else
                    {
                        cColor = bmpFullGradientPalette.GetPixel(xPos, yPos);

                        hsbSelectedColorR.Value = cColor.R;
                        hsbSelectedColorG.Value = cColor.G;
                        hsbSelectedColorB.Value = cColor.B;
                    }
                }

                txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
                txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
                txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();

                DrawPalette(K_MOVE);

                loadingColorModifiersQ = false;
            }
        }

        private void HsbSelectedColorR_ValueChanged(object sender, EventArgs e)
        {
            pbPaletteColor.BackColor = Color.FromArgb(255,
                                                      hsbSelectedColorR.Value,
                                                      hsbSelectedColorG.Value,
                                                      hsbSelectedColorB.Value);

            if (loadingColorModifiersQ) return;

            if (iSelectedColor > -1)
            {
                if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
                DoNotAddPEStateQ = true;

                colorTable[iSelectedColor] = Color.FromArgb(255,
                                                            hsbSelectedColorR.Value,
                                                            colorTable[iSelectedColor].G,
                                                            colorTable[iSelectedColor].B);

                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                bColorsChanged = true;

                PanelEditorPModel_Paint(null, null);
                DoNotAddPEStateQ = false;
            }

            txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
            DrawPalette(K_MOVE);
        }

        private void HsbSelectedColorG_ValueChanged(object sender, EventArgs e)
        {
            pbPaletteColor.BackColor = Color.FromArgb(255,
                                                      hsbSelectedColorR.Value,
                                                      hsbSelectedColorG.Value,
                                                      hsbSelectedColorB.Value);

            if (loadingColorModifiersQ) return;

            if (iSelectedColor > -1)
            {
                if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
                DoNotAddPEStateQ = true;

                colorTable[iSelectedColor] = Color.FromArgb(255,
                                                            colorTable[iSelectedColor].R,
                                                            hsbSelectedColorG.Value,
                                                            colorTable[iSelectedColor].B);

                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                bColorsChanged = true;

                PanelEditorPModel_Paint(null, null);
                DoNotAddPEStateQ = false;
            }

            txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
            DrawPalette(K_MOVE);
        }

        private void HsbSelectedColorB_ValueChanged(object sender, EventArgs e)
        {
            pbPaletteColor.BackColor = Color.FromArgb(255,
                                                      hsbSelectedColorR.Value,
                                                      hsbSelectedColorG.Value,
                                                      hsbSelectedColorB.Value);

            if (loadingColorModifiersQ) return;

            if (iSelectedColor > -1)
            {
                if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
                DoNotAddPEStateQ = true;

                colorTable[iSelectedColor] = Color.FromArgb(255,
                                                            colorTable[iSelectedColor].R,
                                                            colorTable[iSelectedColor].G,
                                                            hsbSelectedColorB.Value);

                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                bColorsChanged = true;

                PanelEditorPModel_Paint(null, null);
                DoNotAddPEStateQ = false;
            }

            txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();
            DrawPalette(K_MOVE);
        }

        private void HsbThresholdSlider_ValueChanged(object sender, EventArgs e)
        {
            iThreshold = (byte)hsbThresholdSlider.Value;

            if (!bColorsChanged) CopyVPColors2Model(ref EditedPModel, vcolorsOriginal, pcolorsOriginal);
            else
            {
                if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
                DoNotAddPEStateQ = true;

                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                bColorsChanged = false;
            }

            FillColorTable(EditedPModel, ref colorTable,
                           ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
            ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);

            PanelEditorPModel_Paint(null, null);
            PbPalette_Paint(null, null);
            txtThresholdSlider.Text = hsbThresholdSlider.Value.ToString();
            DoNotAddPEStateQ = false;
        }

        private void TxtSelectedColorR_TextChanged(object sender, EventArgs e)
        {
            if (loadingColorModifiersQ) return;

            string oldSelColR = hsbSelectedColorR.Value.ToString();

            if (Int32.TryParse(txtSelectedColorR.Text, out int iColorR))
            {
                if (iColorR >= 0 && iColorR <= 255)
                    hsbSelectedColorR.Value = iColorR;
                else
                    txtSelectedColorR.Text = oldSelColR;
            }
            else
                txtSelectedColorR.Text = oldSelColR;
        }

        private void TxtSelectedColorG_TextChanged(object sender, EventArgs e)
        {
            if (loadingColorModifiersQ) return;

            string oldSelColG = hsbSelectedColorG.Value.ToString();

            if (Int32.TryParse(txtSelectedColorG.Text, out int iColorG))
            {
                if (iColorG >= 0 && iColorG <= 255)
                    hsbSelectedColorG.Value = iColorG;
                else
                    txtSelectedColorG.Text = oldSelColG;
            }
            else
                txtSelectedColorG.Text = oldSelColG;
        }

        private void TxtSelectedColorB_TextChanged(object sender, EventArgs e)
        {
            if (loadingColorModifiersQ) return;

            string oldSelColB = hsbSelectedColorB.Value.ToString();

            if (Int32.TryParse(txtSelectedColorB.Text, out int iColorB))
            {
                if (iColorB >= 0 && iColorB <= 255)
                    hsbSelectedColorB.Value = iColorB;
                else
                    txtSelectedColorB.Text = oldSelColB;
            }
            else
                txtSelectedColorB.Text = oldSelColB;
        }

        private void TxtThresholdSlider_TextChanged(object sender, EventArgs e)
        {

            string oldThreshold = hsbThresholdSlider.Value.ToString();

            if (Int32.TryParse(txtThresholdSlider.Text, out int iLocalThreshold))
            {
                if (iLocalThreshold >= 0 && iLocalThreshold <= 255)
                    hsbThresholdSlider.Value = iLocalThreshold;
                else
                    txtThresholdSlider.Text = oldThreshold;
            }
            else
                txtThresholdSlider.Text = oldThreshold;
        }

        private void BtnMoreBrightness_Click(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                iBrightnessFactor += 5;
                //ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal, pcolorsOriginal);
                ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal);

                FillColorTable(EditedPModel, ref colorTable,
                               ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                //ApplyCurrentVColors(ref EditedPModel);
                //CommitContextualizedPChanges(false);

                PanelEditorPModel_Paint(null, null);
            }
        }

        private void BtnLessBrightness_Click(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                iBrightnessFactor -= 5;
                //ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal, pcolorsOriginal);
                ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal);

                FillColorTable(EditedPModel, ref colorTable,
                               ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                //ApplyCurrentVColors(ref EditedPModel);
                //CommitContextualizedPChanges(false);

                PanelEditorPModel_Paint(null, null);
            }
        }

        private void BtnResetBrightness_Click(object sender, EventArgs e)
        {
            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                iBrightnessFactor = 0;
                //ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal, pcolorsOriginal);
                ChangeBrightness(ref EditedPModel, iBrightnessFactor, vcolorsOriginal);

                FillColorTable(EditedPModel, ref colorTable,
                               ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
                ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
                //ApplyCurrentVColors(ref EditedPModel);
                //CommitContextualizedPChanges(false);

                PanelEditorPModel_Paint(null, null);
            }
        }

        public void UpdateColorValues()
        {
            loadingColorModifiersQ = true;

            hsbSelectedColorR.Value = colorTable[iSelectedColor].R;
            hsbSelectedColorG.Value = colorTable[iSelectedColor].G;
            hsbSelectedColorB.Value = colorTable[iSelectedColor].B;
            txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
            txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
            txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();

            loadingColorModifiersQ = false;
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoPE(this);

            PanelEditorPModel_Paint(null, null);
            PbPalette_Paint(null, null);

            if (iSelectedColor > -1) UpdateColorValues();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoPE(this);

            PanelEditorPModel_Paint(null, null);
            PbPalette_Paint(null, null);

            if (iSelectedColor > -1) UpdateColorValues();
        }

        private void UnselectGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedGroup = -1;
            lbGroups.SelectedIndex = -1;

            ChangeGroupEnable(false);
            ChangeGroupStatus(false);
        }

        private void MergeGroupsIntoOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedPModel.Header.numGroups > 1)
            {
                MergeGroupsIntoOne(EditedPModel, out EditedPModel, true);
                ComputeBoundingBox(ref EditedPModel);

                FillGroupsList();

                CommitContextualizedPChanges(false);
            }
        }

        private void MergeGroupsIntoOnenotTexturedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedPModel.Header.numGroups > 1)
            {
                MergeGroupsIntoOne(EditedPModel, out EditedPModel, false);
                ComputeBoundingBox(ref EditedPModel);

                FillGroupsList();

                CommitContextualizedPChanges(false);
            }
        }

        private void ResetCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCameraPE();

            PanelEditorPModel_Paint(null, null);
        }

        private void CutModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point3D> knownPlaneVPoints = new List<Point3D>();

            if (loadedPModel)
            {
                AddStateToBufferPE(this);
                
                CutPModelThroughPlane(ref EditedPModel, planeA, planeB, planeC, planeD, ref knownPlaneVPoints);

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                KillUnusedVertices(ref EditedPModel);

                ApplyCurrentVCoordsPE(ref EditedPModel);

                ComputePColors(ref EditedPModel);
                ComputeEdges(ref EditedPModel);
                ComputeNormals(ref EditedPModel);

                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                
                PanelEditorPModel_Paint(null, null);
            }
        }

        private void EraseLowerEmisphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point3D> knownPlaneVPoints = new List<Point3D>();

            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                CutPModelThroughPlane(ref EditedPModel, planeA, planeB, planeC, planeD, ref knownPlaneVPoints);
                EraseHemisphereVertices(ref EditedPModel, planeA, planeB, planeC, planeD, false, ref knownPlaneVPoints);

                FillGroupsList();

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                KillUnusedVertices(ref EditedPModel);

                ApplyCurrentVCoordsPE(ref EditedPModel);

                ComputePColors(ref EditedPModel);
                ComputeEdges(ref EditedPModel);
                ComputeNormals(ref EditedPModel);

                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                PanelEditorPModel_Paint(null, null);
            }
        }

        private void FattenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            double[] transInverse = new double[16];
            float min_x, min_y, min_z;
            float max_x, max_y, max_z;

            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                min_x = EditedPModel.BoundingBox.min_x;
                min_y = EditedPModel.BoundingBox.min_y;
                min_z = EditedPModel.BoundingBox.min_z;

                max_x = EditedPModel.BoundingBox.max_x;
                max_y = EditedPModel.BoundingBox.max_y;
                max_z = EditedPModel.BoundingBox.max_z;

                for (i = 0; i < 16; i++) transInverse[i] = planeTransformation[i];

                InvertMatrix(ref transInverse);
                ApplyPModelTransformation(ref EditedPModel, transInverse);
                ComputeBoundingBox(ref EditedPModel);

                FattenPModel(ref EditedPModel);
                ApplyPModelTransformation(ref EditedPModel, planeTransformation);

                EditedPModel.BoundingBox.min_x = min_x;
                EditedPModel.BoundingBox.min_y = min_y;
                EditedPModel.BoundingBox.min_z = min_z;

                EditedPModel.BoundingBox.max_x = max_x;
                EditedPModel.BoundingBox.max_y = max_y;
                EditedPModel.BoundingBox.max_z = max_z;

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                ComputeNormals(ref EditedPModel);
                PanelEditorPModel_Paint(null, null);
            }
        }

        private void FrmPEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bmpFullGradientPalette != null) bmpFullGradientPalette.Dispose();
            DestroyPModelResources(ref EditedPModel);

            DisableOpenGL(OGLContextPEditor);
            if (frmGroupProp != null) frmGroupProp.Dispose();
            this.Dispose();

            // Change sliders status
            frmSkelEdit.ChangeGroupBoxesStatusPEditor(true);

            frmSkelEdit.Activate();
            frmSkelEdit.PanelModel_Paint(null, null);
        }

        private void SlimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            double[] transInverse = new double[16];
            float min_x, min_y, min_z;
            float max_x, max_y, max_z;

            if (loadedPModel)
            {
                AddStateToBufferPE(this);

                min_x = EditedPModel.BoundingBox.min_x;
                min_y = EditedPModel.BoundingBox.min_y;
                min_z = EditedPModel.BoundingBox.min_z;

                max_x = EditedPModel.BoundingBox.max_x;
                max_y = EditedPModel.BoundingBox.max_y;
                max_z = EditedPModel.BoundingBox.max_z;

                for (i = 0; i < 16; i++) transInverse[i] = planeTransformation[i];

                InvertMatrix(ref transInverse);
                ApplyPModelTransformation(ref EditedPModel, transInverse);
                ComputeBoundingBox(ref EditedPModel);

                SlimPModel(ref EditedPModel);
                ApplyPModelTransformation(ref EditedPModel, planeTransformation);

                EditedPModel.BoundingBox.min_x = min_x;
                EditedPModel.BoundingBox.min_y = min_y;
                EditedPModel.BoundingBox.min_z = min_z;

                EditedPModel.BoundingBox.max_x = max_x;
                EditedPModel.BoundingBox.max_y = max_y;
                EditedPModel.BoundingBox.max_z = max_z;

                //if (chkEnableLighting.Checked) ComputeNormals(ref EditedPModel);
                ComputeNormals(ref EditedPModel);
                PanelEditorPModel_Paint(null, null);
            }
        }

        private void FrmPEditor_Activated(object sender, EventArgs e)
        {
            //if (GetOGLContext() != OGLContextPEditor)
            //    SetOGLContext(panelEditorPModelDC, OGLContextPEditor);

            //if (ActiveForm != this) return;

            //MessageBox.Show("FrmPEditor", "Test", MessageBoxButtons.OK);

            //panelEditorPModel_Paint(null, null);
        }

        private void PbPaletteColor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && chkPaletteMode.Checked)
            {
                PictureBox pbBC = (PictureBox)sender;
                pbBC.DoDragDrop(pbBC, DragDropEffects.Move);
            }
        }

        private void PbPalette_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
                            e.Effect = DragDropEffects.Move;
        }

        private void PbPalette_DragDrop(object sender, DragEventArgs e)
        {
            int yPos, xPos, xc, yc;

            xPos = pbPalette.PointToClient(new Point(e.X, e.Y)).X;
            yPos = pbPalette.PointToClient(new Point(e.X, e.Y)).Y;

            int numColorCols, numColorRows, szColorRowsHeight, szColorColsWidth;

            numColorCols = Convert.ToInt32(Math.Sqrt(colorTable.Count));
            numColorRows = Convert.ToInt32(Math.Ceiling(colorTable.Count / (double)numColorCols));

            szColorRowsHeight = Convert.ToInt32(pbPalette.ClientRectangle.Height / numColorRows);
            szColorColsWidth = Convert.ToInt32(pbPalette.ClientRectangle.Width / numColorCols);

            yc = (int)Math.Floor((float)yPos / szColorRowsHeight);
            xc = (int)Math.Floor((float)xPos / szColorColsWidth);

            if (!DoNotAddPEStateQ) AddStateToBufferPE(this);
            DoNotAddPEStateQ = true;

            iSelectedColor = yc * numColorCols + xc;

            colorTable[iSelectedColor] = pbPaletteColor.BackColor;

            ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);
            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
            bColorsChanged = true;

            PanelEditorPModel_Paint(null, null);
            PbPalette_Paint(null, null);

            DoNotAddPEStateQ = false;
        }

        private void TmrRenderPModel_Tick(object sender, EventArgs e)
        {
            PanelEditorPModel_Paint(null, null);
            tmrRenderPModel.Stop();
        }



        /////////////////////////////////////////////////////////////
        // Local methods:
        public void InitializeLoadPEditor()
        {
            //  Get HDC for picturebox
            panelEditorPModelDC = GetDC(panelEditorPModel.Handle);
            OGLContextPEditor = CreateOGLContext(panelEditorPModelDC);

            // Initialize OpenGL Context;
            //SetOGLEditorSettings();

            loadedPModel = false;

            pbPalette.AllowDrop = true;

            //  Initialize Undo/Redo feature for PEditor
            InitializeUndoRedoPE();

            Text = "P Editor - " + EditedPModel.fileName;

            // Generic local vars
            hsbLightX.Maximum = LIGHT_STEPS;
            hsbLightX.Minimum = -LIGHT_STEPS;
            hsbLightY.Maximum = LIGHT_STEPS;
            hsbLightY.Minimum = -LIGHT_STEPS;
            hsbLightZ.Maximum = LIGHT_STEPS;
            hsbLightZ.Minimum = -LIGHT_STEPS;

            // Select Vertex colors draw mode by default
            drawMode = 2;
            rbVertexColors.PerformClick();

            chkEnableLighting.Checked = false;

            primaryFunc = K_ROTATE;
            secondaryFunc = K_ZOOM;
            ternaryFunc = K_PAN;
            SetFunctionButtonColors();

            // Adding MouseWheel feature to panelEditorPModel_MouseWheel PictureBox
            panelEditorPModel.MouseWheel += PanelEditorPModel_MouseWheel;

            // Some few Hints/ToolTips
            DefineToolTips();

            // Define CTRL+Home shortcut for reset plane feature ("Home" key is not present in my visual studio notebook? is a VS bug?
            resetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Home;
            resetToolStripMenuItem.ShortcutKeyDisplayString = "CTRL+Home";
            unselectGroupToolStripMenuItem.ShortcutKeyDisplayString = "Q";

            // Undo/Redo PE feature
            DoNotAddPEStateQ = false;

            // Group aspects inits
            SelectedGroup = -1;
            lbGroups.SelectedIndex = -1;
            ChangeGroupEnable(false);

            //  Initialize main form
            InitializeEditorPModelDataControls();

            PanelEditorPModel_Paint(null, null);
            loadedPModel = true;

            // Reset global things of environment
            ResetCameraPE();
            ResetPlane();
        }

        public void InitializeEditorPModelDataControls()
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            //rszXPE = 1;
            //rszYPE = 1;
            //rszZPE = 1;
            rszGroupXPE = 1;
            rszGroupYPE = 1;
            rszGroupZPE = 1;

            //repXPE = 0;
            //repYPE = 0;
            //repZPE = 0;
            repGroupXPE = 0;
            repGroupYPE = 0;
            repGroupZPE = 0;

            //alphaPE = 0;
            //betaPE = 0;
            //gammaPE = 0;
            alphaGroupPE = 0;
            betaGroupPE = 0;
            gammaGroupPE = 0;

            hsbRotateAlpha.Value = 0;
            txtRotateAlpha.Text = "0";
            hsbRotateBeta.Value = 0;
            txtRotateBeta.Text = "0";
            hsbRotateGamma.Value = 0;
            txtRotateGamma.Text = "0";

            // ComputeBoundingBox
            //ComputeBoundingBox(ref EditedPModel);

            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
            DISTPE = -2 * ComputeSceneRadius(p_min, p_max);

            hsbLightX.Value = 0;
            hsbLightY.Value = 1;
            hsbLightZ.Value = 0;
            iLightX = 0;
            iLightY = 1;
            iLightZ = 0;

            // Load modifiers
            loadingModifiersQ = true;

            hsbResizeX.Value = (int)(EditedPModel.resizeX * 100);
            hsbResizeY.Value = (int)(EditedPModel.resizeY * 100);
            hsbResizeZ.Value = (int)(EditedPModel.resizeZ * 100);
            txtResizeX.Text = (EditedPModel.resizeX * 100).ToString();
            txtResizeY.Text = (EditedPModel.resizeY * 100).ToString();
            txtResizeZ.Text = (EditedPModel.resizeZ * 100).ToString();
            rszXPE = EditedPModel.resizeX;
            rszYPE = EditedPModel.resizeY;
            rszZPE = EditedPModel.resizeZ;

            if (EditedPModel.repositionX != 0)
                hsbRepositionX.Value = (int)(EditedPModel.repositionX / EditedPModel.diameter * (100 / fBattleLocationGroupScale));
            else
                hsbRepositionX.Value = 0;

            if (EditedPModel.repositionY != 0)
                hsbRepositionY.Value = (int)(EditedPModel.repositionY / EditedPModel.diameter * (100 / fBattleLocationGroupScale));
            else
                hsbRepositionY.Value = 0;

            if (EditedPModel.repositionY != 0)
                hsbRepositionZ.Value = (int)(EditedPModel.repositionZ / EditedPModel.diameter * (100 / fBattleLocationGroupScale));
            else
                hsbRepositionZ.Value = 0;

            txtRepositionX.Text = hsbRepositionX.Value.ToString();
            txtRepositionY.Text = hsbRepositionY.Value.ToString();
            txtRepositionZ.Text = hsbRepositionZ.Value.ToString();
            repXPE = EditedPModel.repositionX;
            repYPE = EditedPModel.repositionY;
            repZPE = EditedPModel.repositionZ;

            hsbRotateAlpha.Value = (int)(EditedPModel.rotateAlpha * fBattleLocationGroupScale);
            hsbRotateBeta.Value = (int)(EditedPModel.rotateBeta * fBattleLocationGroupScale);
            hsbRotateGamma.Value = (int)(EditedPModel.rotateGamma * fBattleLocationGroupScale);
            txtRotateAlpha.Text = hsbRotateAlpha.Value.ToString();
            txtRotateBeta.Text = hsbRotateBeta.Value.ToString();
            txtRotateGamma.Text = hsbRotateGamma.Value.ToString();

            loadingModifiersQ = false;

            // Initialize some menu items
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;

            // Fill combobox with list of bones
            FillGroupsList();

            // Reset palette things
            if (bmpFullGradientPalette != null) bmpFullGradientPalette.Dispose();
            bmpFullGradientPalette = new DirectBitmap(pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height);

            chkPaletteMode.Checked = false;
            DrawPalette(K_LOAD);

            // Draw palette in pbPalette picturebox
            iSelectedColor = -1;
            iThreshold = 0;
            iBrightnessFactor = 0;
            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
            FillColorTable(EditedPModel, ref colorTable, 
                           ref translationTableVertex, ref translationTablePolys, iThreshold);

            // Header flags
            chkVertexColor.Checked = EditedPModel.Header.vertexColor == 1;
        }

        public void FillGroupsList()
        {
            int gi;
            string strName;

            lbGroups.Items.Clear();

            for (gi = 0; gi < EditedPModel.Header.numGroups; gi++)
            {
                strName = "Group " + gi.ToString("00");

                if (EditedPModel.Groups[gi].texFlag == 1)
                    strName = strName +  " (Tex.Idx: " + EditedPModel.Groups[gi].texID.ToString("00") + ")";

                if (EditedPModel.Groups[gi].HiddenQ) strName += " [H]";

                lbGroups.Items.Add(strName);
            }
        }

        public void ResetCameraPE()
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            if (loadedPModel)
            {
                ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

                alphaPE = 0;
                betaPE = 0;
                gammaPE = 0;
                panXPE = 0;
                panYPE = 0;
                panZPE = 0;

                DISTPE = -2 * ComputeSceneRadius(p_min, p_max);
            }
        }

        public void ComputeCurrentEquations()
        {
            Point3D normal = new Point3D();
            Point3D tmpNormal = new Point3D(planeOriginalA, planeOriginalB, planeOriginalC);

            planePoint.x = (float)planeTransformation[12];
            planePoint.y = (float)planeTransformation[13];
            planePoint.z = (float)planeTransformation[14];

            planeTransformation[12] = 0;
            planeTransformation[13] = 0;
            planeTransformation[14] = 0;

            MultiplyPoint3DByOGLMatrix(planeTransformation, tmpNormal, ref normal);
            normal = Normalize(normal);

            planeA = normal.x;
            planeB = normal.y;
            planeC = normal.z;

            planeTransformation[12] = planePoint.x;
            planeTransformation[13] = planePoint.y;
            planeTransformation[14] = planePoint.z;

            planeD = -planeA * planePoint.x - planeB * planePoint.y - planeC * planePoint.z;
        }

        public void ResetPlane()
        {
            nudAlphaPlane.Value = 0;
            nudBetaPlane.Value = 0;

            nudXPlane.Value = 0;
            nudYPlane.Value = 0;
            nudZPlane.Value = 0;

            oldAlphaPlane = 0;
            oldBetaPlane = 0;
            oldGammaPlane = 0;

            planeA = planeOriginalA = 0;
            planeB = planeOriginalB = 0;
            planeC = planeOriginalC = 1;
            planeD = planeOriginalD = 0;

            planeOriginalPoint.x = 0;
            planeOriginalPoint.y = 0;
            planeOriginalPoint.z = 0;

            planeRotationQuat.x = 0;
            planeRotationQuat.y = 0;
            planeRotationQuat.z = 0;
            planeRotationQuat.w = 1;

            BuildMatrixFromQuaternion(planeRotationQuat, ref planeTransformation);

            planeOriginalPoint1.x = EditedPModel.diameter;
            planeOriginalPoint1.y = EditedPModel.diameter;
            planeOriginalPoint1.z = 0;

            planeOriginalPoint2.x = -EditedPModel.diameter;
            planeOriginalPoint2.y = EditedPModel.diameter;
            planeOriginalPoint2.z = 0;

            planeOriginalPoint3.x = -EditedPModel.diameter;
            planeOriginalPoint3.y = -EditedPModel.diameter;
            planeOriginalPoint3.z = 0;

            planeOriginalPoint4.x = EditedPModel.diameter;
            planeOriginalPoint4.y = -EditedPModel.diameter;
            planeOriginalPoint4.z = 0;

            ComputeCurrentEquations();
        }

        public void CommitContextualizedPChanges(bool bDNormals)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            float tmpDist, modelDiameterNormalized;

            AddStateToBufferPE(this);

            SetCameraModelViewQuat(repXPE, repYPE, repZPE,
                                   EditedPModel.rotationQuaternion,
                                   rszXPE, rszYPE, rszZPE);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            ComputeCurrentBoundingBox(ref EditedPModel);
            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
            tmpDist = -2 * ComputeSceneRadius(p_min, p_max);
            
            if (chkEnableLighting.Checked)
            {
                glViewport(0, 0, panelEditorPModel.ClientRectangle.Width, panelEditorPModel.ClientRectangle.Height);
                glClear(GLBufferMask.GL_COLOR_BUFFER_BIT | GLBufferMask.GL_DEPTH_BUFFER_BIT);

                SetCameraAroundModelQuat(ref p_min, ref p_max, repXPE, repYPE, repZPE + tmpDist,
                                         EditedPModel.rotationQuaternion,
                                         rszXPE, rszYPE, rszZPE);

                glDisable(GLCapability.GL_LIGHT0);
                glDisable(GLCapability.GL_LIGHT1);
                glDisable(GLCapability.GL_LIGHT2);
                glDisable(GLCapability.GL_LIGHT3);

                //ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
                modelDiameterNormalized = (-2 * ComputeSceneRadius(p_min, p_max)) / LIGHT_STEPS;

                SetLighting(GLCapability.GL_LIGHT0, modelDiameterNormalized * hsbLightX.Value,
                                                    modelDiameterNormalized * hsbLightY.Value,
                                                    modelDiameterNormalized * hsbLightZ.Value,
                                                    1, 1, 1, false);

                ApplyCurrentVColors(ref EditedPModel);
            }

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPopMatrix();

            SetCameraModelViewQuat(repXPE, repYPE, repZPE,
                                   EditedPModel.rotationQuaternion,
                                   rszXPE, rszYPE, rszZPE);


            ApplyPChangesPE(ref EditedPModel, bDNormals);

            loadingModifiersQ = true;
            hsbResizeX.Value = 100;
            hsbResizeY.Value = 100;
            hsbResizeZ.Value = 100;
            hsbRepositionX.Value = 0;
            hsbRepositionY.Value = 0;
            hsbRepositionZ.Value = 0;
            hsbRotateAlpha.Value = 0;
            hsbRotateBeta.Value = 0;
            hsbRotateGamma.Value = 0;

            txtResizeX.Text = "100";
            txtResizeY.Text = "100";
            txtResizeZ.Text = "100";
            txtRepositionX.Text = "0";
            txtRepositionY.Text = "0";
            txtRepositionZ.Text = "0";
            txtRotateAlpha.Text = "0";
            txtRotateBeta.Text = "0";
            txtRotateGamma.Text = "0";

            EditedPModel.rotationQuaternion.x = 0;
            EditedPModel.rotationQuaternion.y = 0;
            EditedPModel.rotationQuaternion.z = 0;
            EditedPModel.rotationQuaternion.w = 1;

            rszXPE = 1;
            rszYPE = 1;
            rszZPE = 1;
            repXPE = 0;
            repYPE = 0;
            repZPE = 0;

            rszGroupXPE = 1;
            rszGroupYPE = 1;
            rszGroupZPE = 1;
            repGroupXPE = 0;
            repGroupYPE = 0;
            repGroupZPE = 0;
            loadingModifiersQ = false;

            // Header flags
            EditedPModel.Header.vertexColor = chkVertexColor.Checked ? 1 : 0;

            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
            FillColorTable(EditedPModel, ref colorTable,
                           ref translationTableVertex, ref translationTablePolys, (byte)iThreshold);
            //ApplyColorTable(ref EditedPModel, colorTable, translationTableVertex, translationTablePolys);

            iSelectedColor = -1;
            iBrightnessFactor = 0;

            PanelEditorPModel_Paint(null, null);
            PbPalette_Paint(null, null);
        }

        public void SetFunctionButtonColors()
        {
            SetPaintButtonColor();
            SetCutEdgeButtonColor();
            SetEraseButtonColor();
            SetPickVertexButtonColor();
            SetRotateButtonColor();
            SetZoomButtonColor();
            SetPanButtonColor();
            SetNewPolyButtonColor();
        }

        public void SetPaintButtonColor()
        {
            if (primaryFunc == K_PAINT)
            {
                rbPaintPolygon.BackColor = Color.LightCoral;
                rbPolygonColors.PerformClick();
            }
            else
            {
                if (secondaryFunc == K_PAINT)
                {
                    rbPaintPolygon.BackColor = Color.PowderBlue;
                    rbPolygonColors.PerformClick();
                }                   
                else
                {
                    if (ternaryFunc == K_PAINT)
                    {
                        rbPaintPolygon.BackColor = Color.MediumAquamarine;
                        rbPolygonColors.PerformClick();
                    }
                    else
                        rbPaintPolygon.BackColor = Color.Transparent;
                }
            }                 
        }

        public void SetCutEdgeButtonColor()
        {
            if (primaryFunc == K_CUT_EDGE)
            {
                rbCutEdge.BackColor = Color.LightCoral;
                rbPolygonColors.PerformClick();
            }
            else
            {
                if (secondaryFunc == K_CUT_EDGE)
                {
                    rbCutEdge.BackColor = Color.PowderBlue;
                    rbPolygonColors.PerformClick();
                }
                else
                {
                    if (ternaryFunc == K_CUT_EDGE)
                    {
                        rbCutEdge.BackColor = Color.MediumAquamarine;
                        rbPolygonColors.PerformClick();
                    }
                    else
                        rbCutEdge.BackColor = Color.Transparent;
                }
            }
        }

        public void SetEraseButtonColor()
        {
            if (primaryFunc == K_ERASE_POLY)
            {
                rbErasePolygon.BackColor = Color.LightCoral;
                rbPolygonColors.PerformClick();
            }
            else
            {
                if (secondaryFunc == K_ERASE_POLY)
                {
                    rbErasePolygon.BackColor = Color.PowderBlue;
                    rbPolygonColors.PerformClick();
                }
                else
                {
                    if (ternaryFunc == K_ERASE_POLY)
                    {
                        rbErasePolygon.BackColor = Color.MediumAquamarine;
                        rbPolygonColors.PerformClick();
                    }                        
                    else
                        rbErasePolygon.BackColor = Color.Transparent;
                }
            }
        }

        public void SetPickVertexButtonColor()
        {
            if (primaryFunc == K_PICK_VERTEX)
            {
                rbMoveVertex.BackColor = Color.LightCoral;
                rbPolygonColors.PerformClick();
            }
            else
            {
                if (secondaryFunc == K_PICK_VERTEX)
                {
                    rbMoveVertex.BackColor = Color.PowderBlue;
                    rbPolygonColors.PerformClick();
                }
                else
                {
                    if (ternaryFunc == K_PICK_VERTEX)
                    {
                        rbMoveVertex.BackColor = Color.MediumAquamarine;
                        rbPolygonColors.PerformClick();
                    }                        
                    else
                        rbMoveVertex.BackColor = Color.Transparent;
                }
            }
        }

        public void SetRotateButtonColor()
        {
            if (primaryFunc == K_ROTATE)
            {
                rbFreeRotate.BackColor = Color.LightCoral;
            }
            else
            {
                if (secondaryFunc == K_ROTATE)
                    rbFreeRotate.BackColor = Color.PowderBlue;
                else
                {
                    if (ternaryFunc == K_ROTATE)
                        rbFreeRotate.BackColor = Color.MediumAquamarine;
                    else
                        rbFreeRotate.BackColor = Color.Transparent;
                }
            }
        }

        public void SetZoomButtonColor()
        {
            if (primaryFunc == K_ZOOM)
            {
                rbZoomInOut.BackColor = Color.LightCoral;
            }
            else
            {
                if (secondaryFunc == K_ZOOM)
                    rbZoomInOut.BackColor = Color.PowderBlue;
                else
                {
                    if (ternaryFunc == K_ZOOM)
                        rbZoomInOut.BackColor = Color.MediumAquamarine;
                    else
                        rbZoomInOut.BackColor = Color.Transparent;
                }
            }
        }

        public void SetPanButtonColor()
        {
            if (primaryFunc == K_PAN)
            {
                rbPanning.BackColor = Color.LightCoral;
            }
            else
            {
                if (secondaryFunc == K_PAN)
                    rbPanning.BackColor = Color.PowderBlue;
                else
                {
                    if (ternaryFunc == K_PAN)
                        rbPanning.BackColor = Color.MediumAquamarine;
                    else
                        rbPanning.BackColor = Color.Transparent;
                }
            }
        }

        public void SetNewPolyButtonColor()
        {
            if (primaryFunc == K_NEW_POLY)
            {
                rbNewPolygon.BackColor = Color.LightCoral;
                rbPolygonColors.PerformClick();
            }
            else
            {
                if (secondaryFunc == K_NEW_POLY)
                {
                    rbNewPolygon.BackColor = Color.PowderBlue;
                    rbPolygonColors.PerformClick();
                }
                else
                {
                    if (ternaryFunc == K_NEW_POLY)
                    {
                        rbNewPolygon.BackColor = Color.MediumAquamarine;
                        rbPolygonColors.PerformClick();
                    }
                    else
                        rbNewPolygon.BackColor = Color.Transparent;
                }
            }
        }

        public void DrawPalette(int iEvent)
        {
            int iColorCounter, x, y, numRow, numCol, szColorRowsHeight, szColorColsWidth, numColorRows, numColorCols;
            //float numColorsFactor;
            double fBrightness;
            float fR, fG, fB;
            HatchBrush hBrush;
            SolidBrush sBrush;
            Pen hPen;

            DirectBitmap bmpFullPalette = new DirectBitmap(pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height);

            if (chkPaletteMode.Checked)
            {
                using (Graphics g = Graphics.FromImage(bmpFullPalette.Bitmap))
                {
                    hBrush = new HatchBrush(HatchStyle.ForwardDiagonal, Color.Black, Color.White);
                    g.FillRectangle(hBrush, new Rectangle(0, 0,
                                                          pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height));
                }

                numColorCols = Convert.ToInt32(Math.Sqrt(colorTable.Count));
                numColorRows = Convert.ToInt32(Math.Ceiling(colorTable.Count / (double)numColorCols));

                szColorRowsHeight = Convert.ToInt32(pbPalette.ClientRectangle.Height / numColorRows);
                szColorColsWidth = Convert.ToInt32(pbPalette.ClientRectangle.Width / numColorCols);

                x = 0; y = 0; iColorCounter = 0; numRow = 0; numCol = 0;

                while (iColorCounter < colorTable.Count)
                {
                    if (iColorCounter == iSelectedColor)
                    {

                        using (Graphics g = Graphics.FromImage(bmpFullPalette.Bitmap))
                        {
                            hBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.FromArgb(255, colorTable[iColorCounter].R, 
                                                                                                           colorTable[iColorCounter].G, 
                                                                                                           colorTable[iColorCounter].B));
                            g.FillRectangle(hBrush, new Rectangle(x, y,
                                                                  szColorColsWidth,
                                                                  szColorRowsHeight));

                            sBrush = new SolidBrush(Color.Black);
                            hPen = new Pen(sBrush, 1);
                            g.DrawRectangle(hPen, new Rectangle(x, y,
                                                                szColorColsWidth,
                                                                szColorRowsHeight));
                        }
                    }
                    else
                    {
                        using (Graphics g = Graphics.FromImage(bmpFullPalette.Bitmap))
                        {
                            sBrush = new SolidBrush(Color.FromArgb(255, colorTable[iColorCounter].R,
                                                                        colorTable[iColorCounter].G, 
                                                                        colorTable[iColorCounter].B));
                            g.FillRectangle(sBrush, new Rectangle(x, y,
                                                                  szColorColsWidth,
                                                                  szColorRowsHeight));

                            sBrush = new SolidBrush(Color.Black);
                            hPen = new Pen(sBrush, 1);
                            g.DrawRectangle(hPen, new Rectangle(x, y,
                                                                szColorColsWidth,
                                                                szColorRowsHeight));
                        }
                    }

                    numCol++;
                    x += szColorColsWidth;

                    if (numCol == numColorCols)
                    {
                        numRow++;
                        numCol = 0;

                        x = 0;
                        y += szColorRowsHeight;
                    }

                    iColorCounter++;
                }

                using (Graphics g = Graphics.FromImage(bmpFullPalette.Bitmap))
                {
                    sBrush = new SolidBrush(Color.Black);
                    g.DrawRectangle(new Pen(sBrush, 2),
                                    new Rectangle(0, 0,
                                                  pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height));
                }
            }
            else
            {
                for (x = 0; x < pbPalette.ClientRectangle.Width; x++)
                {
                    if (iEvent == K_LOAD)
                    { 
                        for (y = 0; y < pbPalette.ClientRectangle.Height - 16; y++)
                        {
                            fR = (float)x / pbPalette.ClientRectangle.Width * 255;
                            fG = ((float)y / (pbPalette.ClientRectangle.Height - 16)) * 255;
                            fB = (float)(pbPalette.ClientRectangle.Width - x) / pbPalette.ClientRectangle.Width * 255;

                            bmpFullGradientPalette.SetPixel(x, y, Color.FromArgb(255, (byte)fR, (byte)fG, (byte)fB));
                        }
                    }

                    fBrightness = 2 * ((x - pbPalette.ClientRectangle.Width) / 2f) / pbPalette.ClientRectangle.Width + 1;

                    fR = (float)(hsbSelectedColorR.Value * fBrightness);
                    fG = (float)(hsbSelectedColorG.Value * fBrightness);
                    fB = (float)(hsbSelectedColorB.Value * fBrightness);

                    for (y = pbPalette.ClientRectangle.Height - 16; y < pbPalette.ClientRectangle.Height; y++)
                    {
                        bmpFullGradientPalette.SetPixel(x, y, Color.FromArgb(255, (byte)fR, (byte)fG, (byte)fB));
                    }
                }

                if (bmpFullPalette != null) bmpFullPalette.Dispose();
                bmpFullPalette.Bitmap = bmpFullGradientPalette.Bitmap.Clone(new Rectangle(0, 0, 
                                                                                          bmpFullGradientPalette.Width, 
                                                                                          bmpFullGradientPalette.Height),
                                                                            PixelFormat.Format32bppRgb);
            }

            if (pbPalette.Image != null) pbPalette.Image.Dispose();
            pbPalette.Image = bmpFullPalette.Bitmap.Clone(new Rectangle(0, 0,
                                                                        pbPalette.ClientRectangle.Width,
                                                                        pbPalette.ClientRectangle.Height),
                                                          PixelFormat.Format32bppRgb);
        }

        public void ChangeGroupStatus(bool bChangeGroup)
        {
            if (bChangeGroup)
            {
                // Let's do something to make it know to the user he is in Group Mode edition
                // like change Reposition/Resize/Rotate groups backcolor.
                gbReposition.BackColor = Color.LightSlateGray;
                gbResize.BackColor = Color.LightSlateGray;
                gbRotation.BackColor = Color.LightSlateGray;

                // We will work with the Group
                // ///////// Reposition
                hsbRepositionX.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].repGroupX * (100 / fBattleLocationGroupScale));
                hsbRepositionY.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].repGroupY * (100 / fBattleLocationGroupScale));
                hsbRepositionZ.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].repGroupZ * (100 / fBattleLocationGroupScale));

                // ///////// Resize
                hsbResizeX.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rszGroupX * 100);
                hsbResizeY.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rszGroupY * 100);
                hsbResizeZ.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rszGroupZ * 100);

                // ///////// Rotation
                bGlobalChangeGroup = true;               
                //hsbRotateAlpha.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupAlpha);
                txtRotateAlpha.Text = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupAlpha * fBattleLocationGroupScale).ToString();
                //hsbRotateBeta.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupBeta);
                txtRotateBeta.Text = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupBeta * fBattleLocationGroupScale).ToString();
                //hsbRotateGamma.Value = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupGamma);
                txtRotateGamma.Text = Convert.ToInt32(EditedPModel.Groups[SelectedGroup].rotGroupGamma * fBattleLocationGroupScale).ToString();
                bGlobalChangeGroup = false;
            }
            else
            {
                // Let's leave it as default color when there is not Group Mode edition selected.
                gbReposition.BackColor = SystemColors.ControlDarkDark;
                gbResize.BackColor = SystemColors.ControlDarkDark;
                gbRotation.BackColor = SystemColors.ControlDarkDark;

                // We will work with the Model
                // ///////// Reposition
                hsbRepositionX.Value = Convert.ToInt32(EditedPModel.repositionX * (100 / fBattleLocationGroupScale));
                hsbRepositionY.Value = Convert.ToInt32(EditedPModel.repositionY * (100 / fBattleLocationGroupScale));
                hsbRepositionZ.Value = Convert.ToInt32(EditedPModel.repositionZ * (100 / fBattleLocationGroupScale));

                // ///////// Resize
                hsbResizeX.Value = Convert.ToInt32(EditedPModel.resizeX * 100);
                hsbResizeY.Value = Convert.ToInt32(EditedPModel.resizeY * 100);
                hsbResizeZ.Value = Convert.ToInt32(EditedPModel.resizeZ * 100);

                // ///////// Rotation
                bGlobalChangeGroup = true;
                //hsbRotateAlpha.Value = Convert.ToInt32(EditedPModel.rotateAlpha);
                txtRotateAlpha.Text = Convert.ToInt32(EditedPModel.rotateAlpha).ToString();
                //hsbRotateBeta.Value = Convert.ToInt32(EditedPModel.rotateBeta);
                txtRotateBeta.Text = Convert.ToInt32(EditedPModel.rotateBeta).ToString();
                //hsbRotateGamma.Value = Convert.ToInt32(EditedPModel.rotateGamma);
                txtRotateGamma.Text = Convert.ToInt32(EditedPModel.rotateGamma).ToString();
                bGlobalChangeGroup = false;
            }
        }

        // ORIGINAL BACKUP COPY KIMERAVB6
        //public void DrawPalette(int iEvent)
        //{
        //    int i, x, y, x0, y0;           
        //    uint col;
        //    HDC hBrush, hNewBrush, hOldBrush, hPen, hOldPen;
        //    float sRows, fBrightness;
        //    LOGBRUSH lbNewBrush;

        //    float fR, fG, fB;

        //    if (chkPalettized.Checked)
        //    {
        //        lbNewBrush.lbColor = (int)RGBTOINT(0, 0, 0);
        //        lbNewBrush.lbStyle = 2;
        //        lbNewBrush.lbHatch = 3;

        //        hNewBrush = CreateBrushIndirect(ref lbNewBrush);
        //        hOldBrush = SelectObject(hPalDC, hNewBrush);

        //        DeleteObject(hOldBrush);
        //        Rectangle(hPalDC, 0, 0, pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height);

        //        sRows = 2 * pbPalette.ClientRectangle.Height / Math.Max(nColors, 1);
        //        x0 = 0; y0 = 0; i = 0;

        //        while (i < nColors)
        //        {
        //            hBrush = CreateSolidBrush(RGBTOINT(colorTable[i].R, colorTable[i].G, colorTable[i].B));
        //            hOldBrush = SelectObject(hPalDC, hBrush);
        //            DeleteObject(hOldBrush);

        //            if (i == iSelectedColor)
        //            {
        //                hPen = CreatePen(0, 2, RGBTOINT(255, 255, 255));
        //                hOldPen = SelectObject(hPalDC, hPen);
        //                Rectangle(hPalDC, x0, y0, x0 + pbPalette.ClientRectangle.Width / 2, (int)(y0 + sRows));
        //                SelectObject(hPalDC, hOldPen);
        //                DeleteObject(hPen);
        //            }
        //            else
        //            {
        //                Rectangle(hPalDC, x0, y0, x0 + pbPalette.ClientRectangle.Width / 2, (int)(y0 + sRows));
        //            }

        //            x0 += pbPalette.ClientRectangle.Width / 2;
        //            i++;

        //            if (i % 2 == 0)
        //            {
        //                y0 += (int)sRows;
        //                x0 = 0;
        //            }
        //        }

        //        BitBlt(pbPaletteDC, 0, 0, pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height,
        //               hPalDC, 0, 0, TernaryRasterOperations.SRCCOPY);
        //    }
        //    else
        //    {
        //        for (x = 0; x < pbPalette.ClientRectangle.Width; x++)
        //        {
        //            if (iEvent == K_LOAD)
        //            {
        //                for (y = 0; y < pbPalette.ClientRectangle.Height * 0.9f; y++)
        //                {
        //                    fR = (float)x / pbPalette.ClientRectangle.Width * 255;
        //                    fG = ((float)y / pbPalette.ClientRectangle.Height) * 255 / 0.9f;
        //                    fB = (float)(pbPalette.ClientRectangle.Width - x) / pbPalette.ClientRectangle.Width * 255;

        //                    SetPixel(hFullPalDC, x, y, RGBTOINT((byte)fR, (byte)fG, (byte)fB));
        //                }
        //            }

        //            BitBlt(hPalDC, 0, 0, pbPalette.ClientRectangle.Width, (int)(pbPalette.ClientRectangle.Height * 0.9f),
        //                   hFullPalDC, 0, 0, TernaryRasterOperations.SRCCOPY);

        //            fBrightness = 2 * (x - pbPalette.ClientRectangle.Width / 2f) / pbPalette.ClientRectangle.Width + 1;

        //            fR = hsbSelectedColorR.Value * fBrightness;
        //            fG = hsbSelectedColorG.Value * fBrightness;
        //            fB = hsbSelectedColorB.Value * fBrightness;

        //            col = RGBTOINT((byte)fR, (byte)fG, (byte)fB);

        //            for (y = (int)(pbPalette.ClientRectangle.Height * 0.9f); y < pbPalette.ClientRectangle.Height; y++)
        //            {
        //                SetPixel(hPalDC, x, y, col);
        //            }
        //        }

        //        BitBlt(pbPaletteDC, 0, 0, pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height,
        //               hPalDC, 0, 0, TernaryRasterOperations.SRCCOPY);
        //    }

        //    //IntPtr pbBMPPaletteDC;
        //    //pbBMPPaletteDC = CreateCompatibleBitmap(pbPaletteDC, pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height);

        //    //pbPalette.Image =
        //    //    FitBitmapToPictureBox(pbPalette, pbPalette.ClientRectangle.Width, pbPalette.ClientRectangle.Height, pbBMPPaletteDC);

        //}





        ////////////////////////////////////////////////////////////
        //  Main DoFunction procedure
        public void DoFunction(int nFunc, int iEvent, int x, int y)
        {
            Point3D tmpPoint3D = new Point3D();
            Point3D tmpPoint3D_2 = new Point3D();
            Color tmpColor;
            float fLocalAlpha = 0;

            Point3D p1;
            Point3D p2;
            Point2D tc1 = new Point2D();
            Point2D tc2 = new Point2D();

            Point3D intersectionPoint;
            Point2D intersectionTexCoord;

            int vi1, vi2; 
            int iGroupIdx, tmpGroupIdx, iPolyIdx, iVertIdx, iEdgeIdx;

            switch (nFunc)
            {
                case K_PAINT:
                    //  Change polygon color/get polygon color
                    if (iEvent >= K_CLICK)
                    {
                        //iPolyIdx = GetClosestPolygon(EditedPModel, x, y, DISTPE, panelEditorPModel);
                        iPolyIdx = GetClosestPolygon(EditedPModel, x, y);

                        if (iPolyIdx > -1)
                        {
                            if (iEvent != K_CLICK_SHIFT)
                                AddStateToBufferPE(this);

                            if (chkPaletteMode.Checked)
                            {
                                if (iEvent == K_CLICK_SHIFT)
                                {
                                    iSelectedColor = 
                                        translationTableVertex[EditedPModel.Polys[iPolyIdx].Verts[0] +
                                                               EditedPModel.Groups[GetPolygonGroup(EditedPModel, iPolyIdx)].offsetVert].I;

                                    loadingColorModifiersQ = true;
                                    hsbSelectedColorR.Value = colorTable[iSelectedColor].R;
                                    hsbSelectedColorG.Value = colorTable[iSelectedColor].G;
                                    hsbSelectedColorB.Value = colorTable[iSelectedColor].B;

                                    txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
                                    txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
                                    txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();
                                    loadingColorModifiersQ = false;
                                }
                                else
                                {
                                    //if (iSelectedColor > -1)
                                    //{
                                    PaintPolygon(ref EditedPModel, iPolyIdx, pbPaletteColor.BackColor.R,
                                                                             pbPaletteColor.BackColor.G,
                                                                             pbPaletteColor.BackColor.B);
                                        
                                    if (iSelectedColor > -1)
                                    {
                                        UpdateTranslationTable(ref translationTableVertex, EditedPModel, 
                                                               iPolyIdx, iSelectedColor);
                                        bColorsChanged = true;
                                    }
                                    //}
                                }
                            }
                            else
                            {
                                if (iEvent == K_CLICK_SHIFT)
                                {
                                    tmpColor = ComputePolyColor(EditedPModel, iPolyIdx);

                                    loadingColorModifiersQ = true;
                                    hsbSelectedColorR.Value = tmpColor.R;
                                    hsbSelectedColorG.Value = tmpColor.G;
                                    hsbSelectedColorB.Value = tmpColor.B;

                                    txtSelectedColorR.Text = hsbSelectedColorR.Value.ToString();
                                    txtSelectedColorG.Text = hsbSelectedColorG.Value.ToString();
                                    txtSelectedColorB.Text = hsbSelectedColorB.Value.ToString();
                                    loadingColorModifiersQ = false;

                                }
                                else
                                {
                                    PaintPolygon(ref EditedPModel, iPolyIdx, (byte)hsbSelectedColorR.Value,
                                                                             (byte)hsbSelectedColorG.Value,
                                                                             (byte)hsbSelectedColorB.Value);

                                }
                            }

                            // Apply color arrays of the model to P Editor dynamic arrays.
                            if (iEvent != K_CLICK_SHIFT)
                            {
                                ComputePColors(ref EditedPModel);

                                if (chkPaletteMode.Checked)
                                    FillColorTable(EditedPModel, ref colorTable,
                                                   ref translationTableVertex, ref translationTablePolys, iThreshold);

                                CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                            }


                            //  -- Commented in KimeraVB6
                            //  if (glIsEnabled(GLCapability.GL_LIGHTING)) ComputeNormals(ref EditedPModel);
                        }
                    }
                    break;

                case K_CUT_EDGE:
                    //  Cut an edge on the clicked point (thus splitting the surrounding polygons)
                    if (iEvent == K_CLICK)
                    {
                        //iPolyIdx = GetClosestPolygon(EditedPModel, x, y, DISTPE, panelEditorPModel);
                        iPolyIdx = GetClosestPolygon(EditedPModel, x, y);

                        if (iPolyIdx > -1)
                        {
                            AddStateToBufferPE(this);

                            iEdgeIdx = GetClosestEdge(EditedPModel, iPolyIdx, x, y, ref fLocalAlpha);

                            vi1 = EditedPModel.Polys[iPolyIdx].Verts[iEdgeIdx];
                            vi2 = EditedPModel.Polys[iPolyIdx].Verts[(iEdgeIdx + 1) % 3];
                            iGroupIdx = GetPolygonGroup(EditedPModel, iPolyIdx);
                            p1 = EditedPModel.Verts[EditedPModel.Groups[iGroupIdx].offsetVert + vi1];
                            p2 = EditedPModel.Verts[EditedPModel.Groups[iGroupIdx].offsetVert + vi2];

                            if (EditedPModel.Groups[iGroupIdx].texFlag == 1)
                            {
                                tc1 = EditedPModel.TexCoords[EditedPModel.Groups[iGroupIdx].offsetTex + vi1];
                                tc2 = EditedPModel.TexCoords[EditedPModel.Groups[iGroupIdx].offsetTex + vi2];
                            }

                            intersectionPoint = GetPointInLine(p1, p2, fLocalAlpha);
                            intersectionTexCoord = GetPointInLine2D(tc1, tc2, fLocalAlpha);

                            CutEdgeAtPoint(ref EditedPModel, iPolyIdx, iEdgeIdx, intersectionPoint, intersectionTexCoord);

                            while (FindNextAdjacentPolyEdge(EditedPModel, p1, p2, ref iPolyIdx, ref iEdgeIdx))
                            {
                                //  If crossed group boundaries, recompute intersection_tex_coord
                                tmpGroupIdx = GetPolygonGroup(EditedPModel, iPolyIdx);

                                if (tmpGroupIdx != iGroupIdx)
                                {
                                    iGroupIdx = tmpGroupIdx;

                                    if (EditedPModel.Groups[iGroupIdx].texFlag == 1)
                                    {
                                        vi1 = EditedPModel.Polys[iPolyIdx].Verts[iEdgeIdx];
                                        vi2 = EditedPModel.Polys[iPolyIdx].Verts[(iEdgeIdx + 1) % 3];
                                        tc1 = EditedPModel.TexCoords[EditedPModel.Groups[iGroupIdx].offsetTex + vi1];
                                        tc2 = EditedPModel.TexCoords[EditedPModel.Groups[iGroupIdx].offsetTex + vi2];
                                        intersectionTexCoord = GetPointInLine2D(tc1, tc2, fLocalAlpha);
                                    }
                                }

                                CutEdgeAtPoint(ref EditedPModel, iPolyIdx, iEdgeIdx, 
                                               intersectionPoint, intersectionTexCoord);
                            }

                            KillUnusedVertices(ref EditedPModel);
                            ComputeNormals(ref EditedPModel);
                            ComputeEdges(ref EditedPModel);

                            ComputePColors(ref EditedPModel);

                            // -- Commented in KimeraVB6
                            //if (glIsEnabled(GLCapability.GL_LIGHTING)) ComputeNormals(ref EditedPModel);

                            if (chkPaletteMode.Checked)
                                FillColorTable(EditedPModel, ref colorTable, 
                                               ref translationTableVertex, ref translationTablePolys, iThreshold);

                            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                        }
                    }

                    PbPalette_Paint(null, null);
                    break;

                case K_ERASE_POLY:
                    //  Erase polygon
                    if (iEvent == K_CLICK)
                    {
                        iPolyIdx = GetClosestPolygon(EditedPModel, x, y);

                        if (iPolyIdx > -1)
                        {
                            AddStateToBufferPE(this);

                            if (EditedPModel.Header.numPolys > 1)
                            {
                                RemovePolygon(ref EditedPModel, iPolyIdx);

                                KillUnusedVertices(ref EditedPModel);
                                ComputeNormals(ref EditedPModel);
                                ComputeEdges(ref EditedPModel);
                            }
                            else
                            {
                                MessageBox.Show("A .P Model must have at least one polygon. This last triangle can't be removed.",
                                                "Information", MessageBoxButtons.OK);
                            }

                            // -- Commented in KimeraVB6
                            //if (glIsEnabled(GLCapability.GL_LIGHTING)) ComputeNormals(ref EditedPModel);

                            ComputePColors(ref EditedPModel);

                            if (chkPaletteMode.Checked)
                                FillColorTable(EditedPModel, ref colorTable,
                                               ref translationTableVertex, ref translationTablePolys, iThreshold);

                            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);
                        }
                    }
                    break;

                case K_PICK_VERTEX:
                    //  Pick a vertex. When a vertex is picked, switch to the K_MOVE_VERTEX operation
                    if (iEvent == K_CLICK)
                    {
                        
                        iVertIdx = GetClosestVertex(EditedPModel, x, y);

                        if (iVertIdx > -1)
                        {
                            AddStateToBufferPE(this);

                            dblPickedVertexZ = GetVertexProjectedDepth(ref EditedPModel.Verts, iVertIdx);

                            GetEqualVertices(EditedPModel, iVertIdx, ref lstPickedVertices);

                            if (primaryFunc == K_PICK_VERTEX) primaryFunc = K_MOVE_VERTEX;
                            else secondaryFunc = K_MOVE_VERTEX;

                            if (glIsEnabled(GLCapability.GL_LIGHTING))
                                GetAllNormalDependentPolys(EditedPModel, lstPickedVertices,
                                                           ref lstAdjacentPolys, ref lstAdjacentVerts, ref lstAdjacentAdjacentPolys);
                        }
                        else lstPickedVertices.Clear();
                    }
                    break;

                case K_MOVE_VERTEX:
                    //  Freehand vertex movement
                    if (iEvent == K_MOVE)
                    {
                        if (lstPickedVertices.Count > 0)
                        {
                            AddStateToBufferPE(this);

                            foreach (int itmPickedVertex in lstPickedVertices)
                                MoveVertex(ref EditedPModel, itmPickedVertex, x, y, (float)dblPickedVertexZ);

                            //if (glIsEnabled(GLCapability.GL_LIGHTING))
                            //{
                            UpdateNormals(ref EditedPModel, lstPickedVertices,
                                              lstAdjacentPolys, lstAdjacentVerts, lstAdjacentAdjacentPolys);

                                // -- Commented in KimeraVB6
                                //ComputeNormals(ref EditedPModel);
                            //}
                        }
                    }
                    break;

                case K_NEW_POLY:
                    //  Create new polygon
                    if (iEvent == K_CLICK)
                    {
                        //vi = GetClosestVertex(EditedPModel, x, y, DISTPE, panelEditorPModel);
                        iVertIdx = GetClosestVertex(EditedPModel, x, y);

                        if (iVertIdx > -1)
                        {
                            // We will check if vertex is legit (if it is not repeated for example)
                            tmpVNewPoly[VCountNewPoly] = (ushort)iVertIdx;
                            VCountNewPoly++;

                            if (!ValidateAddPolygonVertices(EditedPModel, tmpVNewPoly, VCountNewPoly))
                            {
                                MessageBox.Show("You are duplicating this last selected vertex for the New Polygon or " +
                                                "selected a vertex from another Group.\nPlease, select another different " +
                                                "vertex.\n\n(INFO: You can 'reset' vertices selected pressing N key.)", "Warning");
                                VCountNewPoly--;
                                return;
                            }

                            rbNewPolygon.Text = VCountNewPoly.ToString() + "/3";
                        }

                        if (VCountNewPoly == 3)
                        {
                            AddStateToBufferPE(this);

                            OrderVertices(EditedPModel.Verts, ref tmpVNewPoly);

                            // Prepare the New Vertices
                            iGroupIdx = GetVertexGroup(EditedPModel, tmpVNewPoly[0]);
                            AddPolygon(ref EditedPModel, ref tmpVNewPoly, iGroupIdx);

                            VCountNewPoly = 0;
                            rbNewPolygon.Text = "0/3";

                            KillUnusedVertices(ref EditedPModel);
                            ComputeNormals(ref EditedPModel);
                            ComputeEdges(ref EditedPModel);

                            ComputePColors(ref EditedPModel);

                            // -- Commented in KimeraVB6
                            //if (glIsEnabled(GLCapability.GL_LIGHTING)) ComputeNormals(ref EditedPModel);

                            if (chkPaletteMode.Checked)
                                FillColorTable(EditedPModel, ref colorTable,
                                               ref translationTableVertex, ref translationTablePolys, iThreshold);

                            CopyModelColors2VP(EditedPModel, ref vcolorsOriginal, ref pcolorsOriginal);

                        }
                    }
                    break;

                case K_ROTATE:
                    if (iEvent == K_MOVE)
                    {
                        betaPE = (betaPE + x - x_lastPE) % 360;
                        alphaPE = (alphaPE + y - y_lastPE) % 360;
                    }
                    break;

                case K_ZOOM:
                    if (iEvent == K_MOVE)
                        DISTPE += (y - y_lastPE) * ComputeDiameter(EditedPModel.BoundingBox) / 100;
                    break;

                case K_PAN:
                    if (iEvent == K_MOVE)
                    {
                        SetCameraPModel(EditedPModel, 0, 0, DISTPE, 0, 0, 0, rszXPE, rszYPE, rszZPE);

                        tmpPoint3D.x = x;
                        tmpPoint3D.y = y;
                        tmpPoint3D.z = GetDepthZ(tmpPoint3D_2);
                        tmpPoint3D = GetUnProjectedCoords(tmpPoint3D);

                        panXPE += tmpPoint3D.x;
                        panYPE += tmpPoint3D.y;
                        panZPE += tmpPoint3D.z;

                        tmpPoint3D.x = x_lastPE;
                        tmpPoint3D.y = y_lastPE;
                        tmpPoint3D.z = GetDepthZ(tmpPoint3D_2);
                        tmpPoint3D = GetUnProjectedCoords(tmpPoint3D);

                        panXPE -= tmpPoint3D.x;
                        panYPE -= tmpPoint3D.y;
                        panZPE -= tmpPoint3D.z;
                    }

                    break;
            }
        }





    }
}

