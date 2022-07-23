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

        private frmSkeletonEditor frmSkelEdit;

        public const int I_TEXTURECOORDVIEWMINSIZE = 512;
        public const int I_WINDOWWIDTHBORDER = 24;
        public const int I_WINDOWHEIGHTBORDER = 44;

        public PModel TexViewModel;

        public frmTextureViewer(frmSkeletonEditor frmSkelEdit, PModel modelIn)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;

            //tmpfRSDResource = CopyRSDResource(fRSDResourceIn);
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

        private void frmTextureViewer_Load(object sender, EventArgs e)
        {
            Text = "Texture Coordinates(UVs) Viewer" + " - Model: " + TexViewModel.fileName +
                                                       " - Tex: " + frmSkelEdit.cbTextureSelect.Items[frmSkelEdit.cbTextureSelect.SelectedIndex];

            // Initialize things
            if (frmSkelEdit.bPaintGreen) btnGreen.Checked = true;

            // zoom
            ChangeZoomButtons();

            // Assign tooltips.
            DefineToolTips();

            ChangeZoomButtons();
            ChangeTexCoordViewSize();
        }

        public void DrawUVs()
        {
            IntPtr hTmpBMP = IntPtr.Zero;

            int iGroupIdx, iPolyIdx, iVertCounter, iWidth, iHeight, iTexID;
            int iU, iV, iSecondU, iSecondV, iFirstU, iFirstV, iRadius;
            decimal fU, fV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            switch(modelType)
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


            iSecondU = 0;
            iSecondV = 0;
            iFirstU = 0;
            iFirstV = 0;
            iRadius = 3;

            //Bitmap tmpBMP = new Bitmap(I_TEXTURECOORDVIEWMINSIZE * frmSkelEdit.iTexCoordViewerScale, 
            //                           I_TEXTURECOORDVIEWMINSIZE * frmSkelEdit.iTexCoordViewerScale);

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
                                fU = (decimal)TexViewModel.TexCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter] +
                                                                     TexViewModel.Groups[iGroupIdx].offsetTex].x;

                                if (fU == 1.000000M) fU = 0.0M;
                                if (fU < 0.0M) 
                                {
                                    if ((fU % 1.0M) == 0) fU = 1.0M;
                                    else fU = -fU;
                                }                                
                                if (fU > 1.000000M) fU -= Math.Floor(fU);                                

                                iU = (int)(fU * iWidth);
                                if (iU == iWidth) iU -= 1;


                                fV = (decimal)TexViewModel.TexCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter] +
                                                                     TexViewModel.Groups[iGroupIdx].offsetTex].y;
                                if (fV == 1.000000M) fV = 0.0M;
                                if (fV < 0.0M)
                                {
                                    if ((fV % 1.0M) == 0) fV = 1.0M;
                                    else fV = -fV;
                                }
                                if (fV > 1.000000M) fV -= Math.Floor(fV);

                                iV = (int)(fV * iHeight);
                                if (iV == iHeight) iV -= 1;

                                // Draw the texture coordinates
                                using (Brush tmpBrush = new SolidBrush(btnGreen.BackColor))
                                {
                                    g.FillEllipse(tmpBrush, iU - iRadius, iV - iRadius, 2 * iRadius, 2 * iRadius);

                                    switch (iVertCounter)
                                    {
                                        case 0:
                                            iFirstU = iU;
                                            iFirstV = iV;
                                            break;

                                        case 1:
                                            iSecondU = iU;
                                            iSecondV = iV;
                                            break;

                                        case 2:
                                            using (Pen tmpPen = new Pen(tmpBrush))
                                            {
                                                g.DrawLine(tmpPen, new Point(iU, iV), new Point(iFirstU, iFirstV));
                                                g.DrawLine(tmpPen, new Point(iFirstU, iFirstV), new Point(iSecondU, iSecondV));
                                                g.DrawLine(tmpPen, new Point(iSecondU, iSecondV), new Point(iU, iV));
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

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            FieldRSDResource tmpfRSDResource;
            PModel tmpPModel;

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
            int iGroupIdx, iVertCounter, iTexID;
            decimal fV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fV = (decimal)TexViewModel.TexCoords[iVertCounter].y;
                        if (fV == 1.000000M) fV = 0.0M;
                        if (fV < 0.0M)
                        {
                            if ((fV % 1.0M) == 0) fV = 1.0M;
                            else fV = -fV;
                        }
                        if (fV > 1.000000M) fV -= Math.Floor(fV);
                        fV = 1.0M - fV;
                        if (fV == 1.000000M) fV = 0.9999999M;

                        TexViewModel.TexCoords[iVertCounter].y = (float)fV;
                    }
                }
            }

            DrawUVs();
        }

        private void btnFlipV_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iVertCounter, iTexID;
            decimal fU;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fU = (decimal)TexViewModel.TexCoords[iVertCounter].x;
                        if (fU == 1.000000M) fU = 0.0M;
                        if (fU < 0.0M)
                        {
                            if ((fU % 1.0M) == 0) fU = 1.0M;
                            else fU = -fU;
                        }
                        if (fU > 1.000000M) fU -= Math.Floor(fU);
                        fU = 1.0M - fU;
                        if (fU == 1.000000M) fU = 0.9999999M;

                        TexViewModel.TexCoords[iVertCounter].x = (float)fU;
                    }
                }
            }

            DrawUVs();
        }

        private void btnRotateUV_Click(object sender, EventArgs e)
        {

            int iGroupIdx, iVertCounter, iTexID;
            decimal fU, fV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fU = (decimal)TexViewModel.TexCoords[iVertCounter].x;

                        if (fU == 1.000000M) fU = 0.0M;
                        if (fU < 0.0M)
                        {
                            if ((fU % 1.0M) == 0) fU = 1.0M;
                            else fU = -fU;
                        }
                        if (fU > 1.000000M) fU -= Math.Floor(fU);
                        if (fU == 1.000000M) fU = 0.9999999M;


                        fV = (decimal)TexViewModel.TexCoords[iVertCounter].y;
                        if (fV == 1.000000M) fV = 0.0M;
                        if (fV < 0.0M)
                        {
                            if ((fV % 1.0M) == 0) fV = 1.0M;
                            else fV = -fV;
                        }
                        if (fV > 1.000000M) fV -= Math.Floor(fV);
                        if (fV == 1.000000M) fV = 0.9999999M;

                        fV = 1.0M - fV;


                        TexViewModel.TexCoords[iVertCounter].x = (float)fV;
                        TexViewModel.TexCoords[iVertCounter].y = (float)fU;

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

            DrawUVs();
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

            DrawUVs();
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

            ChangeZoomButtons();
            ChangeTexCoordViewSize();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            frmSkelEdit.iTexCoordViewerScale++;

            ChangeZoomButtons();
            ChangeTexCoordViewSize();
        }
    }

}
