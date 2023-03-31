using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{
    using static FrmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldRSDResource;
    using static FF7PModel;

    using static Utils;
    using static GDI32;

    public partial class FrmTextureViewer : Form
    {
        // UV Coordinates movement with mouse structs
        public struct STPoint2DXY
        {
            public float x;
            public float y;
        }

        public struct STUVXYCoord
        {
            public List<STPoint2DXY> XYCoords;
        }


        readonly private FrmSkeletonEditor frmSkelEdit;

        public const int I_MAXNUMGROUPS = 128;
        public const int I_TEXTURECOORDVIEWMINSIZE = 512;
        public const int I_WINDOWWIDTHBORDER = 24;
        public const int I_WINDOWHEIGHTBORDER = 44;
        public const int I_RADIUS = 3;

        public PModel TexViewModel;

        public int iTCWidth, iTCHeight;

        // UV Coordinates movement with mouse structs
        public List<STUVXYCoord> lstUVXYCoords;
        bool bFoundXYCoord;
        bool bMouseLeftClicked;
        bool bLoadingTextureViewer;
        int localXYPointIdx, localGroupIdx;

        public bool shiftPressedQ;
        public int iPosXClicked;
        public int iPosYClicked;
        Cursor Hand, HandPlus;


        public FrmTextureViewer(FrmSkeletonEditor frmSkelEdit, PModel modelIn)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;

            TexViewModel = CopyPModel(modelIn);

            shiftPressedQ = false;
        }


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

            toolTip1.SetToolTip(btnFlipH, "Flip UVs Horizontal");
            toolTip1.SetToolTip(btnFlipV, "Flip UVs Vertical");
            toolTip1.SetToolTip(btnRotateUV, "Rotate UVs");
            toolTip1.SetToolTip(btnGreen, "Change White/Lime UVs lines color");
            toolTip1.SetToolTip(btnZoomIn, "Zoom In");
            toolTip1.SetToolTip(btnZoomOut, "Zoom Out");
        }

        public bool NormalizeTextureCoordinates(int iGroupIdx)
        {
            int iVertCounter;

            for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                 iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
            {
                if (TexViewModel.TexCoords[iVertCounter].x > 1.0f || TexViewModel.TexCoords[iVertCounter].x < 0.0f ||
                    TexViewModel.TexCoords[iVertCounter].y > 1.0f || TexViewModel.TexCoords[iVertCounter].y < 0.0f)
                    return true;
            }

            return false;
        }

        public void PrepareUVXYCoords()
        {
            int iGroupIdx, iVertCounter, iTexID;
            float fU, fV;
            STUVXYCoord tmplstUVXYCoords;
            STPoint2DXY tmpP2D;
            bool bNeedNormalize;

            if (lstUVXYCoords == null) lstUVXYCoords = new List<STUVXYCoord>();
            else lstUVXYCoords.Clear();

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            bNeedNormalize = false;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {

                // We have one group to add to the list
                tmplstUVXYCoords = new STUVXYCoord();

                if (TexViewModel.TexCoords != null)
                {
                    if (TexViewModel.TexCoords.Length > 0 &&
                        TexViewModel.Groups[iGroupIdx].texFlag == 1 &&
                        TexViewModel.Groups[iGroupIdx].texID == iTexID)
                    {

                        // First, we will check if we need to normalize texture coordinates. Not is always needed, but we have to check all the UVs for this.
                        if (NormalizeTextureCoordinates(iGroupIdx)) bNeedNormalize = true;

                        // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                        tmplstUVXYCoords.XYCoords = new List<STPoint2DXY>();

                        for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                             iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                            TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                        {
                            fU = TexViewModel.TexCoords[iVertCounter].x;
                            fV = TexViewModel.TexCoords[iVertCounter].y;

                            if (bNeedNormalize)
                            {
                                // Normalize U if needed.
                                if (fU >= 1)
                                {
                                    fU -= (float)Math.Floor(fU);
                                }
                                else if (fU < 0)
                                {
                                    // Maybe some values (like -0.00128) does not need to normalize and must be rounded.
                                    // Found this in Field CMDE.HRC model, Ninostyle Chibi version.
                                    if (fU >= -0.00128) fU = 0;
                                    else
                                    {
                                        if (fU % 1.0f == 0) fU = 0;
                                        else
                                        {
                                            fU = -fU;
                                            fU -= (float)Math.Abs(Math.Floor(fU));
                                            fU = 1 - fU;
                                        }
                                    }
                                }

                                // Normalize V if needed.
                                if (fV >= 1)
                                {
                                    fV -= (float)Math.Floor(fV);
                                }
                                else if (fV < 0)
                                {
                                    // Maybe some values (like -0.00128) does not need to normalize and must be rounded
                                    // Found this for fU in Field CMDE.HRC/AAAA.HRC(texID 2)  model, Ninostyle Chibi version.
                                    if (fV >= -0.00481) fV = 0;
                                    else
                                    {
                                        if (fV % 1.0f == 0) fV = 0;
                                        else
                                        {
                                            fV = -fV;
                                            fV -= (float)Math.Abs(Math.Floor(fV));
                                            fV = 1 - fV;
                                        }
                                    }
                                }
                            }

                            tmpP2D.x = fU * pbTextureView.Width;
                            tmpP2D.y = fV * pbTextureView.Height;

                            tmplstUVXYCoords.XYCoords.Add(tmpP2D);
                        }
                    }

                    lstUVXYCoords.Add(tmplstUVXYCoords);
                }
            }
        }

        private void FrmTextureViewer_Load(object sender, EventArgs e)
        {
            Text = "Texture Coordinates(UVs) Viewer" + " - Model: " + TexViewModel.fileName +
                                                       " - Tex: " + frmSkelEdit.cbTextureSelect.Items[frmSkelEdit.cbTextureSelect.SelectedIndex];

            bLoadingTextureViewer = true;

            // Initialize things
            if (frmSkelEdit.bPaintGreen) btnGreen.Checked = true;

            // Assign tooltips.
            DefineToolTips();

            // Draw UVs and prepare Texture Coordinate(UVs) Viewer form buttons.
            ChangeZoomButtons();
            ChangeTexCoordViewSize();

            // Prepare list of UVs to XY points of the texture for UVpoints movement
            Hand = new Cursor(new MemoryStream(Properties.Resources.hand));
            HandPlus = new Cursor(new MemoryStream(Properties.Resources.handplus));
            PrepareUVXYCoords();
            DrawUVs();

            lblHeight.Text = "H: " + iTCWidth.ToString();
            lblWidth.Text = "W: " + iTCHeight.ToString();

            bLoadingTextureViewer = false;
        }

        public void DrawUVs()
        {
            IntPtr hTmpBMP = IntPtr.Zero;

            int iGroupIdx, iPolyIdx, iVertCounter, iWidth, iHeight, iTexID;
            Point[] pointTriPoly = new Point[4];

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    hTmpBMP = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].HBMP;

                    iTCWidth = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].width;
                    iTCHeight = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].height;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    hTmpBMP = bSkeleton.textures[iTexID].HBMP;

                    iTCWidth = bSkeleton.textures[iTexID].width;
                    iTCHeight = bSkeleton.textures[iTexID].height;

                    break;

                //case K_AA_SKELETON:
                //    if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                //    {
                //        hTmpBMP = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].HBMP;

                //        iTCWidth = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].width;
                //        iTCHeight = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].height;
                //    }
                //    else
                //    {
                //        hTmpBMP = bSkeleton.textures[iTexID].HBMP;

                //        iTCWidth = bSkeleton.textures[iTexID].width;
                //        iTCHeight = bSkeleton.textures[iTexID].height;
                //    }


                //    break;

                //case K_MAGIC_SKELETON:
                //    hTmpBMP = bSkeleton.textures[iTexID].HBMP;

                //    iTCWidth = bSkeleton.textures[iTexID].width;
                //    iTCHeight = bSkeleton.textures[iTexID].height;
                //    break;
            }

            Bitmap tmpBMP = new Bitmap(pbTextureView.Width, pbTextureView.Height);

            // Get the size available
            iWidth = tmpBMP.Width;
            iHeight = tmpBMP.Height;

            using (Graphics g = Graphics.FromImage(tmpBMP))
            {
                //g.InterpolationMode = InterpolationMode.Default;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.DrawImage(Image.FromHbitmap(hTmpBMP), 0, 0, iWidth, iHeight);

                g.PixelOffsetMode = PixelOffsetMode.None;
                g.InterpolationMode = InterpolationMode.Default ;

                for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
                {
                    if (TexViewModel.TexCoords != null)
                    {
                        if (TexViewModel.TexCoords.Length > 0 &&
                            TexViewModel.Groups[iGroupIdx].texFlag == 1 &&
                            TexViewModel.Groups[iGroupIdx].texID == iTexID)
                        {
                            for (iPolyIdx = TexViewModel.Groups[iGroupIdx].offsetPoly;
                                 iPolyIdx < TexViewModel.Groups[iGroupIdx].offsetPoly +
                                            TexViewModel.Groups[iGroupIdx].numPoly; iPolyIdx++)
                            {
                                // Get the 2D point (X,Y pos) of each of the points of the poly and draw the UV coordiantes with triangle shape
                                for (iVertCounter = 0; iVertCounter < 3; iVertCounter++)
                                {
                                    // Draw the texture coordinates
                                    using (Brush tmpBrush = new SolidBrush(btnGreen.BackColor))
                                    {
                                        g.FillEllipse(tmpBrush,
                                                      Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x) - I_RADIUS,
                                                      Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y) - I_RADIUS,
                                                      2 * I_RADIUS, 2 * I_RADIUS);

                                        switch (iVertCounter)
                                        {
                                            case 0:

                                                pointTriPoly[0].X = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x);
                                                pointTriPoly[0].Y = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y);

                                                if (pointTriPoly[0].X >= iWidth) pointTriPoly[0].X--;
                                                if (pointTriPoly[0].Y >= iHeight) pointTriPoly[0].Y--;

                                                pointTriPoly[3].X = pointTriPoly[0].X;
                                                pointTriPoly[3].Y = pointTriPoly[0].Y;

                                                break;

                                            case 1:

                                                pointTriPoly[1].X = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x);
                                                pointTriPoly[1].Y = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y);

                                                if (pointTriPoly[1].X >= iWidth) pointTriPoly[1].X--;
                                                if (pointTriPoly[1].Y >= iHeight) pointTriPoly[1].Y--;

                                                break;

                                            case 2:
                                                using (Pen tmpPen = new Pen(btnGreen.BackColor))
                                                {

                                                    tmpPen.Alignment = PenAlignment.Center;

                                                    pointTriPoly[2].X = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x);
                                                    pointTriPoly[2].Y = Convert.ToInt32(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y);

                                                    if (pointTriPoly[2].X >= iWidth) pointTriPoly[2].X--;
                                                    if (pointTriPoly[2].Y >= iHeight) pointTriPoly[2].Y--;

                                                    g.DrawLines(tmpPen, pointTriPoly);
                                                }

                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            pbTextureView.Image = tmpBMP;
            pbTextureView.Refresh();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            FieldRSDResource tmpfRSDResource;
            PModel tmpPModel;

            // Update texture coordinates from lstUVXYCoords to the TextureViewerModel
            UpdateXYCoords();

            // Commit the previous update to the original model
            switch (modelType)
            {
                case K_HRC_SKELETON:
                    tmpfRSDResource = CopyRSDResource(fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece]);
                    tmpfRSDResource.Model.TexCoords = TexViewModel.TexCoords;
                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece] = CopyRSDResource(tmpfRSDResource);
                    break;

                case K_AA_SKELETON:
                    if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                    {
                        bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex] = CopyPModel(TexViewModel);

                    }
                    else
                    {
                        tmpPModel = CopyPModel(bSkeleton.bones[SelectedBone].Models[SelectedBonePiece]);
                        tmpPModel.TexCoords = TexViewModel.TexCoords;
                        bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = CopyPModel(tmpPModel);
                    }
                    break;

                case K_MAGIC_SKELETON:
                    tmpPModel = CopyPModel(bSkeleton.bones[SelectedBone].Models[SelectedBonePiece]);
                    tmpPModel.TexCoords = TexViewModel.TexCoords;
                    bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = CopyPModel(tmpPModel);
                    break;
            }

            // Update main title window
            bChangesDone = true;
            frmSkelEdit.UpdateMainSkeletonWindowTitle();

            frmSkelEdit.PanelModel_Paint(null, null);
        }

        private void BtnFlipH_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iTexID, iUVCounter, iHeight;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iHeight = pbTextureView.Height;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.TexCoords.Length > 0 && 
                    TexViewModel.Groups[iGroupIdx].texFlag == 1 && 
                    TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY()
                            {
                                x = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x,
                                y = iHeight - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y,
                            };

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void BtnFlipV_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iTexID, iUVCounter, iWidth;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iWidth = pbTextureView.Width;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.TexCoords.Length > 0 && 
                    TexViewModel.Groups[iGroupIdx].texFlag == 1 && 
                    TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the X (U) position for horizontal.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY()
                            {
                                y = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y,
                                x = iWidth - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x,
                            };

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void BtnRotateUV_Click(object sender, EventArgs e)
        {

            int iGroupIdx, iUVCounter, iTexID, iHeight;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iHeight = pbTextureView.Height;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.TexCoords.Length > 0 && 
                    TexViewModel.Groups[iGroupIdx].texFlag == 1 && 
                    TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must rotate clockwise.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY()
                            {
                                y = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x,
                                x = iHeight - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y,
                            };

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void BtnGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGreen.Checked)
            {
                frmSkelEdit.bPaintGreen = true;
                btnGreen.BackColor = Color.Lime;
            }
            else
            {
                frmSkelEdit.bPaintGreen = false;
                btnGreen.BackColor = Color.White;
            }

            if (!bLoadingTextureViewer)
            {
                DrawUVs();
            }
        }

        public void ChangeTexCoordViewSize()
        {
            int iWidth, iHeight, iTexID, iTexSize;
            float fAspectRatio, fWidth, fHeight;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iWidth = I_TEXTURECOORDVIEWMINSIZE;
            iHeight = I_TEXTURECOORDVIEWMINSIZE;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    iWidth = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].width;
                    iHeight = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].height;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    iWidth = bSkeleton.textures[iTexID].width;
                    iHeight = bSkeleton.textures[iTexID].height;
                    break;

                    //case K_AA_SKELETON:
                    //    if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                    //    {
                    //        iWidth = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].width;
                    //        iHeight = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].height;
                    //    }
                    //    else
                    //    {
                    //        iWidth = bSkeleton.textures[iTexID].width;
                    //        iHeight = bSkeleton.textures[iTexID].height;
                    //    }

                    //    break;

                    //case K_MAGIC_SKELETON:
                    //    iWidth = bSkeleton.textures[iTexID].width;
                    //    iHeight = bSkeleton.textures[iTexID].height;
                    //    break;
            }

            fAspectRatio = (float)iWidth / (float)iHeight;
            iTexSize = I_TEXTURECOORDVIEWMINSIZE * frmSkelEdit.iTexCoordViewerScale;

            // Get the size available
            fWidth = iTexSize;
            fHeight = iTexSize;

            if (fWidth / fHeight > fAspectRatio)
            {
                //  The area is too short and wide.
                //  Make it narrower.
                fWidth = fAspectRatio * fHeight;
            }
            else
            {
                //  The area is too tall and thin.
                //  Make it shorter.
                fHeight = fWidth / fAspectRatio;
            }

            pbTextureView.Width = Convert.ToInt32(fWidth);
            pbTextureView.Height = Convert.ToInt32(fHeight);


            // Let's assign a minimum size witdh/height for the window. This will help to the scrolling.
            // Base: Width: 542, Height = 606
            //iWidth = 542;
            //iHeight = 606;
            iWidth = pbTextureView.Width + (int)(I_WINDOWWIDTHBORDER * frmSkelEdit.dDPIScaleFactor);
            if (pbTextureView.Width < I_TEXTURECOORDVIEWMINSIZE) 
            {
                iWidth = I_TEXTURECOORDVIEWMINSIZE + (int)(I_WINDOWWIDTHBORDER * frmSkelEdit.dDPIScaleFactor);

                pbTextureView.Left = (panelTextureViewer.Width - pbTextureView.Width) / 2;
            }
            else
            {
                pbTextureView.Left = 2;
            }

            iHeight = pbTextureView.Height + panelButtons.Height + (int)(I_WINDOWHEIGHTBORDER * frmSkelEdit.dDPIScaleFactor);

            if (iHeight > Screen.PrimaryScreen.WorkingArea.Height) 
            {
                iHeight = Screen.PrimaryScreen.WorkingArea.Height;
                iWidth += 16;       // Add Vertical ScrollBar width to the form window width.
            }

            if (iWidth > Screen.PrimaryScreen.WorkingArea.Width) iWidth = Screen.PrimaryScreen.WorkingArea.Width;


            MinimumSize = new Size(iWidth, iHeight);
            Size = new Size(iWidth, iHeight);

            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - iHeight) / 2;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - iWidth) / 2;
        }

        public void ChangeZoomButtons()
        {
            if (frmSkelEdit.iTexCoordViewerScale == 1) btnZoomOut.Enabled = false;
            else btnZoomOut.Enabled = true;

            if (frmSkelEdit.iTexCoordViewerScale == 3) btnZoomIn.Enabled = false;
            else btnZoomIn.Enabled = true;
        }

        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            frmSkelEdit.iTexCoordViewerScale--;

            // Let's update TextureViewerModel var UVs for make zoom.
            UpdateXYCoords();

            ChangeZoomButtons();
            ChangeTexCoordViewSize();

            // Prepare list of UVs to XY points of the texture for UVpoints movement
            PrepareUVXYCoords();
            DrawUVs();
        }

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            frmSkelEdit.iTexCoordViewerScale++;

            // Let's update TextureViewerModel var UVs for make zoom.
            UpdateXYCoords();

            ChangeZoomButtons();
            ChangeTexCoordViewSize();

            // Prepare list of UVs to XY points of the texture for UVpoints movement
            PrepareUVXYCoords();
            DrawUVs();
        }

        public bool FoundXYPosMouse(int X, int Y)
        {
            localGroupIdx = 0;

            foreach (STUVXYCoord itmXYCoord in lstUVXYCoords)
            {

                if (itmXYCoord.XYCoords != null)
                {

                    localXYPointIdx = 0;

                    foreach (STPoint2DXY itmP2D in itmXYCoord.XYCoords)
                    {
                        if (X >= (Convert.ToInt32(itmP2D.x) - I_RADIUS - 1) && X <= (Convert.ToInt32(itmP2D.x) + I_RADIUS - 1) &&
                            Y >= (Convert.ToInt32(itmP2D.y) - I_RADIUS - 1) && Y <= (Convert.ToInt32(itmP2D.y) + I_RADIUS - 1))
                            return true;

                        localXYPointIdx++;
                    }
                }

                localGroupIdx++;
            }

            return false;
        }


        public void UpdateXYCoords()
        {
            int iGroupIdx, iTexID, iVertCounter, iWidth, iHeight;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iWidth = pbTextureView.Width;
            iHeight = pbTextureView.Height;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.TexCoords.Length > 0 && 
                    TexViewModel.Groups[iGroupIdx].texFlag == 1 && 
                    TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    for (iVertCounter = 0; iVertCounter < TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        TexViewModel.TexCoords[TexViewModel.Groups[iGroupIdx].offsetTex + iVertCounter].x =
                                       lstUVXYCoords[iGroupIdx].XYCoords[iVertCounter].x / iWidth;
                        TexViewModel.TexCoords[TexViewModel.Groups[iGroupIdx].offsetTex + iVertCounter].y =
                                       lstUVXYCoords[iGroupIdx].XYCoords[iVertCounter].y / iHeight;
                    }
                }
            }
        }

        private void PbTextureView_MouseMove(object sender, MouseEventArgs e)
        {
            STPoint2DXY tmpP2D;
            
            if (bMouseLeftClicked)
            {
                if (bFoundXYCoord)
                {
                    if (shiftPressedQ)
                    {
                        int iLocalGroupXYCounter;
                        int iDiffX, iDiffY;

                        iDiffX = e.X - iPosXClicked;
                        iDiffY = e.Y - iPosYClicked;

                        for (iLocalGroupXYCounter = 0; iLocalGroupXYCounter < lstUVXYCoords[localGroupIdx].XYCoords.Count; iLocalGroupXYCounter++)
                        {
                            tmpP2D.x = lstUVXYCoords[localGroupIdx].XYCoords[iLocalGroupXYCounter].x + iDiffX;
                            tmpP2D.y = lstUVXYCoords[localGroupIdx].XYCoords[iLocalGroupXYCounter].y + iDiffY;

                            lstUVXYCoords[localGroupIdx].XYCoords[iLocalGroupXYCounter] = tmpP2D;
                        }

                        iPosXClicked = e.X;
                        iPosYClicked = e.Y;
                    }
                    else
                    {
                        tmpP2D.x = e.X;
                        tmpP2D.y = e.Y;
                        lstUVXYCoords[localGroupIdx].XYCoords[localXYPointIdx] = tmpP2D;
                    }

                    DrawUVs();                    
                }
            }
            else
            {
                if (FoundXYPosMouse(e.X, e.Y))
                {
                    if (shiftPressedQ)
                        pbTextureView.Cursor = HandPlus;
                    else
                        pbTextureView.Cursor = Hand;

                    bFoundXYCoord = true;
                }
                else
                {
                    pbTextureView.Cursor = Cursors.Default;
                    bFoundXYCoord = false;
                }
            }
        }

        private void PbTextureView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bMouseLeftClicked = true;
                iPosXClicked = e.X;
                iPosYClicked = e.Y;
            }
            else
            {
                iPosXClicked = 0;
                iPosYClicked = 0;
            }
        }

        private void PbTextureView_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseLeftClicked = false;
        }

        private void FrmTextureViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey) shiftPressedQ = true;

            if (bFoundXYCoord)
            {
                if (shiftPressedQ)
                    pbTextureView.Cursor = HandPlus;
                else
                    pbTextureView.Cursor = Hand;
            }
        }

        private void FrmTextureViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey) shiftPressedQ = false;

            if (!shiftPressedQ)
            {
                if (bFoundXYCoord)
                    pbTextureView.Cursor = Hand;
                else
                    pbTextureView.Cursor = Cursors.Default;
            }
        }

        private void FrmTextureViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let's clear the List of UV Points.
            foreach (STUVXYCoord itmUVXYCoord in lstUVXYCoords)
            {
                if (itmUVXYCoord.XYCoords != null)
                            itmUVXYCoord.XYCoords.Clear();
            }

            lstUVXYCoords.Clear();
        }

 

    }

}
