using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{
    using static frmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldRSDResource;
    using static FF7PModel;

    using static Utils;
    using static GDI32;

    public partial class frmTextureViewer : Form
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


        private frmSkeletonEditor frmSkelEdit;

        public const int I_MAXNUMGROUPS = 128;
        public const int I_TEXTURECOORDVIEWMINSIZE = 512;
        public const int I_WINDOWWIDTHBORDER = 24;
        public const int I_WINDOWHEIGHTBORDER = 44;
        public const int I_RADIUS = 3;

        public PModel TexViewModel;


        // UV Coordinates movement with mouse structs
        public List<STUVXYCoord> lstUVXYCoords;
        bool bFoundXYCoord;
        bool bMouseLeftClicked;
        bool bLoadingTextureViewer;
        int localXYPointIdx, localGroupIdx;


        public frmTextureViewer(frmSkeletonEditor frmSkelEdit, PModel modelIn)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;

            TexViewModel = CopyPModel(modelIn);
        }


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
            int iGroupIdx, iVertCounter, iTexID, iWidth, iHeight;
            float fU, fV;
            STUVXYCoord tmplstUVXYCoords;
            STPoint2DXY tmpP2D;
            bool bNeedNormalize;

            if (lstUVXYCoords == null) lstUVXYCoords = new List<STUVXYCoord>();
            else lstUVXYCoords.Clear();

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            bNeedNormalize = false;

            iWidth = pbTextureView.Width - 1;
            iHeight = pbTextureView.Height - 1;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {

                // We have one group to add to the list
                tmplstUVXYCoords = new STUVXYCoord();

                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
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

                        tmpP2D.x = fU * iWidth;
                        tmpP2D.y = fV * iHeight;

                        tmplstUVXYCoords.XYCoords.Add(tmpP2D);
                    }
                }

                lstUVXYCoords.Add(tmplstUVXYCoords);
            }
        }

        private void frmTextureViewer_Load(object sender, EventArgs e)
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
            PrepareUVXYCoords();
            DrawUVs();

            bLoadingTextureViewer = false;
        }

        public void DrawUVs()
        {
            IntPtr hTmpBMP = IntPtr.Zero;

            int iGroupIdx, iPolyIdx, iVertCounter, iWidth, iHeight, iTexID;
            float fSecondU, fSecondV, fFirstU, fFirstV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    hTmpBMP = fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[iTexID].HBMP;
                    break;

                case K_AA_SKELETON:
                    if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                        hTmpBMP = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].HBMP;
                    else
                        hTmpBMP = bSkeleton.textures[iTexID].HBMP;

                    break;

                case K_MAGIC_SKELETON:
                    hTmpBMP = bSkeleton.textures[iTexID].HBMP;
                    break;
            }


            fSecondU = 0;
            fSecondV = 0;
            fFirstU = 0;
            fFirstV = 0;

            Bitmap tmpBMP = new Bitmap(pbTextureView.Width, pbTextureView.Height);

            // Get the size available
            iWidth = tmpBMP.Width;
            iHeight = tmpBMP.Height;

            using (Graphics g = Graphics.FromImage(tmpBMP))
            {
                //g.InterpolationMode = InterpolationMode.Default;
                //g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.DrawImage(Image.FromHbitmap(hTmpBMP), 0, 0, iWidth, iHeight);
                     
                for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
                {
                    if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
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
                                                  lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x - I_RADIUS,
                                                  lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y - I_RADIUS, 
                                                  2 * I_RADIUS, 2 * I_RADIUS);

                                    switch (iVertCounter)
                                    {
                                        case 0:
                                            fFirstU = lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x;
                                            fFirstV = lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y;
                                            break;

                                        case 1:
                                            fSecondU = lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x;
                                            fSecondV = lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y;
                                            break;

                                        case 2:
                                            using (Pen tmpPen = new Pen(tmpBrush))
                                            {
                                                g.DrawLine(tmpPen,
                                                    new PointF(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x,
                                                               lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y),
                                                    new PointF(fFirstU, fFirstV));
                                                g.DrawLine(tmpPen, new PointF(fFirstU, fFirstV), new PointF(fSecondU, fSecondV));
                                                g.DrawLine(tmpPen, 
                                                    new PointF(fSecondU, fSecondV),
                                                    new PointF(lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].x,
                                                               lstUVXYCoords[iGroupIdx].XYCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter]].y));
                                            }

                                            break;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCommit_Click(object sender, EventArgs e)
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

            frmSkelEdit.panelModel_Paint(null, null);
        }

        private void btnFlipH_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iTexID, iUVCounter, iHeight;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iHeight = pbTextureView.Height - 1;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY();
                            tmpP2DXY.x = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x;
                            
                            tmpP2DXY.y = iHeight - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y;

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void btnFlipV_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iTexID, iUVCounter, iWidth;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iWidth = pbTextureView.Width - 1;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the X (U) position for horizontal.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY();
                            tmpP2DXY.y = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y;

                            tmpP2DXY.x = iWidth - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x;

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void btnRotateUV_Click(object sender, EventArgs e)
        {

            int iGroupIdx, iUVCounter, iTexID, iHeight;
            STPoint2DXY tmpP2DXY;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;
            iHeight = pbTextureView.Height - 1;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must rotate clockwise.
                    if (lstUVXYCoords[iGroupIdx].XYCoords != null)
                    {
                        for (iUVCounter = 0; iUVCounter < lstUVXYCoords[iGroupIdx].XYCoords.Count; iUVCounter++)
                        {
                            tmpP2DXY = new STPoint2DXY();
                            tmpP2DXY.y = lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].x;

                            tmpP2DXY.x = iHeight - lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter].y;

                            lstUVXYCoords[iGroupIdx].XYCoords[iUVCounter] = tmpP2DXY;
                        }
                    }
                }
            }

            DrawUVs();
        }

        private void btnGreen_CheckedChanged(object sender, EventArgs e)
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
                    if (bSkeleton.wpModels.Count > 0 && SelectedBone == bSkeleton.nBones)
                    {
                        iWidth = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].width;
                        iHeight = bSkeleton.textures[bSkeleton.wpModels[frmSkelEdit.cbWeapon.SelectedIndex].Groups[0].texID].height;
                    }
                    else
                    {
                        iWidth = bSkeleton.textures[iTexID].width;
                        iHeight = bSkeleton.textures[iTexID].height;
                    }

                    break;

                case K_MAGIC_SKELETON:
                    iWidth = bSkeleton.textures[iTexID].width;
                    iHeight = bSkeleton.textures[iTexID].height;
                    break;
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

            pbTextureView.Width = (int)fWidth;
            pbTextureView.Height = (int)fHeight;


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

        private void btnZoomOut_Click(object sender, EventArgs e)
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

        private void btnZoomIn_Click(object sender, EventArgs e)
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

        public bool foundXYPosMouse(int X, int Y)
        {
            localXYPointIdx = 0;
            localGroupIdx = 0;

            foreach (STUVXYCoord itmXYCoord in lstUVXYCoords)
            {

                if (itmXYCoord.XYCoords != null)
                {
                    foreach (STPoint2DXY itmP2D in itmXYCoord.XYCoords)
                    {
                        if (X >= ((int)itmP2D.x - I_RADIUS - 1) && X <= ((int)itmP2D.x + I_RADIUS - 1) &&
                            Y >= ((int)itmP2D.y - I_RADIUS - 1) && Y <= ((int)itmP2D.y + I_RADIUS - 1))
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
            iWidth = pbTextureView.Width - 1;
            iHeight = pbTextureView.Height - 1;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
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

        private void pbTextureView_MouseMove(object sender, MouseEventArgs e)
        {
            STPoint2DXY tmpP2D;
            
            if (bMouseLeftClicked)
            {
                if (bFoundXYCoord)
                {
                    tmpP2D.x = e.X;
                    tmpP2D.y = e.Y;
                    lstUVXYCoords[localGroupIdx].XYCoords[localXYPointIdx] = tmpP2D;

                    DrawUVs();                    
                }
            }
            else
            {
                if (foundXYPosMouse(e.X, e.Y))
                {
                    pbTextureView.Cursor = Cursors.Hand;
                    bFoundXYCoord = true;
                }
                else
                {
                    pbTextureView.Cursor = Cursors.Default;
                    bFoundXYCoord = false;
                }
            }
        }

        private void pbTextureView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) bMouseLeftClicked = true;             
        }

        private void pbTextureView_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseLeftClicked = false;
        }

        private void frmTextureViewer_FormClosed(object sender, FormClosedEventArgs e)
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
